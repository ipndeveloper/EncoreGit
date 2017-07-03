using NetSteps.Common.Base;
using NetSteps.Data.Entities.Commissions;

namespace NetSteps.Data.Entities.Business
{
    public class CalculationOverrideSearchParameters : FilterDateRangePaginatedListParameters<CalculationOverride>
    {
        public int? AccountID { get; set; }
        public int? CalculationOverrideID { get; set; }
        public int? OverrideReasonID { get; set; }
    }
}
