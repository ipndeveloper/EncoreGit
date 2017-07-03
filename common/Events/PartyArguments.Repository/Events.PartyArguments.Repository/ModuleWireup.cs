using System.Data.Entity;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.Events.PartyArguments.Repository.Context;

// Indicates this assembly is dependent upon its own wireup command:
[assembly: Wireup(typeof(NetSteps.Events.PartyArguments.Repository.ModuleWireup))]
namespace NetSteps.Events.PartyArguments.Repository
{
	public class ModuleWireup : WireupCommand
	{
		protected override void PerformWireup(IWireupCoordinator coordinator)
		{
			// Disable EF migrations.
			Database.SetInitializer<PartyEventArgumentContext>(null);
		}
	}
}
