using System;
using System.Linq;

namespace NetSteps.Data.Common.Entities
{
    
    public interface IOrderAdjustmentOrderLineModification
    {
        int ModificationOperationID { get; set; }

        decimal? ModificationDecimalValue { get; set; }

        void MarkAsDeleted();

        string PropertyName { get; set; }

        string ModificationDescription { get; set; }

        int ProductID { get; set; }

        int? MaximumQuantityAffected { get; set; }
    }
}
