using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Sql.CacheNotifications
{
	public interface INotificationSender
	{
		void Send(IEnumerable<string> recipients, IEnumerable<string> contextKeys, NotificationMessageKind messageKind, NotificationIdentityKind identityKind, IEnumerable<string> ids);
	}
}
