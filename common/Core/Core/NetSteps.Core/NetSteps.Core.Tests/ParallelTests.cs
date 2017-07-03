using System;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.Parallel;

namespace NetSteps.Encore.Core.Tests
{
	[TestClass]
	public class ParallelTests
	{
		[TestMethod]
		public void Parallel_ExecuteInParallel()
		{
			Exception caught = null;
			bool completed = false;

			var completion = Go.Parallel(
				() =>
				{
					Thread.Sleep(TimeSpan.FromSeconds(1));
				}
				, e =>
				{
					caught = e;
					completed = true;
				}
				);

			Assert.IsFalse(completion.IsCompleted);
			Assert.IsFalse(completion.IsFaulted);

			completion.Wait(TimeSpan.FromSeconds(2));

			Assert.IsTrue(completion.IsCompleted);
			Assert.IsFalse(completion.IsFaulted);

			Assert.IsNull(caught);
			Assert.IsTrue(completed);
		}

		[TestMethod]
		public void Parallel_ErrorPropagatesToErrorHandler()
		{
			Exception caught = null;
			bool completed = false;
			
			Go.Parallel(
				() =>
				{
					Thread.Sleep(TimeSpan.FromSeconds(0.1));
					throw new InvalidOperationException("Kaboom!");
				},
				 e =>
				 {
					 caught = e;
					 completed = true;
				 }
				);

			Thread.Sleep(TimeSpan.FromSeconds(0.3));

			Assert.IsNotNull(caught);
			Assert.AreEqual("Kaboom!", caught.Message);
			Assert.IsTrue(completed);
		}

		[TestMethod]
		public void Parallel_ErrorThrownByErrorHandlerCausesOnUncaughtException()
		{
			Exception caught = null;
			Exception uncaught = null;
			bool completed = false;

			Go.OnUncaughtException += new EventHandler<UncaughtExceptionArgs>((sender, e) =>
			{
				uncaught = e.Error;
			});

			var completion = Go.Parallel(() =>
				{
					Thread.Sleep(TimeSpan.FromSeconds(0.5));
					throw new InvalidOperationException("Kaboom!");
				},
				e =>
				{
					caught = e;
					completed = true;
					throw new InvalidOperationException("Whammy!");
				}
			);

			Assert.IsFalse(completion.IsCompleted);
			Assert.IsFalse(completion.IsFaulted);

			Assert.IsFalse(completion.Wait(TimeSpan.FromSeconds(10)),
				"false because exception was thrown");

			Assert.IsTrue(completion.IsCompleted);
			Assert.IsTrue(completion.IsFaulted, "faulted due to the exception 'Kaboom!'");

			Assert.IsNotNull(caught);
			Assert.AreSame(caught, completion.Exception);
			Assert.AreEqual("Kaboom!", caught.Message);
			Assert.AreEqual("Whammy!", uncaught.Message);
			Assert.IsTrue(completed);
		}

		[TestMethod]
		public void Parallel_ExecutionCanBeAwaited()
		{
			var completion = Go.Parallel(
				() =>
				{
					Thread.Sleep(TimeSpan.FromSeconds(1));
				});

			Assert.IsFalse(completion.IsCompleted);
			Assert.IsFalse(completion.IsFaulted);

			Assert.IsTrue(completion.Wait(TimeSpan.FromSeconds(1.2)));

			Assert.IsTrue(completion.IsCompleted);
			Assert.IsFalse(completion.IsFaulted);
		}

		[TestMethod]
		public void Parallel_ExecuteInParallelWithArgumentAndResult()
		{
			Exception caught = null;
			bool completed = false;
			int handbackTotal = 0;
						
			var result = Go.Parallel(				
				new { Value = new int[] { 1, 2, 3, 4, 5, 6, 7 } }
				, h =>
				{
					Thread.Sleep(TimeSpan.FromSeconds(1));
					return h.Value.Sum();
				}
				, (e, total) =>
				{
					caught = e;
					completed = true;
					handbackTotal = total;
				}
				).AwaitValue();

			Assert.IsNull(caught);
			Assert.IsTrue(completed);
			Assert.AreEqual(28, handbackTotal);
			Assert.AreEqual(handbackTotal, result);
		}

		[TestMethod]
		public void Parallel_ExecuteInParallelObservedByContinuationAndObservedByEvent()
		{
			Exception caught = null;
			bool completed = false;

			var completion = Go.Parallel(
				() =>
				{
					Thread.Sleep(TimeSpan.FromSeconds(1));
				}
				, e =>
				{
					caught = e;
					completed = true;
				}
				);

			Assert.IsFalse(completion.IsCompleted);
			Assert.IsFalse(completion.IsFaulted);

			Assert.IsTrue(completion.Wait(TimeSpan.FromSeconds(2)));

			Assert.IsTrue(completion.IsCompleted);
			Assert.IsFalse(completion.IsFaulted);

			Assert.IsNull(caught);
			Assert.IsTrue(completed);

			Completion fromEvent = null;

			completion.Completed += (sender, e) =>
			{
				fromEvent = e.Completion;				
			};

			Assert.AreSame(fromEvent, completion);
		}
	}
}
