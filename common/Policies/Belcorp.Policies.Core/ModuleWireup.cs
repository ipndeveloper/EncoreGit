using System;
using System.Data.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;
using Belcorp.Policies.Entities;
using Belcorp.Policies.Data.UnitOfWork.Interface;
using Belcorp.Policies.Data.UnitOfWork;


// Indicates this assembly is dependent upon its own wireup command:
[assembly: Wireup(typeof(Belcorp.Policies.Core.ModuleWireup))]

namespace Belcorp.Policies.Core
{
    /// <summary>
    /// Wireup command called at bootstrap time by the wireup coordinator.
    /// </summary>
    [WireupDependency(typeof(NetSteps.Encore.Core.ModuleWireup))]
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
                 .ForType<ICoreEntities>()
                 .Register<CoreEntities>()
                 .ResolveAsSingleton()
                 .End();

            Container.Root
                .ForType<IUnitOfWork>()
                .Register<UnitOfWork<CoreEntities>>()
                .ResolveAnInstancePerRequest()
                .End();
            
            Container.Root
                .ForType<IWorkPolicy>()
                .Register<WorkPolicy>
                    (
                        Param.Resolve<IUnitOfWork>()
                    )
                .ResolveAnInstancePerRequest()
                .End();
        }
    }
}
