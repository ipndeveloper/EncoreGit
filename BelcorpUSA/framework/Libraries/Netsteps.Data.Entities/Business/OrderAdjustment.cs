using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Data.Common.Context;
using NetSteps.Data.Common.Entities;
using NetSteps.Data.Entities.Business.Interfaces;
using NetSteps.Encore.Core.IoC;
using NetSteps.Extensibility.Core;

namespace NetSteps.Data.Entities
{
    [ContainerRegister(typeof(IOrderAdjustment), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Default)]
    public partial class OrderAdjustment : IOrderAdjustment, IDateLastModified
    {
        public IDataObjectExtension Extension { get; set; }
        public IList<IOrderAdjustmentOrderModification> OrderModifications { get { return OrderAdjustmentOrderModifications.Cast<IOrderAdjustmentOrderModification>().ToList(); } }
        public IList<IOrderAdjustmentOrderLineModification> OrderLineModifications { get { return OrderAdjustmentOrderLineModifications.Cast<IOrderAdjustmentOrderLineModification>().ToList(); } }

        void IOrderAdjustment.MarkAsDeleted()
        {
            this.MarkAsDeleted();
        }

        public IOrderAdjustmentOrderModification AddOrderModification(string Description, string PropertyName, decimal AdjustmentValue, int OrderModificationKind)
        {
            OrderAdjustmentOrderModification modification = new OrderAdjustmentOrderModification() { ModificationDescription = Description, PropertyName = PropertyName, ModificationDecimalValue = AdjustmentValue, ModificationOperationID = OrderModificationKind };
            OrderAdjustmentOrderModifications.Add(modification);
            return modification;
        }

        public IOrderAdjustmentOrderLineModification AddOrderLineModification(string Description, string PropertyName, IOrderItem newItem, decimal AdjustmentValue, int OrderModificationKind, int? MaximumQuantityAffected)
        {
            OrderAdjustmentOrderLineModification modification = new OrderAdjustmentOrderLineModification() { ModificationDescription = Description, PropertyName = PropertyName, OrderItem = (OrderItem)newItem, ModificationDecimalValue = AdjustmentValue, ModificationOperationID = OrderModificationKind };
            OrderAdjustmentOrderLineModifications.Add(modification);
            return modification;
        }

        public int[] GetAllowedOrderStatusIds()
        {
            throw new NotImplementedException();
        }

        public void AddOrderLineModification(IOrderAdjustmentOrderLineModification lineModification)
        {
            OrderAdjustmentOrderLineModifications.Add((OrderAdjustmentOrderLineModification)lineModification);
        }

        public void AddOrderModification(IOrderAdjustmentOrderModification modification)
        {
            OrderAdjustmentOrderModifications.Add((OrderAdjustmentOrderModification)modification);
        }


		public void AddOrderStep(Common.Context.IOrderStep orderStep)
		{
			_injectedOrderSteps.Add(orderStep);
		}

		public List<IOrderStep> _injectedOrderSteps = new List<IOrderStep>();
		public IEnumerable<Common.Context.IOrderStep> InjectedOrderSteps { get { return _injectedOrderSteps; } }
	}
}
