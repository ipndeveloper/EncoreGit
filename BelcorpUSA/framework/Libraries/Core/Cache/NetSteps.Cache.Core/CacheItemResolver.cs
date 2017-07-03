using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetSteps.Core.Cache
{
	/// <summary>
	/// Abstract base class implementation of ICacheItemResolver&lt;K,R>
	/// </summary>
	/// <typeparam name="K">key type K</typeparam>
	/// <typeparam name="R">representation type R</typeparam>
	public abstract class CacheItemResolver<K, R> : ICacheItemResolver<K, R>
	{
		int _resolveAttempts = 0;
		int _resolveSuccesses = 0;

		/// <summary>
		/// Tries to resolve an item that is missing from the cache.
		/// </summary>
		/// <param name="key">the item's key</param>
		/// <param name="value">variable to hold the item upon success</param>
		/// <returns>the resolution kind</returns>
		/// <see cref="ResolutionKind"/>
		public ResolutionKind TryResolve(K key, out R value)
		{
			IncrementResolveAttemptCounter();
			var kind = PerformTryResolve(key, out value);
			if (kind.HasFlag(ResolutionKind.Resolved))
			{
				IncrementResolveSuccessCounter();
			}
			return kind;
		}

		/// <summary>
		/// Gets the number of items attempted.
		/// </summary>
		public int AttemptCount { get { return Thread.VolatileRead(ref _resolveAttempts); } }

		/// <summary>
		/// Gets the number of items resolved.
		/// </summary>
		public int ResolveCount { get { return Thread.VolatileRead(ref _resolveSuccesses); } }

		/// <summary>
		/// Increments the counter tracking successful resolve attempts.
		/// </summary>
		protected virtual void IncrementResolveSuccessCounter()
		{
			Interlocked.Increment(ref _resolveSuccesses);
		}

		/// <summary>
		/// Increments the counter tracking successful resolve attempts.
		/// </summary>
		/// <param name="increment"></param>
		protected virtual void IncrementResolveSuccessCounter(int increment)
		{
			Interlocked.Add(ref _resolveSuccesses, increment);
		}

		/// <summary>
		/// Increments the counter tracking resolve attempts.
		/// </summary>
		protected virtual void IncrementResolveAttemptCounter()
		{
			Interlocked.Increment(ref _resolveAttempts);
		}

		/// <summary>
		/// Increments the counter tracking resolve attempts.
		/// </summary>
		/// <param name="increment"></param>
		protected virtual void IncrementResolveAttemptCounter(int increment)
		{
			Interlocked.Add(ref _resolveAttempts, increment);
		}

		/// <summary>
		/// Overriden by subclasses to try and resolve the item.
		/// </summary>
		/// <param name="key">the item's key</param>
		/// <param name="value">variable to hold the item's value upon success</param>
		/// <returns>true if successful; otherwise false</returns>
		protected abstract ResolutionKind PerformTryResolve(K key, out R value);
	}
}
