using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Server;
using System.Data.SqlTypes;

namespace NetSteps.Sql.CacheNotifications
{
	public class Notifier
	{
		public static INotificationFactory NotificationFactory { get; set; }

		static Notifier()
		{
			NotificationFactory = new DefaultNotificationFactory();
		}

		/// <summary>
		/// Used to send notifications to Clients listening for object change events.
		/// </summary>
		/// <param name="recipients">A list of recipients to send the notification to. 
		///		<example>
		///			Format: host:port 
		///			127.0.0.1:65000
		///			localhost:500
		///			some.client.net:2500
		///		</example>
		/// </param>
		/// <param name="messageKind">The Kind of message being sent</param>
		/// <param name="identityKind">The Kind of identies being sent</param>
		/// <param name="name">A name used to reference to whom the identies belong</param>
		/// <param name="ids">A list of identities for which this notification is about</param>
		public static void Notify(IEnumerable<string> recipients, IEnumerable<string> contextKeys, NotificationMessageKind messageKind, NotificationIdentityKind identityKind, IEnumerable<string> ids)
		{
			if (recipients == null || !recipients.Any()
				|| contextKeys == null || !contextKeys.Any()
				|| ids == null || !ids.Any())
			{
				return;
			}

			INotificationSender sender = NotificationFactory.CreateNotificationSender();
			sender.Send(recipients, contextKeys, messageKind, identityKind, ids);
		}
	}
}
