using NetSteps.Encore.Core;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;

[assembly: WireupDependency(typeof(NetSteps.QueueProcessing.Service.ModuleWireup))]

namespace NetSteps.QueueProcessing.Service
{
	public class ModuleWireup : WireupCommand
	{
		protected override void PerformWireup(IWireupCoordinator coordinator)
		{
		}
	}
}