namespace NetSteps.Data.Entities.Dto
{
    using System;

    /// <summary>
    /// Descripcion de la clase
    /// </summary>
    public partial class PromotionRewardDto
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