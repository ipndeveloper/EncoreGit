using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Admin
{
    public abstract class GMP_Admin_Base_Page : GMP_Base_Page
    {
        public GMP_Admin_SubNav_Control SubNav
        {
            get { return _subNav.As<GMP_Admin_SubNav_Control>(); }
        }
    }
}
