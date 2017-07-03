using System.Collections.Generic;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business;

namespace NetSteps.Data.Entities.Repositories
{
	public partial interface IApplicationUsageLogRepository
	{
		void SaveAll(IEnumerable<ApplicationUsageLog> applicationUsageLogs);
		PaginatedList<ApplicationUsageLog> Search(ApplicationUsageLogSearchParameters searchParameters);
		PaginatedList<ApplicationUsageLog> Search(ApplicationUsageLogSearchParameters searchParameters, string connectionString);
	}
}
