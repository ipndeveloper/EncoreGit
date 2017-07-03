using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.Promotions.Plugins.Common.PromotionKinds.Components;
using NetSteps.Promotions.Plugins.Common.Qualifications;
using NetSteps.Promotions.Plugins.Common.Qualifications.Concrete;
using NetSteps.Promotions.Plugins.Common.Rewards.Effects;
using NetSteps.Promotions.Plugins.Rewards.Effects.Concrete;
using NetSteps.Promotions.Plugins.Common.Rewards;
using NetSteps.Promotions.Plugins.Common.Rewards.Concrete;
using NetSteps.Promotions.Plugins.Common.PromotionKinds;
using NetSteps.Promotions.Plugins.Common.PromotionKinds.Concrete;
using NetSteps.Promotions.Common;
using NetSteps.Promotions.Plugins.Common.Rewards.Effects.Concrete;
using NetSteps.Promotions.Plugins.Common.Rewards.Effects.Components;
using NetSteps.Promotions.Plugins.Common.Rewards.Effects.Components.Concrete;
using NetSteps.Promotions.Plugins.Common.PromotionKinds.Components.Concrete;
using NetSteps.Promotions.Plugins.Common.Helpers;
using NetSteps.Promotions.Plugins.Common.Helpers.Concrete;
using NetSteps.Data.Common.Services;
// Indicates this assembly is dependent upon its own wireup command:
[assembly: Wireup(typeof(NetSteps.Promotions.Plugins.Common.ModuleWireup))]

namespace NetSteps.Promotions.Plugins.Common
{
    /// <summary>
    /// Wireup command called at bootstrap time by the wireup coordinator.
    /// </summary>
    [WireupDependency(typeof(NetSteps.Encore.Core.ModuleWireup))]
    [WireupDependency(typeof(NetSteps.Data.Common.ModuleWireup))]
    [WireupDependency(typeof(NetSteps.Promotions.Common.ModuleWireup))]
    public class ModuleWireup : WireupCommand
    {

        /// <summary>
        /// Wires this module.
        /// </summary>
        /// <param name="coordinator">the coordinator</param>
        /// <seealso cref="IWireupCoordinator"/>
        protected override void PerformWireup(IWireupCoordinator coordinator)
        {
            #region Qualifications

            Container.Root.ForType<IAccountListQualificationExtension>()
                            .Register<AccountListQualification>()
                            .ResolveAnInstancePerRequest()
                            .End();

            //INI - GR_Encore-07
            Container.Root.ForType<IAccountConsistencyStatusQualificationExtension>()
                            .Register<AccountConsistencyStatusQualification>()
                            .ResolveAnInstancePerRequest()
                            .End();

            Container.Root.ForType<IActivityStatusQualificationExtension>()
                            .Register<ActivityStatusQualification>()
                            .ResolveAnInstancePerRequest()
                            .End();

            Container.Root.ForType<IContinuityQualificationExtension>()
                            .Register<ContinuityQualification>()
                            .ResolveAnInstancePerRequest()
                            .End();
            //FIN - GR_Encore-07

            Container.Root.ForType<IAccountTypeQualificationExtension>()
                            .Register<AccountTypeQualification>()
                            .ResolveAnInstancePerRequest()
                            .End();

            Container.Root.ForType<IProductInOrderQualificationExtension>()
                            .Register<ProductInOrderQualification>()
                            .ResolveAnInstancePerRequest()
                            .End();

            Container.Root.ForType<IMarketListQualificationExtension>()
                            .Register<MarketListQualification>()
                            .ResolveAnInstancePerRequest()
                            .End();

            Container.Root.ForType<ICustomerSubtotalRangeQualificationExtension>()
                            .Register<CustomerSubtotalRangeQualification>()
                            .ResolveAnInstancePerRequest()
                            .End();

            Container.Root.ForType<IOrderSubtotalRangeQualificationExtension>()
                            .Register<OrderSubtotalRangeQualification>()
                            .ResolveAnInstancePerRequest()
                            .End();

            Container.Root.ForType<IOrderTypeQualificationExtension>()
                            .Register<OrderTypeQualification>()
                            .ResolveAnInstancePerRequest()
                            .End();

            Container.Root.ForType<IPromotionCodeQualificationExtension>()
                            .Register<PromotionCodeQualification>()
                            .ResolveAnInstancePerRequest()
                            .End();

            Container.Root.ForType<IUseCountQualificationExtension>()
                            .Register<UseCountQualification>()
                            .ResolveAnInstancePerRequest()
                            .End();

            Container.Root.ForType<IOrderHasMinimumProductSelectionsQualificationExtension>()
                            .Register<OrderHasMinimumProductSelectionsQualification>()
                            .ResolveAnInstancePerRequest()
                            .End();

            Container.Root.ForType<IAccountHasTitleQualificationExtension>()
                            .Register<AccountHasTitleQualification>()
                            .ResolveAnInstancePerRequest()
                            .End();

            Container.Root.ForType<ICustomerIsHostQualificationExtension>()
                            .Register<CustomerIsHostQualification>()
                            .ResolveAnInstancePerRequest()
                            .End();

            Container.Root.ForType<ICustomerPriceTypeTotalRangeQualificationExtension>()
                            .Register<CustomerPriceTypeTotalRangeQualification>()
                            .ResolveAnInstancePerRequest()
                            .End();

            Container.Root.ForType<IOrderPriceTypeTotalRangeQualificationExtension>()
                            .Register<OrderPriceTypeTotalRangeQualification>()
                            .ResolveAnInstancePerRequest()
                            .End();

            #endregion

            #region Reward Effects

            Container.Root.ForType<IAddItemByFactorInCartPromotionRewardEffect>()
                            .Register<AddItemByFactorInOrderEffect>()
                            .ResolveAnInstancePerRequest()
                            .End();

            Container.Root.ForType<IAddItemPromotionRewardEffect>()
                            .Register<AddItemEffect>()
                            .ResolveAnInstancePerRequest()
                            .End();

            Container.Root.ForType<IReduceOrderItemPropertyValuePromotionRewardEffect>()
                            .Register<ReduceOrderItemPropertyValueEffect>()
                            .ResolveAnInstancePerRequest()
                            .End();

            Container.Root.ForType<IReduceOrderPropertyValuePromotionRewardEffect>()
                            .Register<ReduceOrderPropertyValueEffect>()
                            .ResolveAnInstancePerRequest()
                            .End();

            Container.Root.ForType<ISelectAllItemsPromotionRewardEffect>()
                            .Register<SelectAllItemsEffect>()
                            .ResolveAnInstancePerRequest()
                            .End();

            Container.Root.ForType<ISelectItemWithProductIDPromotionRewardEffect>()
                            .Register<SelectItemWithProductIDEffect>()
                            .ResolveAnInstancePerRequest()
                            .End();

            Container.Root.ForType<IUserProductSelectionRewardEffect>()
                            .Register<UserProductSelectionRewardEffect>()
                            .ResolveAnInstancePerRequest()
                            .End();

            #endregion

            #region Promotion Rewards

            var promotionRewardKindManager = Create.New<IPromotionRewardKindManager>();

            Container.Root.ForType<IProductReward>()
                            .Register<ProductPromotionReward>()
                            .ResolveAnInstancePerRequest()
                            .End();

            promotionRewardKindManager.RegisterPromotionRewardKind<IProductReward>(RewardKinds.ProductPromotionReward);

            Container.Root.ForType<IOrderShippingTotalReductionReward>()
                            .Register<OrderShippingTotalReductionReward>()
                            .ResolveAnInstancePerRequest()
                            .End();

            promotionRewardKindManager.RegisterPromotionRewardKind<IOrderShippingTotalReductionReward>(RewardKinds.OrderShippingTotalReductionReward);

            Container.Root.ForType<IOrderSubtotalReductionReward>()
                            .Register<OrderSubtotalReductionReward>(Param.Resolve<IPriceTypeService>())
                            .ResolveAnInstancePerRequest()
                            .End();

            promotionRewardKindManager.RegisterPromotionRewardKind<IOrderSubtotalReductionReward>(RewardKinds.OrderSubtotalReductionReward);

            Container.Root.ForType<ISimpleProductAdditionReward>()
                            .Register<SimpleProductAdditionReward>()
                            .ResolveAnInstancePerRequest()
                            .End();

            promotionRewardKindManager.RegisterPromotionRewardKind<ISimpleProductAdditionReward>(RewardKinds.SimpleProductAdditionReward);

            Container.Root.ForType<ISelectFreeItemsFromListReward>()
                            .Register<SelectFreeItemsFromListReward>()
                            .ResolveAnInstancePerRequest()
                            .End();

            promotionRewardKindManager.RegisterPromotionRewardKind<ISelectFreeItemsFromListReward>(RewardKinds.SelectFreeItemsFromListReward);

            #region Select Free Items component

            Container.Current.ForType<IUserProductSelectionOrderStep>()
                .Register<UserProductSelectionOrderStep>()
                .ResolveAnInstancePerRequest()
                .End();

            Container.Current.ForType<IUserProductSelectionOrderStepResponse>()
                .Register<UserProductSelectionOrderStepResponse>()
                .ResolveAnInstancePerRequest()
                .End();

            #endregion

            #endregion

            #region Promotions

            Container.Root.ForType<IAccountTitleOption>()
                .Register<AccountTitleOption>()
                .ResolveAnInstancePerRequest()
                .End();

            Container.Root.ForType<IProductAdjustment>()
                .Register<ProductAdjustment>()
                .ResolveAnInstancePerRequest()
                .End();

            var promotionKindManager = Create.New<IPromotionKindManager>();

            Container.Root.ForType<IProductPromotionFlatDiscount>()
                            .Register<ProductPromotionFlatDiscount>()
                            .ResolveAnInstancePerRequest()
                            .End();

            promotionKindManager.RegisterPromotionKind<IProductPromotionFlatDiscount>(PromotionKindNames.ProductFlatDiscount);

            Container.Root.ForType<IProductPromotionPercentDiscount>()
                            .Register<ProductPromotionPercentDiscount>()
                            .ResolveAnInstancePerRequest()
                            .End();

            promotionKindManager.RegisterPromotionKind<IProductPromotionPercentDiscount>(PromotionKindNames.ProductPercentDiscount);

            Container.Root.ForType<IOrderPromotionDefaultCartRewards>()
                            .Register<OrderPromotionDefaultCartRewards>()
                            .ResolveAnInstancePerRequest()
                            .End();

            promotionKindManager.RegisterPromotionKind<IOrderPromotionDefaultCartRewards>(PromotionKindNames.OrderDefaultCartRewards);

            #endregion

            Container.Root.ForType<IProductOption>()
                .Register<ProductOption>()
                .ResolveAnInstancePerRequest()
                .End();

            Container.Root.ForType<IPromotionInjectedOrderStepReferenceParser>()
                .Register<PromotionInjectedOrderStepReferenceParser>()
                .ResolveAnInstancePerRequest()
                .End();

        }
    }
}
