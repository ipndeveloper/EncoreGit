using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Products.CatalogManagement
{
    public class GMP_Products_CatalogManagement_CategorySectionHeader_Control : Control<Div>
    {
        public GMP_Products_CatalogManagement_CategoryTrees_Page ClickBrowseCategoryTrees(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Products/Categories", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Products_CatalogManagement_CategoryTrees_Page>(timeout, pageRequired);
        }

        public GMP_Products_CatalogManagement_CategoryCreate_page ClickCreateANewCategoryTree(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Products/Categories/NewTree", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Products_CatalogManagement_CategoryCreate_page>(timeout, pageRequired);
        }

    }
}
