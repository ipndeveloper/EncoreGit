using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;

namespace NetSteps.Data.Entities.Repositories.Interfaces
{
    public interface IMonthlyClosureLogRepository
    {
        int SaveMainProcess(MonthlyClosureLogParameters oMonthlyClosureLog, int LanguageID);
        int UpdateMainProcess(MonthlyClosureLogParameters oMonthlyClosureLog, int LanguageID);
        int ExecMonthlyClosing(MonthlyClosureLogParameters oMonthlyClosureLog);
        int InitializePrepareNextCampaign(MonthlyClosureLogParameters oMonthlyClosureLog);
    }
}
