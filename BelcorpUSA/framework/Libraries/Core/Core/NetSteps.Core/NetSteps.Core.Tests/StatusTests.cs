using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NetSteps.Encore.Core.Tests
{
  [TestClass]
  public class StatusTests
  {
    enum Test
    {
      Unknown = 0,
      Starting = 1,
      Started = 2,
      Ending = 5,
      Ended = 6
    }

    [TestMethod]
    public void Status_CanWaitForState()
    {
      var status = new Status<Test>();

      var bgException = default(Exception);
      var bgThreadStarted = false;
      var bgThreadEnding = false;
      var bgThreadDone = false;

      var callback = new WaitCallback(unused =>
      {
        try
        {
          // 1.1 Wait for starting...
          status.SpinWaitForState(Test.Starting, () => Thread.Sleep(0));
          bgThreadStarted = true;
          Thread.Sleep(20);
          // 2 Mark as started...
          status.ChangeState(Test.Started);

          Thread.Sleep(200);

          // 3 Mark as ending...
          bgThreadEnding = true;
          status.SetStateIfLessThan(Test.Ending, Test.Ending);

          // 4.4 Spin wait for ended
          status.SpinWaitForState(Test.Ended, () => Thread.Sleep(0));
        }
        catch (Exception e)
        {
          bgException = e;
        }
        bgThreadDone = true;
      });

      ThreadPool.QueueUserWorkItem(callback);

      Assert.IsFalse(bgThreadStarted);
      Thread.Sleep(20);

      // 1. Change to starting...
      Assert.IsTrue(status.SetStateIfLessThan(Test.Starting, Test.Starting));

      // 2.2 Spin wait for started...
      status.SpinWaitForState(Test.Started, () => Thread.Sleep(20));
      Assert.IsTrue(bgThreadStarted);
      Assert.IsFalse(bgThreadEnding);
      Assert.IsFalse(bgThreadDone);

      // 3.3 Spin wait for ending...
      status.SpinWaitForState(Test.Ending, () => Thread.Sleep(20));
      Assert.IsTrue(bgThreadEnding);
      Assert.IsFalse(bgThreadDone);

      // 4 Mark as ended...
      status.ChangeState(Test.Ended);

      while (!bgThreadDone)
      {
        Thread.Sleep(20);
      }
      Assert.IsTrue(bgThreadDone);
    }

    [TestMethod]
    public void Status_IsEqualWhenStatusIsEqual()
    {
      var lhs = new Status<Test>(Test.Started);
      var rhs = new Status<Test>(Test.Started);

      Assert.AreEqual(Test.Started, lhs.CurrentState);
      Assert.AreEqual(Test.Started, rhs.CurrentState);
      Assert.AreEqual(lhs, rhs);
      Assert.IsTrue(lhs == rhs);
      Assert.IsTrue(lhs.Equals(rhs));
      Assert.IsTrue(rhs.Equals(lhs));
    }

    [TestMethod]
    public void Status_HashCodesAreEqualWhenStatusIsEqual()
    {
      var lhs = new Status<Test>(Test.Started);
      var rhs = new Status<Test>(Test.Started);

      Assert.AreEqual(lhs, rhs);
      Assert.AreEqual(lhs.GetHashCode(), rhs.GetHashCode());
    }

    [TestMethod]
    public void Status_AreEqualToEnumValues()
    {
      var lhs = new Status<Test>(Test.Ended);
      var rhs = Test.Ended;

      Assert.AreEqual(rhs, lhs.CurrentState);
      Assert.IsTrue(lhs.HasState(rhs));
      Assert.IsTrue(lhs == rhs);
      Assert.IsTrue(rhs == lhs);
    }

    enum RaceState
    {
      // The race's initial state
      Initial = 0,
      // The race has begun; or a thread has relinquished its lead
      Running = 1,
      // A thread won the race and is doing some work
      Aquired = 2,
      // The race is ended
      Ended = 3,
      // An error occurred during the race
      Errored = 4
    }

    [TestMethod]
    public void Status_RaceConditionResultsInOneWinner()
    {
      var status = new Status<RaceState>();
      var numberOfThreads = Environment.ProcessorCount * 2;

      var observableLowerBound = 1;
      var observableUpperBound = 1000;

      // Variable being observed by the multiple threads...
      var observable = observableLowerBound;

      // Each thread will write its observations here:
      var observations = new List<int>[numberOfThreads];
      var orderedObservations = new ConcurrentQueue<int>();
      var numberOfFinishedThreads = 0;

      // In case a background thread blows up...
      var backGroundException = default(Exception);

      var callback = new ParameterizedThreadStart(threadNumber =>
      {
        int our_thread_index = (int)threadNumber;
        var ourObservations = observations[our_thread_index];
        try
        {
          // 1  Wait until the race is started...
          if (status.TrySpinWaitForState(RaceState.Running
            , state => state < RaceState.Ended))
          {
            // 2  Continue until the race has ended...
            while (status.IsLessThan(RaceState.Ended))
            {
              // 3  Attempt to transition to the aquired state...
              if (status.TryTransition(RaceState.Aquired, RaceState.Running))
              {
                // 3.1 Record our observation...
                var observed = Thread.VolatileRead(ref observable);
                ourObservations.Add(observed);
                orderedObservations.Enqueue(observed);

                // 3.2 Increment the observable, failing if it was
                //     updated by another thread...
                var concurrent = Interlocked.CompareExchange(ref observable
                  , observed + 1, observed
                  );
                if (concurrent != observed)
                {
                  throw new InvalidOperationException(String.Concat(
                    "Expected ", observed, ", received ", concurrent, "."));
                }

                // 3.3 Mark as ended if we've observed the upper bound;
                //     otherwise mark as running and continue the race...
                if (observed == observableUpperBound)
                {
                  if (!status.TryTransition(RaceState.Ended, RaceState.Aquired))
                  {
                    throw new InvalidOperationException(String.Concat(
                      "Couldn't return the status to 'Ended'."));
                  }
                }
                else
                {
                  if (!status.TryTransition(RaceState.Running, RaceState.Aquired))
                  {
                    throw new InvalidOperationException(String.Concat(
                      "Couldn't return the status to 'Running'."));
                  }
                }
              }
            }
          }
        }
        catch (Exception e)
        {
          // Signal that an error occurred only if no other thread has 
          // already done so...
          if (status.SetStateIfLessThan(RaceState.Errored, RaceState.Errored))
          {
            Util.VolatileWrite(ref backGroundException, e);
          }
        }
        Interlocked.Increment(ref numberOfFinishedThreads);
      });

      for (int i = 0; i < numberOfThreads; i++)
      {
        observations[i] = new List<int>();
        new Thread(callback).Start(i);
      }
      if (!status.TryTransition(RaceState.Running, RaceState.Initial))
      {
        throw new InvalidOperationException(String.Concat(
          "Couldn't start the race by transitioning the status to 'Running'.")
          );
      }

      while (Thread.VolatileRead(ref numberOfFinishedThreads) < numberOfThreads)
      {
        Thread.Sleep(20);
      }

      var combinedObservations = new List<int>();
      for (int i = 0; i < numberOfThreads; i++)
      {
        Console.WriteLine(String.Concat("Thread ", i, " observed: "
          , String.Join(", ", observations[i].ToArray()))
          );
        combinedObservations.AddRange(observations[i]);
      }
      // Ensure threads observed exactly observableUpperBound items...
      Assert.AreEqual(observableUpperBound, combinedObservations.Count);
      // Ensure there were no duplicate observations...
      Assert.AreEqual(0, combinedObservations
        .GroupBy(x => x)
        .Where(group => group.Count() > 1)
        .Select(group => group.Key).Count()
        );
      // Ensure each increment was observed and that they were observed 
      // in order...
      for (int i = observableLowerBound; i <= observableUpperBound; i++)
      {
        Assert.IsTrue(combinedObservations.Contains(i));
        int ordered;
        Assert.IsTrue(orderedObservations.TryDequeue(out ordered));
        Assert.AreEqual(i, ordered);
      }

      if (backGroundException != null) throw backGroundException;
    }
  }
}
