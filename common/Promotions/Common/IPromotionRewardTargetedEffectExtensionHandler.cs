using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NetSteps.Data.Common.Context;
using NetSteps.OrderAdjustments.Common.Model;
using NetSteps.Promotions.Common.Model;
using NetSteps.Promotions.Common.Repository;

namespace NetSteps.Promotions.Common.Repository
{
	public interface IPromotionRewardTargetedEffectExtensionHandler : IPromotionRewardEffectExtensionHandler
	{
		void AddEffectToOrderLineModificationTargets(IOrderContext orderContext, IEnumerable<IOrderAdjustmentProfileOrderItemTarget> orderItemTargets, IPromotionRewardEffect rewardEffect, ModelConcrete.PromotionQualificationResult matchResult, IPromotionRewardEffectResult effectResult);
	}
}
