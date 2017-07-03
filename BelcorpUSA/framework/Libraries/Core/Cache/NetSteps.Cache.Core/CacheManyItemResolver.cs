using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Core.Cache
{
	/// <summary>
	/// Abstract base class implementation of ICacheItemManyResolver&lt;K,R>
	/// </summary>
	/// <typeparam name="K">key type K</typeparam>
	/// <typeparam name="R">representation type R</typeparam>
	public abstract class CacheManyItemResolver<K, R> : CacheItemResolver<K, R>, ICacheManyItemResolver<K, R>
	{
		/// <summary>
		/// Tries to resolve items that are missing from the cache.
		/// </summary>
		/// <param name="keys">the item's key</param>
		/// <param name="values">variable to hold the item upon success</param>
		/// <returns>the resolution kind</returns>
		/// <see cref="ResolutionManyKind"/>
		public ResolutionManyKind TryResolveMany(IEnumerable<K> keys, out IEnumerable<KeyValuePair<K, R>> values)
		{
			IncrementResolveAttemptCounter(keys.Count());
			var kind = PerformTryResolveMany(keys, out values);
			if (kind.HasFlag(ResolutionManyKind.Resolved) || kind.HasFlag(ResolutionManyKind.PartiallyResolved))
			{
				IncrementResolveSuccessCounter(values.Count());
			}
			return kind;
		}

		/// <summary>
		/// Overriden by subclasses to try and resolve the item.
		/// </summary>
		/// <param name="keys">the items keys</param>
		/// <param name="values">variable to hold the items vales upon success</param>
		/// <returns>true if successful; otherwise false</returns>
		protected abstract ResolutionManyKind PerformTryResolveMany(IEnumerable<K> keys, out IEnumerable<KeyValuePair<K, R>> values);
	}
}
