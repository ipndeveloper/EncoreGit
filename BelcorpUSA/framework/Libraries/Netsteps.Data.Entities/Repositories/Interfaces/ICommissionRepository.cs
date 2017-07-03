using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;

namespace NetSteps.Data.Entities.Repositories.Interfaces
{
    public interface ICommissionRepository
    {

        Dictionary<int, string> GetPeriods();

        Dictionary<string, string> GetCommissionTypes();

        List<CommissionSearchData> GetTotalCommissions(CommissionSearchParameters searchParameters);

        List<CommissionDetailSearchData> GetDetailCommissions(CommissionDetailSearchParameters searchParameters);

    }
}
