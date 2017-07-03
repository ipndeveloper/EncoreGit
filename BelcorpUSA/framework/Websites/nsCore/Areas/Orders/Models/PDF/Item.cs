
namespace nsCore.Areas.Orders.Models.PDF
{
    public class Item
    {
        public string Quantity { get; set; }
        public string SKU { get; set; }
        public string ItemDescription { get; set; }
        public string UnitPrice { get; set; }
        public string Amount { get; set; }
        public string VAT { get; set; }
    }
}