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
    public class MonthlyClosureLogRepository : IMonthlyClosureLogRepository
    {
        /// <summary>
        /// Saves the main process
        /// </summary>
        /// <param name="oMonthlyClosure">As input of MonthlyClosureLogParameters class</param>
        /// <returns>returns the last id generated</returns>
        public int SaveMainProcess(MonthlyClosureLogParameters oMonthlyClosureLog, int LanguageID)
        {
            var result = 0;
            try 
	        {
                object[] parameters = { new SqlParameter("@PlanID", SqlDbType.Int) { Value = oMonthlyClosureLog.PlanID },
                                      new SqlParameter("@PeriodID", SqlDbType.Int) { Value = oMonthlyClosureLog.PeriodID },
                                      new SqlParameter("@TermName", SqlDbType.VarChar) { Value = oMonthlyClosureLog.TermName },
                                      new SqlParameter("@StarTime", SqlDbType.DateTime) { Value = oMonthlyClosureLog.StarTime },
                                      new SqlParameter("@Result", SqlDbType.VarChar) { Value = oMonthlyClosureLog.Result },
                                      new SqlParameter("@LanguageID", SqlDbType.Int) { Value = LanguageID }};

                using (var DbContext = new EntityDBContext(ConnectionStrings.BelcorpCommission))
                {result = DbContext.Database.SqlQuery<int>(GenerateQueryString("spSaveMainProcess", parameters), parameters).FirstOrDefault();}

                return result;
	        }
	        catch (Exception)
	        {throw new Exception("An error has occurred, please try again.");}            
        }

        /// <summary>
        /// Updates the main process
        /// </summary>
        /// <param name="oMonthlyClosure">As input of MonthlyClosureLogParameters class</param>
        /// <returns>returns the amount of affected rows</returns>
        public int UpdateMainProcess(MonthlyClosureLogParameters oMonthlyClosureLog, int LanguageID)
        {
            try
            {
                var AffectedRows = 0;
                object[] parameters = { new SqlParameter("@MonthlyClosureLogID", SqlDbType.Int) { Value = oMonthlyClosureLog.MonthlyClosureLogID },
                                      new SqlParameter("@EndTime", SqlDbType.DateTime) { Value = oMonthlyClosureLog.EndTime },
                                      new SqlParameter("@Result", SqlDbType.VarChar) { Value = oMonthlyClosureLog.Result },
                                      new SqlParameter("@LanguageID", SqlDbType.Int) { Value = LanguageID }};

                using (var DbContext = new EntityDBContext(ConnectionStrings.BelcorpCommission))
                {AffectedRows = DbContext.Database.SqlQuery<int>(GenerateQueryString("spUpdateMainProcess", parameters), parameters).First();}
                
                return AffectedRows;
            }
            catch (Exception)
            {throw new Exception("An error has occurred, please try again.");}  
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

        /// <summary>
        /// Saves the main process
        /// </summary>
        /// <param name="oMonthlyClosure">As input of MonthlyClosureLogParameters class</param>
        /// <returns>returns the last id generated</returns>
        public int ExecMonthlyClosing(MonthlyClosureLogParameters oMonthlyClosureLog)
        {
            var result = 0;
            try
            {
                object[] parameters = { new SqlParameter("@PlanID", SqlDbType.Int) { Value = oMonthlyClosureLog.PlanID },
                                      new SqlParameter("@PeriodID", SqlDbType.Int) { Value = oMonthlyClosureLog.PeriodID }};

                using (var DbContext = new EntityDBContext(ConnectionStrings.BelcorpCommission))
                { result = DbContext.Database.SqlQuery<int>(GenerateQueryString("uspMonthlyClosing", parameters), parameters).FirstOrDefault(); }

                return result;
            }
            catch (Exception)
            { throw new Exception("An error has occurred, please try again."); }
        }

        /// <summary>
        /// Saves the main process
        /// </summary>
        /// <param name="oMonthlyClosure">As input of MonthlyClosureLogParameters class</param>
        /// <returns>returns the last id generated</returns>
        public int InitializePrepareNextCampaign(MonthlyClosureLogParameters oMonthlyClosureLog)
        {
            var result = 0;
            try
            {
                object[] parameters = { new SqlParameter("@PeriodID", SqlDbType.Int) { Value = oMonthlyClosureLog.PeriodID }};

                using (var DbContext = new EntityDBContext(ConnectionStrings.BelcorpCommission))
                { result = DbContext.Database.SqlQuery<int>(GenerateQueryString("UspPrepareNextCampaign", parameters), parameters).FirstOrDefault(); }

                return result;
            }
            catch (Exception)
            { throw new Exception("An error has occurred, please try again."); }
        }
    }
}
