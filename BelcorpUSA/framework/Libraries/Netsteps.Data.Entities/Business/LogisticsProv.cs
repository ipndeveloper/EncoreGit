using System;
using System.Collections.Generic;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities
{
    public class LogisticsProv
    {
        public static List<LogisticsProviderSearData> SearchProvider()
        {
            return LogisticsProvRepository.SearchProviders1();
        }

        public static PaginatedList<LogisticsProviderSearData> SearchProvider(LogisticsProvParameters searchParams)
        {
            try
            {
                return LogisticsProvRepository.SearchProvider(searchParams);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
        public static PaginatedList<OrderLogisticProviderSearchData> SearchOrderProvider(OrderLogisticProvParameters searchParams)
        {
            try
            {
                return LogisticsProvRepository.SearchOrderProvider(searchParams);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
        //llenr lokoup
        public static System.Collections.Generic.Dictionary<int, string> GetSearchProvider(string text)
        {
            Dictionary<int, string> results;
            try
            {
                results = LogisticsProvRepository.SearchProviderByText(text);

                return results;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }

        }

        public static int ChangeLogisticProvider(OrderLogisticProvParameters searchParams)
        {
            try
            {
                return LogisticsProvRepository.upChangeLogisticProvider(searchParams);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
    }
}
