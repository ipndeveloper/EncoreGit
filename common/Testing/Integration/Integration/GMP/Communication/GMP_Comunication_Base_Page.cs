using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Communication
{
    public abstract class GMP_Comunication_Base_Page : GMP_Base_Page
    {
        public GMP_Communication_SubNav_Control SubNav
        {
            get { return _subNav.As<GMP_Communication_SubNav_Control>(); }
        }
    }
}
