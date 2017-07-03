using System;
using System.Collections.Generic;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities
{
	public partial class ShippingMethod
	{
		#region Methods
		public static List<int> LoadAllTranslationIds()
		{
			try
			{
				return Repository.LoadAllTranslationIds();
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static ShippingMethod LoadByName(string name)
		{
			try
			{
				return Repository.LoadByName(name);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}
		#endregion
	}
}
