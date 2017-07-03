using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Products.ProductManagement
{
    public abstract class GMP_Products_ProductManagement_Base_Page : GMP_Products_Base_Page
    {
        public GMP_Products_ProductManagement_SectionNav_Control SectionNav
        {
            get { return Control.CreateControl<GMP_Products_ProductManagement_SectionNav_Control>(_secNav); }
        }
    }
}
