using System;
using System.Linq;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using NetSteps.Encore.Core.Log;
using System.Collections.Generic;
using System.Timers;
using System.Threading.Tasks;

namespace NetSteps.Core.Cache
{
	/// <summary>
	/// Local memory cache implementation that keeps only the most recently used
	/// items in memory and actively resolves missing items.
	/// </summary>
	/// <typeparam name="K">key type K</typeparam>
	/// <typeparam name="R">representation type R</typeparam>
	public sealed class ActiveMruLocalMemoryCache<K, R> : MruLocalMemoryCache<K, R>
		where R : class
	{
		static readonly ILogSink __log = typeof(ActiveMruLocalMemoryCache<K, R>).GetLogSink();

		ICacheItemResolver<K, R> _resolver;
		Timer _fullActiveTimer;
		ElapsedEventHandler _fullActiveTimer_Elapsed;

		#region Constructors

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="name">the cache's name</param>
		/// <param name="resolver">a cache item resolver used to resolve missing
		/// items.</param>
		public ActiveMruLocalMemoryCache(string name, ICacheItemResolver<K, R> resolver)
			: this(name, null, resolver, null)
		{
			Contract.Requires<ArgumentNullException>(name != null);
			Contract.Requires<ArgumentException>(name.Length > 0);
			Contract.Requires<ArgumentNullException>(resolver != null);
		}
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="name">the cache's name</param>
		/// <param name="resolver">a cache item resolver used to resolve missing
		/// items.</param>
		/// <param name="manager">a cache eviction manager</param>
		public ActiveMruLocalMemoryCache(string name, ICacheItemResolver<K, R> resolver, ICacheEvictionManager manager)
			: this(name, null, resolver, manager)
		{
			Contract.Requires<ArgumentNullException>(name != null);
			Contract.Requires<ArgumentException>(name.Length > 0);
			Contract.Requires<ArgumentNullException>(resolver != null);
		}
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="name">the cache's name</param>
		/// <param name="options">options</param>
		/// <param name="resolver">a cache item resolver used to resolve missing
		/// items.</param>
		[SuppressMessage("Microsoft.Reliability", "CA2000", Justification = "Guaranteed by the Dispose method of the base class")]
		public ActiveMruLocalMemoryCache(string name,
			MruCacheOptions options,
			ICacheItemResolver<K, R> resolver
			)
			: this(name, options, new ActiveCacheStats(name), resolver, null)
		{
			Contract.Requires<ArgumentNullException>(name != null);
			Contract.Requires<ArgumentException>(name.Length > 0);
			Contract.Requires<ArgumentNullException>(resolver != null);
		}
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="name">the cache's name</param>
		/// <param name="options">options</param>
		/// <param name="resolver">a cache item resolver used to resolve missing
		/// items.</param>
		/// <param name="manager">a cache eviction manager</param>
		[SuppressMessage("Microsoft.Reliability", "CA2000", Justification = "Guaranteed by the Dispose method of the base class")]
		public ActiveMruLocalMemoryCache(string name,
			MruCacheOptions options,
			ICacheItemResolver<K, R> resolver,
			ICacheEvictionManager manager
			)
			: this(name, options, new ActiveCacheStats(name), resolver, manager)
		{
			Contract.Requires<ArgumentNullException>(name != null);
			Contract.Requires<ArgumentException>(name.Length > 0);
			Contract.Requires<ArgumentNullException>(resolver != null);
		}
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="name">the cache's name</param>
		/// <param name="options">options</param>
		/// <param name="stats">statistics collector</param>
		/// <param name="resolver">a cache item resolver used to resolve missing
		/// items.</param>
		public ActiveMruLocalMemoryCache(string name,
			MruCacheOptions options,
			IActiveCacheStatsCollector stats,
			ICacheItemResolver<K, R> resolver
			)
			: this(name, options, stats, resolver, null)
		{
			Contract.Requires<ArgumentNullException>(name != null);
			Contract.Requires<ArgumentException>(name.Length > 0);
			Contract.Requires<ArgumentNullException>(stats != null);
			Contract.Requires<ArgumentNullException>(resolver != null);
		}
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="name">the cache's name</param>
		/// <param name="options">options</param>
		/// <param name="stats">statistics collector</param>
		/// <param name="resolver">a cache item resolver used to resolve missing
		/// items.</param>
		/// <param name="manager">a cache eviction manager</param>
		public ActiveMruLocalMemoryCache(string name,
			MruCacheOptions options,
			IActiveCacheStatsCollector stats,
			ICacheItemResolver<K, R> resolver,
			ICacheEvictionManager manager
			)
			: base(name, options, stats, manager)
		{
			Contract.Requires<ArgumentNullException>(name != null);
			Contract.Requires<ArgumentException>(name.Length > 0);
			Contract.Requires<ArgumentNullException>(stats != null);
			Contract.Requires<ArgumentNullException>(resolver != null);

			_resolver = resolver;

			if (Options.FullActive)
			{
				EngageFullActive();
			}
		}

		#endregion

		private void EngageFullActive()
		{
			_fullActiveTimer_Elapsed = new ElapsedEventHandler(fullActiveTimer_Elapsed);
			_fullActiveTimer = new Timer();
			_fullActiveTimer.Elapsed += _fullActiveTimer_Elapsed;
			_fullActiveTimer.AutoReset = true;
			_fullActiveTimer.Enabled = true;
			_fullActiveTimer.Interval = Math.Ceiling(Options.CacheItemLifespan.TotalMilliseconds * 0.25);
			_fullActiveTimer.Start();
		}

		void fullActiveTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			__log.Verbose("{0}: Active Cache Refresh Timer Firing", Name);

			Task.Factory
				.StartNew(() =>
				{
					IEnumerable<K> result;
					try
					{
						result = InternalCache.Where(kvp => kvp.Value.IsExpired(null)).Select(kvp => kvp.Key).ToArray() ?? Enumerable.Empty<K>();
					}
					catch (Exception ex)
					{
						result = Enumerable.Empty<K>();
						__log.Error(ex);
#if DEBUG
						throw ex;
#endif
					}
					if (result.Any())
					{
						__log.Verbose("{0}: Found {1} items to refresh", Name, result.Count());
					}
					return result;
				})
				.ContinueWith((expired) =>
				{
					IEnumerable<K> result = expired.Result;
					try
					{
						__log.Verbose("{0}: Attempting to remove {1} Items", Name, result.Count());
						if (expired.Result.Any())
						{
							IEnumerable<R> removed;
							TryRemoveMany(result, out removed);
							__log.Verbose("{0}: Removed {1} Items", Name, (removed != null ? removed.Count() : 0));
						}
					}
					catch (Exception ex)
					{
						__log.Error(ex);
#if DEBUG
						throw ex;
#endif
					}
					return result;
				})
				.ContinueWith((expired) =>
				{
					try
					{
						if (expired.Result.Any())
						{
							__log.Verbose("{0}: Attempting to refresh {1} items", Name, expired.Result.Count());
							var exp = expired.Result;
							IEnumerable<R> refreshed;
							TryGetMany(exp, out refreshed);
							__log.Verbose("{0}: Refreshed {1} Items", Name, (refreshed != null ? refreshed.Count() : 0));
						}
					}
					catch (Exception ex)
					{
						__log.Error(ex);
#if DEBUG
						throw ex;
#endif
					}
				});
		}

		/// <summary>
		/// Tries to resolve a missing item from the cache by delegating to the
		/// resolver.
		/// </summary>
		/// <param name="key">the item's key</param>
		/// <param name="value">variable where the item will be returned upon success</param>
		/// <returns>ResolutionKind.Created or ResolutionKind.Resolved if the item was resolved; otherwise ResolutionKind.None</returns>		
		/// <see cref="ResolutionKind"/>
		protected override ResolutionKind TryResolveMissingItem(K key, out R value)
		{
			return _resolver.TryResolve(key, out value);
		}
		/// <summary>
		/// Tries to resolve missing items from the cache by delegating to the resolver.
		/// </summary>
		/// <param name="keys">the items keys</param>
		/// <param name="values">variable where the items will be returned upon success</param>
		/// <returns>ResolutionManyKind.Created or ResolutionManyKind.Resolved if all of the items were resolved or ResolutionManyKind.PartiallyResolved if some of the items were resolved; otherwise ResolutionManyKind.None</returns>		
		/// <see cref="ResolutionKind"/>
		protected override ResolutionManyKind TryResolveManyMissingItems(IEnumerable<K> keys, out IEnumerable<KeyValuePair<K, R>> values)
		{
			if (_resolver is ICacheManyItemResolver<K, R>)
			{
				return ((ICacheManyItemResolver<K, R>)_resolver).TryResolveMany(keys, out values);
			}
			else
			{
				__log.Warning(() => new { Message = "TryResolveManyMissingItems called, but the resolver is not an ICachManyItemResolver, using single item resolution via enumeration.", CacheName = this.Name });
				values = new List<KeyValuePair<K, R>>();
				foreach (var item in keys)
				{
					R result;
					var resolution = TryResolveMissingItem(item, out result);
					if (resolution != ResolutionKind.None && result != null)
					{
						((List<KeyValuePair<K, R>>)values).Add(new KeyValuePair<K, R>(item, result));
					}
				}

				if (values.Any())
				{
					if (values.Count() == keys.Count())
					{
						return ResolutionManyKind.Created;
					}
					else
					{
						return ResolutionManyKind.PartiallyResolved;
					}
				}
				else
				{
					return ResolutionManyKind.None;
				}
			}
		}

		/// <summary>
		/// Updates statistics when items are resolved.
		/// </summary>
		/// <param name="stats">statistics collector</param>
		/// <param name="kind">kind of resolution</param>
		protected override void UpdateResolveCounters(ICacheStatsCollector stats, ResolutionKind kind)
		{
			base.UpdateResolveCounters(stats, kind);
			if (kind.HasFlag(ResolutionKind.Created))
				((IActiveCacheStatsCollector)stats).IncrementResolves();
		}

		/// <summary>
		/// Overridden to cleanup the bound events of the internal timer if required.
		/// </summary>
		/// <param name="disposing"></param>
		/// <returns></returns>
		protected override bool PerformDispose(bool disposing)
		{
			if (_fullActiveTimer != null)
			{
				_fullActiveTimer.Stop();
				_fullActiveTimer.Elapsed -= _fullActiveTimer_Elapsed;
				_fullActiveTimer.Dispose();
			}
			return base.PerformDispose(disposing);
		}
	}
}
