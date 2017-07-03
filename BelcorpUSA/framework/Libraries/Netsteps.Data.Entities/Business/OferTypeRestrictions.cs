using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NetSteps.Data.Entities.EntityModels;
using System.Data.SqlClient;
using System.Data;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Business
{
    public class OferTypeRestrictions
    {
        public static string InsertOferTypeRestrictions(OferTypeRestrictionsTable entidad)
        {
            try
            {
                string Result = string.Empty;
                using (SqlConnection connection = new SqlConnection(DataAccess.GetConnectionString("Core")))
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "dbo.uspInsertOferTypeRestrictions";
                        command.CommandType = CommandType.StoredProcedure;

                        SqlParameter OferTypeRestrictionValue = command.Parameters.AddWithValue("@OferTypeRestrictionValue", entidad.OferTypeRestrictionValue);
                        SqlParameter Error = command.Parameters.AddWithValue("@Error", "");
                        Error.Direction = ParameterDirection.Output;
                        command.ExecuteNonQuery();
                        Result = Error.Value.ToString();
                    }
                }
                return Result;
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        public static string UpdateOferTypeRestrictions(OferTypeRestrictionsTable entidad)
        {
            try
            {
                string Result = string.Empty;
                using (SqlConnection connection = new SqlConnection(DataAccess.GetConnectionString("Core")))
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "dbo.uspUpdateOferTypeRestrictions";
                        command.CommandType = CommandType.StoredProcedure;

                        SqlParameter OferTypeRestrictionID = command.Parameters.AddWithValue("@OferTypeRestrictionID", entidad.OferTypeRestrictionID); 
                        SqlParameter OferTypeRestrictionValue = command.Parameters.AddWithValue("@OferTypeRestrictionValue", entidad.OferTypeRestrictionValue);
                        SqlParameter Error = command.Parameters.AddWithValue("@Error", "");
                        Error.Direction = ParameterDirection.Output;
                        command.ExecuteNonQuery();
                        Result = Error.Value.ToString();
                    }
                }
                return Result;
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        public static bool ExisteOferTypeRestrictionsByValue(string oferTypeRestrictionValue)
        {
            try
            {
                int Result = 0;
                using (SqlConnection connection = new SqlConnection(DataAccess.GetConnectionString("Core")))
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "dbo.uspExisteOferTypeRestrictionsByValue";
                        command.CommandType = CommandType.StoredProcedure;

                        SqlParameter OferTypeRestrictionValue = command.Parameters.AddWithValue("@OferTypeRestrictionValue", oferTypeRestrictionValue);
                        Result = Convert.ToInt32(command.ExecuteScalar());
                    }
                }
                return (Result > 0);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }
    }
}
