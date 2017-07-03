using WatiN.Core;
using System;
using System.Threading;

namespace NetSteps.Testing.Integration.DWS.Support
{
    public class DWS_Support_CreateTicket_Page : DWS_Support_Base_Page
    {
        private Link _submit;
        private SelectList _category;
        private TextField _title, _description;
        private Span _allTickets, _openTickets, _resolvedTickets, _needsInput;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _submit = Document.GetElement<Link>(new Param("Button FormSubmit btnSubmitTicket", AttributeName.ID.ClassName));
            _category = Document.GetElement<SelectList>(new Param("SupportTicketCategoryID"));
            _title = Document.GetElement<TextField>(new Param("Title"));
            _description = Document.GetElement<TextField>(new Param("Description"));
            _allTickets = Document.GetElement<Span>(new Param("allTicketCount"));
            _allTickets.CustomWaitForExist();
            _openTickets = Document.GetElement<Span>(new Param("openTicketCount"));
            _openTickets.CustomWaitForExist();
            _resolvedTickets = Document.GetElement<Span>(new Param("resolvedTicketCount"));
            _resolvedTickets.CustomWaitForExist();
            _needsInput = Document.GetElement<Span>(new Param("needsInputTicketCount"));
            _needsInput.CustomWaitForExist();
            Thread.Sleep(3000);
        }
         public override bool IsPageRendered()
        {
            return _submit.Exists;
        }

         public void FillOutTicket(string title)
         {
             _category.CustomSelectDropdownItem(2);
             _title.CustomSetTextQuicklyHelper(title);
             _description.CustomSetTextQuicklyHelper("Test for DWS submitting ticket");
         }

         public DWS_Support_Browse_Page SubmitTicket()
         {
             _submit.CustomClick();
             DWS_Support_Browse_Page page = Util.GetPage<DWS_Support_Browse_Page>();
             Document.CustomWaitForSpinners();
             return page;
         }

         public int GetAllTicketsNumber
         {
             get { return Convert.ToInt32(_allTickets.Text.Trim(new Char[] { '(', ')' })); }
         }

         public int GetOpenTicketNumber
         {
             get { return Convert.ToInt32(_openTickets.Text.Trim(new Char[] { '(', ')' })); }
         }

         public int GetResolvedTicketNumber
         {
             get { return Convert.ToInt32(_resolvedTickets.Text.Trim(new Char[] { '(', ')' })); }
         }

         public int GetNeedsInput
         {
             get { return Convert.ToInt32(_needsInput.Text.Trim(new Char[] { '(', ')' })); }
         }
    }
}
