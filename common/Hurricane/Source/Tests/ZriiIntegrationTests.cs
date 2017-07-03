using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using HurricaneServer.Plugins.Inbound;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Hurricane.Plugin.Tests.Fakes;

namespace NetSteps.Hurricane.Plugin.Tests
{
    [TestClass]
    public class ZriiIntegrationTests
    {
        // Zrii is gone
        // TODO: Maybe write a generic test for the Autoforward feature?
        [Ignore]
        [TestMethod]
        [TestCategory("Integration")]
        public void Zrii_OnMessageRecieved_AutoForwardUpdatesEnvelope()
        {
            var plugin = new Plugin();
            int sessionId = 12345;
            var envelope = new FakeInboundMessageEnvelope() as IInboundMessageEnvelope;
            envelope.Recipients = "<Wiggles111@zriioffice.com>";
            string messageString = _zriiEmail;
            var messageBytes = ASCIIEncoding.Default.GetBytes(messageString);
            envelope.SetMessage(new MemoryStream(messageBytes), messageBytes.Length);
            var options = new Dictionary<string, object>();
            options["AccountId"] = "1000";

            plugin.OnMessageRecieved(sessionId, ref envelope, ref options);

            Assert.AreEqual("<lcoon@zrii.com>", envelope.Recipients);
        }

        // Zrii is gone
        // TODO: Maybe write a generic test for the Disclaimer feature?
        [Ignore]
        [TestMethod]
        [TestCategory("Integration")]
        public void Zrii_OnMessageRecieved_AppendsDisclaimer()
        {
            var plugin = new Plugin();
            int sessionId = 12345;
            var envelope = new FakeInboundMessageEnvelope() as IInboundMessageEnvelope;
            envelope.Recipients = "<Wiggles111@zriioffice.com>";
            string messageString = _zriiEmail;
            var messageBytes = ASCIIEncoding.Default.GetBytes(messageString);
            envelope.SetMessage(new MemoryStream(messageBytes), messageBytes.Length);
            var options = new Dictionary<string, object>();
            options["AccountId"] = "1000";

            plugin.OnMessageRecieved(sessionId, ref envelope, ref options);

            var resultMessageStream = envelope.GetMessage();
            var resultMessageBytes = new byte[resultMessageStream.Length];
            resultMessageStream.Read(resultMessageBytes, 0, resultMessageBytes.Length);
            string resultString = ASCIIEncoding.Default.GetString(resultMessageBytes);
            
            Assert.AreEqual(_zriiEmailWithDisclaimer, resultString);
        }

        private const string _zriiEmail =
@"MIME-Version: 1.0
From: ""Test Sender"" <testsender@test.com>
To: ""Test Recipient"" <Wiggles111@myzriitesting.com>
Date: Sat, 21 May 2011 02:10:00 -0600
Subject: Test Message
Content-Type: text/html;
	charset=""us-ascii""
Content-Transfer-Encoding: quoted-printable

<p style=3D""text-align: left"">Test Message</p>
<p style=3D""text-align: center""><a href=3D""http://www.google.com"">Google</a=
></p>";

        private const string _zriiEmailWithDisclaimer =
@"MIME-Version: 1.0
From: ""Test Sender"" <testsender@test.com>
To: ""Test Recipient"" <Wiggles111@myzriitesting.com>
Date: Sat, 21 May 2011 02:10:00 -0600
Subject: Test Message
Content-Type: text/html;
	charset=""us-ascii""
Content-Transfer-Encoding: quoted-printable

<p style=3D""text-align: left"">Test Message</p>
<p style=3D""text-align: center""><a href=3D""http://www.google.com"">Google</a=
></p><br /><br /><br><br><br><div class=3D""content""><div class=3D""body""><p>=
</p><p>This email has been automatically forwarded from your Zriimail accou=
nt (zriioffice.com).  You can control which email addresses are used to for=
ward email in your back office.  If you no longer wish to receive Zrii-rela=
ted emails at this address, please log into your back office and remove any=
 forwarding options from there.  It is important to periodically check your=
 back office Zriimail as we cannot guarantee the delivery of forwarded emai=
ls to outside accounts such as Hotmail, Gmail, Yahoo, Comcast etc. due to t=
he spam restrictions of your mail provider.</p><!--endbody--></div><p>MailP=
rocessor7</p></div>";
    }
}
