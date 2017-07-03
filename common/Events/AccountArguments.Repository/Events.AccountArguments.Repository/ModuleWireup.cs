using System.Data.Entity;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.Events.AccountArguments.Repository.Context;

[assembly: WireupDependency(typeof(NetSteps.Events.AccountArguments.Repository.ModuleWireup))]

namespace NetSteps.Events.AccountArguments.Repository
{
	public class ModuleWireup : WireupCommand
	{
		protected override void PerformWireup(IWireupCoordinator coordinator)
		{
			// Disable EF migrations.
			Database.SetInitializer<AccountEventArgumentContext>(null);
		}
	}
}
