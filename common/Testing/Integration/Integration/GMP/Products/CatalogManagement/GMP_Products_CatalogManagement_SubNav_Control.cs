using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Products.CatalogManagement
{
    public class GMP_Products_CatalogManagement_SubNav_Control : Control<Div>
    {
        public GMP_Products_CatalogManagement_Browse_Page ClickBrowseCatalogs(int? timeout = null, bool pageRequired = true)
        {
            Element.CustomRunScript(Util.strSlideDown);
            timeout = Element.GetElement<Link>(new Param("/Products", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Products_CatalogManagement_Browse_Page>(timeout, pageRequired);
        }

        public GMP_Products_CatalogManagement_CategoryTrees_Page ClickBrowseCategoryTrees(int? timeout = null, bool pageRequired = true)
        {
            Element.CustomRunScript(Util.strSlideDown);
            timeout = Element.GetElement<Link>(new Param("/Products/Categories", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Products_CatalogManagement_CategoryTrees_Page>(timeout, pageRequired);
        }
    }
}
