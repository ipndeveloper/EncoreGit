using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Products.ProductManagement.Product
{
    public class GMP_Products_ProductManagement_Product_Details_Page : GMP_Products_ProductManagement_Product_Base_Page
    {
        private TextField _sku;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _sku = Document.GetElement<TextField>(new Param("txtSKU"));
        }
         public override bool IsPageRendered()
        {
            return _sku.Exists;
        }
    }
}
