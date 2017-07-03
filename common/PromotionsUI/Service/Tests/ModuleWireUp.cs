using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;

[assembly: Wireup(typeof(Tests.ModuleWireUp))]
namespace Tests
{
    [WireupDependency(typeof(NetSteps.Encore.Core.ModuleWireup))]
    [WireupDependency(typeof(NetSteps.Core.Cache.ModuleWireup))]
    [WireupDependency(typeof(NetSteps.Promotions.UI.Service.ModuleWireUp))]
    public class ModuleWireUp : WireupCommand
    {
        protected override void PerformWireup(IWireupCoordinator coordinator)
        {

        }     
    }
}
