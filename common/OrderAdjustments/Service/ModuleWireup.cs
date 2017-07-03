using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.OrderAdjustments.Common;
using NetSteps.Extensibility.Core;
using NetSteps.Data.Common.Services;

// Indicates this assembly is dependent upon its own wireup command:
[assembly: Wireup(typeof(NetSteps.OrderAdjustments.Service.ModuleWireup))]

namespace NetSteps.OrderAdjustments.Service
{
    /// <summary>
    /// Wireup command called at bootstrap time by the wireup coordinator.
    /// </summary>
    [WireupDependency(typeof(NetSteps.Encore.Core.ModuleWireup))]
    [WireupDependency(typeof(NetSteps.Extensibility.ModuleWireup))]
    [WireupDependency(typeof(NetSteps.OrderAdjustments.Common.ModuleWireup))]
    public class ModuleWireup : WireupCommand
    {

        /// <summary>
        /// Wires this module.
        /// </summary>
        /// <param name="coordinator">the coordinator</param>
        /// <seealso cref="IWireupCoordinator"/>
        protected override void PerformWireup(IWireupCoordinator coordinator)
        {
            

            Container.Current
                    .ForType<IOrderAdjustmentService>()
                    .Register<OrderAdjustmentService>(Param.Resolve<IOrderAdjustmentProviderManager>())
                    .ResolveAsSingleton()
                    .End();

			Container.Current
                    .ForType<IOrderAdjustmentHandler>()
                    .Register<OrderAdjustmentHandler>(
														Param.Resolve<IOrderAdjustmentProviderManager>(),
														Param.Resolve<IDataObjectExtensionProviderRegistry>(),
														Param.Resolve<IInventoryService>()
													 )
                    .ResolveAsSingleton()
                    .End();
        }
    }
}
