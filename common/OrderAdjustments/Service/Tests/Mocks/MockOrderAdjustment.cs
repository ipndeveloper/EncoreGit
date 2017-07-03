using System.Collections.Generic;
using NetSteps.Data.Common.Entities;
using NetSteps.Extensibility.Core;
using System;
using NetSteps.Data.Common.Context;

namespace NetSteps.OrderAdjustments.Service.Test.Mocks
{
    public class MockOrderAdjustment : IOrderAdjustment
    {
        public MockOrderAdjustment()
        {
            OrderAdjustmentID = new Random().Next();
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

        public int[] GetAllowedOrderStatusIds()
        {
            return new int[] { 3, 5, 7 };
        }

        public string ExtensionProviderKey { get; set; }

        public IDataObjectExtension Extension { get; set; }

        public bool WasCommitted { get; set; }

        public void AddOrderLineModification(IOrderAdjustmentOrderLineModification lineModification)
        {
            OrderLineModifications.Add(lineModification);
        }

        public void AddOrderModification(IOrderAdjustmentOrderModification modification)
        {
            OrderModifications.Add(modification);
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
