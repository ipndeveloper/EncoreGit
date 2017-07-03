using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Extensibility.Core;

namespace NetSteps.Promotions.Common.Model
{
    public interface IPromotionRewardEffectSimpleExtension : IDataObjectExtension
    {
        int PromotionRewardEffectID { get; set; }
    }
}
