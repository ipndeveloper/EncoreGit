using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Core.Cache;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace NetSteps.Cache.Core.Tests
{
	[TestClass]
	public class ActiveLocalMemoryCachedListBaseTests
	{
		public class FakeItem
		{
			public int IntProp { get; set; }
			public string StringProp { get; set; }
		}

		public class FakeItemListService
		{
			public List<FakeItem> GetItems()
			{
				FakeItem[] a = new FakeItem[] {
					new FakeItem() { IntProp = 0, StringProp = "0"},
					new FakeItem() { IntProp = 1, StringProp = "1"},
					new FakeItem() { IntProp = 2, StringProp = "2"},
					new FakeItem() { IntProp = 3, StringProp = "3"},
					new FakeItem() { IntProp = 4, StringProp = "4"},
					new FakeItem() { IntProp = 5, StringProp = "5"},
					new FakeItem() { IntProp = 6, StringProp = "6"},
					new FakeItem() { IntProp = 7, StringProp = "7"},
					new FakeItem() { IntProp = 8, StringProp = "8"},
					new FakeItem() { IntProp = 9, StringProp = "9"}
				};

				return a.ToList();
			}
		}

		public class FakeItemCachedList : ActiveLocalMemoryCachedListBase<FakeItem>
		{

			public FakeItemCachedList()
				: base("FakeItemCachedList")
			{
			}

			protected override IList<FakeItem> PerformRefresh()
			{
				return new FakeItemListService().GetItems();
			}
		}

		[TestMethod]
		public void Basic_Test_If_List_Configures_And_Self_Populates()
		{
			var l = new FakeItemCachedList();
			Assert.IsNotNull(l);
			Assert.AreEqual(TimeSpan.FromSeconds(10), l.RefreshInterval);
			Assert.AreEqual(10, l.Count);
		}

		class ThreadState
		{
			public int Iterations { get; set; }
			public int NullResults { get; set; }
			public int IndexMissMatches { get; set; }
			public DateTime StartedOn { get; set; }
			public DateTime EndedOn { get; set; }
			public ICachedList<FakeItem> Cache { get; set; }
			public Task Task { get; set; }
			public TimeSpan ExecutionDurration { get; set; }
			public TimeSpan LongestWait { get; set; }
		}

		class StopWatch
		{
			DateTime _started;

			public void Start() { _started = DateTime.Now; }
			public TimeSpan Stop()
			{
				var r = DateTime.Now - _started;
				_started = DateTime.MinValue;
				return r;
			}
		}

		void BG_AccessCachedList(object state)
		{
			ThreadState th = (ThreadState)state;
			Random rand = new Random(Environment.TickCount);
			var cache = th.Cache;
			th.StartedOn = DateTime.Now;
			DateTime end = DateTime.Now.Add(th.ExecutionDurration);
			while (end > DateTime.Now)
			{
				var ii = rand.Next(0, cache.Count - 1);

				var s = new StopWatch();
				s.Start();
				var fi = cache[ii];
				var rt = s.Stop();
				th.LongestWait = th.LongestWait > rt ? th.LongestWait : rt;

				if (fi == null) th.NullResults++;
				else if (fi.IntProp != ii) th.IndexMissMatches++;

				th.Iterations++;

				if (th.Iterations % 3 == 0 && end > DateTime.Now)
				{ // pulse/yield every 3 items so that a single thread doesn't 
					// monopolize the activity on the cache.
					Thread.Sleep(0);
				}
			}
			th.EndedOn = DateTime.Now;
		}


		[TestMethod]
		public void Multiple_Rapid_Accessors_With_Refreshes_Test()
		{
			var test = new
			{
				Durration = TimeSpan.FromSeconds(30),
				Threads = 2 * Environment.ProcessorCount
			};

			var cache = new FakeItemCachedList();

			Console.WriteLine(String.Concat("Number of threads using the cache: ", test.Threads));

			var start = DateTime.Now;

			var state = new ThreadState[test.Threads];
			for (int i = 0; i < test.Threads; i++)
			{
				var th = new ThreadState { Cache = cache, ExecutionDurration = test.Durration };
				state[i] = th;
				th.Task = new Task(BG_AccessCachedList, th);
				th.Task.Start();
			}

			foreach (var s in state)
			{
				s.Task.Wait();
			}

			var elapsed = DateTime.Now - start;
			var expectedRefreshes = (elapsed.TotalSeconds - (elapsed.TotalSeconds % 10)) / 10;
			TimeSpan longestWait = state.Max(s => s.LongestWait);
			double avgIterations = state.Average(s => s.Iterations);
			long totalIterations = state.Sum(s => (long)s.Iterations);
			double rps = totalIterations / elapsed.TotalSeconds;

			int maxNullReads = state.Max(s => s.NullResults);
			int maxIndexMissMatch = state.Max(s => s.IndexMissMatches);

			Debug.WriteLine("Longest Wait: {0}\r\nAvg. Iterations: {1:N}\r\nTotal Iterations: {2:N}\r\nReads per Second: {3:N}", longestWait, avgIterations, totalIterations, rps);

			Assert.IsTrue(expectedRefreshes - 1 <= cache.TotalRefreshes);
			Assert.IsTrue(expectedRefreshes + 1 >= cache.TotalRefreshes);
			Assert.AreEqual(0, maxNullReads);
			Assert.AreEqual(0, maxIndexMissMatch);
		}
	}
}
