
namespace NetSteps.Data.Common.Entities
{
    public interface IOrderAdjustmentOrderModification
    {

        string PropertyName { get; set; }

        string ModificationDescription { get; set; }

        decimal? ModificationDecimalValue { get; set; }

        int ModificationOperationID { get; set; }

        void MarkAsDeleted();

    }
}
