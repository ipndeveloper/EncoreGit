using System;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities
{
    public partial class OrderItemPropertyType
    {
        public static PaginatedList<OrderItemPropertyType> Search(FilterPaginatedListParameters<OrderItemPropertyType> searchParams)
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

        public static OrderItemPropertyType GetByName(string name)
        {
            try
            {
                return Repository.GetByName(name);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static int GetIDByName(string name)
        {
            try
            {
                return Repository.GetByName(name).OrderItemPropertyTypeID;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
    }
}
