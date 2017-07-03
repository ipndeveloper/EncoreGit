using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Promotions.Common.Model;
using NetSteps.Data.Common.Entities;
using NetSteps.OrderAdjustments.Common.Model;

namespace NetSteps.Promotions.Common.ModelConcrete
{
	public class PromotionRewardEffectResult : IPromotionRewardEffectResult
	{
		public PromotionRewardEffectResult()
		{
			TargetsSelected = new List<IOrderAdjustmentProfileOrderItemTarget>();
		}

		public IList<IOrderAdjustmentProfileOrderItemTarget> TargetsSelected { get; private set; }
	}
}
