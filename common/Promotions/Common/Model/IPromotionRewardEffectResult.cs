using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Common.Entities;
using NetSteps.OrderAdjustments.Common.Model;

namespace NetSteps.Promotions.Common.Model
{
	public interface IPromotionRewardEffectResult
	{
		IList<IOrderAdjustmentProfileOrderItemTarget> TargetsSelected { get; }
	}
}
