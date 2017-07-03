namespace NetSteps.Data.Entities.EntityModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// PromotionQualificationCustomerPriceTypeTotalRanges Table on BelcorpCommissions
    /// </summary>
    [Table("PromotionQualificationCustomerPriceTypeTotalRanges", Schema = "Promo")]
    public class PromoPromotionQualificationCustomerPriceTypeTotalRangeTable
    {
        [Column("PromotionQualificationID"), Key]
        public int PromotionQualificationID { get; set; }

        [Column("ProductPriceTypeID")]
        public int ProductPriceTypeID { get; set; }
    }
}