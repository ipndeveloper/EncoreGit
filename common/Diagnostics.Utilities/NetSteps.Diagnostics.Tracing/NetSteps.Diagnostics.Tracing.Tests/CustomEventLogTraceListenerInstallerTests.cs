using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Diagnostics.Utilities.Configuration;
using System.Collections;
using System.Configuration.Install;
using System.Security.Permissions;

namespace NetSteps.Diagnostics.Utilities.Tests
{
	//Test requires elevation
	[PrincipalPermission(SecurityAction.Demand, Role = @"BUILTIN\Administrators")]
	[TestClass]
	public class InstallationTests
	{
		[TestCategory("Manual")]
		[TestMethod]
		public void InstallerTest()
		{
			using (AssemblyInstaller installer = new AssemblyInstaller(typeof(CustomEventLogTraceListenerInstaller).Assembly, new string[] { }))
			{
				IDictionary state = new Hashtable();
				installer.Install(state);
				installer.Uninstall(state);
			}
		}

		[TestCategory("Manual")]
		[TestMethod]
		public void InstallTest()
		{
			using (AssemblyInstaller installer = new AssemblyInstaller(typeof(CustomEventLogTraceListenerInstaller).Assembly, new string[] { }))
			{
				IDictionary state = new Hashtable();
				installer.Install(state);
			}

		}


		[TestCategory("Manual")]
		[TestMethod]
		public void UninstallTest()
		{
			using (AssemblyInstaller installer = new AssemblyInstaller(typeof(CustomEventLogTraceListenerInstaller).Assembly, new string[] { }))
			{
				IDictionary state = new Hashtable();
				installer.Uninstall(state);
			}

		}
	}
}
