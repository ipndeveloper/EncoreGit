namespace NetSteps.Data.Entities.EntityModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// PromotionQualificationCustomerSubtotalRangeCurrencyAmounts Table on BelcorpCommissions
    /// </summary>
    [Table("PromotionQualificationCustomerSubtotalRangeCurrencyAmounts", Schema = "Promo")]
    public class PromotionQualificationCustomerSubtotalRangeCurrencyAmountTable
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Column("PromotionQualificationCustomerSubtotalRangeCurrencyAmountID"), Key]
        public int PromotionQualificationCustomerSubtotalRangeCurrencyAmountID { get; set; }

        [Column("PromotionQualificationID")]
        public int PromotionQualificationID { get; set; }

        [Column("MinimumAmount")]
        public decimal MinimumAmount { get; set; }

        [Column("MaximumAmount")]
        public decimal MaximumAmount { get; set; }

        [Column("CurrencyID")]
        public int CurrencyID { get; set; }
    }
}