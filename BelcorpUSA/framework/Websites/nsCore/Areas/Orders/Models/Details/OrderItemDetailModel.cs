using System.Collections.Generic;
using System.Collections.ObjectModel;
using NetSteps.Data.Entities;

namespace nsCore.Areas.Orders.Models.Details
{
	public class OrderItemDetailModel
	{
		public OrderItemDetailModel()
		{
			Messages = new Collection<string>();
		}

		public OrderItem OrderItem { get; set; }
		public Product Product { get; set; }
		public OrderItemReturn OrderItemReturn { get; set; }
		public OrderItemReplacement OrderItemReplacement { get; set; }
		public int CommissionablePriceTypeID { get; set; }
		public decimal AdjustedItemPrice { get; set; }
		public decimal AdjustedCommissionTotal { get; set; }
		public decimal PreAdjustedItemPrice { get; set; }
		public decimal ItemPriceTotal { get; set; }
		public decimal PreAdjustedCommissionTotal { get; set; }
		public string ReturnReason { get; set; }
		public ICollection<string> Messages { get; set; }
	}
}