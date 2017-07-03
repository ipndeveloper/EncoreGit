using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using NetSteps.Modules.Order.Common.Results;
using NetSteps.Modules.Order.Common.Models;

namespace NetSteps.Modules.Order.Common
{
	/// <summary>
	/// Order
	/// </summary>
    [ContractClass(typeof(OrderContract))]
	public interface ISiteOrder
	{
        /// <summary>
        /// Create a new Order.
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        IOrderCreateResult CreateOrder(IOrderCreate order);

		/// <summary>
		/// Load orders for a given account
		/// </summary>
		/// <param name="model">Load Orders Model</param>
		/// <returns></returns>
		IEnumerable<IOrderSearchResult> LoadOrders(ILoadOrderModel model);

		/// <summary>
		/// Move an order to a new Account
		/// </summary>
		/// <param name="orderID"></param>
		/// <param name="userID">AccountID</param>
		/// <returns></returns>
		IOrderMoveResult MoveOrder(int orderID, int userID);
	}

    [ContractClassFor(typeof(ISiteOrder))]
    internal abstract class OrderContract : ISiteOrder
    {
        public IOrderCreateResult CreateOrder(IOrderCreate order)
        {
            Contract.Requires<ArgumentNullException>(order != null);

            Contract.Ensures(Contract.Result<IOrderCreateResult>() != null);

            throw new NotImplementedException();
        }

        public IEnumerable<IOrderSearchResult> LoadOrders(ILoadOrderModel model)
        {
            Contract.Requires<ArgumentNullException>(model != null);

            Contract.Ensures(Contract.Result<IEnumerable<IOrderSearchResult>>() != null);

            throw new NotImplementedException();
        }

        public IOrderMoveResult MoveOrder(int orderID, int userID)
        {
            Contract.Requires<ArgumentNullException>(orderID != 0);
            Contract.Requires<ArgumentNullException>(userID != 0);

            Contract.Ensures(Contract.Result<IOrderMoveResult>() != null);

            throw new NotImplementedException();
        }

	}
}
