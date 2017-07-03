using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business
{
    public class PromotionReward
    {
        /// <summary>
        /// Obtiene o establece PromotionRewardID
        /// </summary>
        public int PromotionRewardID { get; set; }

        public int PromotionID { get; set; }

        public string PromotionPropertyKey { get; set; }

        public string PromotionRewardKind { get; set; }
    }
}
