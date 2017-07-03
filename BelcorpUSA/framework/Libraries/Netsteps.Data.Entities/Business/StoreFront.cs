using System;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities
{
	public partial class StoreFront
	{
	
		protected internal static StoreFront GetStoreFrontWithInventory(int storeFrontId)
		{
			try
			{
				return Repository.GetStoreFrontWithInventory(storeFrontId);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}
	}
}
