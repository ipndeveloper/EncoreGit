using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Sql.CacheNotifications
{
	public class DefaultNotificationFactory : INotificationFactory
	{
		public INotificationSender CreateNotificationSender()
		{
			return new Net.UdpNotificationSender();
		}
	}
}
