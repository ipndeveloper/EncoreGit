using System.Linq;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class ShippingOrderTypeRepository
	{
		public TrackableCollection<ShippingOrderType> LoadAllByOrderTypeId(int orderTypeId)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (var context = CreateContext())
				{
					return context.ShippingOrderTypes.Where(s => s.OrderTypeID == orderTypeId).ToTrackableCollection();
				}
			});
		}
	}
}
