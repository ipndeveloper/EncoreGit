using WatiN.Core;
using System;
using System.Threading;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Support
{
    public class GMP_Support_BrowseTickets_Page : GMP_Support_Base_Page
    {
        private Link _newTicket, _filter;
        private Table _tbl;
        private TableRowCollection _tickets;
        private TextField _title;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _newTicket = Document.GetElement<Link>(new Param("requestTicket"));
            _newTicket.CustomWaitForExist();
            Document.CustomWaitForSpinners(30, 1);
            Util.Browser.CustomWaitForComplete(null);
            _tbl = Document.GetElement<Table>(new Param("paginatedGrid"));
            _title = Document.GetElement<TextField>(new Param("TitleInputFilter"));
            _filter = Document.GetElement<Link>(new Param("Button ModSearch filterButton", AttributeName.ID.ClassName, RegexOptions.None));
        }

         public override bool IsPageRendered()
        {
            return _newTicket.Exists && _tbl.Exists;
        }

         public GMP_Support_TicketEditDetails_Page ClickRequestNewTicket(int? timeout = null, bool pageRequired = true)
        {
            timeout = _newTicket.CustomClick(timeout);
            Thread.Sleep(2000);
            return Util.GetPage<GMP_Support_TicketEditDetails_Page>(timeout, pageRequired);
        }

        public GMP_Support_Ticket_Control GetTicket(int? index = null)
        {
            GMP_Support_Ticket_Control ticket = null;
            GetTicketTable();

            _tickets = _tbl.TableRows;
            if (!index.HasValue)
                index = Util.GetRandom(1, _tickets.Count - 1);
            if (index == 0)
                throw new ArgumentException();
            ticket = _tickets[(int)index].As<GMP_Support_Ticket_Control>();
            Util.LogDoneMessage(string.Format("Ticket {0} selected", ticket.Number));
            return ticket;
        }

        public GMP_Support_Ticket_Control GetCreatedTicket(string title)
        {
            GMP_Support_Ticket_Control ticket = null;
            GetTicketTable();

            foreach (TableRow row in _tickets)
            {
                if (row.GetElement<TableCell>(new Param(3)).CustomGetText() == title)
                {
                    ticket = row.As<GMP_Support_Ticket_Control>();
                    break;
                }
            }
            if (ticket == null)
                throw new ArgumentException(string.Format("Ticket {0} not found", title));
            return ticket;
        }

        public GMP_Support_BrowseTickets_Page FilterTicket(string title)
        {
            _title.CustomSetTextQuicklyHelper(title);
            _filter.CustomClick();
            _tbl.CustomWaitForSpinners();
            return this;
        }

        private void GetTicketTable(int? timeout = null, int? delay = 2)
        {
                _tbl.CustomWaitForSpinners(timeout, delay);
                Thread.Sleep(1000);
                _tickets = Document.GetElement<Table>(new Param("paginatedGrid")).TableBody(Find.Any).TableRows;
        }
    }
}
