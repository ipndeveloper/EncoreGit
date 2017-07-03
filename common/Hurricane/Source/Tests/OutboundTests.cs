using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HurricaneServer.Plugins.Bounce;
using HurricaneServer.Plugins.OpenClickTracking;
using HurricaneServer.Plugins.Outbound;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace NetSteps.Hurricane.Plugin.Tests
{
    [TestClass]
    public class OutboundTests
    {
        [TestMethod]
        public void OnDefer_ReturnsDefer()
        {
            var plugin = new Plugin();
            var mockMessageEnvelope = new Mock<IOutboundMessageEnvelope>();
            var reason = OnFailureReasons.RecipientMailServerResponse;
            var options = new Dictionary<string, object>();

            var result = plugin.OnDefer(mockMessageEnvelope.Object, reason, ref options);

            Assert.AreEqual(DeferAction.Defer, result);
        }

        [TestMethod]
        public void OnFail_Executes()
        {
            var plugin = new Plugin();
            var mockMessageEnvelope = new Mock<IOutboundMessageEnvelope>();
            var reason = OnFailureReasons.RecipientMailServerResponse;
            var type = OnFailureType.Permanent;
            var options = new Dictionary<string, object>();

            plugin.OnFail(mockMessageEnvelope.Object, reason, type, ref options);
        }

        [TestMethod]
        public void OnSent_Executes()
        {
            var plugin = new Plugin();
            var mockMessageEnvelope = new Mock<IOutboundMessageEnvelope>();
            var options = new Dictionary<string, object>();

            plugin.OnSent(mockMessageEnvelope.Object, ref options);
        }

        [TestMethod]
        public void OnBounce_Executes()
        {
            var plugin = new Plugin();
            var mockMessageEnvelope = new Mock<IBounceMessageEnvelope>();
            var options = new Dictionary<string, object>();

            plugin.OnBounce(mockMessageEnvelope.Object, ref options);
        }

        [TestMethod]
        public void OnOpenClickTrackingHit_Executes()
        {
            var plugin = new Plugin();
            var hitType = OpenClickTrackingHitType.Open;
            var mockHitData = new Mock<IOpenClickTrackingHitData>();
            var options = new Dictionary<string, object>();

            plugin.OnOpenClickTrackingHit(hitType, mockHitData.Object, ref options);
        }
    }
}
