using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Encore.Core;
using System.Text;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;

namespace NetSteps.Data.Entities.Business
{
    public class PaymentTicketsSearchParameters : FilterDateRangePaginatedListParameters<PaymetTycketsReportSearchData>
    {
        static readonly int CHashCodeSeed = typeof(PaymentTicketsSearchParameters).GetKeyForType().GetHashCode();


        public int? TicketNumber { get; set; }
        public int? IsDeferred { get; set; }
        public int? Forefit { get; set; }
        public string OrderNumber { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int  AccountID { get; set; }

         public int? NegotiationLevelID { get; set; }
         public int? ExpirationStatusID { get; set; }
         public int? Country { get; set; }
         public int? BankID { get; set; }

         public int? _AccountID { get; set; }
        public DateTime? ExpiriedDateFrom { get; set; }
        public DateTime? ExpiriedDateTo { get; set; }
        public DateTime? LiquidationDateFrom { get; set; }
        public DateTime? LiquidationDateTo { get; set; }
        public int? OrderId { get; set; }
        
        public string FiscalNote { get; set; }

        public int? OrderPaymentStatusId { get; set; }
    }
}
