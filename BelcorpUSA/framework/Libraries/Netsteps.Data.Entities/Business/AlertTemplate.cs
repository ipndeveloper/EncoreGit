using System;
using System.Collections.Generic;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities
{
    public partial class AlertTemplate
    {
        public static List<Alert> GetAlertsForAccount(int accountID, int languageID)
        {
            return BusinessLogic.GetAlertsForAccount(Repository, accountID, languageID);
        }

        public static PaginatedList<AlertTemplate> Search(AlertTemplateSearchParameters alertTemplateSearchParameters)
        {
            try
            {
                return BusinessLogic.Search(Repository, alertTemplateSearchParameters);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
    }
}
