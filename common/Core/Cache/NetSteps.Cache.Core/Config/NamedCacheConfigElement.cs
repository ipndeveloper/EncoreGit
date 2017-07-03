using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.Contracts;
using System.Linq;
using NetSteps.Encore.Core;
using NetSteps.Encore.Core.Reflection;

namespace NetSteps.Core.Cache.Config
{
	/// <summary>
	/// Contains configuration for a named cache.
	/// </summary>
	public class NamedCacheConfigElement : ConfigurationElement
	{
		const string PropertyName_name = "name";
		const string PropertyName_concurrentEvictionFactor = "concurrentEvictionFactor";
		const string PropertyName_synchronizedEvictionFactor = "synchronizedEvictionFactor";
		const string PropertyName_evictionsPerSynchronizedWorker = "evictionsPerSynchronizedWorker";
		const string PropertyName_cacheItemLifespan = "cacheItemLifespan";
		const string PropertyName_cacheDepth = "cacheDepth";
		const string PropertyName_contextKeys = "contextKeys";
		const string PropertyName_fullActive = "fullActive";

		/// <summary>
		/// Default concurrent eviction factor.
		/// </summary>
		public const double CDefaultConcurrentEvictionFactor = 1.2;
		/// <summary>
		/// Default synchronized eviction factor.
		/// </summary>
		public const double CDefaultSynchronizedEvictionFactor = 2.5;
		/// <summary>
		/// Default concurrent eviction workers.
		/// </summary>
		public const int CDefaultConcurrentEvictionWorkers = 2;
		/// <summary>
		/// Default evictions per synchronized worker.
		/// </summary>
		public const int CDefaultEvictionsPerSynchronizedWorker = 10;
		/// <summary>
		/// Default lifespan of cache items. Afterwhich items are evicted.
		/// </summary>
		public const string CDefaultCacheItemLifespan = "0.00:10:00.0";
		/// <summary>
		/// Default cache depth.
		/// </summary>
		public const int CDefaultCacheDepth = 100;

		/// <summary>
		/// Default FullActive setting
		/// </summary>
		public const bool CDefaultFullActive = false;

		/// <summary>
		/// The cache's name.
		/// </summary>
		[ConfigurationProperty(PropertyName_name
			, IsKey = true
			, IsRequired = true)]
		public string Name
		{
			get { return (string)this[PropertyName_name]; }
			set { this[PropertyName_name] = value; }
		}

		/// <summary>
		/// Factor that determines when concurrent eviction threads are 
		/// launched.
		/// </summary>
		/// <remarks>
		/// This factor is combined with a cache's depth in order to
		/// determine the cache threshold. When the threshold is exceeded
		/// concurrent eviction workers will be started to remove the
		/// least-recently-used items from the cache.
		/// </remarks>
		[ConfigurationProperty(PropertyName_concurrentEvictionFactor, DefaultValue = CDefaultConcurrentEvictionFactor)]
		public double ConcurrentEvictionFactor
		{
			get { return (double)this[PropertyName_concurrentEvictionFactor]; }
			set
			{
				Contract.Requires<ArgumentOutOfRangeException>(value >= 1.0, "Eviction factor must be 1.0 or greater");
				this[PropertyName_concurrentEvictionFactor] = value;
			}
		}
		/// <summary>
		/// Factor that determines when evictions become synchronous.
		/// </summary>
		/// <remarks>
		/// This factor is combined with a cache's depth in order to
		/// determine the cache synchronization threshold. When the 
		/// threshold is exceeded the caller's thread will be borrowed 
		/// to process evictions.
		/// </remarks>
		[ConfigurationProperty(PropertyName_synchronizedEvictionFactor, DefaultValue = CDefaultSynchronizedEvictionFactor)]
		public double SynchronizedEvictionFactor
		{
			get { return (double)this[PropertyName_synchronizedEvictionFactor]; }
			set
			{
				Contract.Requires<ArgumentOutOfRangeException>(value >= 1.0, "Eviction factor must be 1.0 or greater");
				this[PropertyName_synchronizedEvictionFactor] = value;
			}
		}
		/// <summary>
		/// Number of evictions processed per synchronized eviction event.
		/// </summary>
		/// <remarks>
		/// If the synchronization threshold is exceeded then a calling
		/// thread is borrowed to process evictions. This setting 
		/// indicates the max number of evictions the borrowed thread
		/// should process before returning the thread to the caller.
		/// </remarks>
		[ConfigurationProperty(PropertyName_evictionsPerSynchronizedWorker, DefaultValue = CDefaultEvictionsPerSynchronizedWorker)]
		public int EvictionsPerSynchronizedWorker
		{
			get { return (int)this[PropertyName_evictionsPerSynchronizedWorker]; }
			set
			{
				Contract.Requires<ArgumentOutOfRangeException>(value >= 1, "Evictions per synchronized worker must be 1 or more");
				this[PropertyName_evictionsPerSynchronizedWorker] = value;
			}
		}

		/// <summary>
		/// Gets a cache item's lifespan.
		/// </summary>
		[ConfigurationProperty(PropertyName_cacheItemLifespan, DefaultValue = CDefaultCacheItemLifespan)]
		public string CacheItemLifespan
		{
			get { return (string)this[PropertyName_cacheItemLifespan]; }
			set
			{
				TimeSpan.Parse(value); // force valid timespan string.
				this[PropertyName_cacheItemLifespan] = value.ToString();
			}
		}

		/// <summary>
		/// Gets the cache's depth; after which evictions will be scheduled/processed.
		/// </summary>
		[ConfigurationProperty(PropertyName_cacheDepth, DefaultValue = CDefaultCacheDepth)]
		public int CacheDepth
		{
			get { return (int)this[PropertyName_cacheDepth]; }
			set
			{
				Contract.Requires<ArgumentOutOfRangeException>(value > 0, "Cache depth must be greater than zero");

				this[PropertyName_cacheDepth] = value;
			}
		}

		/// <summary>
		/// Gets the cache's context keys.
		/// </summary>
		[ConfigurationProperty(PropertyName_contextKeys)]
		public string ContextKeys
		{
			get { return (string)this[PropertyName_contextKeys]; }
			set
			{
				_contextKeys = null;
				this[PropertyName_contextKeys] = value;
			}
		}

		/// <summary>
		/// Indicates if the Cache should be in Full Active mode, meaning should actively monitor and reload expiring cache items.
		/// </summary>
		[ConfigurationProperty(PropertyName_fullActive, DefaultValue = CDefaultFullActive)]
		public bool FullActive
		{
			get { return (bool)this[PropertyName_fullActive]; }
			set { this[PropertyName_fullActive] = value; }
		}

		internal IEnumerable<string> ResolvedContextKeys(Type targetType)
		{
			if (_contextKeys == null)
			{
				var cnf = ContextKeys ?? String.Empty;
				var keys = new List<string>(from k in cnf.Split(';')
											select k);
				var targetName = targetType.GetReadableFullName();
				if (!keys.Contains(targetName))
					keys.Insert(0, targetName);
				if (!keys.Contains(this.Name))
					keys.Insert(0, this.Name);
				_contextKeys = keys.ToReadOnly();
			}
			return _contextKeys;
		}
		IEnumerable<string> _contextKeys;

		internal TimeSpan ResolvedCacheItemLifespan
		{
			get { return TimeSpan.Parse(this.CacheItemLifespan); }
		}

		internal MruCacheOptions ToOptions<T>()
		{
			return new MruCacheOptions
			{
				CacheDepth = this.CacheDepth,
				CacheItemLifespan = ResolvedCacheItemLifespan,
				ConcurrentEvictionFactor = this.ConcurrentEvictionFactor,
				SynchronizedEvictionFactor = this.SynchronizedEvictionFactor,
				EvictionsPerSynchronizedWorker = this.EvictionsPerSynchronizedWorker,
				ContextKeys = ResolvedContextKeys(typeof(T)),
				FullActive = this.FullActive
			};
		}
	}

}
