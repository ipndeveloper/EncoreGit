using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Sql.CacheNotifications
{
	/// <summary>
	/// Indicates message kinds
	/// </summary>
	public enum NotificationMessageKind
	{
		/// <summary>
		/// None.
		/// </summary>
		None = 0,

		/// <summary>
		/// Indicates the message contains expiration(s) by ID.
		/// </summary>
		ExpirationById = 1,

		/// <summary>
		/// Heartbeat sent by cache-net nodes periodically.
		/// </summary>
		Heartbeat = 100,

		/// <summary>
		/// Reply to heartbeat; communicates known cache-net nodes
		/// and their liveliness.
		/// </summary>
		Gossip = 101,
	}

	/// <summary>
	/// Indicates identity kinds
	/// </summary>
	public enum NotificationIdentityKind
	{
		/// <summary>
		/// Indicates the identity kind is Int32
		/// </summary>
		Int32 = 0,

		/// <summary>
		/// Indicates the identity kind is Int64
		/// </summary>
		Int64 = 1,

		/// <summary>
		/// Indicates the identity kind is String
		/// </summary>
		String = 2,

		/// <summary>
		/// Indicates the identity kind is Guid
		/// </summary>
		Guid = 3,
	}
}
