using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Common.Context;
using NetSteps.Extensibility.Core;
using NetSteps.OrderAdjustments.Common.Model;
using NetSteps.Promotions.Common.Model;

namespace NetSteps.Promotions.Common.Repository
{
	public interface IPromotionRewardEffectExtensionHandler : IDataObjectExtensionProvider
	{
		bool AreEqual(IPromotionRewardEffectExtension effect1, IPromotionRewardEffectExtension effect2);

		void CheckValidity(string promotionRewardEffectKey, IPromotionRewardEffect rewardEffect, IPromotionState state);
	}
}
