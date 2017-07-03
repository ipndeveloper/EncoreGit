using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using HurricaneServer.Plugins.Inbound;
using MailBee.Mime;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Hurricane.Plugin.Tests.Fakes;
using MailMessage = MailBee.Mime.MailMessage;

namespace NetSteps.Hurricane.Plugin.Tests
{
    [TestClass]
    public class InboundTests
    {
        [TestMethod]
        public void Constructor_WithoutParameters_ExecutesSuccessfully()
        {
            var plugin = new Plugin();
        }

        [TestMethod]
        public void OnConnect_ReturnsSuccess()
        {
            var plugin = new Plugin();
            int sessionId = 12345;
            string ip = "127.0.0.1";
            var options = new Dictionary<string, object>();
            string response = string.Empty;

            var result = plugin.OnConnect(sessionId, ip, ref options, ref response);

            Assert.AreEqual(InboundResponseAction.Success, result);
        }

        [TestMethod]
        public void OnConnectionClosed_Executes()
        {
            var plugin = new Plugin();
            int sessionId = 12345;
            var options = new Dictionary<string, object>();

            plugin.OnConnectionClosed(sessionId, ref options);
        }

        [TestMethod]
        public void OnRcptTo_LocalIP_LocalDomain_ReturnsSuccess()
        {
            var plugin = new Plugin();
            int sessionId = 12345;
            string ip = "127.0.0.1";
            var options = new Dictionary<string, object>();
            string response = string.Empty;
            string recipient = "test@netstepsdemo.com";

            plugin.OnConnect(sessionId, ip, ref options, ref response);
            var result = plugin.OnRcptTo(sessionId, recipient, ref options, ref response);
            plugin.OnConnectionClosed(sessionId, ref options);

            Assert.AreEqual(InboundResponseAction.Success, result);
        }

        [TestMethod]
        public void OnRcptTo_RemoteIP_LocalDomain_ReturnsSuccess()
        {
            var plugin = new Plugin();
            int sessionId = 12345;
            string ip = "4.2.2.1";
            var options = new Dictionary<string, object>();
            string response = string.Empty;
            string recipient = "test@netstepsmail.com";

            plugin.OnConnect(sessionId, ip, ref options, ref response);
            var result = plugin.OnRcptTo(sessionId, recipient, ref options, ref response);
            plugin.OnConnectionClosed(sessionId, ref options);

            Assert.AreEqual(InboundResponseAction.Success, result);
        }

        [TestMethod]
        public void OnRcptTo_LocalIP_RemoteDomain_ReturnsSuccess()
        {
            var plugin = new Plugin();
            int sessionId = 12345;
            string ip = "127.0.0.1";
            var options = new Dictionary<string, object>();
            string response = string.Empty;
            string recipient = "test@test.com";

            plugin.OnConnect(sessionId, ip, ref options, ref response);
            var result = plugin.OnRcptTo(sessionId, recipient, ref options, ref response);
            plugin.OnConnectionClosed(sessionId, ref options);

            Assert.AreEqual(InboundResponseAction.Success, result);
        }

        [TestMethod]
        public void OnRcptTo_RemoteIP_RemoteDomain_ReturnsFailure()
        {
            var plugin = new Plugin();
            int sessionId = 12345;
            string ip = "4.2.2.1";
            var options = new Dictionary<string, object>();
            string response = string.Empty;
            string recipient = "test@test.com";

            plugin.OnConnect(sessionId, ip, ref options, ref response);
            var result = plugin.OnRcptTo(sessionId, recipient, ref options, ref response);
            plugin.OnConnectionClosed(sessionId, ref options);

            Assert.AreEqual(InboundResponseAction.Failure, result);
        }

        [TestMethod]
        public void OnMessageRecieved_LocalDomain_ReturnsIgnore()
        {
            var plugin = new Plugin();
            int sessionId = 12345;
            var envelope = new FakeInboundMessageEnvelope() as IInboundMessageEnvelope;
            envelope.Recipients = "<info@NetstepsMail.com>";
            var messageBytes = ASCIIEncoding.Default.GetBytes(_emailAsciiHeadersAsciiBody);
            envelope.SetMessage(new MemoryStream(messageBytes), messageBytes.Length);
            var options = new Dictionary<string, object>();
            options["AccountId"] = "1003";

            var result = plugin.OnMessageRecieved(sessionId, ref envelope, ref options);
            
            Assert.AreEqual(MessageResponseAction.Ignore, result);
        }

        [TestMethod]
        public void OnMessageRecieved_RemoteDomain_ReturnsAccept()
        {
            var plugin = new Plugin();
            int sessionId = 12345;
            var envelope = new FakeInboundMessageEnvelope() as IInboundMessageEnvelope;
            envelope.Recipients = "<testrecipient@test.com>";
            var messageBytes = ASCIIEncoding.Default.GetBytes(_emailAsciiHeadersAsciiBody);
            envelope.SetMessage(new MemoryStream(messageBytes), messageBytes.Length);
            var options = new Dictionary<string, object>();
            options["AccountId"] = "1003";

            var result = plugin.OnMessageRecieved(sessionId, ref envelope, ref options);

            Assert.AreEqual(MessageResponseAction.Accept, result);
        }

        [TestMethod]
        public void OnMessageRecieved_UTFHeadersRemainIntact()
        {
            var plugin = new Plugin();
            int sessionId = 12345;
            var envelope = new FakeInboundMessageEnvelope() as IInboundMessageEnvelope;
            envelope.Recipients = "<testrecipient@test.com>";
            string messageString = _emailUTFHeadersAsciiBody;
            var messageBytes = ASCIIEncoding.Default.GetBytes(messageString);
            envelope.SetMessage(new MemoryStream(messageBytes), messageBytes.Length);
            var options = new Dictionary<string, object>();
            options["AccountId"] = "1001"; // Natura will update the message headers (overrideSender)

            plugin.OnMessageRecieved(sessionId, ref envelope, ref options);

            var resultMessageStream = envelope.GetMessage();
            var resultMessageBytes = new byte[resultMessageStream.Length];
            resultMessageStream.Read(resultMessageBytes, 0, resultMessageBytes.Length);
            string resultString = ASCIIEncoding.Default.GetString(resultMessageBytes);

            Assert.AreEqual(messageString, resultString);
        }

        private const string _emailAsciiHeadersAsciiBody =
@"MIME-Version: 1.0
From: ""Test Sender"" <testsender@test.com>
To: ""Test Recipient"" <testrecipient@test.com>
Date: Sat, 21 May 2011 02:10:00 -0600
Subject: Test Message
Sender: noreply@naturastaging.fr
Content-Type: text/html;
	charset=""us-ascii""
Content-Transfer-Encoding: quoted-printable

<p style=3D""text-align: left"">Test Message</p>
<p style=3D""text-align: center""><a href=3D""http://www.google.com"">Google</a=
></p>";

        private const string _emailUTFHeadersAsciiBody =
@"MIME-Version: 1.0
From: ""Test Sender"" <testsender@test.com>
To: ""Test Recipient"" <testrecipient@test.com>
Date: Sat, 21 May 2011 02:10:00 -0600
Subject: =?utf-8?Q?F=C3=A9licitation?=
Sender: noreply@naturastaging.fr
Content-Type: text/html;
	charset=""us-ascii""
Content-Transfer-Encoding: quoted-printable

<p style=3D""text-align: left"">Test Message</p>
<p style=3D""text-align: center""><a href=3D""http://www.google.com"">Google</a=
></p>";
    }
}
