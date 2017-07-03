using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Promotions.Common.ModelConcrete
{
    [Serializable]
    public class BasicPromotionReward : BasePromotionReward
    {
        public const string PromotionRewardKindName = "Basic";

        public override string PromotionRewardKind
        {
            get { return PromotionRewardKindName;  }
        }

        public override string[] OrderOfApplication
        {
            get { return Effects.Keys.ToArray(); }
        }
    }
}
