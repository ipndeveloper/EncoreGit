using System;
using System.Collections.Generic;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business
{
	public partial class GlobalSearch
	{
		public static List<GlobalSearchData> Search(int accountID, int siteID, string query, int? langaugeID = null)
		{
			try
			{
				return new GlobalSearchRepository().Search(accountID, siteID, query, langaugeID);
			}
			catch (Exception ex)
			{
					
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

	}
}
