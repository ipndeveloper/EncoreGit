
namespace NetSteps.Data.Entities.Repositories
{
	public partial interface IShippingRegionRepository
	{
		ShippingRegion LoadByName(string shippingRegionName);
	}
}
