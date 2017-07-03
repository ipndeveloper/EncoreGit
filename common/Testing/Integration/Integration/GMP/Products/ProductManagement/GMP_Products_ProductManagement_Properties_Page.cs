using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Products.ProductManagement
{
    public class GMP_Products_ProductManagement_Properties_Page : GMP_Products_ProductManagement_Base_Page
    {
         public override bool IsPageRendered()
        {
            return Document.GetElement<Link>(new Param("/Products/Properties/Edit", AttributeName.ID.Href, RegexOptions.None)).Exists;
        }

         public GMP_Products_ProductManagement_PropertiesEdit_Page ClickCreateNewProperty(int? timeout = null, bool pageRequired = true)
        {
            timeout = Document.GetElement<Link>(new Param("/Products/Properties/Edit", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Products_ProductManagement_PropertiesEdit_Page>(timeout, pageRequired);
        }
    }
}
