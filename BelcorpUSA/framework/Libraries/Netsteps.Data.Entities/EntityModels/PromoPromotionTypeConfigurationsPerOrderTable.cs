namespace NetSteps.Data.Entities.EntityModels
{
    using System.ComponentModel.DataAnnotations;

    [Table("PromotionTypeConfigurationsPerOrder", Schema = "Promo")]
    public class PromoPromotionTypeConfigurationsPerOrderTable
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Column("PromotionTypeConfigurationsPerOrderID"),Key]
        public int PromotionTypeConfigurationsPerOrderID { get; set; }

        [Column("PromotionTypeConfigurationID")]
        public int PromotionTypeConfigurationID { get; set; }

        [Column("IncludeBAorders")]
        public bool IncludeBAorders { get; set; }        
    }
}
