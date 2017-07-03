using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;
using System.Data.Entity;

[module: WireupDependency(typeof(NetSteps.Agreements.Services.Tests.ModuleWireup))]

namespace NetSteps.Agreements.Services.Tests
{
    [WireupDependency(typeof(Encore.Core.ModuleWireup))]
    [WireupDependency(typeof(Services.ModuleWireup))]
    public class ModuleWireup : WireupCommand
    {
        protected override void PerformWireup(IWireupCoordinator coordinator)
        {
            Database.SetInitializer<AgreementsContext>(null);
        }
    }
}
