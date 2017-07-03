using System;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Repositories;
using System.Collections.Generic;
using NetSteps.Common.Base;
using System.Data;
using System.Linq;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using System.Data.SqlClient;
using NetSteps.Data.Entities.Extensions;

namespace NetSteps.Data.Entities
{
    public partial class Warehouse
    {
        #region Methods
        public static Warehouse FindNearestByAddress(Address address)
        {
            try
            {
                using (NetStepsEntities context = new NetStepsEntities())
                {

                    return WarehouseProductRepository.GetWarehouse(context, address);
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
        #endregion


        #region Methods CSTI. -FHP 

         
        public static Dictionary<string, string> GetWareHouse()
        {
            return WarehouseExtensions.GetWareHouse();             
        }  

        #endregion



        //csti-mescobar-25-02-2016-inicio
        public static List<WareHouseMaterialSearchData> ListWareHouseInventoryReplacement(int ParentProductID, int WarehouseMaterialID)
        {
            return WarehouseExtensions.ListWareHouseInventoryReplacement(ParentProductID, WarehouseMaterialID);
        }
        //csti-mescobar-25-02-2016-fin
    }
}
