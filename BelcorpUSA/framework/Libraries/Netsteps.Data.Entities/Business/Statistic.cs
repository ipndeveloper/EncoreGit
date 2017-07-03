using System;
using System.Collections.Generic;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities
{
    public partial class Statistic
    {
        public static void SaveBatch(IEnumerable<Statistic> items)
        {
            try
            {
               BusinessLogic.SaveBatch(Repository, items);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
    }
}
