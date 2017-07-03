using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using NetSteps.Encore.Core;
using NetSteps.Core.Cache.Config;

namespace NetSteps.Core.Cache
{
	/// <summary>
	/// Options for MRU caches
	/// </summary>
	public sealed class MruCacheOptions
	{
		double _concurrentEvictionFactor = NamedCacheConfigElement.CDefaultConcurrentEvictionFactor;
		double _synchronizedEvictionFactor = NamedCacheConfigElement.CDefaultSynchronizedEvictionFactor;
		int _evictionsPerSynchronizedWorker = NamedCacheConfigElement.CDefaultEvictionsPerSynchronizedWorker;
		TimeSpan _cacheItemLifespan = TimeSpan.Parse(NamedCacheConfigElement.CDefaultCacheItemLifespan);
		int _cacheDepth = NamedCacheConfigElement.CDefaultCacheDepth;
		IEnumerable<string> _contextKeys;
		bool _fullActive = NamedCacheConfigElement.CDefaultFullActive;

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
		public double ConcurrentEvictionFactor
		{
			get { return _concurrentEvictionFactor; }
			set
			{
				Contract.Requires<ArgumentOutOfRangeException>(value >= 1.0, "Eviction factor must be 1.0 or greater");
				_concurrentEvictionFactor = value;
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
		public double SynchronizedEvictionFactor
		{
			get { return _synchronizedEvictionFactor; }
			set
			{
				Contract.Requires<ArgumentOutOfRangeException>(value >= ConcurrentEvictionFactor, "Sychronized eviction factor must be greater than or equal to the concurrent eviction factor");
				_synchronizedEvictionFactor = value;
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
		public int EvictionsPerSynchronizedWorker
		{
			get { return _evictionsPerSynchronizedWorker; }
			set
			{
				Contract.Requires<ArgumentOutOfRangeException>(value >= 1, "Evictions per synchronized worker must be 1 or more");
				_evictionsPerSynchronizedWorker = value;
			}
		}

		/// <summary>
		/// Gets a cache item's lifespan.
		/// </summary>
		public TimeSpan CacheItemLifespan
		{
			get { return _cacheItemLifespan; }
			set { _cacheItemLifespan = value; }
		}

		/// <summary>
		/// Gets the cache's depth; after which evictions will be scheduled/processed.
		/// </summary>
		public int CacheDepth
		{
			get { return _cacheDepth; }
			set
			{
				Contract.Requires<ArgumentOutOfRangeException>(value >= 1, "Cache depth must be greater than zero");

				_cacheDepth = value;
			}
		}

		/// <summary>
		/// Gets the cache's context keys.
		/// </summary>
		public IEnumerable<string> ContextKeys
		{
			get { return _contextKeys; }
			set
			{
				if (value == null)
					_contextKeys = Enumerable.Empty<string>();
				else
					_contextKeys = value.ToReadOnly();
			}
		}

		/// <summary>
		/// Indicates if the Cache should be in Full Active mode, meaning should actively monitor and reload expiring cache items.
		/// </summary>
		public bool FullActive
		{
			get { return _fullActive; }
			set { _fullActive = value; }
		}
	}
}
