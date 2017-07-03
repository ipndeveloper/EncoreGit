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
	public class MruCacheTests
	{
		class Item
		{
			static int __itemCount = 0;
			int _hits = 0;

			public Item()
			{
				this.Name = String.Concat("Item: ", Convert.ToString(Interlocked.Increment(ref __itemCount)));
			}

			public string Name { get; private set; }
			public int Hits { get { return Thread.VolatileRead(ref _hits); } }
			void Increment()
			{
				Interlocked.Increment(ref _hits);
			}
		}
		class ThreadState
		{
			public int Iterations { get; set; }
			public Item[] Items { get; set; }
			public int HighWaterMark { get; set; }
			public MruLocalMemoryCache<string, Item> Cache { get; set; }
			public Task Task { get; set; }

			public int Misses { get; set; }

			public int Hits { get; set; }

			public int Adds { get; set; }
			public int AddMisses { get; set; }

			public TimeSpan RunningTime { get; set; }
		}

		public struct Totals
		{
			public int Operations;
			public int Adds;
			public int Hits;
			public int Misses;
			public int AddMisses;
		}

		[TestMethod]
		public void Removing_By_Key_Should_Not_Cause_Early_Evictions()
		{
			var test = new
			{
				Iterations = 1000000,
				Threads = 2 * Environment.ProcessorCount,
				MaxCacheDepth = 1000,
				ItemArrayCount = 950
			};

			Totals totals = new Totals();
			var cache = new MruLocalMemoryCache<string, Item>("Testing MRU Cache", new MruCacheOptions { CacheDepth = test.MaxCacheDepth }, Create.New<ICacheEvictionManager>());

			var items = new Item[test.ItemArrayCount];
			for (int i = 0; i < test.ItemArrayCount; i++)
			{
				items[i] = new Item();
			}

			for (int l = 0; l < 2; l++)
			{
				var state = new ThreadState[test.Threads];
			
				for (int i = 0; i < test.Threads; i++)
				{
					var th = new ThreadState { Items = items, Cache = cache, Iterations = test.Iterations };
					state[i] = th;
					th.Task = new Task(Background_ThreadGetOrAddLessRandom, th);
					th.Task.Start();
				}

				foreach (var s in state)
				{
					s.Task.Wait();
					Console.WriteLine(String.Concat("High: ", Convert.ToString(s.HighWaterMark)
						, ", Hits: ", Convert.ToString(s.Hits)
						, ", Misses: ", Convert.ToString(s.Misses)
						, ", Adds: ", Convert.ToString(s.Adds)
						, ", AddMisses: ", Convert.ToString(s.AddMisses)
						));
				}

				totals.Operations += state.Sum(i => i.Hits + i.Misses + i.AddMisses);
				totals.Adds += state.Sum(i => i.Adds);
				totals.Hits = state.Sum(i => i.Hits);
				totals.Misses = state.Sum(i => i.Misses);
				totals.AddMisses = state.Sum(i => i.AddMisses);
			}

			decimal total = Convert.ToDecimal(cache.CacheHits + cache.CacheMisses);
			Console.WriteLine(String.Concat("CacheStats: Count = ", Convert.ToString(cache.Count), Environment.NewLine
				, ", Total: ", total.ToString("N"), Environment.NewLine
				, ", Ops: ", totals.Operations.ToString("N"), Environment.NewLine
				, ", Adds: ", totals.Adds.ToString("N"), Environment.NewLine
				, ", Hits: ", totals.Hits.ToString("N"), Environment.NewLine
				, ", Misses: ", totals.Misses.ToString("N"), Environment.NewLine
				, ", AddMisses: ", totals.AddMisses.ToString("N"), Environment.NewLine
				, ", Hit %: ", Math.Round(cache.CacheHits / total * 100), Environment.NewLine
				, ", Miss %: ", Math.Round(cache.CacheMisses / total * 100m), Environment.NewLine
				, ", Evictions: ", cache.CacheEvictions.ToString("N"), Environment.NewLine
				));


			Assert.AreEqual(cache.CacheEvictions, 0);
		}

		[TestMethod]
		public void HalfFullCacheCausesEarlyEvictions()
		{
			var test = new
			{
				Iterations = 1000000,
				Threads = 2 * Environment.ProcessorCount,
				MaxCacheDepth = 1000,
				ItemArrayCount = 950
			};

			var cache = new MruLocalMemoryCache<string, Item>("Testing MRU Cache", new MruCacheOptions { CacheDepth = test.MaxCacheDepth },
				Create.New<ICacheEvictionManager>());

			var items = new Item[test.ItemArrayCount];
			for (int i = 0; i < test.ItemArrayCount; i++)
			{
				items[i] = new Item();
			}
			var state = new ThreadState[test.Threads];
			for (int i = 0; i < test.Threads; i++)
			{
				var th = new ThreadState { Items = items, Cache = cache, Iterations = test.Iterations };
				state[i] = th;
				th.Task = new Task(Background_ThreadGetOrAdd, th);
				th.Task.Start();
			}

			foreach (var s in state)
			{
				s.Task.Wait();
				Console.WriteLine(String.Concat("High: ", Convert.ToString(s.HighWaterMark)
					, ", Hits: ", Convert.ToString(s.Hits)
					, ", Misses: ", Convert.ToString(s.Misses)
					, ", Adds: ", Convert.ToString(s.Adds)
					, ", AddMisses: ", Convert.ToString(s.AddMisses)
					));
			}

			var totals = new
			{
				Operations = state.Sum(i => i.Hits + i.Misses + i.AddMisses),
				Adds = state.Sum(i => i.Adds),
				Hits = state.Sum(i => i.Hits),
				Misses = state.Sum(i => i.Misses),
				AddMisses = state.Sum(i => i.AddMisses)
			};

			decimal total = Convert.ToDecimal(cache.CacheHits + cache.CacheMisses);
			Console.WriteLine(String.Concat("CacheStats: Count = ", Convert.ToString(cache.Count), Environment.NewLine
				, ", Total: ", total.ToString("N"), Environment.NewLine
				, ", Ops: ", totals.Operations.ToString("N"), Environment.NewLine
				, ", Adds: ", totals.Adds.ToString("N"), Environment.NewLine
				, ", Hits: ", totals.Hits.ToString("N"), Environment.NewLine
				, ", Misses: ", totals.Misses.ToString("N"), Environment.NewLine
				, ", AddMisses: ", totals.AddMisses.ToString("N"), Environment.NewLine
				, ", Hit %: ", Math.Round(cache.CacheHits / total * 100), Environment.NewLine
				, ", Miss %: ", Math.Round(cache.CacheMisses / total * 100m), Environment.NewLine
				, ", Evictions: ", cache.CacheEvictions.ToString("N"), Environment.NewLine
				));

		}

		[TestMethod]
		public void CanGetAndAddWithTwiceAsManyItemsThanCache()
		{
			var test = new
			{
				Iterations = 100000,
				Threads = 2 * Environment.ProcessorCount,
				MaxCacheDepth = 10000,
				ItemArrayCount = 20000
			};

			var cache = new MruLocalMemoryCache<string, Item>("Testing MRU Cache", new MruCacheOptions { CacheDepth = test.MaxCacheDepth },
				Create.New<ICacheEvictionManager>());

			var items = new Item[test.ItemArrayCount];
			for (int i = 0; i < test.ItemArrayCount; i++)
			{
				items[i] = new Item();
			}
			var state = new ThreadState[test.Threads];
			for (int i = 0; i < test.Threads; i++)
			{
				var th = new ThreadState { Items = items, Cache = cache, Iterations = test.Iterations };
				state[i] = th;
				th.Task = new Task(Background_ThreadGetOrAdd, th);
				th.Task.Start();
			}

			foreach (var s in state)
			{
				s.Task.Wait();
				Console.WriteLine(String.Concat("High: ", Convert.ToString(s.HighWaterMark)
					, ", Hits: ", Convert.ToString(s.Hits)
					, ", Misses: ", Convert.ToString(s.Misses)
					, ", Adds: ", Convert.ToString(s.Adds)
					, ", AddMisses: ", Convert.ToString(s.AddMisses)
					));
			}

			var totals = new
			{
				Operations = state.Sum(i => i.Hits + i.Misses + i.AddMisses),
				Adds = state.Sum(i => i.Adds),
				Hits = state.Sum(i => i.Hits),
				Misses = state.Sum(i => i.Misses),
				AddMisses = state.Sum(i => i.AddMisses)
			};

			decimal total = Convert.ToDecimal(cache.CacheHits + cache.CacheMisses);
			Console.WriteLine(String.Concat("CacheStats: Count = ", Convert.ToString(cache.Count), Environment.NewLine
				, ", Total: ", total.ToString("N"), Environment.NewLine
				, ", Ops: ", totals.Operations.ToString("N"), Environment.NewLine
				, ", Adds: ", totals.Adds.ToString("N"), Environment.NewLine
				, ", Hits: ", totals.Hits.ToString("N"), Environment.NewLine
				, ", Misses: ", totals.Misses.ToString("N"), Environment.NewLine
				, ", AddMisses: ", totals.AddMisses.ToString("N"), Environment.NewLine
				, ", Hit %: ", Math.Round(cache.CacheHits / total * 100), Environment.NewLine
				, ", Miss %: ", Math.Round(cache.CacheMisses / total * 100m), Environment.NewLine
				, ", Evictions: ", cache.CacheEvictions.ToString("N"), Environment.NewLine
				));

		}

		[TestMethod]
		public void CanGetAndAddWith20PercentMoreItemsThanCache()
		{
			var test = new
			{
				Iterations = 10000,
				Threads = 2 * Environment.ProcessorCount,
				MaxCacheDepth = 100,
				ItemArrayCount = 120
			};

			var cache = new MruLocalMemoryCache<string, Item>("Testing MRU Cache"
				, new MruCacheOptions { CacheDepth = test.MaxCacheDepth }
				);

			var items = new Item[test.ItemArrayCount];
			for (int i = 0; i < test.ItemArrayCount; i++)
			{
				items[i] = new Item();
			}
			var state = new ThreadState[test.Threads];
			for (int i = 0; i < test.Threads; i++)
			{
				var th = new ThreadState { Items = items, Cache = cache, Iterations = test.Iterations };
				state[i] = th;
				th.Task = new Task(Background_ThreadGetOrAdd, th);
				th.Task.Start();
			}

			foreach (var s in state)
			{
				s.Task.Wait();
				Console.WriteLine(String.Concat("High: ", Convert.ToString(s.HighWaterMark)
					, ", Hits: ", Convert.ToString(s.Hits)
					, ", Misses: ", Convert.ToString(s.Misses)
					, ", Adds: ", Convert.ToString(s.Adds)
					, ", AddMisses: ", Convert.ToString(s.AddMisses)
					));
			}

			var totals = new
			{
				Operations = state.Sum(i => i.Hits + i.Misses + i.AddMisses),
				Adds = state.Sum(i => i.Adds),
				Hits = state.Sum(i => i.Hits),
				Misses = state.Sum(i => i.Misses),
				AddMisses = state.Sum(i => i.AddMisses)
			};

			decimal total = Convert.ToDecimal(cache.CacheHits + cache.CacheMisses);
			Console.WriteLine(String.Concat("CacheStats: Count = ", Convert.ToString(cache.Count), Environment.NewLine
				, ", Total: ", total.ToString("N"), Environment.NewLine
				, ", Ops: ", totals.Operations.ToString("N"), Environment.NewLine
				, ", Adds: ", totals.Adds.ToString("N"), Environment.NewLine
				, ", Hits: ", totals.Hits.ToString("N"), Environment.NewLine
				, ", Misses: ", totals.Misses.ToString("N"), Environment.NewLine
				, ", AddMisses: ", totals.AddMisses.ToString("N"), Environment.NewLine
				, ", Hit %: ", Math.Round(cache.CacheHits / total * 100), Environment.NewLine
				, ", Miss %: ", Math.Round(cache.CacheMisses / total * 100m), Environment.NewLine
				, ", Evictions: ", cache.CacheEvictions.ToString("N"), Environment.NewLine
				));

		}

		[TestMethod]
		public void CanRunHotFor1MinuteWith20PercentMoreItemsThanCache()
		{
			var test = new
			{
				RunningTime = TimeSpan.FromMinutes(1),
				Threads = 2 * Environment.ProcessorCount,
				MaxCacheDepth = 100,
				ItemArrayCount = 120
			};

			var cache = new MruLocalMemoryCache<string, Item>("Testing MRU Cache",
				new MruCacheOptions { CacheDepth = test.MaxCacheDepth }
				, Create.New<ICacheEvictionManager>());

			Console.WriteLine(String.Concat("Number of threads using the cache: ", test.Threads));
			Console.WriteLine(String.Concat("Synchronized eviction threshold: ", cache.SynchronizedEvictionThreshold));

			var items = new Item[test.ItemArrayCount];
			for (int i = 0; i < test.ItemArrayCount; i++)
			{
				items[i] = new Item();
			}
			var state = new ThreadState[test.Threads];
			for (int i = 0; i < test.Threads; i++)
			{
				var th = new ThreadState { Items = items, Cache = cache, RunningTime = test.RunningTime };
				state[i] = th;
				th.Task = new Task(BG_ThreadTimedGetOrAdd, th);
				th.Task.Start();
			}

			foreach (var s in state)
			{
				s.Task.Wait();
				Console.WriteLine(String.Concat("High: ", Convert.ToString(s.HighWaterMark)
					, ", Hits: ", Convert.ToString(s.Hits)
					, ", Misses: ", Convert.ToString(s.Misses)
					, ", Adds: ", Convert.ToString(s.Adds)
					, ", AddMisses: ", Convert.ToString(s.AddMisses)
					));
			}

			var totals = new
			{
				Operations = state.Sum(i => i.Hits + i.Misses + i.AddMisses),
				Adds = state.Sum(i => i.Adds),
				Hits = state.Sum(i => i.Hits),
				Misses = state.Sum(i => i.Misses),
				AddMisses = state.Sum(i => i.AddMisses)
			};

			decimal total = Convert.ToDecimal(cache.CacheHits + cache.CacheMisses);
			Console.WriteLine(String.Concat("CacheStats: Count = ", Convert.ToString(cache.Count), Environment.NewLine
				, ", Total: ", total.ToString("N"), Environment.NewLine
				, ", Ops: ", totals.Operations.ToString("N"), Environment.NewLine
				, ", Adds: ", totals.Adds.ToString("N"), Environment.NewLine
				, ", Hits: ", totals.Hits.ToString("N"), Environment.NewLine
				, ", Misses: ", totals.Misses.ToString("N"), Environment.NewLine
				, ", AddMisses: ", totals.AddMisses.ToString("N"), Environment.NewLine
				, ", Hit %: ", Math.Round(cache.CacheHits / total * 100), Environment.NewLine
				, ", Miss %: ", Math.Round(cache.CacheMisses / total * 100m), Environment.NewLine
				, ", Evictions: ", cache.CacheEvictions.ToString("N"), Environment.NewLine
				));
		}

		void Background_ThreadGetOrAddLessRandom(object state)
		{
			ThreadState th = (ThreadState)state;
			Random rand = new Random(Environment.TickCount);
			var cache = th.Cache;
			var hits = 0;
			var misses = 0;
			var adds = 0;
			var addmiss = 0;
			var iter = th.Iterations;
			var items = th.Items;
			var len = items.Length;
			var first = items.First();
			for (var i = 0; i < iter; i++)
			{
				var item = first;
				var ii = rand.Next(0, len);
				if (i % 5 == 0)
				{
					item = items[ii];
				}

				if (i > 0 && i % 100 == 0)
				{
					Item iOut;
					cache.TryRemove(first.Name, out iOut);
				}

				Item existing;
				if (cache.TryGet(item.Name, out existing))
				{
					Assert.AreEqual(item, existing);
					hits++;
				}
				else
				{
					misses++;
					if (cache.TryAdd(item.Name, item))
					{
						adds++;
					}
					else
					{
						addmiss++;
					}
				}
				th.HighWaterMark = Math.Max(th.HighWaterMark, cache.Count);
				if (ii % 3 == 0)
				{
					Thread.Sleep(0);
				}
			}
			th.Misses = misses;
			th.Hits = hits;
			th.Adds = adds;
			th.AddMisses = addmiss;
		}

		void Background_ThreadGetOrAdd(object state)
		{
			ThreadState th = (ThreadState)state;
			Random rand = new Random(Environment.TickCount);
			var cache = th.Cache;
			var hits = 0;
			var misses = 0;
			var adds = 0;
			var addmiss = 0;
			var iter = th.Iterations;
			var items = th.Items;
			var len = items.Length;
			for (var i = 0; i < iter; i++)
			{
				var ii = rand.Next(0, len);
				var item = items[ii];
				Item existing;
				if (cache.TryGet(item.Name, out existing))
				{
					Assert.AreEqual(item, existing);
					hits++;
				}
				else
				{
					misses++;
					if (cache.TryAdd(item.Name, item))
					{
						adds++;
					}
					else
					{
						addmiss++;
					}
				}
				th.HighWaterMark = Math.Max(th.HighWaterMark, cache.Count);
				if (ii % 3 == 0)
				{
					Thread.Sleep(0);
				}
			}
			th.Misses = misses;
			th.Hits = hits;
			th.Adds = adds;
			th.AddMisses = addmiss;
		}
		void BG_ThreadTimedGetOrAdd(object state)
		{
			ThreadState th = (ThreadState)state;
			Random rand = new Random(Environment.TickCount);
			var endTime = DateTime.Now.Add(th.RunningTime);
			var cache = th.Cache;
			var hits = 0;
			var misses = 0;
			var adds = 0;
			var addmiss = 0;
			var iter = th.Iterations;
			var items = th.Items;
			var len = items.Length;
			while (endTime >= DateTime.Now)
			{
				var ii = rand.Next(0, len);
				var item = items[ii];
				Item existing;
				if (cache.TryGet(item.Name, out existing))
				{
					Assert.AreEqual(item, existing);
					hits++;
				}
				else
				{
					misses++;
					if (cache.TryAdd(item.Name, item))
					{
						adds++;
					}
					else
					{
						addmiss++;
					}
				}
				th.HighWaterMark = Math.Max(th.HighWaterMark, cache.Count);
				if (ii % 3 == 0)
				{
					Thread.Sleep(0);
				}
			}
			th.Misses = misses;
			th.Hits = hits;
			th.Adds = adds;
			th.AddMisses = addmiss;
		}
	}
}
