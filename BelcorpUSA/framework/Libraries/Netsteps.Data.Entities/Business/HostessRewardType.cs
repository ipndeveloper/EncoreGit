using System;
using System.Collections.Generic;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities
{
	public partial class HostessRewardType
	{
		public static List<int> GetAvailableCatalogs(IEnumerable<int> rewardTypes)
		{
			try
			{
				return Repository.GetAvailableCatalogs(rewardTypes);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}
	}
}
