using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.EntityModels;
using System.Data;
using System.Data.SqlClient;
using System;
using NetSteps.Common.Exceptions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class OrderItemReturnRepository
	{
		#region Members
		#endregion

		#region Methods
		public OrderItemReturn LoadByOrderItemID(int orderItemID)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					var orderItemReturn = (from c in context.OrderItemReturns
										   where c.OrderItemID == orderItemID
										   select c).FirstOrDefault();
					return orderItemReturn;
				}
			});
		}

		public List<OrderItemReturn> LoadAllByOrderItemID(int orderItemID)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					var orderItemReturns = (from c in context.OrderItemReturns
											where c.OrderItemID == orderItemID
											select c).ToList();
					return orderItemReturns;
				}
			});
		}

        #region Devolucion
        #region GetQuantityItemsValidReturnByParentOrderID
        public List<OrderReturnTable> GetQuantityItemsValidReturnByParentOrderID(int ParentOrderID)
        {
            IDbCommand dbCommand = null;
            try
            {
                var collection = new List<OrderReturnTable>();
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ParentOrderID", ParentOrderID } };
                SqlDataReader reader = DataAccess.GetDataReader("uspGetQuantityItemsValidReturnByParentOrderID", parameters, "Core");
                while (reader.Read())
                {
                    OrderReturnTable entidad = new OrderReturnTable();
                    entidad.OrderItemID = Convert.ToInt32(reader["OrderItemID"]); 
                    entidad.SKU = reader["SKU"].ToString();
                    entidad.ProductID = Convert.ToInt32(reader["ProductID"]);
                    entidad.QuantityReturn = Convert.ToInt32(reader["QuantityReturn"]);
                    entidad.BlockHead = Convert.ToBoolean(reader["BlockHead"]);
                    collection.Add(entidad);
                }
                return collection;
            }
            catch (Exception ex)
            {
                throw new LoadDataException("Unable to load uspGetQuantityItemsValidReturnByParentOrderID", ex);
            }
            finally
            {
                DataAccess.Close(dbCommand);
            }
        }
        #endregion

        #region GetQuantityReturnItemsByParentOrderID
        public List<OrderReturnDetailsTable> GetQuantityReturnItemsByParentOrderID(int ParentOrderID)
        {
            IDbCommand dbCommand = null;
            try
            {
                var collection = new List<OrderReturnDetailsTable>();
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ParentOrderID", ParentOrderID } };
                SqlDataReader reader = DataAccess.GetDataReader("uspGetQuantityReturnItemsByParentOrderID", parameters, "Core");
                while (reader.Read())
                {
                    OrderReturnDetailsTable entidad = new OrderReturnDetailsTable();
                    entidad.OrderItemID = Convert.ToInt32(reader["OrderItemID"]);
                    entidad.OrderStatusID = Convert.ToInt32(reader["OrderStatusID"]);
                    entidad.SKU = reader["SKU"].ToString();
                    entidad.ProductID = Convert.ToInt32(reader["ProductID"]);
                    entidad.QuantityReturn = Convert.ToInt32(reader["QuantityReturn"]);
                    collection.Add(entidad);
                }
                return collection;
            }
            catch (Exception ex)
            {
                throw new LoadDataException("Unable to load GetQuantityReturnItemsByParentOrderID", ex);
            }
            finally
            {
                DataAccess.Close(dbCommand);
            }
        }
        #endregion

        #region GetOrderItemChildrenReturnByOrderID
        public List<OrderReturnChildrenTable> GetOrderItemChildrenReturnByOrderID(int ParentOrderID)
        {
            IDbCommand dbCommand = null;
            try
            {
                var collection = new List<OrderReturnChildrenTable>();
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ParentOrderID", ParentOrderID } };
                SqlDataReader reader = DataAccess.GetDataReader("uspGetOrderItemChildrenReturnByOrderID", parameters, "Core");
                while (reader.Read())
                {
                    OrderReturnChildrenTable entidad = new OrderReturnChildrenTable();
                    entidad.OrderItemID = Convert.ToInt32(reader["OrderItemID"]);
                    entidad.MaterialSKU = reader["MaterialSKU"].ToString();
                    entidad.MaterialID = Convert.ToInt32(reader["MaterialID"]);
                    entidad.QuantityReturn = Convert.ToInt32(reader["QuantityReturn"]);
                    collection.Add(entidad);
                }
                return collection;
            }
            catch (Exception ex)
            {
                throw new LoadDataException("Unable to load GetOrderItemChildrenReturnByOrderID", ex);
            }
            finally
            {
                DataAccess.Close(dbCommand);
            }
        }
        #endregion

        #region GetQuantityItemsByOrderID
        public List<OrderReturnDetailsTable> GetQuantityItemsByOrderID(int OrderID)
        {
            IDbCommand dbCommand = null;
            try
            {
                var collection = new List<OrderReturnDetailsTable>();
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@OrderID", OrderID } };
                SqlDataReader reader = DataAccess.GetDataReader("uspGetQuantityItemsByOrderID", parameters, "Core");
                while (reader.Read())
                {
                    OrderReturnDetailsTable entidad = new OrderReturnDetailsTable();
                    entidad.OrderItemID = Convert.ToInt32(reader["OrderItemID"]);
                    entidad.ProductID = Convert.ToInt32(reader["ProductID"]);
                    entidad.SKU = reader["SKU"].ToString();
                    entidad.Quantity = Convert.ToInt32(reader["Quantity"]);
                    collection.Add(entidad);
                }
                return collection;
            }
            catch (Exception ex)
            {
                throw new LoadDataException("Unable to load uspGetQuantityItemsByOrderID", ex);
            }
            finally
            {
                DataAccess.Close(dbCommand);
            }
        }
        #endregion
        
        #endregion

        #endregion
    }
}