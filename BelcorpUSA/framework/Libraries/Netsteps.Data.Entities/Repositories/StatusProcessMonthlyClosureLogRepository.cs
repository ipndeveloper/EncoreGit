using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Repositories.Interfaces;
using NetSteps.Data.Entities.Business;
using System.Data.SqlClient;
using System.Data;

namespace NetSteps.Data.Entities.Repositories
{
    public class StatusProcessMonthlyClosureLogRepository : IStatusProcessMonthlyClosureLogRepository
    {
        /// <summary>
        /// List the Statuses
        /// </summary>
        /// <param name="LanguageID">Current User's languageID </param>
        /// <returns>Returns a generic list of StatusProcessMonthlyClosureSearchData class</returns>
        public List<StatusProcessMonthlyClosureLogSearchData> ListStatuses(int LanguageID)
        {
            var oList = new List<StatusProcessMonthlyClosureLogSearchData>();
            try
            {
                object[] parameters = { new SqlParameter("@LanguageID", SqlDbType.Int) { Value = LanguageID } };

                using (var DbContext = new EntityDBContext(ConnectionStrings.BelcorpCommission))
                {
                    oList = DbContext.Database.SqlQuery<StatusProcessMonthlyClosureLogSearchData>
                        (GenerateQueryString("spListStatusProcessMonthlyClosure", parameters), parameters).ToList();
                }
            }
            catch (Exception)
            {
                return oList;
            }
            return oList;
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
