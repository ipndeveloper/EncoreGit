using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Promotions.Common.Model;

namespace NetSteps.Promotions.Common
{
    public interface IPromotionRulesProvider
    {
        IEnumerable<IPromotion> ApplyRules(IEnumerable<IPromotion> applicablePromotions);
    }
}
