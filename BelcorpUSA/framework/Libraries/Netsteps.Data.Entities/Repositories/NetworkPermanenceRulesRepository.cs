namespace NetSteps.Data.Entities.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using NetSteps.Data.Entities.Dto;
    using NetSteps.Data.Entities.Repositories.Interfaces;
    using System.Data.SqlClient;
    using System.Data;
    using NetSteps.Data.Entities.Exceptions;

    /// <summary>
    /// Implementation of the NetworkPermanenceRules Interface
    /// </summary>
    public class NetworkPermanenceRulesRepository : INetworkPermanenceRulesRepository
    {
        /// <summary>
        /// Applies Network Permanence Rules
        /// </summary>
        public void ApplyNetworkPermanenceRules()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Commissions"].ConnectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[upsCompresionYReconstruccionRed]";

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
    }
}
