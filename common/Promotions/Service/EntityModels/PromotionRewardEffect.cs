using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Extensibility.Core;
using NetSteps.Promotions.Common.Model;

namespace NetSteps.Promotions.Service.EntityModels
{
    public partial class PromotionRewardEffect : IPromotionRewardEffect, IPromotionRewardEffectExtension
    {
        public IDataObjectExtension Extension { get; set; }
    }
}
