using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business
{
    public class GLPaymeTycketsSearchData
    {
        public int TicketNumber { get; set; }
        public string StatusExpiration { get; set; }
        public string PaymentMetod { get; set; }
        public string DateCreated { get; set; }
        public decimal InitialAmount { get; set; }
        public string DateLastTotalAmount { get; set; }
        public string DateLastModified { get; set; }

        public string OrderCustomer { get; set; }
        public string DataExpiration { get; set; }
        public string PaymentType { get; set; }
        public string OriginalExpiration { get; set; }
        public decimal FinancialAmount { get; set; }
        public string Accepted { get; set; }


        public int Orders { get; set; }
        public string DateValidity { get; set; }
        public int DeferredAmount { get; set; }
        public string StatusPayment { get; set; }
        public decimal DiscountedAmount { get; set; }
        public string Forefit { get; set; }
       
        public string NegotiationLevel { get; set; }
        public decimal TotalAmount { get; set; }
        

        public string period { get; set; }
      
        
    }
}
