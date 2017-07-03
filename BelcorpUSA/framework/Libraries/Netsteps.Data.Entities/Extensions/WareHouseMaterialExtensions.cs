using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using NetSteps.Common.Base;

namespace NetSteps.Data.Entities.Extensions
{
    public class WareHouseMaterialExtensions
    { 
        //Por Definir
        public static List<WareHouseMaterialControls> ListWareHouseMaterial(WareHouseMaterialSearchParameters Parameters)
        {
            List<WareHouseMaterialControls> WareHouseMaterialResult = DataAccess.ExecWithStoreProcedureListParam<WareHouseMaterialControls>("Core", "uspListWareHouseMaterials",
                new SqlParameter("WareHouseMaterialID", SqlDbType.Int) { Value = Parameters.WareHouseMaterialID }
                ).ToList();
            return WareHouseMaterialResult;
        }

        public static List<WareHouseMaterialDetails> ListWareHouseMaterialByID(int wareHouseMaterialID)
        {
            return DataAccess.ExecWithStoreProcedureListParam<WareHouseMaterialDetails>("Core", "uspListWareHouseMaterials",
                new SqlParameter("WareHouseMaterialID", SqlDbType.Int) { Value = wareHouseMaterialID }
                ).ToList();
            
        }

        public static PaginatedList<WareHouseMaterialSearchData> Search(WareHouseMaterialSearchParameters searchParameter, bool GetAll = false)
        {
            object RowsCount;
            List<WareHouseMaterialSearchData> paginatedResult = DataAccess.ExecWithStoreProcedureListParam<WareHouseMaterialSearchData>("Core", "uspListWareHouseInventory",out RowsCount,
                new SqlParameter("GetAll", SqlDbType.Bit) { Value = (object)GetAll ?? DBNull.Value },
                new SqlParameter("WareHouseID", SqlDbType.Int) { Value = searchParameter.WareHouseID },
                new SqlParameter("ProductID", SqlDbType.Int) { Value = (object)searchParameter.ProductID ?? DBNull.Value },
                new SqlParameter("MaterialID", SqlDbType.Int) { Value = (object)searchParameter.MaterialID ?? DBNull.Value},
                new SqlParameter("PageSize", SqlDbType.Int) { Value = searchParameter.PageSize },
                new SqlParameter("PageNumber", SqlDbType.Int) { Value = searchParameter.PageIndex },
                new SqlParameter("Colum", SqlDbType.VarChar) { Value = searchParameter.OrderBy },
                new SqlParameter("Order", SqlDbType.VarChar) { Value = searchParameter.Order },
                new SqlParameter("RowsCount", SqlDbType.Int) { Value = 0, Direction=ParameterDirection.Output }
                ).ToList();

            IQueryable<WareHouseMaterialSearchData> matchingItems = paginatedResult.AsQueryable<WareHouseMaterialSearchData>();

            var resultTotalCount = (int)RowsCount;// matchingItems.Count();
            //ya no pues el SP pagina
            //matchingItems = matchingItems.ApplyPagination(searchParameter);

            return matchingItems.ToPaginatedList<WareHouseMaterialSearchData>(searchParameter, resultTotalCount);
        }

        public static List<WareHouseMaterialSearchData> ListWareHouseMaterialsByID(int MaterialID, int WarehouseID)
        {
            List<WareHouseMaterialSearchData> WareHouseMaterialResult = DataAccess.ExecWithStoreProcedureListParam<WareHouseMaterialSearchData>("Core", "listWareHouseMaterialsXID",
                 new SqlParameter("MaterialID", SqlDbType.Int) { Value = MaterialID },
                 new SqlParameter("WarehouseID", SqlDbType.Int) { Value = WarehouseID }).ToList(); 
            return WareHouseMaterialResult;
        }

        //UpdWareHouseMaterialXTypes
        public static void UpdWareHouseMaterialByTypes(WareHouseMaterialSearchParameters parameter)
        {
            var result = DataAccess.ExecWithStoreProcedureSave("Core", "UpdWareHouseMaterialXTypes",
                new SqlParameter("Eval", SqlDbType.Int) { Value = parameter.Eval },
                new SqlParameter("QuantityField", SqlDbType.Int) { Value = parameter.QuantityField },
                new SqlParameter("WareHouseMaterialID", SqlDbType.Int) { Value = parameter.WareHouseMaterialID }
                );
        }

        public static void InsWarehouseMaterialLogs(WareHouseMaterialSearchParameters parameter)
        {
            var result = DataAccess.ExecWithStoreProcedureSave("Core", "upsInsWarehouseMaterialLogs",
                new SqlParameter("QuantityBefore", SqlDbType.Int) { Value = parameter.QuantityBefore },
                new SqlParameter("InventoryMovementTypeID", SqlDbType.Int) { Value = parameter.InventiryMovementTypeID },
                new SqlParameter("QuantityField", SqlDbType.Int) { Value = parameter.QuantityField },
                new SqlParameter("QuantityOnHandAfter", SqlDbType.Int) { Value = parameter.QuantityOnHandAfter },
                new SqlParameter("AverageCost", SqlDbType.Int) { Value = parameter.AverageCost },
                new SqlParameter("Description", SqlDbType.VarChar) { Value = parameter.Description },
                new SqlParameter("UserID", SqlDbType.Int) { Value = parameter.UserID },
                new SqlParameter("WarehouseMaterialID", SqlDbType.Int) { Value = parameter.WareHouseMaterialID }
                );
        }
    }
}
