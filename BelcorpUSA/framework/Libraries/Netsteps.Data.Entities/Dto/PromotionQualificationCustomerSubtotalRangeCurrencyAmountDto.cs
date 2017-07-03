namespace NetSteps.Data.Entities.Dto
{
    using System;

    /// <summary>
    /// Descripcion de la clase
    /// </summary>
    public partial class PromotionQualificationCustomerSubtotalRangeCurrencyAmountDto
    {
        /// <summary>
        /// Obtiene o establece PromotionQualificationCustomerSubtotalRangeCurrencyAmountID
        /// </summary>
        public int PromotionQualificationCustomerSubtotalRangeCurrencyAmountID { get; set; }

        public int PromotionQualificationID { get; set; }

        public decimal MinimumAmount { get; set; }

        public decimal MaximumAmount { get; set; }

        public int CurrencyID { get; set; }

        //extended properties

        /// <summary>
        /// Obtiene o establece PromotionID, esta propiedad solo esta disponible si es llenada desde el repository
        /// </summary>
        public int PromotionID { get; set; }

        /// <summary>
        /// Obtiene o establece Discount, esta propiedad solo esta disponible si es llenada desde el repository
        /// </summary>
        public decimal Discount { get; set; }
    }
}