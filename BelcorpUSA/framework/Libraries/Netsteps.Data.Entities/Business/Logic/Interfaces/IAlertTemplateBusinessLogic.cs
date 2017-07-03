using System.Collections.Generic;
using NetSteps.Common.Base;

namespace NetSteps.Data.Entities.Business.Logic.Interfaces
{
    public partial interface IAlertTemplateBusinessLogic
    {
        List<Alert> GetAlertsForAccount(Repositories.IAlertTemplateRepository repository, int accountID, int languageID);
        PaginatedList<AlertTemplate> Search(Repositories.IAlertTemplateRepository repository, AlertTemplateSearchParameters alertTemplateSearchParameters);
    }
}
