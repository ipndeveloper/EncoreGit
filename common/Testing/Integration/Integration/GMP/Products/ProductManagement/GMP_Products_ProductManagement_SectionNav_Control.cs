using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Products.ProductManagement
{
    public class GMP_Products_ProductManagement_SectionNav_Control : Control<UnorderedList>
    {
        public GMP_Products_ProductManagement_Browse_Page ClickBrowseProducts(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Products/Products", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Products_ProductManagement_Browse_Page>(timeout, pageRequired);
        }

        public GMP_Products_ProductManagement_Create_Page ClickAddANewProduct(int? timeout = null, bool pageRequired = true)
        {
            Element.GetElement<Link>(new Param("/Products/Products/Create", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Products_ProductManagement_Create_Page>(timeout, pageRequired);
        }

        public GMP_Products_ProductManagement_Properties_Page ClickPropertiesManagement(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Products/Properties", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Products_ProductManagement_Properties_Page>(timeout, pageRequired);
        }

        public GMP_Products_ProductManagement_Types_Page ClickProductTypesManagement(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Products/ProductTypes", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Products_ProductManagement_Types_Page>(timeout, pageRequired);
        }

        public GMP_Products_ProductManagement_PriceType_Page ClickPriceTypesManagement(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Products/PriceTypes", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Products_ProductManagement_PriceType_Page>(timeout, pageRequired);
        }

        public GMP_Products_ProductManagement_ResourceType_Page ClickResourceTypesManagement(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Products/ResourceTypes", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Products_ProductManagement_ResourceType_Page>(timeout, pageRequired);
        }

        public GMP_Products_ProductManagement_CustomerType_Page ClickCustomerTypesManagement(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Products/CustomerTypes", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Products_ProductManagement_CustomerType_Page>(timeout, pageRequired);
        }
    }
}
