using System;
using System.Collections.Generic;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities
{
    public partial class AccountPropertyType
    {
        public static List<AccountPropertyType> LoadAllFullAccountPropertyTypes()
        {
            try
            {
                return BusinessLogic.LoadAllFullAccountPropertyTypes(Repository);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static PaginatedList<AccountPropertyType> Search(FilterPaginatedListParameters<AccountPropertyType> searchParams)
        {
            try
            {
                return Repository.Search(searchParams);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

    }
}
