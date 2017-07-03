using System;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Server;
using System.Data.SqlTypes;
using NetSteps.Sql.CacheNotifications;
using System.Security;

public class SqlNotifier
{
	/// <summary>
	/// Used to send notifications to Clients listening for object change events from within SQL server.
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
	/// <param name="listSeperator">The character used as the separator for the two list parameters, <paramref name="recipients"/> and <paramref name="ids"/></param>
	[SqlProcedure]
	public static void Notify(SqlString recipients, SqlString contextKeys, SqlInt32 messageKind, SqlInt32 identityKind, SqlString ids, SqlString listSeperator)
	{
		if (recipients == null || contextKeys == null || ids == null) return;
		if (recipients.IsNull || messageKind.IsNull || identityKind.IsNull || contextKeys.IsNull || ids.IsNull) return;

		ThreadPool.QueueUserWorkItem(o =>
			{
				try
				{
					char sep = listSeperator.IsNull ? ',' : listSeperator.Value.First();

					string[] recList = recipients.Value.Split(sep);
					int msgKind = messageKind.Value;
					int idKind = identityKind.Value;
					string[] ctxKeys = contextKeys.Value.Split(sep);
					string[] idList = ids.Value.Split(sep);

					Notifier.Notify(recList, ctxKeys, (NotificationMessageKind)msgKind, (NotificationIdentityKind)idKind, idList);
				}
				catch (Exception e) 
				{
					string source = "Sql Cache Notifier";
					string log = "Application";
					if (!EventLog.SourceExists(source))
					{
						EventLog.CreateEventSource(source, log);
					}
					EventLog.WriteEntry(source, e.ToString());
				}
			});
	}
}
