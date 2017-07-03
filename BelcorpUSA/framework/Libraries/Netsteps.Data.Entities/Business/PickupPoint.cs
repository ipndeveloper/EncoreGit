using System;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities
{
	public partial class PickupPoint
	{
		public static PickupPoint LoadByAddressID(int addressID)
		{
			try
			{
					return BusinessLogic.LoadByAddressID(Repository, addressID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}
	}
}
