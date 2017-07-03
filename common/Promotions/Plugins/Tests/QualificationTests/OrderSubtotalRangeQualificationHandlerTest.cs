using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Plugins.Common.Qualifications.Concrete;
using NetSteps.Extensibility.Core;
using NetSteps.Promotions.Plugins.Qualifications;
using NetSteps.Promotions.Plugins.Common.Qualifications;
using NetSteps.Promotions.Plugins.Tests.QualificationTests;
using NetSteps.Promotions.Common.Model;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Data.Common.Entities;
using Moq;

namespace NetSteps.Promotions.Plugins.Test.QualificationTests
{
    [TestClass]
    public class OrderSubtotalRangeQualificationHandlerTest : BaseQualificationHandlerTestProxy<OrderSubtotalRangeQualificationHandler>
    {
        private void mockWireup()
        {
        }

        [TestMethod]
        public void OrderSubtotalRangeQualificationHandler_should_be_registered()
        {

            using (var container = Create.NewContainer())
            {
                mockWireup();
                // test the IOC registration to verify that we've got it correct
                var registry = Create.New<IDataObjectExtensionProviderRegistry>();
                var handler = registry.RetrieveExtensionProvider(QualificationExtensionProviderKeys.OrderSubtotalRangeProviderKey);
                Assert.IsNotNull(handler);
				Assert.IsInstanceOfType(handler, typeof(OrderSubtotalRangeQualificationHandler));
            }
        }

        [TestMethod]
        public void OrderSubtotalRangeQualificationHandler_should_match_if_subtotal_equals_minimum()
        {

            using (var container = Create.NewContainer())
            {
                var orderContext = GetMockOrderContext(1);

                var mockOrderItem = new Mock<IOrderItem>().SetupAllProperties();
                mockOrderItem.Object.ItemPrice = 1000;
                orderContext.Order.OrderCustomers[0].OrderItems.Add(mockOrderItem.Object);


                orderContext.Order.AddItem(new Random().Next(), 1);
                orderContext.Order.CalculateTotals();
                OrderSubtotalRangeQualificationHandler handler = new OrderSubtotalRangeQualificationHandler();
                IOrderSubtotalRangeQualificationExtension extension = Create.New<IOrderSubtotalRangeQualificationExtension>();
                extension.OrderSubtotalRangesByCurrencyID.Add(orderContext.Order.CurrencyID, new OrderSubtotalRange(1000, null));

                Assert.IsTrue(handler.Matches(Create.New<IPromotion>(), extension, orderContext));
            }
        }

        [TestMethod]
        public void OrderSubtotalRangeQualificationHandler_should_match_if_subtotal_is_greater_than_minimum()
        {

            using (var container = Create.NewContainer())
            {
                var orderContext = GetMockOrderContext(1);

                var mockOrderItem = new Mock<IOrderItem>().SetupAllProperties();
                mockOrderItem.Object.ItemPrice = 1000;
                orderContext.Order.OrderCustomers[0].OrderItems.Add(mockOrderItem.Object);

                orderContext.Order.AddItem(new Random().Next(), 1);
                orderContext.Order.CalculateTotals();
                OrderSubtotalRangeQualificationHandler handler = new OrderSubtotalRangeQualificationHandler();
                IOrderSubtotalRangeQualificationExtension extension = Create.New<IOrderSubtotalRangeQualificationExtension>();
				extension.OrderSubtotalRangesByCurrencyID.Add(orderContext.Order.CurrencyID, new OrderSubtotalRange(1000 - .01M, null));

                Assert.IsTrue(handler.Matches(Create.New<IPromotion>(), extension, orderContext));
            }
        }

        [TestMethod]
        public void OrderSubtotalRangeQualificationHandler_should_not_match_if_subtotal_is_less_than_minimum()
        {

            using (var container = Create.NewContainer())
            {
                var orderContext = GetMockOrderContext(1);

                var mockOrderItem = new Mock<IOrderItem>().SetupAllProperties();
                mockOrderItem.Object.ItemPrice = 1000;
                orderContext.Order.OrderCustomers[0].OrderItems.Add(mockOrderItem.Object);


                orderContext.Order.AddItem(new Random().Next(), 1);
                orderContext.Order.CalculateTotals();
                OrderSubtotalRangeQualificationHandler handler = new OrderSubtotalRangeQualificationHandler();
                IOrderSubtotalRangeQualificationExtension extension = Create.New<IOrderSubtotalRangeQualificationExtension>();
				extension.OrderSubtotalRangesByCurrencyID.Add(orderContext.Order.CurrencyID, new OrderSubtotalRange(1000 + .01M, null));

                Assert.IsTrue(!handler.Matches(Create.New<IPromotion>(), extension, orderContext));
            }
        }

		[TestMethod]
		public void OrderSubtotalRangeQualificationHandler_should_not_match_if_subtotal_is_greater_than_maximum()
		{

			using (var container = Create.NewContainer())
			{
                var orderContext = GetMockOrderContext(1);

                var mockOrderItem = new Mock<IOrderItem>().SetupAllProperties();
                mockOrderItem.Object.ItemPrice = 1000;
                orderContext.Order.OrderCustomers[0].OrderItems.Add(mockOrderItem.Object);


				orderContext.Order.AddItem(new Random().Next(), 1);
				orderContext.Order.CalculateTotals();
				OrderSubtotalRangeQualificationHandler handler = new OrderSubtotalRangeQualificationHandler();
				IOrderSubtotalRangeQualificationExtension extension = Create.New<IOrderSubtotalRangeQualificationExtension>();
				extension.OrderSubtotalRangesByCurrencyID.Add(orderContext.Order.CurrencyID, new OrderSubtotalRange(0M, 1000 - .01M));

				Assert.IsTrue(!handler.Matches(Create.New<IPromotion>(), extension, orderContext));
			}
		}
    }
}
