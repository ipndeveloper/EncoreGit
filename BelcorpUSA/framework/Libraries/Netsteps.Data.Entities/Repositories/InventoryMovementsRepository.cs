using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;

namespace NetSteps.Data.Entities.Repositories
{
    public class InventoryMovementsRepository
    {
        public static List<WarehouseInventoryMovementsSearchData> Search()
        {
            return InventoryMovementsDataAccess.Search();
        }

        public static PaginatedList<WarehouseInventoryMovementsSearchData> Search(InventoryMovementsSearchParameters searchParams)
        {
            // Apply filters
            var inventoryMovements = InventoryMovementsDataAccess.Search(searchParams);

            if (inventoryMovements == null)
                inventoryMovements = new List<WarehouseInventoryMovementsSearchData>();

            // Apply pagination
            IQueryable<WarehouseInventoryMovementsSearchData> matchingItems = inventoryMovements.AsQueryable<WarehouseInventoryMovementsSearchData>();

            var resultTotalCount = matchingItems.Count();
            matchingItems = matchingItems.ApplyPagination(searchParams);

            return matchingItems.ToPaginatedList<WarehouseInventoryMovementsSearchData>(searchParams, resultTotalCount);
        }

        public static List<InventoryMovementTypeSearchData> SearchInventoryMovementTypes()
        {
            return InventoryMovementsDataAccess.SearchInventoryMovementTypes();
        }
    }

    #region DataAccess

    public class InventoryMovementsDataAccess
    {

        public static List<WarehouseInventoryMovementsSearchData> Search()
        {
            List<WarehouseInventoryMovementsSearchData> result = null;
            try
            {
                SqlDataReader reader = DataAccess.GetDataReader("upsGetInventoryMovements", null, "Core");

                if (reader.HasRows)
                {
                    result = new List<WarehouseInventoryMovementsSearchData>();
                    while (reader.Read())
                    {
                        result.Add(new WarehouseInventoryMovementsSearchData()
                        {
                            MaterialID = Convert.ToInt32(reader["MaterialID"]),
                            MaterialSKU = Convert.ToString(reader["MaterialSKU"]),
                            MaterialName = Convert.ToString(reader["MaterialName"]),
                            MovementDateUTC = Convert.ToDateTime(reader["MovementDateUTC"]),
                            WarehouseID = Convert.ToInt32(reader["WarehouseID"]),
                            WarehouseName = Convert.ToString(reader["WarehouseName"]),
                            InventoryMovementTypeID = Convert.ToInt32(reader["InventoryMovementTypeID"]),
                            InventoryMovementTypeName = Convert.ToString(reader["InventoryMovementTypeName"]),
                            QuantityOnHandBefore = Convert.ToInt32(reader["QuantityOnHandBefore"]),
                            QuantityMov = Convert.ToInt32(reader["QuantityMov"]),
                            QuantityOnHandAfter = Convert.ToInt32(reader["QuantityOnHandAfter"]),
                            AverageCost = Convert.ToDouble(reader["AverageCost"]),
                            OrderID = Convert.ToInt32(reader["OrderID"]),
                            OrderNumber = Convert.ToString(reader["OrderNumber"]),
                            Description = Convert.ToString(reader["Description"]),
                            UserID = Convert.ToInt32(reader["UserID"]),
                            UserName = Convert.ToString(reader["UserName"])
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

        public static List<InventoryMovementTypeSearchData> SearchInventoryMovementTypes()
        {
            List<InventoryMovementTypeSearchData> result = null;
            try
            {
                SqlDataReader reader = DataAccess.GetDataReader("upsGetInventoryMovementTypes", null, "Core");

                if (reader.HasRows)
                {
                    result = new List<InventoryMovementTypeSearchData>();
                    while (reader.Read())
                    {
                        result.Add(new InventoryMovementTypeSearchData()
                        {
                            InventoryMovementTypeID = Convert.ToInt32(reader["InventoiryMovementTypeID"]),
                            Name = Convert.ToString(reader["Name"]),
                            TermName = Convert.ToString(reader["TermName"]),
                            Active = Convert.ToBoolean(reader["Active"]),
                            PositiveMovement = Convert.ToBoolean(reader["PositiveMovement"])
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

        public static List<WarehouseInventoryMovementsSearchData> Search(InventoryMovementsSearchParameters searchParams)
        {
            List<WarehouseInventoryMovementsSearchData> result = null;
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@WarehouseID", searchParams.WarehouseID }, 
                                                                                            { "@ProductID", searchParams.ProductID },
                                                                                            { "@MaterialID", searchParams.MaterialID },
                                                                                            { "@MovementTypeID", searchParams.InventoryMovementTypeID },
                                                                                            { "@StartDate", searchParams.StartDate }, 
                                                                                            { "@EndDate", searchParams.EndDate }};

                SqlDataReader reader = DataAccess.GetDataReader("upsGetInventoryMovements", parameters, "Core");

                if (reader.HasRows)
                {
                    result = new List<WarehouseInventoryMovementsSearchData>();
                    while (reader.Read())
                    {
                        result.Add(new WarehouseInventoryMovementsSearchData()
                        {
                            MaterialID = Convert.ToInt32(reader["MaterialID"]),
                            MaterialSKU = Convert.ToString(reader["MaterialSKU"]),
                            MaterialName = Convert.ToString(reader["MaterialName"]),
                            MovementDateUTC = Convert.ToDateTime(reader["MovementDateUTC"]),
                            WarehouseID = Convert.ToInt32(reader["WarehouseID"]),
                            WarehouseName = Convert.ToString(reader["WarehouseName"]),
                            InventoryMovementTypeID = Convert.ToInt32(reader["InventoryMovementTypeID"]),
                            InventoryMovementTypeName = Convert.ToString(reader["InventoryMovementTypeName"]),
                            QuantityOnHandBefore = Convert.ToInt32(reader["QuantityOnHandBefore"]),
                            QuantityMov = Convert.ToInt32(reader["QuantityMov"]),
                            QuantityOnHandAfter = Convert.ToInt32(reader["QuantityOnHandAfter"]),
                            AverageCost = Convert.ToDouble(reader["AverageCost"]),
                            OrderID = Convert.ToInt32(reader["OrderID"]),
                            OrderNumber = Convert.ToString(reader["OrderNumber"]),
                            Description = Convert.ToString(reader["Description"]),
                            UserID = Convert.ToInt32(reader["UserID"]),
                            UserName = Convert.ToString(reader["UserName"])
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
    }

    #endregion
}
