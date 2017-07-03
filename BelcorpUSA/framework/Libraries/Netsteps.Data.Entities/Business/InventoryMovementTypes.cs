using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Extensions;

namespace NetSteps.Data.Entities.Business
{
    public class InventoryMovementTypes
    {


        public static List<InventoryMovementTypeSearchData> ListInventoryMovementTypes()
        {
            return InventoryMovementTypesExtensions.ListInventoryMovementTypes();
        }


        public static Dictionary<string, string> ListInventoryMovementTypesDictionary()
        {
            return InventoryMovementTypesExtensions.ListInventoryMovementTypesDictionary();
        }

        public static int ListInventoryMovementTypesByID(int inventoryMovementTypeID)
        {
            return InventoryMovementTypesExtensions.ListInventoryMovementTypesByID(inventoryMovementTypeID);
        }
    }
}
