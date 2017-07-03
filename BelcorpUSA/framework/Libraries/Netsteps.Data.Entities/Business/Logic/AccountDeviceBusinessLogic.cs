using System;
using System.Collections.Generic;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business.Logic
{
    public partial class AccountDeviceBusinessLogic
    {
        public List<AccountDevice> GetDevicesForNewsNotifications(IAccountDeviceRepository repository, int newsID, int maxResults)
        {
            try
            {
                return repository.GetDevicesForNewsNotifications(newsID, maxResults);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

		public AccountDevice Load(IAccountDeviceRepository repository, string deviceid)
		{
			try
			{
				return repository.Load(deviceid);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}
    }
}
