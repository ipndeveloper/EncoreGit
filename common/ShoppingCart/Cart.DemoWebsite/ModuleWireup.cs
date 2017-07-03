using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;

[assembly: Wireup(typeof(Cart.DemoWebsite.ModuleWireup))]

namespace Cart.DemoWebsite
{
	[WireupDependency(typeof(NetSteps.Encore.Core.ModuleWireup))]
	[WireupDependency(typeof(Cart.Common.ModuleWireup))]
	public class ModuleWireup : WireupCommand
	{
		protected override void PerformWireup(IWireupCoordinator coordinator)
		{
		}
	}
}