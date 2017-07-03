using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.OrderAdjustments.Common.Model
{
    public interface IOrderAdjustmentProfileOrderItemTarget
    {
        int OrderCustomerAccountID { get; set; }
        int ProductID { get; set; }
        int? Quantity { get; set; }
        IList<IOrderAdjustmentProfileOrderLineModification> Modifications { get; }
    }
}
