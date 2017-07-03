using System;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using NetSteps.Data.Entities.Repositories;
using System.Collections.Generic; 

namespace NetSteps.Data.Entities.Business
{
    public class CollectionEntities
    {

        public static PaginatedList<CollectionEntitySearchData> SearchDetails(CollectionEntitiesSearchParameter searchParameter)
        {
            try
            {
                return CollectionEntitiesRepository.SearchDetails(searchParameter);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);

            }
        } 
    }
}
