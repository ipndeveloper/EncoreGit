using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Diagnostics.Utilities.Configuration;
using NetSteps.Diagnostics.Utilities.Listeners;
using System.Collections;
using System.Configuration.Install;
using System.Diagnostics;
using System.Security.Permissions;

namespace NetSteps.Diagnostics.Utilities.Tests
{

	[TestClass]
	public class CustomEventLogTraceListenerTests
	{
		#region Fields

		#endregion

		#region Properties

		public TestContext TestContext { get; set; }
		#endregion

		#region Construction

		#endregion

		#region Methods

		//Test requires elevation
		[PrincipalPermission(SecurityAction.Demand, Role = @"BUILTIN\Administrators")]
		[TestCategory("Manual")]
		[TestMethod]
		public void InstallWithElevationAndTraceTest()
		{
			using (AssemblyInstaller installer = new AssemblyInstaller(typeof(CustomEventLogTraceListenerInstaller).Assembly, new string[] { }))
			{
				IDictionary state = new Hashtable();
				installer.Install(state);
			}
			var listener = new CustomEventLogTraceListener();
			Trace.Listeners.Add(listener);
			Trace.TraceInformation("Information");
			Trace.TraceWarning("Warning");
			Trace.TraceError("Error");
			Trace.TraceInformation("Testing multi-part message\r\n".PadRight(CustomEventLogTraceListener.MaximumMessageSize * 2, '*'));
			Trace.Listeners.Remove(listener);
			TestContext.WriteLine("Check the event log to ensure that the above messages were written successfully");
		}


		//Test should require elevation and assumes destination log has already been installed
		[TestCategory("Manual")]
		[TestMethod]
		public void TraceWithoutElevationTest()
		{
			var listener = new CustomEventLogTraceListener();
			Trace.Listeners.Add(listener);
			Trace.TraceInformation("Information");
			Trace.TraceWarning("Warning");
			Trace.TraceError("Error");
			Trace.TraceInformation("Testing multi-part message\r\n".PadRight(CustomEventLogTraceListener.MaximumMessageSize * 2, '*'));
			Trace.Listeners.Remove(listener);
			TestContext.WriteLine("Check the event log to ensure that the above messages were written successfully");
		}

		#endregion
	}
}