using System;
using System.Collections.Generic;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business
{
    public class InventoryAllocated
    {
        //KLC - CSTI (BR-IN-004)
        public static List<WarehouseInventoryAllocatedSearchData> Search()
        {
            try
            {
                return InventoryAllocatedRepository.Search();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
    
        public static PaginatedList<WarehouseInventoryAllocatedSearchData> Search(InventoryAllocatedParameters searchParams)
        {
            try
            {
                return InventoryAllocatedRepository.Search(searchParams);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }


    }
}
