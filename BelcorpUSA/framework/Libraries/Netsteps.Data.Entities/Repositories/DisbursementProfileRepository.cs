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
    using NetSteps.Commissions.Common.Models;
    using NetSteps.Data.Entities.Business.HelperObjects;

    public partial class DisbursementProfileRepository : IDisbusementProfileRepository
    {
        public void SaveCheckDisbursementProfile(EFTAccount EFTAccount)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Commissions"].ConnectionString))
                {
                    connection.Open();

                    SqlCommand command = connection.CreateCommand();
                    SqlTransaction transaction;

                    transaction = connection.BeginTransaction();

                    command.Connection = connection;
                    command.Transaction = transaction;

                    try
                    {
                        command.CommandText = "SaveCheckDisbursementProfile";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@AccountID", EFTAccount.AccountID));
                        command.Parameters.Add(new SqlParameter("@DisbursementTypeID", EFTAccount.DisbursementTypeID));
                        command.Parameters.Add(new SqlParameter("@Percentage", EFTAccount.PercentToDeposit));
                        command.Parameters.Add(new SqlParameter("@NameOnAccount", EFTAccount.Name ?? string.Empty));
                        command.Parameters.Add(new SqlParameter("@BankAccountNumber", EFTAccount.AccountNumber ?? string.Empty));
                        command.Parameters.Add(new SqlParameter("@BankID", EFTAccount.BankID));
                        command.Parameters.Add(new SqlParameter("@BankName", EFTAccount.BankName ?? string.Empty));
                        command.Parameters.Add(new SqlParameter("@BankAgency", EFTAccount.BankAgency ?? string.Empty));
                        command.Parameters.Add(new SqlParameter("@BankAccountTypeID", EFTAccount.AccountType));

                        command.ExecuteNonQuery();

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                    finally
                    {
                        transaction.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
