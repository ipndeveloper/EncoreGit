using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Products.ProductManagement.Product
{
    public class GMP_Products_ProductManagement_Product_Pricing_Page : GMP_Products_ProductManagement_Product_Base_Page
    {
        private SelectList _currency;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _currency = Document.SelectList("sCurrency");
        }

         public override bool IsPageRendered()
        {
            return _currency.Exists;
        }
    }
}
