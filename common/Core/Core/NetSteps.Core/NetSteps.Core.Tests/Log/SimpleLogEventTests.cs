using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Log;
using NetSteps.Encore.Core.Wireup;

namespace NetSteps.Encore.Core.Tests.Log
{
	[TestClass]
	public class SimpleLogEventTests
	{
		[TestInitialize]
		public void Init()
		{
			WireupCoordinator.SelfConfigure();
		}

		[TestMethod]
		public void SimpleLogEvent_DoesGetMessage()
		{
			var expect = new { 
				Source = "my source for testing",				
				Type = TraceEventType.Verbose,
				AppKind = 3113,
				AppKindName = "spectacular kind name",

			};
			var rand = new Random(Environment.TickCount);
			var gen = Create.New<DataGenerator>();
			var msg = gen.GetString(rand.Next(4000));
			var evt = new SimpleLogEvent(expect.Source, expect.Type, expect.AppKind, expect.AppKindName, msg, null);

			Assert.AreEqual(expect.Source, evt.SourceName);
		}
	}
}
