using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Common.Attributes;
using System.ComponentModel.DataAnnotations; 

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchData
{
    [Serializable]
    public class SupportTicketSearchDetailsData
    {         
        [Display(AutoGenerateField = false)]
        public int SupportTicketID { get; set; }        

        [TermName("SupportTicketNumber", "Support Ticket Number")]
        public string SupportTicketNumber { get; set; }

        [TermName("AssignedUsername", "Assigned Username")]
        public string AssignedUsername { get; set; }

        [TermName("SupportTicketPriority", "Priority")]
        public string PriorityName { get; set; }

        [TermName("Title", "Title")]
        [Display(AutoGenerateField = false)]
        public string Title { get; set; }

        [TermName("AccountNumber", "Account Number")]
        [Display(AutoGenerateField = true)]
        public string AccountNumber { get; set; }

        /*CS.19AG2016.Inicio*/
        [TermName("OrderNumber", "Order Number")]
        [Display(AutoGenerateField = true)]
        public string OrderNumber { get; set; }

        [TermName("State", "State")]
        [Display(AutoGenerateField = true)]
        public string State { get; set; }

        [TermName("City", "City")]
        [Display(AutoGenerateField = true)]
        public string City { get; set; }

        [TermName("InvoiceNumber", "Invoice Number")]
        [Display(AutoGenerateField = true)]
        public string InvoiceNumber { get; set; }
        /*CS.19AG2016.Fin*/

        [TermName("FirstName", "First Name")]
        public string FirstName { get; set; }

        [TermName("LastName", "Last Name")]
        public string LastName { get; set; }

        [TermName("SupportTicketStatus", "Status")]
        public string StatusName { get; set; }

        [TermName("SupportTicketCategory", "Category")]
        public string CategoryName { get; set; }

        [TermName("CreateUserName", "Create User Name")]
        public string CreateUserName { get; set; }

        [TermName("Created", "Created")]
        public string DateCreated { get; set; }

        [TermName("Changed", "Changed")]
        public string DateLastModified { get; set; }

        [TermName("Confirm", "Confirm")]
        [Display(AutoGenerateField = false)]
        public string Comfim { get; set; }

        [TermName("Close", "Close")]
        public string Close { get; set; }

        [TermName("Question", "Question")]
        [Display(AutoGenerateField = false)]
        public string Question { get; set; }

        [TermName("Respuesta", "Respuesta")]
        [Display(AutoGenerateField = false)]
        public string Respuesta { get; set; }

        [Display(AutoGenerateField = false)]
        public string RowTotal { get; set; }        
    }
}
