using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business
{
    public class GLCalculateUpdateBalanceSearchData
    {
        public int TicketNumber { get; set; }
        public int AccountID { get; set; }
        public string Name { get; set; }
        public decimal InitialAmount { get; set; }
        public DateTime DateCreatedUTC { get; set; }
        public decimal FinancialAmount { get; set; }       
        public decimal TotalAmount { get; set; }
        public DateTime CurrentExpirationDateUTC { get; set; }
        public decimal DisCountedAmount { get; set; }
        public int ExpirationStatusID { get; set; }

        public string ProcessOnDateUTC { get; set; }
    }
}
