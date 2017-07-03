namespace NetSteps.Data.Entities.EntityModels
{
    using System.ComponentModel.DataAnnotations;

    [Table("PromotionTypeConfigurations", Schema = "Promo")] 
    public class PromoPromotionTypeConfigurationTable
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Column("PromotionTypeConfigurationID"), Key]
        public int PromotionTypeConfigurationID { get; set; }

        [Column("PromotionTypeID")]
        public int PromotionTypeID { get; set; }

        [Column("Active")]
        public bool Active { get; set; }

        [Column("IncludeBAorders")]
        public bool IncludeBAorders { get; set; }
    }
}
