using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#region referencias
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using System.Data.SqlClient;
using System.Data;
#endregion

namespace NetSteps.Data.Entities.Extensions
{
    public class DisbursementManagementExtensions
    {
        public static string ExistsDisbursementsByPeriod(int PeriodID)
        {
            var mensaje = DataAccess.ExecWithStoreProcedureListParam<string>("Commissions", "upsExistsDisbursementsByPeriod",
                new SqlParameter("PeriodID", SqlDbType.Int) { Value = PeriodID }).ToList();
            return mensaje[0].ToString();
        }

        public static void MoveBonusValuesToLedgers(int PeriodID)
        {
            DataAccess.ExecWithStoreProcedureSave("Commissions", "uspMoveBonusValuesToLedgers",
                new SqlParameter("PeriodID", SqlDbType.Int) { Value = PeriodID });
        }

        public static string ExistsDibsCreateRecordsByPeriod(int PeriodID)
        {
            var mensaje = DataAccess.ExecWithStoreProcedureListParam<string>("Commissions", "upsExistsDibsCreateRecordsByPeriod",
                new SqlParameter("PeriodID", SqlDbType.Int) { Value = PeriodID }).ToList();
            return mensaje[0].ToString();
        }

        public static void DisbCreateRecords(int PeriodID)
        {
            DataAccess.ExecWithStoreProcedureSave("Commissions", "uspDisbCreateRecords",
                new SqlParameter("PeriodID", SqlDbType.Int) { Value = PeriodID });
        }

        public static string ExistsSendToBankByPeriod(int PeriodID)
        {
            var mensaje = DataAccess.ExecWithStoreProcedureListParam<string>("Commissions", "upsExistsSendToBankByPeriod",
                new SqlParameter("PeriodID", SqlDbType.Int) { Value = PeriodID }).ToList();
            return mensaje[0].ToString();
        }

        public static void SendToBank(int PeriodID)
        {
            DataAccess.ExecWithStoreProcedureSave("Commissions", "upsSendToBank",
                new SqlParameter("PeriodID", SqlDbType.Int) { Value = PeriodID });
        }

        public static int GetLatestClosedPeriodByPlan()
        {

            var result = DataAccess.ExecWithStoreProcedureListParam<int>("Commissions", "uspGetLatestClosedPeriodByPlan").ToList();
            return result[0];
        }

    }

}
