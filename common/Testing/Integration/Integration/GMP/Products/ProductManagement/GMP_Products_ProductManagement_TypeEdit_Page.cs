using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Products.ProductManagement
{
    public class GMP_Products_ProductManagement_TypeEdit_Page : GMP_Products_ProductManagement_Base_Page
    {
         public override bool IsPageRendered()
        {
            return Document.GetElement<TextField>(new Param("name")).Exists;
        }
    }
}
