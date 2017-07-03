using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchData
{
    public class TicketPaymentPerMonthSearchData
    {

        public int PaymentTicketNumber { get; set; }
        public int OrderNumber { get; set; }
        public decimal? NfeNumber { get; set; }

        public DateTime? OrderDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTime? BalanceDate { get; set; }

        public decimal OriginalBalance { get; set; }
        public decimal CurrentBalance { get; set; }

        public int Status { get; set; }
        public DateTime? OriginalExpirationDate { get; set; }

        public int AccountNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }

        public int TotalPages { get; set; }
        public int TotalRows { get; set; }

    }
}
