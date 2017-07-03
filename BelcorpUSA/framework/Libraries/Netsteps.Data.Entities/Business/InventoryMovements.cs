using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Data.Entities.Exceptions;
using System;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;

namespace NetSteps.Data.Entities.Business
{
    /// <summary>
    /// Warehouse Inventory Movements Business Logic
    /// </summary>
    public class InventoryMovements
    {
        /// <summary>
        /// Search All
        /// </summary>
        /// <returns></returns>
        public static List<WarehouseInventoryMovementsSearchData> Search()
        {
            try
            {
                return InventoryMovementsRepository.Search();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        /// <summary>
        /// Search Warehouse Inventory Movements with filter & pagination parameters
        /// </summary>
        /// <param name="searchParams"></param>
        /// <returns></returns>
        public static PaginatedList<WarehouseInventoryMovementsSearchData> Search(InventoryMovementsSearchParameters searchParams)
        {
            try
            {
                return InventoryMovementsRepository.Search(searchParams);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        /// <summary>
        /// Search Invetory Movement Types without parameters
        /// </summary>
        /// <returns></returns>
        public static List<InventoryMovementTypeSearchData> SearchInventoryMovementTypes()
        {
            try
            {
                return InventoryMovementsRepository.SearchInventoryMovementTypes();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static Dictionary<string, string> DictionatySearchInventoryMovementTypes()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            foreach (var item in InventoryMovementsRepository.SearchInventoryMovementTypes())
            {
                dictionary.Add(item.InventoryMovementTypeID.ToString(), item.Name);
            }

            return dictionary;
        }
    }
}
