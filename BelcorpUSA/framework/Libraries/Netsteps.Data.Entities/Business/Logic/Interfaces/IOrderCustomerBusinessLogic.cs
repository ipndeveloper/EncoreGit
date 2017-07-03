using System.Collections.Generic;
using NetSteps.Common.Base;

namespace NetSteps.Data.Entities.Business.Logic.Interfaces
{
    public partial interface IOrderCustomerBusinessLogic
	{
        IEnumerable<ShippingMethodWithRate> GetShippingMethods(OrderCustomer customer);

		/// <summary>
		/// Clear out any orderItems that are hostess rewards from given orderCustomer.
		/// </summary>
		/// <param name="orderCustomer"></param>
		void ClearHostessRewards(OrderCustomer orderCustomer);
        BasicResponse ValidateHostessRewardItem(OrderCustomer orderCustomer, OrderItem orderItem, Order order, bool includeQuantityInCheck);
        BasicResponse ValidateHostessRewardItem(OrderCustomer customer, int quantity, int? hostRewardRuleId, Order order);
        BasicResponse ValidateHostessRewardTotal(HostessRewardRule rule, Order order);
        BasicResponse ValidateHostessRewards(OrderCustomer orderCustomer);
		decimal ParentSubtotalForShippingOrderItemTotalByType(OrderCustomer orderCustomer, Constants.OrderItemType kind);
    }
}
