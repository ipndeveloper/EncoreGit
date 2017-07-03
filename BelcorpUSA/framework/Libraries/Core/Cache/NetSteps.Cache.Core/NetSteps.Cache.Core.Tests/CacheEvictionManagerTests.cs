using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Core.Cache;

namespace NetSteps.Core.Cache.Tests
{
	[TestClass]
	public class CacheEvictionManagerTests
	{
		[TestMethod]
		public void AnAddedEvictionCallbackGetsInvokedInBackgroundThread()
		{
			var test = new { 
				NumberOfTimesToReschedule = 1,
				ThreadId = Thread.CurrentThread.ManagedThreadId
			};
			var mgr = new CacheEvictionManager();

			int evictionThreadId = 0;
			int calls = 0;
			mgr.AddEvictionCallback(isSynchronous =>
			{
				Assert.IsFalse(isSynchronous, "Eviction manager invocations should be asynchrounous");
				evictionThreadId = Thread.CurrentThread.ManagedThreadId;
				calls += 1;
				return calls < test.NumberOfTimesToReschedule;
			});

			Thread.Sleep(1000); // wait plenty of time

			Assert.AreEqual(1, calls, "Should have been invoked only once");
			Assert.AreNotEqual(test.ThreadId, evictionThreadId, "Eviction manager should use a worker thread.");
		}

		[TestMethod]
		public void WhenAnEvictionCallbackIsRescheduledItGetsInvokedAgain()
		{
			var test = new { NumberOfTimesToReschedule = 4 };
			var mgr = new CacheEvictionManager();

			int calls = 0;
			mgr.AddEvictionCallback(isSynchronous => {
				Assert.IsFalse(isSynchronous, "Eviction manager invocations should be asynchrounous");
				calls += 1;
				return calls < test.NumberOfTimesToReschedule;
			});

			Thread.Sleep(1000); // wait plenty of time

			Assert.AreEqual(test.NumberOfTimesToReschedule, calls, String.Concat("Should have rescheduled ", test.NumberOfTimesToReschedule, " times."));
		}

		class CallbackTestWrapper
		{
			readonly int _times;
			int _invokeCount = 0;
			public CallbackTestWrapper(int times2reschedule)
			{
				_times = times2reschedule;
			}
			public bool OnEviction(bool isSynchronous)
			{
				Interlocked.Increment(ref _invokeCount);
				return Thread.VolatileRead(ref _invokeCount) < _times;
			}
			public bool AwaitSuccess(TimeSpan timeout)
			{
				DateTime expired = DateTime.Now.AddMilliseconds(timeout.TotalMilliseconds);
				while (Thread.VolatileRead(ref _invokeCount) < _times)
				{
					if (expired > DateTime.Now) Thread.Sleep(0);
					else return false;
				}
				return Thread.VolatileRead(ref _invokeCount) == _times;
			}
		}

		[TestMethod]
		public void AllScheduledEvictionCallbacksAreEventuallyInvoked()
		{
			var rand = new Random();
			var numberOfCallbacks = rand.Next(10, 1000);

			var mgr = new CacheEvictionManager();
			var callbacks = new List<CallbackTestWrapper>();

			for (int i = 0; i < numberOfCallbacks; i++)
			{
				var cb = new CallbackTestWrapper(rand.Next(1, 10));
				mgr.AddEvictionCallback(cb.OnEviction);
				callbacks.Add(cb);
			}

			foreach (var cb in callbacks)
			{
				Assert.IsTrue(cb.AwaitSuccess(TimeSpan.FromSeconds(1)));
			}
		}
	}
}
