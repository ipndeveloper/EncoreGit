using NetSteps.Common.Attributes;
using System;

namespace NetSteps.Data.Entities.Business
{
    [Serializable]
    public class TotalSalesSearchData
    {
        [TermName("Month")]
        public int Month { get; set; }

        [TermName("Subtotal")]
        public decimal Subtotal { get; set; }

        [TermName("Gross")]
        public decimal Gross { get; set; }

        [TermName("State")]
        public string State { get; set; }
    }
}
