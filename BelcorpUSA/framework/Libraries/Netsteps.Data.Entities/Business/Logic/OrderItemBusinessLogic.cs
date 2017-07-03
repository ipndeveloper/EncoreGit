using System;
using System.Collections.Generic;

namespace NetSteps.Data.Entities.Business.Logic
{
    public partial class OrderItemBusinessLogic
    {
        public override void DefaultValues(Repositories.IOrderItemRepository repository, OrderItem entity)
        {
            entity.Guid = Guid.NewGuid();
        }

        public override System.Collections.Generic.List<string> ValidatedChildPropertiesSetByParent(Repositories.IOrderItemRepository repository)
        {
            List<string> list = new List<string>() { "OrderItemID" };
            return list;
        }
    }
}

