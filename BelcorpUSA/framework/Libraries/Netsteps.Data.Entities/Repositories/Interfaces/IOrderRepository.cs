using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using NetSteps.Addresses.Common.Models;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Dto;

namespace NetSteps.Data.Entities.Repositories
{
	[ContractClass(typeof(Contracts.OrderRepositoryContracts))]
	public partial interface IOrderRepository : IBaseRepository<Order, int>, ISearchRepository<OrderSearchParameters, PaginatedList<OrderSearchData>>
	{
        //@01 A01 BAL
        Order LoadByID(int orderID);

		Order LoadByOrderNumber(string orderNumber);
		Order LoadByOrderNumberFull(string orderNumber);
		List<OrderShipment> LoadOrderShipments(int orderID);
		List<Order> LoadChildOrders(int parentOrderID, params int[] childOrderTypeID);
		List<Order> LoadChildOrdersFull(int parentOrderID, params int[] childOrderTypeID);
		List<Order> LoadChildOrdersForHostessRewards(int parentOrderID, params int[] childOrderTypeIDs);
		bool ExistsByOrderNumber(string orderNumber);
		PaginatedList<AuditLogRow> GetAuditLog(Order fullyLoadedOrder, AuditLogSearchParameters searchParameters);
		int Count(OrderSearchParameters searchParameters);
		IEnumerable<Order> LoadOrderWithShipmentAndPaymentDetails(IEnumerable<string> orderNumbers);
		Order LoadWithShipmentDetails(int orderID);
		List<Order> LoadBatchWithShipmentDetails(IEnumerable<int> orderIDs);
		List<DateTime> GetCompletedOrderDates(int? orderTypeID = null, int? parentOrderID = null, int? orderCustomerAccountID = null, Constants.SortDirection sortDirection = Constants.SortDirection.Ascending, int? pageSize = null);
		Order LoadOrderWithPaymentDetails(int orderID);
		IEnumerable<Order> LoadOrdersByParentOrderIdAndOrderType(int parentOrderId, short orderTypeId);
		Order LoadFullFirstOrDefault(Expression<Func<Order, bool>> predicate);
		Order LoadFullFirstOrDefault(Expression<Func<Order, bool>> predicate, NetStepsEntities context);
		Order LoadFirstOrDefault(Expression<Func<Order, bool>> predicate, Order.Relations relations);
		Order LoadFirstOrDefault(Expression<Func<Order, bool>> predicate, Order.Relations relations, NetStepsEntities context);
		List<Order> LoadFullWhere(Expression<Func<Order, bool>> predicate);
		List<Order> LoadFullWhere(Expression<Func<Order, bool>> predicate, NetStepsEntities context);
		List<Order> LoadWhere(Expression<Func<Order, bool>> predicate, Order.Relations relations);
		List<Order> LoadWhere(Expression<Func<Order, bool>> predicate, Order.Relations relations, NetStepsEntities context);
		IList<OrderCustomer> LoadOrderCustomers(int orderID);
		IAddress GetDefaultShippingAddress(int orderId, int orderCustomerId = 0);
		int GetAccountCompletedOrderCount(int accountId);
		decimal GetTotal(int orderId, Constants.ProductPriceType productPriceType); 
	  
        bool ValidateOrderRulesPreCondition(IOrderRepository repository, int? accountId, out string message);
        
        /// <summary>
        /// Obtiene el Id de la order por filtros
        /// </summary>
        /// <param name="number">Numero de Orden</param>
        /// <param name="idsupport">Id de Ticket de Soporte</param>
        /// <returns>Id de la orden</returns>
        int OrderIdByFilters(string number, int idsupport);

        /// <summary>
        /// Obtiene el ID support
        /// </summary>
        /// <param name="orderId">Id de la Orden</param>
        /// <returns></returns>
        int? IDSupportTicketByOrder(int orderId);

        IEnumerable<OrderDto> OrdersInPeriod(int periodID);
	} 

	namespace Contracts
	{
		[ContractClassFor(typeof(IOrderRepository))]
		internal abstract class OrderRepositoryContracts : IOrderRepository
		{
            Order IOrderRepository.LoadByID(int orderID)
            {
                throw new NotImplementedException();
            }

			Order IOrderRepository.LoadByOrderNumber(string orderNumber)
			{
				throw new NotImplementedException();
			}

			Order IOrderRepository.LoadByOrderNumberFull(string orderNumber)
			{
				throw new NotImplementedException();
			}

			List<OrderShipment> IOrderRepository.LoadOrderShipments(int orderID)
			{
				throw new NotImplementedException();
			}

			List<Order> IOrderRepository.LoadChildOrders(int parentOrderID, params int[] childOrderTypeIDs)
			{
				throw new NotImplementedException();
			}

			List<Order> IOrderRepository.LoadChildOrdersFull(int parentOrderID, params int[] childOrderTypeIDs)
			{
				throw new NotImplementedException();
			}

			List<Order> IOrderRepository.LoadChildOrdersForHostessRewards(int parentOrderID, params int[] childOrderTypeIDs)
			{
				throw new NotImplementedException();
			}

			bool IOrderRepository.ExistsByOrderNumber(string orderNumber)
			{
				throw new NotImplementedException();
			}

			PaginatedList<AuditLogRow> IOrderRepository.GetAuditLog(Order fullyLoadedOrder, AuditLogSearchParameters searchParameters)
			{
				throw new NotImplementedException();
			}

			int IOrderRepository.Count(OrderSearchParameters searchParameters)
			{
				throw new NotImplementedException();
			}

			IEnumerable<Order> IOrderRepository.LoadOrderWithShipmentAndPaymentDetails(IEnumerable<string> orderNumbers)
			{
				throw new NotImplementedException();
			}

			Order IOrderRepository.LoadWithShipmentDetails(int orderID)
			{
				throw new NotImplementedException();
			}

			List<Order> IOrderRepository.LoadBatchWithShipmentDetails(IEnumerable<int> orderIDs)
			{
				throw new NotImplementedException();
			}

			List<DateTime> IOrderRepository.GetCompletedOrderDates(int? orderTypeID, int? parentOrderID, int? orderCustomerAccountID, NetSteps.Common.Constants.SortDirection sortDirection, int? pageSize)
			{
				throw new NotImplementedException();
			}

			Order IOrderRepository.LoadOrderWithPaymentDetails(int orderID)
			{
				throw new NotImplementedException();
			}

			IEnumerable<Order> IOrderRepository.LoadOrdersByParentOrderIdAndOrderType(int parentOrderId, short orderTypeId)
			{
				throw new NotImplementedException();
			}

			Order IOrderRepository.LoadFullFirstOrDefault(Expression<Func<Order, bool>> predicate)
			{
				Contract.Requires<ArgumentNullException>(predicate != null);

				throw new NotImplementedException();
			}

			Order IOrderRepository.LoadFullFirstOrDefault(Expression<Func<Order, bool>> predicate, NetStepsEntities context)
			{
				Contract.Requires<ArgumentNullException>(predicate != null);
				Contract.Requires<ArgumentNullException>(context != null);

				throw new NotImplementedException();
			}

			Order IOrderRepository.LoadFirstOrDefault(Expression<Func<Order, bool>> predicate, Order.Relations relations)
			{
				Contract.Requires<ArgumentNullException>(predicate != null);

				throw new NotImplementedException();
			}

			Order IOrderRepository.LoadFirstOrDefault(Expression<Func<Order, bool>> predicate, Order.Relations relations, NetStepsEntities context)
			{
				Contract.Requires<ArgumentNullException>(predicate != null);
				Contract.Requires<ArgumentNullException>(context != null);

				throw new NotImplementedException();
			}

			List<Order> IOrderRepository.LoadFullWhere(Expression<Func<Order, bool>> predicate)
			{
				Contract.Requires<ArgumentNullException>(predicate != null);

				throw new NotImplementedException();
			}

			List<Order> IOrderRepository.LoadFullWhere(Expression<Func<Order, bool>> predicate, NetStepsEntities context)
			{
				Contract.Requires<ArgumentNullException>(predicate != null);
				Contract.Requires<ArgumentNullException>(context != null);

				throw new NotImplementedException();
			}

			List<Order> IOrderRepository.LoadWhere(Expression<Func<Order, bool>> predicate, Order.Relations relations)
			{
				Contract.Requires<ArgumentNullException>(predicate != null);

				throw new NotImplementedException();
			}

			List<Order> IOrderRepository.LoadWhere(Expression<Func<Order, bool>> predicate, Order.Relations relations, NetStepsEntities context)
			{
				Contract.Requires<ArgumentNullException>(predicate != null);
				Contract.Requires<ArgumentNullException>(context != null);

				throw new NotImplementedException();
			}

			IList<OrderCustomer> IOrderRepository.LoadOrderCustomers(int orderID)
			{
				throw new NotImplementedException();
			}

			IAddress IOrderRepository.GetDefaultShippingAddress(int orderId, int orderCustomerId)
			{
				throw new NotImplementedException();
			}

			NetSteps.Common.PrimaryKeyInfo IBaseRepository<Order, int>.PrimaryKeyInfo
			{
				get { throw new NotImplementedException(); }
			}

			Order IBaseRepository<Order, int>.Load(int primaryKey)
			{
				throw new NotImplementedException();
			}

			Order IBaseRepository<Order, int>.LoadFull(int primaryKey)
			{
				throw new NotImplementedException();
			}

			List<Order> IBaseRepository<Order, int>.LoadAll()
			{
				throw new NotImplementedException();
			}

			List<Order> IBaseRepository<Order, int>.LoadAllFull()
			{
				throw new NotImplementedException();
			}

			List<Order> IBaseRepository<Order, int>.LoadBatch(IEnumerable<int> primaryKeys)
			{
				throw new NotImplementedException();
			}

			List<Order> IBaseRepository<Order, int>.LoadBatchFull(IEnumerable<int> primaryKeys)
			{
				throw new NotImplementedException();
			}

			SqlUpdatableList<Order> IBaseRepository<Order, int>.LoadAllFullWithSqlDependency()
			{
				throw new NotImplementedException();
			}

			SqlUpdatableList<Order> IBaseRepository<Order, int>.LoadBatchWithSqlDependency(IEnumerable<int> primaryKeys)
			{
				throw new NotImplementedException();
			}

			void IBaseRepository<Order, int>.Save(Order obj)
			{
				throw new NotImplementedException();
			}

			void IBaseRepository<Order, int>.SaveBatch(IEnumerable<Order> items)
			{
				throw new NotImplementedException();
			}

			void IBaseRepository<Order, int>.Delete(Order obj)
			{
				throw new NotImplementedException();
			}

			void IBaseRepository<Order, int>.Delete(int primaryKey)
			{
				throw new NotImplementedException();
			}

			void IBaseRepository<Order, int>.DeleteBatch(IEnumerable<int> primaryKeys)
			{
				throw new NotImplementedException();
			}

			bool IBaseRepository<Order, int>.Exists(int primaryKey)
			{
				throw new NotImplementedException();
			}

			PaginatedList<AuditLogRow> IBaseRepository<Order, int>.GetAuditLog(int primaryKey, AuditLogSearchParameters param)
			{
				throw new NotImplementedException();
			}

			Order IBaseRepository<Order, int>.GetRandomRecord()
			{
				throw new NotImplementedException();
			}

			Order IBaseRepository<Order, int>.GetRandomRecordFull()
			{
				throw new NotImplementedException();
			}

			int IBaseRepository<Order, int>.Count()
			{
				throw new NotImplementedException();
			}

			int IBaseRepository<Order, int>.Count(Expression<Func<Order, bool>> predicate)
			{
				throw new NotImplementedException();
			}

			bool IBaseRepository<Order, int>.Any()
			{
				throw new NotImplementedException();
			}

			bool IBaseRepository<Order, int>.Any(Expression<Func<Order, bool>> predicate)
			{
				throw new NotImplementedException();
			}

			List<Order> IBaseRepository<Order, int>.Where(Expression<Func<Order, bool>> predicate)
			{
				throw new NotImplementedException();
			}

			List<Order> IBaseRepository<Order, int>.Where(Expression<Func<Order, bool>> predicate, IEnumerable<string> includes)
			{
				throw new NotImplementedException();
			}

			List<TSelected> IBaseRepository<Order, int>.WhereSelect<TSelected>(Expression<Func<Order, bool>> predicate, Expression<Func<Order, TSelected>> selector)
			{
				throw new NotImplementedException();
			}

			Order IBaseRepository<Order, int>.FirstOrDefault(Expression<Func<Order, bool>> predicate)
			{
				throw new NotImplementedException();
			}

			Order IBaseRepository<Order, int>.FirstOrDefault(Expression<Func<Order, bool>> predicate, IEnumerable<string> includes)
			{
				throw new NotImplementedException();
			}

			TSelected IBaseRepository<Order, int>.FirstOrDefaultSelect<TSelected>(Expression<Func<Order, bool>> predicate, Expression<Func<Order, TSelected>> selector)
			{
				throw new NotImplementedException();
			}

			PaginatedList<OrderSearchData> ISearchRepository<OrderSearchParameters, PaginatedList<OrderSearchData>>.Search(OrderSearchParameters searchParams)
			{
				throw new NotImplementedException();
			}

			int IOrderRepository.GetAccountCompletedOrderCount(int accountId)
			{
				throw new NotImplementedException();
			}

			public decimal GetTotal(int orderId, Constants.ProductPriceType productPriceType)
			{
				Contract.Requires<ArgumentOutOfRangeException>(orderId > 0);
				Contract.Requires<ArgumentOutOfRangeException>(productPriceType != Constants.ProductPriceType.NotSet);

				throw new NotImplementedException();
			}


            public bool ValidateOrderRulesPreCondition(IOrderRepository repository, int? accountId, out string message)
            {
                throw new NotImplementedException();
            }



            public int OrderIdByFilters(string number, int idsupport)
            {
                throw new NotImplementedException();
            }


            public int? IDSupportTicketByOrder(int orderId)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<OrderDto> OrdersInPeriod(int periodID)
            {
                throw new NotImplementedException();
            }
        }

	}
}
