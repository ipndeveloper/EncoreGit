namespace NetSteps.Data.Entities.EntityModels
{
    using System.ComponentModel.DataAnnotations;
    using System;

    [Table("ProductPrices")]
    public class ProductPriceTable
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Column("ProductPriceID"), Key]
        public int ProductPriceID	   { get; set; }

        [Column("ProductPriceTypeID")]
        public int ProductPriceTypeID { get; set; }

        [Column("ProductID")]
        public int ProductID { get; set; }

        [Column("CurrencyID")]
        public int CurrencyID { get; set; }

        [Column("CatalogID")]
        public int CatalogID { get; set; }

        [Column("Price")]
        public decimal Price { get; set; }

        [Column("ModifiedByUserID")]
        public int ModifiedByUserID { get; set; }

        [Column("ETLNaturalKey")]
        public string ETLNaturalKey { get; set; }

        [Column("ETLHash")]
        public string ETLHash { get; set; }

        [Column("ETLPhase")]
        public string ETLPhase { get; set; }

        [Column("ETLDate")]
        public DateTime ETLDate { get; set; }
    }
}
