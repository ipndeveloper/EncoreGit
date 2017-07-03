using System.Collections.Generic;
using System.Diagnostics.Contracts;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;

namespace nsCore.Areas.Orders.Models.Details
{
	public class PartialOrderCustomerDetailsModel
	{
		#region -- Constructor --

		public PartialOrderCustomerDetailsModel(Order order, OrderCustomer orderCustomer)
			: this(order, orderCustomer, false)
		{
		}

		public PartialOrderCustomerDetailsModel(Order order, OrderCustomer orderCustomer, bool actingAsChildOrder)
		{
			Order = order;
			OrderCustomer = orderCustomer;
			SetupOrderCustomerDetailsProperties();
			ActingAsChildOrder = actingAsChildOrder;
		}

		#endregion

		#region -- Properties --

		public Order Order { get; set; }
		public OrderCustomer OrderCustomer { get; set; }
		public IEnumerable<OrderItemDetailModel> OrderItemDetails { get; set; }
		public IEnumerable<OrderShipment> OrderShipments
		{
			get
			{
				return IsPartyOrder ? OrderCustomer.OrderShipments : Order.OrderShipments;
			}
		}
		public PartialOrderCustomerTotalsModel OrderCustomerTotals { get; set; }
		public bool ActingAsChildOrder { get; set; }

		/// <summary>
		/// Checks the Order if available to determine if the orderTypeID == PartyOrder.
		/// If there is no order object set this will return false by default.
		/// </summary>
		public bool IsPartyOrder { get; set; }

		/// <summary>
		/// Checks whether the order is an online order attached to a party
		/// </summary>
		public bool IsPartyAttachedOrder { get; set; }

		/// <summary>
		/// Checks the Order if available to determine if the orderTypeID == ReturnOrder.
		/// If there is no order object set this will return false by default.
		/// </summary>
		public bool IsReturnOrder { get; set; }

		/// <summary>
		/// The current currency on the order.
		/// </summary>
		public Currency Currency { get; set; }

		#endregion

		#region -- Methods --

		private void SetupOrderCustomerDetailsProperties()
		{
			Contract.Requires(Order != null);
			Contract.Requires(OrderCustomer != null);


			Currency = SmallCollectionCache.Instance.Currencies.GetById(Order.CurrencyID);
			IsPartyOrder = Order.OrderTypeID == (int)Constants.OrderType.PartyOrder;
			IsPartyAttachedOrder = Order.OrderTypeID == (int)Constants.OrderType.OnlineOrder && Order.ParentOrderID != null;
			IsReturnOrder = Order.OrderTypeID == (int)Constants.OrderType.ReturnOrder;
			OrderCustomerTotals = new PartialOrderCustomerTotalsModel(Order, OrderCustomer);

		}

		#endregion
	}
}