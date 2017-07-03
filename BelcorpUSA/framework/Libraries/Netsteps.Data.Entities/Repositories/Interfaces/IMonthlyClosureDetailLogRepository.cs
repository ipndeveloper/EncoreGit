using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;

namespace NetSteps.Data.Entities.Repositories.Interfaces
{
    public interface IMonthlyClosureDetailLogRepository
    {
        int SaveSubProcess(MonthlyClosureDetailLogParameters oMonthlyClosureDetail, int LanguageID);
        int UpdateSubProcess(MonthlyClosureDetailLogParameters oMonthlyClosureDetail, int LanguageID);
        int UpdateStatusProcessToCanceled(MonthlyClosureDetailLogParameters oMonthlyClosureDetail);
        string GetFailedSubprocessName(int LanguageID, string CodeSubprocess);
    }
}
