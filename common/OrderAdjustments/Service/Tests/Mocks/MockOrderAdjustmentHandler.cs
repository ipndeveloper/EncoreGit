using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.OrderAdjustments.Common.Model;
using NetSteps.OrderAdjustments.Common;
using NetSteps.OrderAdjustments.Common.Exceptions;
using NetSteps.Data.Common.Entities;
using NetSteps.Data.Common.Context;

namespace NetSteps.OrderAdjustments.Service.Test.Mocks
{
    public class MockOrderAdjustmentHandler : IOrderAdjustmentHandler
    {

        public IEnumerable<IOrderAdjustmentProfile> GetOrderAdjustments(IOrderContext orderContext)
        {
            return new List<IOrderAdjustmentProfile>();
        }

        public void RemoveAdjustment(IOrderContext orderContext, IOrderAdjustment adjustment)
        {
            throw new NotImplementedException();
        }

        private bool mockFilter(IOrderContext orderContext)
        {
            return (orderContext.ValidOrderStatusIdsForOrderAdjustment.Contains(orderContext.Order.OrderStatusID));
        }

        public void ApplyAdjustments(IOrderContext orderContext, IEnumerable<IOrderAdjustmentProfile> adjustments, Predicate<IOrderContext> orderValidityFilter, Func<IOrderContext, IEnumerable<IOrderAdjustmentProfile>, IEnumerable<IOrderAdjustmentProfile>> orderAdjustmentValidityFilter, bool stripExistingAdjustments)
        {
            if (!orderValidityFilter(orderContext))
                throw new OrderAdjustmentProviderException(OrderAdjustmentProviderException.ExceptionKind.ORDER_INVALID_FOR_ADJUSTMENT_APPLICATION, "Order validity filter for the order returned false.");
            throw new NotImplementedException();
        }


        public void RemoveAllAdjustments(IOrderContext orderContext)
        {
            throw new NotImplementedException();
        }


        public void CommitAdjustments(IOrderContext orderContext)
        {
            MockOrderAdjustmentProvider provider = new MockOrderAdjustmentProvider();
            foreach (IOrderAdjustment adjustment in orderContext.Order.OrderAdjustments)
            {
                provider.CommitAdjustment(adjustment, orderContext);
            }
        }
    }
}
