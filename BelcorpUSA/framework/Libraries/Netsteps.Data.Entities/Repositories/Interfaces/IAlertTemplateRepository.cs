using System.Collections.Generic;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business;

namespace NetSteps.Data.Entities.Repositories
{
    public partial interface IAlertTemplateRepository
    {
        PaginatedList<AlertTemplate> Search(AlertTemplateSearchParameters alertTemplateSearchParameters);
        List<Alert> GetAlertsForAccount(int accountID, int lanuageID);
    }
}
