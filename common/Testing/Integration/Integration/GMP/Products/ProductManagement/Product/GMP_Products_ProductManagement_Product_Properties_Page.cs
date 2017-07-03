using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Products.ProductManagement.Product
{
    public class GMP_Products_ProductManagement_Product_Properties_Page : GMP_Products_ProductManagement_Product_Base_Page
    {
         public override bool IsPageRendered()
        {
            return Document.GetElement<Table>(new Param("properties")).Exists;
        }
    }
}
