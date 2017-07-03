using WatiN.Core;
using System;
using System.Threading;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.DWS.Support
{
    /// <summary>
    /// Class related to controls and ops of DWS Support page.
    /// </summary>
    public class DWS_Support_Browse_Page : DWS_Support_Base_Page
    {
        #region Controls
        private Span _allTickets, _openTickets, _resolvedTickets, _needsInput;
        private TableRowCollection _tickets;
        private Table _ticketTable;
        private TextField _title;
        private Link _go;

        protected override void InitializeContents()
        {
            base.InitializeContents();
           
            
            _resolvedTickets = Document.GetElement<Span>(new Param("resolvedTicketCount"));
            _resolvedTickets.CustomWaitForExist();
            _needsInput = Document.GetElement<Span>(new Param("needsInputTicketCount"));
            _needsInput.CustomWaitForExist();
            _ticketTable = Document.GetElement<Table>(new Param("paginatedGrid"));
            _ticketTable.CustomWaitForSpinners();
            _title = Document.GetElement<TextField>(new Param("TitleInputFilter"));
            _go = Document.GetElement<Link>(new Param("Button ModSearch filterButton", AttributeName.ID.ClassName, RegexOptions.None));
            Thread.Sleep(3000);
        }

        #endregion

        #region Methods

        public int GetAllTicketsNumber()
        {
            _allTickets = Document.GetElement<Span>(new Param("allTicketCount")); 
            return Convert.ToInt32(_allTickets.Text.Trim(new Char[] { '(', ')' }));
        }

        public int GetOpenTicketNumber()
        {
            _openTickets = Document.GetElement<Span>(new Param("openTicketCount"));
            return Convert.ToInt32(_openTickets.Text.Trim(new Char[] { '(', ')' }));
        }

        public int GetResolvedTicketNumber
        {
            get { return Convert.ToInt32(_resolvedTickets.Text.Trim(new Char[] { '(', ')' })); }
        }

        public int GetNeedsInput
        {
            get { return Convert.ToInt32(_needsInput.Text.Trim(new Char[] { '(', ')' })); }
        }
        
        public override bool IsPageRendered()
        {
            return _title.Exists;
        }

         public ControlCollection<DWS_SupportTicket_Control> GetTickets()
         {
             GetTicketTable();
             return _tickets.As<DWS_SupportTicket_Control>();
         }

         public DWS_SupportTicket_Control GetTicket(string title)
         {
             DWS_SupportTicket_Control ticket = null;
             GetTicketTable();

             foreach (TableRow row in _tickets)
             {
                 if (row.GetElement<TableCell>(new Param(1)).CustomGetText() == title)
                 {
                     ticket = row.As<DWS_SupportTicket_Control>();
                     break;
                 }
             }
             if (ticket == null)
                 throw new ArgumentException(string.Format("Ticket {0} not found", title));
             return ticket;
         }

         private void GetTicketTable(int? timeout = null, int? delay = 2)
         {
                 _ticketTable.CustomWaitForSpinners(timeout, delay);
                 Thread.Sleep(1000);
                 _tickets = _ticketTable.TableBody(Find.Any).TableRows;
         }

         public DWS_Support_Browse_Page FindTicket(string title, int? timeout = null, int? delay = 2)
         {
             _title.CustomSetTextQuicklyHelper(title);
             _go.CustomClick();
             _ticketTable.CustomWaitForSpinners(timeout, delay);
             Thread.Sleep(1000);
             return this;
         }

        #endregion
    }
}