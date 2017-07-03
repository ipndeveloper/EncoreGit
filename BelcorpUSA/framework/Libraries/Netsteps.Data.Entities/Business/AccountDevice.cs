using System.Collections.Generic;

namespace NetSteps.Data.Entities
{
    public partial class AccountDevice
    {
        public static List<AccountDevice> GetDevicesForNewsNotifications(int newsID, int maxResults)
        {
            return BusinessLogic.GetDevicesForNewsNotifications(Repository, newsID, maxResults);
        }

		public static AccountDevice Load(string deviceid)
		{
			return BusinessLogic.Load(Repository, deviceid);
		}
    }
}
