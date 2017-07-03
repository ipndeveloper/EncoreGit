using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.ApplicationServer.Caching;
using NetSteps.Core.Cache;
using NetSteps.Encore.Core.Representation;
using NetSteps.Encore.Core.Log;
using NetSteps.Core.Cache.Config;
using Newtonsoft.Json;
using System.Diagnostics.Contracts;
using System.Diagnostics;

namespace NetSteps.Cache.AppFabric
{
	public class BinaryDistributedCacheAside<K, R> : DistributedCacheAsideBase<K, R>
		where R : class//, IEquatable<R>
	{
		#region Fields

		static readonly ILogSink __log = typeof(BinaryDistributedCacheAside<K, R>).GetLogSink();

		#endregion

		#region Properties

		protected override ILogSink Log { get { return __log; } }

		#endregion

		#region Constructors


		public BinaryDistributedCacheAside(string cacheName)
			: this(cacheName, null)
		{ }

		public BinaryDistributedCacheAside(string cacheName, MruCacheOptions options)
			: base(cacheName, options)
		{ }

		#endregion

		#region Methods

		public override bool TryAdd(K key, R value)
		{
			Contract.Requires<ArgumentNullException>(value != null);

			string stringKey = GetKey(key);
			Contract.Assert(!String.IsNullOrWhiteSpace(stringKey), "key must not be null or empty.");

			bool result = false;
			string cacheKey = CreateCacheKey(stringKey);
			try
			{
				Cache.Add(cacheKey, value, ItemLifeSpan);
				result = true;
			}
			catch (DataCacheException dce)
			{
				if (dce.ErrorCode == DataCacheErrorCode.KeyAlreadyExists && Log.IsLogging(SourceLevels.Information))
					Log.Information("Failed to Add item to Cache '{0}'.  Message: {1}", cacheKey, dce.Message);
				else if (Log.IsLogging(SourceLevels.Warning))
					Log.Warning("Failed to Add item to Cache '{0}'.  Message: {1}", cacheKey, dce.Message);
			}
			return result;
		}

		public override bool TryGet(K key, out R value)
		{
			string stringKey = GetKey(key);
			Contract.Assert(!String.IsNullOrWhiteSpace(stringKey), "key must not be null or empty.");

			string cacheKey = CreateCacheKey(stringKey);
			value = Cache.Get(cacheKey) as R;

			if (value != null) return true;

			R resolved;
			var rkind = TryResolveMissingItem(key, out resolved);
			if (rkind == ResolutionKind.Created)
			{
				if (Log.IsLogging(SourceLevels.Verbose))
					Log.Verbose(() => new { Message = "Resolved item via resolver", Key = key });

				TryAdd(key, resolved);
			}
			value = resolved;
			return rkind.HasFlag(ResolutionKind.Resolved);
		}

		public override bool TryRemove(K key, out R removed)
		{
			string stringKey = GetKey(key);
			Contract.Assert(!String.IsNullOrWhiteSpace(stringKey), "key must not be null or empty.");

			bool result = false;
			string cacheKey = CreateCacheKey(stringKey);
			DataCacheItemVersion ver;
			removed = default(R);
			R item = Cache.Get(cacheKey, out ver) as R;
			if (item != null)
			{
				result = Cache.Remove(cacheKey, ver);
				if (result) removed = item;
			}
			return result;
		}

		public override bool TryUpdate(K key, R value, R expected)
		{
			/*
			 *	Will need to support  IEquatable<R> before this is viable... 
			 *	unfortunately, the majority of objects currently in the NetSteps Encore 
			 *	system do not, and adding this support will be a fairly monumental task...
			 */

			Contract.Requires<ArgumentNullException>(value != null);
			Contract.Requires<ArgumentNullException>(expected != null);
			Func<Boolean> equatableContract = () =>
			{
				Type valType = value.GetType();
				Type equatable = typeof(IEquatable<>).MakeGenericType(valType);
				return equatable.IsAssignableFrom(valType);
			};
			Contract.Requires<ArgumentException>(equatableContract());

			string stringKey = GetKey(key);
			Contract.Assert(!String.IsNullOrWhiteSpace(stringKey), "key must be convertable to string and should not result in null or empty value.");

			bool result = false;
			string cacheKey = CreateCacheKey(stringKey);
			DataCacheLockHandle handle;
			while (!result)
			{
				try
				{
					R existing = Cache.GetAndLock(cacheKey, TimeSpan.FromSeconds(5), out handle) as R;
					if (existing.Equals(expected))
					{
						Cache.PutAndUnlock(cacheKey, value, handle, ItemLifeSpan);
						result = true;
					}
					else break;
				}
				catch (DataCacheException dce)
				{
					if (dce.ErrorCode != DataCacheErrorCode.ObjectLocked)
					{
						if (Log.IsLogging(SourceLevels.Information))
							Log.Information("Failed to Update item in Cache '{0}'. Message: {1}", cacheKey, dce.Message);
						break;
					}
				}
			}

			return result;
		}

		#endregion
	}
}
