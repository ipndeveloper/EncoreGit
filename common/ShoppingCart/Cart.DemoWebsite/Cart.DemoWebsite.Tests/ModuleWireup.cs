using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;

[assembly: Wireup(typeof(Cart.DemoWebsite.Tests.ModuleWireup))]

namespace Cart.DemoWebsite.Tests
{
	[WireupDependency(typeof(Cart.DemoWebsite.ModuleWireup))]
	public class ModuleWireup : WireupCommand
	{
		protected override void PerformWireup(IWireupCoordinator coordinator)
		{
		}
	}
}
