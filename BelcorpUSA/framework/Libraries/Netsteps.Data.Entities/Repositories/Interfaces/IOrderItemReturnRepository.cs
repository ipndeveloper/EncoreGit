using System.Collections.Generic;
using NetSteps.Data.Entities.EntityModels;

namespace NetSteps.Data.Entities.Repositories
{
	public partial interface IOrderItemReturnRepository
	{
		OrderItemReturn LoadByOrderItemID(int orderItemID);
		List<OrderItemReturn> LoadAllByOrderItemID(int orderItemID);
        //List<OrderReturnTable> GetOrderItemReturnByParentOrderID(int orderItemID);
	}
}
