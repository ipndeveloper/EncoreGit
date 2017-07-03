using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using NetSteps.Core.Cache.Config;
using NetSteps.Encore.Core;
using NetSteps.Encore.Core.Log;
using NetSteps.Encore.Core.Parallel;
using NetSteps.Encore.Core.Reflection;
using NetSteps.Encore.Core.Representation;
using NetSteps.Encore.Core.IoC;
using System.Collections.Generic;
using System.Configuration;

namespace NetSteps.Core.Cache
{
	/// <summary>
	/// Indicator flags used to return a status for an EvictionCandidate
	/// </summary>
	[Flags]
	internal enum ShouldEvict
	{
		None = 0,
		Yes = 1,
		No = 1 << 1,
		Expired = 1 << 2 | Yes,
		Revised = 1 << 3 | Yes,
		Cleared = 1 << 4 | No
	}

	/// <summary>
	/// Local memory cache implementation that keeps only the most recently used
	/// items in memory.
	/// </summary>
	/// <typeparam name="K">key type K</typeparam>
	/// <typeparam name="R">representation type R</typeparam>
	public class MruLocalMemoryCache<K, R> : Disposable, ICache<K, R>, ICacheMany<K, R>
		where R : class
	{
		static readonly ILogSink __log = typeof(MruLocalMemoryCache<K, R>).GetLogSink();

		/// <summary>
		/// Class representing a cached item.
		/// </summary>
		protected class Item
		{
			K _key;
			R _value;
			int _revision;
			DateTime _expiry;
			int _expirationObserved = 0;

			/// <summary>
			/// Constructs a new Cache Item.
			/// </summary>
			/// <param name="key">The cache key.</param>
			/// <param name="value">The cached value.</param>
			/// <param name="revision">Cache item revision.</param>
			/// <param name="lifespan">The TTL of the cached item.</param>
			public Item(K key, R value, int revision, TimeSpan lifespan)
			{
				this._key = key;
				this._value = value;
				this._revision = revision;
				this._expiry = DateTime.Now.Add(lifespan);
			}

			internal int Revision
			{
				get
				{
					Thread.MemoryBarrier();
					var value = _revision;
					Thread.MemoryBarrier();
					return value;
				}
			}

			internal K Key { get { return _key; } }

			internal R Value
			{
				get
				{
					Thread.MemoryBarrier();
					var value = _value;
					Thread.MemoryBarrier();
					return value;
				}
			}

			internal bool IsExpired(ICacheStatsCollector stats)
			{
				var revision = Thread.VolatileRead(ref _revision);
				var observed = Thread.VolatileRead(ref _expirationObserved);
				Thread.MemoryBarrier();
				var expire = _expiry;
				Thread.MemoryBarrier();
				bool expired = DateTime.Now > expire;
				// test:
				// 1. the item is expired
				// 2. we are the first thread to observe it
				// 3. we are the first thread to mark it observed
				// 4. and no concurrent thread revised the item
				if (stats != null
					&& expired
					&& observed == 0
					&& Interlocked.CompareExchange(ref _expirationObserved, 1, observed) == 0
					&& Thread.VolatileRead(ref _revision) == revision
					)
				{
					stats.IncrementExpired();
				}
				return expired;
			}

			internal bool TryUpdate(R newValue, R expected, TimeSpan lifespan)
			{
				Thread.MemoryBarrier();
				var existing = _value;
				Thread.MemoryBarrier();

				var result = Object.ReferenceEquals(existing, expected);
				if (result && Interlocked.CompareExchange(ref _value, newValue, existing) == existing)
				{
					var expire = DateTime.Now.Add(lifespan);
					Thread.MemoryBarrier();
					_expiry = expire;
					_expirationObserved = 0;
					Thread.MemoryBarrier();
					return true;
				}
				else
				{
					return false;
				}
			}

			internal void UpdateRevision(int revision)
			{
				Thread.MemoryBarrier();
				_revision = revision;
				Thread.MemoryBarrier();
			}

			internal R Remove(int revision)
			{
				Thread.MemoryBarrier();
				var existing = _value;
				Thread.MemoryBarrier();
				_value = default(R);
				_revision = revision;
				Thread.MemoryBarrier();
				return existing;
			}
		}

		#region Fields

		struct EvictionCandidate
		{
			int _captureRevision;
			readonly Item _item;

			public EvictionCandidate(Item item, int revision)
			{
				Contract.Requires(item != null);
				_item = item;
				_captureRevision = revision;
			}

			internal Item Item { get { return _item; } }

			internal ShouldEvict ShouldEvict(ICacheStatsCollector stats)
			{
				if (_item.Value == null)
				{
					return Cache.ShouldEvict.Cleared;
				}

				if (_item.Revision < _captureRevision)
				{
					return Cache.ShouldEvict.Revised;
				}

				if (_item.IsExpired(stats))
				{
					return Cache.ShouldEvict.Expired;
				}

				return Cache.ShouldEvict.No;
			}

			internal bool RevisionBefore(int revision)
			{
				return (_item.Revision < revision
					|| _item.Value == null);
			}
		}

		readonly ConcurrentDictionary<K, Item> _cache = new ConcurrentDictionary<K, Item>();
		readonly DateTime _initializationTimeStamp;

		readonly int _cacheDepth;
		readonly int _evictionThreshold;
		readonly int _evictionSynchronizedThreshold;
		readonly int _evictionsPerSynchronizedWorker;
		readonly MruCacheOptions _options;
		readonly ConcurrentQueue<EvictionCandidate> _evictionQueue = new ConcurrentQueue<EvictionCandidate>();
		readonly ConcurrentQueue<EvictionCandidate> _lruQueue = new ConcurrentQueue<EvictionCandidate>();
		readonly ICacheEvictionManager _evictionManager;

		ICacheEvictionMonitor _evictionMonitor;
		ICacheStatsCollector _stats;
		TimeSpan _cacheItemLifespan;
		int _evictions = 0;
		int _hits = 0;
		int _misses = 0;
		int _revision = 0;
		int _evictorCount = 0;
		int _activeEvictorCount = 0; 

		#endregion

		#region Properties

		/// <summary>
		/// The internal cache.
		/// </summary>
		protected ConcurrentDictionary<K, Item> InternalCache { get { return _cache; } }

		/// <summary>
		/// A timestamp indicating when the Cache was initialized.  Useful for internal housekeeping.
		/// </summary>
		protected DateTime InitializationTimeStamp { get { return _initializationTimeStamp; } }

		/// <summary>
		/// The cache's name. Must be unique within the process.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Gets the cache's depth.
		/// </summary>
		public int CacheDepth { get { return _cacheDepth; } }
		/// <summary>
		/// Gets the number of items in the cache.
		/// </summary>
		public int Count { get { return _cache.Count; } }
		/// <summary>
		/// Gets the number of items evicted from the cache.
		/// </summary>
		public int CacheEvictions { get { return Thread.VolatileRead(ref _evictions); } }
		/// <summary>
		/// Gets the number of cache hits.
		/// </summary>
		public int CacheHits { get { return Thread.VolatileRead(ref _hits); } }
		/// <summary>
		/// Gets the number of cache misses.
		/// </summary>
		public int CacheMisses { get { return Thread.VolatileRead(ref _misses); } }

		/// <summary>
		/// Gets the threshold at which concurrent eviction threads are 
		/// launched.
		/// </summary>
		/// <remarks>
		/// When the threshold is exceeded (cach count)
		/// concurrent eviction workers will be started to remove the
		/// least-recently-used items from the cache.
		/// </remarks>
		public decimal EvictionThreshold { get { return _evictionThreshold; } }

		/// <summary>
		/// Gets the threshold at which the caller's thread will be borrowed
		/// to process evictions.
		/// </summary>
		/// <remarks>
		/// When the threshold is exceeded the caller's thread will be borrowed 
		/// to process evictions. This threshold prevents the cache from growing
		/// when it encounters heavy writes of unique items greatly exceeding
		/// the eviction threshold. It limits the cache's growth when there are 
		/// more writer threads than eviction workers.
		/// </remarks>
		public decimal SynchronizedEvictionThreshold { get { return _evictionSynchronizedThreshold; } }

		/// <summary>
		/// Number of evictions processed per synchronized eviction event.
		/// </summary>
		/// <remarks>
		/// If the synchronization threshold is exceeded then a calling
		/// thread is borrowed to process evictions. This setting 
		/// indicates the max number of evictions the borrowed thread
		/// should process before returning the thread to the caller.
		/// </remarks>
		public int EvictionsPerSynchronizedWorker { get { return _evictionsPerSynchronizedWorker; } }

		/// <summary>
		/// The Options governing the behavior of this Cache.
		/// </summary>
		protected MruCacheOptions Options { get { return _options; } }

		#endregion

		#region Constructors

		/// <summary>
		/// Creates a new instance using the default cache options.
		/// </summary>
		/// <param name="name">the cache's name</param>
		public MruLocalMemoryCache(string name)
			: this(name, null, null, null)
		{
			Contract.Requires<ArgumentNullException>(name != null);
			Contract.Requires<ArgumentException>(name.Length > 0);
		}
		/// <summary>
		/// Creates a new instance using the default cache options.
		/// </summary>
		/// <param name="name">the cache's name</param>
		/// <param name="evictionManager">eviction manager</param>
		public MruLocalMemoryCache(string name, ICacheEvictionManager evictionManager)
			: this(name, null, null, evictionManager)
		{
			Contract.Requires<ArgumentNullException>(name != null);
			Contract.Requires<ArgumentException>(name.Length > 0);
			Contract.Requires<ArgumentNullException>(evictionManager != null);
		}
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="name">the cache's name</param>
		/// <param name="options">options</param>
		public MruLocalMemoryCache(string name, MruCacheOptions options)
			: this(name, options, null, null)
		{
			Contract.Requires<ArgumentNullException>(name != null);
			Contract.Requires<ArgumentException>(name.Length > 0);
		}
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="name">the cache's name</param>
		/// <param name="options">options</param>
		/// <param name="evictionManager">eviction manager</param>
		public MruLocalMemoryCache(string name,
			MruCacheOptions options, ICacheEvictionManager evictionManager)
			: this(name, options, null, evictionManager)
		{
			Contract.Requires<ArgumentNullException>(name != null);
			Contract.Requires<ArgumentException>(name.Length > 0);
			Contract.Requires<ArgumentNullException>(evictionManager != null);
		}
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="name">the cache's name</param>
		/// <param name="stats">statistics collector</param>
		/// <param name="options">options</param>
		public MruLocalMemoryCache(string name,
			MruCacheOptions options, ICacheStatsCollector stats)
			: this(name, options, stats, null)
		{
			Contract.Requires<ArgumentNullException>(name != null);
			Contract.Requires<ArgumentException>(name.Length > 0);
		}
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="name">the cache's name</param>
		/// <param name="stats">statistics collector</param>
		/// <param name="options">options</param>
		/// <param name="evictionManager">eviction manager</param>
		public MruLocalMemoryCache(string name,
			MruCacheOptions options, ICacheStatsCollector stats, ICacheEvictionManager evictionManager)
		{
			Contract.Requires<ArgumentNullException>(name != null);
			Contract.Requires<ArgumentException>(name.Length > 0);

			this.Name = name;

			var resolvedOptions = options ?? CacheConfigSection.Current.NamedOrDefaultOptions<R>(name);
			_options = resolvedOptions;
			_stats = stats ?? new CacheStats(name);
			_cacheDepth = resolvedOptions.CacheDepth;
			_evictionThreshold = Convert.ToInt32(Math.Round(_cacheDepth * resolvedOptions.ConcurrentEvictionFactor));
			_evictionSynchronizedThreshold = Convert.ToInt32(Math.Round(_evictionThreshold * resolvedOptions.SynchronizedEvictionFactor));
			_evictionsPerSynchronizedWorker = resolvedOptions.EvictionsPerSynchronizedWorker;
			_cacheItemLifespan = resolvedOptions.CacheItemLifespan;
			_evictionManager = evictionManager ?? Create.New<ICacheEvictionManager>();
			var contextKeys = resolvedOptions.ContextKeys;
			if (contextKeys != null && contextKeys.Count() > 0)
			{
				_evictionMonitor = new ReactiveCacheEvictionMonitor<K>(contextKeys, EvictionReactorCallback);
				_evictionManager.AddEvictionMonitor(_evictionMonitor);
			}

			_initializationTimeStamp = DateTime.Now;
		} 

		#endregion

		private TimeSpan CalculateItemTTL()
		{
			TimeSpan itemTTL = _cacheItemLifespan;
			if (Options.FullActive 
				&& ((DateTime.Now - InitializationTimeStamp).TotalMilliseconds < (_cacheItemLifespan.TotalMilliseconds * 0.25)))
			{
				//Inside the early initialization period... randomize item TTL to stager reloads.
				double newTTL = _cacheItemLifespan.TotalMilliseconds;

				//I don't expect cache lifespans to cause an integer overflow... but just in case.
				try
				{
					newTTL = (new Random()).Next((int)Math.Ceiling((_cacheItemLifespan.TotalMilliseconds * 0.25)), (int)Math.Floor(_cacheItemLifespan.TotalMilliseconds));
				}
				catch (Exception ex)
				{
					__log.Error(ex.Message);
				}
				itemTTL = TimeSpan.FromMilliseconds(newTTL);
			}
			return itemTTL;
		}

		/// <summary>
		/// Tries to add an item.
		/// </summary>
		/// <param name="key">the item's key</param>
		/// <param name="value">the item's representation</param>
		/// <returns>true if added; otherwise false.</returns>
        public bool TryAdd(K key, R value)
        {
            bool manejaElasticache = Convert.ToBoolean(ConfigurationManager.AppSettings["ManejaElasticache"]);
            if (!manejaElasticache)
                return TryAdd(key, value, true);
            else
            {
                UtilElastiCache objElasticache = new UtilElastiCache();
                return objElasticache.GuardarCacheTime(key, value, CalculateItemTTL());
            }
        }

		bool TryAdd(K key, R value, bool incrementCounters)
		{
			var item = new Item(key, value, Thread.VolatileRead(ref _revision), CalculateItemTTL());
			var result = _cache.TryAdd(key, item);
			if (result)
			{
				if (incrementCounters)
				{
					_stats.IncrementWrites();
				}
				MostRecentlyUsed(item);
			}
			return result;
		}

		/// <summary>
		/// Tries to update an expected item.
		/// </summary>
		/// <param name="key">the item's key</param>
		/// <param name="value">the item</param>
		/// <param name="expected">the expected value used for comparison</param>
		/// <returns>true if the item was equal to the expected value and replaced; otherwise false</returns>
		public bool TryUpdate(K key, R value, R expected)
		{
			Item item;
			if (_cache.TryGetValue(key, out item))
			{
				if (item.TryUpdate(value, expected, CalculateItemTTL()))
				{
					_stats.IncrementWrites();
					MostRecentlyUsed(item);
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Tries to remove an item.
		/// </summary>
		/// <param name="key">the item's key</param>
		/// <param name="removed">variable where the removed item will be returned upon success</param>
		/// <returns>true if the item was removed; otherwise false</returns>
		public bool TryRemove(K key, out R removed)
		{
			if (InternalTryRemove(key, out removed))
			{
				_stats.IncrementRemoves();
				return true;
			}
			return false;
		}

		/// <summary>
		/// Removes the item without incrementing the remove counter.
		/// </summary>
		protected bool InternalTryRemove(K key, out R removed)
		{
			Item item;
			var result = _cache.TryRemove(key, out item);
			if (result)
			{
				removed = item.Remove(Interlocked.Increment(ref _revision));
				return true;
			}
			removed = default(R);
			return result;
		}

		/// <summary>
		/// Tries to get an item.
		/// </summary>
		/// <param name="key">the item's key</param>
		/// <param name="value">variable where the item will be returned upon success</param>
		/// <returns>true if the item was retrieved; otherwise false</returns>
        public bool TryGet(K key, out R value)
        {
            bool manejaElasticache = Convert.ToBoolean(ConfigurationManager.AppSettings["ManejaElasticache"]);
            if (!manejaElasticache)
            {
                _stats.IncrementReadAttempts();
                Item item;
                R existing = default(R), resolved;
                if (_cache.TryGetValue(key, out item))
                {
                    existing = item.Value;
                    if (!item.IsExpired(_stats) && existing != null)
                    {
                        Interlocked.Increment(ref _hits);
                        _stats.IncrementHits();
                        MostRecentlyUsed(item, true);
                        value = existing;
                        return true;
                    }
                }

                // Second-chance... try to resolve...
                var rkind = TryResolveMissingItem(key, out resolved);
                UpdateResolveCounters(_stats, rkind);
                if (rkind == ResolutionKind.Created)
                {   // trust the newly resolved value...		
                    item = new Item(key, resolved, Thread.VolatileRead(ref _revision), CalculateItemTTL());
                    _cache.AddOrUpdate(key, item, (y, z) => item);
                    if (__log.IsLogging(SourceLevels.Verbose))
                        __log.Verbose(() => new { Message = "Resolved item via resolver", Key = key });
                }
                value = resolved;
                return rkind.HasFlag(ResolutionKind.Resolved);
            }
            else
            {
                UtilElastiCache objElasticache = new UtilElastiCache();
                if (!objElasticache.LeerCache(key, out value))
                {
                    value = null;
                    TryResolveMissingItem(key, out value);
                    if (value != null)
                    {
                        try
                        {
                            objElasticache.GuardarCacheTime(key, value, CalculateItemTTL());
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                if (value == null)
                    return false;
                else
                    return true;
            }
        }

		/// <summary>
		/// Updates statistics when items are resolved.
		/// </summary>
		/// <param name="stats">statistics collector</param>
		/// <param name="kind">kind of resolution</param>
		protected virtual void UpdateResolveCounters(ICacheStatsCollector stats, ResolutionKind kind)
		{
			if (kind.HasFlag(ResolutionKind.Resolved))
			{
				Interlocked.Increment(ref _hits);
				_stats.IncrementHits();
			}
			else
			{
				Interlocked.Increment(ref _misses);
			}
		}

		/// <summary>
		/// Updates statistics when items are resolved.
		/// </summary>
		/// <param name="stats">statistics collector</param>
		/// <param name="kind">kind of resolution</param>
		/// <param name="increment">The number of resolved or failed items</param>
		protected virtual void UpdateResolveManyCounters(ICacheStatsCollector stats, ResolutionManyKind kind, int increment = 1)
		{
			if (kind.HasFlag(ResolutionManyKind.Resolved) || kind.HasFlag(ResolutionManyKind.PartiallyResolved))
			{
				Interlocked.Add(ref _hits, increment);
				_stats.IncrementHits();
			}
			else
			{
				Interlocked.Add(ref _misses, increment);
			}
		}

		/// <summary>
		/// Provides subclasses the ability to resolve missing items.
		/// Default behavior does nothing and returns false.
		/// </summary>
		/// <param name="key">the item's key</param>
		/// <param name="value">variable where the item will be returned upon success</param>
		/// <returns>ResolutionKind.Created or ResolutionKind.Resolved if the item was resolved; otherwise ResolutionKind.None</returns>		
		/// <see cref="ResolutionKind"/>
		protected virtual ResolutionKind TryResolveMissingItem(K key, out R value)
		{
			value = default(R);
			return ResolutionKind.None;
		}

		/// <summary>
		/// Provides subclasses the ability to resolve a group of missing items.
		/// Default behavior does nothing and returns false.
		/// </summary>
		/// <param name="keys">the item's keys</param>
		/// <param name="values">variable where the items will be returned upon success</param>
		/// <returns>ResolutionKind.Created or ResolutionKind.Resolved if the item was resolved; otherwise ResolutionKind.None</returns>		
		/// <see cref="ResolutionKind"/>
		protected virtual ResolutionManyKind TryResolveManyMissingItems(IEnumerable<K> keys, out IEnumerable<KeyValuePair<K, R>> values)
		{
			values = default(IEnumerable<KeyValuePair<K, R>>);
			return ResolutionManyKind.None;
		}

		/// <summary>
		/// Flushes all items from the cache.
		/// </summary>
		public void FlushAll()
		{
			_cache.Clear();
		}

		internal ICacheStatsCollector Stats { get { return _stats; } }

		class EvictionOptions
		{
			public bool Concurrent { get; set; }
			public int SynchronizedEvictions { get; set; }
		}
		void MostRecentlyUsed(Item item, bool reading = false)
		{
			var rev = reading ? Thread.VolatileRead(ref _revision) : Interlocked.Increment(ref _revision);
			item.UpdateRevision(rev);
			_lruQueue.Enqueue(new EvictionCandidate(item, rev));

			EvictionCandidate evictionCandidate;
			if (_lruQueue.Count > _cacheDepth && _lruQueue.TryDequeue(out evictionCandidate)
				&& evictionCandidate.RevisionBefore(rev - _cacheDepth))
			{
				_evictionQueue.Enqueue(new EvictionCandidate(evictionCandidate.Item, rev));
				if (_evictionQueue.Count > _evictionSynchronizedThreshold)
				{
					Interlocked.Increment(ref _evictorCount);
					ProcessEvictions(true);
				}
				else if (Interlocked.CompareExchange(ref _evictorCount, 1, 0) == 0)
				{
					_evictionManager.AddEvictionCallback(ProcessEvictions);
				}
			}
		}

		string What
		{
			get
			{
				return Util.NonBlockingLazyInitializeVolatile(ref _what, () => this.GetType().GetReadableFullName());
			}
		}
		string _what;

		bool ProcessEvictions(bool isSynchronous)
		{
			int items = 0, evictions = 0, remaining = 0;
			bool reschedule = false;
			try
			{
				if (isSynchronous)
				{
					if (__log.IsLogging(SourceLevels.Verbose))
					{
						__log.Verbose(String.Concat(this.What, ".ProcessEvictions: synchronous worker started"));
					}
				}
				else
				{
					Interlocked.Increment(ref _activeEvictorCount);
				}
				EvictionCandidate candidate;
				while (_evictionQueue.TryDequeue(out candidate))
				{
					items += 1;
					ShouldEvict evict = candidate.ShouldEvict(_stats);
					if (evict.HasFlag(ShouldEvict.Yes))
					{
						R removed;
						if (InternalTryRemove(candidate.Item.Key, out removed))
						{
							Interlocked.Increment(ref _evictions);
							evictions++;
						}
					}
					if (isSynchronous && items > _evictionsPerSynchronizedWorker)
						break;
				}
			}
			finally
			{
				Interlocked.Add(ref _evictions, evictions);
				_stats.IncrementEvictions(evictions);
				if (__log.IsLogging(SourceLevels.Verbose))
				{
					__log.Verbose(String.Concat(this.What, "ProcessEvictions: worker done; processed ", items, " items and evicted ", evictions));
				}
				remaining = _evictionQueue.Count;
				if (isSynchronous)
				{
					Interlocked.Decrement(ref _evictorCount);
				}
				else
				{
					// Only reschedule when we are the last background thread observing
					// that there are remaining items.
					if (Interlocked.Decrement(ref _activeEvictorCount) == 0)
					{
						reschedule = remaining > 0;
						if (!reschedule) Interlocked.Decrement(ref _evictorCount);
					}
				}
			}

			return reschedule;
		}

		/// <summary>
		/// Invoked by the framework when eviction notifications arrive.
		/// </summary>
		/// <param name="reactor">the reactor</param>
		/// <param name="key">the key to be evicted</param>
		protected virtual void EvictionReactorCallback(Reactor<K> reactor, K key)
		{
			R removed;
			if (TryRemove(key, out removed))
			{
				Interlocked.Increment(ref _evictions);
				_stats.IncrementEvictions();
			}
		}

		/// <summary>
		/// Performs dispose logic.
		/// </summary>
		/// <param name="disposing">whether called from Dispose method</param>
		/// <returns>true if disposed as a result of this call</returns>
		protected override bool PerformDispose(bool disposing)
		{
			Util.Dispose(ref _stats);
			if (_evictionMonitor != null)
			{
				_evictionManager.RemoveEvictionMonitor(_evictionMonitor.RegistrationKey);
				_evictionMonitor.Dispose();
				_evictionMonitor = null;
			}
			return true;
		}

		/// <summary>
		/// Creates a cache axis.
		/// </summary>
		/// <typeparam name="AK">axisKey type AK</typeparam>
		/// <param name="name">the axis name</param>
		/// <param name="transform">a transform for getting axis keys from representations</param>
		/// <returns>a cache axis</returns>
		public ICacheAxis<K, AK, R> CreateAxis<AK>(string name, IRepresentation<R, AK> transform)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Gets a cache axis by name.
		/// </summary>
		/// <typeparam name="AK">axisKey type AK</typeparam>
		/// <param name="name">the axis name</param>
		/// <returns>a cache axis</returns>
		public ICacheAxis<K, AK, R> GetAxis<AK>(string name)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Tries to get an item by an axis key.
		/// </summary>
		/// <typeparam name="AK">axis key type AK</typeparam>
		/// <param name="axisName">the name of the axis</param>
		/// <param name="axisKey">the axis key</param>
		/// <param name="value">variable where the item will be returned upon success</param>
		/// <returns>true if the item is present in the cache; otherwise false</returns>
		public bool TryGetOnAxis<AK>(string axisName, AK axisKey, out R value)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Tries to add items.
		/// </summary>
		/// <param name="values">the items representations</param>
		/// <param name="failedKeys">the items keys that we unable to be added</param>
		/// <returns>true if added; otherwise false.</returns>
		public bool TryAddMany(IEnumerable<KeyValuePair<K, R>> values, out IEnumerable<K> failedKeys)
		{
			failedKeys = null;

			foreach(var item in values)
			{
				if (!TryAdd(item.Key, item.Value))
				{
					if (failedKeys == null) failedKeys = new List<K>();
					((List<K>)failedKeys).Add(item.Key);
				}
			}

			return failedKeys == null || !failedKeys.Any();
		}
		
		/// <summary>
		/// Tries to add items.
		/// </summary>
		/// <param name="values">the items representations</param>
		/// <returns>true if added; otherwise false.</returns>
		public bool TryAddAny(IEnumerable<KeyValuePair<K, R>> values)
		{
			IEnumerable<K> failedKeys;
			if (!TryAddMany(values, out failedKeys))
				return !(failedKeys.Count() < values.Count());
			return true;
		}

		/// <summary>
		/// Tries to add items.
		/// </summary>
		/// <param name="values">the items representations</param>
		/// <param name="expected">the expected values used for comparison</param>
		/// <returns>true if all updated; otherwise false.</returns>
		public bool TryUpdateMany(IEnumerable<KeyValuePair<K, R>> values, IEnumerable<KeyValuePair<K, R>> expected)
		{
			bool result = true;

			foreach(var val in values)
			{
				R currentValue = val.Value;
				K currentKey = val.Key;
				R currentExpected;

				if (typeof(IEquatable<K>).IsAssignableFrom(typeof(K)))
				{
					currentExpected = expected.Where(e => currentKey.Equals(e.Key)).Select(e => e.Value).FirstOrDefault();
					if (currentExpected != null)
					{
						if (!TryUpdate(currentKey, currentValue, currentExpected))
						{
							result = false;
						}
					}
					else
					{
						result = false;
					}
				}	
			}

			return result;
		}

		/// <summary>
		/// Removes items from the cache
		/// </summary>
		/// <param name="keys">the items keys</param>
		/// <param name="removed">the items removed</param>
		/// <returns>true if the items were removed; otherwise false</returns>
		public bool TryRemoveMany(IEnumerable<K> keys, out IEnumerable<R> removed)
		{
			bool result = false;
			removed = null;
			foreach (var key in keys)
			{
				R rem;
				if (TryRemove(key, out rem))
				{
					if (removed == null) removed = new List<R>();
					((List<R>)removed).Add(rem);
				}
			}

			result = (removed != null && removed.Count() == keys.Count());

			return result;
		}

		/// <summary>
		/// Tries to get items.
		/// </summary>
		/// <param name="keys">the items keys</param>
		/// <param name="values">variable where the items will be returned upon success</param>
		/// <returns>true if all of the items were retrieved; otherwise false</returns>
		public bool TryGetMany(IEnumerable<K> keys, out IEnumerable<R> values)
		{
			values = null;
			List<K> missedKeys = new List<K>();
			List<R> results = new List<R>();

			R existing = default(R);
			Item item;

			foreach (var k in keys)
			{
				_stats.IncrementReadAttempts();
				if (_cache.TryGetValue(k, out item))
				{
					existing = item.Value;
					if (!item.IsExpired(_stats) && existing != null)
					{
						Interlocked.Increment(ref _hits);
						_stats.IncrementHits();
						MostRecentlyUsed(item, true);
						results.Add(existing);
					}
					else missedKeys.Add(k);
				}
				else missedKeys.Add(k);
			}

			if (results.Count() == keys.Count())
			{
				values = results;
				return true;
			}

			// Second-chance... try to resolve...
			IEnumerable<KeyValuePair<K, R>> resolvedItems;
			var rkind = TryResolveManyMissingItems(missedKeys, out resolvedItems);
			UpdateResolveManyCounters(_stats, rkind, resolvedItems.Count());
			if (rkind.HasFlag(ResolutionManyKind.Resolved))
			{
				foreach (var resolved in resolvedItems)
				{
					K key = resolved.Key;
					R val = resolved.Value;
					// trust the newly resolved value...		
					item = new Item(key, val, Thread.VolatileRead(ref _revision), CalculateItemTTL());
					_cache.AddOrUpdate(key, item, (k, r) => item);
					if (__log.IsLogging(SourceLevels.Verbose))
						__log.Verbose(() => new { Message = "Resolved item via resolver", Key = key });

					results.Add(val);
				}
			}

			values = results;

			return rkind.HasFlag(ResolutionManyKind.Resolved);
		}

		/// <summary>
		/// Tries to get any of the items.
		/// </summary>
		/// <param name="keys">the item's keys</param>
		/// <param name="values">variable where the items will be returned upon success</param>
		/// <returns>true if any of the items were retrieved; otherwise false</returns>
		public bool TryGetAny(IEnumerable<K> keys, out IEnumerable<R> values)
		{
			TryGetMany(keys, out values);
			return values != null && values.Any();
		}
	}
}
