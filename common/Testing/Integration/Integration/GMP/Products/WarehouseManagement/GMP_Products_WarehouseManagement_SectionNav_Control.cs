using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Products.WarehouseManagement
{
    public class GMP_Products_WarehouseManagement_SectionNav_Control : Control<UnorderedList>
    {
        public GMP_Products_WarehouseManagement_Inventory_Page ClickWarehouseInventory(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Products/Warehouses", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Products_WarehouseManagement_Inventory_Page>(timeout, pageRequired);
        }

        public GMP_Products_WarehouseManagement_BrowseShippingRegions_Page ClickShippingRegions(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Products/ShippingRegions", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Products_WarehouseManagement_BrowseShippingRegions_Page>(timeout, pageRequired);
        }

    }
}
