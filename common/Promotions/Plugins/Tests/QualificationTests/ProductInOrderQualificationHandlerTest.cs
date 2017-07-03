using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.IoC;
using NetSteps.Extensibility.Core;
using NetSteps.Promotions.Common.Model;
using NetSteps.Promotions.Plugins.Common.Qualifications;
using NetSteps.Promotions.Plugins.Qualifications;
using NetSteps.Promotions.Plugins.Tests.QualificationTests;
using NetSteps.Encore.Core.Wireup;

namespace NetSteps.Promotions.Plugins.Test.QualificationTests
{
    [TestClass]
    public class ProductInOrderQualificationHandlerTest : BaseQualificationHandlerTestProxy<ProductInOrderQualificationHandler>
    {
        private void mockWireup()
        {
        }

        [TestMethod]
        public void ProductInOrderQualificationHandler_should_be_registered()
        {
            using (var container = Create.NewContainer())
            {
                mockWireup();
                // test the IOC registration to verify that we've got it correct
                var registry = Create.New<IDataObjectExtensionProviderRegistry>();
                var handler = registry.RetrieveExtensionProvider(QualificationExtensionProviderKeys.ProductInOrderProviderKey);
                Assert.IsNotNull(handler);
                Assert.IsInstanceOfType(handler, typeof(ProductInOrderQualificationHandler));
            }
        }

        [TestMethod]
        public void ProductInOrderQualification_handler_should_match_if_orderContext_contains_matching_productid_and_matching_quantity()
        {
            using (var container = Create.NewContainer())
            {
                mockWireup();
                var orderContext = GetMockOrderContext(1);

                ProductInOrderQualificationHandler handler = new ProductInOrderQualificationHandler();
                IProductInOrderQualificationExtension extension = Create.New<IProductInOrderQualificationExtension>();
                extension.ProductID = new Random().Next();
                extension.Quantity = 1;

                orderContext.Order.AddItem(new Random().Next(), 1);
                Assert.IsTrue(handler.Matches(Create.New<IPromotion>(), extension, orderContext));
            }
        }

        [TestMethod]
        public void ProductInOrderQualification_handler_should_match_if_orderContext_contains_matching_productid_and_greater_quantity()
        {
            using (var container = Create.NewContainer())
            {
                mockWireup(); var orderContext = GetMockOrderContext(1);

                ProductInOrderQualificationHandler handler = new ProductInOrderQualificationHandler();
                IProductInOrderQualificationExtension extension = Create.New<IProductInOrderQualificationExtension>();
                extension.ProductID = new Random().Next();
                extension.Quantity = 1;

                orderContext.Order.AddItem(new Random().Next(), 2);
                Assert.IsTrue(handler.Matches(Create.New<IPromotion>(), extension, orderContext));
            }
        }

        [TestMethod]
        public void ProductInOrderQualification_handler_should_not_match_if_orderContext_contains_matching_productid_and_lesser_quantity()
        {
            using (var container = Create.NewContainer())
            {
                var orderContext = GetMockOrderContext(1);

                ProductInOrderQualificationHandler handler = new ProductInOrderQualificationHandler();
                IProductInOrderQualificationExtension extension = Create.New<IProductInOrderQualificationExtension>();
                extension.ProductID = new Random().Next();
                extension.Quantity = 2;

                orderContext.Order.AddItem(new Random().Next(), 1);
                Assert.IsTrue(!handler.Matches(Create.New<IPromotion>(), extension, orderContext));
            }
        }

        [TestMethod]
        public void ProductInOrderQualification_handler_should_not_match_if_order_context_contains_nonmatching_productid_and_matching_quantity()
        {
            using (var container = Create.NewContainer())
            {
                mockWireup(); 
                
                var orderContext = GetMockOrderContext(1);

                ProductInOrderQualificationHandler handler = new ProductInOrderQualificationHandler();
                IProductInOrderQualificationExtension extension = Create.New<IProductInOrderQualificationExtension>();
                extension.ProductID = new Random().Next();
                extension.Quantity = 1;

                int altProduct = 0;
                while (altProduct == 0 || altProduct == extension.ProductID)
                {
                    altProduct = new Random().Next();
                }
                orderContext.Order.AddItem(altProduct, 1);
                Assert.IsTrue(!handler.Matches(Create.New<IPromotion>(), extension, orderContext));
            }
        }

        [TestMethod]
        public void ProductInOrderQualification_handler_should_not_match_if_orderContext_contains_nonmatching_productid_and_lesser_quantity()
        {
            using (var container = Create.NewContainer())
            {
                var orderContext = GetMockOrderContext(1);

                ProductInOrderQualificationHandler handler = new ProductInOrderQualificationHandler();
                IProductInOrderQualificationExtension extension = Create.New<IProductInOrderQualificationExtension>();
                extension.ProductID = new Random().Next();
                extension.Quantity = 2;

                orderContext.Order.AddItem(new Random().Next(), 1);
                Assert.IsTrue(!handler.Matches(Create.New<IPromotion>(), extension, orderContext));
            }
        }
    }
}
