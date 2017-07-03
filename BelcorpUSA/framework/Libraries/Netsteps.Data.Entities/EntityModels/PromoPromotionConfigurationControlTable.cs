namespace NetSteps.Data.Entities.EntityModels
{
    using System.ComponentModel.DataAnnotations;

    [Table("PromotionConfigurationControl", Schema = "Promo")]
    public class PromoPromotionConfigurationControlTable
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Column("PromotionConfigurationControlID"),Key]
        public int PromotionConfigurationControlID { get; set; }

        [Column("PromotionTypeConfigurationID")]
        public int PromotionTypeConfigurationID { get; set; }

        [Column("PromotionID")]
        public int PromotionID { get; set; }

        [Column("AccountID")]
        public int AccountID { get; set; }

        [Column("PeriodID")]
        public int PeriodID { get; set; }

        [Column("Amount")]
        public decimal Amount { get; set; }
    }
}
