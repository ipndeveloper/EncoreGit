using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business;

namespace NetSteps.Data.Entities.Repositories
{
	public partial interface INewsRepository : ISearchRepository<NewsSearchParameters, PaginatedList<NewsSearchData>>
	{
	}
}
