
namespace NetSteps.Data.Entities.Repositories
{
	public partial interface IPickupPointRepository
	{
		PickupPoint GetPickupPoint(int addressID);
	}
}
