using WatiN.Core;

namespace NetSteps.Testing.Integration.DWS.Communication
{
    public class DWS_Communications_Base_Page : DWS_Base_Page
    {
        private DWS_Communication_SectionNav_Control _secNav;
        private DWS_Communication_MailboxNav_Control _mailNav;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _secNav = _sectionNavList.As<DWS_Communication_SectionNav_Control>();
            _mailNav = Document.UnorderedList(Find.ByClass("listNav")).As<DWS_Communication_MailboxNav_Control>();
        }

        public override bool IsPageRendered()
        {
            return _mailNav.Exists;
        }

        public DWS_Communication_SectionNav_Control SectionNav
        {
            get { return _secNav; }
        }

        public DWS_Communication_MailboxNav_Control MailboxNav
        {
            get { return _mailNav; }
        }

        public DWS_Communication_Group_Page ClickNewGroup(int? timeout = null, bool pageRequired = true)
        {
            timeout = Document.GetElement<Link>(new Param("Create New Group", AttributeName.ID.Title)).CustomClick(timeout);
            return Util.GetPage<DWS_Communication_Group_Page>(timeout, pageRequired);
        }
    }
}
