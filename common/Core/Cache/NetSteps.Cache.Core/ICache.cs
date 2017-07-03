using System.Collections.Concurrent;
using System;
using NetSteps.Encore.Core.Representation;

namespace NetSteps.Core.Cache
{
	
	/// <summary>
	/// Interface for cache objects.
	/// </summary>
	/// <typeparam name="K">key type K</typeparam>
	/// <typeparam name="R">representation type R</typeparam>
	public interface ICache<K, R> : IDisposable
        where R: class
	{
		/// <summary>
		/// The Cache's name. Names must be unique within a process in order 
		/// to differentiate the performance counters that provide performance metrics.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Tries to add an item.
		/// </summary>
		/// <param name="key">the item's key</param>
		/// <param name="value">the item's representation</param>
		/// <returns>true if added; otherwise false.</returns>
		bool TryAdd(K key, R value);

		/// <summary>
		/// Tries to update an expected item.
		/// </summary>
		/// <param name="key">the item's key</param>
		/// <param name="value">the item</param>
		/// <param name="expected">the expected value used for comparison</param>
		/// <returns>true if the item was equal to the expected value and replaced; otherwise false</returns>
		bool TryUpdate(K key, R value, R expected);

		/// <summary>
		/// Tries to remove an item.
		/// </summary>
		/// <param name="key">the item's key</param>
		/// <param name="removed">variable where the removed item will be returned upon success</param>
		/// <returns>true if the item was removed; otherwise false</returns>
		bool TryRemove(K key, out R removed);

		/// <summary>
		/// Tries to get an item.
		/// </summary>
		/// <param name="key">the item's key</param>
		/// <param name="value">variable where the item will be returned upon success</param>
		/// <returns>true if the item was retrieved; otherwise false</returns>
		bool TryGet(K key, out R value);		

		/// <summary>
		/// Flushes all items from the cache.
		/// </summary>
		void FlushAll();

		/// <summary>
		/// Creates a cache axis.
		/// </summary>
		/// <typeparam name="AK">axisKey type AK</typeparam>
		/// <param name="name">the axis name</param>
		/// <param name="transform">a transform for getting axis keys from representations</param>
		/// <returns>a cache axis</returns>
		ICacheAxis<K, AK, R> CreateAxis<AK>(string name, IRepresentation<R, AK> transform);

		/// <summary>
		/// Gets a cache axis by name.
		/// </summary>
		/// <typeparam name="AK">axisKey type AK</typeparam>
		/// <param name="name">the axis name</param>
		/// <returns>a cache axis</returns>
		ICacheAxis<K, AK, R> GetAxis<AK>(string name);

		/// <summary>
		/// Tries to get an item by an axis key.
		/// </summary>
		/// <typeparam name="AK">axis key type AK</typeparam>
		/// <param name="axisName">the name of the axis</param>
		/// <param name="axisKey">the axis key</param>
		/// <param name="value">variable where the item will be returned upon success</param>
		/// <returns>true if the item is present in the cache; otherwise false</returns>
		bool TryGetOnAxis<AK>(string axisName, AK axisKey, out R value);
	}

	/// <summary>
	/// Extensions over ICache&lt;,>
	/// </summary>
	public static class ICacheExtensions
	{
		/// <summary>
		/// Gets an item's representation from the cache by the item's key.
		/// </summary>
		/// <typeparam name="K">key type K</typeparam>
		/// <typeparam name="R">representation type R</typeparam>
		/// <param name="cache">the cache</param>
		/// <param name="key">the item's key</param>
		/// <returns>the item's cached representation</returns>
		public static R Get<K, R>(this ICache<K, R> cache, K key)
            where R : class
		{
			R representation;
			return (cache.TryGet(key, out representation)) ? representation : default(R);
		}

		/// <summary>
		/// Puts an item's representation into the cache.
		/// </summary>
		/// <typeparam name="K">key type K</typeparam>
		/// <typeparam name="R">representation type R</typeparam>
		/// <param name="cache">the cache</param>
		/// <param name="key">the item's key</param>
		/// <param name="representation">the item's representation</param>
		/// <returns>the item's representation</returns>
		public static R Put<K, R>(this ICache<K, R> cache, K key, R representation)
            where R : class
		{
            while (!cache.TryAdd(key, representation))
            {
                R existing;
                if (cache.TryGet(key, out existing))
                {
                    if (cache.TryUpdate(key, representation, existing))
                    {
                        break;
                    }
                }
            }
            return representation;
		}
	}
}
