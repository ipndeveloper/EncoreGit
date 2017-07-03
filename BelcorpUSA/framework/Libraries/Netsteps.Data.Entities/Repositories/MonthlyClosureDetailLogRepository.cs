using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Repositories.Interfaces;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using System.Data.SqlClient;
using System.Data;

namespace NetSteps.Data.Entities.Repositories
{
    public class MonthlyClosureDetailLogRepository : IMonthlyClosureDetailLogRepository
    {
        /// <summary>
        /// Saves the sub process
        /// </summary>
        /// <param name="oMonthlyClosureDetail">As input of MonthlyClosureDetailLogParameters class</param>
        /// <returns>returns the last id generated</returns>
        public int SaveSubProcess(MonthlyClosureDetailLogParameters oMonthlyClosureDetail, int LanguageID)
        {
            var result = 0;
            try
            {
                object[] parameters = { new SqlParameter("@MonthlyClosureLogID", SqlDbType.Int) { Value = oMonthlyClosureDetail.MonthlyClosureLogID },
                                      new SqlParameter("@CodeSubProcess", SqlDbType.VarChar) { Value = oMonthlyClosureDetail.CodeSubProcess },                                          
                                      new SqlParameter("@TermName", SqlDbType.VarChar) { Value = oMonthlyClosureDetail.TermName },
                                      new SqlParameter("@StarTime", SqlDbType.DateTime) { Value = oMonthlyClosureDetail.StarTime },
                                      new SqlParameter("@CodeStatusName", SqlDbType.VarChar) { Value = oMonthlyClosureDetail.CodeStatusName },
                                      new SqlParameter("@MessageToShow", SqlDbType.VarChar) { Value = oMonthlyClosureDetail.MessageToShow },
                                      new SqlParameter("@RealError", SqlDbType.VarChar) { Value = oMonthlyClosureDetail.RealError }};

                using (var DbContext = new EntityDBContext(ConnectionStrings.BelcorpCommission))
                {result = DbContext.Database.SqlQuery<int>(GenerateQueryString("spSaveSubProcess", parameters), parameters).First();}

                return result;
            }
            catch (Exception)
            {throw;}     
        }

        /// <summary>
        /// Updates the sub process
        /// </summary>
        /// <param name="oMonthlyClosureDetail">As input of MonthlyClosureDetailLogParameters class</param>
        /// <returns>returns the amount of affected rows</returns>
        public int UpdateSubProcess(MonthlyClosureDetailLogParameters oMonthlyClosureDetail, int LanguageID)
        {
            try
            {
                var AffectedRows = 0;

                object[] parameters = { new SqlParameter("@MonthlyClosureDetailLogID", SqlDbType.Int) { Value = oMonthlyClosureDetail.MonthlyClosureDetailLogID },
                                      new SqlParameter("@EndTime", SqlDbType.DateTime) { Value = oMonthlyClosureDetail.EndTime },
                                      new SqlParameter("@CodeStatusName", SqlDbType.VarChar) { Value = oMonthlyClosureDetail.CodeStatusName },
                                      new SqlParameter("@MessageToShow", SqlDbType.VarChar) { Value = oMonthlyClosureDetail.MessageToShow },
                                      new SqlParameter("@RealError", SqlDbType.VarChar) { Value = oMonthlyClosureDetail.RealError }};

                using (var DbContext = new EntityDBContext(ConnectionStrings.BelcorpCommission))
                {AffectedRows = DbContext.Database.SqlQuery<int>(GenerateQueryString("spUpdateSubProcess", parameters), parameters).First();}

                return AffectedRows;
            }
            catch (Exception)
            {throw;}  
        }

        /// <summary>
        /// Updates the sub process to Calceled Status
        /// </summary>
        /// <param name="oMonthlyClosureDetail">As input of MonthlyClosureDetailLogParameters class</param>
        /// <returns>returns the amount of affected rows</returns>
        public int UpdateStatusProcessToCanceled(MonthlyClosureDetailLogParameters oMonthlyClosureDetail)
        {
            try
            {
                var AffectedRows = 0;

                object[] parameters = { new SqlParameter("@MonthlyClosureLogID", SqlDbType.Int) { Value = oMonthlyClosureDetail.MonthlyClosureLogID },
                                      new SqlParameter("@MonthlyClosureDetailLogID", SqlDbType.Int) { Value = oMonthlyClosureDetail.MonthlyClosureDetailLogID },
                                      new SqlParameter("@CodeStatusName", SqlDbType.VarChar) { Value = oMonthlyClosureDetail.CodeStatusName }};

                using (var DbContext = new EntityDBContext(ConnectionStrings.BelcorpCommission))
                {AffectedRows = DbContext.Database.SqlQuery<int>(GenerateQueryString("spUpdateStatusProcessToCanceled", parameters), parameters).First();}

                return AffectedRows;
            }
            catch (Exception)
            {throw;}  
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="LanguageID"></param>
        /// <param name="CodeSubprocess"></param>
        /// <returns></returns>
        public string GetFailedSubprocessName(int LanguageID, string CodeSubprocess)
        {
            try
            {
                var Subprocess = "";

                object[] parameters = { new SqlParameter("@LanguageID", SqlDbType.Int) { Value = LanguageID },
                                      new SqlParameter("@TermName", SqlDbType.VarChar) { Value = CodeSubprocess}};

                using (var DbContext = new EntityDBContext(ConnectionStrings.BelcorpCommission))
                { Subprocess = DbContext.Database.SqlQuery<string>(GenerateQueryString("spGetFailedSubprocessName", parameters), parameters).First(); }

                return Subprocess;
            }
            catch (Exception)
            { throw; }
        }

        /// <summary>
        /// Add @ as pref of parameters
        /// </summary>
        /// <param name="query">Query or store procedure</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>string format Query @parameter ...</returns>
        private string GenerateQueryString(string query, params object[] parameters)
        {
            if (!query.Contains("@") && parameters != null)
            {
                var parameterNames = from p in parameters select ((System.Data.SqlClient.SqlParameter)p).ParameterName;
                query = string.Format("{0} {1}", query, string.Join(", ", parameterNames));
            }

            return query;
        }
    }
}
