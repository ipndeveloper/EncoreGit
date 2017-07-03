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
	public interface IPromotionRewardTargetingEffectExtensionHandler : IPromotionRewardEffectExtensionHandler
	{
		IEnumerable<IOrderAdjustmentProfileOrderItemTarget> CreateOrderLineModificationTargets(IOrderContext orderContext, IOrderAdjustmentProfile adjustmentProfile, IPromotionRewardEffect rewardEffect, ModelConcrete.PromotionQualificationResult matchResult, IPromotionRewardEffectResult effectResult);
	}
}
