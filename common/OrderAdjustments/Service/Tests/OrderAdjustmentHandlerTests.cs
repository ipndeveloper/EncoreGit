using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Data.Common.Entities;
using NetSteps.Encore.Core.IoC;
using NetSteps.Extensibility.Core;
using NetSteps.OrderAdjustments.Common;
using NetSteps.OrderAdjustments.Common.Exceptions;
using NetSteps.OrderAdjustments.Common.Model;
using NetSteps.OrderAdjustments.Service.Test.Mocks;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Data.Common.Services;
using Moq;
using NetSteps.Data.Common.Context;

namespace NetSteps.OrderAdjustments.Service.Test
{
    [TestClass]
    public class OrderAdjustmentHandlerTests
    {
        MockOrderAdjustmentProviderManager _providerManager { get; set; }
        MockOrderAdjustmentProvider _provider { get; set; }
        int _productID;
        int _accountID;


        [TestInitialize]
        public void Init()
        {
        }

		private void MockWireup(IContainer container)
		{
			WireupCoordinator.SelfConfigure();

			_productID = new Random().Next();
			_accountID = new Random().Next();
			_providerManager = new MockOrderAdjustmentProviderManager(_productID, _accountID);
			_provider = new MockOrderAdjustmentProvider();
			_providerManager.RegisterAdjustmentProvider(_provider);
			Create.New<IDataObjectExtensionProviderRegistry>().RegisterExtensionProvider<MockOrderAdjustmentProvider>(MockOrderAdjustmentProvider.ProviderKey);
		}

        [TestMethod]
        public void OrderAdjustmentHandler_should_be_registered()
        {
			using (var container = Create.NewContainer())
			{
				container.ForType<IOrderAdjustmentProviderManager>()
					.Register<IOrderAdjustmentProviderManager>((c, p) => { return _providerManager; })
					.ResolveAsSingleton()
					.End();

				container.ForType<IInventoryService>()
					.Register<IInventoryService>((c, p) => { return new Mock<IInventoryService>().Object; })
					.ResolveAsSingleton()
					.End();

				MockWireup(container);

				// test the IOC registration to verify that we've got it correct
				var adjustmentHandler = Create.New<IOrderAdjustmentHandler>();
				Assert.IsNotNull(adjustmentHandler);
				Assert.IsInstanceOfType(adjustmentHandler, typeof(IOrderAdjustmentHandler));
			}
        }

        [TestMethod]
        public void OrderAdjustmentHandler_should_get_order_adjustments_currently_applied_to_an_order()
        {
			using (var container = Create.NewContainer())
			{
				MockWireup(container);

				var mockInventoryService = new Mock<IInventoryService>();
				mockInventoryService.Setup(x => x.GetProductAvailabilityForOrder(It.IsAny<IOrderContext>(), It.IsAny<int>(), It.IsAny<int>()))
					.Returns(
								(IOrderContext context, int productID, int quantity) =>
								{
									var mockResponse = new Mock<IInventoryProductCheckResponse>();
									mockResponse.SetupAllProperties();
									mockResponse.SetupGet<int>(prop => prop.CanAddNormally).Returns(1);
									return mockResponse.Object;
								}
							);

				
				OrderAdjustmentHandler handler = new OrderAdjustmentHandler(_providerManager,
					Create.New<IDataObjectExtensionProviderRegistry>(),
					mockInventoryService.Object);

				MockOrderContext orderContext = new MockOrderContext(_accountID);

				// add adjustment profiles
				MockOrderAdjustmentProfile newProfile = _provider.AddAdjustment(MockOrderAdjustmentCreator.Generate(MockAdjustmentTypes.AddedItem, _productID, _accountID));
				IEnumerable<IOrderAdjustmentProfile> adjustments = _provider.GetApplicableAdjustments(orderContext);
				Console.WriteLine("newProfile ID:" + newProfile.MockOrderAdjustmentProfileID);
				// apply adjustments
				handler.ApplyAdjustments(orderContext, adjustments);

				//verify that an adjustment has been applied
				Assert.AreEqual(1, orderContext.Order.OrderCustomers[0].OrderItems.Count());
				Assert.AreEqual(_productID, orderContext.Order.OrderCustomers[0].OrderItems[0].ProductID);
				Assert.AreEqual(1, orderContext.Order.OrderCustomers[0].OrderItems[0].Quantity);

				// get the applied adjustments and verify that they match the adjustments created.
				List<IOrderAdjustmentProfile> appliedAdjustments = handler.GetOrderAdjustments(orderContext).ToList();
				Assert.AreEqual(1, appliedAdjustments.Count);
				Assert.IsInstanceOfType(appliedAdjustments[0], typeof(MockOrderAdjustmentProfile));
				Assert.AreEqual(newProfile.MockOrderAdjustmentProfileID, (appliedAdjustments[0] as MockOrderAdjustmentProfile).MockOrderAdjustmentProfileID);
			}
        }

        [TestMethod]
        public void OrderAdjustmentHandler_should_remove_an_orderContext_adjustment_from_an_orderContext()
        {
			using (var container = Create.NewContainer())
			{
				MockWireup(container);
				var mockInventoryService = new Mock<IInventoryService>();
				mockInventoryService.Setup(x => x.GetProductAvailabilityForOrder(It.IsAny<IOrderContext>(), It.IsAny<int>(), It.IsAny<int>()))
					.Returns(
								(IOrderContext context, int productID, int quantity) =>
								{
									var mockResponse = new Mock<IInventoryProductCheckResponse>();
									mockResponse.SetupAllProperties();
									mockResponse.SetupGet<int>(prop => prop.CanAddNormally).Returns(1);
									return mockResponse.Object;
								}
							);

				
				OrderAdjustmentHandler handler = new OrderAdjustmentHandler(_providerManager,
					Create.New<IDataObjectExtensionProviderRegistry>(),
					mockInventoryService.Object);

				MockOrderContext orderContext = new MockOrderContext(_accountID);

				// add adjustments
				MockOrderAdjustmentProfile addProfile = _provider.AddAdjustment(MockOrderAdjustmentCreator.Generate(MockAdjustmentTypes.AddedItem, _productID, _accountID));
				IEnumerable<IOrderAdjustmentProfile> adjustments = _provider.GetApplicableAdjustments(orderContext);

				// apply adjustments
				handler.ApplyAdjustments(orderContext, adjustments);

				// verify that the adjustment took place
				Assert.AreEqual(1, orderContext.Order.OrderCustomers[0].OrderItems.Count());
				Assert.AreEqual(_productID, orderContext.Order.OrderCustomers[0].OrderItems[0].ProductID);
				Assert.AreEqual(1, orderContext.Order.OrderCustomers[0].OrderItems[0].Quantity);

				orderContext.Order.CalculationsDirty = true;
				orderContext.Order.CalculateTotals();
				decimal? orderTotal = orderContext.Order.GrandTotal;

				handler.RemoveAdjustment(orderContext, orderContext.Order.OrderAdjustments[0]);
				Assert.AreEqual(0, orderContext.Order.OrderAdjustments.Count());
				Assert.AreEqual(0, orderContext.Order.OrderCustomers[0].OrderItems.Count());
			}
        }

        [TestMethod]
        public void OrderAdjustmentHandler_should_remove_anorderContextmodification_fromorderContextadjustment_from_anorderContext()
        {
			using (var container = Create.NewContainer())
			{
				MockWireup(container);
				var mockInventoryService = new Mock<IInventoryService>();
				mockInventoryService.Setup(x => x.GetProductAvailabilityForOrder(It.IsAny<IOrderContext>(), It.IsAny<int>(), It.IsAny<int>()))
					.Returns(
								(IOrderContext context, int productID, int quantity) =>
								{
									var mockResponse = new Mock<IInventoryProductCheckResponse>();
									mockResponse.SetupAllProperties();
									mockResponse.SetupGet<int>(prop => prop.CanAddNormally).Returns(1);
									return mockResponse.Object;
								}
							);

				
				OrderAdjustmentHandler handler = new OrderAdjustmentHandler(_providerManager,
					Create.New<IDataObjectExtensionProviderRegistry>(),
					mockInventoryService.Object);

				MockOrderContext orderContext = new MockOrderContext(_accountID);

				// add adjustments
				MockOrderAdjustmentProfile addProfile = _provider.AddAdjustment(MockOrderAdjustmentCreator.Generate(MockAdjustmentTypes.ReducedShippingTotalBy10Flat, _productID, _accountID));
				IEnumerable<IOrderAdjustmentProfile> adjustments = _provider.GetApplicableAdjustments(orderContext);

				orderContext.Order.ShippingTotal = 1500.25M;

				// apply adjustments
				handler.ApplyAdjustments(orderContext, adjustments);
				orderContext.Order.CalculationsDirty = true;
				orderContext.Order.CalculateTotals();

				Assert.AreEqual(10M, orderContext.Order.OrderAdjustments[0].OrderModifications[0].ModificationDecimalValue);

				handler.RemoveAdjustment(orderContext, orderContext.Order.OrderAdjustments[0]);
				foreach (IOrderAdjustment adjustment in orderContext.Order.OrderAdjustments)
				{
					IOrderAdjustmentProvider provider = _providerManager.GetProvider(adjustment.ExtensionProviderKey);
					Assert.IsFalse(provider.IsInstanceOfProfile(adjustment, addProfile));
				}

				orderContext.Order.CalculationsDirty = true;
				orderContext.Order.CalculateTotals();
				Assert.AreEqual(0, orderContext.Order.OrderAdjustments.Count());
			}
        }

        [TestMethod]
        public void OrderAdjustmentHandler_should_remove_an_item_if_added_by_orderContextadjustment_from_an_orderContext()
        {
			using (var container = Create.NewContainer())
			{
				MockWireup(container);
				var mockInventoryService = new Mock<IInventoryService>();
				mockInventoryService.Setup(x => x.GetProductAvailabilityForOrder(It.IsAny<IOrderContext>(), It.IsAny<int>(), It.IsAny<int>()))
					.Returns(
								(IOrderContext context, int productID, int quantity) =>
								{
									var mockResponse = new Mock<IInventoryProductCheckResponse>();
									mockResponse.SetupAllProperties();
									mockResponse.SetupGet<int>(prop => prop.CanAddNormally).Returns(1);
									return mockResponse.Object;
								}
							);

				
				OrderAdjustmentHandler handler = new OrderAdjustmentHandler(_providerManager,
					Create.New<IDataObjectExtensionProviderRegistry>(),
					mockInventoryService.Object);

				MockOrderContext orderContext = new MockOrderContext(_accountID);

				// add adjustments
				MockOrderAdjustmentProfile addProfile = _provider.AddAdjustment(MockOrderAdjustmentCreator.Generate(MockAdjustmentTypes.AddedItem, _productID, _accountID));
				IEnumerable<IOrderAdjustmentProfile> adjustments = _provider.GetApplicableAdjustments(orderContext);

				// apply adjustments
				handler.ApplyAdjustments(orderContext, adjustments);

				// verify that the adjustment took place
				Assert.AreEqual(1, orderContext.Order.OrderCustomers[0].OrderItems.Count());
				Assert.AreEqual(_productID, orderContext.Order.OrderCustomers[0].OrderItems[0].ProductID);
				Assert.AreEqual(1, orderContext.Order.OrderCustomers[0].OrderItems[0].Quantity);

				handler.RemoveAdjustment(orderContext, orderContext.Order.OrderAdjustments[0]);
				foreach (IOrderAdjustment adjustment in orderContext.Order.OrderAdjustments)
				{
					IOrderAdjustmentProvider provider = _providerManager.GetProvider(adjustment.ExtensionProviderKey);
					Assert.IsFalse(provider.IsInstanceOfProfile(adjustment, addProfile));
				}
				Assert.AreEqual(0, orderContext.Order.OrderCustomers[0].OrderItems.Count());
			}
        }

        [TestMethod]
        public void OrderAdjustmentHandler_should_applyorderContext_modification_AddedItem_to_OrderContext()
        {
			using (var container = Create.NewContainer())
			{
				MockWireup(container);
				var mockInventoryService = new Mock<IInventoryService>();
				mockInventoryService.Setup(x => x.GetProductAvailabilityForOrder(It.IsAny<IOrderContext>(), It.IsAny<int>(), It.IsAny<int>()))
					.Returns(
								(IOrderContext context, int productID, int quantity) =>
								{
									var mockResponse = new Mock<IInventoryProductCheckResponse>();
									mockResponse.SetupAllProperties();
									mockResponse.SetupGet<int>(prop => prop.CanAddNormally).Returns(1);
									return mockResponse.Object;
								}
							);

				
				OrderAdjustmentHandler handler = new OrderAdjustmentHandler(_providerManager,
					Create.New<IDataObjectExtensionProviderRegistry>(),
					mockInventoryService.Object);

				MockOrderContext orderContext = new MockOrderContext(_accountID);

				// add adjustments
				_provider.AddAdjustment(MockOrderAdjustmentCreator.Generate(MockAdjustmentTypes.AddedItem, _productID, _accountID));
				IEnumerable<IOrderAdjustmentProfile> adjustments = _provider.GetApplicableAdjustments(orderContext);

				// apply adjustments
				handler.ApplyAdjustments(orderContext, adjustments);

				// verify that the adjustment took place
				Assert.AreEqual(1, orderContext.Order.OrderCustomers[0].OrderItems.Count());
				Assert.AreEqual(_productID, orderContext.Order.OrderCustomers[0].OrderItems[0].ProductID);
				Assert.AreEqual(1, orderContext.Order.OrderCustomers[0].OrderItems[0].Quantity);
			}
        }

        [TestMethod]
        public void OrderAdjustmentHandler_should_applyorderContext_modification_AddedItemWithQuantity2_to_OrderContext()
        {
			using (var container = Create.NewContainer())
			{
				MockWireup(container);
				var mockInventoryService = new Mock<IInventoryService>();
				mockInventoryService.Setup(x => x.GetProductAvailabilityForOrder(It.IsAny<IOrderContext>(), It.IsAny<int>(), It.IsAny<int>()))
					.Returns(
								(IOrderContext context, int productID, int quantity) =>
								{
									var mockResponse = new Mock<IInventoryProductCheckResponse>();
									mockResponse.SetupAllProperties();
									mockResponse.SetupGet<int>(prop => prop.CanAddNormally).Returns(2);
									return mockResponse.Object;
								}
							);

				
				OrderAdjustmentHandler handler = new OrderAdjustmentHandler(_providerManager,
					Create.New<IDataObjectExtensionProviderRegistry>(),
					mockInventoryService.Object);

				MockOrderContext orderContext = new MockOrderContext(_accountID);

				// add adjustments
				_provider.AddAdjustment(MockOrderAdjustmentCreator.Generate(MockAdjustmentTypes.AddedItemWithQuantity2, _productID, _accountID));
				IEnumerable<IOrderAdjustmentProfile> adjustments = _provider.GetApplicableAdjustments(orderContext);

				// apply adjustments
				handler.ApplyAdjustments(orderContext, adjustments);

				Assert.AreEqual(1, orderContext.Order.OrderCustomers[0].OrderItems.Count());
				Assert.AreEqual(_productID, orderContext.Order.OrderCustomers[0].OrderItems[0].ProductID);
				Assert.AreEqual(2, orderContext.Order.OrderCustomers[0].OrderItems[0].Quantity);
			}
        }

        [TestMethod]
        public void OrderAdjustmentHandler_should_applyorderContext_modification_ReducedProduct1PriceBy14Percent_to_OrderContext()
        {
			using (var container = Create.NewContainer())
			{
				MockWireup(container);
				var mockInventoryService = new Mock<IInventoryService>();
				mockInventoryService.Setup(x => x.GetProductAvailabilityForOrder(It.IsAny<IOrderContext>(), It.IsAny<int>(), It.IsAny<int>()))
					.Returns(
								(IOrderContext context, int productID, int quantity) =>
								{
									var mockResponse = new Mock<IInventoryProductCheckResponse>();
									mockResponse.SetupAllProperties();
									mockResponse.SetupGet<int>(prop => prop.CanAddNormally).Returns(1);
									return mockResponse.Object;
								}
							);

				
				OrderAdjustmentHandler handler = new OrderAdjustmentHandler(_providerManager,
					Create.New<IDataObjectExtensionProviderRegistry>(),
					mockInventoryService.Object);

				MockOrderContext orderContext = new MockOrderContext(_accountID);

				// add adjustments
				_provider.AddAdjustment(MockOrderAdjustmentCreator.Generate(MockAdjustmentTypes.ReducedProduct1PriceBy14Percent, _productID, _accountID));
				IEnumerable<IOrderAdjustmentProfile> adjustments = _provider.GetApplicableAdjustments(orderContext);

				//add the item to the cart.
				orderContext.Order.AddItem(_productID, 1);

				// apply adjustments
				handler.ApplyAdjustments(orderContext, adjustments);

				Assert.AreEqual(1, orderContext.Order.OrderCustomers[0].OrderItems.Count());
				Assert.AreEqual(_productID, orderContext.Order.OrderCustomers[0].OrderItems[0].ProductID);
				Assert.AreEqual(1, orderContext.Order.OrderCustomers[0].OrderItems[0].OrderLineModifications.Count);
				Assert.AreEqual(.14M, orderContext.Order.OrderCustomers[0].OrderItems[0].OrderLineModifications[0].ModificationDecimalValue);
			}
        }

        [TestMethod]
        public void OrderAdjustmentHandler_should_applyorderContext_modification_ReducedProduct1PriceBy16Flat_to_OrderContext()
        {
			using (var container = Create.NewContainer())
			{
				MockWireup(container);
				var mockInventoryService = new Mock<IInventoryService>();
				mockInventoryService.Setup(x => x.GetProductAvailabilityForOrder(It.IsAny<IOrderContext>(), It.IsAny<int>(), It.IsAny<int>()))
					.Returns(
								(IOrderContext context, int productID, int quantity) =>
								{
									var mockResponse = new Mock<IInventoryProductCheckResponse>();
									mockResponse.SetupAllProperties();
									mockResponse.SetupGet<int>(prop => prop.CanAddNormally).Returns(1);
									return mockResponse.Object;
								}
							);

				
				OrderAdjustmentHandler handler = new OrderAdjustmentHandler(_providerManager,
					Create.New<IDataObjectExtensionProviderRegistry>(),
					mockInventoryService.Object);

				MockOrderContext orderContext = new MockOrderContext(_accountID);

				// add adjustments
				_provider.AddAdjustment(MockOrderAdjustmentCreator.Generate(MockAdjustmentTypes.ReducedProduct1PriceBy16Flat, _productID, _accountID));
				IEnumerable<IOrderAdjustmentProfile> adjustments = _provider.GetApplicableAdjustments(orderContext);

				//add the item to the cart.
				orderContext.Order.AddItem(_productID, 1);

				// apply adjustments
				handler.ApplyAdjustments(orderContext, adjustments);

				Assert.AreEqual(1, orderContext.Order.OrderCustomers[0].OrderItems.Count());
				Assert.AreEqual(_productID, orderContext.Order.OrderCustomers[0].OrderItems[0].ProductID);
				Assert.AreEqual(1, orderContext.Order.OrderCustomers[0].OrderItems[0].OrderLineModifications.Count);
				Assert.AreEqual(16M, orderContext.Order.OrderCustomers[0].OrderItems[0].OrderLineModifications[0].ModificationDecimalValue);
			}
        }

        [TestMethod]
        public void OrderAdjustmentHandler_should_applyorderContext_modification_ReducedShippingTotalBy10Flat_to_OrderContext()
        {
			using (var container = Create.NewContainer())
			{
				MockWireup(container);
				var mockInventoryService = new Mock<IInventoryService>();
				mockInventoryService.Setup(x => x.GetProductAvailabilityForOrder(It.IsAny<IOrderContext>(), It.IsAny<int>(), It.IsAny<int>()))
					.Returns(
								(IOrderContext context, int productID, int quantity) =>
								{
									var mockResponse = new Mock<IInventoryProductCheckResponse>();
									mockResponse.SetupAllProperties();
									mockResponse.SetupGet<int>(prop => prop.CanAddNormally).Returns(1);
									return mockResponse.Object;
								}
							);

				OrderAdjustmentHandler handler = new OrderAdjustmentHandler(_providerManager,
					Create.New<IDataObjectExtensionProviderRegistry>(),
					mockInventoryService.Object);

				MockOrderContext orderContext = new MockOrderContext(_accountID);

				// add adjustments
				_provider.AddAdjustment(MockOrderAdjustmentCreator.Generate(MockAdjustmentTypes.ReducedShippingTotalBy10Flat, _productID, _accountID));
				IEnumerable<IOrderAdjustmentProfile> adjustments = _provider.GetApplicableAdjustments(orderContext);

				//set the shipping total
				orderContext.Order.ShippingTotal = 15M;

				// apply adjustments
				handler.ApplyAdjustments(orderContext, adjustments);

				Assert.AreEqual(1, orderContext.Order.OrderAdjustments[0].OrderModifications.Count);
				Assert.AreEqual(10M, orderContext.Order.OrderAdjustments[0].OrderModifications[0].ModificationDecimalValue);
			}
        }

        [TestMethod]
        public void OrderAdjustmentHandler_should_applyorderContext_modification_ReducedShippingTotalBy20Percent_to_OrderContext()
        {
			using (var container = Create.NewContainer())
			{
				MockWireup(container);
				var mockInventoryService = new Mock<IInventoryService>();
				mockInventoryService.Setup(x => x.GetProductAvailabilityForOrder(It.IsAny<IOrderContext>(), It.IsAny<int>(), It.IsAny<int>()))
					.Returns(
								(IOrderContext context, int productID, int quantity) =>
								{
									var mockResponse = new Mock<IInventoryProductCheckResponse>();
									mockResponse.SetupAllProperties();
									mockResponse.SetupGet<int>(prop => prop.CanAddNormally).Returns(1);
									return mockResponse.Object;
								}
							);

				OrderAdjustmentHandler handler = new OrderAdjustmentHandler(_providerManager,
					Create.New<IDataObjectExtensionProviderRegistry>(),
					mockInventoryService.Object);

				MockOrderContext orderContext = new MockOrderContext(_accountID);

				// add adjustments
				_provider.AddAdjustment(MockOrderAdjustmentCreator.Generate(MockAdjustmentTypes.ReducedShippingTotalBy20Percent, _productID, _accountID));
				IEnumerable<IOrderAdjustmentProfile> adjustments = _provider.GetApplicableAdjustments(orderContext);

				//add the shipping total
				orderContext.Order.ShippingTotal = 1300M;

				// apply adjustments
				handler.ApplyAdjustments(orderContext, adjustments);

				Assert.AreEqual(1, orderContext.Order.OrderAdjustments[0].OrderModifications.Count);
				Assert.AreEqual(.20M, orderContext.Order.OrderAdjustments[0].OrderModifications[0].ModificationDecimalValue);
			}
        }

        [TestMethod]
        public void OrderAdjustmentHandler_should_applyorderContext_modification_ReducedSingleProduct1PriceBy23Percent_to_OrderContext()
        {
			using (var container = Create.NewContainer())
			{
				MockWireup(container);
				var mockInventoryService = new Mock<IInventoryService>();
				mockInventoryService.Setup(x => x.GetProductAvailabilityForOrder(It.IsAny<IOrderContext>(), It.IsAny<int>(), It.IsAny<int>()))
					.Returns(
								(IOrderContext context, int productID, int quantity) =>
								{
									var mockResponse = new Mock<IInventoryProductCheckResponse>();
									mockResponse.SetupAllProperties();
									mockResponse.SetupGet<int>(prop => prop.CanAddNormally).Returns(1);
									return mockResponse.Object;
								}
							);

				OrderAdjustmentHandler handler = new OrderAdjustmentHandler(_providerManager,
					Create.New<IDataObjectExtensionProviderRegistry>(),
					mockInventoryService.Object);

				MockOrderContext orderContext = new MockOrderContext(_accountID);

				// add adjustments
				_provider.AddAdjustment(MockOrderAdjustmentCreator.Generate(MockAdjustmentTypes.ReducedSingleProduct1PriceBy23Percent, _productID, _accountID));
				IEnumerable<IOrderAdjustmentProfile> adjustments = _provider.GetApplicableAdjustments(orderContext);

				//add the item to the cart.
				orderContext.Order.AddItem(_productID, 2);

				// apply adjustments
				handler.ApplyAdjustments(orderContext, adjustments);

				Assert.AreEqual(_productID, orderContext.Order.OrderCustomers[0].OrderItems[0].ProductID);
				Assert.AreEqual(2, orderContext.Order.OrderCustomers[0].OrderItems.Count);
				Assert.AreEqual(1, orderContext.Order.OrderCustomers[0].OrderItems[0].OrderLineModifications.Count);
				Assert.AreEqual(.23M, orderContext.Order.OrderCustomers[0].OrderItems[0].OrderLineModifications[0].ModificationDecimalValue);
			}
        }

        [TestMethod]
        public void OrderAdjustmentHandler_should_applyorderContext_modification_ReducedSingleProduct1PriceBy24Flat_to_OrderContext()
        {
			using (var container = Create.NewContainer())
			{
				MockWireup(container);
				var mockInventoryService = new Mock<IInventoryService>();
				mockInventoryService.Setup(x => x.GetProductAvailabilityForOrder(It.IsAny<IOrderContext>(), It.IsAny<int>(), It.IsAny<int>()))
					.Returns(
								(IOrderContext context, int productID, int quantity) =>
								{
									var mockResponse = new Mock<IInventoryProductCheckResponse>();
									mockResponse.SetupAllProperties();
									mockResponse.SetupGet<int>(prop => prop.CanAddNormally).Returns(1);
									return mockResponse.Object;
								}
							);

				OrderAdjustmentHandler handler = new OrderAdjustmentHandler(_providerManager,
					Create.New<IDataObjectExtensionProviderRegistry>(),
					mockInventoryService.Object);

				MockOrderContext orderContext = new MockOrderContext(_accountID);

				// add adjustments
				_provider.AddAdjustment(MockOrderAdjustmentCreator.Generate(MockAdjustmentTypes.ReducedSingleProduct1PriceBy24Flat, _productID, _accountID));
				IEnumerable<IOrderAdjustmentProfile> adjustments = _provider.GetApplicableAdjustments(orderContext);

				//add the item to the cart.
				orderContext.Order.AddItem(_productID, 2);

				// apply adjustments
				handler.ApplyAdjustments(orderContext, adjustments);

				Assert.AreEqual(_productID, orderContext.Order.OrderCustomers[0].OrderItems[0].ProductID);
				Assert.AreEqual(2, orderContext.Order.OrderCustomers[0].OrderItems.Count);
				Assert.AreEqual(1, orderContext.Order.OrderCustomers[0].OrderItems[0].OrderLineModifications.Count);
				Assert.AreEqual(24M, orderContext.Order.OrderCustomers[0].OrderItems[0].OrderLineModifications[0].ModificationDecimalValue);
			}
        }

        [TestMethod]
        public void OrderAdjustmentHandler_should_use_registered_IOrderAdjustmentProvider_to_apply_adjustments()
        {
			using (var container = Create.NewContainer())
			{
				MockWireup(container);
				var mockInventoryService = new Mock<IInventoryService>();
				mockInventoryService.Setup(x => x.GetProductAvailabilityForOrder(It.IsAny<IOrderContext>(), It.IsAny<int>(), It.IsAny<int>()))
					.Returns(
								(IOrderContext context, int productID, int quantity) =>
								{
									var mockResponse = new Mock<IInventoryProductCheckResponse>();
									mockResponse.SetupAllProperties();
									mockResponse.SetupGet<int>(prop => prop.CanAddNormally).Returns(1);
									return mockResponse.Object;
								}
							);

				OrderAdjustmentHandler handler = new OrderAdjustmentHandler(_providerManager,
					Create.New<IDataObjectExtensionProviderRegistry>(),
					mockInventoryService.Object);

				MockOrderContext orderContext = new MockOrderContext(_accountID);
				orderContext.Order.ShippingTotal = 1000M;
				
				mockInventoryService.Setup(x => x.GetProductAvailabilityForOrder(It.IsAny<IOrderContext>(), It.IsAny<int>(), It.IsAny<int>()))
					.Returns(
								(IOrderContext context, int productID, int quantity) =>
								{
									var mockResponse = new Mock<IInventoryProductCheckResponse>();
									mockResponse.SetupAllProperties();
									mockResponse.SetupGet<int>(prop => prop.CanAddNormally).Returns(1);
									return mockResponse.Object;
								}
							);

				// add adjustments
				IEnumerable<IOrderAdjustmentProfile> allAdjustmentsCreated = MockOrderAdjustmentCreator.GenerateOneOfEach(_productID, _accountID);
				foreach (IOrderAdjustmentProfile profile in allAdjustmentsCreated)
				{
					_provider.AddAdjustment(profile as MockOrderAdjustmentProfile);
				}
				IEnumerable<IOrderAdjustmentProfile> adjustments = _provider.GetApplicableAdjustments(orderContext);

				// verify that there are currently no adjustments on the order
				Assert.AreEqual(0, orderContext.Order.OrderAdjustments.Count());

				// apply adjustments
				handler.ApplyAdjustments(orderContext, adjustments);

				// verify that all adjustments have been applied.
				Assert.AreEqual(allAdjustmentsCreated.Count(), orderContext.Order.OrderAdjustments.Count());
			}
        }

        [TestMethod]
        [ExpectedException(typeof(OrderAdjustmentProviderException))]
        public void OrderAdjustmentHandler_should_not_modifyorderContext_withorderContextStatus_of_NotSet()
        {
            helperMethod_orderContextAdjustmentHandler_should_or_should_not_modifyorderContext_withorderContextValidator(1);
        }

        [TestMethod]
        public void OrderAdjustmentHandler_should_modifyorderContext_withorderContextStatus_of_Pending()
        {
            helperMethod_orderContextAdjustmentHandler_should_or_should_not_modifyorderContext_withorderContextValidator(3);
        }

        [TestMethod]
        public void OrderAdjustmentHandler_should_modifyorderContext_withorderContextStatus_of_PendingError()
        {
            helperMethod_orderContextAdjustmentHandler_should_or_should_not_modifyorderContext_withorderContextValidator(5);
        }

        [TestMethod]
        [ExpectedException(typeof(OrderAdjustmentProviderException))]
        public void OrderAdjustmentHandler_should_not_modifyorderContext_withorderContextStatus_of_Cancelled()
        {
            helperMethod_orderContextAdjustmentHandler_should_or_should_not_modifyorderContext_withorderContextValidator(2);
        }

        [TestMethod]
        [ExpectedException(typeof(OrderAdjustmentProviderException))]
        public void OrderAdjustmentHandler_should_not_modifyorderContext_withorderContextStatus_of_CancelledPaid()
        {
            helperMethod_orderContextAdjustmentHandler_should_or_should_not_modifyorderContext_withorderContextValidator(4);
        }

        [TestMethod]
        [ExpectedException(typeof(OrderAdjustmentProviderException))]
        public void OrderAdjustmentHandler_should_not_modifyorderContext_withorderContextStatus_of_CreditCardDeclined()
        {
            helperMethod_orderContextAdjustmentHandler_should_or_should_not_modifyorderContext_withorderContextValidator(6);
        }

        [TestMethod]
        [ExpectedException(typeof(OrderAdjustmentProviderException))]
        public void OrderAdjustmentHandler_should_not_modifyorderContext_withorderContextStatus_of_CreditCardDeclinedRetry()
        {
            helperMethod_orderContextAdjustmentHandler_should_or_should_not_modifyorderContext_withorderContextValidator(8);
        }

        [TestMethod]
        [ExpectedException(typeof(OrderAdjustmentProviderException))]
        public void OrderAdjustmentHandler_should_not_modifyorderContext_withorderContextStatus_of_Paid()
        {
            helperMethod_orderContextAdjustmentHandler_should_or_should_not_modifyorderContext_withorderContextValidator(9);
        }

        [TestMethod]
        [ExpectedException(typeof(OrderAdjustmentProviderException))]
        public void OrderAdjustmentHandler_should_not_modifyorderContext_withorderContextStatus_of_PartiallyPaid()
        {
            helperMethod_orderContextAdjustmentHandler_should_or_should_not_modifyorderContext_withorderContextValidator(10);
        }

        [TestMethod]
        [ExpectedException(typeof(OrderAdjustmentProviderException))]
        public void OrderAdjustmentHandler_should_not_modifyorderContext_withorderContextStatus_of_PartiallyShipped()
        {
            helperMethod_orderContextAdjustmentHandler_should_or_should_not_modifyorderContext_withorderContextValidator(11);
        }

        [TestMethod]
        [ExpectedException(typeof(OrderAdjustmentProviderException))]
        public void OrderAdjustmentHandler_should_not_modifyorderContext_withorderContextStatus_of_Printed()
        {
            helperMethod_orderContextAdjustmentHandler_should_or_should_not_modifyorderContext_withorderContextValidator(12);
        }

        [TestMethod]
        [ExpectedException(typeof(OrderAdjustmentProviderException))]
        public void OrderAdjustmentHandler_should_not_modifyorderContext_withorderContextStatus_of_Shipped()
        {
            helperMethod_orderContextAdjustmentHandler_should_or_should_not_modifyorderContext_withorderContextValidator(13);
        }

        [TestMethod]
        public void OrderAdjustmentHandler_should_notify_providers_of_commit()
        {
			using (var container = Create.NewContainer())
			{
				MockWireup(container);
				var mockInventoryService = new Mock<IInventoryService>();
				mockInventoryService.Setup(x => x.GetProductAvailabilityForOrder(It.IsAny<IOrderContext>(), It.IsAny<int>(), It.IsAny<int>()))
					.Returns(
								(IOrderContext context, int productID, int quantity) =>
								{
									var mockResponse = new Mock<IInventoryProductCheckResponse>();
									mockResponse.SetupAllProperties();
									mockResponse.SetupGet<int>(prop => prop.CanAddNormally).Returns(1);
									return mockResponse.Object;
								}
							);

				OrderAdjustmentHandler handler = new OrderAdjustmentHandler(_providerManager,
					Create.New<IDataObjectExtensionProviderRegistry>(),
					mockInventoryService.Object);

				MockOrderContext orderContext = new MockOrderContext(_accountID);
				orderContext.Order.ShippingTotal = 1000M;

				// add adjustments
				IEnumerable<IOrderAdjustmentProfile> allAdjustmentsCreated = MockOrderAdjustmentCreator.GenerateOneOfEach(_productID, _accountID);
				foreach (IOrderAdjustmentProfile profile in allAdjustmentsCreated)
				{
					_provider.AddAdjustment(profile as MockOrderAdjustmentProfile);
				}
				IEnumerable<IOrderAdjustmentProfile> adjustments = _provider.GetApplicableAdjustments(orderContext);

				// verify that there are currently no adjustments on the order
				Assert.AreEqual(0, orderContext.Order.OrderAdjustments.Count());

				// apply adjustments
				handler.ApplyAdjustments(orderContext, adjustments);

				// verify that all adjustments have been applied.
				Assert.AreEqual(allAdjustmentsCreated.Count(), orderContext.Order.OrderAdjustments.Count());

				handler.CommitAdjustments(orderContext);
				foreach (IOrderAdjustment adjustment in orderContext.Order.OrderAdjustments)
				{
					Assert.IsTrue((adjustment as MockOrderAdjustment).WasCommitted);
				}
			}
        }

        private void helperMethod_orderContextAdjustmentHandler_should_or_should_not_modifyorderContext_withorderContextValidator(int testingStatus)
        {
			using (var container = Create.NewContainer())
			{
				MockWireup(container);
				MockOrderContext orderContext = new MockOrderContext(_accountID);
				orderContext.Order.OrderStatusID = (short)testingStatus;
				var mockInventoryService = new Mock<IInventoryService>();
				mockInventoryService.Setup(x => x.GetProductAvailabilityForOrder(It.IsAny<IOrderContext>(), It.IsAny<int>(), It.IsAny<int>()))
					.Returns(
								(IOrderContext context, int productID, int quantity) =>
								{
									var mockResponse = new Mock<IInventoryProductCheckResponse>();
									mockResponse.SetupAllProperties();
									mockResponse.SetupGet<int>(prop => prop.CanAddNormally).Returns(1);
									return mockResponse.Object;
								}
							);

				
				OrderAdjustmentHandler handler = new OrderAdjustmentHandler(_providerManager,
					Create.New<IDataObjectExtensionProviderRegistry>(),
					mockInventoryService.Object);


				try
				{
					MockOrderAdjustmentProvider provider = new MockOrderAdjustmentProvider();
					provider.AddAdjustment(MockOrderAdjustmentCreator.Generate(MockAdjustmentTypes.AddedItem, _productID, _accountID));
					MockOrderAdjustmentProviderManager adjustmentProviderManager = new MockOrderAdjustmentProviderManager(_productID, _accountID);
					adjustmentProviderManager.RegisterAdjustmentProvider(provider);
					handler.ApplyAdjustment(
																							orderContext,
																							MockOrderAdjustmentCreator.Generate(MockAdjustmentTypes.AddedItem, _productID, _accountID),
																							(x) =>
																							{
																								return orderContext.ValidOrderStatusIdsForOrderAdjustment.Contains(orderContext.Order.OrderStatusID);
																							});
				}
				catch (OrderAdjustmentProviderException ex)
				{
					Assert.IsTrue(ex.SpecificKind == OrderAdjustmentProviderException.ExceptionKind.ORDER_INVALID_FOR_ADJUSTMENT_APPLICATION, "Order validity filter for the order returned false.");
					throw ex;
				}
			}
        }
    }
}
