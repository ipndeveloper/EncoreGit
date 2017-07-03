namespace NetSteps.Data.Entities.EntityModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// PromoPromotionRewardss Table on BelcorpCommissions
    /// </summary>
    [Table("PromotionRewards", Schema = "Promo")]
    public class PromoPromotionRewardTable
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Column("PromotionRewardID"), Key]
        public int PromotionRewardID { get; set; }

        [Column("PromotionID")]
        public int PromotionID { get; set; }

        [Column("PromotionPropertyKey")]
        public string PromotionPropertyKey { get; set; }

        [Column("PromotionRewardKind")]
        public string PromotionRewardKind { get; set; }
    }
}