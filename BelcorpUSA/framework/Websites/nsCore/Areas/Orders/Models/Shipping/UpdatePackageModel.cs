
namespace nsCore.Areas.Orders.Models.Shipping
{
    public class UpdatePackageModel
    {
        public int OrderID { get; set; }
        public int OrderShipmentID { get; set; }
        public int? OrderShipmentPackageID { get; set; }
        public string TrackingNumber { get; set; }
        public string DateShipped { get; set; }
    }
}