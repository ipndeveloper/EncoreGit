namespace NetSteps.Data.Entities.EntityModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// PromoPromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmounts Table on BelcorpCommissions
    /// </summary>
    [Table("PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmounts", Schema = "Promo")]
    public class PromoPromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmountTable
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Column("PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmountID"), Key]
        public int PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmountID { get; set; }

        [Column("PromotionQualificationID")]
        public int PromotionQualificationID { get; set; }

        [Column("MinimumAmount")]
        public decimal? MinimumAmount { get; set; }

        [Column("MaximumAmount")]
        public decimal? MaximumAmount { get; set; }

        [Column("CurrencyID")]
        public int CurrencyID { get; set; }

        [Column("Cumulative")]
        public bool? Cumulative { get; set; }
    }
}