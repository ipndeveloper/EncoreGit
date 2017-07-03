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
    using NetSteps.Data.Entities.EntityModels;
    /// <summary>
    /// Implementation of the AccountSponsor Interface
    /// </summary>
    public partial class AccountSuppliedIDsRepository : IAccountSuppliedIDsRepository
    {
        public void InsertAccountSuppliedIDs(AccountSuppliedIDsParameters AccountSuppliedIDsParameters)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    connection.Open();

                    SqlCommand command = connection.CreateCommand();
                    SqlTransaction transaction = connection.BeginTransaction();

                    command.Connection = connection;
                    command.Transaction = transaction;

                    try
                    {

                        string IDExpeditionIDate = string.Empty;

                        if (AccountSuppliedIDsParameters.IDTypeID == 4 && AccountSuppliedIDsParameters.IDExpeditionIDate.HasValue)
                        {
                            IDExpeditionIDate = String.Format("{0:dd/MM/yyyy}", AccountSuppliedIDsParameters.IDExpeditionIDate);
                        }

                        command.CommandText = "InsertAccountSuppliedIDs";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@IDTypeID", AccountSuppliedIDsParameters.IDTypeID));
                        command.Parameters.Add(new SqlParameter("@AccountID", AccountSuppliedIDsParameters.AccountID));
                        command.Parameters.Add(new SqlParameter("@AccountSuppliedIDValue", AccountSuppliedIDsParameters.AccountSuppliedIDValue ?? string.Empty));
                        command.Parameters.Add(new SqlParameter("@IsPrimaryID", AccountSuppliedIDsParameters.IsPrimaryID));
                        command.Parameters.Add(new SqlParameter("@IDExpeditionIDate", IDExpeditionIDate));
                        command.Parameters.Add(new SqlParameter("@ExpeditionEntity", AccountSuppliedIDsParameters.ExpeditionEntity ?? string.Empty));

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

        public List<AccountSuppliedIDsTable> GetAccountSuppliedIDByAccountID(AccountSuppliedIDsParameters entidad)
        {
            List<AccountSuppliedIDsTable> listQuery=new List<AccountSuppliedIDsTable>();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
 		    {
 			    connection.Open();
 			     using (SqlCommand command = connection.CreateCommand())
                 {
 			        command.Connection = connection;
 			        command.CommandText = "uspAccountSuppliedIDByAccountID";
 			        command.CommandType = CommandType.StoredProcedure;
 			        command.Parameters.Add(new SqlParameter("@AccountID",entidad.AccountID));
 				    SqlDataReader dtrQuery = command.ExecuteReader();

 				    while (dtrQuery.Read())
 				    {
                        AccountSuppliedIDsTable entQuery = new AccountSuppliedIDsTable();
 					    entQuery.AccountSuppliedID = Convert.ToInt32(dtrQuery["AccountSuppliedID"]);
 					    entQuery.IDTypeID = Convert.ToInt32(dtrQuery["IDTypeID"]);
 					    entQuery.Name = Convert.ToString(dtrQuery["Name"]);
 					    entQuery.AccountID = Convert.ToInt32(dtrQuery["AccountID"]);
 					    entQuery.AccountSuppliedIDValue = Convert.ToString(dtrQuery["AccountSuppliedIDValue"]);
 					    entQuery.IsPrimaryID = Convert.ToBoolean(dtrQuery["IsPrimaryID"]);
 					    if (dtrQuery["IDExpeditionIDate"] != DBNull.Value) entQuery.IDExpeditionIDate = Convert.ToDateTime(dtrQuery["IDExpeditionIDate"]);
 					    entQuery.ExpeditionEntity = Convert.ToString(dtrQuery["ExpeditionEntity"]);
                        listQuery.Add(entQuery);
 				    }
                 }
            }
            return listQuery;
        }

        public string DeleteAccountSuppliedIDsByAccountID(int accountID)
        {
            string resultado = string.Empty;
            using (SqlConnection connection = new SqlConnection(DataAccess.GetConnectionString("Core")))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "dbo.uspDeleteAccountSuppliedIDsByAccountID";
                    command.CommandType = CommandType.StoredProcedure;
                    SqlParameter DispatchID = command.Parameters.AddWithValue("@AccountID", accountID);
                    SqlParameter Error = command.Parameters.AddWithValue("@Error", "");
                    Error.Direction = ParameterDirection.Output;

                    SqlDataReader dr = command.ExecuteReader();
                    resultado = Error.Value.ToString();
                }
            }
            return resultado;
        }

        public List<AccountSuppliedIDsTable> GetAccountSuppliedIDsByType(AccountSuppliedIDsParameters SearchSuppliedID)
        {
            return DataAccess.ExecWithStoreProcedureListParam<AccountSuppliedIDsTable>("Core", "uspAccountSuppliedIDByType",
                new SqlParameter("IDTypeID", SqlDbType.Int) { Value = SearchSuppliedID.IDTypeID},
                new SqlParameter("AccountSuppliedIDValue", SqlDbType.NVarChar) { Value = SearchSuppliedID.AccountSuppliedIDValue}
                ).ToList();                  
        }
    }
}
