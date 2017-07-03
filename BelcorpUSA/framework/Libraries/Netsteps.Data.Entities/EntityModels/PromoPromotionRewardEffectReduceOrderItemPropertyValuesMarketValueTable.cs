namespace NetSteps.Data.Entities.EntityModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// PromotionRewardEffectReduceOrderItemPropertyValuesMarketValues Table on BelcorpCommissions
    /// </summary>
    [Table("PromotionRewardEffectReduceOrderItemPropertyValuesMarketValues", Schema = "Promo")]
    public class PromoPromotionRewardEffectReduceOrderItemPropertyValuesMarketValueTable
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Column("PromotionRewardEffectReduceOrderItemPropertyValuesMarketValueID"), Key]
        public int PromotionRewardEffectReduceOrderItemPropertyValuesMarketValueID { get; set; }

        [Column("PromotionRewardEffectID")]
        public int PromotionRewardEffectID { get; set; }

        [Column("MarketID")]
        public int MarketID { get; set; }

        [Column("DecimalValue")]
        public decimal DecimalValue { get; set; }
    }
}