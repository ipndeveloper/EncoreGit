using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Products.ProductManagement.Product
{
    public class GMP_Products_ProductManagement_Product_Availability_Page : GMP_Products_ProductManagement_Product_Base_Page
    {
        private Table _availabity;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _availabity = Document.GetElement<Table>(new Param("availability"));
        }

         public override bool IsPageRendered()
        {
            return _availabity.Exists;
        }
    }
}
