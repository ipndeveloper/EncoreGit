using WatiN.Core;

namespace NetSteps.Testing.Integration.DWS.Contacts
{
    public abstract class DWS_Contacts_Base_Page : DWS_Base_Page
    {
        private DWS_Contacts_SectionNavigation_Control _sectionNav;

        protected override void InitializeContents()
        {
            base.InitializeContents();

            // Prospect.
            _sectionNav = _sectionNavList.As<DWS_Contacts_SectionNavigation_Control>();
        }

        public DWS_Contacts_SectionNavigation_Control SectionNav
        {
            get { return _sectionNav; }
        }
    }
}
