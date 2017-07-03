using System;

namespace NetSteps.Data.Entities
{
	[Serializable]
	public class OrderProductAmount
	{
		public int OrderID { get; set; }
		public int ProductID { get; set; }
		public int Quantity { get; set; }
		public int OrderItemID { get; set; }
	}
}