using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;

namespace NetSteps.Data.Entities.Repositories.Interfaces
{
    public partial interface IReportRepository : ISearchRepository<ReportSearchParameters, PaginatedList<ReportSearchData>>
    {
    }
}
