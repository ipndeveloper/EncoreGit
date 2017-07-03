using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace NetSteps.Data.Entities.Repositories
{
	[ContractClass(typeof(Contracts.OrderItemRepositoryContracts))]
    public partial interface IOrderItemRepository
    {
        IEnumerable<OrderItem> GetOrderItemsNotInStoreFront(Order order, int storeFrontId);
    }

	namespace Contracts
	{
		[ContractClassFor(typeof(IOrderItemRepository))]
		abstract class OrderItemRepositoryContracts : IOrderItemRepository
		{
			public IEnumerable<OrderItem> GetOrderItemsNotInStoreFront(Order order, int storeFrontId)
			{
				Contract.Requires<ArgumentNullException>(order != null);
				Contract.Requires<ArgumentOutOfRangeException>(storeFrontId > 0);

				throw new NotImplementedException();
			}

			public NetSteps.Common.PrimaryKeyInfo PrimaryKeyInfo
			{
				get { throw new NotImplementedException(); }
			}

			public OrderItem Load(int primaryKey)
			{
				throw new NotImplementedException();
			}

			public OrderItem LoadFull(int primaryKey)
			{
				throw new NotImplementedException();
			}

			public List<OrderItem> LoadAll()
			{
				throw new NotImplementedException();
			}

			public List<OrderItem> LoadAllFull()
			{
				throw new NotImplementedException();
			}

			public List<OrderItem> LoadBatch(IEnumerable<int> primaryKeys)
			{
				throw new NotImplementedException();
			}

			public List<OrderItem> LoadBatchFull(IEnumerable<int> primaryKeys)
			{
				throw new NotImplementedException();
			}

			public NetSteps.Common.Base.SqlUpdatableList<OrderItem> LoadAllFullWithSqlDependency()
			{
				throw new NotImplementedException();
			}

			public NetSteps.Common.Base.SqlUpdatableList<OrderItem> LoadBatchWithSqlDependency(IEnumerable<int> primaryKeys)
			{
				throw new NotImplementedException();
			}

			public void Save(OrderItem obj)
			{
				throw new NotImplementedException();
			}

			public void SaveBatch(IEnumerable<OrderItem> items)
			{
				throw new NotImplementedException();
			}

			public void Delete(OrderItem obj)
			{
				throw new NotImplementedException();
			}

			public void Delete(int primaryKey)
			{
				throw new NotImplementedException();
			}

			public void DeleteBatch(IEnumerable<int> primaryKeys)
			{
				throw new NotImplementedException();
			}

			public bool Exists(int primaryKey)
			{
				throw new NotImplementedException();
			}

			public NetSteps.Common.Base.PaginatedList<AuditLogRow> GetAuditLog(int primaryKey, Business.AuditLogSearchParameters param)
			{
				throw new NotImplementedException();
			}

			public OrderItem GetRandomRecord()
			{
				throw new NotImplementedException();
			}

			public OrderItem GetRandomRecordFull()
			{
				throw new NotImplementedException();
			}

			public int Count()
			{
				throw new NotImplementedException();
			}

			public int Count(System.Linq.Expressions.Expression<System.Func<OrderItem, bool>> predicate)
			{
				throw new NotImplementedException();
			}

			public bool Any()
			{
				throw new NotImplementedException();
			}

			public bool Any(System.Linq.Expressions.Expression<System.Func<OrderItem, bool>> predicate)
			{
				throw new NotImplementedException();
			}

			public List<OrderItem> Where(System.Linq.Expressions.Expression<System.Func<OrderItem, bool>> predicate)
			{
				throw new NotImplementedException();
			}

			public List<OrderItem> Where(System.Linq.Expressions.Expression<System.Func<OrderItem, bool>> predicate, IEnumerable<string> includes)
			{
				throw new NotImplementedException();
			}

			public List<TSelected> WhereSelect<TSelected>(System.Linq.Expressions.Expression<System.Func<OrderItem, bool>> predicate, System.Linq.Expressions.Expression<System.Func<OrderItem, TSelected>> selector)
			{
				throw new NotImplementedException();
			}

			public OrderItem FirstOrDefault(System.Linq.Expressions.Expression<System.Func<OrderItem, bool>> predicate)
			{
				throw new NotImplementedException();
			}

			public OrderItem FirstOrDefault(System.Linq.Expressions.Expression<System.Func<OrderItem, bool>> predicate, IEnumerable<string> includes)
			{
				throw new NotImplementedException();
			}

			public TSelected FirstOrDefaultSelect<TSelected>(System.Linq.Expressions.Expression<System.Func<OrderItem, bool>> predicate, System.Linq.Expressions.Expression<System.Func<OrderItem, TSelected>> selector)
			{
				throw new NotImplementedException();
			}
		}
	}
}