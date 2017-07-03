using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;

namespace NetSteps.Diagnostics.Utilities.Configuration
{
	[RunInstaller(true)]
	public partial class CustomEventLogTraceListenerInstaller : Installer
	{
		#region Fields

		public static readonly string Name = "NetSteps";
		public static readonly string Source = "None";

		#endregion

		#region Properties

		#endregion

		#region Construction
		public CustomEventLogTraceListenerInstaller()
		{
			InitializeComponent();
			Installers.Clear();
			EventLogInstaller eventLogInstaller = new EventLogInstaller();
			eventLogInstaller.Log = Name;
			eventLogInstaller.Source = Source;
			Installers.Add(eventLogInstaller);
		}

		#endregion

		#region Methods

		#endregion

	}
}
