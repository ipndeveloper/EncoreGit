namespace nsDistributor.Models.Cart
{
	public class ProductQuantityContainer
	{
		public int ProductID { get; set; }
		public int Quantity { get; set; }
        public string CustomValue { get; set; }
        public string OrderItemGuid { get; set; }
        public bool HasQuantityChange { get; set; }
	}
}