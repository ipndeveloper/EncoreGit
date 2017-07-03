using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Common.Base;
using System.Data.SqlClient;
using System.Data;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;

namespace NetSteps.Data.Entities.Extensions
{
    public class WarehouseExtensions
    {
        public class WareHouse
        {
            public int WarehouseID { get; set; }
            public string Name { get; set; }
        }
        
        public static Dictionary<string, string> GetWareHouse()
        {
            return DataAccess.ExecQueryEntidadDictionary("Core", "uspListWarehouse");
        }

        public static int WareHouseByAddresID(int AccountID)
        {
            return DataAccess.ExecWithStoreProcedureSaveIdentity("Core", "uspWareHouseByAddresID",
                new SqlParameter("AccountID", SqlDbType.Int) { Value = AccountID }
                );
        } 

        public static int GetShippingOrderTypeID(int OrderCustomerID)
        {
            return DataAccess.ExecWithStoreProcedureSaveIdentity("Core", "uspGetShippingOrderTypeID",
                new SqlParameter("OrderCustomerID", SqlDbType.Int) { Value = OrderCustomerID }
                );
        }

        public static List<WareHouseMaterialSearchData> ListWareHouseInventoryReplacement(int ParentProductID, int WarehouseMaterialID)
        {
            List<WareHouseMaterialSearchData> WareHouseMaterialReplacements = DataAccess.ExecWithStoreProcedureListParam<WareHouseMaterialSearchData>("Core", "uspListWareHouseInventoryReplacement",
                 new SqlParameter("ParentProductID", SqlDbType.Int) { Value = ParentProductID},
                new SqlParameter("WarehouseMaterialID", SqlDbType.Int) { Value = WarehouseMaterialID }
                ).ToList();
            return WareHouseMaterialReplacements;

        } 
    }
}
