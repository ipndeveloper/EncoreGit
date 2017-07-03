using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NetSteps.Promotions.Common.Repository;
using NetSteps.Promotions.Plugins.Rewards.Base;
using NetSteps.Promotions.Plugins.Common.Rewards.Effects;
using NetSteps.Promotions.Plugins.Common;
using NetSteps.OrderAdjustments.Common.Model;
using NetSteps.Data.Common.Context;
using NetSteps.Promotions.Common.Model;
using NetSteps.Promotions.Common.ModelConcrete;

namespace NetSteps.Promotions.Plugins.Rewards.Effects
{
    public class SelectItemsInProductIDSetEffectHandler : BasePromotionRewardEffectExtensionHandler<ISelectItemsInProductIDSetPromotionRewardEffect, ISelectItemsInProductIDSetPromotionRewardEffectRepository, IEncorePromotionsPluginsUnitOfWork>, IPromotionRewardTargetingEffectExtensionHandler
    {
        public virtual IEnumerable<IOrderAdjustmentProfileOrderItemTarget> CreateOrderLineModificationTargets(
			IOrderContext orderContext,
			IOrderAdjustmentProfile adjustmentProfile,
			IPromotionRewardEffect rewardEffect,
			PromotionQualificationResult matchResult,
			IPromotionRewardEffectResult effectResult)
        {
            throw new NotImplementedException();
        }

        public override bool AreEqual(Promotions.Common.Model.IPromotionRewardEffectExtension effect1, Promotions.Common.Model.IPromotionRewardEffectExtension effect2)
        {
            throw new NotImplementedException();
        }

        public override string GetProviderKey()
        {
            throw new NotImplementedException();
        }

		public override void CheckValidity(string promotionRewardEffectKey, ISelectItemsInProductIDSetPromotionRewardEffect rewardEffect, Promotions.Common.Model.IPromotionState state)
		{
			throw new NotImplementedException();
		}
	}
}
