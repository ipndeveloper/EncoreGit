namespace NetSteps.Data.Entities.EntityModels
{
    using System.ComponentModel.DataAnnotations;

    [Table("PromotionTypes",Schema="Promo")]
    public class PromoPromotionTypeTable
    {
        /// <summary>
        /// Gets or sets Promotion Type Id
        /// </summary>
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Column("PromotionTypeID"),Key]
        public int PromotionTypeID { get; set; }

        /// <summary>
        /// Gets or sets Name
        /// </summary>
        [Column("Name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Term Name
        /// </summary>
        [Column("TermName")]
        public string TermName { get; set; }

        /// <summary>
        /// Gets or sets Active
        /// </summary>
        [Column("Active")]
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets SortIndex
        /// </summary>
        [Column("SortIndex")]
        public int SortIndex { get; set; }
    }
}
