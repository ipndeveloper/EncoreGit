using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Sql.CacheNotifications.Tests.Net
{
	public class TestRecievedMessage
	{
		public int DataLength;
		public List<string> ContextKeys = new List<string>();
		public NotificationMessageKind MessageKind;
		public NotificationIdentityKind IdentityKind;
		public List<object> Ids = new List<object>();
	}
}
