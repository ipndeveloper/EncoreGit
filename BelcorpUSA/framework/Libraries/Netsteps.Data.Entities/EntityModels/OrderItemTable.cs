namespace NetSteps.Data.Entities.EntityModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// orderItems Table on BelcorpCommissions
    /// </summary>
    [Table("OrderItems")]
    public class OrderItemTable
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Column("OrderItemID"), Key]
        public int OrderItemID { get; set; }

        [Column("OrderCustomerID")]
        public int OrderCustomerID { get; set; }

        [Column("Quantity")]
        public int Quantity { get; set; }
    }
}