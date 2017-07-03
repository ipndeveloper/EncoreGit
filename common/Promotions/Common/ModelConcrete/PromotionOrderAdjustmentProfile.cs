using System.Collections.Generic;
using NetSteps.Data.Common.Context;
using NetSteps.OrderAdjustments.Common.Model;
using NetSteps.Promotions.Common.Model;
using System;

namespace NetSteps.Promotions.Common.ModelConcrete
{
    [Serializable]
    public class PromotionOrderAdjustmentProfile : IPromotionOrderAdjustmentProfile
    {
        public PromotionOrderAdjustmentProfile()
        {
            OrderModifications = new List<IOrderAdjustmentProfileOrderModification>();
            OrderLineModificationTargets = new List<IOrderAdjustmentProfileOrderItemTarget>();
			AddedOrderSteps = new List<IOrderAdjustmentOrderStep>();
            AffectedAccountIDs = new List<int>();

        }
        public int PromotionID { get; set; }

        public IList<IOrderAdjustmentProfileOrderModification> OrderModifications { get; private set; }

        public IList<IOrderAdjustmentProfileOrderItemTarget> OrderLineModificationTargets { get; private set; }
    
        public IList<IOrderAdjustmentOrderStep> AddedOrderSteps { get; private set; }

        public string ExtensionProviderKey { get; set; }

        public string Description { get; set; }

        public int OrderID { get; set; }

        public IList<int> AccountIDs { get; private set; }

        public ICollection<int> AffectedAccountIDs { get; private set; }
    }
}
