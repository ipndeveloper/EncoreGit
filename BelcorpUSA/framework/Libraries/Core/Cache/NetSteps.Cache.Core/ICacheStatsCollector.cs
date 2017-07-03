using System;
namespace NetSteps.Core.Cache
{
	/// <summary>
	/// Collects a cache's statistics.
	/// </summary>
	public interface ICacheStatsCollector
	{
		/// <summary>
		/// Increments evicitons.
		/// </summary>
		void IncrementEvictions();
		/// <summary>
		/// Increments evicitons by <paramref name="count"/>.
		/// </summary>
		void IncrementEvictions(int count);
		/// <summary>
		/// Increments eviciton workers.
		/// </summary>
		void IncrementEvictionWorkers();
		/// <summary>
		/// Decrements eviction workers.
		/// </summary>
		void DecrementEvictionWorkers();
		/// <summary>
		/// Increments expirations.
		/// </summary>
		void IncrementExpired();
		/// <summary>
		/// Increments hits (for hit ratio).
		/// </summary>
		void IncrementHits();
		/// <summary>
		/// Increments read attempts. This is the base of various ratios.
		/// </summary>
		void IncrementReadAttempts();
		/// <summary>
		/// Increments removals.
		/// </summary>
		void IncrementRemoves();
		/// <summary>
		/// Increments writes.
		/// </summary>
		void IncrementWrites();
	}	
}
