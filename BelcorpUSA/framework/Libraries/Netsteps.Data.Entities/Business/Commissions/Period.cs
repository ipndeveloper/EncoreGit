using System;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Commissions
{
    public partial class Period
    {
        public static Period GetLastClosedPeriod(int planID)
        {
            try
            {
                return Repository.GetLastClosedPeriod(planID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
    }
}
