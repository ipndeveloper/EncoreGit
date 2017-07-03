using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Repositories.Interfaces;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace NetSteps.Data.Entities.Repositories
{
    public class MonthlyClosingRepository : IMonthlyClosingRepository
    {      
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> ListAvailablePlans()
        {
            try
            {
                return DataAccess.ExecQueryEntidadDictionary("Commissions", "spListAvailablePlans");//R2841 HUNDRED(JICM)
            }
            catch (Exception)
            {throw new Exception("Active Plans were not found.");}            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetActivePeriod()
        {
            string PeriodID = "";
            try
            {               
                using (var context = new EntityDBContext(ConnectionStrings.BelcorpCommission))
                {
                    var result = context.Database.SqlQuery<int>("spGetActivePeriod").First();
                    PeriodID = result.ToString();
                }
            }
            catch (Exception)
            {throw new Exception("PerioID was not found.");}
            return PeriodID;
        }

        public string GetOpenPeriod()
        {
            string PeriodID = "";
            try
            {
                using (var context = new EntityDBContext(ConnectionStrings.BelcorpCommission))
                {
                    var result = context.Database.SqlQuery<int>("spGetOpenPeriod").First();
                    PeriodID = result.ToString();
                }
            }
            catch (Exception)
            { throw new Exception("PerioID was not found."); }
            return PeriodID;
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
