using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Extensibility.Core;
using NetSteps.OrderAdjustments.Common.Model;
using NetSteps.OrderAdjustments.Service.Test.Mocks;

namespace NetSteps.OrderAdjustments.Service.Test
{
    // just to verify that the mock is functioning as intended.

    [TestClass]
    public class MockAdjustmentProviderTests
    {
        int _accountID;

        [TestInitialize]
        public void Init()
        {
            WireupCoordinator.SelfConfigure();
            _accountID = new Random().Next();
            var registry = Create.New<IDataObjectExtensionProviderRegistry>();
            registry.RegisterExtensionProvider<MockOrderAdjustmentProvider>(MockOrderAdjustmentProvider.ProviderKey);

        }

        [TestMethod]
        public void MockAdjustmentProvider_should_pass_sanity_check_and_return_applicable_adjustments()
        {
            MockOrderAdjustmentProvider provider = new MockOrderAdjustmentProvider();
            MockOrderContext orderContext = new MockOrderContext(_accountID);
            int productID = new Random().Next();
            int accountID = orderContext.Order.OrderCustomers[0].AccountID;

            for (int i = 0; i < 12; i++)
            {
                MockAdjustmentTypes nextType = (MockAdjustmentTypes)(1 << i);
                provider.AddAdjustment(MockOrderAdjustmentCreator.Generate(nextType, productID, accountID));
                IEnumerable<IOrderAdjustmentProfile> adjustments = provider.GetApplicableAdjustments(orderContext);
                Assert.IsNotNull(adjustments);
                Assert.AreEqual(i + 1, adjustments.Count());
            }
        }
    }
}
