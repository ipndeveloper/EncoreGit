using NetSteps.Encore.Core.Wireup.Meta;

[module: WireupDependency(typeof(NetSteps.Bundles.Component.ModuleWireup))]

namespace NetSteps.Bundles.Component
{
	using NetSteps.Encore.Core.Wireup;

	public class ModuleWireup : WireupCommand
	{
		protected override void PerformWireup(IWireupCoordinator coordinator)
		{
		}
	}
}
