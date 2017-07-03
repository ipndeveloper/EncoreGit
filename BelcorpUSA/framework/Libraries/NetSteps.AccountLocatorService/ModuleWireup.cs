using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;

[assembly: Wireup(typeof(NetSteps.AccountLocatorService.ModuleWireup))]

namespace NetSteps.AccountLocatorService
{
	[WireupDependency(typeof(NetSteps.AccountLocatorService.Common.ModuleWireup))]
	public class ModuleWireup : WireupCommand
	{
		protected override void PerformWireup(IWireupCoordinator coordinator)
		{
		}
	}
}