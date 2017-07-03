namespace DistributorBackOffice.Areas.Orders.Models.Party
{
    using System.Collections.Generic;
    using System.Linq;

    using NetSteps.Data.Entities;

    /// <summary>
    /// The order items model.
    /// </summary>
    public class OrderItemsModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderItemsModel"/> class.
        /// </summary>
        public OrderItemsModel()
        {
            // Allow parameterless instantiation.  
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderItemsModel"/> class.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        public OrderItemsModel(OrderCustomer model)
        {
            this.OrderCustomer = model;
            this.Hostess = model.Order.GetHostess();
            this.Order = model.Order;
            this.HostessRewardParentOrderItems = this.Order.GetHostessRewardOrderItems().Where(oi => oi.ParentOrderItem == null).Select(oi => (OrderItem)oi);
        }

        /// <summary>
        /// Gets or sets the order customer.
        /// </summary>
        public OrderCustomer OrderCustomer { get; set; }

        /// <summary>
        /// Gets or sets the hostess.
        /// </summary>
        public OrderCustomer Hostess { get; set; }

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        public Order Order { get; set; }

        /// <summary>
        /// Gets or sets the hostess reward parent order items.
        /// </summary>
        public IEnumerable<OrderItem> HostessRewardParentOrderItems { get; set; }
    }
}