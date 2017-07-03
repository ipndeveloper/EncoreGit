
namespace NetSteps.Data.Entities.Repositories
{
	public partial interface IShippingOrderTypeRepository
	{
		TrackableCollection<ShippingOrderType> LoadAllByOrderTypeId(int orderTypeId);
	}
}
