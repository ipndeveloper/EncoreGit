using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace NetSteps.Diagnostics.Utilities.Tests
{
	[TestClass]
	public class TracerTests
	{
		private static readonly StringBuilder TraceOutput = new StringBuilder();
		private static readonly TextWriterTraceListener Listener = new TextWriterTraceListener(new StringWriter(TraceOutput));

		[ClassInitialize]
		public static void ClassInitialize(TestContext context)
		{
			TraceSource source = Tracer.Resolve(typeof(TracerTests)).First();
			source.Switch.Level = SourceLevels.All;
			source.Listeners.Add(Listener);
		}

		[ClassCleanup]
		public static void ClassCleanup()
		{
			TraceSource source = Tracer.Resolve(typeof(TracerTests)).First();
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
