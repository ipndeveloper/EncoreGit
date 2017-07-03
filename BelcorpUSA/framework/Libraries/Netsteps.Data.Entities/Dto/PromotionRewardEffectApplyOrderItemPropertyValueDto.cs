namespace NetSteps.Data.Entities.Dto
{
    using System;

    /// <summary>
    /// Descripcion de la clase
    /// </summary>
    public partial class PromotionRewardEffectApplyOrderItemPropertyValueDto
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