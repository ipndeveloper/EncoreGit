using System.Data.Entity;
using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.Events.EmailEventTemplate.Repository.Context;
using NetSteps.Encore.Core.Wireup;

[assembly: WireupDependency(typeof(NetSteps.Events.EmailEventTemplate.Repository.ModuleWireup))]

namespace NetSteps.Events.EmailEventTemplate.Repository
{
	public class ModuleWireup : WireupCommand
	{
		protected override void PerformWireup(IWireupCoordinator coordinator)
		{
			// Disable EF migrations.
			Database.SetInitializer<EmailEventTemplateContext>(null);
		}
	}
}
