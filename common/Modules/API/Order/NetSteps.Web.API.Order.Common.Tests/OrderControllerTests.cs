using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NetSteps.Content.Common;
using NetSteps.Diagnostics.Logging.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Modules.Order.Common;
using NetSteps.Web.API.Order.Common;
using NetSteps.Web.API.Order.Common.Models;
using NetSteps.Modules.Order.Common.Results;

namespace NetSteps.Web.API.Order.Common.Tests
{
    [TestClass]
    public class OrderControllerTests
    {
        private IEnumerable<IOrderSearchResult> CreateOrderSearchResultCollection(int userID)
        {
            IList<IOrderSearchResult> results = new List<IOrderSearchResult>();

            for (int i = 0; i < 3; i++)
            {
                var order = Create.New<IOrderSearchResult>();
                order.Date = DateTime.Now;
                order.OrderID = (i + 1) * 1000;
                order.OwnerID = userID;
                order.Status = "Paid";
                order.Total = 100M;
                order.Volume = 25M;

                results.Add(order);
            }            

            return results;
        }

        private IOrderMoveResult CreateOrderMoveResult(int userID)
        {
            var result = Create.New<IOrderMoveResult>();
            result.AccountID = userID;
            result.Success = true;

            return result;            
        }

        private IOrderCreateResult CreateNewOrderCreateResult(int orderID)
        {
            var result = Create.Mutation(Create.New<IOrderCreateResult>(), it =>
            {
                it.ErrorMessages = new List<string>();
                it.OrderID = orderID;
                it.Success = true;
            });

            return result;
        }

        private OrderViewModel CreateNewOrderViewModel()
        {
            var result = Create.New<OrderViewModel>();            
            result.AccountID = 1000;
            result.AccountTypeID = 1;
            result.CurrencyID = 1;
            result.OrderTypeID = 11;            

            IList<ProductViewModel> products = new List<ProductViewModel>();
            products.Add(CreateNewProduct());
            result.Products = products;

            return result;
        }

        private ProductViewModel CreateNewProduct()
        {
            var result = Create.New<ProductViewModel>();            
            result.ProductID = 1;
            result.Quantity = 1;
            result.Sku = "111";            

            return result;
        }

        [TestMethod]
        public void Order_OrderLoad_Returns_Json()
        {
            // Arrange            
            int userID = 1000;

			var model = new LoadOrderModel();
			model.AccountID = userID;
			model.NumberOfRecords = 10;

            var site = new Mock<ISiteOrder>();
            var log = new Mock<ILogResolver>();
            var term = new Mock<ITermResolver>();

            var orders = CreateOrderSearchResultCollection(userID);
            var controller = new OrderController(site.Object, log.Object, term.Object);

            site.Setup<IEnumerable<IOrderSearchResult>>(x => x.LoadOrders(It.IsAny<ILoadOrderModel>())).Returns(orders);

            // Act
            var result = controller.LoadOrders(userID, model);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NetSteps.Web.API.Base.Common.JsonResult));            
        }

        [TestMethod]
        public void Order_OrderMove_Returns_Json()
        {
            // Arrange            
            int userID = 1000;
            int orderID = 10000;

            var site = new Mock<ISiteOrder>();
            var log = new Mock<ILogResolver>();
            var term = new Mock<ITermResolver>();

            var moveResult = CreateOrderMoveResult(userID);
            var controller = new OrderController(site.Object, log.Object, term.Object);

            site.Setup<IOrderMoveResult>(x => x.MoveOrder(It.IsAny<int>(), It.IsAny<int>())).Returns(moveResult);

            // Act
            var result = controller.MoveOrder(orderID, userID);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NetSteps.Web.API.Base.Common.JsonResult));
        }

        [TestMethod]
        public void Order_CreateOrder_Returns_Json()
        {
            // Arrange
            int orderID = 5;

            var site = new Mock<ISiteOrder>();
            var log = new Mock<ILogResolver>();
            var term = new Mock<ITermResolver>();

            var controller = new OrderController(site.Object, log.Object, term.Object);

            var orderModel = CreateNewOrderViewModel();
            var orderCreate = CreateNewOrderCreateResult(orderID);
            site.Setup<IOrderCreateResult>(x => x.CreateOrder(It.IsAny<IOrderCreate>())).Returns(orderCreate);
            
            // Act
            var result = controller.CreateOrder(orderModel);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NetSteps.Web.API.Base.Common.JsonResult));
        }

    }
}
