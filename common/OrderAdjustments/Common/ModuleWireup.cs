using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.OrderAdjustments.Common.Component;
using NetSteps.OrderAdjustments.Common.Model;
using NetSteps.OrderAdjustments.Common.Model.ModelConcrete;

// Indicates this assembly is dependent upon its own wireup command:
[assembly: Wireup(typeof(NetSteps.OrderAdjustments.Common.ModuleWireup))]

namespace NetSteps.OrderAdjustments.Common
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
            Container.Root.ForType<IOrderAdjustmentProfileOrderModification>()
                .Register<OrderAdjustmentProfileOrderModification>()
                .ResolveAnInstancePerRequest()
                .End();

            Container.Root.ForType<IOrderAdjustmentProfileOrderLineModification>()
                .Register<OrderAdjustmentProfileOrderLineModification>()
                .ResolveAnInstancePerRequest()
                .End();

            Container.Root.ForType<IOrderAdjustmentProfileOrderItemTarget>()
                .Register<OrderAdjustmentProfileOrderItemTarget>()
                .ResolveAnInstancePerRequest()
                .End();

            Container.Root.ForType<IOrderAdjustmentValueCalculator>()
                .Register<OrderAdjustmentValueCalculator>()
                .ResolveAsSingleton()
                .End();

            Container.Root
                    .ForType<IOrderAdjustmentProviderManager>()
                    .Register<OrderAdjustmentProviderManager>()
                    .ResolveAsSingleton()
                    .End();
        }
    }
}
