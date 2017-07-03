using System.Collections.Generic;

namespace NetSteps.Data.Entities.Repositories
{
    public partial interface IAccountDeviceRepository
    {
        List<AccountDevice> GetDevicesForNewsNotifications(int newsID, int maxResults);

		AccountDevice Load(string deviceid);
    }
}
