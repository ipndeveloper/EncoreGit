using System;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business.Logic
{
	public partial class PickupPointBusinessLogic
	{
		public PickupPoint LoadByAddressID(IPickupPointRepository pickupPointRepository, int addressID)
		{
			try
			{
				return pickupPointRepository.GetPickupPoint(addressID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

	}
}
