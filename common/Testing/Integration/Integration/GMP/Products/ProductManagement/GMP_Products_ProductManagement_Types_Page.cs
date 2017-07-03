using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Products.ProductManagement
{
    public class GMP_Products_ProductManagement_Types_Page : GMP_Products_ProductManagement_Base_Page
    {
         public override bool IsPageRendered()
        {
            return Document.GetElement<Link>(new Param("/Products/ProductTypes/Edit", AttributeName.ID.Href, RegexOptions.None)).Exists; ;
        }

         public GMP_Products_ProductManagement_TypeEdit_Page ClickAddNewProductType(int? timeout = null, bool pageRequired = true)
        {
            timeout = Document.GetElement<Link>(new Param("btnAdd")).CustomClick(timeout);
            return Util.GetPage<GMP_Products_ProductManagement_TypeEdit_Page>(timeout, pageRequired);
        }
    }
}
