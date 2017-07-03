using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Sql.CacheNotifications
{
	public interface INotificationFactory
	{
		INotificationSender CreateNotificationSender();
	}
}
