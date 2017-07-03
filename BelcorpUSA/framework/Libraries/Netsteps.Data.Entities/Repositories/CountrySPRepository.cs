namespace NetSteps.Data.Entities.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NetSteps.Data.Entities.Repositories.Interfaces;
    using System.Data.SqlClient;
    using System.Data;

    public partial class CountrySPRepository : ICountrySPRepository
    {
        public int CountryIdByName(string name)
        { 
            using (var context = new EntityDBContext(ConnectionStrings.BelcorpCommission))
            {
                object[] parameters = { new SqlParameter("@Name", SqlDbType.VarChar) { Value = name }};

                var result = context.Database.SqlQuery<int>(GenerateQueryString(CountryByNameUSP, parameters), parameters);
                
                return result.First();
            }          
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

        private const string CountryByNameUSP = "CountryByName";
    }
}
