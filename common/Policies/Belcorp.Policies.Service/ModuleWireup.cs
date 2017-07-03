using System;
using System.Data.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;
using Belcorp.Policies.Core;


// Indicates this assembly is dependent upon its own wireup command:
[assembly: Wireup(typeof(Belcorp.Policies.Service.ModuleWireup))]

namespace Belcorp.Policies.Service
{
    /// <summary>
    /// Wireup command called at bootstrap time by the wireup coordinator.
    /// </summary>
    [WireupDependency(typeof(NetSteps.Encore.Core.ModuleWireup))]
    [WireupDependency(typeof(Belcorp.Policies.Core.ModuleWireup))]
    public class ModuleWireup : WireupCommand
    {

        /// <summary>
        /// Wires this module.
        /// </summary>
        /// <param name="coordinator">the coordinator</param>
        /// <seealso cref="IWireupCoordinator"/>
        protected override void PerformWireup(IWireupCoordinator coordinator)
        {
            Container.Root
                .ForType<IPoliciesService>()
                .Register<PoliciesService>
                    (
                        Param.Resolve<IWorkPolicy>()
                    )
                .ResolveAnInstancePerRequest()
                .End();
        }
    }
}
