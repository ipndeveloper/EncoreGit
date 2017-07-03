using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using NetSteps.Data.Common.Context;
using NetSteps.Data.Common.Entities;
using NetSteps.Data.Common.Services;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Encore.Core.IoC;
using NetSteps.Extensibility.Core;
using NetSteps.NonCommissionablePaymentTypeProvider.Common;
using NetSteps.OrderAdjustments.Common.Model;

namespace NetSteps.NonCommissionablePaymentTypeProvider.Service
{
	public class NonCommissionablePaymentOrderAdjustmentProvider : INonCommissionablePaymentOrderAdjustmentProvider
	{
		public const string AdjustmentDescription = "Non-Commissionable Payment Adjustment";

		public const string GenericVolumeAdjustmentDescription = "Non-Commissionable Payment {0} Adjustment";

		public string GetProviderKey()
		{
			return NonCommissionablePaymentOrderAdjustmentProviderInfo.OrderAdjustmentProviderKey;
		}

		public IDataObjectExtension CreateOrderAdjustmentDataObjectExtension(IOrderAdjustmentProfile profile)
		{
			return null;
		}

		public void DeleteDataObjectExtension(IExtensibleDataObject x)
		{
		}

		public IDataObjectExtension GetDataObjectExtension(IExtensibleDataObject dataObject)
		{
			return null;
		}

		public IDataObjectExtension SaveDataObjectExtension(IExtensibleDataObject dataObject)
		{
			return null;
		}

		public void UpdateDataObjectExtension(IExtensibleDataObject dataObject)
		{
			// Do nothing
		}

		public IEnumerable<IOrderAdjustmentProfile> GetApplicableAdjustments(IOrderContext order)
		{
			var orderAdjustments = new List<IOrderAdjustmentProfile>();
			var profile = CreateOrderAdjustmentProfile(order);
			if (profile != null) orderAdjustments.Add(profile);
			return orderAdjustments;
		}

		public IOrderAdjustmentProfile GetOrderAdjustmentProfile(IOrderContext orderContext, int adjustmentID)
		{
			var adjustmentProfile = Create.New<IOrderAdjustmentProfile>();
			adjustmentProfile.ExtensionProviderKey = GetProviderKey();
			adjustmentProfile.Description = AdjustmentDescription;
			return adjustmentProfile;
		}

		public bool IsInstanceOfProfile(IOrderAdjustment adjustment, IOrderAdjustmentProfile adjustmentProfile)
		{
			return true;
		}

		public void NotifyOfRemoval(IOrderContext orderContext, IOrderAdjustment adjustment)
		{
			// Do nothing
		}

		public void CommitAdjustment(IOrderAdjustment adjustment, IOrderContext orderContext)
		{
			// Do nothing
		}

		/// <summary>
		/// Creates the order adjustment profile object
		/// </summary>
		/// <param name="order"></param>
		/// <returns>An Order Adjustment Profile</returns>
		protected virtual INonCommissionablePaymentOrderAdjustmentProfile CreateOrderAdjustmentProfile(IOrderContext order)
		{
			Contract.Requires(order != null);

			Order oldStyleOrder = order.Order.AsOrder();
			if (oldStyleOrder == null) throw new NotSupportedException("The IOrderContext currently needs to contain a NetSteps.Data.Entities.Order.");

			decimal adjustmentDiscountMultiplier = CalculateCommissionsDiscountMultiplier(order);

			if (adjustmentDiscountMultiplier > 0.0M)
			{
				var adjustmentProfile = Create.New<INonCommissionablePaymentOrderAdjustmentProfile>();
				adjustmentProfile.ExtensionProviderKey = GetProviderKey();
				adjustmentProfile.Description = AdjustmentDescription;

				var adjustableOrderItems = GetAllAdjustableItemsInOrder(oldStyleOrder);

				foreach (var orderItem in adjustableOrderItems)
				{
					adjustmentProfile.OrderLineModificationTargets.Add(CreateOrderItemTargetAdjustment(orderItem, adjustmentDiscountMultiplier));

					if (!adjustmentProfile.AffectedAccountIDs.Contains(orderItem.OrderCustomer.AccountID))
					{
						adjustmentProfile.AffectedAccountIDs.Add(orderItem.OrderCustomer.AccountID);
					}
				}

				return adjustmentProfile;
			}
			return null;
		}

		/// <summary>
		/// Creates the Adjustment for the OrderItem
		/// </summary>
		/// <param name="orderItem">The OrderItem to adjust</param>
		/// <param name="adjustmentMultiplier">The decimal fraction to use as the commissions adjustment, as a discount.</param>
		/// <returns>An OrderItemTargetAdjustment for the OrderItem</returns>
		protected virtual IOrderAdjustmentProfileOrderItemTarget CreateOrderItemTargetAdjustment(OrderItem orderItem, decimal adjustmentMultiplier)
		{
			Contract.Requires(orderItem != null);
			Contract.Requires(adjustmentMultiplier >= 0.0M);
			Contract.Requires(adjustmentMultiplier <= 1.0M);

			var priceTypeService = Create.New<IPriceTypeService>();

			var adjustmentTarget = Create.New<IOrderAdjustmentProfileOrderItemTarget>();
			adjustmentTarget.OrderCustomerAccountID = orderItem.OrderCustomer.AccountID;
			adjustmentTarget.ProductID = orderItem.ProductID ?? 0;

			foreach (var volumePriceType in priceTypeService.GetVolumePriceTypes())
			{
				var volumeAdjustment = Create.New<IOrderAdjustmentProfileOrderLineModification>();
				volumeAdjustment.ModificationOperationID = (int)OrderAdjustmentOrderLineOperationKind.Multiplier;
				volumeAdjustment.ModificationValue = adjustmentMultiplier;
				volumeAdjustment.Property = volumePriceType.Name;
				volumeAdjustment.Description = String.Format(GenericVolumeAdjustmentDescription, volumePriceType.Name);

				adjustmentTarget.Modifications.Add(volumeAdjustment);
			}

			return adjustmentTarget;
		}

		#region Helper Methods

		/// <summary>
		/// Calculates the CV adjustment multiplier
		/// </summary>
		/// <param name="order">The IOrderContext</param>
		/// <returns>A decimal multiplier to use to calculate the new CV</returns>
		protected decimal CalculateCommissionsDiscountMultiplier(IOrderContext order)
		{
			Contract.Requires(order != null);
			Contract.Ensures(Contract.Result<decimal>() >= 0.0M);
			Contract.Ensures(Contract.Result<decimal>() <= 1.0M);

			var oldStyleOrder = order.Order.AsOrder();

			var priceTypeService = Create.New<IPriceTypeService>();
			var priceType = priceTypeService.GetPriceType(oldStyleOrder.OrderCustomers[0].AccountTypeID, (int)Constants.PriceRelationshipType.Products, 1);

			decimal nonCommissionableAmount = GetAllNonCommissionableOrderPayments(oldStyleOrder).Sum(op => op.Amount);

			//decimal subtotal = oldStyleOrder.Subtotal ?? 0.0M;
			decimal subtotal = oldStyleOrder.OrderCustomers.Sum(oc => oc.ParentOrderItems.Sum(oi => oi.GetAdjustedPrice(priceType.PriceTypeID) * oi.Quantity));

			return subtotal > 0
				? Math.Min(nonCommissionableAmount / subtotal, 1.0M)
				: 0.0M;
		}

		/// <summary>
		/// Gets a unique flat list of OrderPayments
		/// </summary>
		/// <param name="order">Retrieves all order payments on the OrderCustomer and Order level</param>
		/// <returns>Returns a list of all order payments</returns>
		protected List<OrderPayment> GetAllOrderPayments(Order order)
		{
			Contract.Requires(order != null);
			Contract.Ensures(Contract.Result<List<OrderPayment>>() != null);

			List<OrderPayment> orderPayments = order.OrderPayments.ToList();
			IEnumerable<IEnumerable<OrderPayment>> orderCustomerPayments = order.OrderCustomers.Where(oc => oc.OrderPayments.Any()).Select(oc => oc.OrderPayments);

			foreach (IEnumerable<OrderPayment> payments in orderCustomerPayments)
			{
				orderPayments.AddRange(payments.Where(p => !orderPayments.Contains(p)));
			}

			return orderPayments;
		}

		/// <summary>
		/// Gets all non-commissionable payments
		/// </summary>
		/// <param name="order">The Order in consideration</param>
		/// <returns>A list of all non-commissionable payments</returns>
		/// <remarks>Returns only valid non-commissionable payments, i.e. those completed or pending.</remarks>
		protected List<OrderPayment> GetAllNonCommissionableOrderPayments(Order order)
		{
			Contract.Requires(order != null);
			Contract.Ensures(Contract.Result<List<OrderPayment>>() != null);

			return
				GetAllOrderPayments(order).Where(
					op =>
					!op.IsCommissionable
					&&
					(op.OrderPaymentStatusID == (short)Constants.OrderPaymentStatus.Completed
					 || op.OrderPaymentStatusID == (short)Constants.OrderPaymentStatus.Pending)).ToList();
		}

		/// <summary>
		/// Gets a flat list of OrderItems regardless of which customer they are under
		/// </summary>
		/// <param name="order">Order from which to retrieve orderItems</param>
		/// <returns>All items in the order</returns>
		protected IEnumerable<OrderItem> GetAllItemsInOrder(Order order)
		{
			Contract.Requires(order != null);
			Contract.Ensures(Contract.Result<IEnumerable<OrderItem>>() != null);

			return order.OrderCustomers.SelectMany(oc => oc.OrderItems);
		}

		/// <summary>
		/// Gets a flat list of OrderItems regardless of which customer they are under
		/// </summary>
		/// <param name="order">Order from which to retrieve orderItems</param>
		/// <returns>All adjustable items in the order</returns>
		/// <remarks>Filters out children of bundles and hostess rewards items</remarks>
		protected IEnumerable<OrderItem> GetAllAdjustableItemsInOrder(Order order)
		{
			Contract.Requires(order != null);
			Contract.Ensures(Contract.Result<IEnumerable<OrderItem>>() != null);

			// Filtering out children order items in bundles.
			// Filtering out hostess rewards items, which at this point in execution have a commissionable value but will later be set to $0.
			var orderItems = GetAllItemsInOrder(order).Where(oi => oi.ParentOrderItem == null && oi.HostessRewardRuleID == null);

			var returnValue = new Dictionary<int, OrderItem>();
			foreach (var orderItem in orderItems)
			{
				if(!orderItem.ProductID.HasValue)
				{
					continue;
				}

				if(!returnValue.ContainsKey(orderItem.ProductID.Value))
				{
					returnValue.Add(orderItem.ProductID.Value, orderItem);
				}
			}

			return returnValue.Values;
		}

		#endregion
	}
}