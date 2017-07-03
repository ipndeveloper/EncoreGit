using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business
{
    public class PromoPromotionTypeConfigurationPerPromotion
    {
        public int PromotionTypeConfigurationPerPromotionID { get; set; }
        public int PromotionTypeConfigurationID { get; set; }
        public int PromotionID { get; set; }   
    }
}
