using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using NetSteps.Common.Base;
using NetSteps.Data.Common.Context;
using NetSteps.Data.Common.Entities;
using NetSteps.Data.Common.Models;

namespace NetSteps.Data.Common.Services
{
	[ContractClass(typeof(Contracts.OrderServiceContracts))]
	public interface IOrderService
	{
		/// <summary>
		/// Loads the specified order by its OrderID.
		/// </summary>
		/// <param name="orderID">The order ID.</param>
		/// <returns></returns>
		IOrder Load(int orderID);

		/// <summary>
		/// Adds child order items to bundle item already in the cart.
		/// </summary>
		/// <param name="orderContext">The order context.</param>
		/// <param name="bundleItem">The bundle item.</param>
		/// <param name="itemsToAdd">The items to add.</param>
		/// <param name="bundleGroupID">The bundle group Id that receives the item.</param>
		void AddOrderItemsToOrderBundle(IOrderContext orderContext, IOrderItem bundleItem, IEnumerable<IOrderItemQuantityModification> itemsToAdd, int bundleGroupID);

		/// <summary>
		/// Updates quantities of order items within the order.
		/// </summary>
		/// <param name="orderContext">The order context.</param>
		/// <param name="itemsToUpdate">The items to update.</param>
		IEnumerable<IOrderItem> UpdateOrderItemQuantities(IOrderContext orderContext, IEnumerable<IOrderItemQuantityModification> itemsToUpdate);

		/// <summary>
		/// Updates properties of order items within the order.
		/// </summary>
		/// <param name="orderContext">The order context.</param>
		/// <param name="itemsToUpdate">The items to update.</param>
		IEnumerable<IOrderItem> UpdateOrderItemProperties(IOrderContext orderContext, IEnumerable<IOrderItemPropertyModification> itemsToUpdate);

		/// <summary>
		/// Splits order items within the order into multiple line items.
		/// </summary>
		/// <param name="orderContext">The order context.</param>
		/// <param name="targetOrderItem">The target order item.</param>
		/// <param name="newOrderItemQuantities">The new order item quantities.</param>
		/// <returns></returns>
		IEnumerable<IOrderItem> SplitOrderItem(IOrderContext orderContext, IOrderItem targetOrderItem, IEnumerable<int> newOrderItemQuantities);

		/// <summary>
		/// Updates the order, applies order adjustments and autobundling, totals the order.
		/// </summary>
		/// <param name="orderContext">The order context.</param>
		void UpdateOrder(IOrderContext orderContext);

		/// <summary>
		/// Submits the order and commits order adjustments.
		/// </summary>
		/// <param name="orderContext">The order context.</param>
		/// <returns></returns>
		BasicResponse SubmitOrder(IOrderContext orderContext);

		/// <summary>
		/// Gets the total count of orders for the specified account
		/// </summary>
		/// <param name="accountID">The accountID.</param>
		int GetAccountOrderCount(int accountID);

		/// <summary>
		/// Adds the current order to a party
		/// </summary>
		/// <param name="order">The order</param>
		/// <param name="partyID">ID of the party you'd like to use</param>
		void AttachToParty(IOrder order, int partyID);

		/// <summary>
		/// Removes the order from the party it's currently attached to. If not attached to a party, it does nothing
		/// </summary>
		/// <param name="order">The order</param>
		void DetachFromParty(IOrder order);

		/// <summary>
		/// Retrieves promotion codes that can be applied for a particular account. 
		/// </summary>
		/// <param name="accountID">The accountID to check.</param>
		/// <returns>List of valid promotion codes</returns>
		List<string> GetActivePromotionCodes(int accountID);

		/// <summary>
		/// Adds an item to an order
		/// </summary>
		/// <param name="order"></param>
		/// <param name="orderCustomer"></param>
		/// <param name="product"></param>
		/// <param name="quantity"></param>
		/// <param name="orderItemTypeID">Specifies whether this is a normal item, a booking item, host reward, etc</param>
		/// <param name="hostRewardRuleId">Only applicable when adding an item as a host reward</param>
		/// <param name="parentGuid">Used for bundles to specify what item on the order is supposed to be the parent item</param>
		/// <param name="dynamicKitGroupId"></param>
		/// <returns></returns>
		IOrderItem AddItem(IOrder order, IOrderCustomer orderCustomer, IProduct product, int quantity, short orderItemTypeID,
						   int? hostRewardRuleId, string parentGuid, int? dynamicKitGroupId);

		/// <summary>
		/// Adds or updates an order item
		/// </summary>
		/// <param name="order"></param>
		/// <param name="orderCustomer"></param>
		/// <param name="productUpdates">List of changes to apply to the order</param>
		/// <param name="overrideQuantity"></param>
		/// <param name="parentGuid"></param>
		/// <param name="dynamicKitGroupID"></param>
		void AddOrUpdateOrderItem(IOrder order, IOrderCustomer orderCustomer, IEnumerable<OrderItemUpdateInfo> productUpdates, bool overrideQuantity, string parentGuid, int? dynamicKitGroupID);

		/// <summary>
		/// Removes the given item from the given customer
		/// </summary>
		/// <param name="orderCustomer"></param>
		/// <param name="orderItem"></param>
		void RemoveOrderItem(IOrderCustomer orderCustomer, IOrderItem orderItem);

		bool TryCancel(IOrder order, out string message);

		#region Methods moved from OrderBusinessLogic

		BasicResponse IsProductValid(IOrder order, IProduct product, int quantity);
		bool IsOrderValidForSubmission(IOrder order, BasicResponse response);
		void SetCurrencyID(IOrder order);
		void GenerateAndSetNewOrderNumber(IOrder order, bool saveOrder = true);
		IProduct GetRestockingFeeProduct();
		bool IsReturnable(IOrder order);
		bool IsOrderFullyReturned(IOrder order);
		bool IsOrderFullyReturned(IOrder order, List<IOrder> returnOrders);
		bool IsAutoshipOrder(short orderTypeID);
		bool IsCancellable(IOrder order);
		bool IsEditable(IOrder order);
		bool HasConsultantOnOrder(IOrder order);
		bool CanChangeToPaidStatus(IOrder order);
		bool IsDynamicKitValid(IOrderItem orderItem);
		bool IsStaticKitValid(IOrderItem orderItem);
		BasicResponse CanBeAddedToDynamicKit(IOrderCustomer orderCustomer, IProduct product, int quantity, string parentGuid, int? dynamicKitGroupId);
		bool CanBeDynamicKitUpSold(IOrder entity, IOrderCustomer customer, IProduct product, int numberOfProductsAway);
		IEnumerable<IProduct> GetPotentialDynamicKitUpSaleProducts(IOrder entity, IOrderCustomer customer, IEnumerable<IProduct> sortedDynamicKitProducts);
		string ConvertToDynamicKit(IOrder entity, IOrderCustomer customer, IProduct kitProduct);
		short GetStandardOrderTypeID(IOrderCustomer orderCustomer);
		void UndoReplacementOrderPrices(IOrder entity);
		void CancelOverrides(IOrder entity);
		void UpdateItem(IOrder entity, IOrderCustomer orderCustomer, IOrderItem orderItem, int quantity, decimal? itemPriceOverride = null, decimal? itemCVOverride = null, bool wholesaleOverride = false);
		BasicResponse ValidateOrderItemsByStoreFront(IOrder order);
		void OnOrderSuccessfullyCompleted(IOrder entity);
		void SetHostess(IOrder order, IOrderCustomer hostess);
		IOrderCustomer GetHostess(IOrder order);
		Nullable<decimal> GetRemainingHostessRewardsCredit(IOrder order);
		void UpdateInventoryLevels(IOrder order, bool? returnProducts = null, bool? originalOrderCancelled = null);
		List<IOrderItem> GetHostessRewardOrderItems(IOrder order);
		bool IsPaidInFull(IOrder order);
		short CalculateOrderShippedStatus(IOrder order);
		IList<int> GetValidOrderStatusIdsForOrderAdjustment();
		bool ShouldDividePartyShipping();
		void ChangeToPaidStatus(IOrder order);
        void IsKitItemPricesAlive(int kitItemPricesID);
	    void AddKitItemPrices(IOrder order);

		#endregion

		//These methods SHOULD be on the interface but would require moving several interfaces out of Data.Entities 
		//and would involve circular references that will need more work to resolve.
		#region Needed Methods
		//bool IsDynamicKitGroupValid(IOrderItem kitOrderItem, DynamicKitGroup dynamicKitGroup);
		//bool CanBeAddedToDynamicKitGroup(int productId, DynamicKitGroup group);
		//void PerformOverrides(IOrder entity, List<OrderItemOverride> items, decimal taxAmount, decimal shippingAmount);
		//void SetReplacementOrderPrices(IOrder entity, List<OrderItemOverride> orderItemOverrides, decimal taxAmount, decimal shippingAmount);
		//OrderShipment GetDefaultShipment(IOrder order);
		//OrderShipment GetDefaultShipmentNoDefault(IOrder order);
		//void UpdateOrderShipmentAddress(IOrder entity, OrderShipment shipment, int addressId);
		//void UpdateOrderShipmentAddress(IOrder order, OrderShipment shipment, NetSteps.Addresses.Common.Models.IAddress address);
		///// <param name="customer">Will use the first customer on the order if left null</param>
		///// <param name="user">User is only used to validate the role-function for GMP, otherwise, we use the Distributor account type to validate the role-function</param>
		//BasicResponseItem<OrderPayment> ApplyPaymentToCustomer(IOrder entity, IOrderCustomer customer, IPayment paymentMethod, decimal amount, NetSteps.Common.Interfaces.IUser user = null);
		///// <summary>
		///// Applies the payment to the order instead of the order customer (used for party orders) - DES
		///// </summary>
		//BasicResponseItem<OrderPayment> ApplyPaymentToOrder(IOrder entity, IPayment paymentMethod, decimal amount);
		//IEnumerable<ShippingMethodWithRate> UpdateOrderShipmentAddressAndDefaultShipping(IOrder order, int addressId);
		//IEnumerable<ShippingMethodWithRate> UpdateOrderShipmentAddressAndDefaultShipping(IOrder order, NetSteps.Addresses.Common.Models.IAddress address);
		//void ValidateOrderShipmentShippingMethod(OrderShipment shipment, IOrderCustomer customer);
		//void ValidateOrderShipmentShippingMethod(OrderShipment shipment, IEnumerable<ShippingMethodWithRate> shippingMethods);
		//ShippingMethodWithRate SelectDefaultShippingMethod(IEnumerable<ShippingMethodWithRate> shippingMethods);
		//SubmitPaymentResponse SubmitPayments(IOrder order);
		//void SubmitPayment(SubmitPaymentResponse response, IOrder order, OrderPayment orderPayment, IOrderCustomer orderCustomer = null);
		//IEnumerable<ShippingMethodWithRate> GetShippingMethods(IOrder order);
		//IEnumerable<ShippingMethodWithRate> GetShippingMethods(IOrder order, OrderShipment orderShipment);
		//void SetConsultantID(IOrder order, Account account, Account siteOwner = null);
		//void UpdateCustomer(IOrder order, Account account);
		//List<DateTime> GetCompletedOrderDates(int? orderTypeID = null, int? parentOrderID = null, int? orderCustomerAccountID = null, Constants.SortDirection sortDirection = Constants.SortDirection.Ascending, int? pageSize = null);
		//IEnumerable<HostessRewardRule> GetApplicableHostessRewardRules(IOrder order, int? marketID = null);
		#endregion
	}

	namespace Contracts
	{
		[ContractClassFor(typeof(IOrderService))]
		internal abstract class OrderServiceContracts : IOrderService
		{
			IOrder IOrderService.Load(int orderID)
			{
				throw new System.NotImplementedException();
			}

			void IOrderService.AddOrderItemsToOrderBundle(IOrderContext orderContext, IOrderItem bundleItem, IEnumerable<IOrderItemQuantityModification> itemsToAdd, int bundleGroupID)
			{
				throw new System.NotImplementedException();
			}

			IEnumerable<IOrderItem> IOrderService.UpdateOrderItemQuantities(IOrderContext orderContext, IEnumerable<IOrderItemQuantityModification> itemsToUpdate)
			{
				throw new System.NotImplementedException();
			}

			IEnumerable<IOrderItem> IOrderService.SplitOrderItem(IOrderContext orderContext, IOrderItem targetOrderItem, IEnumerable<int> newOrderItemQuantities)
			{
				throw new System.NotImplementedException();
			}

			void IOrderService.UpdateOrder(IOrderContext orderContext)
			{
				throw new System.NotImplementedException();
			}

			int IOrderService.GetAccountOrderCount(int accountID)
			{
				throw new System.NotImplementedException();
			}

			void IOrderService.AttachToParty(IOrder order, int partyID)
			{
				Contract.Requires<ArgumentNullException>(order != null);
				Contract.Requires<ArgumentException>(partyID > 0);
				throw new System.NotImplementedException();
			}

			void IOrderService.DetachFromParty(IOrder order)
			{
				Contract.Requires<ArgumentNullException>(order != null);
				throw new System.NotImplementedException();
			}

			IEnumerable<IOrderItem> IOrderService.UpdateOrderItemProperties(IOrderContext orderContext, IEnumerable<IOrderItemPropertyModification> itemsToUpdate)
			{
				Contract.Requires<ArgumentNullException>(orderContext != null);
				throw new NotImplementedException();
			}

			List<string> IOrderService.GetActivePromotionCodes(int accountID)
			{
				throw new NotImplementedException();
			}

			public BasicResponse SubmitOrder(IOrderContext orderContext)
			{
				Contract.Requires<ArgumentNullException>(orderContext != null);
				throw new NotImplementedException();
			}

			BasicResponse IOrderService.SubmitOrder(IOrderContext orderContext)
			{
				throw new NotImplementedException();
			}

			IOrderItem IOrderService.AddItem(IOrder order, IOrderCustomer orderCustomer, IProduct product, int quantity, short orderItemTypeID, int? hostRewardRuleId, string parentGuid, int? dynamicKitGroupId)
			{
				Contract.Requires<ArgumentNullException>(order != null);
				Contract.Requires<ArgumentNullException>(orderCustomer != null);
				Contract.Requires<ArgumentNullException>(product != null);
				throw new NotImplementedException();
			}

			void IOrderService.AddOrUpdateOrderItem(IOrder order, IOrderCustomer orderCustomer, IEnumerable<OrderItemUpdateInfo> productUpdates, bool overrideQuantity, string parentGuid, int? dynamicKitGroupID)
			{
				Contract.Requires<ArgumentNullException>(order != null);
				Contract.Requires<ArgumentNullException>(orderCustomer != null);
				Contract.Requires<ArgumentNullException>(productUpdates != null);
				throw new NotImplementedException();
			}

			void IOrderService.RemoveOrderItem(IOrderCustomer orderCustomer, IOrderItem orderItem)
			{
				Contract.Requires<ArgumentNullException>(orderCustomer != null);
				Contract.Requires<ArgumentNullException>(orderItem != null);
				throw new NotImplementedException();
			}


			public BasicResponse IsProductValid(IOrder order, IProduct product, int quantity)
			{
				throw new NotImplementedException();
			}

			public bool IsOrderValidForSubmission(IOrder order, BasicResponse response)
			{
				throw new NotImplementedException();
			}

			public void SetCurrencyID(IOrder order)
			{
				throw new NotImplementedException();
			}

			public void GenerateAndSetNewOrderNumber(IOrder order, bool saveOrder = true)
			{
				throw new NotImplementedException();
			}

			public IProduct GetRestockingFeeProduct()
			{
				throw new NotImplementedException();
			}

			public bool IsReturnOrder(short orderTypeID)
			{
				throw new NotImplementedException();
			}

			public bool IsReturnable(IOrder order)
			{
				throw new NotImplementedException();
			}

			public bool IsOrderFullyReturned(IOrder order)
			{
				throw new NotImplementedException();
			}

			public bool IsOrderFullyReturned(IOrder order, List<IOrder> returnOrders)
			{
				throw new NotImplementedException();
			}

			public bool IsAutoshipOrder(short orderTypeID)
			{
				throw new NotImplementedException();
			}

			public bool IsCancellable(IOrder order)
			{
				throw new NotImplementedException();
			}

			public bool IsEditable(IOrder order)
			{
				throw new NotImplementedException();
			}

			public bool HasConsultantOnOrder(IOrder order)
			{
				throw new NotImplementedException();
			}

			public bool CanChangeToPaidStatus(IOrder order)
			{
				Contract.Requires<ArgumentNullException>(order != null);

				throw new NotImplementedException();
			}

			public bool IsDynamicKitValid(IOrderItem orderItem)
			{
				throw new NotImplementedException();
			}

			public bool IsStaticKitValid(IOrderItem orderItem)
			{
				throw new NotImplementedException();
			}

			public BasicResponse CanBeAddedToDynamicKit(IOrderCustomer orderCustomer, IProduct product, int quantity, string parentGuid, int? dynamicKitGroupId)
			{
				throw new NotImplementedException();
			}

			public bool CanBeDynamicKitUpSold(IOrder entity, IOrderCustomer customer, IProduct product, int numberOfProductsAway)
			{
				throw new NotImplementedException();
			}

			public IEnumerable<IProduct> GetPotentialDynamicKitUpSaleProducts(IOrder entity, IOrderCustomer customer, IEnumerable<IProduct> sortedDynamicKitProducts)
			{
				throw new NotImplementedException();
			}

			public string ConvertToDynamicKit(IOrder entity, IOrderCustomer customer, IProduct kitProduct)
			{
				throw new NotImplementedException();
			}

			public short GetStandardOrderTypeID(IOrderCustomer orderCustomer)
			{
				throw new NotImplementedException();
			}

			public void UndoReplacementOrderPrices(IOrder entity)
			{
				throw new NotImplementedException();
			}

			public void CancelOverrides(IOrder entity)
			{
				throw new NotImplementedException();
			}

			public void UpdateItem(IOrder entity, IOrderCustomer orderCustomer, IOrderItem orderItem, int quantity, decimal? itemPriceOverride = null, decimal? itemCVOverride = null, bool wholesaleOverride = false)
			{
				throw new NotImplementedException();
			}

			public BasicResponse ValidateOrderItemsByStoreFront(IOrder order)
			{
				throw new NotImplementedException();
			}

			public void OnOrderSuccessfullyCompleted(IOrder entity)
			{
				throw new NotImplementedException();
			}

			public void SetHostess(IOrder order, IOrderCustomer hostess)
			{
				throw new NotImplementedException();
			}

			public IOrderCustomer GetHostess(IOrder order)
			{
				throw new NotImplementedException();
			}

			public decimal? GetRemainingHostessRewardsCredit(IOrder order)
			{
				throw new NotImplementedException();
			}

			public IEnumerable<IOrder> LoadOrderWithShipmentAndPaymentDetails(IEnumerable<string> orderNumbers)
			{
				throw new NotImplementedException();
			}

			public void UpdateInventoryLevels(IOrder order, bool? returnProducts = null, bool? originalOrderCancelled = null)
			{
				throw new NotImplementedException();
			}

			public void ChangeToPaidStatus(IOrder order)
			{
				throw new NotImplementedException();
			}

		    public void IsKitItemPricesAlive(int kitItemPricesID)
		    {
		        throw new NotImplementedException();
		    }

		    public void AddKitItemPrices(IOrder order)
		    {
		        throw new NotImplementedException();
		    }

			public List<IOrderItem> GetHostessRewardOrderItems(IOrder order)
			{
				throw new NotImplementedException();
			}

			public bool IsPaidInFull(IOrder order)
			{
				throw new NotImplementedException();
			}

			public short CalculateOrderShippedStatus(IOrder order)
			{
				throw new NotImplementedException();
			}

			public IList<int> GetValidOrderStatusIdsForOrderAdjustment()
			{
				throw new NotImplementedException();
			}

			public bool ShouldDividePartyShipping()
			{
				throw new NotImplementedException();
			}

			public bool TryCancel(IOrder order, out string message)
			{
				Contract.Requires<ArgumentNullException>(order != null);

				throw new NotImplementedException();
			}
		}
	}
}
