using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;

namespace NetSteps.Events.Service.Tests
{
	[WireupDependency(typeof(NetSteps.Common.ModuleWireup))]
	public class ModuleWireup : WireupCommand
	{
		protected override void PerformWireup(IWireupCoordinator coordinator)
		{
		}
	}
}
