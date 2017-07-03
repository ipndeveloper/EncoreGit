using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.IoC;
using NetSteps.Extensibility.Core;
using NetSteps.Promotions.Plugins.Qualifications;
using NetSteps.Promotions.Plugins.Common.Qualifications;
using NetSteps.Promotions.Plugins.Tests.QualificationTests;
using NetSteps.Promotions.Common.Model;
using NetSteps.Encore.Core.Wireup;

namespace NetSteps.Promotions.Plugins.Test.QualificationTests
{
    [TestClass]
    public class OrderTypeQualificationHandlerTest : BaseQualificationHandlerTestProxy<OrderTypeQualificationHandler>
    {
        private void mockWireup()
        {
        }

        [TestMethod]
        public void OrderTypeQualificationHandler_should_be_registered()
        {
            using (var container = Create.NewContainer())
            {
                mockWireup();
                // test the IOC registration to verify that we've got it correct
                var registry = Create.New<IDataObjectExtensionProviderRegistry>();
                var handler = registry.RetrieveExtensionProvider(QualificationExtensionProviderKeys.OrderTypeProviderKey);
                Assert.IsNotNull(handler);
                Assert.IsInstanceOfType(handler, typeof(OrderTypeQualificationHandler));
            }
        }

        [TestMethod]
        public void OrderTypeQualificationHandler_should_match_iforderContext_type_is_contained_inorderContext_type_list()
        {
            using (var container = Create.NewContainer())
            {
                var orderContext = GetMockOrderContext(1);

                OrderTypeQualificationHandler handler = new OrderTypeQualificationHandler();
                IOrderTypeQualificationExtension extension = Create.New<IOrderTypeQualificationExtension>();
                extension.OrderTypes.Add(orderContext.Order.OrderTypeID);

                Assert.IsTrue(handler.Matches(Create.New<IPromotion>(), extension, orderContext));
            }
        }

        [TestMethod]
        public void OrderTypeQualificationHandler_should_not_match_iforderContext_type_is_not_contained_inorderContext_type_list()
        {
            using (var container = Create.NewContainer())
            {
                mockWireup(); 
                
                var orderContext = GetMockOrderContext(1);

                OrderTypeQualificationHandler handler = new OrderTypeQualificationHandler();
                IOrderTypeQualificationExtension extension = Create.New<IOrderTypeQualificationExtension>();
                int randomOrderTypeID = 0;
                while (randomOrderTypeID == 0 && randomOrderTypeID == orderContext.Order.OrderTypeID)
                {
                    randomOrderTypeID = new Random().Next();
                }
                extension.OrderTypes.Add(randomOrderTypeID);

                Assert.IsTrue(!handler.Matches(Create.New<IPromotion>(), extension, orderContext));
            }
        }
    }
}
