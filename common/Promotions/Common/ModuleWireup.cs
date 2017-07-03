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
using NetSteps.Promotions.Common.Cache;
using NetSteps.Promotions.Common.Repository;

// Indicates this assembly is dependent upon its own wireup command:
[assembly: Wireup(typeof(NetSteps.Promotions.Common.ModuleWireup))]

namespace NetSteps.Promotions.Common
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
            var root = Container.Root;

			root.ForType<IPromotionKindManager>().Register<PromotionKindManager>().ResolveAsSingleton().End();
            root.ForType<IPromotionRewardKindManager>().Register<PromotionRewardKindManager>().ResolveAsSingleton().End();
            root.ForType<IPromotionRewardHandlerManager>().Register<PromotionRewardHandlerManager>().ResolveAsSingleton().End();
			root.ForType<IPromotionState>().Register<PromotionState>().ResolveAnInstancePerRequest().End();
			
            root.ForType<IPromotion>().Register<BasicPromotion>().ResolveAnInstancePerRequest().End();
            var promotionKindManager = Create.New<IPromotionKindManager>();
            promotionKindManager.RegisterPromotionKind<BasicPromotion>(BasicPromotion.PromotionKindName);

            root.ForType<IPromotionOrderAdjustmentProfile>().Register<PromotionOrderAdjustmentProfile>().ResolveAnInstancePerRequest().End();

            root.ForType<IPromotionOrderAdjustment>().Register<PromotionOrderAdjustment>().ResolveAnInstancePerRequest().End();

            root.ForType<IPromotionReward>()
                .Register<BasicPromotionReward>()
                .ResolveAnInstancePerRequest()
                .End();

			root.ForType<IPromotionRewardEffect>().Register<PromotionRewardEffect>().ResolveAnInstancePerRequest().End();

            //root.ForType<IPromotionDataProvider>()
            //    .Register<NonCachingPromotionDataProvider>(Param.Resolve<IPromotionRepository>())
            //    .ResolveAsSingleton()
            //    .End();

			root.ForType<IPromotionRewardEffectResult>()
				.Register<PromotionRewardEffectResult>()
				.ResolveAnInstancePerRequest()
				.End();
        }
    }
}
