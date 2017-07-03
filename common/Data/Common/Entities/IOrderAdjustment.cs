using System.Collections.Generic;
using NetSteps.Extensibility.Core;
using NetSteps.Data.Common.Context;

namespace NetSteps.Data.Common.Entities
{
    public interface IOrderAdjustment : IExtensibleDataObject
    {
		IEnumerable<IOrderStep> InjectedOrderSteps { get; }
		IList<IOrderAdjustmentOrderModification> OrderModifications { get; }
        IList<IOrderAdjustmentOrderLineModification> OrderLineModifications { get; }
        string Description { get; set; }
        int OrderAdjustmentID { get; set; }
        void MarkAsDeleted();
        int[] GetAllowedOrderStatusIds();

        void AddOrderModification(IOrderAdjustmentOrderModification modification);
        void AddOrderLineModification(IOrderAdjustmentOrderLineModification lineModification);
		void AddOrderStep(IOrderStep orderStep);
    }
}
