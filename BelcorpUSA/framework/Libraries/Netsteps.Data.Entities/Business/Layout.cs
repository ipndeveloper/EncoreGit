using System;
using System.Collections.Generic;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities
{
    public partial class Layout
    {
        public static List<int> GetLayoutsForSite(int siteID)
        {
            try
            {
                return Repository.GetLayoutsForSite(siteID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
    }
}
