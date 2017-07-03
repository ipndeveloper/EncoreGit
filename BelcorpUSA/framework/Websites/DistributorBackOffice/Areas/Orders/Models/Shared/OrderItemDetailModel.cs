using System.Collections.Generic;
using System.Collections.ObjectModel;
using NetSteps.Orders.Common.Models;

namespace DistributorBackOffice.Areas.Orders.Models.Shared
{
    public class OrderItemDetailModel
    {
        public OrderItemDetailModel()
        {
            Messages = new Collection<string>();
        }

        public ICollection<string> Messages { get; set; }
        public IOrderItem OrderItem { get; set; }
    }
}