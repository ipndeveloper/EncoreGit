namespace NetSteps.Data.Entities.EntityModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// OrderItemPrices Table on BelcorpCommissions
    /// </summary>
    [Table("OrderItemPrices")]
    public class OrderItemPriceTable
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Column("OrderItemPriceID"), Key]
        public int OrderItemPriceID { get; set; }

        [Column("OrderItemID")]
        public int OrderItemID { get; set; }

        [Column("OriginalUnitPrice")]
        public decimal? OriginalUnitPrice { get; set; }

        [Column("ProductPriceTypeID")]
        public int ProductPriceTypeID { get; set; }
    }
}