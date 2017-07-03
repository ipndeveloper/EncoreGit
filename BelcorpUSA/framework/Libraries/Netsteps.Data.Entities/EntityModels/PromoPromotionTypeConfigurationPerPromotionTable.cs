namespace NetSteps.Data.Entities.EntityModels
{
    using System.ComponentModel.DataAnnotations;

    [Table("PromotionTypeConfigurationPerPromotion", Schema = "Promo")]
    public class PromoPromotionTypeConfigurationPerPromotionTable
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Column("PromotionTypeConfigurationPerPromotionID"),Key]
        public int PromotionTypeConfigurationPerPromotionID { get; set; }

        [Column("PromotionTypeConfigurationID")]
        public int PromotionTypeConfigurationID { get; set; }

        [Column("PromotionID")]
        public int PromotionID { get; set; }       
    }
}
