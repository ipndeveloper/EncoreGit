namespace NetSteps.Data.Entities.Dto
{
    using System;    
    public class PromoPromotionDto
    {
        public int PromotionID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Description { get; set; }
        public int PromotionStatusTypeID { get; set; }
        public int? SuccessorPromotionID { get; set; }
        public string PromotionKind { get; set; }

        public string Status { get; set; }
        public int PromotionTypeConfigurationPerPromotionID { get; set; }

        /// <summary>
        /// Obtiene o establece Cumulative -Foraneo
        /// </summary>
        public bool? Cumulative { get; set; }

        /// <summary>
        /// Obtiene o establece ConditionProductPriceTypeId
        /// </summary>
        public int ConditionProductPriceTypeId { get; set; }

        /// <summary>
        /// Obtiene o establece RewardProductPriceTypeId
        /// </summary>
        public int RewardProductPriceTypeId { get; set; }
    }
}
