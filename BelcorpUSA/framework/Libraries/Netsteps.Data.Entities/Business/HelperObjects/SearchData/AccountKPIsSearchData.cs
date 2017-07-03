using NetSteps.Common.Attributes;
using System;

namespace NetSteps.Data.Entities.Business
{
    [Serializable]
    public class AccountKPIsSearchData
    {
        [TermName("MLMColAccountNumber")]
        public string AccountNumber { get; set; }

        [TermName("MLMColName")]
        public string Name { get; set; }

        [TermName("MLMColPeriod")]
        public int Period { get; set; }

        [TermName("MLMColCareerTitle")]
        public string CareerTitle { get; set; }

        [TermName("MLMColPaidAsTitle")]
        public string PaidAsTitle { get; set; }

        [TermName("MLMColIndicator")]
        public string Indicator { get; set; }

        [TermName("MLMColAmount")]
        public double Amount { get; set; }

    }
}
