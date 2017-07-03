using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using Microsoft.ApplicationServer.Caching;
using NetSteps.Core.Cache;
using NetSteps.Encore.Core.Log;
using Newtonsoft.Json;

namespace NetSteps.Cache.AppFabric
{
	public class JsonDistributedCacheAside<K, R> : DistributedCacheAsideBase<K, R>
		where R : class
	{
		#region Fields

		static readonly ILogSink __log = typeof(JsonDistributedCacheAside<K, R>).GetLogSink();

		#endregion

		#region Properties

		protected override ILogSink Log { get { return __log; } }

		protected JsonSerializerSettings JsonSettings { get; set; }

		#endregion

		#region Constructors

		public JsonDistributedCacheAside(string cacheName)
			: this(cacheName, null)
		{ }

		public JsonDistributedCacheAside(string cacheName, MruCacheOptions options)
			: base(cacheName, options)
		{
			JsonSerializerSettings jsonSettings = new JsonSerializerSettings()
			{
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
				MissingMemberHandling = MissingMemberHandling.Error,
				DateFormatHandling = DateFormatHandling.IsoDateFormat,
				TypeNameHandling = TypeNameHandling.All,
				TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Full

			};

			JsonSettings = jsonSettings;
		}

		#endregion

		#region Methods

		public override bool TryAdd(K key, R value)
		{
			Contract.Requires<ArgumentNullException>(value != null);

			string stringKey = GetKey(key);
			Contract.Assert(!String.IsNullOrWhiteSpace(stringKey), "key must not be null or empty.");

			bool result = false;
			string cacheKey = CreateCacheKey(stringKey);

			string jsonValue = JsonConvert.SerializeObject(value, JsonSettings);
			try
			{
				Cache.Add(cacheKey, jsonValue, ItemLifeSpan);
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
			string item = (string)Cache.Get(cacheKey);

			if (!String.IsNullOrWhiteSpace(item))
			{
				value = JsonConvert.DeserializeObject<R>(item, JsonSettings);
				return true;
			}

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
			string item = (string)Cache.Get(cacheKey);
			if (item != null)
			{
				removed = JsonConvert.DeserializeObject<R>(item, JsonSettings);
				result = Cache.Remove(cacheKey);
			}
			else
				removed = default(R);

			return result;
		}

		public override bool TryUpdate(K key, R value, R expected)
		{
			Contract.Requires<ArgumentNullException>(value != null);
			Contract.Requires<ArgumentNullException>(expected != null);

			string stringKey = GetKey(key);
			Contract.Assert(!String.IsNullOrWhiteSpace(stringKey), "key must not be null or empty.");

			bool result = false;
			string cacheKey = CreateCacheKey(stringKey);
			string jsonValue = JsonConvert.SerializeObject(value, JsonSettings);
			string jsonExpected = JsonConvert.SerializeObject(expected, JsonSettings);
			DataCacheLockHandle handle;
			while (!result)
			{
				try
				{
					string jsonExisting = (string)Cache.GetAndLock(cacheKey, TimeSpan.FromSeconds(5), out handle);
					if (jsonExisting.Equals(jsonExpected))
					{
						Cache.PutAndUnlock(cacheKey, jsonValue, handle, ItemLifeSpan);
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
