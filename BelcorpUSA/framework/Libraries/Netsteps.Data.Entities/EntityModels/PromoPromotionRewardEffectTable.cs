namespace NetSteps.Data.Entities.EntityModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// PromoPromotionRewardEffects Table on BelcorpCommissions
    /// </summary>
    [Table("PromotionRewardEffects", Schema = "Promo")]
    public class PromoPromotionRewardEffectTable
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Column("PromotionRewardEffectID"), Key]
        public int PromotionRewardEffectID { get; set; }

        [Column("PromotionRewardID")]
        public int PromotionRewardID { get; set; }

        [Column("ExtensionProviderKey")]
        public string ExtensionProviderKey { get; set; }

        [Column("RewardPropertyKey")]
        public string RewardPropertyKey { get; set; }
    }
}