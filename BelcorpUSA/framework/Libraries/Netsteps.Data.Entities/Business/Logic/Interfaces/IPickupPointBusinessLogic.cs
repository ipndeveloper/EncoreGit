using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business.Logic.Interfaces
{
	public partial interface IPickupPointBusinessLogic
	{
		PickupPoint LoadByAddressID(IPickupPointRepository pickupPointRepository, int addressID);
	}
}
