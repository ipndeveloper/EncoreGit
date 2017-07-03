using System;

namespace NetSteps.Core.Cache
{
	/// <summary>
	/// Interface for accessing cache items on an axis other than the
	/// item's key.
	/// </summary>
	/// <typeparam name="K">cache's key type K</typeparam>
	/// <typeparam name="AK">axis key type AK</typeparam>
	/// <typeparam name="R">representation type R</typeparam>
	public interface ICacheAxis<K, AK, R> : IDisposable
        where R : class
    {
		/// <summary>
		/// Tries to get an item.
		/// </summary>
		/// <param name="axisKey">the item's axis key</param>
		/// <param name="value">variable where the item will be returned upon success</param>
		/// <returns>true if the item was retrieved; otherwise false</returns>
		bool TryGet(AK axisKey, out R value);

		/// <summary>
		/// Gets the cache over which this axis operates.
		/// </summary>
		ICache<K, R> Cache { get; }
	}

	/// <summary>
	/// Interface for objects that resolve items that are missing from the cache
	/// on an axis other than the item's key.
	/// </summary>
	/// <typeparam name="K">key type K</typeparam>
	/// <typeparam name="AK">axis key type AK</typeparam>
	/// <typeparam name="R">representation type R</typeparam>
	public interface ICacheAxisItemResolver<K, AK, R>
	{
		/// <summary>
		/// Gets the number of items resolved.
		/// </summary>
		int ResolveCount { get; }

		/// <summary>
		/// Tries to resolve an item that is missing from the cache.
		/// </summary>
		/// <param name="key">the item's key</param>
		/// <param name="value">variable to hold the item upon success</param>
		/// <returns>the resolution kind</returns>
		/// <see cref="ResolutionKind"/>
		ResolutionKind TryResolve(AK key, out R value);
	}
}
