using NetSteps.Data.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.Extensibility.Core;
using NetSteps.Promotions.Common.Model;
using NetSteps.Promotions.Plugins.Base;
using NetSteps.Promotions.Plugins.Common;
using NetSteps.Promotions.Plugins.Common.Qualifications;
using NetSteps.Promotions.Plugins.Common.Rewards.Effects;
using NetSteps.Promotions.Plugins.Qualifications;
using NetSteps.Promotions.Plugins.Rewards.Base;
using NetSteps.Promotions.Plugins.Rewards.Effects;
using NetSteps.Promotions.Common;
using NetSteps.Promotions.Plugins.Rewards;
using NetSteps.Promotions.Plugins.Common.Rewards;
using System;
using NetSteps.Data.Common.Services;

// Indicates this assembly is dependent upon its own wireup command:
[assembly: Wireup(typeof(NetSteps.Promotions.Plugins.ModuleWireup))]

namespace NetSteps.Promotions.Plugins
{
    /// <summary>
    /// Wireup command called at bootstrap time by the wireup coordinator.
    /// </summary>
    [WireupDependency(typeof(NetSteps.Encore.Core.ModuleWireup))]
    [WireupDependency(typeof(NetSteps.Promotions.Common.ModuleWireup))]
    [WireupDependency(typeof(NetSteps.Promotions.Plugins.Common.ModuleWireup))]
    [WireupDependency(typeof(NetSteps.Extensibility.ModuleWireup))]
    public class ModuleWireup : WireupCommand
    {

        /// <summary>
        /// Wires this module.
        /// </summary>
        /// <param name="coordinator">the coordinator</param>
        /// <seealso cref="IWireupCoordinator"/>
        protected override void PerformWireup(IWireupCoordinator coordinator)
        {
            Container.Root.ForType<AccountHasTitleQualificationHandler>()
                .Register<AccountHasTitleQualificationHandler>(Param.Resolve<ITitleService>())
                .ResolveAnInstancePerRequest()
                .End();

            RegisterQualification<IAccountHasTitleQualificationExtension, IAccountHasTitleQualificationRepository, AccountHasTitleQualificationRepository, AccountHasTitleQualificationHandler, IEncorePromotionsPluginsUnitOfWork>(() => Create.New<AccountHasTitleQualificationHandler>());
            RegisterQualification<IAccountListQualificationExtension, IAccountListQualificationRepository, AccountListQualificationRepository, AccountListQualificationHandler, IEncorePromotionsPluginsUnitOfWork>();
            RegisterQualification<IAccountTypeQualificationExtension, IAccountTypeQualificationRepository, AccountTypeQualificationRepository, AccountTypeQualificationHandler, IEncorePromotionsPluginsUnitOfWork>();
            //INI - GR_Encore-07
            RegisterQualification<IAccountConsistencyStatusQualificationExtension, IAccountConsistencyStatusQualificationRepository, AccountConsistencyStatusQualificationRepository, AccountConsistencyStatusQualificationHandler, IEncorePromotionsPluginsUnitOfWork>();
            RegisterQualification<IActivityStatusQualificationExtension, IActivityStatusQualificationRepository, ActivityStatusQualificationRepository, ActivityStatusQualificationHandler, IEncorePromotionsPluginsUnitOfWork>();
            RegisterQualification<IContinuityQualificationExtension, IContinuityQualificationRepository, ContinuityQualificationRepository, ContinuityQualificationHandler, IEncorePromotionsPluginsUnitOfWork>();
            //FIN - GR_Encore-07
            RegisterQualification<IProductInOrderQualificationExtension, IProductInOrderQualificationRepository, ProductInOrderQualificationRepository, ProductInOrderQualificationHandler, IEncorePromotionsPluginsUnitOfWork>();
            RegisterQualification<IMarketListQualificationExtension, IMarketListQualificationRepository, MarketListQualificationRepository, MarketListQualificationHandler, IEncorePromotionsPluginsUnitOfWork>();
            RegisterQualification<ICustomerSubtotalRangeQualificationExtension, ICustomerSubtotalRangeQualificationRepository, CustomerSubtotalRangeQualificationRepository, CustomerSubtotalRangeQualificationHandler, IEncorePromotionsPluginsUnitOfWork>();
            RegisterQualification<IOrderSubtotalRangeQualificationExtension, IOrderSubtotalRangeQualificationRepository, OrderSubtotalRangeQualificationRepository, OrderSubtotalRangeQualificationHandler, IEncorePromotionsPluginsUnitOfWork>();
            RegisterQualification<ICustomerPriceTypeTotalRangeQualificationExtension, ICustomerPriceTypeTotalRangeQualificationRepository, CustomerPriceTypeTotalRangeQualificationRepository, CustomerPriceTypeTotalRangeQualificationHandler, IEncorePromotionsPluginsUnitOfWork>();
            RegisterQualification<IOrderPriceTypeTotalRangeQualificationExtension, IOrderPriceTypeTotalRangeQualificationRepository, OrderPriceTypeTotalRangeQualificationRepository, OrderPriceTypeTotalRangeQualificationHandler, IEncorePromotionsPluginsUnitOfWork>();
            RegisterQualification<IOrderTypeQualificationExtension, IOrderTypeQualificationRepository, OrderTypeQualificationRepository, OrderTypeQualificationHandler, IEncorePromotionsPluginsUnitOfWork>();
            RegisterQualification<IPromotionCodeQualificationExtension, IPromotionCodeQualificationRepository, PromotionCodeQualificationRepository, PromotionCodeQualificationHandler, IEncorePromotionsPluginsUnitOfWork>();

            Container.Root.ForType<UseCountQualificationHandler>()
                .Register<UseCountQualificationHandler>(Param.Resolve<IOrderService>(), Param.Resolve<IPromotionProvider>(), Param.Resolve<IUseCountQualificationRepository>(), Param.Value<Func<IEncorePromotionsPluginsUnitOfWork>>(() => Create.New<IEncorePromotionsPluginsUnitOfWork>()))
                .ResolveAnInstancePerRequest()
                .End();

            RegisterQualification<IUseCountQualificationExtension, IUseCountQualificationRepository, UseCountQualificationRepository, UseCountQualificationHandler, IEncorePromotionsPluginsUnitOfWork>(() => Create.New<UseCountQualificationHandler>());
            RegisterQualification<IOrderHasMinimumProductSelectionsQualificationExtension, IOrderHasMinimumProductSelectionsQualificationRepository, OrderHasMinimumProductSelectionsQualificationRepository, OrderHasMinimumProductSelectionsQualificationHandler, IEncorePromotionsPluginsUnitOfWork>();
            RegisterQualification<ICustomerIsHostQualificationExtension, ICustomerIsHostQualificationRepository, CustomerIsHostQualificationRepository, CustomerIsHostQualificationHandler, IEncorePromotionsPluginsUnitOfWork>();


            RegisterRewardEffect<IAddItemByFactorInCartPromotionRewardEffect, IAddItemByFactorInCartPromotionRewardEffectRepository, AddItemByFactorInOrderRewardEffectRepository, AddItemByFactorInOrderRewardEffectHandler, IEncorePromotionsPluginsUnitOfWork>();
            RegisterRewardEffect<IAddItemPromotionRewardEffect, IAddItemPromotionRewardEffectRepository, AddItemsRewardEffectRepository, AddItemsRewardEffectHandler, IEncorePromotionsPluginsUnitOfWork>();
            RegisterRewardEffect<IReduceOrderItemPropertyValuePromotionRewardEffect, IReduceOrderItemPropertyValuePromotionRewardEffectRepository, ReduceOrderItemPropertyValueRewardEffectRepository, ReduceOrderItemPropertyValueRewardEffectHandler, IEncorePromotionsPluginsUnitOfWork>();
            RegisterRewardEffect<IReduceOrderPropertyValuePromotionRewardEffect, IReduceOrderPropertyValuePromotionRewardEffectRepository, ReduceOrderPropertyValueRewardEffectRepository, ReduceOrderPropertyValueRewardEffectHandler, IEncorePromotionsPluginsUnitOfWork>();
            RegisterRewardEffect<ISelectAllItemsPromotionRewardEffect, ISelectAllItemsPromotionRewardEffectRepository, SelectAllItemsRewardEffectRepository, SelectAllItemsRewardEffectHandler, IEncorePromotionsPluginsUnitOfWork>();
            RegisterRewardEffect<ISelectItemWithProductIDPromotionRewardEffect, ISelectItemWithProductIDPromotionRewardEffectRepository, SelectItemWithProductIDRewardEffectRepository, SelectItemWithProductIDRewardEffectHandler, IEncorePromotionsPluginsUnitOfWork>();
            RegisterRewardEffect<IUserProductSelectionRewardEffect, IUserProductSelectionRewardEffectRepository, UserProductSelectionRewardEffectRepository, UserProductSelectionRewardHandler, IEncorePromotionsPluginsUnitOfWork>();

            var promotionRewardHandlerManager = Create.New<IPromotionRewardHandlerManager>();
            promotionRewardHandlerManager.RegisterHandler<OrderShippingTotalReductionRewardHandler>(RewardKinds.OrderShippingTotalReductionReward);
            promotionRewardHandlerManager.RegisterHandler<OrderSubtotalReductionRewardHandler>(RewardKinds.OrderSubtotalReductionReward);
            promotionRewardHandlerManager.RegisterHandler<ProductRewardHandler>(RewardKinds.ProductPromotionReward);
            promotionRewardHandlerManager.RegisterHandler<SelectFreeItemsFromListRewardHandler>(RewardKinds.SelectFreeItemsFromListReward);
            promotionRewardHandlerManager.RegisterHandler<SimpleProductAdditionRewardHandler>(RewardKinds.SimpleProductAdditionReward);


        }


        /// <summary>
        /// Performs wireup for the extension types.
        /// </summary>
        /// <typeparam name="I">The interface type</typeparam>
        /// <typeparam name="IR">The interface of the repository.</typeparam>
        /// <typeparam name="CR">The concrete type of the repository.</typeparam>
        /// <typeparam name="H">The concrete handler for the type.</typeparam>
        /// <typeparam name="TUnitOfWork"></typeparam>
        private void RegisterQualification<I, IR, CR, H, TUnitOfWork>()
            where I : IPromotionQualificationExtension
            where IR : IPromotionQualificationRepository<I>
            where CR : IR
            where TUnitOfWork : IUnitOfWork
            where H : BasePromotionQualificationHandler<I, IR, TUnitOfWork>, new()
        {
            RegisterQualification<I, IR, CR, H, TUnitOfWork>(() => new H());
        }

        /// <summary>
        /// Registers the qualification.
        /// </summary>
        /// <typeparam name="I"></typeparam>
        /// <typeparam name="IR">The type of the r.</typeparam>
        /// <typeparam name="CR">The type of the r.</typeparam>
        /// <typeparam name="H"></typeparam>
        /// <typeparam name="TUnitOfWork">The type of the unit of work.</typeparam>
        /// <param name="HandlerConstructor">The handler constructor.</param>
        private void RegisterQualification<I, IR, CR, H, TUnitOfWork>(Func<H> HandlerConstructor)
            where I : IPromotionQualificationExtension
            where IR : IPromotionQualificationRepository<I>
            where CR : IR
            where TUnitOfWork : IUnitOfWork
            where H : BasePromotionQualificationHandler<I, IR, TUnitOfWork>
        {
            // register repository
            Container.Root.ForType<IR>()
                .Register<CR>()
                .ResolveAnInstancePerRequest()
                .End();

            // register handler
            var registry = Create.New<IDataObjectExtensionProviderRegistry>();
            string providerKey = HandlerConstructor().GetProviderKey();
            registry.RegisterExtensionProvider<H>(providerKey);

            // register dto type
            registry.RegisterProvidedTypeForRegisteredProvider(typeof(I).ToString(), providerKey);
            registry.RegisterProvidedTypeForRegisteredProvider(Container.Root.Registry.ForType<I>().Resolver.TargetType.ToString(), providerKey);
        }

        /// <summary>
        /// Performs wireup for the extension types.
        /// </summary>
        /// <typeparam name="IEffectType">The interface type</typeparam>
        /// <typeparam name="IEffectRepository">The interface of the repository.</typeparam>
        /// <typeparam name="EffectConcreteRepository">The concrete type of the repository.</typeparam>
        /// <typeparam name="EffectHandler">The concrete handler for the type.</typeparam>
        /// <typeparam name="TUnitOfWork"></typeparam>
        private void RegisterRewardEffect<IEffectType, IEffectRepository, EffectConcreteRepository, EffectHandler, TUnitOfWork>()
            where IEffectType : IPromotionRewardEffectExtension
            where IEffectRepository : IPromotionRewardEffectExtensionRepository<IEffectType>
            where EffectConcreteRepository : IEffectRepository
            where TUnitOfWork : IUnitOfWork
            where EffectHandler : BasePromotionRewardEffectExtensionHandler<IEffectType, IEffectRepository, TUnitOfWork>, new()
        {
            // register repository
            Container.Root.ForType<IEffectRepository>()
                .Register<EffectConcreteRepository>()
                .ResolveAnInstancePerRequest()
                .End();

            // register handler
            var registry = Create.New<IDataObjectExtensionProviderRegistry>();
            string providerKey = new EffectHandler().GetProviderKey();
            registry.RegisterExtensionProvider<EffectHandler>(providerKey);

            // register dto type
            registry.RegisterProvidedTypeForRegisteredProvider(typeof(IEffectType).ToString(), providerKey);
            registry.RegisterProvidedTypeForRegisteredProvider(Create.New<IEffectType>().GetType().ToString(), providerKey);
        }
    }
}
