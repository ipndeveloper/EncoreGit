using System;
using System.Collections.Generic;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Business.Logic
{
    public partial class AlertTemplateBusinessLogic
    {
        public virtual List<Alert> GetAlertsForAccount(Repositories.IAlertTemplateRepository repository, int accountID, int languageID)
        {
            return repository.GetAlertsForAccount(accountID, languageID);
        }

        public virtual PaginatedList<AlertTemplate> Search(Repositories.IAlertTemplateRepository repository,
                                   AlertTemplateSearchParameters alertTemplateSearchParameters)
        {
            try
            {
                return repository.Search(alertTemplateSearchParameters);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
    }
}
