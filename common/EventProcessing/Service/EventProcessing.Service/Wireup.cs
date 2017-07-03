// Indicates this assembly is dependent upon its own wireup command:

using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.EventProcessing.Service;

[module: WireupDependency(typeof(Wireup))]
namespace NetSteps.EventProcessing.Service
{
	using NetSteps.Encore.Core.Wireup.Meta;
	using NetSteps.Encore.Core.Wireup;

	[WireupDependency(typeof(Encore.Core.ModuleWireup))]
	public class Wireup : WireupCommand
	{
		protected override void PerformWireup(IWireupCoordinator coordinator)
		{

		}
	}
}