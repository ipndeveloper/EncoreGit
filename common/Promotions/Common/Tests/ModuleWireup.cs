using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Common.Model;
using NetSteps.Promotions.Common.ModelConcrete;
using NetSteps.Promotions.Common.CoreImplementations;

// Indicates this assembly is dependent upon its own wireup command:
[assembly: Wireup(typeof(NetSteps.Promotions.Common.Tests.ModuleWireup))]

namespace NetSteps.Promotions.Common.Tests
{
    /// <summary>
    /// Wireup command called at bootstrap time by the wireup coordinator.
    /// </summary>
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
