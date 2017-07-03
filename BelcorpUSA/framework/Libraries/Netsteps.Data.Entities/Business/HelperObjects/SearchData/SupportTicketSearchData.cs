using System;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business
{
    [Serializable]
    public struct SupportTicketSearchData
    {
        [TermName("ID")]
        [Display(AutoGenerateField = false)]
        public int SupportTicketID { get; set; }

        [TermName("AccountNumber", "Account Number")]
        [Display(AutoGenerateField = false)]
        public string AccountNumber { get; set; }

        [TermName("SupportTicketNumber", "Support Ticket Number")]
        public string SupportTicketNumber { get; set; }

        [TermName("AssignedUsername", "Assigned Username")]
        public string AssignedUsername { get; set; }

        [TermName("SupportTicketPriority", "Priority")]
        public short SupportTicketPriorityID { get; set; }

        [TermName("Title", "Title")]
        public string Title { get; set; }

        /*CS.19AG2016.Inicio*/
        [TermName("OrderNumber", "Order Number")]
        public string OrderNumber { get; set; }

        [TermName("State", "State")]
        public string State { get; set; }

        [TermName("City", "City")]
        public string City { get; set; }

        [TermName("InvoiceNumber", "Invoice Number")]
        public string InvoiceNumber { get; set; }
        /*CS.19AG2016.Fin*/

        [TermName("FirstName", "First Name")]
        public string FirstName { get; set; }

        [TermName("LastName", "Last Name")]
        public string LastName { get; set; }

        [TermName("SupportTicketStatus", "Status")]
        public short SupportTicketStatusID { get; set; }

        [TermName("SupportTicketCategory", "Category")]
        public short SupportTicketCategoryID { get; set; }

        [TermName("Created", "Created")]
        public DateTime DateCreated { get; set; }

        [TermName("Changed", "Changed")]
        public DateTime DateLastModified { get; set; }
        
        [Display(AutoGenerateField = false)]
        public string SupportLevelMotive { get; set; }

        [TermName("Site", "Site")]
        public Byte IsSiteDWS { get; set; }
         
    }
}
