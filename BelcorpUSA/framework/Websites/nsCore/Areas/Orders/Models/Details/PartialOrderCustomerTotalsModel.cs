using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;

namespace nsCore.Areas.Orders.Models.Details
{
	public class PartialOrderCustomerTotalsModel
	{
		#region -- Constructor --

		public PartialOrderCustomerTotalsModel(Order order, OrderCustomer orderCustomer)
		{
			Order = order;
			OrderCustomer = orderCustomer;
			SetupTotalsProperties();
		}

		#endregion

		#region -- Properties --

		public Order Order { get; set; }
		public OrderCustomer OrderCustomer { get; set; }
		public IEnumerable<OrderShipment> OrderShipments { get; set; }

		public bool IsPartyOrder { get; set; }

		public bool IsReturnOrder { get; set; }

		public Currency Currency { get; set; }

		public decimal ShippingTotal { get; set; }

		public decimal Subtotal { get; set; }

		public decimal GrandTotal { get; set; }

		public decimal CommissionableTotal { get; set; }

		public decimal TaxAmountTotal { get; set; }

		#endregion

		#region -- Methods --

		/// <summary>
		/// One stop shop to set the values on the totals properties.
		/// I'm going this route instead of getters because getters are icky.
		/// </summary>
		private void SetupTotalsProperties()
		{
			Contract.Requires(Order != null);
			Contract.Requires(OrderCustomer != null);

			OrderShipments = GetOrderShipments();

			Currency = SmallCollectionCache.Instance.Currencies.GetById(Order.CurrencyID);
			IsPartyOrder = Order.OrderTypeID == (int)Constants.OrderType.PartyOrder;
			IsReturnOrder = Order.OrderTypeID == (int)Constants.OrderType.ReturnOrder;

			Subtotal = GetSubtotal();
			CommissionableTotal = GetCommissionableTotal();
			TaxAmountTotal = GetTaxAmountTotal();
			ShippingTotal = GetShippingTotal();
			GrandTotal = GetGrandTotal();
		}

		/// <summary>
		/// Makes a determination to return the Order's Subtotal or OrderCustomer's Subtotal.
		/// The determining factor is whether or not this is a party.
		/// On orders that are not a party there should only be 1 OrderCustomer.
		/// This means we want the Order's total.
		/// </summary>
		/// <returns>Subtotal for the OrderCustomer</returns>
		public virtual decimal GetSubtotal()
		{
			Contract.Requires(Order != null);
			Contract.Requires(OrderCustomer != null);

			return IsPartyOrder ? OrderCustomer.Subtotal.ToDecimal() : Order.Subtotal.ToDecimal();
		}

		/// <summary>
		/// Makes a determination to return the Order's Commissionable Total or OrderCustomer's Commissionable Total.
		/// The determining factor is whether or not this is a party.
		/// On orders that are not a party there should only be 1 OrderCustomer.
		/// This means we want the Order's total.
		/// </summary>
		/// <returns>Commissionable for the OrderCustomer</returns>
		public virtual decimal GetCommissionableTotal()
		{
			Contract.Requires(Order != null);
			Contract.Requires(OrderCustomer != null);

			return IsPartyOrder ? OrderCustomer.CommissionableTotal.ToDecimal() : Order.CommissionableTotal.ToDecimal();
		}

		/// <summary>
		/// Makes a determination to return the Order's TaxAmountTotal or OrderCustomer's TaxAmountTotal.
		/// The determining factor is whether or not this is a party.
		/// On orders that are not a party there should only be 1 OrderCustomer.
		/// This means we want the Order's total.
		/// </summary>
		/// <returns>Tax Amount Total for the OrderCustomer.  If there is an override on the Order.TaxAmountTotal this will be returned instead.</returns>
		public virtual decimal GetTaxAmountTotal()
		{
			Contract.Requires(Order != null);
			Contract.Requires(OrderCustomer != null);

			return (Order.TaxAmountTotalOverride != null) 
						? Order.TaxAmountTotalOverride.ToDecimal() 
						: (IsPartyOrder ? OrderCustomer.TaxAmountTotal.ToDecimal() : Order.TaxAmountTotal.ToDecimal());
		}

		/// <summary>
		/// Makes a determination to return the Order's Shipping Total or OrderCustomer's Shipping Total.
		/// The determining factor is whether or not this is a party.
		/// On orders that are not a party there should only be 1 OrderCustomer.
		/// This means we want the Order's total.
		/// </summary>
		/// <returns>Shipping Total for the OrderCustomer</returns>
		public virtual decimal GetShippingTotal()
		{
			Contract.Requires(Order != null);
			Contract.Requires(OrderCustomer != null);

			return Order.ShippingTotalOverride != null
						  ? Order.ShippingTotalOverride.ToDecimal()
						  : (IsPartyOrder && OrderShipments.Count() > 0)
							  ? (OrderCustomer.ShippingTotal.ToDecimal() + OrderCustomer.HandlingTotal.ToDecimal())
							  : (Order.ShippingTotal.ToDecimal() + Order.HandlingTotal.ToDecimal());
		}

		/// <summary>
		/// Makes a determination to return the Order's Grand Total or OrderCustomer's Grand Total.
		/// The determining factor is whether or not this is a party.
		/// On orders that are not a party there should only be 1 OrderCustomer.
		/// This means we want the Order's total.
		/// </summary>
		/// <returns>Grand Total for the OrderCustomer</returns>
		public virtual decimal GetGrandTotal()
		{
			Contract.Requires(Order != null);
			Contract.Requires(OrderCustomer != null);

			return IsPartyOrder ? OrderCustomer.Total.ToDecimal() : Order.GrandTotal.ToDecimal();
		}


		/// <summary>
		/// Makes a determination to return the Order's OrderShipments or OrderCustomer's OrderShipments.
		/// The determining factor is whether or not this is a party.
		/// On orders that are not a party there should only be 1 OrderCustomer.
		/// This means we want the Order's Shipments.
		/// </summary>
		/// <returns>Order Shipments for the OrderCustomer</returns>
		public virtual IEnumerable<OrderShipment> GetOrderShipments()
		{
			Contract.Requires(Order != null);
			Contract.Requires(OrderCustomer != null);

			return IsPartyOrder ? OrderCustomer.OrderShipments : Order.OrderShipments;
		}

		#endregion
	}
}