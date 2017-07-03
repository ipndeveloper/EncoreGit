using NetSteps.Data.Entities.Tax;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;

[assembly: Wireup(typeof(AutoshipProcessor.ModuleWireup))]

namespace AutoshipProcessor
{
    /// <summary>
    /// Wireup command called at bootstrap time by the wireup coordinator.
    /// </summary>
    [WireupDependency(typeof(NetSteps.Encore.Core.ModuleWireup))]
    [WireupDependency(typeof(NetSteps.Common.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Data.Entities.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Payments.ModuleWireup))]
    public class ModuleWireup : WireupCommand
    {
        /// <summary>
        /// Wires this module.
        /// </summary>
        /// <param name="coordinator">the coordinator</param>
        /// <seealso cref="NetSteps.Encore.Core.IWireupCoordinator"/>
        protected override void PerformWireup(IWireupCoordinator coordinator)
        {
            // TODO: Add custom wireup logic.

            // HACK:To avoid threading problems
            Create.New<ITaxCacheKey>();
        }
    }
}
