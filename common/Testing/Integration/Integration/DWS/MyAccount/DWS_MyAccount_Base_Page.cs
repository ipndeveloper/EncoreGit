using WatiN.Core;

namespace NetSteps.Testing.Integration.DWS.MyAccount
{
    public abstract class DWS_MyAccount_Base_Page : DWS_Base_Page
    {
        private DWS_MyAcount_SectionNav_Control _secNav;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _secNav = _sectionNavList.As<DWS_MyAcount_SectionNav_Control>();
        }

        public DWS_MyAcount_SectionNav_Control SectionNav
        {
            get { return _secNav; }
        }
    }
}
