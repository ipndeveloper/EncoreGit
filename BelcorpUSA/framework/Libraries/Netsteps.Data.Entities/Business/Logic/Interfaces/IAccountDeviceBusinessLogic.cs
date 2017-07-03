using System.Collections.Generic;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business.Logic.Interfaces
{
    public partial interface IAccountDeviceBusinessLogic
    {
        List<AccountDevice> GetDevicesForNewsNotifications(IAccountDeviceRepository repository, int newsID, int maxResults);

		AccountDevice Load(IAccountDeviceRepository repository, string deviceid);
    }
}
