
namespace NetSteps.Data.Entities.EntityModels
{
    using System.ComponentModel.DataAnnotations;

    [Table("PromotionStatusTypes", Schema = "Promo")]
    public class PromoPromotionStatusTypeTable
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Column("PromotionStatusType"), Key]
        public int PromotionStatusType { get; set; }

        [Column("TermName")]
        public string TermName { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        [Column("Active")]
        public bool Active { get; set; }
    }
}
