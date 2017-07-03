using System.Data.Entity;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.EventProcessing.Repository.Context;

[module: WireupDependency(typeof(NetSteps.EventProcessing.Repository.Wireup))]
namespace NetSteps.EventProcessing.Repository
{
	public class Wireup : WireupCommand
	{
		protected override void PerformWireup(IWireupCoordinator coordinator)
		{
			// Disable EF migrations.
			Database.SetInitializer<EventContext>(null);
		}
	}
}
