using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.Extensibility.Core;
using NetSteps.Promotions.Common.Cache;
using NetSteps.Promotions.Common.Repository;

// Indicates this assembly is dependent upon its own wireup command:
[assembly: Wireup(typeof(NetSteps.Promotions.Caching.ModuleWireup))]

namespace NetSteps.Promotions.Caching
{
    /// <summary>
    /// Wireup command called at bootstrap time by the wireup coordinator.
    /// </summary>
    [WireupDependency(typeof(NetSteps.Encore.Core.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Core.Cache.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Promotions.Common.ModuleWireup))]
    public class ModuleWireup : WireupCommand
    {
        /// <summary>
        /// Wires this module.
        /// </summary>
        /// <param name="coordinator">the coordinator</param>
        /// <seealso cref="NetSteps.Encore.Core.Wireup.IWireupCoordinator"/>
        protected override void PerformWireup(IWireupCoordinator coordinator)
        {
			Container.Root.ForType<IPromotionDataProvider>()
				.Register<CachingPromotionDataProvider>(Param.Resolve<IPromotionRepository>())
				.ResolveAsSingleton()
				.End();
        }
    }
}
