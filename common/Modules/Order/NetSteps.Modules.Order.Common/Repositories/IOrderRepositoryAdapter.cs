using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Modules.Order.Common.Results;
using NetSteps.Modules.Order.Common.Models;

namespace NetSteps.Modules.Order.Common
{
	/// <summary>
	/// Order Adapter
	/// </summary>
	public interface IOrderRepositoryAdapter
	{
        /// <summary>
        /// Create a new Order
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        IOrderCreateResult CreateOrder(IOrderCreate order);

		/// <summary>
		/// Load orders for a given account
		/// </summary>
		/// <param name="model">Load Order Model</param>
		/// <returns></returns>
		IEnumerable<IOrder> LoadOrders(ILoadOrderModel model);

		/// <summary>
		/// Move an order to a new Account
		/// </summary>
		/// <param name="orderID"></param>
		/// <param name="userID">AccountID</param>
		/// <returns></returns>
		IOrder MoveOrder(int orderID, int userID);

    }
}
