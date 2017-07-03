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
    public partial class AccountSponsorRepository : IAccountSponsorRepository
    {
        public void InsertAccountSponsor(AccountSponsorSearchParameters AccountSponsor)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    connection.Open();

                    SqlCommand command = connection.CreateCommand();
                    SqlTransaction transaction;

                    transaction = connection.BeginTransaction();

                    command.Connection = connection;
                    command.Transaction = transaction;

                    try
                    {
                        command.CommandText = "InsertAccountSponsors";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@AccountID", AccountSponsor.AccountID));
                        command.Parameters.Add(new SqlParameter("@SponsorID", AccountSponsor.SponsorID));
                        command.Parameters.Add(new SqlParameter("@AccountSponsorTypeID", AccountSponsor.AccountSponsorTypeID));
                        command.Parameters.Add(new SqlParameter("@Position", AccountSponsor.Position));
                        command.Parameters.Add(new SqlParameter("@ModifiedByUserID", AccountSponsor.ModifiedByUserID));

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

        /// <summary>
        /// Retrieves Account Data for the EditSponsorView
        /// </summary>
        public AccountSponsorSearchData GetAccountInformationEditSponsor(AccountSponsorSearchParameters searchParameters)
        {
            try
            {
                AccountSponsorSearchData account = null;

                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Commissions"].ConnectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[GetAccountInformationEditSponsor]";
                    cmd.Parameters.AddWithValue("@AccountID", searchParameters.AccountID);
                    cmd.Parameters.AddWithValue("@PeriodID", searchParameters.PeriodID);
                    cmd.Parameters.AddWithValue("@LanguageID", searchParameters.LanguageID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            account = new AccountSponsorSearchData();

                            #region [Assign Values]

                            account.AccountID = Convert.ToInt32(reader["AccountID"]);
                            account.FirstName = reader["FirstName"] != DBNull.Value ? reader["FirstName"].ToString() : null;
                            account.LastName = reader["LastName"] != DBNull.Value ? reader["LastName"].ToString() : null;
                            account.AccountStatusID = reader["AccountStatusID"] != DBNull.Value ? Convert.ToInt32(reader["AccountStatusID"]) : -1;
                            account.AccountStatus = reader["AccountStatus"] != DBNull.Value ? reader["AccountStatus"].ToString() : null;
                            account.TitleID = reader["TitleID"] != DBNull.Value ? Convert.ToInt32(reader["TitleID"]) : -1;
                            account.TitleName = reader["TitleName"] != DBNull.Value ? reader["TitleName"].ToString() : null;
                            account.SponsorID = reader["SponsorID"] != DBNull.Value ? Convert.ToInt32(reader["SponsorID"]) : -1;
                            account.TerminatedSponsorID = reader["TerminatedSponsorID"] != DBNull.Value ? Convert.ToInt32(reader["TerminatedSponsorID"]) : -1;
                            account.TerminatedSponsor = reader["TerminatedSponsor"] != DBNull.Value ? reader["TerminatedSponsor"].ToString() : null;
                            #endregion

                        }
                    }
                }

                return account;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        /// <summary>
        /// Gets Account Information for the EditSponsorView
        /// </summary>
        public PaginatedList<AccountSponsorLogSearchData> GetUpdateLogEditSponsor(AccountSponsorLogSearchParameters searchParameters)
        {
            try
            {
                PaginatedList<AccountSponsorLogSearchData> UpdateLog = new PaginatedList<AccountSponsorLogSearchData>();
                UpdateLog.TotalCount = 0;

                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Commissions"].ConnectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[GetEditSponsorUpdateLog]";
                    cmd.Parameters.AddWithValue("@AccountID", searchParameters.AccountID);
                    cmd.Parameters.AddWithValue("@PageNumber", searchParameters.PageNumber);
                    cmd.Parameters.AddWithValue("@PageSize", searchParameters.PageSize);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            AccountSponsorLogSearchData LogEntry = new AccountSponsorLogSearchData();

                            #region [Assign Values]

                            LogEntry.OldSponsorID = Convert.ToInt32(reader["OldSponsorID"]);
                            LogEntry.OldSponsorFirstName = reader["OldSponsorFirstName"] != DBNull.Value ? reader["OldSponsorFirstName"].ToString() : null;
                            LogEntry.OldSponsorLastName = reader["OldSponsorLastName"] != DBNull.Value ? reader["OldSponsorLastName"].ToString() : null;
                            LogEntry.NewSponsorID = Convert.ToInt32(reader["NewSponsorID"]);
                            LogEntry.NewSponsorFirstName = reader["NewSponsorFirstName"] != DBNull.Value ? reader["NewSponsorFirstName"].ToString() : null;
                            LogEntry.NewSponsorLastName = reader["NewSponsorLastName"] != DBNull.Value ? reader["NewSponsorLastName"].ToString() : null;
                            LogEntry.CampainStart = reader["CampainStart"] != DBNull.Value ? reader["CampainStart"].ToString() : null;
                            LogEntry.UpdateDate = Convert.ToDateTime(reader["UpdateDate"]);
                            LogEntry.UpdateUser = reader["UpdateUser"] != DBNull.Value ? reader["UpdateUser"].ToString() : null;

                            #endregion
                            UpdateLog.TotalCount = Convert.ToInt32(reader["TotalRecords"]);
                            UpdateLog.Add(LogEntry);
                        }
                    }
                }

                return UpdateLog;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        /// <summary>
        /// Updates Sponsor Information
        /// </summary>
        public string UpdateSponsorInformation(AccountSponsorSearchParameters updateParameters, Boolean PeriodoFuturo = false)
        {
            string result = string.Empty;
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
                        result = InsertUpdateLogEditSponsor(updateParameters, transaction);
                        if (!PeriodoFuturo)
                        {
                            command.CommandText = "UpdateSponsorInformation";
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.Add(new SqlParameter("@AccountID", updateParameters.AccountID));
                            command.Parameters.Add(new SqlParameter("@NewSponsorID", updateParameters.NewSponsorID));
                            command.ExecuteNonQuery();
                        }
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
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
                //throw ex;
            }
        }

        /// <summary>
        /// Updates Sponsor Edit Log
        /// </summary>
        public string InsertUpdateLogEditSponsor(AccountSponsorSearchParameters insertParameters, SqlTransaction transaction)
        {
            string result = string.Empty;
            try
            {
                bool output = false;
                SqlCommand command = transaction.Connection.CreateCommand();
                command.Connection = transaction.Connection;
                command.Transaction = transaction;
                command.CommandText = "InsertEditSponsorUpdateLog";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@AccountID", insertParameters.AccountID));
                command.Parameters.Add(new SqlParameter("@OldSponsorID", insertParameters.LogParameters.OldSponsorID));
                command.Parameters.Add(new SqlParameter("@NewSponsorID", insertParameters.NewSponsorID));
                command.Parameters.Add(new SqlParameter("@PeriodID", insertParameters.NewPeriodID));
                command.Parameters.Add(new SqlParameter("@Active", insertParameters.LogParameters.Active));
                command.Parameters.Add(new SqlParameter("@CreatedUserID", insertParameters.LogParameters.CreatedUserID));
                command.Parameters.Add(new SqlParameter("@Result", output));
                command.Parameters["@Result"].Direction = ParameterDirection.Output;
                command.ExecuteNonQuery();
                bool sw = Convert.ToBoolean(command.Parameters["@Result"].Value);
                return sw ? string.Empty : NetSteps.Common.Globalization.Translation.GetTerm("AlreadyRegisteredSponsor", "Already registered sponsor for this account");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Valida si consultor puede tener cambios en el periodo actual
        /// </summary>
        public bool ValidacionPeriodoActual(AccountSponsorSearchParameters searchParameters)
        {
            try
            {

                bool result = false;

                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Commissions"].ConnectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[APTOCONSULPACTUAL]";
                    cmd.Parameters.AddWithValue("@accountID", searchParameters.AccountID);
                    cmd.Parameters.AddWithValue("@statusconsul", searchParameters.AccountStatusID);
                    cmd.Parameters.AddWithValue("@Aptconsul", result);
                    cmd.Parameters["@Aptconsul"].Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    result = Convert.ToBoolean(cmd.Parameters["@Aptconsul"].Value);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        /// <summary>
        /// Valida reglas de cambio de sponsor
        /// </summary>
        public Tuple<bool, string> ValidateSponsorShipRules(AccountSponsorSearchParameters searchParameters, string processType)
        {
            try
            {

                Tuple<bool, string> result = new Tuple<bool, string>(false, string.Empty);

                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Commissions"].ConnectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[ValidateSponsorShipRules]";
                    cmd.Parameters.AddWithValue("@Account", searchParameters.NewSponsorID);
                    cmd.Parameters.AddWithValue("@ProcessType", processType);
                    cmd.Parameters.AddWithValue("@ScheduledPeriod", searchParameters.NewPeriodID);
                    cmd.Parameters.AddWithValue("@Valid", result.Item1);
                    cmd.Parameters.Add(new SqlParameter("@Reason", SqlDbType.VarChar, 100));
                    cmd.Parameters["@Valid"].Direction = ParameterDirection.Output;
                    cmd.Parameters["@Reason"].Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();
                    result = Tuple.Create(Convert.ToBoolean(cmd.Parameters["@Valid"].Value), cmd.Parameters["@Reason"].Value.ToString());
                }

                return result;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        /// <summary>
        /// Valida si el nuevo sponsor cumple las reglas correctas
        /// </summary>
        public bool ValidateManagerOrHigher(AccountSponsorSearchParameters searchParameters)
        {
            try
            {

                bool result = false;

                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Commissions"].ConnectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[SPONSORVAL]";
                    cmd.Parameters.AddWithValue("@Titleconsul", searchParameters.TitleID);
                    cmd.Parameters.AddWithValue("@NewsponsorTitle", searchParameters.NewSponsorTitleID);
                    cmd.Parameters.AddWithValue("@Aptosponsor", result);
                    cmd.Parameters["@Aptosponsor"].Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    result = Convert.ToBoolean(cmd.Parameters["@Aptosponsor"].Value);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        /// <summary>
        /// seleccion automatica del accountnumber de un sponsor
        /// </summary>
        /// <param name="CEP"></param>
        /// <returns></returns>
        public AccountLocator SeleccionAutomaticaSponsor(string CEP, int MarketID)
        {
            AccountLocator objAccountLocator = new AccountLocator();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[AutomaticSponsor]";
                    cmd.Parameters.AddWithValue("@CEP", CEP);

                    cmd.Parameters.AddWithValue("@MarketID", MarketID);


                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (!Convert.IsDBNull(reader["AccountID"]))
                            {
                                objAccountLocator.AccountID = Convert.ToInt32(reader["AccountID"]);
                            }
                            if (!Convert.IsDBNull(reader["City"]))
                            {
                                objAccountLocator.City = Convert.ToString(reader["City"]);
                            }
                            if (!Convert.IsDBNull(reader["CountryID"]))
                            {
                                objAccountLocator.CountryID = Convert.ToInt32(reader["CountryID"]);
                            }
                            if (!Convert.IsDBNull(reader["FirstName"]))
                            {
                                objAccountLocator.FirstName = Convert.ToString(reader["FirstName"]);
                            }
                            if (!Convert.IsDBNull(reader["html"]))
                            {
                                objAccountLocator.html = Convert.ToString(reader["html"]);
                            }
                            if (!Convert.IsDBNull(reader["LastName"]))
                            {
                                objAccountLocator.LastName = Convert.ToString(reader["LastName"]);
                            }
                            if (!Convert.IsDBNull(reader["PwsUrl"]))
                            {
                                objAccountLocator.PwsUrl = Convert.ToString(reader["PwsUrl"]);
                            }
                            if (!Convert.IsDBNull(reader["State"]))
                            {
                                objAccountLocator.State = Convert.ToString(reader["State"]);
                            }
                            if (!Convert.IsDBNull(reader["Street"]))
                            {
                                objAccountLocator.Street = Convert.ToString(reader["Street"]);
                            }

                            if (!Convert.IsDBNull(reader["Country"]))
                            {
                                objAccountLocator.Country = Convert.ToString(reader["Country"]);
                            }
                            if (!Convert.IsDBNull(reader["Lograduro"]))
                            {
                                objAccountLocator.Lograduro = Convert.ToString(reader["Lograduro"]);
                            }
                            if (!Convert.IsDBNull(reader["Lograduro"]))
                            {
                                objAccountLocator.Lograduro = Convert.ToString(reader["Lograduro"]);
                            }
                            if (!Convert.IsDBNull(reader["EmailAddress"]))
                            {
                                objAccountLocator.EmailAddress = Convert.ToString(reader["EmailAddress"]);
                            }
                            if (!Convert.IsDBNull(reader["PhoneNumber"]))
                            {
                                objAccountLocator.PhoneNumber = Convert.ToString(reader["PhoneNumber"]);
                            }
                            return objAccountLocator;
                        }
                        return objAccountLocator;
                    }


                }


            }
            catch (Exception)
            {

                throw;
            }
        }

        public int[] AplicarreglacValiacionPatrocinio(DataTable dtAccountIds)
        {
            int[] accountIdValid = null;
            int contador = 0;
            SqlParameter op = new SqlParameter();
            op.ParameterName = "@AccountIDs";
            op.SqlDbType = SqlDbType.Structured;
            op.Value = dtAccountIds;

            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Commissions"].ConnectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[UspValidateAcountIDs]";
                    cmd.Parameters.Add(op);


                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            accountIdValid = new int[Convert.ToInt32(reader["total"])];
                            accountIdValid[contador] = Convert.ToInt32(reader["AccountID"]);
                            contador++;
                        }
                        else { accountIdValid = new int[1] { 0 }; }
                        while (reader.Read())
                        {
                            accountIdValid[contador] = Convert.ToInt32(reader["AccountID"]);
                            contador++;
                        }
                        return accountIdValid;
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Dictionary<int, string> ListarCuentasPorCodigoPostal(string CEP, int MarketID)
        {
            Dictionary<int, string> dcAccountPorCep = new Dictionary<int, string>();

            SqlParameter ParamCep = new SqlParameter();
            ParamCep.ParameterName = "@CEP";
            ParamCep.SqlDbType = SqlDbType.VarChar;
            ParamCep.Value = CEP;

            SqlParameter ParamMarketID = new SqlParameter();
            ParamMarketID.ParameterName = "@marketID";
            ParamMarketID.SqlDbType = SqlDbType.Int;
            ParamMarketID.Value = MarketID;

            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Commissions"].ConnectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[UspSearchSponsorPorCodigoCep]";
                    cmd.Parameters.Add(ParamCep);
                    cmd.Parameters.Add(ParamMarketID);


                    using (IDataReader reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            dcAccountPorCep[Convert.ToInt32(reader["Acocuntid"])] = reader["FullName"].ToString();

                        }
                        return dcAccountPorCep;
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Account GetSponsorBasicInfo(int AccountID)
        {
            try
            {
                Account account = new Account();

                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[GetSponsorBasicInfo]";
                    cmd.Parameters.AddWithValue("@AccountID", AccountID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                #region [Assign Values]

                                account.AccountID = Convert.ToInt32(reader["AccountID"]);
                                account.FirstName = reader["FirstName"] != DBNull.Value ? reader["FirstName"].ToString() : string.Empty;
                                account.LastName = reader["LastName"] != DBNull.Value ? reader["LastName"].ToString() : string.Empty;
                                account.EmailAddress = reader["EmailAddress"] != DBNull.Value ? reader["EmailAddress"].ToString() : string.Empty;
                                account.MainPhone = reader["MainPhone"] != DBNull.Value ? reader["MainPhone"].ToString() : string.Empty;
                                #endregion
                            }
                        }
                    }
                }

                return account;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public string TerminateConsultant(int AccountID)
        {
            try
            {
                string result = string.Empty;

                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[TerminateConsultant]";
                    cmd.Parameters.AddWithValue("@AccountID", AccountID);
                    cmd.Parameters.Add(new SqlParameter("@Message", SqlDbType.VarChar, 100));
                    cmd.Parameters["@Message"].Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    result = cmd.Parameters["@Message"].Value.ToString();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
    }
}
