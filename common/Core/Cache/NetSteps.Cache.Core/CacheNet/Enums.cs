
namespace NetSteps.Core.Cache.CacheNet
{
	/// <summary>
	/// Indicates CacheNet message kinds
	/// </summary>
	public enum CacheNetMessageKind
	{
		/// <summary>
		/// None.
		/// </summary>
		None = 0,
		/// <summary>
		/// Indicates the message contains expiration(s) by ID.
		/// </summary>
		ExpirationById = 1,

		//ExpirationMatch = 2,

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
	/// Indicates identity kinds used in CacheNet
	/// </summary>
	public enum CacheNetIdentityKind
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
