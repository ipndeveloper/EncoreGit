namespace DistributorBackOffice.Areas.Orders.Models.Party
{
    using NetSteps.Data.Entities;

    /// <summary>
    /// The order item model.
    /// </summary>
    public class OrderItemModel
    {
        /// <summary>
        /// Gets or sets the order item.
        /// </summary>
        public OrderItem OrderItem { get; set; }

        /// <summary>
        /// Gets or sets the hostess.
        /// </summary>
        public OrderCustomer Hostess { get; set; }

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        public Order Order { get; set; }
    }
}