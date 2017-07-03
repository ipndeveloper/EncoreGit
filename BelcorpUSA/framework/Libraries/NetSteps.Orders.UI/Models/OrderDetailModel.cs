using System;
using System.Collections.Generic;
using NetSteps.Encore.Core.IoC;
using NetSteps.Orders.UI.Common.Models;

namespace NetSteps.Orders.UI.Models
{
	[ContainerRegister(typeof(IOrderDetailModel), RegistrationBehaviors.Default)]
	public class OrderDetailModel : IOrderDetailModel
	{
		#region Properties

		public IList<IOrderDetailItemModel> OrderDetailItems { get; private set; }

		public string OrderCurrencySymbol { get; set; }

		public decimal OrderCVTotal { get; set; }

		public decimal OrderSubtotal { get; set; }

		public decimal OrderTaxTotal { get; set; }

		public decimal OrderShippingAndHandlingTotal { get; set; }

		public decimal OrderTotal { get; set; }

		public string OrderSummaryDescription { get; set; }

		public string OrderCVTotalText { get; set; }

		public string OrderTaxTotalText { get; set; }

		public string OrderShippingAndHandlingTotalText { get; set; }

		public string OrderSubtotalText { get; set; }

		public string OrderTotalText { get; set; }

		public string SKUHeaderText { get; set; }

		public string ProductHeaderText { get; set; }

		public string PriceHeaderText { get; set; }

		public string QuantityHeaderText { get; set; }

		public string CVHeaderText { get; set; }

		public string TotalHeaderText { get; set; }

		public string CustomersFullName { get; set; }

		public Guid CustomersGuid { get; set; }

		public bool IsHostess { get; set; }

        public bool ShowBookAPartyLink { get; set; }

		public string OnlineOrderText { get; set; }

		public string OnlineOrderStatusText { get; set; }

		public decimal OrderSubTotalSavings { get; set; }

		public string OrderSubTotalSavingsText { get; set; }

		public decimal OrderTotalSavings { get; set; }

		public string OrderTotalSavingsText { get; set; }

        public string BookAPartyText { get; set; }

		#endregion

		#region Constructor

		public OrderDetailModel()
		{
			OrderDetailItems = new List<IOrderDetailItemModel>();
		}

		#endregion
	}
}
