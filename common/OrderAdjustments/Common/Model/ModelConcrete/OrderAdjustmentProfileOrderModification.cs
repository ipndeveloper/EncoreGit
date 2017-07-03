using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.OrderAdjustments.Common.Model.ModelConcrete
{
    [Serializable]
    public class OrderAdjustmentProfileOrderModification : IOrderAdjustmentProfileOrderModification
    {
        public string Description { get; set; }

        public string Property { get; set; }

        public int ModificationOperationID { get; set; }

        public decimal ModificationValue { get; set; }
    }
}
