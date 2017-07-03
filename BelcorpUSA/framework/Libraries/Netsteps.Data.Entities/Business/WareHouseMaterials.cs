using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Business
{
    public class WareHouseMaterials
    { 
        public static List<WareHouseMaterialControls> ListWareHouseMaterial(WareHouseMaterialSearchParameters parameters)
        {
            try
            {
                return WareHouseMaterialExtensions.ListWareHouseMaterial(parameters);
            }
            catch (Exception ex)
            {
                 throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }
        }

        public static PaginatedList<WareHouseMaterialSearchData> Search(WareHouseMaterialSearchParameters parameters, bool GetAll = false)
        {
            try
            {
                return WareHouseMaterialExtensions.Search(parameters, GetAll);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);

            }
        }

        public static List<WareHouseMaterialSearchData> ListWareHouseMaterialsByID(int MaterialID, int WarehouseID)
        {
            try
            {
                return WareHouseMaterialExtensions.ListWareHouseMaterialsByID(MaterialID, WarehouseID);
            }
            catch (Exception ex)
            {

                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }
        }

        public static List<WareHouseMaterialDetails> ListWareHouseMaterialByID(int wareHouseMaterialID)
        {
            try
            {
                return WareHouseMaterialExtensions.ListWareHouseMaterialByID(wareHouseMaterialID);
            }
            catch (Exception ex)
            {

                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }

        }
        public static void UpdWareHouseMaterialByTypes(WareHouseMaterialSearchParameters parameter)
        {
            try
            {
                  WareHouseMaterialExtensions.UpdWareHouseMaterialByTypes(parameter);
            }
            catch (Exception ex)
            {
                
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }
        }

        public static void InsWarehouseMaterialLogs(WareHouseMaterialSearchParameters parameter)
        {
            try
            {
                WareHouseMaterialExtensions.InsWarehouseMaterialLogs(parameter);
            }
            catch (Exception ex)
            {
               
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }
        }

    }
}
