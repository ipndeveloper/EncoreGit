using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Products.ProductManagement.Product
{
    public class GMP_Products_ProductManagement_Product_SectionNav_Control : Control<UnorderedList>
    {
        public GMP_Products_ProductManagement_Product_Overview_Page ClickProductOverView(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Products/Products/Overview", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Products_ProductManagement_Product_Overview_Page>(timeout, pageRequired);
        }

        public GMP_Products_ProductManagement_Product_Details_Page ClickDetails(int? timeout = null, bool pageRequired = true)
        {
            Element.GetElement<Link>(new Param("/Products/Products/Details", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Products_ProductManagement_Product_Details_Page>(timeout, pageRequired);
        }

        public GMP_Products_ProductManagement_Product_Pricing_Page ClickPricing(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Products/Products/Pricing", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Products_ProductManagement_Product_Pricing_Page>(timeout, pageRequired);
        }

        public GMP_Products_ProductManagement_Product_VariantsSetup_Page ClickVariants(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Products/Products/Variants", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Products_ProductManagement_Product_VariantsSetup_Page>(timeout, pageRequired);
        }

        public GMP_Products_ProductManagement_Product_CMS_Page ClickCMS(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Products/Products/CMS", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Products_ProductManagement_Product_CMS_Page>(timeout, pageRequired);
        }

        public GMP_Products_ProductManagement_Product_Categories_Page ClickCategories(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Products/Products/Categories", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Products_ProductManagement_Product_Categories_Page>(timeout, pageRequired);
        }

        public GMP_Products_ProductManagement_Product_Properties_Page ClickProperties(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Products/Products/Properties", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Products_ProductManagement_Product_Properties_Page>(timeout, pageRequired);
        }

        public GMP_Products_ProductManagement_Product_Resources_Page ClickResources(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Products/Products/Resources", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Products_ProductManagement_Product_Resources_Page>(timeout, pageRequired);
        }

        public GMP_Products_ProductManagement_Product_Relationships_Page ClickRelationships(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Products/Products/Relations", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Products_ProductManagement_Product_Relationships_Page>(timeout, pageRequired);
        }

        public GMP_Products_ProductManagement_Product_Availability_Page ClickAvailability(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Products/Products/Availability", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Products_ProductManagement_Product_Availability_Page>(timeout, pageRequired);
        }

        public GMP_Products_ProductManagement_Product_Inventory_Page ClickInventory(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Products/Products/Inventory", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Products_ProductManagement_Product_Inventory_Page>(timeout, pageRequired);
        }
    }
}
