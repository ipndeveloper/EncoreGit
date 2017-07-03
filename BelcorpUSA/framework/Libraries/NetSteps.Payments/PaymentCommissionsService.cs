using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Interfaces;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Payments
{
	/// <summary>
	/// Default implementation of IPaymentCommissions
	/// </summary>
	[ContainerRegister(typeof(IPaymentCommissions), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
	public class PaymentCommissionsService : IPaymentCommissions
	{
		#region Properties
		/// <summary>
		/// Gets the inventory repository.
		/// </summary>
		protected InventoryBaseRepository Inventory
		{
			get
			{
				return Create.New<InventoryBaseRepository>();
			}
		}
		#endregion

		#region IPaymentCommissions
		/// <summary>
		/// Calculates new commissionable value based upon nonCommissionable payments on the order
		/// </summary>
		/// <param name="order">Order to modify</param>
		public void CalculateCommissions(Order order)
		{
			// We do not change override orders,
			// but pull the Commissionable totals from the OrderCustomers' OrderItems where the override values are set.
			if (order.OrderTypeID != (int)Constants.OrderType.OverrideOrder)
			{
				SetAllOrderItemsBackToOriginalCommission(order);

				decimal nonCommissionableAmount = GetAllNonCommissionableOrderPayments(order).Sum(op => op.Amount);

				int itemQuantity = GetAllAdjustableItemsInOrder(order).Sum(oi => oi.Quantity);

				decimal subtotal = order.OrderCustomers.Sum(oc => oc.Subtotal ?? 0);
						//.SelectMany(oc => oc.OrderItems).Sum(oi => oi.AdjustedPrice ?? 0);//order.Subtotal ?? order.OrderCustomers.Sum(oc => oc.Subtotal).Value;

				if (nonCommissionableAmount > 0 && itemQuantity > 0 && subtotal > 0)
				{
					PerformProportionalCVCalculations(order, nonCommissionableAmount, itemQuantity, subtotal);
				}
			}

			//SetCustomerCommissionableTotal(order.OrderCustomers);
			//SetOrderCommissionableTotal(order);
		}
		#endregion

		#region Commission Modifications
		/// <summary>
		/// Modifies the commissionable value on an order based upon how much nonCommissionable funds have been used
		/// The new commissionable value is proportionate to the remaining subtotal after nonCommissionable funds have been applied
		/// </summary>
		/// <param name="order">Order to modify</param>
		/// <param name="nonCommissionable">Amount of nonCommissionable funds. Must be greater than 0</param>
		/// <param name="itemQuantity">Total number of items on the order. Must be greater than 0</param>
		/// <param name="subtotal">Order subtotal. Must be greater than 0</param>
		protected void PerformProportionalCVCalculations(Order order, decimal nonCommissionable, int itemQuantity, decimal subtotal)
		{
			Contract.Requires<ArgumentException>(nonCommissionable > 0);
			Contract.Requires<ArgumentException>(itemQuantity > 0);
			Contract.Requires<ArgumentException>(subtotal > 0);

			decimal sumOfCommissions = Math.Abs(
				GetAllAdjustableItemsInOrder(order)
					.Sum(oi => oi.CommissionableTotal ?? 0));

			if (sumOfCommissions == 0)
			{
				return;
			}

			// Finds the commissionable total that needs to be removed from the order items. This is figured proportionally
			//      nonCommisionable    commissions to remove
			//      ----------     =    ----------
			//      subtotal            commissionable value
			decimal bonusValueToRemove = nonCommissionable / subtotal * sumOfCommissions;
			if (bonusValueToRemove > sumOfCommissions)
			{
				bonusValueToRemove = sumOfCommissions;
			}
			
			RemoveBonusValueFromOrder(order, bonusValueToRemove, itemQuantity);
		}


		/// <summary>
		/// Removes commissionable value based on the bonusValueToRemove
		/// </summary>
		/// <param name="order">Order to remove commissionable value from</param>
		/// <param name="bonusValueToRemove">The amount of value that should be removed</param>
		/// <param name="itemQuantity">Number of items</param>
		protected void RemoveBonusValueFromOrder(Order order, decimal bonusValueToRemove, int itemQuantity)
		{
			Contract.Requires<ArgumentException>(itemQuantity > 0);
			Contract.Requires<ArgumentException>(bonusValueToRemove > 0);

			List<OrderItem> allItems = GetAllAdjustableItemsInOrder(order).ToList();

			// This is a collection of items whose Commissionable value is less than how much we need to remove per item. Thus, they will ultimately have a commissionable value of 0
			IEnumerable<OrderItem> itemsBelowCommissionsToRemove = GetItemsWithCommissionalValueBelowCommissionsToRemove(bonusValueToRemove, itemQuantity, allItems);

			// While we still have items below the removePerItem point which we haven't dealt with...
			while (itemsBelowCommissionsToRemove.Any())
			{
				foreach (OrderItem t in itemsBelowCommissionsToRemove)
				{
					// Remove the item from our totals because we are dealing with it manually
					itemQuantity -= t.Quantity;
					bonusValueToRemove -= t.CommissionableTotal ?? 0;
					t.CommissionableTotal = 0;
					allItems.Remove(t);
				}

				// Recalculate the itemsBelowCommissionsToRemove due to changed itemQuantity and bonusValueToRemove
				itemsBelowCommissionsToRemove = GetItemsWithCommissionalValueBelowCommissionsToRemove(bonusValueToRemove, itemQuantity, allItems);
			}

			// Once we leave the loop, all that is left are items which have a higher commissionableTotal than how much we need to remove per item. So remove per item.
			decimal removePerItem = bonusValueToRemove / itemQuantity;
			foreach (OrderItem t in allItems)
			{
				t.CommissionableTotal = ((t.CommissionableTotal ?? 0) - (removePerItem * t.Quantity)).GetRoundedNumber();
			}
		}


		/// <summary>
		/// Returns all items that are below the average commission (bonusValueToRemove / itemQuantity)
		/// </summary>
		/// <param name="bonusValueToRemove">Amount of commissionable value left to remove from the order</param>
		/// <param name="itemQuantity">Total number of items that are not dealt with</param>
		/// <param name="unprocessedItems">All items that are not dealt with on the order</param>
		/// <returns>List of items that will need to be manually dealt with</returns>
		protected IEnumerable<OrderItem> GetItemsWithCommissionalValueBelowCommissionsToRemove(decimal bonusValueToRemove, int itemQuantity, IEnumerable<OrderItem> unprocessedItems)
		{
			decimal removePerItem = bonusValueToRemove / itemQuantity;
			return unprocessedItems.Where(o => (Math.Abs(o.CommissionableTotal ?? 0) / o.Quantity) < removePerItem).ToList();
		}
		#endregion

		#region Helper Methods
		/// <summary>
		/// Gets a unique flat list of OrderPayments
		/// </summary>
		/// <param name="order">Retrieves all order payments on the OrderCustomer and Order level</param>
		/// <returns>Returns a list of order payments</returns>
		protected List<OrderPayment> GetAllOrderPayments(Order order)
		{
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
			// Filtering out children order items in bundles.
			// Filtering out hostess rewards items, which at this point in execution have a commissionable value but will later be set to $0.
			return GetAllItemsInOrder(order)
				.Where(oi => oi.ParentOrderItem == null && oi.HostessRewardRuleID == null);
		}


		#region Commission helper methods
		/// <summary>
		/// Examine the original items and reset commissionableTotals on order
		/// </summary>
		/// <param name="order">Order to set child products' commissionable value back to original commissionable value</param>
		protected void SetAllOrderItemsBackToOriginalCommission(Order order)
		{
			// Iterate through each OrderItem
			foreach (OrderItem item in GetAllItemsInOrder(order))
			{
				// Load original product
				Product tempProduct = Inventory.GetProduct(item.ProductID ?? 0);

				// Calculate old commissionablePrice and set commissionableTotals
				decimal commissionablePrice = tempProduct.Prices.GetPriceByAccountTypeAndRelationship(item.OrderCustomer.AccountTypeID, Data.Entities.Generated.ConstantsGenerated.PriceRelationshipType.Commissions, order.CurrencyID) ?? 0;
				item.CommissionableTotal = commissionablePrice * item.Quantity;
			}
		}
		#endregion
		#endregion
	}
}