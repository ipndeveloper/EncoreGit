using System;
using System.Collections.Generic;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business.Logic
{
	public partial class StatisticBusinessLogic
	{
		public virtual void SaveBatch(IStatisticRepository repository, IEnumerable<Statistic> items)
		{
			try
			{
				repository.SaveBatch(items);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}
	}
}
