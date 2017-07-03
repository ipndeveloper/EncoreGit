using NetSteps.Common.Base;
using NetSteps.Data.Entities.Commissions;

namespace NetSteps.Data.Entities.Business
{
    public class CheckHoldSearchParameters : FilterDateRangePaginatedListParameters<CheckHold>
    {
        public int? AccountID { get; set; }
        public int? CheckHoldID { get; set; }
        public int? ReasonID { get; set; }
    }
}
