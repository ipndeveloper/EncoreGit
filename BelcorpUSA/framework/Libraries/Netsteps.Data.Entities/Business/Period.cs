using System;
using System.Collections.Generic;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;

namespace NetSteps.Data.Entities.Commissions
{
    public partial class Period
    {
        public static bool IsEditable(int periodID)
        {
            bool result = false;

            if (periodID == 0)
            {
                result = true;
            }
            else
            {
                result = !(periodID < SmallCollectionCache.Instance.Periods.GetCurrentPeriod().PeriodID);
            }

            return result;
        }

        public static List<int> GetPeriodIds(int accountID)
        {
            try
            {
                return Repository.GetPeriodIds(accountID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public bool IsClosed()
        {
            return StartDate <= DateTime.Now.ApplicationNow() && ClosedDate <= DateTime.Now.ApplicationNow() && ClosedDate != DateTime.MinValue;
        }

        public bool IsOpen()
        {
            return StartDate <= DateTime.Now.ApplicationNow() && (ClosedDate == null || ClosedDate == DateTime.MinValue || ClosedDate > DateTime.Now.ApplicationNow());
        }
    }
}
