using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NetSteps.Core.Cache;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Core.Cache.Tests
{
	[TestClass]
	public class ActiveMruCacheTests
	{
		class Item
		{
			int _hits = 0;

			public Item(int id)
			{
				this.ID = id;
			}
			public int ID { get; private set; }
			public string Name { get { return String.Concat("Item: ", Convert.ToString(ID)); } }
			public int Hits { get { return Thread.VolatileRead(ref _hits); } }
			public void Increment()
			{
				Interlocked.Increment(ref _hits);
			}
		}
		class ThreadState
		{
			public int Iterations { get; set; }
			public ICache<string, Item> Cache { get; set; }
			public Task Task { get; set; }
			public TimeSpan RunningTime { get; set; }
			public int MaxItemID { get; set; }
		}

		class ItemDemuxResolver : DemuxCacheItemResolver<string, Item>
		{
			bool _useSimulatedLatency;
			int _latencyMinMS;
			int _latencyMaxMS;
			int _totalSimulatedLatencyMS = 0;

			public ItemDemuxResolver()
				: this(false, 0, 0)
			{
			}
			public ItemDemuxResolver(bool useSimulatedLatency, int latencyMinMS, int latencyMaxMS)
			{
				_useSimulatedLatency = useSimulatedLatency;
				if (useSimulatedLatency)
				{
					_latencyMinMS = latencyMinMS;
					_latencyMaxMS = latencyMaxMS;
				}
			}
			public int TotalSimulatedLatencyMS { get { return _totalSimulatedLatencyMS; } }

			protected override bool DemultiplexedTryResolve(string key, out Item value)
			{
				int id = Int32.Parse(key.Split(':')[1].TrimStart());
				if (_useSimulatedLatency)
				{
					var wait = new Random(Environment.TickCount).Next(_latencyMinMS, _latencyMaxMS);
					Thread.Sleep(wait); // simulated latency
					Interlocked.Add(ref _totalSimulatedLatencyMS, wait);
				}
				value = new Item(id);
				return true;
			}
		}

		void BG_ThreadGetOrAdd(object state)
		{
			ThreadState th = (ThreadState)state;
			Random rand = new Random(Environment.TickCount);
			var cache = th.Cache;
			var iter = th.Iterations;
			var len = th.MaxItemID;
			for (var i = 0; i < iter; i++)
			{
				var ii = rand.Next(0, len);
				var name = String.Concat("Item: ", Convert.ToString(ii));
				Item existing;
				// The resolver should always get an item not in the cache
				Assert.IsTrue(cache.TryGet(name, out existing));
				existing.Increment();
				if (ii % 3 == 0)
				{ // pulse/yield every 3 items so that a single thread doesn't 
					// monopolize the activity on the cache.
					Thread.Sleep(0);
				}
			}
		}

		/// <summary>
		/// This test forces cache misses in order to test the ItemDemuxResolver and 
		/// its ability to multiplex results to calling threads. It should run at a high
		/// resolve ratio and due to the lack of latency should resolve few of the total
		/// number of items resolved.
		/// </summary>
		[TestMethod]
		public void CanGetAndAddWith100PercentMoreItemsThanCacheUsingDemuxResolverWithoutSimulatedLatency()
		{
			var test = new
			{
				Iterations = 10000,
				Threads = 2 * Environment.ProcessorCount,
				MaxCacheDepth = 1000,
				ItemArrayCount = 2000
			};

			// The resolver we're attaching will provide all items to the cache 
			// when cache misses occur. If multiple threads request the same item
			// while it is being resolved, they will convoy on the result - this is
			// the intent of the Demux logic - when the requested item is resolved
			// it is given to all waiting threads.
			//
			// The best way to think of what is happening here is an active cache
			// is smart enough to resolve missing items (thus "active") and the
			// demux resolver is smart enough to only issue a request once to 
			// whatever backend is providing the items (such as a database).
			//
			// In other words: it is a select-through-cache.
			var resolver = new ItemDemuxResolver();
			using (var container = Create.SharedOrNewContainer())
			{
				var cache = new ActiveMruLocalMemoryCache<string, Item>("Testing Active MRU Cache",
					new MruCacheOptions { CacheDepth = test.MaxCacheDepth }, resolver);

				Console.WriteLine(String.Concat("Number of threads using the cache: ", test.Threads));

				var state = new ThreadState[test.Threads];
				for (int i = 0; i < test.Threads; i++)
				{
					var th = new ThreadState { MaxItemID = test.ItemArrayCount, Cache = cache, Iterations = test.Iterations };
					state[i] = th;
					th.Task = new Task(BG_ThreadGetOrAdd, th);
					th.Task.Start();
				}

				foreach (var s in state)
				{
					s.Task.Wait();
				}

				decimal total = Convert.ToDecimal(cache.CacheHits + cache.CacheMisses);

				Console.WriteLine(String.Concat("CacheStats: Count = ", Convert.ToString(cache.Count), Environment.NewLine
						, ", Hits: ", cache.CacheHits, Environment.NewLine
						, ", Hit %: ", Math.Round(cache.CacheHits / total * 100), Environment.NewLine
						, ", Misses: ", cache.CacheMisses, Environment.NewLine
						, ", Miss %: ", Math.Round(cache.CacheMisses / total * 100m), Environment.NewLine
						, ", Evictions: ", cache.CacheEvictions.ToString("N"), Environment.NewLine
						, ", Resolver's total simulated latency: ", TimeSpan.FromMilliseconds(resolver.TotalSimulatedLatencyMS).ToString()
						));

			}
		}

		/// <summary>
		/// This test forces cache misses in order to test the ItemDemuxResolver and 
		/// its ability to multiplex results to calling threads. The simulated latency
		/// should force demultiplex logic. The cache lifespan should force expirations.
		/// The resolve ratio should be somewhere near 50% (+-10%)
		/// </summary>
		[TestMethod]
		public void CanGetAndAddWith100PercentMoreItemsThanCacheUsingDemuxResolverWithSimulatedLatency()
		{
			var test = new
			{
				Iterations = 1000,
				Threads = 2 * Environment.ProcessorCount,
				MaxCacheDepth = 100,
				ItemArrayCount = 200
			};

			// The resolver we're attaching will provide all items to the cache 
			// when cache misses occur. If multiple threads request the same item
			// while it is being resolved, they will convoy on the result - this is
			// the intent of the Demux logic - when the requested item is resolved
			// it is given to all waiting threads.
			//
			// The best way to think of what is happening here is an active cache
			// is smart enough to resolve missing items (thus "active") and the
			// demux resolver is smart enough to only issue a request once to 
			// whatever backend is providing the items (such as a database).
			//
			// In other words: it is a select-through-cache.
			var resolver = new ItemDemuxResolver(true, 10, 100);
			var options = new MruCacheOptions
			{
				CacheItemLifespan = TimeSpan.FromMilliseconds(99) // force occasional/infrequent expiration
				,
				CacheDepth = test.MaxCacheDepth
			};
			var cache = new ActiveMruLocalMemoryCache<string, Item>("Testing Active MRU Cache",
				options, resolver, Create.New<ICacheEvictionManager>());

			Console.WriteLine(String.Concat("Number of threads using the cache: ", test.Threads));

			var state = new ThreadState[test.Threads];
			for (int i = 0; i < test.Threads; i++)
			{
				var th = new ThreadState { MaxItemID = test.ItemArrayCount, Cache = cache, Iterations = test.Iterations };
				state[i] = th;
				th.Task = new Task(BG_ThreadGetOrAdd, th);
				th.Task.Start();
			}

			foreach (var s in state)
			{
				s.Task.Wait();
			}
			decimal total = Convert.ToDecimal(cache.CacheHits + cache.CacheMisses);

			Console.WriteLine(String.Concat("CacheStats: Count = ", Convert.ToString(cache.Count), Environment.NewLine
					, ", Hits: ", cache.CacheHits, Environment.NewLine
					, ", Hit %: ", Math.Round(cache.CacheHits / total * 100), Environment.NewLine
					, ", Misses: ", cache.CacheMisses, Environment.NewLine
					, ", Miss %: ", Math.Round(cache.CacheMisses / total * 100m), Environment.NewLine
					, ", Evictions: ", cache.CacheEvictions.ToString("N"), Environment.NewLine
					, ", Resolver's total simulated latency: ", TimeSpan.FromMilliseconds(resolver.TotalSimulatedLatencyMS).ToString()
					));
		}

		/// <summary>
		/// This test forces cache misses in order to test the ItemDemuxResolver and 
		/// its ability to multiplex results to calling threads. The simulated latency
		/// should force demultiplex logic. The cache lifespan should force expirations.
		/// The resolve ratio should be somewhere near 50% (+-10%)
		/// </summary>
		[TestMethod]
		public void FullActiveCacheTestWithSimulatedLatency()
		{
			var test = new
			{
				Iterations = 10000,
				Threads = 2 * Environment.ProcessorCount,
				MaxCacheDepth = 1000,
				ItemArrayCount = 200
			};

			// The resolver we're attaching will provide all items to the cache 
			// when cache misses occur. If multiple threads request the same item
			// while it is being resolved, they will convoy on the result - this is
			// the intent of the Demux logic - when the requested item is resolved
			// it is given to all waiting threads.
			//
			// The best way to think of what is happening here is an active cache
			// is smart enough to resolve missing items (thus "active") and the
			// demux resolver is smart enough to only issue a request once to 
			// whatever backend is providing the items (such as a database).
			//
			// In other words: it is a select-through-cache.
			var resolver = new ItemDemuxResolver(true, 10, 100);
			var options = new MruCacheOptions
			{
				CacheItemLifespan = TimeSpan.FromMilliseconds(60000),
				CacheDepth = test.MaxCacheDepth,
				FullActive = true
			};
			var cache = new ActiveMruLocalMemoryCache<string, Item>("Testing Active MRU Cache - FullActive",
				options, resolver, Create.New<ICacheEvictionManager>());

			Console.WriteLine(String.Concat("Number of threads using the cache: ", test.Threads));

			var state = new ThreadState[test.Threads];
			for (int i = 0; i < test.Threads; i++)
			{
				var th = new ThreadState { MaxItemID = test.ItemArrayCount, Cache = cache, Iterations = test.Iterations };
				state[i] = th;
				th.Task = new Task(BG_ThreadGetOrAdd, th);
				th.Task.Start();
			}

			foreach (var s in state)
			{
				s.Task.Wait();
			}

			Thread.Sleep(150000);

			decimal total = Convert.ToDecimal(cache.CacheHits + cache.CacheMisses);

			Console.WriteLine(String.Concat("CacheStats: Count = ", Convert.ToString(cache.Count), Environment.NewLine
					, ", Hits: ", cache.CacheHits, Environment.NewLine
					, ", Hit %: ", Math.Round(cache.CacheHits / total * 100), Environment.NewLine
					, ", Misses: ", cache.CacheMisses, Environment.NewLine
					, ", Miss %: ", Math.Round(cache.CacheMisses / total * 100m), Environment.NewLine
					, ", Evictions: ", cache.CacheEvictions.ToString("N"), Environment.NewLine
					, ", Resolver's total simulated latency: ", TimeSpan.FromMilliseconds(resolver.TotalSimulatedLatencyMS).ToString()
					));
		}
	}
}