using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Dto
{
    public class PromoPromotionConfigurationControlDto
    {
        public int PromotionConfigurationControlID { get; set; }
        public int PromotionTypeConfigurationID { get; set; }
        public int PromotionID { get; set; }
        public int AccountID { get; set; }
        public int PeriodID { get; set; }

        /// <summary>
        /// Obtiene o establece Amount
        /// </summary>
        public decimal Amount { get; set; }
    }
}
