using System;

namespace NetSteps.Data.Entities
{
	[Serializable]
	public class OrderItemOverride
	{
		public int OrderItemID { get; set; }
		public decimal PricePerItem { get; set; }
		public decimal CommissionableValue { get; set; }
		public string OrderItemGuid { get; set; }
	}
}