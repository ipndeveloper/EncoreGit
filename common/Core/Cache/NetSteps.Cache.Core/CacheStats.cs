using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using NetSteps.Encore.Core;

namespace NetSteps.Core.Cache
{
	internal class CacheStats : Disposable, ICacheStatsCollector
	{
		internal static readonly string CPerformanceCounterCategory = "NetSteps Cache";
		internal static readonly string CPerformanceCounterCategoryEa = "NetSteps Cache (ea)";
		internal static readonly string CPerformanceCounterCacheHitRatio = "hit ratio";
		internal static readonly string CPerformanceCounterCacheHitRatioBase = "hit ratio (base)";
		internal static readonly string CPerformanceCounterEvictionWorkers = "eviction workers";
		internal static readonly string CPerformanceCounterCacheReadsPerSecond = "reads/second";
		internal static readonly string CPerformanceCounterCacheWritesPerSecond = "writes/second";
		internal static readonly string CPerformanceCounterCacheRemovesPerSecond = "removes/second";
		internal static readonly string CPerformanceCounterCacheExpiresPerSecond = "expires/second";
		internal static readonly string CPerformanceCounterCacheEvictionsPerSecond = "evictions/second";

		PerformanceCounter _hRatio = new PerformanceCounter(CPerformanceCounterCategory, CPerformanceCounterCacheHitRatio, String.Empty, false);
		PerformanceCounter _hRatioBase = new PerformanceCounter(CPerformanceCounterCategory, CPerformanceCounterCacheHitRatioBase, String.Empty, false);
		PerformanceCounter _evictionWorkers = new PerformanceCounter(CPerformanceCounterCategory, CPerformanceCounterEvictionWorkers, String.Empty, false);
		PerformanceCounter _hitRatioBaseInstance;
		PerformanceCounter _hitRatioInstance;
		PerformanceCounter _readsPerSec;
		PerformanceCounter _writesPerSec;
		PerformanceCounter _removesPerSec;
		PerformanceCounter _expiresPerSec;
		PerformanceCounter _evictionsPerSec;

		public CacheStats(string instanceName)
			: this(CPerformanceCounterCategoryEa, instanceName)
		{
		}
		public CacheStats(string category, string instanceName)
		{
			_hitRatioInstance = new PerformanceCounter(category
				, CPerformanceCounterCacheHitRatio
				, instanceName
				, false
				);
			_hitRatioInstance.RawValue = 0;
			_hitRatioBaseInstance = new PerformanceCounter(category
				, CPerformanceCounterCacheHitRatioBase
				, instanceName, false
				);
			_hitRatioBaseInstance.RawValue = 0;
			_readsPerSec = new PerformanceCounter(category
					, CPerformanceCounterCacheReadsPerSecond
					, instanceName
					, false
					);
			_writesPerSec = new PerformanceCounter(category
					, CPerformanceCounterCacheWritesPerSecond
					, instanceName
					, false
					);
			_removesPerSec = new PerformanceCounter(category
					, CPerformanceCounterCacheRemovesPerSecond
					, instanceName
					, false
					);
			_expiresPerSec = new PerformanceCounter(category
					, CPerformanceCounterCacheExpiresPerSecond
					, instanceName
					, false
					);
			_evictionsPerSec = new PerformanceCounter(category
					, CPerformanceCounterCacheEvictionsPerSecond
					, instanceName
					, false
					);
		}

		[ContractInvariantMethod]
		void InvariantContracts()
		{
			Contract.Invariant(_hRatio != null);
			Contract.Invariant(_hRatioBase != null);
			Contract.Invariant(_hitRatioInstance != null);
			Contract.Invariant(_hitRatioBaseInstance != null);
			Contract.Invariant(_readsPerSec != null);
			Contract.Invariant(_writesPerSec != null);
			Contract.Invariant(_removesPerSec != null);
			Contract.Invariant(_expiresPerSec != null);
			Contract.Invariant(_evictionsPerSec != null);
			Contract.Invariant(_evictionWorkers != null);
		}

		public void IncrementEvictionWorkers()
		{
			_evictionWorkers.Increment();
		}

		public void DecrementEvictionWorkers()
		{
			_evictionWorkers.Decrement();
		}

		public void IncrementHits()
		{
			_hitRatioInstance.Increment();
			_hRatio.Increment();
		}

		public virtual void IncrementReadAttempts()
		{
			_hitRatioBaseInstance.Increment();
			_hRatioBase.Increment();
			_readsPerSec.Increment();
		}

		public void IncrementWrites()
		{
			_writesPerSec.Increment();
		}

		public void IncrementRemoves()
		{
			_removesPerSec.Increment();
		}

		public void IncrementExpired()
		{
			_expiresPerSec.Increment();
		}

		public void IncrementEvictions()
		{
			_evictionsPerSec.Increment();
		}

		public void IncrementEvictions(int count)
		{
			_evictionsPerSec.IncrementBy(count);
		}

		protected override bool PerformDispose(bool disposing)
		{
			Util.Dispose(ref _hitRatioBaseInstance);
			Util.Dispose(ref _hitRatioInstance);
			Util.Dispose(ref _hRatio);
			Util.Dispose(ref _hRatioBase);
			Util.Dispose(ref _readsPerSec);
			Util.Dispose(ref _writesPerSec);
			Util.Dispose(ref _removesPerSec);
			Util.Dispose(ref _expiresPerSec);
			Util.Dispose(ref _evictionsPerSec);
			return true;
		}
	}
}
