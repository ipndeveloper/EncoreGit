using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Communication.Common;
using NetSteps.Communication.Services.Models;

namespace NetSteps.Communication.Services.Tests.Models
{
    [TestClass]
    public class PromotionAccountAlertTests
    {
        [TestMethod]
        public void Constructor_SetsProviderKey()
        {
            var target = new PromotionAccountAlert();
            Assert.AreEqual(CommunicationConstants.AccountAlertProviderKey.Promotion, target.ProviderKey);
        }
    }
}
