using System;
using System.Data.Common;
using NetSteps.Data.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.Extensibility.Core;
using NetSteps.OrderAdjustments.Common;
using NetSteps.Promotions.Common;
using NetSteps.Promotions.Common.Repository;
using NetSteps.Promotions.Service.EntityModels;
using NetSteps.Promotions.Service.Repository;
using NetSteps.Foundation.Common;
using NetSteps.Promotions.Common.Model;
using NetSteps.Promotions.Common.Cache;

// Indicates this assembly is dependent upon its own wireup command:
[assembly: Wireup(typeof(NetSteps.Promotions.Service.ModuleWireup))]

namespace NetSteps.Promotions.Service
{
    /// <summary>
    /// Wireup command called at bootstrap time by the wireup coordinator.
    /// </summary>
    [WireupDependency(typeof(NetSteps.Encore.Core.ModuleWireup))]
    [WireupDependency(typeof(NetSteps.Extensibility.ModuleWireup))]
    [WireupDependency(typeof(NetSteps.OrderAdjustments.Common.ModuleWireup))]
    [WireupDependency(typeof(NetSteps.Promotions.Common.ModuleWireup))]
    [WireupDependency(typeof(NetSteps.Promotions.Caching.ModuleWireup))]
    public class ModuleWireup : WireupCommand
    {

        /// <summary>
        /// Wires this module.
        /// </summary>
        /// <param name="coordinator">the coordinator</param>
        /// <seealso cref="IWireupCoordinator"/>
        protected override void PerformWireup(IWireupCoordinator coordinator)
        {
			Container.Current.ForType<IPromotionValidator>().Register<PromotionValidator>().ResolveAsSingleton().End();

			Container.Root
				.ForType<IPromotionOrderContextQualifier>()
				.Register<PromotionOrderContextQualifier>()
				.ResolveAsSingleton()
				.End();
			
            Container.Root
                 .ForType<IPromotionRepository>()
                 .Register<PromotionRepository>(Param.Resolve<IDataObjectExtensionProviderRegistry>(), Param.Resolve<IPromotionValidator>(), Param.Resolve<IPromotionKindManager>(), Param.Resolve<IPromotionRewardKindManager>(), Param.Value<Func<IPromotionRewardEffect>>(() => Create.New<IPromotionRewardEffect>()))
                 .ResolveAnInstancePerRequest()
                 .End();

            Container.Root
                 .ForType<IPromotionOrderAdjustmentRepository>()
                 .Register<PromotionOrderAdjustmentRepository>(Param.Resolve<IPromotionUnitOfWork>())
                 .ResolveAnInstancePerRequest()
                 .End();
            
            Container.Root
                .ForType<IPromotionProvider>()
                .Register<PromotionProvider>(
                                            Param.Resolve<IPromotionService>(), 
                                            Param.Resolve<IDataObjectExtensionProviderRegistry>(),  
                                            Param.Resolve<IPromotionRewardHandlerManager>(),  
                                            Param.Resolve<IPromotionDataProvider>(),  
                                            Param.Resolve<IPromotionOrderAdjustmentRepository>(),  
                                            Param.Resolve<IPromotionOrderContextQualifier>(),  
                                            Param.Value<Func<IPromotionUnitOfWork>>(() => Create.New<IPromotionUnitOfWork>()),
                                            Param.Value<Func<IPromotionOrderAdjustment>>(() => Create.New<IPromotionOrderAdjustment>()),
                                            Param.Value<Func<IPromotionOrderAdjustmentProfile>>(() => Create.New<IPromotionOrderAdjustmentProfile>())
                                            )
                                            
                .ResolveAsSingleton()
                .End();

            Container.Root
                .ForType<IPromotionService>()
                .Register<PromotionService>
                    (
                        Param.Resolve<IPromotionValidator>(),
                        Param.Resolve<IPromotionDataProvider>(),
                        Param.Resolve<IPromotionOrderAdjustmentRepository>(),
                        Param.Resolve<IPromotionRewardHandlerManager>(),
                        Param.Resolve<IDataObjectExtensionProviderRegistry>(),
                        Param.Resolve<IPromotionKindManager>(),
                        Param.Resolve<IPromotionOrderContextQualifier>(),
                        Param.Value<Func<IPromotionUnitOfWork>>(() => { return Create.New<IPromotionUnitOfWork>(); })
                    )
                .ResolveAsSingleton()
                .End();

            Create.New<IDataObjectExtensionProviderRegistry>().RegisterExtensionProvider<IPromotionProvider>(PromotionProvider.ProviderKey);
            Create.New<IOrderAdjustmentProviderManager>().RegisterAdjustmentProvider(Create.New<IPromotionProvider>());
        }
    }
}
