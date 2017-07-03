using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Common.Context;
using NetSteps.Data.Common.Entities;
using NetSteps.Data.Common.Services;

namespace NetSteps.Enrollment.Service.Tests.Mocks
{
	public class MockOrderService : IOrderService
	{
		public IOrder Load(int orderID)
		{
			throw new NotImplementedException();
		}

		public void AddOrderItemsToOrderBundle(IOrderContext orderContext, IOrderItem bundleItem, IEnumerable<IOrderItemQuantityModification> itemsToAdd, int bundleGroupID)
		{
			throw new NotImplementedException();
		}

		public void UpdateOrderItemQuantities(IOrderContext orderContext, IEnumerable<IOrderItemQuantityModification> itemsToUpdate)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<IOrderItem> SplitOrderItem(IOrderContext orderContext, IOrderItem targetOrderItem, IEnumerable<int> newOrderItemQuantities)
		{
			throw new NotImplementedException();
		}

		public void UpdateOrder(IOrderContext orderContext)
		{
			throw new NotImplementedException();
		}

		public int GetAccountOrderCount(int accountID)
		{
			throw new NotImplementedException();
		}

		public void AttachToParty(IOrder order, int partyID)
		{
			throw new NotImplementedException();
		}

		public void DetachFromParty(IOrder order)
		{
			order.ParentOrderID = null;
		}

		public IOrderItem AddItem(IOrder order, IOrderCustomer orderCustomer, IProduct product, int quantity, short orderItemTypeID, int? hostRewardRuleId, string parentGuid, int? dynamicKitGroupId)
		{
			throw new NotImplementedException();
		}

		public void AddOrUpdateOrderItem(IOrder order, IOrderCustomer orderCustomer, IEnumerable<Data.Common.Models.OrderItemUpdateInfo> productUpdates, bool overrideQuantity, string parentGuid, int? dynamicKitGroupID)
		{
			throw new NotImplementedException();
		}

		public short CalculateOrderShippedStatus(IOrder order)
		{
			throw new NotImplementedException();
		}

		public NetSteps.Common.Base.BasicResponse CanBeAddedToDynamicKit(IOrderCustomer orderCustomer, IProduct product, int quantity, string parentGuid, int? dynamicKitGroupId)
		{
			throw new NotImplementedException();
		}

		public bool CanBeDynamicKitUpSold(IOrder entity, IOrderCustomer customer, IProduct product, int numberOfProductsAway)
		{
			throw new NotImplementedException();
		}

		public bool CanChangeToPaidStatus(IOrder order)
		{
			throw new NotImplementedException();
		}

		public void CancelOverrides(IOrder entity)
		{
			throw new NotImplementedException();
		}

		public void ChangeToPaidStatus(IOrder order)
		{
			throw new NotImplementedException();
		}

		public string ConvertToDynamicKit(IOrder entity, IOrderCustomer customer, IProduct kitProduct)
		{
			throw new NotImplementedException();
		}

		public void GenerateAndSetNewOrderNumber(IOrder order, bool saveOrder = true)
		{
			throw new NotImplementedException();
		}

		public List<string> GetActivePromotionCodes(int accountID)
		{
			throw new NotImplementedException();
		}

		public IOrderCustomer GetHostess(IOrder order)
		{
			throw new NotImplementedException();
		}

		public List<IOrderItem> GetHostessRewardOrderItems(IOrder order)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<IProduct> GetPotentialDynamicKitUpSaleProducts(IOrder entity, IOrderCustomer customer, IEnumerable<IProduct> sortedDynamicKitProducts)
		{
			throw new NotImplementedException();
		}

		public decimal? GetRemainingHostessRewardsCredit(IOrder order)
		{
			throw new NotImplementedException();
		}

		public IProduct GetRestockingFeeProduct()
		{
			throw new NotImplementedException();
		}

		public short GetStandardOrderTypeID(IOrderCustomer orderCustomer)
		{
			throw new NotImplementedException();
		}

		public IList<int> GetValidOrderStatusIdsForOrderAdjustment()
		{
			throw new NotImplementedException();
		}

		public bool HasConsultantOnOrder(IOrder order)
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

		public bool IsDynamicKitValid(IOrderItem orderItem)
		{
			throw new NotImplementedException();
		}

		public bool IsEditable(IOrder order)
		{
			throw new NotImplementedException();
		}

		public bool IsOrderFullyReturned(IOrder order, List<IOrder> returnOrders)
		{
			throw new NotImplementedException();
		}

		public bool IsOrderFullyReturned(IOrder order)
		{
			throw new NotImplementedException();
		}

		public bool IsOrderValidForSubmission(IOrder order, NetSteps.Common.Base.BasicResponse response)
		{
			throw new NotImplementedException();
		}

		public bool IsPaidInFull(IOrder order)
		{
			throw new NotImplementedException();
		}

		public NetSteps.Common.Base.BasicResponse IsProductValid(IOrder order, IProduct product, int quantity)
		{
			throw new NotImplementedException();
		}

		public bool IsReturnable(IOrder order)
		{
			throw new NotImplementedException();
		}

		public bool IsStaticKitValid(IOrderItem orderItem)
		{
			throw new NotImplementedException();
		}

		public void OnOrderSuccessfullyCompleted(IOrder entity)
		{
			throw new NotImplementedException();
		}

		public void RemoveOrderItem(IOrderCustomer orderCustomer, IOrderItem orderItem)
		{
			throw new NotImplementedException();
		}

		public void SetCurrencyID(IOrder order)
		{
			throw new NotImplementedException();
		}

		public void SetHostess(IOrder order, IOrderCustomer hostess)
		{
			throw new NotImplementedException();
		}

		public bool ShouldDividePartyShipping()
		{
			throw new NotImplementedException();
		}

		public NetSteps.Common.Base.BasicResponse SubmitOrder(IOrderContext orderContext)
		{
			throw new NotImplementedException();
		}

		public void UndoReplacementOrderPrices(IOrder entity)
		{
			throw new NotImplementedException();
		}

		public void UpdateInventoryLevels(IOrder order, bool? returnProducts = null, bool? originalOrderCancelled = null)
		{
			throw new NotImplementedException();
		}

		public void UpdateItem(IOrder entity, IOrderCustomer orderCustomer, IOrderItem orderItem, int quantity, decimal? itemPriceOverride = null, decimal? itemCVOverride = null, bool wholesaleOverride = false)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<IOrderItem> UpdateOrderItemProperties(IOrderContext orderContext, IEnumerable<IOrderItemPropertyModification> itemsToUpdate)
		{
			throw new NotImplementedException();
		}

		IEnumerable<IOrderItem> IOrderService.UpdateOrderItemQuantities(IOrderContext orderContext, IEnumerable<IOrderItemQuantityModification> itemsToUpdate)
		{
			throw new NotImplementedException();
		}

		public NetSteps.Common.Base.BasicResponse ValidateOrderItemsByStoreFront(IOrder order)
		{
			throw new NotImplementedException();
		}
	}
}
