using System;
using System.Collections.Generic;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Data.Entities.Extensions;
using System.Globalization;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;

namespace NetSteps.Data.Entities
{
    public class KPIperPeriod
    {
        /// <summary>
        /// Search all Periods
        /// </summary>
        /// <returns>List<PeriodSearchData></returns>
        public static KPIsPerPeriodSearchData GetKPISbyPeriodIdandAccountId(int periodId, int accountId)
        {
            return KPIsPerPeriodDataAccess.GetKPISbyPeriodIdandAccountId(periodId, accountId);
        }
    }
}
