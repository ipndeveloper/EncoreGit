using System;
using System.Collections.Generic;
using System.Text;

#region Referencias
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.EntityModels;
using NetSteps.Data.Entities.Extensions;
#endregion
//Reports Earnings
namespace NetSteps.Data.Entities.Business
{
    public class Earnings
    {
        public IEnumerable<EarningReportBasic> GetEarningReportBasics(int accountID, int period)
        {
            try
            {
                return EarningsExtensions.GetEarningReportBasics(accountID, period);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);

            }
        }

        public IEnumerable<EarningsTotal> GetEarningsTotals(int accountID, int period)
        {
            try
            {
                return EarningsExtensions.GetEarningsTotals(accountID, period);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);

            }
        }
    }
}
