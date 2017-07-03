using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Orders
{
    public abstract class GMP_Orders_Base_Page : GMP_Base_Page
    {
        public GMP_Orders_SubNav_Control SubNav
        {
            get { return _subNav.As<GMP_Orders_SubNav_Control>(); }
        }
    }
}
