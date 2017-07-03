using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;

namespace NetSteps.Data.Entities.Repositories
{
    public class InventoryAllocatedRepository
    {

        public static List<WarehouseInventoryAllocatedSearchData> Search()
        {
            return InventoryAllocatedRepository.SearchIn();
        }

        public static PaginatedList<WarehouseInventoryAllocatedSearchData> Search(InventoryAllocatedParameters searchParams)
        {
            // Apply filters
            var inventoryMovements = InventoryAllocatedRepository.SearchInvAllocated(searchParams);

            if (inventoryMovements == null)
                inventoryMovements = new List<WarehouseInventoryAllocatedSearchData>();

            // Apply pagination
            IQueryable<WarehouseInventoryAllocatedSearchData> matchingItems = inventoryMovements.AsQueryable<WarehouseInventoryAllocatedSearchData>();

            var resultTotalCount = matchingItems.Count();
            matchingItems = matchingItems.ApplyPagination(searchParams);

            return matchingItems.ToPaginatedList<WarehouseInventoryAllocatedSearchData>(searchParams, resultTotalCount);
        }


        #region DataAcces

        public static List<WarehouseInventoryAllocatedSearchData> SearchIn()
        {
            List<WarehouseInventoryAllocatedSearchData> result = null;
            try
            {
                SqlDataReader reader = DataAccess.GetDataReader("spSearchMovementsAllocated", null, "Core");

                if (reader.HasRows)
                {
                    result = new List<WarehouseInventoryAllocatedSearchData>();
                    while (reader.Read())
                    {
                        result.Add(new WarehouseInventoryAllocatedSearchData()
                        {
                            WarehouseID = Convert.ToInt32(reader["WarehouseID"]),
                            WarehouseName = Convert.ToString(reader["WarehouseName"]),
                            MaterialID = Convert.ToInt32(reader["MaterialID"]),
                            ProductID = Convert.ToInt32(reader["ProductID"]),
                            ProductSKU = Convert.ToString(reader["SKU"]),
                            MaterialSKU = Convert.ToString(reader["MaterialCode"]),
                            MaterialName = Convert.ToString(reader["MaterialName"]),
                            ProductName = Convert.ToString(reader["ProductName"]),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            PreOrderID = Convert.ToInt32(reader["PreOrderID"]),
                            AllocationDateUTC = Convert.ToDateTime(reader["AllocationDateUTC"])

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return result;
        }

        public static List<WarehouseInventoryAllocatedSearchData> SearchInvAllocated(InventoryAllocatedParameters searchParams)
        {
            List<WarehouseInventoryAllocatedSearchData> result = new List<WarehouseInventoryAllocatedSearchData>();
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@WarehouseID", searchParams.WarehouseID }, 
                                                                                            { "@ProductID", searchParams.ProductID },
                                                                                            { "@MaterialID", searchParams.MaterialID },                                                                                            
                                                                                            { "@StartDate", searchParams.StartDate }, 
                                                                                            { "@EndDate", searchParams.EndDate }};

                SqlDataReader reader = DataAccess.GetDataReader("spSearchMovementsAllocated", parameters, "Core");

                if (reader.HasRows)
                {
                    result = new List<WarehouseInventoryAllocatedSearchData>();
                    while (reader.Read())
                    {
                        result.Add(new WarehouseInventoryAllocatedSearchData()
                        {
                            WarehouseID = Convert.ToInt32(reader["WarehouseID"]),
                            WarehouseName = Convert.ToString(reader["WarehouseName"]),
                            ProductID = Convert.ToInt32(reader["ProductID"]),
                            ProductSKU = Convert.ToString(reader["SKU"]),
                            ProductName = Convert.ToString(reader["ProductName"]),
                            MaterialID = Convert.ToInt32(reader["MaterialID"]),                            
                            MaterialSKU = Convert.ToString(reader["MaterialCode"]),
                            MaterialName = Convert.ToString(reader["MaterialName"]),                           
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            PreOrderID = Convert.ToInt32(reader["PreOrderID"]),
                            AllocationDateUTC = Convert.ToDateTime(reader["AllocationDateUTC"])
                            
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return result;
        }

        #endregion
    }
}
