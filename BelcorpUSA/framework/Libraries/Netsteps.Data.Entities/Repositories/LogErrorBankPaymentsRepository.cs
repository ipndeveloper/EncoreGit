using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using System.Data.SqlClient;

using System.Configuration;
using System.Data;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;

namespace NetSteps.Data.Entities.Repositories
{
    /// <summary>
    /// Repositorio para los metodos utilizados con la entidad  LogErrorLogErrorBankPayments
    /// </summary>
    public class LogErrorBankPaymentsRepository : IDisposable
    {
        /// <summary>
        /// Insertar nueva entidad tipo LogErrorBankPayments
        /// </summary>
        /// <param name="LogErrorBankPayments">Entidad LogErrorBankPayments</param>
        /// <returns>Retorna la cantidad de elementos afectados</returns>
        public int InserLogErrorBankPayments(LogErrorBankPayments LogErrorBankPayments)
        {
            int rpta = 0;

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("[dbo].[uspInsertBankPaymentLog]", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@pBankPaymentId", LogErrorBankPayments.BankPaymentID);
                        cmd.Parameters.AddWithValue("@pBankName", LogErrorBankPayments.BankName);
                        cmd.Parameters.AddWithValue("@pTicketNumber", LogErrorBankPayments.TicketNumber);
                        cmd.Parameters.AddWithValue("@pOrderNumber", LogErrorBankPayments.OrderNumber);
                        cmd.Parameters.AddWithValue("@pDate", LogErrorBankPayments.Date);

                        rpta = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                rpta = 0;
            }

            return rpta;
        }

        public void Dispose()
        {
            GC.Collect();
        }

        public PaginatedList<LogErrorBankPaymentsSearchData> SearchLogErrorBankPayments(LogErrorBankPaymentsSearchParameter searchParameter)
        {


            List<LogErrorBankPaymentsSearchData> paginatedResult = DataAccess.ExecWithStoreProcedureListParam<LogErrorBankPaymentsSearchData>("Core", "upsListLogErrorBankPayments",

                new SqlParameter("DateBankLog", SqlDbType.VarChar) { Value = (object)searchParameter.DateBankLog ?? DBNull.Value },
                new SqlParameter("Date", SqlDbType.VarChar) { Value = (object)searchParameter.Date ?? DBNull.Value },
                new SqlParameter("BankId", SqlDbType.Int) { Value = (object)searchParameter.BankId ?? DBNull.Value },
                new SqlParameter("FileSequenceLog", SqlDbType.Int) { Value = (object)searchParameter.FileSequenceLog ?? DBNull.Value },
                new SqlParameter("StatusLog", SqlDbType.Int) { Value = (object)searchParameter.StatusLog ?? DBNull.Value }
                ).ToList();

            IQueryable<LogErrorBankPaymentsSearchData> matchingItems = paginatedResult.AsQueryable<LogErrorBankPaymentsSearchData>();

            var resultTotalCount = matchingItems.Count();
            matchingItems = matchingItems.ApplyPagination(searchParameter);

            return matchingItems.ToPaginatedList<LogErrorBankPaymentsSearchData>(searchParameter, resultTotalCount);
        }

        public int ValidatePaymentApplication(LogErrorBankPaymentsSearchParameter data)
        {
            try
            {

                //Insert Zone
                Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "TicketNumber", data.TicketNumber },
                                                                                            { "UserID", data.UserID },
                                                                                            { "LogErrorBankPaymentID",  data.LogErrorBankPaymentID }
                                                                                            };
                //

                SqlCommand cmd = DataAccess.GetCommand("uspValidatePaymentApplication", parameters, "Core") as SqlCommand;
                cmd.Connection.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
    }
}