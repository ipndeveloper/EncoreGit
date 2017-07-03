using System.Collections.Generic;
using NetSteps.Data.Common.Entities;
using NetSteps.Extensibility.Core;
using NetSteps.Data.Common.Context;

namespace NetSteps.OrderAdjustments.Common.Test.Mocks
{
    public class MockOrderAdjustment : IOrderAdjustment
    {
        public MockOrderAdjustment()
        {
            OrderModifications = new List<IOrderAdjustmentOrderModification>();
            OrderLineModifications = new List<IOrderAdjustmentOrderLineModification>();
        }
        public IList<IOrderAdjustmentOrderModification> OrderModifications { get; private set; }

        public IList<IOrderAdjustmentOrderLineModification> OrderLineModifications { get; private set; }

        public string Description { get; set; }

        public int OrderAdjustmentID { get; set; }

        public void MarkAsDeleted()
        {
        }

        public IOrderAdjustmentOrderModification AddOrderModification(string Description, string PropertyName, decimal AdjustmentValue, int OrderModificationKind)
        {
            MockOrderAdjustmentOrderModification modification = new MockOrderAdjustmentOrderModification();
            modification.ModificationDescription = Description;
            modification.ModificationOperationID = OrderModificationKind;
            modification.ModificationDecimalValue = AdjustmentValue;
            modification.PropertyName = PropertyName;
            OrderModifications.Add(modification);
            return modification;
        }

        public IOrderAdjustmentOrderLineModification AddOrderLineModification(string Description, string PropertyName, IOrderItem newItem, decimal AdjustmentValue, int OrderModificationKind, int? MaximumQuantityAffected)
        {
            MockOrderAdjustmentOrderLineModification modification = new MockOrderAdjustmentOrderLineModification();
            modification.ModificationDescription = Description;
            modification.PropertyName = PropertyName;
            modification.ProductID = newItem.ProductID.Value;
            modification.ModificationOperationID = OrderModificationKind;
            modification.ModificationDecimalValue = AdjustmentValue;
            modification.MaximumQuantityAffected = MaximumQuantityAffected;
            newItem.OrderLineModifications.Add(modification);
            OrderLineModifications.Add(modification);
            return modification;
        }

        public int[] GetAllowedOrderStatusIds()
        {
            return new int[] { 3, 5, 7 };
        }

        public string ExtensionProviderKey { get; set; }

        public IDataObjectExtension Extension { get; set; }

        public bool WasCommitted { get; set; }

        public void AddOrderLineModification(IOrderAdjustmentOrderLineModification lineModification)
        {
            throw new System.NotImplementedException();
        }

        public void AddOrderModification(IOrderAdjustmentOrderModification modification)
        {
            throw new System.NotImplementedException();
        }


		public void AddOrderStep(Data.Common.Context.IOrderStep orderStep)
		{
			_injectedOrderSteps.Add(orderStep);
		}

		private List<IOrderStep> _injectedOrderSteps = new List<IOrderStep>();

		public IEnumerable<Data.Common.Context.IOrderStep> InjectedOrderSteps
		{
			get { return _injectedOrderSteps; }
		}
	}
}
