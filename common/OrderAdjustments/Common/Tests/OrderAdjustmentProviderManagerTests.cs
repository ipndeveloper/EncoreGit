using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.IoC;
using NetSteps.OrderAdjustments.Common;
using NetSteps.OrderAdjustments.Common.Component;

namespace NetSteps.OrderAdjustments.Common.Test
{
    [TestClass]
    public class OrderAdjustmentProviderManagerTests
    {
        [TestMethod]
        public void OrderAdjustmentProviderManager_should_be_registered()
        {
            Encore.Core.Wireup.WireupCoordinator.SelfConfigure();
            // test the IOC registration to verify that we've got it correct
            var adjustmentProviderManager = Create.New<IOrderAdjustmentProviderManager>();
            Assert.IsNotNull(adjustmentProviderManager);
            Assert.IsInstanceOfType(adjustmentProviderManager, typeof(IOrderAdjustmentProviderManager));
        }

        [TestMethod]
        public void OrderAdjustmentProviderManager_should_register_an_IOrderAdjustmentProviderService()
        {
            Mocks.MockOrderAdjustmentProvider provider = new Mocks.MockOrderAdjustmentProvider();
            OrderAdjustmentProviderManager adjustmentProviderManager = new OrderAdjustmentProviderManager();
            adjustmentProviderManager.RegisterAdjustmentProvider(provider);
            Assert.AreEqual(1, adjustmentProviderManager.GetAllProviders().Count());
        }

        [TestMethod]
        public void OrderAdjustmentProviderManager_should_unregister_an_IOrderAdjustmentProviderService()
        {
            Mocks.MockOrderAdjustmentProvider provider = new Mocks.MockOrderAdjustmentProvider();
            OrderAdjustmentProviderManager adjustmentProviderManager = new OrderAdjustmentProviderManager();
            adjustmentProviderManager.RegisterAdjustmentProvider(provider);
						Assert.AreEqual(1, adjustmentProviderManager.GetAllProviders().Count());

            adjustmentProviderManager.UnregisterAdjustmentProvider(provider);
						Assert.AreEqual(0, adjustmentProviderManager.GetAllProviders().Count());
        }
    }
}
