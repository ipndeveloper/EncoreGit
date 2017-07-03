
namespace NetSteps.Data.Entities.Repositories
{
	public partial interface IStoreFrontRepository
	{
		StoreFront GetStoreFrontWithInventory(int storeFrontId);
	}
}
