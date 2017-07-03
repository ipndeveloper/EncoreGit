using WatiN.Core;
using System.Threading;

namespace NetSteps.Testing.Integration.GMP.Support
{
    public class GMP_Support_CreateTicket_Page : GMP_Support_Base_Page
    {
        TextField _distributor;
        SearchSuggestionBox_Control _distributorControl;
        TextField _title, _description;
        Link _save;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _distributor = Document.GetElement<TextField>(new Param("txtSearch"));
            _distributorControl = _distributor.As<SearchSuggestionBox_Control>();
            _title = Document.GetElement<TextField>(new Param("title"));
            _description = Document.GetElement<TextField>(new Param("description"));
            _save = Document.GetElement<Link>(new Param("btnSave"));
        }

         public override bool IsPageRendered()
        {
            return _distributor.Exists;
        }

        public GMP_Support_CreateTicket_Page SelectDistributor(string distributor)
        {
            _distributorControl.Select(distributor);
            return this;
        }

        public GMP_Support_CreateTicket_Page EnterTitle(string title)
        {
            _title.CustomSetTextQuicklyHelper(title);
            return this;
        }

        public GMP_Support_CreateTicket_Page EnterDescription(string description)
        {
            _description.CustomSetTextQuicklyHelper(description);
            return this;
        }

        public GMP_Support_TicketEditDetails_Page ClickSaveTicket(int? timeout = null, bool pageRequired = true)
        {
            timeout = _save.CustomClick(timeout);
            timeout = Util.Browser.CustomWaitForComplete(timeout); // System saves the page and then navigates to Edit page
            Thread.Sleep(2000);
            return Util.GetPage<GMP_Support_TicketEditDetails_Page>(timeout, pageRequired);
        }
    }
}
