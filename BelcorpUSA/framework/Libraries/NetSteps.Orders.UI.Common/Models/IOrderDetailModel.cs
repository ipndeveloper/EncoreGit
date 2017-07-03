using System;
using System.Collections.Generic;

namespace NetSteps.Orders.UI.Common.Models
{
	public interface IOrderDetailModel
	{
		IList<IOrderDetailItemModel> OrderDetailItems { get; }

		string OrderCurrencySymbol { get; set; }

		decimal OrderCVTotal { get; set; }
		decimal OrderSubtotal { get; set; }
		decimal OrderTaxTotal { get; set; }
		decimal OrderShippingAndHandlingTotal { get; set; }
		decimal OrderTotal { get; set; }
		decimal OrderSubTotalSavings { get; set; }
        decimal OrderTotalSavings { get; set; }

        string CustomersFullName { get; set; }

        Guid CustomersGuid { get; set; }

        bool IsHostess { get; set; }
        bool ShowBookAPartyLink { get; set; }

		#region Terms

		string OrderSummaryDescription { get; set; }

		string OrderCVTotalText { get; set; }
		string OrderTaxTotalText { get; set; }
		string OrderShippingAndHandlingTotalText { get; set; }
		string OrderSubtotalText { get; set; }
		string OrderTotalText { get; set; }
		string OrderSubTotalSavingsText { get; set; }
		string OrderTotalSavingsText { get; set; }

		string SKUHeaderText { get; set; }
		string ProductHeaderText { get; set; }
		string PriceHeaderText { get; set; }
		string QuantityHeaderText { get; set; }
		string CVHeaderText { get; set; }
		string TotalHeaderText { get; set; }

		string OnlineOrderText { get; set; }
		string OnlineOrderStatusText { get; set; }

        string BookAPartyText { get; set; }

		#endregion
	}
}
