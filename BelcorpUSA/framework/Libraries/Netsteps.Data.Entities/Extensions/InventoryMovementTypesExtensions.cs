using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using System.Data.SqlClient;
using System.Data;

namespace NetSteps.Data.Entities.Extensions
{
    public class InventoryMovementTypesExtensions
    {

        public static List<InventoryMovementTypeSearchData> ListInventoryMovementTypes()
        {
            List<InventoryMovementTypeSearchData> WareHouseMaterialResult = DataAccess.ExecWithStoreProcedureLists<InventoryMovementTypeSearchData>("Core", "uspListInventoryMovementTypes").ToList();
            return WareHouseMaterialResult;
        } 

        public static Dictionary<string, string> ListInventoryMovementTypesDictionary()
        {
            List<InventoryMovementTypeSearchData> InventoryMovementTypesResult = DataAccess.ExecWithStoreProcedureLists<InventoryMovementTypeSearchData>("Core", "uspListInventoryMovementTypes").ToList();
            Dictionary<string, string> wareHouseResultDic = new Dictionary<string, string>();
            wareHouseResultDic.Add("Select a Type", "0");
            foreach (var item in InventoryMovementTypesResult)
            {
                wareHouseResultDic.Add(item.Name, Convert.ToString(item.InventoryMovementTypeID));
            }
            return wareHouseResultDic;
        }


        //Retonara un entero ListInventoryMovementTypesXID
        public static int ListInventoryMovementTypesByID(int inventoryMovementTypeID)
        {
            return DataAccess.ExecWithStoreProcedureSaveIdentity("Core", "InventoryMovementTypesXID",
                    new SqlParameter("InventoryMovementTypeID", SqlDbType.Int) { Value = inventoryMovementTypeID }); 
        }
    }
}
