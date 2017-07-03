using System.Collections.Generic;
using NetSteps.OrderAdjustments.Common.Model;

namespace NetSteps.OrderAdjustments.Common.Test.Mocks
{
    public class MockOrderAdjustmentProfile : IOrderAdjustmentProfile
    {
        public MockOrderAdjustmentProfile()
        {
            OrderModifications = new List<IOrderAdjustmentProfileOrderModification>();
            OrderLineModificationTargets = new List<IOrderAdjustmentProfileOrderItemTarget>();
            AffectedAccountIDs = new List<int>();
        }

        public int MockOrderAdjustmentProfileID { get; set; }

        public IList<IOrderAdjustmentProfileOrderModification> OrderModifications { get; set; }

        public IList<IOrderAdjustmentProfileOrderItemTarget> OrderLineModificationTargets { get; set; }

        public string ExtensionProviderKey { get; set; }

        public string Description { get; set; }


		public IList<IOrderAdjustmentOrderStep> AddedOrderSteps { get; set; }

        public ICollection<int> AffectedAccountIDs { get; set; }
    }
}
