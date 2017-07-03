using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;

[assembly: Wireup(typeof(NetSteps.Auth.UI.Service.ModuleWireup))]

namespace NetSteps.Auth.UI.Service
{
	[WireupDependency(typeof(NetSteps.Auth.Common.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Auth.UI.Common.ModuleWireup))]
	public class ModuleWireup : WireupCommand
	{
		protected override void PerformWireup(IWireupCoordinator coordinator)
		{

		}
	}
}
