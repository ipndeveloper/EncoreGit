using System;

namespace NetSteps.Data.Common.Models
{
	[Serializable]
	public class OrderItemUpdateInfo
	{
		public int ProductID { get; set; }
		public int Quantity { get; set; }
		public int? DynamicKitGroupID { get; set; }
		public bool OverrideQuantity { get; set; }
		public string ParentGuid { get; set; }
		public object Custom { get; set; }
		public decimal? ItemPriceActual { get; set; }
	}
}
