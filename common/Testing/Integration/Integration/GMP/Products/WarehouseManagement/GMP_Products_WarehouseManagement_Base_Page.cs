using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Products.WarehouseManagement
{
    public abstract class GMP_Products_WarehouseManagement_Base_Page : GMP_Products_Base_Page
    {
        public GMP_Products_WarehouseManagement_SectionNav_Control SectionNav
        {
            get { return Control.CreateControl<GMP_Products_WarehouseManagement_SectionNav_Control>(_secNav); }
        }
    }
}
