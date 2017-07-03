using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace NetSteps.Core.Cache
{
	/// <summary>
	/// Interface for cache objects.
	/// </summary>
	/// <typeparam name="K">key type K</typeparam>
	/// <typeparam name="R">representation type R</typeparam>
	[ContractClass(typeof(CodeContracts.ContractForICacheMany<,>))]
	public interface ICacheMany<K, R> : ICache<K, R>
		where R : class
	{
		/// <summary>
		/// Tries to add items.
		/// </summary>
		/// <param name="values">The items</param>
		/// <param name="failedKeys">the keys of items that were not added</param>
		/// <returns>true if all added; otherwise false.</returns>
		bool TryAddMany(IEnumerable<KeyValuePair<K, R>> values, out IEnumerable<K> failedKeys);

		/// <summary>
		/// Tries to add items.
		/// </summary>
		/// <param name="values">The items</param>
		/// <returns>true if any added; otherwise false.</returns>
		bool TryAddAny(IEnumerable<KeyValuePair<K, R>> values);

		/// <summary>
		/// Tries to update the expected items.
		/// </summary>
		/// <param name="values">the items</param>
		/// <param name="expected">the expected values used for comparison</param>
		/// <returns>true if all of the items were equal to the expected values and replaced; otherwise false</returns>
		bool TryUpdateMany(IEnumerable<KeyValuePair<K, R>> values, IEnumerable<KeyValuePair<K, R>> expected);

		/// <summary>
		/// Tries to remove an item.
		/// </summary>
		/// <param name="keys">the item's keys</param>
		/// <param name="removed">variable where the removed items will be returned upon success</param>
		/// <returns>true if the items were removed; otherwise false</returns>
		bool TryRemoveMany(IEnumerable<K> keys, out IEnumerable<R> removed);

		/// <summary>
		/// Tries to get items.
		/// </summary>
		/// <param name="keys">the item's keys</param>
		/// <param name="values">variable where the items will be returned upon success</param>
		/// <returns>true if the items were retrieved; otherwise false</returns>
		bool TryGetMany(IEnumerable<K> keys, out IEnumerable<R> values);

		/// <summary>
		/// Tries to get any of the items.
		/// </summary>
		/// <param name="keys">the item's keys</param>
		/// <param name="values">variable where the items will be returned upon success</param>
		/// <returns>true if any of the items were retrieved; otherwise false</returns>
		bool TryGetAny(IEnumerable<K> keys, out IEnumerable<R> values);
	}

	/// <summary>
	/// Extensions over ICacheMany&lt;,>
	/// </summary>
	public static class ICacheManyExtensions
	{
		/// <summary>
		/// Gets item representations from the cache by the items' keys.
		/// </summary>
		/// <typeparam name="K">key type K</typeparam>
		/// <typeparam name="R">representation type R</typeparam>
		/// <param name="cache">the cache</param>
		/// <param name="keys">the item's key</param>
		/// <returns>the item's cached representation</returns>
		public static IEnumerable<R> GetMany<K, R>(this ICacheMany<K, R> cache, IEnumerable<K> keys)
			where R : class
		{
			IEnumerable<R> representations;
			return (cache.TryGetMany(keys, out representations)) ? representations : default(IEnumerable<R>);
		}

		/// <summary>
		/// Puts an item's representation into the cache.
		/// </summary>
		/// <typeparam name="K">key type K</typeparam>
		/// <typeparam name="R">representation type R</typeparam>
		/// <param name="cache">the cache</param>
		/// <param name="representations">the item's representation</param>
		/// <param name="keySelector">Func to select the key for a given item</param>
		/// <returns>the item's representation</returns>
		public static IEnumerable<R> PutMany<K, R>(this ICacheMany<K, R> cache, IEnumerable<R> representations, Func<R, K> keySelector)
			where R : class
		{
			var kvpa = representations.Select(r => new KeyValuePair<K, R>(keySelector(r), r)).ToArray();
			while (!cache.TryAddAny(kvpa))
			{
				IEnumerable<R> existing;
				var keys = kvpa.Select(tr => tr.Key).ToArray();
				if (cache.TryGetMany(keys, out existing))
				{
					if (cache.TryUpdateMany(kvpa, existing.Select(e => new KeyValuePair<K, R>(keySelector(e), e)).ToArray()))
					{
						break;
					}
				}
			}
			return representations;
		}

		/// <summary>
		/// Gets an item's representation from the cache by the item's key.
		/// </summary>
		/// <typeparam name="K">key type K</typeparam>
		/// <typeparam name="R">representation type R</typeparam>
		/// <param name="cache">the cache</param>
		/// <param name="key">the item's key</param>
		/// <returns>the item's cached representation</returns>
		public static R Get<K, R>(this ICacheMany<K, R> cache, K key)
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
		public static R Put<K, R>(this ICacheMany<K, R> cache, K key, R representation)
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

	namespace CodeContracts
	{
		[ContractClassFor(typeof(ICacheMany<,>))]
		abstract class ContractForICacheMany<K, R> : ICacheMany<K, R>
			where R : class
		{
			bool ICacheMany<K, R>.TryAddMany(IEnumerable<KeyValuePair<K, R>> values, out IEnumerable<K> failedKeys)
			{
				Contract.Requires<ArgumentNullException>(values != null);
				Contract.Requires<ArgumentOutOfRangeException>(values.Any());

				throw new NotImplementedException();
			}

			bool ICacheMany<K, R>.TryAddAny(IEnumerable<KeyValuePair<K, R>> values)
			{
				Contract.Requires<ArgumentNullException>(values != null);
				Contract.Requires<ArgumentOutOfRangeException>(values.Any());

				throw new NotImplementedException();
			}

			bool ICacheMany<K, R>.TryUpdateMany(IEnumerable<KeyValuePair<K, R>> values, IEnumerable<KeyValuePair<K, R>> expected)
			{
				Contract.Requires<ArgumentNullException>(values != null);
				Contract.Requires<ArgumentOutOfRangeException>(values.Any());
				Contract.Requires<InvalidOperationException>(expected.Count() == values.Count());

				throw new NotImplementedException();
			}

			bool ICacheMany<K, R>.TryRemoveMany(IEnumerable<K> keys, out IEnumerable<R> removed)
			{
				Contract.Requires<ArgumentNullException>(keys != null);
				Contract.Requires<ArgumentOutOfRangeException>(keys.Any());

				throw new NotImplementedException();
			}

			bool ICacheMany<K, R>.TryGetMany(IEnumerable<K> keys, out IEnumerable<R> values)
			{
				Contract.Requires<ArgumentNullException>(keys != null);
				Contract.Requires<ArgumentOutOfRangeException>(keys.Any());

				throw new NotImplementedException();
			}

			bool ICacheMany<K, R>.TryGetAny(IEnumerable<K> keys, out IEnumerable<R> values)
			{
				Contract.Requires<ArgumentNullException>(keys != null);
				Contract.Requires<ArgumentOutOfRangeException>(keys.Any());

				throw new NotImplementedException();
			}

			public string Name
			{
				get { throw new NotImplementedException(); }
			}

			public bool TryAdd(K key, R value)
			{
				throw new NotImplementedException();
			}

			public bool TryUpdate(K key, R value, R expected)
			{
				throw new NotImplementedException();
			}

			public bool TryRemove(K key, out R removed)
			{
				throw new NotImplementedException();
			}

			public bool TryGet(K key, out R value)
			{
				throw new NotImplementedException();
			}

			public void FlushAll()
			{
				throw new NotImplementedException();
			}

			public ICacheAxis<K, AK, R> CreateAxis<AK>(string name, Encore.Core.Representation.IRepresentation<R, AK> transform)
			{
				throw new NotImplementedException();
			}

			public ICacheAxis<K, AK, R> GetAxis<AK>(string name)
			{
				throw new NotImplementedException();
			}

			public bool TryGetOnAxis<AK>(string axisName, AK axisKey, out R value)
			{
				throw new NotImplementedException();
			}

			public void Dispose()
			{
				throw new NotImplementedException();
			}
		}
	}
}
