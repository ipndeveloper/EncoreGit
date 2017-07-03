using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace NetSteps.OrderAdjustments.Common.Model.ModelConcrete
{
    [Serializable]
    public class OrderAdjustmentProfileOrderLineModification : IOrderAdjustmentProfileOrderLineModification
    {
        public int ProductID { get; set; }

        public string Description { get; set; }

        public string Property { get; set; }

        public int ModificationOperationID { get; set; }

        public int? Quantity { get; set; }

        public decimal? ModificationValue { get; set; }

        public int OrderCustomerAccountID { get; set; }

        public IOrderAdjustmentProfileOrderLineModification NextOrderLineModification { get; set; }
    }
}
