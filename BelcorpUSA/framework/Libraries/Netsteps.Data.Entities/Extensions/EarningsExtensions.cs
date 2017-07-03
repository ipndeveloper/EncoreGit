using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SqlClient;
using System.Data;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.EntityModels;


namespace NetSteps.Data.Entities.Extensions
{
    public class EarningsExtensions
    {
        public static IEnumerable<EarningReportBasic> GetEarningReportBasics(int accountID, int periodo)
        {

            return DataAccess.ExecWithStoreProcedureListParam<EarningReportBasic>("Commissions", "GetEarningReportBasics",
                new SqlParameter("Period", SqlDbType.Int) { Value = periodo },
                new SqlParameter("AccountID", SqlDbType.Int) { Value = accountID }).ToList();
        }

        public static IEnumerable<EarningsTotal> GetEarningsTotals(int accountID, int periodo)
        {

            return DataAccess.ExecWithStoreProcedureListParam<EarningsTotal>("Commissions", "GetEarningsTotals",
                new SqlParameter("Period", SqlDbType.Int) { Value = periodo },
                new SqlParameter("AccountID", SqlDbType.Int) { Value = accountID }).ToList();
        }
    }
}
