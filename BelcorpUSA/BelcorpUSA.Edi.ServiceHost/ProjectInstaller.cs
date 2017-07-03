using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;

namespace BelcorpUSA.Edi.ServiceHost
{
	[RunInstaller(true)]
	public partial class ProjectInstaller : Installer
	{
		public ProjectInstaller()
		{
			InitializeComponent();
		}
	}
}
