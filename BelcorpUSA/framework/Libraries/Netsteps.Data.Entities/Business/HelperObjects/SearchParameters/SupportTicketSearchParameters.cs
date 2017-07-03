using NetSteps.Common.Base;
using System;

namespace NetSteps.Data.Entities.Business
{
    public class SupportTicketSearchParameters : FilterDateRangePaginatedListParameters<SupportTicket>
    {
        public int? SupportTicketID { get; set; }

        public string SupportTicketNumber { get; set; }

        public string Title { get; set; }

        public int? AccountID { get; set; }

        public int? AssignedUserID { get; set; }

        public int? SupportTicketPriorityID { get; set; }

        public int? SupportTicketStatusID { get; set; }

        public int? SupportTicketCategoryID { get; set; }

        public bool? SupportTicketStatusOpen { get; set; }

        public bool? IsVisibleToOwner { get; set; } 
        public string Description { get; set; }

        public string OrderNumber { get; set; }
        public string InvoiceNumber { get; set; }

        public int CreatedByUserID  {get;  set;}

        public int ModifiedByUserID { get; set; }

        public DateTime DateCreatedUTC { get; set; }

        public DateTime DateLastModifiedUTC { get; set; }

        public DateTime DateCloseUTC { get; set; }

        public int? SupportLevelID { get; set; }
        public int? SupportMotiveID { get; set; }
        public byte IsSiteDWS { get; set; } 
        
    }
}
