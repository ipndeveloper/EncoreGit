using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NetSteps.Content.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Modules.Order.Common;
using NetSteps.Modules.Order.Common.Results;

namespace NetSteps.Modules.Order.Tests
{
	[TestClass]
	public class OrderTests
	{
		private TestContext testContextInstance;

		public OrderTests() { }

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}

		private IOrder CreateSearchTestOrder(int userID)
		{
			Random random = new Random();
			int orderID = random.Next(1, 10000);


			var order = Create.Mutation(Create.New<IOrder>(), it =>
			{
				it.Date = DateTime.Now;
				it.OrderID = orderID;
				it.OwnerID = userID;
				it.Status = "Paid";
				it.Total = 100M;
				it.Volume = 25M;
			});

			return order;
		}

		private IOrder CreateMoveTestOrder(int userID, int orderID)
		{
			var order = Create.Mutation(Create.New<IOrder>(), it =>
			{
				it.AccountID = userID;
				it.OrderID = orderID;
				it.Success = true;
			});

			return order;
		}

        private IOrderCreate CreateNewOrderModel(int accountID)
        {
            var order = Create.Mutation(Create.New<IOrderCreate>(), it =>
            {
                it.AccountID = accountID;
                it.AccountTypeID = 1;
                it.CurrencyID = 1;
                it.OrderTypeID = 11;
				it.ShippingMethodID = 1;
            });

            IList<IProduct> products = new List<IProduct>();
            products.Add(CreateNewProduct());
            order.Products = products;

            return order;
        }

        private IProduct CreateNewProduct()
        {
            var product = Create.Mutation(Create.New<IProduct>(), it =>
                {
                    it.ProductID = 1;
                    it.Quantity = 1;
                    it.Sku = "500";
                });

            return product;
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

		[TestMethod]
		public void OrderSearch_ReturnACollectionOfOrderResults()
		{
			// Arrange
			var model = Create.New<ILoadOrderModel>();
			int count = 3;
			int userID = 1000;
			model.AccountID = userID;
			model.NumberOfRecords = count;
			model.OrderDate = DateTime.Now.AddDays(-30.0);

			IList<IOrder> orders = new List<IOrder>();

			var repository = new Mock<IOrderRepositoryAdapter>();
            var termTranslation = new Mock<ITermResolver>();

			for (int i = 0; i < count; i++)
			{
				orders.Add(CreateSearchTestOrder(userID));
			}

			repository.Setup(x => x.LoadOrders(model)).Returns(orders.AsEnumerable());

            var orderSearch = new DefaultOrder(repository.Object, termTranslation.Object);

			// Act
			var result = orderSearch.LoadOrders(model);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(IEnumerable<IOrderSearchResult>));
			Assert.AreEqual(result.Count(), count);
		}

		[TestMethod]
		public void OrderMove_ReturnNewIOrderResult()
		{
			// Arrange
			int userID = 1000;
			int orderID = 5;
            
			var repository = new Mock<IOrderRepositoryAdapter>();
            var termTranslation = new Mock<ITermResolver>();
			repository.Setup<IOrder>(x => x.MoveOrder(It.IsAny<int>(), It.IsAny<int>())).Returns(CreateMoveTestOrder(userID, orderID));

			var orderMove = new DefaultOrder(repository.Object, termTranslation.Object);

			// Act
			var result = orderMove.MoveOrder(orderID, userID);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(IOrderMoveResult));
			Assert.IsTrue(result.Success);
			Assert.AreEqual(result.AccountID, userID);
		}

        [TestMethod]
        public void CreateOrder_ReturnsNewIOrderCreateResult()
        {
            // Arrange
            int accountID = 1000;
            int orderID = 5;

            var repository = new Mock<IOrderRepositoryAdapter>();
            var termTranslation = new Mock<ITermResolver>();

            var order = CreateNewOrderModel(accountID);
            var orderResult = CreateNewOrderCreateResult(orderID);

            repository.Setup<IOrderCreateResult>(x => x.CreateOrder(It.IsAny<IOrderCreate>())).Returns(orderResult);

            var adapter = new DefaultOrder(repository.Object, termTranslation.Object);
            
            // Act
            var result = adapter.CreateOrder(order);
            
            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IOrderCreateResult));
            Assert.IsTrue(result.Success);
            Assert.AreEqual(orderID, result.OrderID);
        }
        
	}
}
