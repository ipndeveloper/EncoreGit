using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;

[assembly: Wireup(typeof(NetSteps.SOD.Wrapper.Tests.ModuleWireup))]

namespace NetSteps.SOD.Wrapper.Tests
{
	[WireupDependency(typeof(NetSteps.SOD.Wrapper.ModuleWireup))]
	public class ModuleWireup : WireupCommand
	{
		protected override void PerformWireup(IWireupCoordinator coordinator)
		{
			
		}
	}
}