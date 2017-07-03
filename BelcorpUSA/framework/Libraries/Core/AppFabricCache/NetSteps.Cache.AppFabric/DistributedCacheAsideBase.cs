using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Core.Cache;
using NetSteps.Encore.Core.Log;
using Microsoft.ApplicationServer.Caching;
using System.Diagnostics;
using NetSteps.Core.Cache.Config;
using System.Diagnostics.Contracts;

namespace NetSteps.Cache.AppFabric
{
	public abstract class DistributedCacheAsideBase<K, R> : ICacheMany<K, R>
		where R : class
	{
		#region Fields

		private static DataCacheFactory __dataCacheFactory;
		private static DataCache __dataCache;

		#endregion

		#region Propeties

		public string Name { get; private set; }

		public TimeSpan ItemLifeSpan { get; private set; }

		protected abstract ILogSink Log { get; }

		protected DataCache Cache { get { return __dataCache; } }

		#endregion

		#region Constructors

		static DistributedCacheAsideBase()
		{
			__dataCacheFactory = new DataCacheFactory();
			__dataCache = __dataCacheFactory.GetDefaultCache();
		}

		public DistributedCacheAsideBase(string cacheName)
			: this(cacheName, null)
		{ }

		public DistributedCacheAsideBase(string cacheName, MruCacheOptions options)
		{
			Contract.Requires<ArgumentNullException>(cacheName != null);
			Contract.Requires<ArgumentException>(cacheName.Length > 0);

			Name = cacheName;

			var resolvedOptions = options ?? CacheConfigSection.Current.NamedOrDefaultOptions<R>(Name);

			ItemLifeSpan = resolvedOptions.CacheItemLifespan;
		}

		#endregion

		#region Methods

		protected string GetKey(K key)
		{
			if (key is string || typeof(K).IsPrimitive)
			{
				return Convert.ToString(key);
			}
			else
			{
				return Convert.ToString(key.GetHashCode());
			}
		}

		public void Dispose()
		{ }

		public void FlushAll()
		{
			throw new NotImplementedException();
		}

		public abstract bool TryAdd(K key, R value);

		public abstract bool TryGet(K key, out R value);

		public abstract bool TryRemove(K key, out R removed);

		public abstract bool TryUpdate(K key, R value, R expected);

		protected virtual string CreateCacheKey(string forKey)
		{
			return String.Concat(Name, "::", forKey);
		}

		public virtual bool TryAddAny(IEnumerable<KeyValuePair<K, R>> values)
		{
			bool result = false;
			foreach (var item in values)
				result |= TryAdd(item.Key, item.Value);
			return result;
		}

		public virtual bool TryAddMany(IEnumerable<KeyValuePair<K, R>> values, out IEnumerable<K> failedKeys)
		{
			failedKeys = new List<K>();
			foreach (var item in values)
			{
				if (!TryAdd(item.Key, item.Value))
					((List<K>)failedKeys).Add(item.Key);
			}
			return !failedKeys.Any();
		}

		public virtual bool TryGetMany(IEnumerable<K> keys, out IEnumerable<R> values)
		{
			values = null;
			List<K> missedKeys = new List<K>();
			List<R> results = new List<R>();

			foreach (K k in keys)
			{
				R item;
				if (TryGet(k, out item))
					results.Add(item);
				else
					missedKeys.Add(k);
			}

			if (results.Count() == keys.Count())
			{
				values = results;
				return true;
			}

			IEnumerable<KeyValuePair<K, R>> resolvedItems;
			var rkind = TryResolveManyMissingItems(missedKeys, out resolvedItems);
			if (rkind == ResolutionManyKind.Created)
			{
				foreach (var resolved in resolvedItems)
				{
					if (Log.IsLogging(SourceLevels.Verbose))
						Log.Verbose(() => new { Message = "Resolved item via resolver", Key = resolved.Key });

					TryAdd(resolved.Key, resolved.Value);
					results.Add(resolved.Value);
				}
			}

			values = results;

			return rkind.HasFlag(ResolutionManyKind.Resolved);
		}

		public virtual bool TryGetAny(IEnumerable<K> keys, out IEnumerable<R> values)
		{
			TryGetMany(keys, out values);
			return values != null && values.Any();
		}

		public virtual bool TryRemoveMany(IEnumerable<K> keys, out IEnumerable<R> removed)
		{
			removed = new List<R>();
			foreach (K key in keys)
			{
				R item;
				if (TryRemove(key, out item))
					((List<R>)removed).Add(item);
			}
			return removed.Count() == keys.Count();
		}

		public virtual bool TryUpdateMany(IEnumerable<KeyValuePair<K, R>> values, IEnumerable<KeyValuePair<K, R>> expected)
		{
			bool result = true;

			foreach (var item in values)
			{
				R currentValue = item.Value;
				K currentKey = item.Key;
				R currentExpected = expected.Where(e => e.Key.Equals(currentKey)).Select(e => e.Value).FirstOrDefault();
				if (currentExpected == null)
					throw new ArgumentOutOfRangeException(String.Format("Key:{0} does not produce a value from the expected set.", currentKey));
				if (!TryUpdate(currentKey, currentValue, currentExpected))
					result = false;
			}


			return result;
		}

		public virtual ICacheAxis<K, AK, R> CreateAxis<AK>(string name, Encore.Core.Representation.IRepresentation<R, AK> transform)
		{
			throw new NotImplementedException();
		}

		public virtual ICacheAxis<K, AK, R> GetAxis<AK>(string name)
		{
			throw new NotImplementedException();
		}

		public virtual bool TryGetOnAxis<AK>(string axisName, AK axisKey, out R value)
		{
			throw new NotImplementedException();
		}

		protected virtual ResolutionKind TryResolveMissingItem(K key, out R value)
		{
			value = default(R);
			return ResolutionKind.None;
		}

		protected virtual ResolutionManyKind TryResolveManyMissingItems(IEnumerable<K> keys, out IEnumerable<KeyValuePair<K, R>> values)
		{
			values = default(IEnumerable<KeyValuePair<K, R>>);
			return ResolutionManyKind.None;
		}

		#endregion
	}
}
