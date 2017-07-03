using System.Collections.Concurrent;
using System;

namespace NetSteps.Core.Cache
{	
	/// <summary>
	/// Interface for managing a cache's hierarchy.
	/// </summary>
	/// <typeparam name="K">key type K</typeparam>
	/// <typeparam name="R">representation type R</typeparam>
	public interface ICacheHerarchyManagement<K, R>
        where R : class
    {
		/// <summary>
		/// Links the current cache with a fallback cache.
		/// </summary>
		/// <param name="cache">a fallback cache</param>
		void LinkFallbackCache(ICache<K, R> cache);
	}
}
