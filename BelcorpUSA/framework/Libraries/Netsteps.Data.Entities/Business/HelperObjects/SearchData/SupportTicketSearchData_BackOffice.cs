using System;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchData
{
    [Serializable]
    public struct SupportTicketSearchData_BackOffice
    {
        [TermName("ID")]
        [Display(AutoGenerateField = false)]
        public int SupportTicketID { get; set; }

        [TermName("AccountNumber", "Account Number")]
        [Display(AutoGenerateField = false)]
        public string AccountNumber { get; set; }

        [TermName("SupportTicketNumber", "Support Ticket Number")]
        public string SupportTicketNumber { get; set; }

        [TermName("SupportTicketStatus", "Status")]
        [Display(AutoGenerateField = false)]
        public short SupportTicketStatusID { get; set; }

        [TermName("SupportTicketCategory", "Category")]
        [Display(AutoGenerateField = false)]
        public short SupportTicketCategoryID { get; set; }

        [TermName("Changed", "Changed")]
        [Display(AutoGenerateField = false)]
        public DateTime DateLastModified { get; set; }

        /******************** Visible Fields *****************/


        [TermName("Title", "Title")]
        public string Title { get; set; }


        public string Status { get; set; }


        public string Category { get; set; }

        [TermName("Submitted", "Submitted")]
        public DateTime DateCreated { get; set; }


    }
}
