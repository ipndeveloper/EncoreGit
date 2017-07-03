using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business
{
    public class PromotionRewardEffect
    {
        /// <summary>
        /// Obtiene o establece PromotionRewardEffectID
        /// </summary>
        public int PromotionRewardEffectID { get; set; }

        public int PromotionRewardID { get; set; }

        public string ExtensionProviderKey { get; set; }

        public string RewardPropertyKey { get; set; }

        /// <summary>
        /// Obtiene o establece DecimalValue
        /// </summary>
        public decimal DecimalValue { get; set; }
    }
}
