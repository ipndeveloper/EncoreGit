namespace NetSteps.Data.Entities.EntityModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// PromoPromotionQualifications Table on BelcorpCommissions
    /// </summary>
    [Table("PromotionQualifications", Schema = "Promo")]
    public class PromoPromotionQualificationTable
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Column("PromotionQualificationID"), Key]
        public int PromotionQualificationID { get; set; }

        [Column("PromotionID")]
        public int PromotionID { get; set; }

        [Column("ExtensionProviderKey")]
        public string ExtensionProviderKey { get; set; }

        [Column("PromotionPropertyKey")]
        public string PromotionPropertyKey { get; set; }
    }
}