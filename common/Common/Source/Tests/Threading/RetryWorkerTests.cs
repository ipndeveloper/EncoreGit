using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Common.Threading;

namespace NetSteps.Common.Tests.Threading
{
    [TestClass]
    public class RetryWorkerTests
    {
        [TestMethod]
        public void RetryWorkerCanSucceedOnFirstTryWithoutCallbacks()
        {
            var test = new
            {
                Initial = "initial",
                Final = "final",
                DurationMilliseconds = 2000
            };

            var value = test.Initial;
            
            var worker = new RetryWorker<String>().Fork(val =>
            {
                Assert.AreEqual(test.Initial, value);
                Thread.Sleep(200);
                value = test.Final;
            }, value);
            Assert.AreEqual(test.Initial, value, "value should not have changed yet");

            Thread.Sleep(test.DurationMilliseconds);
            Assert.AreEqual(test.Final, value, "value should have changed to final");
        }

        [TestMethod]
        public void RetryWorkerCanSucceedOnFirstTry()
        {
            var test = new
            {
                Initial = "initial",
                Final = "final",
                RetriesBeforeSuccess = 0,
                DurationMilliseconds = 2000
            };

            var value = test.Initial;
            var failures = 0;

            var worker = new NetSteps.Common.Threading.RetryWorker<String>((w, e, i) =>
            {
                failures += 1;
                return true;
            }, (w,e) =>
            {
                Assert.Fail();
            }).Fork(val =>
            {
                Assert.AreEqual(test.Initial, value);
                Thread.Sleep(200);
                value = test.Final;
            }, value);
            Assert.AreEqual(test.Initial, value, "value should not have changed yet");

            Thread.Sleep(test.DurationMilliseconds);
            Assert.AreEqual(test.Final, value, "value should have changed to final");
            Assert.AreEqual(test.RetriesBeforeSuccess, failures, "should have failed before succeeding");
        }

        [TestMethod]
        public void RetryWorkerSucceedsAfterRetrying()
        {
            var test = new {
                Initial = "initial",
                Final = "final",
                RetriesBeforeSuccess = 2,
                DurationMilliseconds = 10000
            };
            
            var value = test.Initial;
            var retries = 0;
            var failures = 0;

            var worker = new RetryWorker<String>((w, e, i) =>
            {
                failures += 1;
                return true;
            }, (w,e) =>
            {
                Assert.IsInstanceOfType(e, typeof(InvalidOperationException));
            }).Fork(val =>
            {
                Assert.AreEqual(test.Initial, value);
                Thread.Sleep(200);
                if (retries < test.RetriesBeforeSuccess)
                {
                    retries += 1;
                    throw new InvalidOperationException();
                }
                else
                {
                    value = test.Final;
                }
            }, value);
            Assert.AreEqual(test.Initial, value, "value should not have changed yet");

            Thread.Sleep(test.DurationMilliseconds);
            Assert.AreEqual(test.Final, value, "value should have changed to final");
            Assert.AreEqual(test.RetriesBeforeSuccess, failures, "should have failed before succeeding");
        }

        [TestMethod]
        public void RetryWorker_AllAttemptsFail_StopsRetryingAfterMaxAttempts()
        {
            var test = new
            {
                ExpectedAttempts = 3,
                DurationMilliseconds = 15000
            };
            var attempts = 0;
            var worker = new RetryWorker<string>();

            // Act
            worker.Fork(val =>
                {
                    attempts++;
                    throw new Exception("FAIL");
                }
            );
            Thread.Sleep(test.DurationMilliseconds);

            Assert.AreEqual(test.ExpectedAttempts, attempts, "Unexpected number of attempts.");
        }

        [TestMethod]
        public void RetryWorker_FirstAttemptFails_WaitsBeforeRetrying()
        {
            var test = new
            {
                ExpectedAttempts = 2,
                ExpectedMinimumWaitMilliseconds = 2000,
                DurationMilliseconds = 4000
            };
            var attempts = 0;
            var stopwatch = new Stopwatch();
            var worker = new RetryWorker<string>();

            // Act
            worker.Fork(val =>
                {
                    attempts++;
                    if (attempts == 1)
                    {
                        stopwatch.Start();
                        throw new Exception("FAIL");
                    }
                    stopwatch.Stop();
                }
            );
            Thread.Sleep(test.DurationMilliseconds);

            Assert.AreEqual(test.ExpectedAttempts, attempts, "Unexpected number of attempts.");
            Assert.IsTrue(
                stopwatch.ElapsedMilliseconds >= test.ExpectedMinimumWaitMilliseconds,
                "Expected: >={0}ms. Actual: {1}ms. Should have waited between retries.",
                test.ExpectedMinimumWaitMilliseconds,
                stopwatch.ElapsedMilliseconds
            );
        }
    }
}
