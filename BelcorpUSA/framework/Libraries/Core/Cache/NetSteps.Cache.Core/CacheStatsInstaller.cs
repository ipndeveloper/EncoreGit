using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Win32;


namespace NetSteps.Core.Cache
{
	/// <summary>
	/// Installs cache statistics performance counters
	/// </summary>
	[RunInstaller(true)]
	public partial class CacheStatsInstaller : Installer
	{
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		[SuppressMessage("Microsoft.Reliability", "CA2000")]
		public CacheStatsInstaller()
		{
			InitializeComponent();
			// Always remove pre-existing categories.
			if (PerformanceCounterCategory.Exists(CacheStats.CPerformanceCounterCategory))
				PerformanceCounterCategory.Delete(CacheStats.CPerformanceCounterCategory);
			if (PerformanceCounterCategory.Exists(CacheStats.CPerformanceCounterCategoryEa))
				PerformanceCounterCategory.Delete(CacheStats.CPerformanceCounterCategoryEa);
			if (PerformanceCounterCategory.Exists(ActiveCacheStats.CPerformanceCounterCategoryActiveEa))
				PerformanceCounterCategory.Delete(ActiveCacheStats.CPerformanceCounterCategoryActiveEa);

			var overall = new PerformanceCounterInstaller();
			overall.CategoryName = CacheStats.CPerformanceCounterCategory;
			overall.CategoryType = PerformanceCounterCategoryType.SingleInstance;
			overall.CategoryHelp = "Provides combined statistics for NetSteps caches";
			overall.Counters.Add(new CounterCreationData
				(CacheStats.CPerformanceCounterCacheHitRatio
				, CacheStats.CPerformanceCounterCacheHitRatio
				, PerformanceCounterType.RawFraction
				));
			overall.Counters.Add(new CounterCreationData
				(CacheStats.CPerformanceCounterCacheHitRatioBase
				, CacheStats.CPerformanceCounterCacheHitRatioBase
				, PerformanceCounterType.RawBase
				));
			overall.Counters.Add(new CounterCreationData
				(ActiveCacheStats.CPerformanceCounterCacheResolveRatio
				, ActiveCacheStats.CPerformanceCounterCacheResolveRatio
				, PerformanceCounterType.RawFraction
				));
			overall.Counters.Add(new CounterCreationData
				(ActiveCacheStats.CPerformanceCounterCacheResolveRatioBase
				, ActiveCacheStats.CPerformanceCounterCacheResolveRatioBase
				, PerformanceCounterType.RawBase
				));
			overall.Counters.Add(new CounterCreationData
				(CacheStats.CPerformanceCounterEvictionWorkers
				, CacheStats.CPerformanceCounterEvictionWorkers
				, PerformanceCounterType.NumberOfItems32
				));
			this.Installers.Add(overall);

			var individual = new PerformanceCounterInstaller();
			individual.CategoryName = CacheStats.CPerformanceCounterCategoryEa;
			individual.CategoryType = PerformanceCounterCategoryType.MultiInstance;
			individual.CategoryHelp = "Provides statistics for NetSteps local memory caches";
			
			individual.Counters.Add(new CounterCreationData
				(	CacheStats.CPerformanceCounterCacheHitRatio
				, CacheStats.CPerformanceCounterCacheHitRatio
				, PerformanceCounterType.RawFraction)
				);
			individual.Counters.Add(new CounterCreationData
				( CacheStats.CPerformanceCounterCacheHitRatioBase
				, CacheStats.CPerformanceCounterCacheHitRatioBase
				, PerformanceCounterType.RawBase)
				);

			individual.Counters.Add(new CounterCreationData
				( CacheStats.CPerformanceCounterCacheReadsPerSecond
				, "Items read from the cache per second"
				, PerformanceCounterType.RateOfCountsPerSecond64)
				);
			individual.Counters.Add(new CounterCreationData
				(CacheStats.CPerformanceCounterCacheWritesPerSecond
				, "Items write from the cache per second"
				, PerformanceCounterType.RateOfCountsPerSecond64)
				);
			individual.Counters.Add(new CounterCreationData
				(CacheStats.CPerformanceCounterCacheRemovesPerSecond
				, "Items removed from the cache per second"
				, PerformanceCounterType.RateOfCountsPerSecond64)
				);
			individual.Counters.Add(new CounterCreationData
				(CacheStats.CPerformanceCounterCacheExpiresPerSecond
				, "Items expired per second"
				, PerformanceCounterType.RateOfCountsPerSecond64)
				);
			individual.Counters.Add(new CounterCreationData
				( CacheStats.CPerformanceCounterCacheEvictionsPerSecond
				, "Items evicted from the cache per second"
				, PerformanceCounterType.RateOfCountsPerSecond64)
				);

			this.Installers.Add(individual);

			var active = new PerformanceCounterInstaller();
			active.CategoryName = ActiveCacheStats.CPerformanceCounterCategoryActiveEa;
			active.CategoryType = PerformanceCounterCategoryType.MultiInstance;
			active.CategoryHelp = "Provides statistics for NetSteps local memory caches";

			active.Counters.Add(new CounterCreationData
				(CacheStats.CPerformanceCounterCacheHitRatio
				, CacheStats.CPerformanceCounterCacheHitRatio
				, PerformanceCounterType.RawFraction)
				);
			active.Counters.Add(new CounterCreationData
				(CacheStats.CPerformanceCounterCacheHitRatioBase
				, CacheStats.CPerformanceCounterCacheHitRatioBase
				, PerformanceCounterType.RawBase)
				);

			active.Counters.Add(new CounterCreationData
				(ActiveCacheStats.CPerformanceCounterCacheResolveRatio
				, ActiveCacheStats.CPerformanceCounterCacheResolveRatio
				, PerformanceCounterType.RawFraction)
				);
			active.Counters.Add(new CounterCreationData
				(ActiveCacheStats.CPerformanceCounterCacheResolveRatioBase
				, ActiveCacheStats.CPerformanceCounterCacheResolveRatioBase
				, PerformanceCounterType.RawBase)
				);

			active.Counters.Add(new CounterCreationData
				(CacheStats.CPerformanceCounterCacheReadsPerSecond
				, "Items read from the cache per second"
				, PerformanceCounterType.RateOfCountsPerSecond64)
				);
			active.Counters.Add(new CounterCreationData
				(CacheStats.CPerformanceCounterCacheWritesPerSecond
				, "Items write from the cache per second"
				, PerformanceCounterType.RateOfCountsPerSecond64)
				);
			active.Counters.Add(new CounterCreationData
				(CacheStats.CPerformanceCounterCacheRemovesPerSecond
				, "Items removed from the cache per second"
				, PerformanceCounterType.RateOfCountsPerSecond64)
				);
			active.Counters.Add(new CounterCreationData
				(ActiveCacheStats.CPerformanceCounterCacheResolvesPerSecond
				, "Items resolved from the cache per second"
				, PerformanceCounterType.RateOfCountsPerSecond64)
				);
			active.Counters.Add(new CounterCreationData
				(CacheStats.CPerformanceCounterCacheExpiresPerSecond
				, "Items expired per second"
				, PerformanceCounterType.RateOfCountsPerSecond64)
				);
			active.Counters.Add(new CounterCreationData
				(CacheStats.CPerformanceCounterCacheEvictionsPerSecond
				, "Items evicted from the cache per second"
				, PerformanceCounterType.RateOfCountsPerSecond64)
				);

			this.Installers.Add(active);
		}
		
		/// <summary>
		/// Performs the install
		/// </summary>
		/// <param name="stateSaver"></param>
		public override void Install(IDictionary stateSaver)
		{
			base.Install(stateSaver);

			RegistryKey hklm = Registry.LocalMachine;
			RegistryKey overall = hklm.OpenSubKey(String.Format(@"SYSTEM\CurrentControlSet\Services\{0}\Performance", CacheStats.CPerformanceCounterCategory), true);
			overall.SetValue("FileMappingSize", 524288, RegistryValueKind.DWord);
			RegistryKey individual = hklm.OpenSubKey(String.Format(@"SYSTEM\CurrentControlSet\Services\{0}\Performance", CacheStats.CPerformanceCounterCategoryEa), true);
			individual.SetValue("FileMappingSize", 524288, RegistryValueKind.DWord);
			RegistryKey active = hklm.OpenSubKey(String.Format(@"SYSTEM\CurrentControlSet\Services\{0}\Performance", ActiveCacheStats.CPerformanceCounterCategoryActiveEa), true);
			active.SetValue("FileMappingSize", 524288, RegistryValueKind.DWord);
		}
	}
}
