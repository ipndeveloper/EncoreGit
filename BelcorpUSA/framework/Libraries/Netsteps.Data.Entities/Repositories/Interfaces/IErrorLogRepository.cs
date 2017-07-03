using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business;

namespace NetSteps.Data.Entities.Repositories
{
	public partial interface IErrorLogRepository : ISearchRepository<ErrorLogSearchParameters, PaginatedList<ErrorLog>>
	{
		PaginatedList<ErrorLog> Search(ErrorLogSearchParameters searchParameters, string connectionString);
	}
}
