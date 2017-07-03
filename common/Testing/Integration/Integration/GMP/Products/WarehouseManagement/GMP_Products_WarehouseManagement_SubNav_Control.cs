using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Products.WarehouseManagement
{
    public class GMP_Products_WarehouseManagement_SubNav_Control : Control<Div>
    {
        public int Index
        { get; set; }

        public GMP_Products_WarehouseManagement_Inventory_Page ClickWarehouseInventory(int? timeout = null, bool pageRequired = true)
        {
            Element.CustomRunScript(Util.strSlideDown, null, Index);
            timeout = Element.GetElement<Link>(new Param("/Products/Warehouses", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Products_WarehouseManagement_Inventory_Page>(timeout, pageRequired);
        }

        public GMP_Products_WarehouseManagement_BrowseShippingRegions_Page ClickShippingRegions(int? timeout = null, bool pageRequired = true)
        {
            Element.CustomRunScript(Util.strSlideDown, null, Index);
            timeout = Element.GetElement<Link>(new Param("/Products/ShippingRegions", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Products_WarehouseManagement_BrowseShippingRegions_Page>(timeout, pageRequired);
        }
    }
}
