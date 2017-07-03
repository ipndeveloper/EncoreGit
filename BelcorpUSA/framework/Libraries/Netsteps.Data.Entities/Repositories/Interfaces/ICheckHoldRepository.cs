using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Commissions;

namespace NetSteps.Data.Entities.Repositories
{
    public partial interface ICheckHoldRepository : IBaseRepository<CheckHold, int>, ISearchRepository<CheckHoldSearchParameters, PaginatedList<CheckHoldSearchData>>
    {
        void InsertOrUpdate(CheckHold checkHold);
    }
}
