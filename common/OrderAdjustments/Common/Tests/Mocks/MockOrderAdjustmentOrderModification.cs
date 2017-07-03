using NetSteps.Data.Common.Entities;

namespace NetSteps.OrderAdjustments.Common.Test.Mocks
{
    public class MockOrderAdjustmentOrderModification : IOrderAdjustmentOrderModification
    {
        public string PropertyName { get; set; }

        public string ModificationDescription { get; set; }

        public decimal? ModificationDecimalValue { get; set; }

        public int ModificationOperationID { get; set; }

        public bool IsDeleted { get; private set; }

        public void MarkAsDeleted()
        {
            IsDeleted = true;
        }
    }
}
