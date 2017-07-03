using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Business;

namespace NetSteps.Data.Entities.Repositories.Interfaces
{
    public interface IStatusProcessMonthlyClosureLogRepository
    {
        List<StatusProcessMonthlyClosureLogSearchData> ListStatuses(int LanguageID);
    }
}
