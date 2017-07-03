using EFTQuery.Common;
using EFTQuery.Service;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;

// Indicates this assembly is dependent upon its own wireup command:
[assembly: Wireup(typeof(EFTQuery.NachaGenerator.Core.ModuleWireup))]

namespace EFTQuery.NachaGenerator.Core
{
    /// <summary>
    /// Wireup command called at bootstrap time by the wireup coordinator.
    /// </summary>
    [WireupDependency(typeof(EFTQuery.Service.ModuleWireup))]
    public class ModuleWireup : WireupCommand
    {
        /// <summary>
        /// Wires this module.
        /// </summary>
        /// <param name="coordinator">the coordinator</param>
        /// <seealso cref="IWireupCoordinator"/>
        protected override void PerformWireup(IWireupCoordinator coordinator)
        {
            var root = Container.Root;
            root.ForType<IEFTQueryProcessor>()
                .Register<NachaQueryProcessor>(Param.Resolve<IOrderPaymentRepository>())
                      .ResolveAnInstancePerRequest()
                      .End();
        }
    }
}