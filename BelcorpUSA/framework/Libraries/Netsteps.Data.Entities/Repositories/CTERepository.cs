﻿using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Extensions;
using System.Data.SqlClient;
﻿using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
﻿using NetSteps.Data.Entities.Exceptions;
using System.Configuration;
using System.Data;

//Modificaciones:
//@1 20151607 BR-CC-012 GYS MD: Se implemento el metodo OrderStatusUpdate
//@2 20151607 BR-CC-012 GYS MD: Se implemento el metodo updateForReceivedBankPayment
//@03 20153108 BR-CC-014 GYS EFP: Creación de método que llama al que se conecta con la BD (SaveStructuredRule)
namespace NetSteps.Data.Entities.Repositories
{
    public class CTERepository
    {
        #region ListarNegotiation KLC - CSTI
        public static List<CTENegotiationSearchData> ListNegotiation()
        {
            List<CTENegotiationSearchData> result = new List<CTENegotiationSearchData>();
            try
            {
                SqlDataReader reader = DataAccess.GetDataReader("spGetNegotiation", null, "Core");

                if (reader.HasRows)
                {
                    result = new List<CTENegotiationSearchData>();
                    while (reader.Read())
                    {
                        result.Add(new CTENegotiationSearchData()
                        {
                            NegotiationLevelID = Convert.ToInt32(reader["NegotiationLevelID"]),
                            Name = Convert.ToString(reader["Name"])
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
        #endregion
        #region Listar BaseAmounts KLC - CSTI
        public static List<CTEFineBaseAmountsData> ListFineBaseAmounts()
        {
            List<CTEFineBaseAmountsData> result = new List<CTEFineBaseAmountsData>();
            try
            {
                SqlDataReader reader = DataAccess.GetDataReader("spFineBaseAmounts", null, "Core");

                if (reader.HasRows)
                {
                    result = new List<CTEFineBaseAmountsData>();
                    while (reader.Read())
                    {
                        result.Add(new CTEFineBaseAmountsData()
                        {
                            FineBaseAmountID = Convert.ToInt32(reader["FineBaseAmountID"]),
                            Name = Convert.ToString(reader["Name"])
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
        #endregion        
        #region BrowseRules KLC - CSTI
        public static PaginatedList<CTERulesSearchData> BrowseRules(CTERulesParameters searchParams)
        {
            // Apply filters
            var rules = CTERepository.BrowseRules().FindAll(x => x.FineAndInterestRulesID == (searchParams.FineAndInterestRulesID.HasValue ? searchParams.FineAndInterestRulesID.Value : x.FineAndInterestRulesID));
            // Apply pagination
            IQueryable<CTERulesSearchData> matchingItems = rules.AsQueryable<CTERulesSearchData>();

            var resultTotalCount = matchingItems.Count();
            matchingItems = matchingItems.ApplyPagination(searchParams);
            return matchingItems.ToPaginatedList<CTERulesSearchData>(searchParams, resultTotalCount);
        }
        public static List<CTERulesSearchData> BrowseRules()
        {
            List<CTERulesSearchData> result = new List<CTERulesSearchData>();
            try
            {
                SqlDataReader reader = DataAccess.GetDataReader("spGetRules", null, "Core");

                if (reader.HasRows)
                {
                    result = new List<CTERulesSearchData>();
                    while (reader.Read())
                    {
                        result.Add(new CTERulesSearchData()
                        {
                            FineAndInterestRulesID = Convert.ToInt32(reader["FineAndInterestRulesID"]),
                            Name = Convert.ToString(reader["Name"])

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
        #endregion
        #region BrowseRulesNegotiation KLC - CSTI
        public static PaginatedList<CTERulesNegotiationData> BrowseRulesNegotiation(CTERulesParameters searchParams)
        {
            // Apply filters
            var rules = CTERepository.BrowseRulesNegotiation().FindAll(x => x.FineAndInterestRulesID == (searchParams.FineAndInterestRulesID.HasValue ? searchParams.FineAndInterestRulesID.Value : x.FineAndInterestRulesID)).OrderBy(x => x.OpeningDay).OrderBy(z => z.Negotiation);
            // Apply pagination
            IQueryable<CTERulesNegotiationData> matchingItems = rules.AsQueryable<CTERulesNegotiationData>();

            var resultTotalCount = matchingItems.Count();
            matchingItems = matchingItems.ApplyPagination(searchParams);
            return matchingItems.ToPaginatedList<CTERulesNegotiationData>(searchParams, resultTotalCount);
        }
        public static List<CTERulesNegotiationData> BrowseRulesNegotiation()
        {
            List<CTERulesNegotiationData> result = new List<CTERulesNegotiationData>();
            try
            {
                SqlDataReader reader = DataAccess.GetDataReader("spGetRulesDetails", null, "Core");

                if (reader.HasRows)
                {
                    result = new List<CTERulesNegotiationData>();
                    while (reader.Read())
                    {
                        result.Add(new CTERulesNegotiationData()
                        {
                            FineAndInterestRulesPerNegotiationLevelID = Convert.ToInt32(reader["FineAndInterestRulesPerNegotiationLevelID"]),
                            FineAndInterestRulesID = Convert.ToInt32(reader["FineAndInterestRulesID"]),
                            Negotiation = Convert.ToString(reader["Negotiation"]),
                            OpeningDay = Convert.ToInt32(reader["OpeningDay"]),
                            FinalDay = Convert.ToInt32(reader["FinalDay"]),
                            FinePercentage = Convert.ToString(reader["FinePercentage"]),
                            AppliedValue = Convert.ToString(reader["AppliedValue"]),
                            MinimumDebt = Convert.ToDouble(reader["MinimumDebt"]),
                            InterestPercentage = Convert.ToString(reader["InterestPercentage"]),
                            Interest = Convert.ToString(reader["Interest"]),
                            Discount = Convert.ToString(reader["Discount"]),
                            FineBaseAmountIDReg = Convert.ToString(reader["FineBaseAmountIDReg"])
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
        #endregion
        #region Save Rules KLC - CSTI
        public static int SaveRules(CTERulesNegotiationData rule)
        {
            try
            {

                Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@Name", rule.Name },
                                                                                            { "@NegotiationLevelID", rule.NegotiationLevelID },
                                                                                            { "@StartDay", rule.OpeningDay },
                                                                                            { "@EndDay", rule.FinalDay },
                                                                                            {"@FinePercentage",rule.FinePercentage},
                                                                                            {"@FineBaseAmountID",rule.FineBaseAmountID},
                                                                                            {"@MinimumAmountForFine",rule.MinimumDebt},
                                                                                            {"@InterestPercentage",rule.InterestPercentage},
                                                                                            {"@InterestBaseAmountID",rule.InterestBaseAmountID}
                                                                                          };
               

                SqlCommand cmd = DataAccess.GetCommand("spSaveRule", parameters, "Core") as SqlCommand;
                cmd.Connection.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static bool ValidateFineAndInterestsRule(int FineAndInterestRulesID)
        {
            bool rpta = false;
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@FineAndInterestRulesID", @FineAndInterestRulesID } };
                SqlCommand cmd = DataAccess.GetCommand("spValidateFineAndInterestsRule", parameters, "Core") as SqlCommand;
                cmd.Connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    rpta = Convert.ToBoolean(reader["Existe"]);
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }

            return rpta;
        }
        #endregion

        public static void ApplyManualPayment(Int32 TicketNumber, Int32 BankID, Int32 UserID, DateTime ProcessOnDateUTC, string Tipollamado, int BankPaymentID)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@TicketNumber", TicketNumber },
                                                                                            { "@BankID", BankID },
                                                                                            { "@UserID", UserID },
                                                                                            { "@ProcessOnDateUTC", ProcessOnDateUTC },
                                                                                            { "@Tipollamado", Tipollamado },
                                                                                            { "@BankPaymentID", BankPaymentID }
                                                                                          };
                SqlCommand cmd = DataAccess.GetCommand("spApplyManualPayment", parameters, "Core") as SqlCommand;
                cmd.Connection.Open();


                cmd.ExecuteNonQuery();
              
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        #region Search Rules Negotiation
        public static List<CTERulesNegotiationData> SearchRulesNegotiations()
        {
            List<CTERulesNegotiationData> result = new List<CTERulesNegotiationData>();
            try
            {
                SqlDataReader reader = DataAccess.GetDataReader("spGetSearchRules", null, "Core");

                if (reader.HasRows)
                {
                    result = new List<CTERulesNegotiationData>();
                    while (reader.Read())
                    {
                        result.Add(new CTERulesNegotiationData()
                        {
                            FineAndInterestRulesID = Convert.ToInt32(reader["FineAndInterestRulesID"]),                            
                            Name = Convert.ToString(reader["Name"])                           
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
        #endregion

        public void ApplyReceivedBankPayment()
        {
            /* 
             * El proceso toma como ingreso la data de tabla BankPayments.
            En caso tal de no encontrar un registro en la tabla OrderPayment 
            se deberá crear un registro log del error en la tabla LogErrorsBankPayments.
             
            Si satisface los requisitos normales este proceso llama al proceso OrderStatusUpdate 
            para actualizar el estado de la orden.
             
            Si se determina que no cumple los requisitos y existe una diferencia considerable 
            entonces se invoca el proceso ApplyCredit
            
            Al final del proceso se invoca el método CreateLog
            en OrderPaymentsLogExtensions.cs
            Ruta: …\BelcorpUSA\framework\Libraries\Netsteps.Data.Entities\Extensions
            
             */
        }

        public void ApplyPayment(int TicketNumber, object valor)
        {
            /* 
            en base al TicketNumber y la logica del negocio(criterios en base a diferencias en montos)
            si satisface los requisitos este proceso llama al proceso OrderStatusUpdate para actualizar el estado
            de la orden.
            pero si se determina que no cumple los requisitos entonces se invoca el proceso ApplyCredit
             * Pendiente definir el significado del parametro VALOR.
             */
        }

        public void ApplyCredit(int AccountID, int EntryReasonID, int EntryOriginID, int EntryTypeID,
                double EntryAmount, int OrderID, int OrderPaymentID)
        {
            /* 
            se invoca el proceso ApplyCredit que está descrito en el caso de uso: 
             * “BR-CT-007 - Gerenciar Cuenta Corriente”. 
             */
        }

        /// <summary>
        /// @2 Actualiza BankPayment y OrderPayment 
        /// OrderPayments.BankName = BankPayments.BankName
        /// OrderPayments.ProcessOnDateUTC  = BankPayments.DateReceivedBank 
        /// OrderPayments.ProcessedOnDateUTC  = BankPayments.DateApplied 
        /// OrderPayments.Accepted  = true
        /// BankPayments .Applied   = true
        /// </summary>
        /// <param name="CTEParameters"></param>
        /// <returns></returns>
        public bool updateForReceivedBankPayment(CTEParameters CTEParameters)
        {
            bool rpta = false;
            try
            {

                Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@OrderPaymentID", CTEParameters.OrderPaymentID },
                                                                                            { "@BankPaymentID", CTEParameters.BankPaymentID },
                                                                                            { "@BankName", CTEParameters.BankName },
                                                                                            { "@ProcessOnDateUTC", CTEParameters.ProcessOnDateUTC },
                                                                                            { "@ProcessedDateUTC", CTEParameters.ProcessedDateUTC },
                                                                                            { "@Accepted", CTEParameters.Accepted },
                                                                                            { "@Applied",CTEParameters.Applied},
                                                                                          };


                SqlCommand cmd = DataAccess.GetCommand("spupdateForReceivedBankPayment", parameters, "Core") as SqlCommand;
                cmd.Connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    rpta = (Convert.ToInt32(reader["Result"]) > 0);
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }

            return rpta;
        }

        /// <summary>
        /// @1 OrderStatusUpdate El proceso actualiza el estatus de una orden a paid,
        /// si se encuentra que todos sus títulos ya han sido pagados.
        /// </summary>
        /// <param name="OrderID"></param>
        public bool OrderStatusUpdate(int OrderID)
        {
            /* con OrderNumber se obtiene el @OrderID de tabla Ordes.
             Si y solo si, todos los registros de OrderPayments en los cuales el campo OrderID es igual a @OrderID 
             * y el campo OrderPaymentStatusID es igual a 2, entonces, en la tabla Orders, en el registro cuyo OrderID
             * es igual a @OrderID se cambiará el valor de su campo OrderStatusID por 4.
             * */
            bool rpta = false;
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@OrderID", OrderID }, };
                SqlCommand cmd = DataAccess.GetCommand("spOrderStatusUpdate", parameters, "Core") as SqlCommand;
                cmd.Connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    rpta = (Convert.ToInt32(reader["Result"]) > 0);
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }

            return rpta;
        }

        /// <summary>
        /// El proceso devuelve una Fecha , partiendo de una Feche de ingreso mas los dias , sin tomar en cuenta los dias Festivos
        /// </summary>
        /// <param name="Fecha"></param>
        /// <param name="days"></param>
        public DateTime ValidateFinalDate(DateTime Date,int Days)
        {         
            DateTime rpta = new DateTime();
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@Date", Date }, { "@Days", Days } };
                SqlCommand cmd = DataAccess.GetCommand("upsValidateFinalDate", parameters, "Core") as SqlCommand;
                cmd.Connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    rpta = Convert.ToDateTime(reader["Fecha"]);
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }

            return rpta;
        }


        #region Modifications @03

        public bool SaveStructuredRule(string name, List<CTERulesNegotiationData> details, int FineAndInterestRulesID)
        {
            bool rpta = false;

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[spSaveRule]", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@FineAndInterestRulesID", FineAndInterestRulesID);
                        cmd.Parameters.AddWithValue("@Name", name);
                        cmd.Parameters.Add("@Details", SqlDbType.Structured).Value = RuleDetailsToDataTable(details);

                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                                rpta = Convert.ToInt32(reader["Answer"]) > 0;
                        }
                    }
                }

            }
            catch (Exception e)
            {
                var msg = e.Message.ToString();
            }

            return rpta;
        }

        private DataTable RuleDetailsToDataTable(List<CTERulesNegotiationData> details)
        {
            DataTable dt = new DataTable("RuleDetail");
            dt.Columns.Add("FineAndInterestRulesPerNegotiationLevelID");
            dt.Columns.Add("NegotiationLevelID");
            dt.Columns.Add("FinePercentage");
            dt.Columns.Add("InterestPercentage");
            dt.Columns.Add("StartDay");
            dt.Columns.Add("EndDay");
            dt.Columns.Add("MinimumAmountForFine");
            dt.Columns.Add("FineBaseAmountID	");
            dt.Columns.Add("InterestBaseAmountID");
            dt.Columns.Add("Discount");
            dt.Columns.Add("FineBaseAmountIDReg");
            foreach (var rule in details)
            {
                DataRow row = dt.NewRow();

                #region Assign Values
                row[0] = rule.FineAndInterestRulesPerNegotiationLevelID;
                row[1] = rule.NegotiationLevelID;
                row[2] = rule.FinePercentage;
                row[3] = rule.InterestPercentage;
                row[4] = rule.StartDay;
                row[5] = rule.EndDay;
                row[6] = rule.MinimumAmountForFine;
                row[7] = rule.FineBaseAmountID;
                row[8] = rule.InterestBaseAmountID;
                row[9] = rule.Discount;
                row[10] = rule.FineBaseAmountIDReg;
                #endregion

                dt.Rows.Add(row);
            }

            return dt;
        }

        #endregion


        public bool SaveAccountCredit(List<int> details, int secc, int UserID)
        {
            bool rpta = false;

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[upsSaveAccountCredit]", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Secc", secc);
                        cmd.Parameters.Add("@Details", SqlDbType.Structured).Value = AccountCredit(details);
                        cmd.Parameters.AddWithValue("@UserID", UserID);
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                                rpta = Convert.ToInt32(reader["Answer"]) > 0;
                        }
                    }
                }

            }
            catch (Exception e)
            {
                var msg = e.Message.ToString();
            }

            return rpta;
        }

        private DataTable AccountCredit(List<int> details)
        {
            DataTable dt = new DataTable("AccountCredits");       
            dt.Columns.Add("AccountID");
            foreach (var id in details)
            {
                DataRow row = dt.NewRow();

                #region Assign Values
                row[0] = id;
                
                #endregion

                dt.Rows.Add(row);
            }

            return dt;
        }

        public  int UpdateSaldoAsignar(AccountCreditSearchData AccountCredit)
        {
            /* con OrderNumber se obtiene el @OrderID de tabla Ordes.
             Si y solo si, todos los registros de OrderPayments en los cuales el campo OrderID es igual a @OrderID 
             * y el campo OrderPaymentStatusID es igual a 2, entonces, en la tabla Orders, en el registro cuyo OrderID
             * es igual a @OrderID se cambiará el valor de su campo OrderStatusID por 4.
             * */
            int rpta = 0;
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@AccountID", AccountCredit.AccountID },
                                                                                            { "@NewSaldoAsignar", AccountCredit.AccountCreditAsg },
                                                                                             { "@S", AccountCredit.Site },
                                                                                             { "@UserID", AccountCredit.UserID }};
                SqlCommand cmd = DataAccess.GetCommand("spUpdateAccountCreditAsig", parameters, "Core") as SqlCommand;
                cmd.Connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    rpta = Convert.ToInt32(reader["Rpta"]);
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }

            return rpta;
        }

        public static int AplicateDescto(string DesctoAplicate, int TicketNumber)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@TicketNumber", TicketNumber },
                                                                                            { "@DesctoAplicate", DesctoAplicate }
                                                                                          };
                SqlCommand cmd = DataAccess.GetCommand("spAplicateDescto", parameters, "Core") as SqlCommand;
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
