using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Encore.Core;
using System.Text;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using System.Runtime.Serialization;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchParameters
{
    [Serializable]
    public class LogErrorBankPaymentsSearchParameter : FilterDateRangePaginatedListParameters<LogErrorBankPaymentsSearchData>
    {

      

        public string DateBankLog { get; set; }
        public string Date { get; set; }
        public int? BankId { get; set; }
        public int? FileSequenceLog { get; set; }
        public int? StatusLog { get; set; }
        public int  TicketNumber { get; set; }
        public int LogErrorBankPaymentID { get; set; }
        public int UserID { get; set; }

      


        
    }
}
