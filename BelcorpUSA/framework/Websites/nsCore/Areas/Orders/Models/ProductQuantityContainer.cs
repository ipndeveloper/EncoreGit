
namespace nsCore.Areas.Orders.Models
{
	public class ProductQuantityContainer
    {
        public string OrderItemGuid { get; set; }
		public int ProductID { get; set; }
		public int Quantity { get; set; }
        public bool HasQuantityChange { get; set; }
	}
}