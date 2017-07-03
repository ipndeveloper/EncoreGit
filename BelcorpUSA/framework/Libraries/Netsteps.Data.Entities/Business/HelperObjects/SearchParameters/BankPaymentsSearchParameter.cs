using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Encore.Core;
using System.Text;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchParameters
{
    public class BankPaymentsSearchParameter : FilterDateRangePaginatedListParameters<BankPayments>
    {

        static readonly int CHashCodeSeed = typeof(BankConsolidateApplicationSearchParameter).GetKeyForType().GetHashCode();

        public int? Bankid { get; set; }
        public int? FileSequence { get; set; }
        public int? TicketNumber { get; set; }
        public int? AccountCode { get; set; }
        public string Amount { get; set; }
        public string DateReceivedBank { get; set; }
        public string BankName { get; set; }
        public string FileNameBank { get; set; }
        public int logSequence { get; set; }
        public string TipoCredito { get; set; }
        public int UserID { get; set; }
        public string FileDate { get; set; }
        
    }
}
