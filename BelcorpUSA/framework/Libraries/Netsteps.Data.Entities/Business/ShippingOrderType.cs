using System;

using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities
{
	public partial class ShippingOrderType
	{
		public static TrackableCollection<ShippingOrderType> LoadAllByOrderTypeId(int orderTypeId)
		{
			try
			{
				return Repository.LoadAllByOrderTypeId(orderTypeId);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}
	}
}
