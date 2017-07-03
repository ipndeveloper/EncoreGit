using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Accounts
{
    public abstract class GMP_Accounts_Base_Page : GMP_Base_Page
    {
        protected override void InitializeContents()
        {
            base.InitializeContents();
        }

        public GMP_Accounts_SubNav_Control SubNav
        {
            get { return _subNav.As<GMP_Accounts_SubNav_Control>(); }
        }
    }
}
