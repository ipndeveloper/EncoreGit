using System.Collections.Generic;
using NetSteps.Data.Entities;

namespace DistributorBackOffice.Areas.Orders.Models.Shared
{
    public class IndexModel
    {
        public Order Order { get; set; }
        public IList<OrderItemDetailModel> OrderItemDetails { get; set; }
    }
}