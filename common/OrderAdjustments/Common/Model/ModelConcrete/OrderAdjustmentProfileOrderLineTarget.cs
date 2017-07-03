using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.OrderAdjustments.Common.Model.ModelConcrete
{
    [Serializable]
    public class OrderAdjustmentProfileOrderItemTarget : IOrderAdjustmentProfileOrderItemTarget
    {
        public OrderAdjustmentProfileOrderItemTarget()
        {
            Modifications = new List<IOrderAdjustmentProfileOrderLineModification>();
        }

        public int OrderCustomerAccountID { get; set; }

        public int ProductID { get; set; }

        public int? Quantity { get; set; }

        public IList<IOrderAdjustmentProfileOrderLineModification> Modifications { get; set; }
    }
}
