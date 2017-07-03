namespace NetSteps.Data.Entities.EntityModels
{
    using System.ComponentModel.DataAnnotations;

    [Table("PromotionRewardEffectApplyOrderItemPropertyValues", Schema = "Promo")]
    public class PromoPromotionRewardEffectApplyOrderItemPropertyValueTable
    {
        [Column("PromotionRewardEffectID"), Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PromotionRewardEffectID { get; set; }

        [Column("ProductPriceTypeID")]
        public int ProductPriceTypeID { get; set; }        
    }
}
