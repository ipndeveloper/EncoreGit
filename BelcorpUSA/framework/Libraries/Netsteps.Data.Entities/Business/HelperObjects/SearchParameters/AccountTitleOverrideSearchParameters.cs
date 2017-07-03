using NetSteps.Common.Base;
using NetSteps.Data.Entities.Commissions;

namespace NetSteps.Data.Entities.Business
{
    public class AccountTitleOverrideSearchParameters : FilterDateRangePaginatedListParameters<AccountTitleOverride>
    {
        public int? AccountID { get; set; }
        public int? AccountTitleOverrideID { get; set; }
        public int? OverrideReasonID { get; set; }
    }
}
