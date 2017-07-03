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
    /// <summary>
    /// Implementation of the AccountSponsor Interface
    /// </summary>
    public partial class AccountAdditionalTitularsRepository : IAccountAdditionalTitularsRepository
    {
        public CoApplicantSearchParameters GetAccountAdditionalTitulars(int AccountID)
        {
            try
            {
                CoApplicantSearchParameters CoApplicant = null;

                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[GetAccountAdditionalTitulars]";
                    cmd.Parameters.AddWithValue("@AccountID", AccountID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            CoApplicant = new CoApplicantSearchParameters();

                            #region [Assign Values]

                            CoApplicant.AccountAdditionalTitularID = Convert.ToInt32(reader["AccountAdditionalTitularID"]);
                            CoApplicant.FirstName = reader["FirstName"] != DBNull.Value ? reader["FirstName"].ToString() : null;
                            CoApplicant.LastName = reader["LastName"] != DBNull.Value ? reader["LastName"].ToString() : null;
                            CoApplicant.Relationship = reader["Relationship"] != DBNull.Value ? Convert.ToInt32(reader["Relationship"]) : -1;
                            CoApplicant.Gender = reader["Gender"] != DBNull.Value ? Convert.ToInt32(reader["Gender"]) : -1;
                            CoApplicant.DateOfBirth = reader["DateOfBirth"] != DBNull.Value ? Convert.ToDateTime(reader["DateOfBirth"]) : new DateTime();
                            //CoApplicant.DateOfBirth = reader["DateOfBirth"] != DBNull.Value ? Convert.ToDateTime(reader["DateOfBirth"]) : null;
                            CoApplicant.CPF = reader["CPF"] != DBNull.Value ? reader["CPF"].ToString() : null;
                            CoApplicant.PIS = reader["PIS"] != DBNull.Value ? reader["PIS"].ToString() : null;
                            CoApplicant.RG = reader["RG"] != DBNull.Value ? reader["RG"].ToString() : null;
                            CoApplicant.OrgExp = reader["OrgExp"] != DBNull.Value ? reader["OrgExp"].ToString() : null;
                            CoApplicant.IssueDate = reader["IssueDate"] != DBNull.Value ? Convert.ToDateTime(reader["IssueDate"]) : new DateTime();

                            CoApplicant.Phones = GetAccountAdditionalPhonesParameters(CoApplicant.AccountAdditionalTitularID);
                            #endregion

                        }
                    }
                }

                return CoApplicant;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public List<AccountAdditionalPhonesParameters> GetAccountAdditionalPhonesParameters(int AccountAdditionalTitularID)
        {
            try
            {
                List<AccountAdditionalPhonesParameters> AdditionalPhones = null;

                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[GetAccountAdditionalPhones]";
                    cmd.Parameters.AddWithValue("@AccountAdditionalTitularID", AccountAdditionalTitularID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            AdditionalPhones = new List<AccountAdditionalPhonesParameters>();
                            while (reader.Read())
                            {
                                AccountAdditionalPhonesParameters Phone = new AccountAdditionalPhonesParameters();

                                #region [Assign Values]

                                Phone.AccountAdditionalPhoneID = Convert.ToInt32(reader["AccountAdditionalPhoneID"]);
                                Phone.PhoneTypeID = reader["PhoneTypeID"] != DBNull.Value ? Convert.ToInt32(reader["PhoneTypeID"]) : -1;
                                Phone.PhoneNumber = reader["PhoneNumber"] != DBNull.Value ? reader["PhoneNumber"].ToString() : null;
                                Phone.IsDefault = reader["IsDefault"] != DBNull.Value ? Convert.ToBoolean(reader["IsDefault"]) : false;

                                #endregion

                                AdditionalPhones.Add(Phone);
                            }
                        }
                    }
                }

                return AdditionalPhones;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public int InsertAccountAdditionalTitulars(CoApplicantSearchParameters CoApplicantSearchParameters)
        {
            try
            {

                int result = 0;

                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[InsertAccountAdditionalTitulars]";
                    cmd.Parameters.AddWithValue("@AccountID", CoApplicantSearchParameters.AccountID);
                    cmd.Parameters.AddWithValue("@FirstName", CoApplicantSearchParameters.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", CoApplicantSearchParameters.LastName);
                    cmd.Parameters.AddWithValue("@GerderID", CoApplicantSearchParameters.Gender);
                    cmd.Parameters.AddWithValue("@Brithday", CoApplicantSearchParameters.DateOfBirth);
                    cmd.Parameters.AddWithValue("@SortIndex", 1);
                    cmd.Parameters.AddWithValue("@RelationshipID", CoApplicantSearchParameters.Relationship);
                    cmd.Parameters.AddWithValue("@AccountAdditionalTitularID", 0);

                    cmd.Parameters["@AccountAdditionalTitularID"].Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    result = Convert.ToInt32(cmd.Parameters["@AccountAdditionalTitularID"].Value);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public int UpdateAccountAdditionalTitulars(CoApplicantSearchParameters CoApplicantSearchParameters)
        {
            try
            {

                int result = 0;

                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[UpdateAccountAdditionalTitulars]";
                    cmd.Parameters.AddWithValue("@AccountID", CoApplicantSearchParameters.AccountID);
                    cmd.Parameters.AddWithValue("@FirstName", CoApplicantSearchParameters.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", CoApplicantSearchParameters.LastName);
                    cmd.Parameters.AddWithValue("@GerderID", CoApplicantSearchParameters.Gender);
                    cmd.Parameters.AddWithValue("@Brithday", CoApplicantSearchParameters.DateOfBirth);
                    cmd.Parameters.AddWithValue("@SortIndex", 1);
                    cmd.Parameters.AddWithValue("@RelationshipID", CoApplicantSearchParameters.Relationship);
                    cmd.Parameters.AddWithValue("@AccountAdditionalTitularID", 0);

                    cmd.Parameters["@AccountAdditionalTitularID"].Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    result = Convert.ToInt32(cmd.Parameters["@AccountAdditionalTitularID"].Value);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public void InsertAccountAdditionalTitularSuppliedIDs(AccountAdditionalTitularSuppliedIDsParameters AccountAdditionalTitularSuppliedIDsParameters)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[InsertAccountAdditionalTitularSuppliedIDs]";
                    cmd.Parameters.AddWithValue("@IDTypeID", AccountAdditionalTitularSuppliedIDsParameters.IDTypeID);
                    cmd.Parameters.AddWithValue("@AccountAdditionalTitularID", AccountAdditionalTitularSuppliedIDsParameters.AccountAdditionalTitularID);
                    cmd.Parameters.AddWithValue("@AccountSuppliedValue", AccountAdditionalTitularSuppliedIDsParameters.AccountSuppliedValue);
                    cmd.Parameters.AddWithValue("@IsPrimaryID", AccountAdditionalTitularSuppliedIDsParameters.IsPrimaryID);

                    if (AccountAdditionalTitularSuppliedIDsParameters.IDTypeID == 4 && AccountAdditionalTitularSuppliedIDsParameters.IDExpeditionDate.HasValue)
                        cmd.Parameters.AddWithValue("@IDExpeditionDate", AccountAdditionalTitularSuppliedIDsParameters.IDExpeditionDate);
                    else
                        cmd.Parameters.AddWithValue("@IDExpeditionDate", DBNull.Value);

                    cmd.Parameters.AddWithValue("@ExpeditionEntity", AccountAdditionalTitularSuppliedIDsParameters.ExpeditionEntity ?? string.Empty);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public void UpdateAccountAdditionalTitularSuppliedIDs(AccountAdditionalTitularSuppliedIDsParameters AccountAdditionalTitularSuppliedIDsParameters)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[UpdateAccountAdditionalTitularSuppliedIDs]";
                    cmd.Parameters.AddWithValue("@IDTypeID", AccountAdditionalTitularSuppliedIDsParameters.IDTypeID);
                    cmd.Parameters.AddWithValue("@AccountAdditionalTitularID", AccountAdditionalTitularSuppliedIDsParameters.AccountAdditionalTitularID);
                    cmd.Parameters.AddWithValue("@AccountSuppliedValue", AccountAdditionalTitularSuppliedIDsParameters.AccountSuppliedValue);
                    cmd.Parameters.AddWithValue("@IsPrimaryID", AccountAdditionalTitularSuppliedIDsParameters.IsPrimaryID);

                    if (AccountAdditionalTitularSuppliedIDsParameters.IDTypeID == 4)
                        cmd.Parameters.AddWithValue("@IDExpeditionDate", AccountAdditionalTitularSuppliedIDsParameters.IDExpeditionDate);
                    else
                        cmd.Parameters.AddWithValue("@IDExpeditionDate", DBNull.Value);

                    cmd.Parameters.AddWithValue("@ExpeditionEntity", AccountAdditionalTitularSuppliedIDsParameters.ExpeditionEntity ?? string.Empty);

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
