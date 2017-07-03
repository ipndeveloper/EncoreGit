using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Products.CatalogManagement
{
    public class GMP_Products_CatalogManagement_BrowseSectionHeader_Control : Control<Div>
    {

        public GMP_Products_CatalogManagement_Browse_Page ClickBrowseCatalogs(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Products/Catalogs", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Products_CatalogManagement_Browse_Page>(timeout, pageRequired);
        }

        public GMP_Products_CatalogManagement_Edit_Page ClickCreateNewCatalog(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Products/Catalogs/Edit", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Products_CatalogManagement_Edit_Page>(timeout, pageRequired);
        }
    }
}
