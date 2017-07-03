using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Commissions
{
    public abstract class GMP_Commissions_Base_Page : GMP_Base_Page
    {
        public GMP_Commissions_SubNav_Control SubNav
        {
            get { return _subNav.As<GMP_Commissions_SubNav_Control>(); }
        }
    }
}
