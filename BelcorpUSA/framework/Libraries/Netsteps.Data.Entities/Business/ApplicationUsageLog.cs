using System;
using System.Collections.Generic;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities
{
	public partial class ApplicationUsageLog
	{
		#region Methods
		public static PaginatedList<ApplicationUsageLog> Search(ApplicationUsageLogSearchParameters searchParameters)
		{
			try
			{
				var logs = Repository.Search(searchParameters);
				return logs;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}
		public static PaginatedList<ApplicationUsageLog> Search(ApplicationUsageLogSearchParameters searchParameters, string connectionString)
		{
			try
			{
				var logs = Repository.Search(searchParameters, connectionString);
				return logs;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public void SaveAll(IEnumerable<ApplicationUsageLog> applicationUsageLogs)
		{
			Repository.SaveAll(applicationUsageLogs);
		}
		#endregion
	}
}
