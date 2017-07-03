using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Repositories.Interfaces;
using System.Data.SqlClient;
using System.Data;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Repositories
{
    public class BonusRepository : IBonusRepository
    {

        public bool CalcularBonusTeamBuilding(int PeriodID)
        {

            SqlParameter pPeriodID = null;
            pPeriodID = new SqlParameter() { ParameterName = "@PeriodID", Value = PeriodID, SqlDbType = SqlDbType.Int };
            
            try
            {
                using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Commissions"].ConnectionString))
                {
                    connection.Open();
 
                    SqlCommand cmd = new SqlCommand();
                    cmd.Parameters.Add(pPeriodID);
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[UspMainBonusTeamBuilding]";

                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return false;
        }

        public bool CalcularBonuTurboInfinityByPeriod(int PeriodID)
        {
            try
            {
                SqlParameter pPeriodID = null;
                pPeriodID = new SqlParameter() { ParameterName = "@PeriodID", Value = PeriodID, SqlDbType = SqlDbType.Int };
                using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Commissions"].ConnectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Parameters.Add(pPeriodID);
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[BonuTurboInfinityByPeriod]";

                    cmd.ExecuteNonQuery();
                    return true;
                         
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            
            }
            return false;
        }


        public bool CalcularDiscountType(int PeriodID)
        {
            try
            {
                SqlParameter pPeriodID = null;
                pPeriodID = new SqlParameter() { ParameterName = "@PeriodID", Value = PeriodID, SqlDbType = SqlDbType.Int };
                using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Parameters.Add(pPeriodID);
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[uspDiscountType3]";

                    cmd.ExecuteNonQuery();
                    return true;

                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);

            }
            return false;
        }
    }
}
