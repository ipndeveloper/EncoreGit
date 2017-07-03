
namespace nsCore.Models
{
    public class ProductReturn
    {
        public int ProductID { get; set; }
        public int OrderItemID { get; set; }
        public int ReturnReasonID { get; set; }
        public int QuantityReturned { get; set; }
        public bool IsRestockable { get; set; }
        public bool HasBeenReceived { get; set; }
        public int? ParentOrderItemID { get; set; }
        public int? DynamicKitGroupID { get; set; }
        public decimal ItemPrice { get; set; }
        public decimal ReturnItemPrice { get; set; }
        public decimal ReturnItemCV { get; set; }
        public int OrderCustomerID { get; set; }
    }
}