using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Products.ProductManagement.Product
{
    public class GMP_Products_ProductManagement_Product_Resources_Page : GMP_Products_ProductManagement_Product_Base_Page
    {
        private SelectList _type;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _type = Document.SelectList("resourceType");
        }

         public override bool IsPageRendered()
        {
            return _type.Exists;
        }
    }
}
