using System;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities
{
	public partial class ShippingRegion
	{
		#region Methods

		public static ShippingRegion LoadByName(string shippingRegionName)
		{
			try
			{
				return Repository.LoadByName(shippingRegionName);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		#endregion
	}
}
