using System.Collections.Generic;
using NetSteps.Data.Common.Entities;
using NetSteps.Data.Common.Models;
using System.Linq;

namespace NetSteps.Data.Common.Services
{
	public static class IOrderServiceExtensions
	{
		public static IOrderItem AddItem(this IOrderService service, IOrder order, IOrderCustomer orderCustomer, IProduct product, int quantity, short orderItemTypeID)
		{
			return service.AddItem(order, orderCustomer, product, quantity, orderItemTypeID, null, null, null);
		}

		public static void AddOrUpdateOrderItem(this IOrderService service, IOrder order, IOrderCustomer orderCustomer, IEnumerable<OrderItemUpdateInfo> productUpdates)
		{
			service.AddOrUpdateOrderItem(order, orderCustomer, productUpdates, false, null, null);
		}
	}
}
