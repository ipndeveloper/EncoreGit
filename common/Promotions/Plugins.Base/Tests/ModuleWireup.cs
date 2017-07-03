using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;

// Indicates this assembly is dependent upon its own wireup command:
[assembly: Wireup(typeof(Base.Tests.ModuleWireup))]

namespace Base.Tests
{
    /// <summary>
    /// Wireup command called at bootstrap time by the wireup coordinator.
    /// </summary>
    [WireupDependency(typeof(NetSteps.Encore.Core.ModuleWireup))]
    [WireupDependency(typeof(NetSteps.Extensibility.ModuleWireup))]
    [WireupDependency(typeof(NetSteps.OrderAdjustments.Common.ModuleWireup))]
    [WireupDependency(typeof(NetSteps.Promotions.Common.ModuleWireup))]
    public class ModuleWireup : WireupCommand
    {

        protected override void PerformWireup(IWireupCoordinator coordinator)
        {
            var isroot = Container.Current.IsRoot;

            
        }
    }
}
