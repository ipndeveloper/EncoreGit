using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Promotions.Common.Model;

namespace NetSteps.Promotions.Common.ModelConcrete
{
    [Serializable]
    public class PromotionOrderAdjustment : IPromotionOrderAdjustment
    {
        public int OrderAdjustmentID { get; set; }

        public int PromotionID { get; set; }
    }
}
