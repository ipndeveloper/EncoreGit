using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business;

using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Extensions;
using System.Data.SqlClient;
﻿using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
﻿using NetSteps.Data.Entities.Exceptions;
using System.Configuration;
using System.Data;
using NetSteps.Data.Entities.Dto;

namespace NetSteps.Data.Entities.Repositories
{
    public  class RenegotiationMethodsRepository
    { 
        
        #region Form Mantenimiento Renegotiation
        public static PaginatedList<RenegotiationSearchData> BrowseRenegotiation(RenegotiationSearchParameters searchParams)
        {

            var rules = RenegotiationMethodsRepository.BrowseRenegotiation().FindAll((x => x.DescriptionRenegotiation.Contains(searchParams.DescriptionRenegotiation != "" ? searchParams.DescriptionRenegotiation : x.DescriptionRenegotiation))); 
            IQueryable<RenegotiationSearchData> matchingItems = rules.AsQueryable<RenegotiationSearchData>();

            var resultTotalCount = matchingItems.Count();
            matchingItems = matchingItems.ApplyPagination(searchParams);
            return matchingItems.ToPaginatedList<RenegotiationSearchData>(searchParams, resultTotalCount);
        }

        public static List<RenegotiationSearchData> BrowseRenegotiation()
        {

            List<RenegotiationSearchData> result = new List<RenegotiationSearchData>();
            try
            {
                SqlDataReader reader = DataAccess.GetDataReader("spListRenegotiationMethods", null, "Core");

                if (reader.HasRows)
                {
                    result = new List<RenegotiationSearchData>();
                    while (reader.Read())
                    {
                        result.Add(new RenegotiationSearchData()
                        {
                            RenegotiationConfigurationID = Convert.ToInt32(reader["RenegotiationConfigurationID"]),
                            DescriptionRenegotiation = Convert.ToString(reader["DescriptionRenegotiation"]),
                            SharesNumber = Convert.ToInt32(reader["SharesNumber"]),
                            Site = Convert.ToString(reader["Site"]),
                            FineAndInterestRules = Convert.ToString(reader["FineAndInterestRule"]),
                            //DayExpiration = Convert.ToInt32(reader["DayExpiration"]),
                            DayValidate = Convert.ToInt32(reader["DayValidate"]),
                            FirstSharesday = Convert.ToInt32(reader["FirstSharesday"]),
                            SkillfulCalendarFirst = Convert.ToString(reader["SkillfulCalendarFirst"]),
                            SharesInterval = Convert.ToInt32(reader["SharesInterval"]),
                            SkillfulRemainingCalendar = Convert.ToString(reader["SkillfulRemainingCalendar"]),
                            ModifiesDates = Convert.ToString(reader["ModifiesDates"]),
                            ModifiesValues = Convert.ToString(reader["ModifiesValues"]),
                            Observation = Convert.ToString(reader["Observation"]),
                            FineAndInterestRuleID = Convert.ToString(reader["FineAndInterestRuleID"])

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
       
        public static RenegotiationSearchData DataRenegotiation(int Id)
        {

            RenegotiationSearchData result = new RenegotiationSearchData();
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@RenegotiationConfigurationID",Id }
                                                                                           
                                                                                          };


                SqlDataReader reader = DataAccess.GetDataReader("spGetRenegotiationMethod", parameters, "Core");

                if (reader.HasRows)
                {
                    result = new RenegotiationSearchData();
                    while (reader.Read())
                    {
                        
                            result.RenegotiationConfigurationID = Convert.ToInt32(reader["RenegotiationConfigurationID"]);
                            result.DescriptionRenegotiation = Convert.ToString(reader["DescriptionRenegotiation"] == null ? "" : reader["DescriptionRenegotiation"]);
                            result.SharesNumber = Convert.ToInt32(reader["SharesNumber"]);
                            result.Site = Convert.ToString(reader["Site"]);
                            //result.RegFinePercentage = Convert.ToString(reader["RegFinePercentage"]).Replace(",", ".");
                            //result.RegInteresPercentage = Convert.ToString(reader["RegInteresPercentage"]).Replace(",",".");
                            //result.RegMinimumAmountForFine = Convert.ToInt32(reader["RegMinimumAmountForFine"]);
                            result.FineAndInterestRules = Convert.ToString(reader["FineAndInterestRule"]);
                            //result.DayExpiration = Convert.ToInt32(reader["DayExpiration"]);
                            result.DayValidate = Convert.ToInt32(reader["DayValidate"]);

                            result.FirstSharesday = Convert.ToInt32(reader["FirstSharesday"]);
                            result.SkillfulCalendarFirst = Convert.ToString(reader["SkillfulCalendarFirst"]);
                            result.SharesInterval = Convert.ToInt32(reader["SharesInterval"]);
                            result.SkillfulRemainingCalendar = Convert.ToString(reader["SkillfulRemainingCalendar"]);
                            result.ModifiesDates = Convert.ToString(reader["ModifiesDates"]);
                            result.ModifiesValues = Convert.ToString(reader["ModifiesValues"]);
                            result.Observation = Convert.ToString(reader["Observation"]);
                            result.DisabledFineAndInterestRules = Convert.ToBoolean(reader["DisabledFineAndInterestRules"]);
                       
                    }
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return result;

        }

        //public static PaginatedList<RenegotiationDetSearchData> BrowseRenegotiationDet(RenegotiationDetSearchParameters searchParams)
        //{
        //    // Apply filters
        //    var rules = RenegotiationMethodsRepository.ListRenegotiationDet(searchParams);
            
        //    IQueryable<RenegotiationDetSearchData> matchingItems = rules.AsQueryable<RenegotiationDetSearchData>();

        //    var resultTotalCount = matchingItems.Count();
        //    matchingItems = matchingItems.ApplyPagination(searchParams);
        //    return matchingItems.ToPaginatedList<RenegotiationDetSearchData>(searchParams, resultTotalCount);
        //}

        //public static List<RenegotiationDetSearchData> ListRenegotiationDet(RenegotiationDetSearchParameters searchParams)
        //{
            
        //    List<RenegotiationDetSearchData> result = new List<RenegotiationDetSearchData>();
        //    try
        //    {
        //        Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@RenegotiationConfigurationID",searchParams.RenegotiationConfigurationID }
                                                                                           
        //                                                                                  };


        //        SqlDataReader reader = DataAccess.GetDataReader("spListRenegotiationDet", parameters, "Core");

        //        if (reader.HasRows)
        //        {
        //            result = new List<RenegotiationDetSearchData>();
        //            while (reader.Read())
        //            {
        //                result.Add(new RenegotiationDetSearchData()
        //                {

        //                    RenegotiationConfigurationDetailsID = Convert.ToInt32(reader["RenegotiationConfigurationDetailsID"]),
        //                    RenegotiationConfigurationID = Convert.ToInt32(reader["RenegotiationConfigurationID"]),
        //                    OpeningDay = Convert.ToInt32(reader["OpeningDay"]),
        //                    FinalDay = Convert.ToInt32(reader["FinalDay"]),
        //                    FineBaseAmountID = Convert.ToInt32(reader["FineBaseAmountID"]),
        //                    Discount = Convert.ToString(reader["Discount"]),
        //                    FineBaseAmountDesc = Convert.ToString(reader["FineBaseAmountDesc"])

        //                });
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
        //    }
        //    return result;
           
        //}


       

        public static bool SaveStructuredReng(RenegotiationSearchData Renegotiation)//, List<RenegotiationDetSearchData> details)
        {
            bool rpta = false;

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[spSaveRenegotiation]", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@RenegotiationConfigurationID", Renegotiation.RenegotiationConfigurationID);
                        cmd.Parameters.AddWithValue("@DescriptionRenegotiation", Renegotiation.DescriptionRenegotiation);
                        cmd.Parameters.AddWithValue("@SharesNumber", Renegotiation.SharesNumber);
                        cmd.Parameters.AddWithValue("@Site", Renegotiation.Site);
                        cmd.Parameters.AddWithValue("@FineAndInterestRulesID", Renegotiation.FineAndInterestRules);
                        //cmd.Parameters.AddWithValue("@DayExpiration", 0);
                        cmd.Parameters.AddWithValue("@DayValidate", Renegotiation.DayValidate);
                        cmd.Parameters.AddWithValue("@FirstSharesday", Renegotiation.FirstSharesday);
                        cmd.Parameters.AddWithValue("@SkillfulCalendarFirst", Renegotiation.SkillfulCalendarFirst);
                        cmd.Parameters.AddWithValue("@SharesInterval", Renegotiation.SharesInterval);
                        cmd.Parameters.AddWithValue("@SkillfulRemainingCalendar", Renegotiation.SkillfulRemainingCalendar);
                        cmd.Parameters.AddWithValue("@ModifiesDates", Renegotiation.ModifiesDates);
                        cmd.Parameters.AddWithValue("@ModifiesValues", Renegotiation.ModifiesValues);
                        cmd.Parameters.AddWithValue("@Observation", Renegotiation.Observation);
                        //cmd.Parameters.Add("@Details", SqlDbType.Structured).Value = RDetailsToDataTable(details);

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
               string m= e.Message;
            }

            return rpta;
        }

        //private   static  DataTable RDetailsToDataTable(List<RenegotiationDetSearchData> details)
        //{
        //    DataTable dt = new DataTable("RenegotiationDetail");
        //    dt.Columns.Add("RenegotiationConfigurationDetailsID");
        //    dt.Columns.Add("OpeningDay");
        //    dt.Columns.Add("FinalDay");
        //    dt.Columns.Add("FineBaseAmountDesc");
        //    dt.Columns.Add("Discount");
           

        //    foreach (var rule in details)
        //    {
        //        DataRow row = dt.NewRow();

        //        #region Assign Values
        //        row[0] = int.Parse(rule.RenegotiationConfigurationDetailsID.ToString());
        //        row[1] = rule.OpeningDay;
        //        row[2] = rule.FinalDay;
        //        row[3] = rule.FineBaseAmountID;
        //        row[4] = rule.Discount;
        //        #endregion

        //        dt.Rows.Add(row);
        //    }

        //    return dt;
        //}

        #endregion

        #region Proces Renegotiation


        public static List<RenegotiationMethodDto> ListRenegotiationMethodsByOrder(RenegotiationMethodDto datRegMet)
        {

            List<RenegotiationMethodDto> ListResult = new List<RenegotiationMethodDto>();
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {  
                { "@Site",datRegMet.Site },
                { "@OrderPaymentID",datRegMet.OrderPaymentID } };

                SqlDataReader reader = DataAccess.GetDataReader("spGetRenegotiationMethodsByOrder", parameters, "Core");

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        RenegotiationMethodDto result = new RenegotiationMethodDto();
                        result.FineAndInterestRulesPerNegotiationLevelID = Convert.ToInt32(reader["FineAndInterestRulesPerNegotiationLevelID"]);
                        result.FineAndInterestRulesID = Convert.ToInt32(reader["FineAndInterestRulesID"]);
                        result.RenegotiationConfigurationID = Convert.ToInt32(reader["RenegotiationConfigurationID"]);
                        result.Plano = Convert.ToString(reader["Plano"]);
                        result.Cuotas = Convert.ToInt32(reader["Cuotas"]);
                        result.Juros_Dia = Convert.ToDecimal(reader["Juros_Dia"]);
                        result.Taxa = Convert.ToString(reader["Taxa"]);
                        result.DiscountDesc = Convert.ToString(reader["DiscountDesc"]);
                        ListResult.Add(result);
                    }
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return ListResult;

        }


        private static string _reportConnectionString = string.Empty;

        private static string GetReportConnectionString()
        {

            _reportConnectionString = EntitiesHelpers.GetAdoConnectionString<NetStepsEntities>();

            return _reportConnectionString;
        }

        public static RenegotiationSharedDto ListRenegotiationShares(
             RenegotiationMethodDto datRegMet)
        {
            string cadena = GetReportConnectionString();

            RenegotiationSharedDto returnReg = new RenegotiationSharedDto();

            DataSet dst = new DataSet();
            SqlParameter op = null;

            try
            {
                using (SqlConnection ocon = new SqlConnection(cadena))
                {
                    ocon.Open();
                    using (SqlCommand ocom = new SqlCommand())
                    {
                        ocom.Connection = ocon;
                        ocom.CommandTimeout = 0;
                        ocom.CommandType = CommandType.StoredProcedure;
                        ocom.CommandText = "spGetRenegotiationMethodsShares";

                        op = new SqlParameter();
                        op.DbType = DbType.Int32;
                        op.ParameterName = "@OrderPaymentID";
                        op.Value = datRegMet.OrderPaymentID;
                        ocom.Parameters.Add(op);

                        op = new SqlParameter();
                        op.DbType = DbType.Int32;
                        op.ParameterName = "@RenegotiationConfigurationID";
                        op.Value = datRegMet.RenegotiationConfigurationID;
                        ocom.Parameters.Add(op);

                        op = new SqlParameter();
                        op.DbType = DbType.Int32;
                        op.ParameterName = "@FineAndInterestRulesPerNegotiationLevelID";
                        op.Value = datRegMet.FineAndInterestRulesPerNegotiationLevelID;
                        ocom.Parameters.Add(op);

                        using (SqlDataAdapter odta = new SqlDataAdapter())
                        {
                            odta.SelectCommand = ocom;
                            odta.Fill(dst);

                            returnReg.TotalAmount = dst.Tables[0].Rows[0]["TotalAmount"].ToString();
                            returnReg.Discount = dst.Tables[0].Rows[0]["Discount"].ToString();// descuento
                            returnReg.TotalPay = dst.Tables[0].Rows[0]["TotalPay"].ToString();//  total a Pagar        
                            returnReg.ModifiesValues = dst.Tables[0].Rows[0]["ModifiesValues"].ToString();
                            returnReg.FirstDateExpirated = dst.Tables[0].Rows[0]["FirstDateExpirated"].ToString();//PrimeraFecha
                            returnReg.SharesInterval = dst.Tables[0].Rows[0]["SharesInterval"].ToString();
                            returnReg.ModifiesDates = dst.Tables[0].Rows[0]["ModifiesDates"].ToString();
                            returnReg.LastDateExpirated = dst.Tables[0].Rows[0]["LastDateExpirated"].ToString();//UltimaFecha
                            returnReg.ValShared = dst.Tables[0].Rows[0]["ValShared"].ToString();//  valor de cuotas
                            returnReg.NumShared = dst.Tables[0].Rows[0]["NumShared"].ToString();//Numero de Cuotas
                            returnReg.DayValidate = dst.Tables[0].Rows[0]["DayValidate"].ToString();

                            foreach (var item in dst.Tables[1].AsEnumerable())
                            {
                                var reg = new RenegotiationSharedDetDto()
                                {
                                    Parcela = item["Parcela"].ToString(),
                                    ExpirationDate = item["ExpirationDate"].ToString(),
                                    ValShared = item["ValShared"].ToString()
                                };

                                returnReg.ListShared.Add(reg);
                            }

                            return returnReg;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        public bool RegisterRenegotiationOrderPayment(string site, List<OrderPaymentNegotiationData> oenOrderPayments, int ultRegis, int numerocuotas, decimal DescuentoGlobal)
        {
            bool rpta = false;

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[UpsInsRenegotiationOrderPayments]", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@site", site);
                        cmd.Parameters.Add("@piTypeOrderPayment", SqlDbType.Structured).Value = RuleDetailsToDataTable(oenOrderPayments);
                        cmd.Parameters.AddWithValue("@ultRegis", ultRegis);
                        cmd.Parameters.AddWithValue("@numerocuotas", numerocuotas);
                        cmd.Parameters.AddWithValue("@DescuentoGlobal", DescuentoGlobal);
                        
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

        private DataTable RuleDetailsToDataTable(List<OrderPaymentNegotiationData> details)
        {
            DataTable dt = new DataTable("RenegotiationOrderPayments");
            dt.Columns.Add("TicketNumber");
            dt.Columns.Add("CurrentExpirationDateUTC");
            dt.Columns.Add("InitialAmount");
            dt.Columns.Add("TotalAmount");
            dt.Columns.Add("ModifiedByUserID");
            dt.Columns.Add("DateValidity");
            dt.Columns.Add("RenegotiationConfigurationID");

            foreach (var rule in details)
            {
                DataRow row = dt.NewRow();

                #region Assign Values
                row[0] = rule.TicketNumber;
                row[1] = rule.CurrentExpirationDateUTC;
                row[2] = rule.InitialAmount;
                row[3] = rule.TotalAmount;
                row[4] = rule.ModifiedByUserID;
                row[5] = rule.DateValidity;
                row[6] = rule.RenegotiationConfigurationID;

                #endregion

                dt.Rows.Add(row);
            }

            return dt;
        }

        #endregion

    }
}
