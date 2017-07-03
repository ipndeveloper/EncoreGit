using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Diagnostics.Utilities;
using System.Diagnostics;
using Moq;
using System.IO;
using System.Threading;
using FluentAssertions;

namespace NetSteps.Diagnostics.Utilities.Tests
{
    [TestClass]
    public class TracerTest
    {
        private static readonly StringBuilder TraceOutput = new StringBuilder();
        private static readonly TextWriterTraceListener Listener = new TextWriterTraceListener(new StringWriter(TraceOutput));

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            TraceSource source = Tracer.Resolve(typeof(TracerTest)).First();
            source.Switch.Level = SourceLevels.All;
            source.Listeners.Add(Listener);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TraceSource source = Tracer.Resolve(typeof(TracerTest)).First();
            source.Switch.Level = SourceLevels.Off;
            source.Listeners.Remove(Listener);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            TraceOutput.Clear();
        }

        [TestMethod]
        public void TraceEventTest()
        {
            this.TraceVerbose("Verbose");
            this.TraceInformation("Information");
            this.TraceWarning("Warning");
            this.TraceError("Error");
            this.TraceException(new Exception("Exception"));
            this.TraceEvent("Event");
            TraceOutput.ToString().Should().Contain("Verbose");
            TraceOutput.ToString().Should().Contain("Information");
            TraceOutput.ToString().Should().Contain("Warning");
            TraceOutput.ToString().Should().Contain("Error");
            TraceOutput.ToString().Should().Contain("Exception");
            TraceOutput.ToString().Should().Contain("Event");
        }

        [TestMethod]
        public void TraceActivityTest()
        {
            using (var activity = this.TraceActivity("TraceActivityTest").After(TimeSpan.FromMilliseconds(20), "Warning"))
            {
                Thread.Sleep(500);
            }
            TraceOutput.ToString().Should().Contain("Started TraceActivityTest");
            TraceOutput.ToString().Should().Contain("Warning");
            TraceOutput.ToString().Should().Contain("Stopped TraceActivityTest");
        }
    }
}
