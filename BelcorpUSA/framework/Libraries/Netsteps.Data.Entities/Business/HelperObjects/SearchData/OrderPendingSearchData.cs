using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchData
{
    [Serializable]
    public class OrderPendingSearchData
    {
            [TermName("OrderNumber", "Order Return Number")]
            public string OrderNumber { get; set; }
           
            [TermName("AccountNumber", "ID Account")]
            public string AccountNumber { get; set; }

            [TermName("FirstName", "First Name")]
            public string FirstName { get; set; }

            [TermName("LastName", "Last Name")]
            public string LastName { get; set; }

            [TermName("NumberSupportTicket", "Number Support Ticket")]
            public int? IDSupportTicket { get; set; }

            [TermName(" NumberNationalMail", " Number National Mail")]
            public string IDNationalMail { get; set; }   
  
            [TermName("DateCreated", "Date Created")]
            public DateTime DateCreatedUTC { get; set; }
           
           
    }
}
