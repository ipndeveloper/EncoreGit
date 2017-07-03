using NetSteps.Core.Cache;
using NetSteps.Encore.Core.Log;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.Base
{
	public abstract class CommissionsActiveMruCacheAdapter<K, R> : ICache<K, R>
		where R : class
	{
		protected readonly ICache<K, R> Cache;

		public CommissionsActiveMruCacheAdapter()
		{
			Cache = InitializeCache();
		}

		protected abstract ICache<K, R> InitializeCache();
		public ICacheAxis<K, AK, R> CreateAxis<AK>(string name, Encore.Core.Representation.IRepresentation<R, AK> transform)
		{
			return Cache.CreateAxis<AK>(name, transform);
		}

		public void FlushAll()
		{
			Cache.FlushAll();
		}

		public ICacheAxis<K, AK, R> GetAxis<AK>(string name)
		{
			return GetAxis<AK>(name);
		}

		public string Name
		{
			get { return Cache.Name; }
		}

		public bool TryAdd(K key, R value)
		{
			return Cache.TryAdd(key, value);
		}

		public bool TryGet(K key, out R value)
		{
			return Cache.TryGet(key, out value);
		}

		public bool TryGetOnAxis<AK>(string axisName, AK axisKey, out R value)
		{
			return Cache.TryGetOnAxis<AK>(axisName, axisKey, out value);
		}

		public bool TryRemove(K key, out R removed)
		{
			return Cache.TryRemove(key, out removed);
		}

		public bool TryUpdate(K key, R value, R expected)
		{
			return Cache.TryUpdate(key, value, expected);
		}

		public void Dispose()
		{
			Cache.Dispose();
		}
	}
}
