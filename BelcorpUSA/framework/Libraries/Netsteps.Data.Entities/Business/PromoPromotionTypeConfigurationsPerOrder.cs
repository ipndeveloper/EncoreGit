using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business
{
    public class PromoPromotionTypeConfigurationsPerOrder
    {
        public int PromotionTypeConfigurationsPerOrderID { get; set; }
        public int PromotionTypeConfigurationID { get; set; }
        public bool IncludeBAorders { get; set; }
    }
}
