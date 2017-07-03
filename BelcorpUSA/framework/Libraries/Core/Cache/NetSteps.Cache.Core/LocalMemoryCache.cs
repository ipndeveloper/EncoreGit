using System.Collections.Concurrent;
using NetSteps.Encore.Core.Representation;
using NetSteps.Encore.Core;

namespace NetSteps.Core.Cache
{
	/// <summary>
	/// Local memory cache implementation of ICache
	/// </summary>
	/// <typeparam name="K">key type K</typeparam>
	/// <typeparam name="R">representation type R</typeparam>
	public sealed class LocalMemoryCache<K, R> : Disposable, ICache<K, R>
        where R: class
	{
		readonly ConcurrentDictionary<K, R> _cache = new ConcurrentDictionary<K, R>();

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		public LocalMemoryCache()
		{
		}

		/// <summary>
		/// Gets the cache's name.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Tries to add an item to the cache.
		/// </summary>
		/// <param name="key">the item's key</param>
		/// <param name="value">the item</param>
		/// <returns>true if successful; otherwise false</returns>
		public bool TryAdd(K key, R value)
		{
			return _cache.TryAdd(key, value);
		}

		/// <summary>
		/// Tries to update a cache item.
		/// </summary>
		/// <param name="key">the item's key</param>
		/// <param name="value">the updated item</param>
		/// <param name="expected">the cached item</param>
		/// <returns>true if successful; otherwise false</returns>
		public bool TryUpdate(K key, R value, R expected)
		{
			return _cache.TryUpdate(key, value, expected);
		}

		/// <summary>
		/// Tries to remove an item from the cache.
		/// </summary>
		/// <param name="key">the item's key</param>
		/// <param name="removed">variable to hold the removed item upon success</param>
		/// <returns>true if successful; otherwise false</returns>
		public bool TryRemove(K key, out R removed)
		{
			return _cache.TryRemove(key, out removed);
		}

		/// <summary>
		/// Tries to get an item from the cache.
		/// </summary>
		/// <param name="key">the item's key</param>
		/// <param name="value">variable to hold the item upon success</param>
		/// <returns>true if successful; otherwise false.</returns>
		public bool TryGet(K key, out R value)
		{
			return _cache.TryGetValue(key, out value);
		}

		/// <summary>
		/// Gets an existing item or adds an item to the cache.
		/// </summary>
		/// <param name="key">the item's key</param>
		/// <param name="value">the item to add if not already present</param>
		/// <returns>the item</returns>
		public R GetOrAdd(K key, R value)
		{
			return _cache.GetOrAdd(key, value);
		}

		/// <summary>
		/// Gets an existing item or adds an item to the cache.
		/// </summary>
		/// <param name="key">the item's key</param>
		/// <param name="factory">a factory that produces the item to add if not already present</param>
		/// <returns>the item</returns>
		public R GetOrAdd(K key, System.Func<K, R> factory)
		{
			return _cache.GetOrAdd(key, factory);
		}

		/// <summary>
		/// Adds an item to the cache if the key does not already exist, or updates the item already present in the cache.
		/// </summary>
		/// <param name="key">the item's key</param>
		/// <param name="value">the value to be added if not already present</param>
		/// <param name="updateFactory">a factory that updates the item already present</param>
		/// <returns>the added or updated item</returns>
		public R AddOrUpdate(K key, R value, System.Func<K, R, R> updateFactory)
		{
			return _cache.AddOrUpdate(key, value, updateFactory);
		}

		/// <summary>
		/// Adds an item to the cache if the key does not already exist, or updates the item already present in the cache.
		/// </summary>
		/// <param name="key">the item's key</param>
		/// <param name="valueFactory">a factory that produces the value to be added if not already present</param>
		/// <param name="updateFactory">a factory that updates the item already present</param>
		/// <returns>the added or updated item</returns>
		public R AddOrUpdate(K key, System.Func<K, R> valueFactory, System.Func<K, R, R> updateFactory)
		{
			return _cache.AddOrUpdate(key, valueFactory, updateFactory);
		}

		/// <summary>
		/// Flushes all items from the cache.
		/// </summary>
		public void FlushAll()
		{
			_cache.Clear();
		}

		/// <summary>
		/// Performs dispose logic.
		/// </summary>
		/// <param name="disposing">whether called from Dispose method</param>
		/// <returns>true if disposed as a result of this call</returns>
		protected override bool PerformDispose(bool disposing)
		{
			return true;
		}

		/// <summary>
		/// Creates a cache axis.
		/// </summary>
		/// <typeparam name="AK">axisKey type AK</typeparam>
		/// <param name="name">the axis name</param>
		/// <param name="transform">a transform for getting axis keys from representations</param>
		/// <returns>a cache axis</returns>
		public ICacheAxis<K, AK, R> CreateAxis<AK>(string name, IRepresentation<R, AK> transform)
		{
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// Gets a cache axis by name.
		/// </summary>
		/// <typeparam name="AK">axisKey type AK</typeparam>
		/// <param name="name">the axis name</param>
		/// <returns>a cache axis</returns>
		public ICacheAxis<K, AK, R> GetAxis<AK>(string name)
		{
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// Tries to get an item by an axis key.
		/// </summary>
		/// <typeparam name="AK">axis key type AK</typeparam>
		/// <param name="axisName">the name of the axis</param>
		/// <param name="axisKey">the axis key</param>
		/// <param name="value">variable where the item will be returned upon success</param>
		/// <returns>true if the item is present in the cache; otherwise false</returns>
		public bool TryGetOnAxis<AK>(string axisName, AK axisKey, out R value)
		{
			throw new System.NotImplementedException();
		}
	}
}
