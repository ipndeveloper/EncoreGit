using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using HurricaneServer.Plugins.Bounce;
using HurricaneServer.Plugins.Inbound;
using HurricaneServer.Plugins.OpenClickTracking;
using HurricaneServer.Plugins.Outbound;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NetSteps.Hurricane.Plugin.Tests.Fakes;

namespace NetSteps.Hurricane.Plugin.Tests
{
    [TestClass]
    public class EncoreIntegrationTests
    {
        private const int _encoreHurricaneAccountID = 1004;

        [TestMethod]
        [TestCategory("Integration")]
        public void Framework_OnOpenClickTrackingHit_Open()
        {
            var plugin = new Plugin();
            var hitType = OpenClickTrackingHitType.Open;
            var hitData = new Mock<IOpenClickTrackingHitData>();
            hitData.Setup(x => x.AccountId).Returns(_encoreHurricaneAccountID);
            hitData.Setup(x => x.CustomMessageId).Returns("364");
            var options = new Dictionary<string, object>();
            options["EmailAddress"] = "Herbert@example.com";

            plugin.OnOpenClickTrackingHit(hitType, hitData.Object, ref options);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Framework_OnOpenClickTrackingHit_Click()
        {
            var plugin = new Plugin();
            var hitType = OpenClickTrackingHitType.Click;
            var hitData = new Mock<IOpenClickTrackingHitData>();
            hitData.Setup(x => x.AccountId).Returns(_encoreHurricaneAccountID);
            hitData.Setup(x => x.CustomMessageId).Returns("364");
            hitData.Setup(x => x.URL).Returns("http://www.google.com");
            var options = new Dictionary<string, object>();
            options["EmailAddress"] = "Herbert@example.com";

            plugin.OnOpenClickTrackingHit(hitType, hitData.Object, ref options);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Framework_OnBounce()
        {
            var plugin = new Plugin();
            var message = new Mock<IBounceMessageEnvelope>();
            message.Setup(x => x.AccountId).Returns(_encoreHurricaneAccountID);
            message.Setup(x => x.CustomMessageId).Returns("364");
            message.Setup(x => x.BounceAddress).Returns("Herbert@example.com");
            var options = new Dictionary<string, object>();

            plugin.OnBounce(message.Object, ref options);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Framework_OnSent()
        {
            var plugin = new Plugin();
            var message = new Mock<IOutboundMessageEnvelope>();
            message.Setup(x => x.AccountId).Returns(_encoreHurricaneAccountID);
            message.Setup(x => x.CustomMessageId).Returns("364");
            message.Setup(x => x.Recipient).Returns("Herbert@example.com");
            var options = new Dictionary<string, object>();

            plugin.OnSent(message.Object, ref options);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Framework_OnFail()
        {
            var plugin = new Plugin();
            var message = new Mock<IOutboundMessageEnvelope>();
            message.Setup(x => x.AccountId).Returns(_encoreHurricaneAccountID);
            message.Setup(x => x.CustomMessageId).Returns("364");
            message.Setup(x => x.Recipient).Returns("Herbert@example.com");
            var options = new Dictionary<string, object>();

            plugin.OnFail(message.Object, OnFailureReasons.RecipientMailServerConnection, OnFailureType.Permanent, ref options);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Framework_OnMessageReceived_InboundSavesToDBAndReturnsIgnore()
        {
            var plugin = new Plugin();
            int sessionId = 12345;
            var envelope = new FakeInboundMessageEnvelope() as IInboundMessageEnvelope;
            envelope.Recipients = "<info@NetstepsMail.com>";
            string messageString = "Test";
            var messageBytes = ASCIIEncoding.Default.GetBytes(messageString);
            envelope.SetMessage(new MemoryStream(messageBytes), messageBytes.Length);
            var options = new Dictionary<string, object>();
            options["AccountId"] = _encoreHurricaneAccountID.ToString();

            var response = plugin.OnMessageRecieved(sessionId, ref envelope, ref options);

            Assert.AreEqual(MessageResponseAction.Ignore, response);
        }
    }
}
