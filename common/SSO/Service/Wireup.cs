using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.Encore.Core.Wireup;

[module: WireupDependency(typeof(NetSteps.SSO.Service.ModuleWireup))]

namespace NetSteps.SSO.Service
{
    /// <summary>
    /// Wireup command called at bootstrap time by the wireup coordinator.
    /// </summary>
    [WireupDependency(typeof(NetSteps.SSO.Common.Wireup))]
    public class ModuleWireup : WireupCommand
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