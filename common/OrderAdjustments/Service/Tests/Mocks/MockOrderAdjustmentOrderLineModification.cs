using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Common.Entities;

namespace NetSteps.OrderAdjustments.Service.Test.Mocks
{
    public class MockOrderAdjustmentOrderLineModification : IOrderAdjustmentOrderLineModification
    {
        public IOrderItem OrderItem { get; set; }

        public int ModificationOperationID { get; set; }

        public decimal? ModificationDecimalValue { get; set; }

        public bool IsDeleted { get; private set; }
        public void MarkAsDeleted()
        {
            IsDeleted = true;
        }

        public string PropertyName { get; set; }

        public string ModificationDescription { get; set; }

        public int ProductID { get; set; }

        public int? MaximumQuantityAffected { get; set; }
    }
}
