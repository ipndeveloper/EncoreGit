using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchData
{
    public class DebtsPerAgeSearchData
    {

        public int AccountNumber { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int? PaymentTicketNumber { get; set; }
        public int OrderNumber { get; set; }
        public decimal? NfeNumber { get; set; }              // OrderInvoices.InvoiceNumber

        public DateTime? OrderDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTime? BalanceDate { get; set; }

        public decimal OriginalBalance { get; set; }    // OrderPayments.InitialAmount
        public decimal CurrentBalance { get; set; }     // OrderPayments.TotalAmount}

        public int OverdueDays { get; set; }
        public bool Forfeit { get; set; }

        public int Period { get; set; }                 // CompletedPeriodID

        public DateTime? DateOfBirth { get; set; }

        public int TotalPages { get; set; }
        public int TotalRows { get; set; }

    }
}
