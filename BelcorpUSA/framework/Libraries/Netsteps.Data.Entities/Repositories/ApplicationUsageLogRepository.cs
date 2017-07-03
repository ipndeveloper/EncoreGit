using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Common.Interfaces;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class ApplicationUsageLogRepository : BaseRepository<ApplicationUsageLog, int, NetStepsEntities>, IApplicationUsageLogRepository, IDefaultImplementation
	{
		#region Methods
		public void SaveAll(IEnumerable<ApplicationUsageLog> applicationUsageLogs)
		{
			ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					foreach (var applicationUsageLog in applicationUsageLogs)
						context.ApplicationUsageLogs.AddObject(applicationUsageLog);

					context.SaveChanges();

					foreach (var applicationUsageLog in applicationUsageLogs)
						applicationUsageLog.AcceptChanges();
				}
			});
		}

		public PaginatedList<ApplicationUsageLog> Search(ApplicationUsageLogSearchParameters searchParameters)
		{
			return Search(searchParameters, NetStepsEntities.ConnectionString);
		}
		public PaginatedList<ApplicationUsageLog> Search(ApplicationUsageLogSearchParameters searchParameters, string connectionString)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = new NetStepsEntities(connectionString))
				{
					PaginatedList<ApplicationUsageLog> results = new PaginatedList<ApplicationUsageLog>(searchParameters);

					var matchingItems = from a in context.ApplicationUsageLogs
										select a;

					if (!searchParameters.AssemblyName.IsNullOrEmpty())
						matchingItems = from a in matchingItems
										where a.MachineName.Contains(searchParameters.AssemblyName)
										select a;

					if (!searchParameters.MachineName.IsNullOrEmpty())
						matchingItems = from a in matchingItems
										where a.MachineName.Contains(searchParameters.MachineName)
										select a;

					if (!searchParameters.ClassName.IsNullOrEmpty())
						matchingItems = from a in matchingItems
										where a.MachineName.Contains(searchParameters.ClassName)
										select a;

					if (!searchParameters.MethodName.IsNullOrEmpty())
						matchingItems = from a in matchingItems
										where a.MachineName.Contains(searchParameters.MethodName)
										select a;

					if (searchParameters.ApplicationID.HasValue)
						matchingItems = from a in matchingItems
										where a.ApplicationID == searchParameters.ApplicationID.Value
										select a;

					if (searchParameters.UserID.HasValue)
						matchingItems = from a in matchingItems
										where a.UserID == searchParameters.UserID.Value
										select a;

					matchingItems = matchingItems.ApplyDateRangeFilters("UsageDateUTC", searchParameters);

					if (searchParameters.WhereClause != null)
						matchingItems = matchingItems.Where(searchParameters.WhereClause);

					matchingItems = matchingItems.ApplyOrderByFilters(searchParameters, a => a.UsageDateUTC, context);

					// TotalCount must be set before applying Pagination - JHE
					results.TotalCount = matchingItems.Count();

					matchingItems = matchingItems.ApplyPagination(searchParameters);

					var logs = from a in matchingItems
							   select a;

					foreach (var a in logs.ToList())
						results.Add(a);

					return results;
				}
			});
		}
		#endregion
	}
}