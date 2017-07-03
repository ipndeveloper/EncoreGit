using NetSteps.Common.Extensions;
using NetSteps.Data.Entities;
using NetSteps.Web.Mvc.Helpers;

namespace nsCore.Areas.Orders.Models.Details
{
	public class PartialOrderInformationModel
	{
		/// <summary>
		/// Initializes a new PartialOrderInformationModel.
		/// </summary>
		/// <param name="order">The current order.</param>
		/// <param name="actingAsChildOrder"></param>
		public PartialOrderInformationModel(Order order, bool actingAsChildOrder)
		{
			Order = order;
			ActingAsChildOrder = actingAsChildOrder;
		}

		/// <summary>
		/// The current order being viewed.
		/// </summary>
		public Order Order { get; set; }

		/// <summary>
		/// This is true if the order is acting as a child order for the current parent order.
		/// </summary>
		public bool ActingAsChildOrder { get; private set; }

		/// <summary>
		/// Determines if the order is a Return Order.
		/// </summary>
		public bool IsReturnOrder
		{
			get
			{
				return Order.OrderTypeID == Constants.OrderType.ReturnOrder.ToShort();
			}
		}

		/// <summary>
		/// Determines if the order is a Replacement Order.
		/// </summary>
		public bool IsReplacementOrder
		{
			get
			{
				return Order.OrderTypeID == Constants.OrderType.ReplacementOrder.ToShort();
			}
		}

		/// <summary>
		/// Determines if the order can attach to a party.
		/// </summary>
		public bool IsOrderAttachableToParty
		{
			get
			{
				return (Order.OrderTypeID == Constants.OrderType.OnlineOrder.ToShort()) ||
						(Order.OrderTypeID == Constants.OrderType.PortalOrder.ToShort());
			}
		}

		/// <summary>
		/// Gets the created Date in text form using a short date.
		/// </summary>
		public string CreatedDate
		{
			get
			{
				return Order.DateCreated.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo);
			}
		}

		/// <summary>
		/// Gets the completed Date in text form using a short date.
		/// </summary>
		public string CompletedDate
		{
			get
			{
				return Order.CompleteDate.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo);
			}
		}

		/// <summary>
		/// Gets the commissions Date in text form using a short date.
		/// </summary>
		public string CommissionDate
		{
			get
			{
				return Order.CommissionDate.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo);
			}
		}
	}
}