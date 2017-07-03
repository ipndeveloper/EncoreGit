using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business
{
    public class PromotionRewardEffectApplyOrderItemPropertyValue
    {
        public int PromotionRewardEffectID { get; set; }
        
        public int ProductPriceTypeID { get; set; }

        /// <summary>
        /// Obtiene o establece PromotionRewardID
        /// </summary>
        public int PromotionRewardID { get; set; }

        /// <summary>
        /// Obtiene o establece Percentage
        /// </summary>
        public decimal DecimalValue { get; set; }


    }
}
