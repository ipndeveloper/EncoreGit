using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;

[module: WireupDependency(typeof(NetSteps.SSO.Common.Wireup))]

namespace NetSteps.SSO.Common
{
    /// <summary>
    /// Wireup command called at bootstrap time by the wireup coordinator.
    /// </summary>
    public class Wireup : WireupCommand
    {
        /// <summary>
        /// Wires this module.
        /// </summary>
        /// <param name="coordinator">the coordinator</param>
        protected override void PerformWireup(IWireupCoordinator coordinator)
        {
            // TODO: Add custom wireup logic.

        }
    }
}
