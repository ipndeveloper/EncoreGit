using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.Extensibility.Core;
using NetSteps.Promotions.Common;
using NetSteps.Data.Common.Services;

// Indicates this assembly is dependent upon its own wireup command:
[assembly: Wireup(typeof(NetSteps.Promotions.Service.Tests.ModuleWireup))]

namespace NetSteps.Promotions.Service.Tests
{
    /// <summary>
    /// Wireup command called at bootstrap time by the wireup coordinator.
    /// </summary>
    [WireupDependency(typeof(NetSteps.Encore.Core.ModuleWireup))]
    [WireupDependency(typeof(NetSteps.Extensibility.ModuleWireup))]
    [WireupDependency(typeof(NetSteps.OrderAdjustments.Common.ModuleWireup))]
    [WireupDependency(typeof(NetSteps.Promotions.Common.ModuleWireup))]
    [WireupDependency(typeof(NetSteps.Promotions.Service.ModuleWireup))]
    public class ModuleWireup : WireupCommand
    {
        /// <summary>
        /// Wires this module.
        /// </summary>
        /// <param name="coordinator">the coordinator</param>
        /// <seealso cref="NetSteps.Encore.Core.IWireupCoordinator"/>
        protected override void PerformWireup(IWireupCoordinator coordinator)
        {
			
        }
    }
}
