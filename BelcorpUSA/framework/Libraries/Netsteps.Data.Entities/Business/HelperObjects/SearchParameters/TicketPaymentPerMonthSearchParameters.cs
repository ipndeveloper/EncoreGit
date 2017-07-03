using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchParameters
{
    public class TicketPaymentPerMonthSearchParameters
    {

        public int? TicketNumber { get; set; }
        public int? AccountId { get; set; }

        public DateTime? StartIssueDate { get; set; }
        public DateTime? EndIssueDate { get; set; }

        public DateTime? StartDueDate { get; set; }
        public DateTime? EndDueDate { get; set; }

        public string OrderNumber { get; set; }
        public int? StatusId { get; set; }

        public DateTime Month { get; set; }

        public Constants.SortDirection SortOrder { get; set; }
        public string OrderBy { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public string AccountText { get; set; }
        public string TicketText { get; set; }

    }
}
