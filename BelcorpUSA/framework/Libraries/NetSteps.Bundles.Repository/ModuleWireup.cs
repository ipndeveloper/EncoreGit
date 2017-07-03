using NetSteps.Encore.Core.Wireup.Meta;

[assembly: Wireup(typeof(NetSteps.Bundles.Repository.ModuleWireup))]

namespace NetSteps.Bundles.Repository
{
	using NetSteps.Encore.Core.Wireup;

	public class ModuleWireup : WireupCommand
	{
		protected override void PerformWireup(IWireupCoordinator coordinator)
		{

		}
	}
}
