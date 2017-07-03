using System;
namespace NetSteps.Core.Cache
{
	/// <summary>
	/// Collects an active cache's statistics.
	/// </summary>
	public interface IActiveCacheStatsCollector : ICacheStatsCollector
	{
		/// <summary>
		/// Increments the number of items resolved.
		/// </summary>
		void IncrementResolves();
	}
}
