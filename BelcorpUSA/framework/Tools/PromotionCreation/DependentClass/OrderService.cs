using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSteps.Data.Common.Services;

namespace DependentClass
{
    public class OrderService : IOrderService
    {
        public void AddOrderItemsToOrderBundle(NetSteps.Data.Common.Context.IOrderContext orderContext, NetSteps.Data.Common.Entities.IOrderItem bundleItem, IEnumerable<NetSteps.Data.Common.Context.IOrderItemQuantityModification> itemsToAdd, int bundleGroupID)
        {
            throw new NotImplementedException();
        }

        public void AttachToParty(NetSteps.Data.Common.Entities.IOrder order, int partyID)
        {
            throw new NotImplementedException();
        }

        public void DetachFromParty(NetSteps.Data.Common.Entities.IOrder order)
        {
            throw new NotImplementedException();
        }

        public int GetAccountOrderCount(int accountID)
        {
            throw new NotImplementedException();
        }

        public NetSteps.Data.Common.Entities.IOrder Load(int orderID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<NetSteps.Data.Common.Entities.IOrderItem> SplitOrderItem(NetSteps.Data.Common.Context.IOrderContext orderContext, NetSteps.Data.Common.Entities.IOrderItem targetOrderItem, IEnumerable<int> newOrderItemQuantities)
        {
            throw new NotImplementedException();
        }

        public void UpdateOrder(NetSteps.Data.Common.Context.IOrderContext orderContext)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<NetSteps.Data.Common.Entities.IOrderItem> UpdateOrderItemProperties(NetSteps.Data.Common.Context.IOrderContext orderContext, IEnumerable<NetSteps.Data.Common.Context.IOrderItemPropertyModification> itemsToUpdate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<NetSteps.Data.Common.Entities.IOrderItem> UpdateOrderItemQuantities(NetSteps.Data.Common.Context.IOrderContext orderContext, IEnumerable<NetSteps.Data.Common.Context.IOrderItemQuantityModification> itemsToUpdate)
        {
            throw new NotImplementedException();
        }


        public List<string> GetActivePromotionCodes(int accountID)
        {
            throw new NotImplementedException();
        }

        public NetSteps.Common.Base.BasicResponse SubmitOrder(NetSteps.Data.Common.Context.IOrderContext orderContext)
        {
            throw new NotImplementedException();
        }
    }
}
