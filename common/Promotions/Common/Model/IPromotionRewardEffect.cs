using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Extensibility.Core;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Promotions.Common.Model
{
    public interface IPromotionRewardEffect : IExtensibleDataObject
    {
        int PromotionRewardEffectID { get; set; }
        int PromotionRewardID { get; set; }
	}
}
