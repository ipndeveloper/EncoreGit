namespace NetSteps.Data.Entities.Dto
{
    using System;

    /// <summary>
    /// Descripcion de la clase
    /// </summary>
    public partial class PromotionRewardEffectDto
    {
        /// <summary>
        /// Obtiene o establece PromotionRewardEffectID
        /// </summary>
        public int PromotionRewardEffectID { get; set; }

        public int PromotionRewardID { get; set; }

        public string ExtensionProviderKey { get; set; }

        public string RewardPropertyKey { get; set; }

        //Externos

        /// <summary>
        /// Obtiene o establece DecimalValue
        /// </summary>
        public decimal DecimalValue { get; set; }
    }
}