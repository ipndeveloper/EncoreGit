using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchData
{
    [Serializable]
    public class EarningsPerPeriodSearchData
    {
        [TermName("CommissionReportPeriodID")]                                                              
        public string PeriodID { get; set; }

        [TermName("CommissionReportBonusName")]
        public string BonusName { get; set; }

        [TermName("CommissionReportAccountID")]
        public string AccountNumber { get; set; }

        [TermName("CommissionReportAccountName")]
        public string AccountName { get; set; } 
                                                                         
        [TermName("CommissionReportAmount")]
        public string Amount { get; set; }    

    }
}
