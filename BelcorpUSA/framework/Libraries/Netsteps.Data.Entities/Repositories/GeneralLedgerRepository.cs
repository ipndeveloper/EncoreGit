﻿using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Extensions;
using System.Data.SqlClient;
using NetSteps.Data.Entities.Exceptions;
using System.Data;

namespace NetSteps.Data.Entities.Repositories
{
    public class GeneralLedgerRepository
    {
        /// <summary>
        /// Developed By KLC - CSTI
        /// </summary>
        /// <returns>Datos de la Vista Ticket Details</returns>        
        public static List<GLPaymeTycketsSearchData> BrowseTicketDetails(int TicketNumber)
        {
            try
            {
                return DataAccess.ExecWithStoreProcedureListParam<GLPaymeTycketsSearchData>("Core", "spGetPaymetTyckets",
                new SqlParameter("TicketNumber", SqlDbType.Int) { Value = TicketNumber }
                ).ToList();
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
            
        }
        /// <summary>
        /// Developed By KLC - CSTI
        /// </summary>
        /// <param name="TicketNumber"></param>
        /// <returns>Datos para Vista Calculate Update Balance</returns>
        public static List<GLCalculateUpdateBalanceSearchData> GetCalculateUpdateBalance(int TicketNumber)
        {
            try
            {
               return DataAccess.ExecWithStoreProcedureListParam<GLCalculateUpdateBalanceSearchData>("Core", "spGetCalculateUpdateBalance",
               new SqlParameter("TicketNumber", SqlDbType.Int) { Value = TicketNumber }
               ).ToList();
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }           
        }
        /// <summary>
        /// Developed By KLC - CSTI
        /// </summary>
        /// <param name="TicketNumber"></param>
        /// <returns>Datos para la Tabla de la Vista Control Log</returns>
        public static PaginatedList<GLControlLogSearchData> GetControlLog(GLControlLogParameters TicketNumber)
        {
            try
            {
                List<GLControlLogSearchData> paginatedResult = DataAccess.ExecWithStoreProcedureListParam<GLControlLogSearchData>("Core", "spGetControlLog",
               new SqlParameter("OrderPaymentID", SqlDbType.Int) { Value = TicketNumber.OrderPaymentID },
                new SqlParameter("AccountNumber", SqlDbType.Int) { Value = (object)TicketNumber.ModifiedByUserID ?? DBNull.Value },
                new SqlParameter("StarDate", SqlDbType.DateTime) { Value = (object)TicketNumber.DateModifiedUTC ?? DBNull.Value }
               ).ToList();

                IQueryable<GLControlLogSearchData> matchingItems = paginatedResult.AsQueryable<GLControlLogSearchData>();

                var resultTotalCount = matchingItems.Count();
                matchingItems = matchingItems.ApplyPagination(TicketNumber);

                return matchingItems.ToPaginatedList<GLControlLogSearchData>(TicketNumber, resultTotalCount);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }    
           
        }       
        /// <summary>
        /// Developed By KLC - CSTI
        /// </summary>
        /// <returns></returns>
        public static List<GLDropdownlistUtilSearchData> GetEntity()
        {
            List<GLDropdownlistUtilSearchData> result = new List<GLDropdownlistUtilSearchData>();
            try
            {
                SqlDataReader reader = DataAccess.GetDataReader("spGetEntity", null, "Core");

                if (reader.HasRows)
                {
                    //result = new List<RoutesData>();
                    while (reader.Read())
                    {
                        result.Add(new GLDropdownlistUtilSearchData()
                        {
                            id = Convert.ToInt32(reader["BankID"]),
                            Name = Convert.ToString(reader["BankName"]),
                            Value = Convert.ToInt32(reader["BankCode"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return result;
        }
        /// <summary>
        /// Developed By KLC - CSTI
        /// </summary>
        /// <param name="BanckID"></param>
        /// <returns></returns>
        public static List<GLDropdownlistUtilSearchData> GetPaymentType(int BanckID)
        {
            List<GLDropdownlistUtilSearchData> result = new List<GLDropdownlistUtilSearchData>();
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@BankID", BanckID }
                                                                                           };

                SqlDataReader reader = DataAccess.GetDataReader("spGetPaymentTypes", parameters, "Core");

                if (reader.HasRows)
                {
                    //result = new List<RoutesData>();
                    while (reader.Read())
                    {
                        result.Add(new GLDropdownlistUtilSearchData()
                        {
                            id = Convert.ToInt32(reader["CollectionEntityID"]),
                            Name = Convert.ToString(reader["CollectionEntityName"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return result;
        }
        /// <summary>
        /// Developed By KLC - CSTI
        /// BR-CC-013
        /// </summary>
        /// <param name="TicketNumber"></param>
        /// <returns>Retorna Vista</returns>
        #region BrowseRulesNegotiation AGA Background
        public static List<GeneralLedgerNegotiationData> BrowseRulesNegotiation(int TicketNumber)
        {
            List<GeneralLedgerNegotiationData> result = new List<GeneralLedgerNegotiationData>();
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@TicketNumber", TicketNumber } };

                SqlDataReader reader = DataAccess.GetDataReader("uspGetOrdersRenegotiation", parameters, "Core");

                if (reader.HasRows)
                {
                    result = new List<GeneralLedgerNegotiationData>();
                    while (reader.Read())
                    {
                        result.Add(new GeneralLedgerNegotiationData()
                        {
                            TicketNumber = Convert.ToInt32(reader["TicketNumber"]),
                            PaymentMethod = Convert.ToString(reader["PaymentMethod"]),
                            TicketStatus = Convert.ToString(reader["TicketStatus"]),
                            OrderNumber = Convert.ToString(reader["OrderNumber"]),
                            
                            AuthorizationNumber = Convert.ToString(reader["AuthorizationNumber"]),
                            PaymentTypeID = Convert.ToInt32(reader["PaymentTypeID"]),
                            ExpirationStatusID = Convert.ToInt32(reader["ExpirationStatusID"]),
                            IsDeferred = Convert.ToBoolean(reader["IsDeferred"]),
                            NegotiationLevelID = Convert.ToInt32(reader["NegotiationLevelID"]),   
                            MaximumAmountOfPayments = Convert.ToInt32(reader["MaximumAmountOfPayments"]),
                            OrderPaymentID = Convert.ToInt32(reader["OrderPaymentID"]),
                            DaysForPayment = Convert.ToInt32(reader["DaysForPayment"]),
                            OrderID = Convert.ToInt32(reader["OrderID"]),

                            NegotiationLevel = Convert.ToString(reader["NegotiationLevel"]),
                            InitialAmount = Convert.ToDecimal(reader["InitialAmount"]),
                            FinancialAmount = Convert.ToDecimal(reader["FinancialAmount"]),
                            DiscountedAmount = Convert.ToDecimal(reader["DiscountedAmount"]),
                            TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                            CurrentExpirationDateUTC = Convert.ToString(reader["CurrentExpirationDateUTC"]),
                            DayExpiration = Convert.ToInt32(reader["DayExpiration"]),
                            DayValidate = Convert.ToString(reader["DayValidate"]),
                            ViewMethodsRenegotiation = Convert.ToInt32(reader["ViewMethodsRenegotiation"]),
                            PaymentCredit = Convert.ToString(reader["PaymentCredit"])
                            
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return result;
        }


        /// <summary>
        /// Developed By KLC - CSTI
        /// </summary>
        /// <param name="PaymentTicketsSearchParameter"></param>
        /// <returns></returns>
        public static PaginatedList<PaymentTicketsSearchData> SearchPaymentTickets(PaymentTicketsSearchParameters PaymentTicketsSearchParameter)
        {


            //List<PaymentTicketsSearchData> paginatedResult = DataAccess.ExecWithStoreProcedureListParam<PaymentTicketsSearchData>("Core", "GetPaymentTickets",
            //    //new SqlParameter("TicketNumber", SqlDbType.Int) { Value = (object)PaymentTicketsSearchParameter.TicketNumber ?? DBNull.Value },
            //    new SqlParameter("OrderNumber", SqlDbType.NVarChar) { Value = (object)PaymentTicketsSearchParameter.OrderNumber}   
            //    ).ToList();

            List<PaymentTicketsSearchData> paginatedResult = new List<PaymentTicketsSearchData>();
            Dictionary<string, object> parameters = new Dictionary<string, object>() { 
            {"@TicketNumber", (object)PaymentTicketsSearchParameter.TicketNumber ?? DBNull.Value},
            {"@OrderNumber", (object)PaymentTicketsSearchParameter.OrderNumber?? ""},
            {"@AccountID", (object)PaymentTicketsSearchParameter.AccountID},
            {"@IsDeferred", (object)PaymentTicketsSearchParameter.IsDeferred?? DBNull.Value},
            {"@Forefit", (object)PaymentTicketsSearchParameter.Forefit?? DBNull.Value},
            {"@ExpirationDate", (object)PaymentTicketsSearchParameter.ExpirationDate?? DBNull.Value},
            {"@ToDate", (object)PaymentTicketsSearchParameter.ToDate?? DBNull.Value},
                                                                                    };

            SqlDataReader reader = DataAccess.GetDataReader("GetPaymentTickets", parameters, "Core");

            if (reader.HasRows)
            {
                paginatedResult = new List<PaymentTicketsSearchData>();
                while (reader.Read())
                {
                    DateTime? dateValidity=null;
                    if (!Convert.IsDBNull(reader["DateValidity"])) dateValidity = Convert.ToDateTime(reader["DateValidity"]);

                    DateTime? CompletedOn = null;
                    if (!Convert.IsDBNull(reader["CompletedOn"])) CompletedOn = Convert.ToDateTime(reader["CompletedOn"]);

                    DateTime? expirationDate = null;
                    if (!Convert.IsDBNull(reader["ExpirationDate"])) expirationDate = Convert.ToDateTime(reader["ExpirationDate"]);


                    paginatedResult.Add(new PaymentTicketsSearchData()
                    {
                        ID = Convert.ToInt32(reader["ID"]),
                        TicketNumber = Convert.ToInt32(reader["TicketNumber"]),
                        OrderNumber = Convert.ToString(reader["OrderNumber"]),
                        CompletedOn = CompletedOn,
                        Status = Convert.ToString(reader["Status"]),
                        InitialAmount = Convert.ToDecimal(reader["InitialAmount"]),
                        FinancialAmount = Convert.ToDecimal(reader["FinancialAmount"]),
                        TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                        DateValidity = dateValidity,
                        ExpirationDate = expirationDate,
                        EpirationStatus = Convert.ToString(reader["EpirationStatus"]),
                        PaymentType = Convert.ToString(reader["PaymentType"])

                    });
                }
            }


            IQueryable<PaymentTicketsSearchData> matchingItems = paginatedResult.AsQueryable<PaymentTicketsSearchData>();
            var resultTotalCount = matchingItems.Count();
            matchingItems = matchingItems.ApplyPagination(PaymentTicketsSearchParameter);
            return matchingItems.ToPaginatedList<PaymentTicketsSearchData>(PaymentTicketsSearchParameter, resultTotalCount);
        }

       
        #endregion       


    }
}
