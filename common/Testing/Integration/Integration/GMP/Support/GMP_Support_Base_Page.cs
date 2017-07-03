using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Support
{
    public abstract class GMP_Support_Base_Page : GMP_Base_Page
    {
        public GMP_Support_SubNav_Control SubNav
        {
            get { return _subNav.As<GMP_Support_SubNav_Control>(); }
        }

    }
}
