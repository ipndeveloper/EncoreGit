

namespace NetSteps.Testing.Integration.GMP.Products.ProductManagement.Product
{
    public abstract class GMP_Products_ProductManagement_Product_Base_Page : GMP_Products_Base_Page
    {
        private GMP_Products_ProductManagement_Product_SectionNav_Control _sectionNav;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _sectionNav = _secNav.As<GMP_Products_ProductManagement_Product_SectionNav_Control>();
        }


        public GMP_Products_ProductManagement_Product_SectionNav_Control SectionNav
        {
            get { return _sectionNav; }
        }
    }
}
