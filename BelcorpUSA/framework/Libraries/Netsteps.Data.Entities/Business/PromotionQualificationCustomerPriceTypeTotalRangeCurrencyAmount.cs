using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business
{
    public class PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmount
    {
        /// <summary>
        /// Obtiene o establece PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmountID
        /// </summary>
        public int PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmountID { get; set; }

        public int PromotionQualificationID { get; set; }

        public decimal? MinimumAmount { get; set; }

        public decimal? MaximumAmount { get; set; }

        public int CurrencyID { get; set; }

        public bool? Cumulative { get; set; }

        /// <summary>
        /// Obtiene o establece PromotionID
        /// Externo
        /// </summary>
        public int PromotionID { get; set; }

        /// <summary>
        /// Obtiene o establece Description
        /// Externo
        /// </summary>
        public string Description { get; set; }
    }
}
