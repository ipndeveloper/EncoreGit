using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;

namespace NetSteps.Data.Entities.Repositories.Interfaces
{
    public interface IManagerExecutionsRepository
    {
        List<SubProcessStatusSearchData> ListSubProcessStatus(int LanguageID, int status, int page, int pageSize, string column, string order);
        FailedSubProcessSearchData GetFailedSubProcess(int LanguageID, int MonthlyClosureDetailLogID);
        PeriodPlanSearchData GetPlanAndPeriod(int MonthlyClosureLogID);
        List<MainProcessesDetailSearchData> ListMainProcessesDetail(int LanguageID, int status, int page, int pageSize, string column, string order);
        FailedSubProcessPersonalIndicatorSearchData GetFailedSubProcess_PI(int LanguageID, int PersonalIndicatorDetailLogID);
        OrderOrderStatusSearchData GetOderAndOrderStatus(int PersonalIndicatorLogID);
    }
}
