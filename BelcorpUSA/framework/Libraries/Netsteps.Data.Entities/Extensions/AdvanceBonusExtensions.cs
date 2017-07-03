using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using NetSteps.Data.Entities.Business;

namespace NetSteps.Data.Entities.Extensions
{
    public class AdvanceBonusExtensions
    {
        /**********************************************************************************************************************
        Requirement: BR-BO-002 - Bono de avance
        Developed by KLC - CSTI
        **********************************************************************************************************************/
        public static void InsAdvanceBonus(int PeriodID)
        {
            var result = DataAccess.ExecWithStoreProcedureSave("Core", "uspBonusAdvancement2",
                new SqlParameter("PeriodID", SqlDbType.Int) { Value = PeriodID }                
                );
            InsUpdateTotalCommissions(PeriodID);
        }
        public static void InsUpdateTotalCommissions(int PeriodID)
        {
            var result = DataAccess.ExecWithStoreProcedureSave("Core", "uspBonusLoadValues",
                new SqlParameter("PeriodID", SqlDbType.Int) { Value = PeriodID }
                );
        }
    }
}
