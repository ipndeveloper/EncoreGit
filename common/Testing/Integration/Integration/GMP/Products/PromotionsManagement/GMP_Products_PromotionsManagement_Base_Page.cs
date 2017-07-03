using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Products.PromotionsManagement
{
    public abstract class GMP_Products_PromotionsManagement_Base_Page : GMP_Products_Base_Page
    {
        public GMP_Products_PromotionsManagement_SectionNav_Control SectionNav
        {
            get { return Control.CreateControl<GMP_Products_PromotionsManagement_SectionNav_Control>(_secNav); }
        }
    }
}
