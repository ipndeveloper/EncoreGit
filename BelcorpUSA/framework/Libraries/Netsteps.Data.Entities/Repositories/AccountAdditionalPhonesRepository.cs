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
    using NetSteps.Data.Entities.Business;
    using NetSteps.Common.Base;
    using System.Configuration;

    public partial class AccountAdditionalPhonesRepository : IAccountAdditionalPhonesRepository
    {
        public void InsertAccountAdditionalPhones(AccountAdditionalPhonesParameters AccountAdditionalPhonesParameters)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[InsertAccountAdditionalPhones]";
                    cmd.Parameters.AddWithValue("@AccountAdditionalTitularID", AccountAdditionalPhonesParameters.AccountAdditionalTitularID);
                    cmd.Parameters.AddWithValue("@PhoneTypeID", AccountAdditionalPhonesParameters.PhoneTypeID);
                    cmd.Parameters.AddWithValue("@PhoneNumber", AccountAdditionalPhonesParameters.PhoneNumber);
                    cmd.Parameters.AddWithValue("@IsPrivate", AccountAdditionalPhonesParameters.IsPrivate);
                    cmd.Parameters.AddWithValue("@IsDefault", AccountAdditionalPhonesParameters.IsDefault);
                    cmd.Parameters.AddWithValue("@ModifiedByUserID", AccountAdditionalPhonesParameters.ModifiedByUserID);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public void UpdateAccountAdditionalPhones(AccountAdditionalPhonesParameters AccountAdditionalPhonesParameters)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[UpdateAccountAdditionalPhones]";
                    cmd.Parameters.AddWithValue("@AccountAdditionalTitularID", AccountAdditionalPhonesParameters.AccountAdditionalTitularID);
                    cmd.Parameters.AddWithValue("@PhoneTypeID", AccountAdditionalPhonesParameters.PhoneTypeID);
                    cmd.Parameters.AddWithValue("@PhoneNumber", AccountAdditionalPhonesParameters.PhoneNumber);
                    cmd.Parameters.AddWithValue("@IsPrivate", AccountAdditionalPhonesParameters.IsPrivate);
                    cmd.Parameters.AddWithValue("@IsDefault", AccountAdditionalPhonesParameters.IsDefault);
                    cmd.Parameters.AddWithValue("@ModifiedByUserID", AccountAdditionalPhonesParameters.ModifiedByUserID);

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
