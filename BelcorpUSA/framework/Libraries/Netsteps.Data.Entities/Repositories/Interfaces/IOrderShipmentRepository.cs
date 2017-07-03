using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business;
namespace NetSteps.Data.Entities.Repositories
{
	[ContractClass(typeof(Contracts.OrderShipmentRepositoryContracts))]
    public partial interface IOrderShipmentRepository
    {
		OrderShipment LoadFullByShipmentID(int shipmentID);
        IPaginatedList<OrderShippingSearchData> Search(OrderShipmentSearchParameters searchParameters);
        IList<OrderShippingSearchData> GetOrderShippingSearchData(IEnumerable<int> orderIDs);
    }

	namespace Contracts
	{
		[ContractClassFor(typeof(IOrderShipmentRepository))]
		abstract class OrderShipmentRepositoryContracts : IOrderShipmentRepository
		{
			public OrderShipment LoadFullByShipmentID(int shipmentID)
			{
				throw new NotImplementedException();
			}

			public IPaginatedList<OrderShippingSearchData> Search(OrderShipmentSearchParameters searchParameters)
			{
				Contract.Requires<ArgumentNullException>(searchParameters != null);

				throw new NotImplementedException();
			}

			public IList<OrderShippingSearchData> GetOrderShippingSearchData(IEnumerable<int> orderIDs)
			{
				Contract.Requires<ArgumentNullException>(orderIDs != null);

				throw new NotImplementedException();
			}

			public NetSteps.Common.PrimaryKeyInfo PrimaryKeyInfo
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public OrderShipment Load(int primaryKey)
			{
				throw new NotImplementedException();
			}

			public OrderShipment LoadFull(int primaryKey)
			{
				throw new NotImplementedException();
			}

			public List<OrderShipment> LoadAll()
			{
				throw new NotImplementedException();
			}

			public List<OrderShipment> LoadAllFull()
			{
				throw new NotImplementedException();
			}

			public List<OrderShipment> LoadBatch(IEnumerable<int> primaryKeys)
			{
				throw new NotImplementedException();
			}

			public List<OrderShipment> LoadBatchFull(IEnumerable<int> primaryKeys)
			{
				throw new NotImplementedException();
			}

			public SqlUpdatableList<OrderShipment> LoadAllFullWithSqlDependency()
			{
				throw new NotImplementedException();
			}

			public SqlUpdatableList<OrderShipment> LoadBatchWithSqlDependency(IEnumerable<int> primaryKeys)
			{
				throw new NotImplementedException();
			}

			public void Save(OrderShipment obj)
			{
				throw new NotImplementedException();
			}

			public void SaveBatch(IEnumerable<OrderShipment> items)
			{
				throw new NotImplementedException();
			}

			public void Delete(OrderShipment obj)
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

			public PaginatedList<AuditLogRow> GetAuditLog(int primaryKey, AuditLogSearchParameters param)
			{
				throw new NotImplementedException();
			}

			public OrderShipment GetRandomRecord()
			{
				throw new NotImplementedException();
			}

			public OrderShipment GetRandomRecordFull()
			{
				throw new NotImplementedException();
			}

			public int Count()
			{
				throw new NotImplementedException();
			}

			public int Count(System.Linq.Expressions.Expression<System.Func<OrderShipment, bool>> predicate)
			{
				throw new NotImplementedException();
			}

			public bool Any()
			{
				throw new NotImplementedException();
			}

			public bool Any(System.Linq.Expressions.Expression<System.Func<OrderShipment, bool>> predicate)
			{
				throw new NotImplementedException();
			}

			public List<OrderShipment> Where(System.Linq.Expressions.Expression<System.Func<OrderShipment, bool>> predicate)
			{
				throw new NotImplementedException();
			}

			public List<OrderShipment> Where(System.Linq.Expressions.Expression<System.Func<OrderShipment, bool>> predicate, IEnumerable<string> includes)
			{
				throw new NotImplementedException();
			}

			public List<TSelected> WhereSelect<TSelected>(System.Linq.Expressions.Expression<System.Func<OrderShipment, bool>> predicate, System.Linq.Expressions.Expression<System.Func<OrderShipment, TSelected>> selector)
			{
				throw new NotImplementedException();
			}

			public OrderShipment FirstOrDefault(System.Linq.Expressions.Expression<System.Func<OrderShipment, bool>> predicate)
			{
				throw new NotImplementedException();
			}

			public OrderShipment FirstOrDefault(System.Linq.Expressions.Expression<System.Func<OrderShipment, bool>> predicate, IEnumerable<string> includes)
			{
				throw new NotImplementedException();
			}

			public TSelected FirstOrDefaultSelect<TSelected>(System.Linq.Expressions.Expression<System.Func<OrderShipment, bool>> predicate, System.Linq.Expressions.Expression<System.Func<OrderShipment, TSelected>> selector)
			{
				throw new NotImplementedException();
			}
		}
	}
}