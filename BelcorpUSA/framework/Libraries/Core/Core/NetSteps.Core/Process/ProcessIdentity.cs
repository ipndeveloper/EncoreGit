using System;
using System.Security;
using System.Diagnostics.CodeAnalysis;

namespace NetSteps.Encore.Core.Process
{
	/// <summary>
	/// Interface for a process identity. Used when a process self-identifies.
	/// </summary>
	public class ProcessIdentity : IProcessIdentity
	{
		/// <summary>
		/// Identifies the customer/tenant associated with the process.
		/// </summary>
		public string Tenant { get; private set; }
		/// <summary>
		/// Indicates the software component represented by the process.
		/// </summary>
		public string Component { get; private set; }
		/// <summary>
		/// Identifies the environment in which the process is operating.
		/// </summary>
		public string Environment { get; private set; }
		/// <summary>
		/// Identifies the machine name where the process is located.
		/// </summary>
		public string MachineName { get; private set; }
		/// <summary>
		/// Indicates process' name.
		/// </summary>
		public string ProcessName { get; private set; }
		/// <summary>
		/// Indicates the process' operating system ID.
		/// </summary>
		public int ProcessID { get; private set; }

		[SuppressMessage("Microsoft.Security", "CA2122", Justification="Doesn't expose the process to external code.")]
		internal static IProcessIdentity MakeProcessIdentity()
		{
			var config = ProcessIdentifyConfigurationSection.Current;
			var processName = String.Empty;
			var processID = 0;
			try
			{
				using (var process = System.Diagnostics.Process.GetCurrentProcess())
				{
					processName = process.ProcessName;
					processID = process.Id;
				}
			}
			catch(SecurityException) {
			}
			return new ProcessIdentity
			{
				Tenant = config.Tenant,
				Component = config.Component,
				Environment = config.Environment,				
				MachineName = System.Environment.MachineName,
				ProcessName = processName,
				ProcessID = processID
			};
		}
	}
}
