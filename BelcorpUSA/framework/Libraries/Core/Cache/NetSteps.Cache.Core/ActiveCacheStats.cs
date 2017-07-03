using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using NetSteps.Encore.Core;


namespace NetSteps.Core.Cache
{	
	internal class ActiveCacheStats : CacheStats, IActiveCacheStatsCollector
	{
		internal static readonly string CPerformanceCounterCategoryActiveEa = "NetSteps Active Cache (ea)";
		internal static readonly string CPerformanceCounterCacheResolveRatio = "resolve ratio";
		internal static readonly string CPerformanceCounterCacheResolveRatioBase = "resolve ratio (base)";
		internal static readonly string CPerformanceCounterCacheResolvesPerSecond = "resolves/second";

		PerformanceCounter _resolveRatio = new PerformanceCounter(CPerformanceCounterCategory, CPerformanceCounterCacheResolveRatio, String.Empty, false);
		PerformanceCounter _resolveRatioBase = new PerformanceCounter(CPerformanceCounterCategory, CPerformanceCounterCacheResolveRatioBase, String.Empty, false);
		PerformanceCounter _resolveRatioBaseInstance;
		PerformanceCounter _resolveRatioInstance;
		PerformanceCounter _resolvesPerSec;

		internal ActiveCacheStats(string instanceName)
			: base(CPerformanceCounterCategoryActiveEa, instanceName)
		{
			_resolveRatioInstance = new PerformanceCounter(CPerformanceCounterCategoryActiveEa
				, CPerformanceCounterCacheResolveRatio
				, instanceName
				, false
				);
			_resolveRatioInstance.RawValue = 0;
			_resolveRatioBaseInstance = new PerformanceCounter(CPerformanceCounterCategoryActiveEa
				, CPerformanceCounterCacheResolveRatioBase
				, instanceName, false
				);
			_resolveRatioBaseInstance.RawValue = 0;
			_resolvesPerSec = new PerformanceCounter(CPerformanceCounterCategoryActiveEa
					, CPerformanceCounterCacheResolvesPerSecond
					, instanceName
					, false
					);
		}

		[ContractInvariantMethod]
		void InvariantContracts()
		{
			Contract.Invariant(_resolveRatio != null);
			Contract.Invariant(_resolveRatioBase != null);
			Contract.Invariant(_resolveRatioInstance != null);
			Contract.Invariant(_resolveRatioBaseInstance != null);
			Contract.Invariant(_resolvesPerSec != null);
		}

		public override void IncrementReadAttempts()
		{
			base.IncrementReadAttempts();
			_resolveRatioBaseInstance.Increment();
			_resolveRatioBase.Increment();
		}

		public void IncrementResolves()
		{
			_resolvesPerSec.Increment();
			_resolveRatioInstance.Increment();
			_resolveRatio.Increment();
		}

		protected override bool PerformDispose(bool disposing)
		{
			Util.Dispose(ref _resolveRatioBaseInstance);
			Util.Dispose(ref _resolveRatioInstance);
			Util.Dispose(ref _resolveRatio);
			Util.Dispose(ref _resolveRatioBase);
			Util.Dispose(ref _resolvesPerSec);
			return true;
		}
	}
}
