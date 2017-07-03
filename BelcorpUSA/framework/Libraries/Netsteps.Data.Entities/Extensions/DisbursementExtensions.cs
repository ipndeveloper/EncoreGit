using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using NetSteps.Common.Base;

namespace NetSteps.Data.Entities.Extensions
{
    public class DisbursementExtensions
    {

        public static Dictionary<int, string> ListPeriod()
        {
            List<DisbursementsPeriod> PeriodResult = DataAccess.ExecWithStoreProcedureLists<DisbursementsPeriod>("Commissions", "uspListDisbursementPeriod").ToList();
            Dictionary<int, string> PeriodResultDic = new Dictionary<int, string>();
            foreach (var item in PeriodResult)
            {
                PeriodResultDic.Add(item.PeriodID, Convert.ToString(item.PeriodID));
            }
            return PeriodResultDic;
        }

    }
}
