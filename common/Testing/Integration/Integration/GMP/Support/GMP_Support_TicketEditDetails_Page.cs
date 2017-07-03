using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Support
{
    public class GMP_Support_TicketEditDetails_Page : GMP_Support_TicketEditBase_Page
    {
        TextField _title, _description;
        Link _save;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _title = Document.GetElement<TextField>(new Param("title"));
            _description = Document.GetElement<TextField>(new Param("description"));
            _save = Document.GetElement<Link>(new Param("btnSave"));
        }

         public override bool IsPageRendered()
        {
            return Document.GetElement<TextField>(new Param("title")).Exists;
        }

        public GMP_Support_TicketEditDetails_Page EnterTitle(string title)
        {
            _title.CustomSetTextQuicklyHelper(title);
            return this;
        }

        public GMP_Support_TicketEditDetails_Page EnterDescription(string description)
        {
            _description.CustomSetTextQuicklyHelper(description);
            return this;
        }

        public GMP_Support_TicketEditDetails_Page ClickSaveTicket(int? timeout = null)
        {
            _save.CustomClick(timeout);
            return this;
        }
    }
}
