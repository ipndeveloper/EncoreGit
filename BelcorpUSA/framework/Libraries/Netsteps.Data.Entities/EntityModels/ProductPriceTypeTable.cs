namespace NetSteps.Data.Entities.EntityModels
{
    using System.ComponentModel.DataAnnotations;

    [Table("ProductPriceTypes")]
    public class ProductPriceTypeTable
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Column("ProductPriceTypeID"), Key]
        public int ProductPriceTypeID { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        [Column("TermName")]
        public string TermName { get; set; }

        [Column("Description")]
        public string Description { get; set; }

        [Column("Active")]
        public bool Active { get; set; }

        [Column("Editable")]
        public bool Editable { get; set; }

        [Column("Mandatory")]
        public bool Mandatory { get; set; }
    }
}
