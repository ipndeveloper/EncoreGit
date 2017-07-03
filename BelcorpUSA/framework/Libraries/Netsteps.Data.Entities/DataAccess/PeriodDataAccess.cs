using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Business;
using System.Data.SqlClient;
using System.Data;
using NetSteps.Data.Entities.Exceptions;
using System.Globalization;

/*
 * @01 20150811 BR-MLM-007 CSTI JMO: Added GetThreeNextPeriods Method
 */

namespace NetSteps.Data.Entities
{
    public class PeriodDataAccess
    {
        public static List<PeriodSearchData> Search()
        {
            List<PeriodSearchData> result = null;
            try
            {
                SqlDataReader reader = DataAccess.GetDataReader("upsGetPeriods", null, "Core");

                if (reader.HasRows)
                {
                    result = new List<PeriodSearchData>();
                    while (reader.Read())
                    {
                        PeriodSearchData period = new PeriodSearchData();
                        period.PeriodID = Convert.ToInt32(reader["PeriodID"]);
                        period.PlanID = Convert.ToInt32(reader["PlanID"]);
                        period.PlanName = Convert.ToString(reader["PlanName"]);
                        period.StartDate = Convert.ToDateTime(reader["StartDate"]);
                        period.LockDate = Convert.ToDateTime(reader["LockDate"]);
                        period.EndDate = Convert.ToDateTime(reader["EndDate"]);
                        if (!Convert.IsDBNull(reader["ClosedDate"]))
                        {
                            period.ClosedDate = Convert.ToDateTime(reader["ClosedDate"]);    
                        }
                        period.Description = Convert.ToString(reader["Description"]);

                        result.Add(period);
                    }
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return result;
        }

        public static void Insert(PeriodSearchData period)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@StartDate", period.StartDate },
                                                                                            { "@EndDate", period.EndDate },
                                                                                            { "@ClosedDate", period.ClosedDate },
                                                                                            { "@PlanID", period.PlanID },
                                                                                            { "@EarningsViewable", period.EarningsViewable }, 
                                                                                            { "@BackOfficeDisplayStartDate", period.BackOfficeDisplayStartDate }, 
                                                                                            { "@DisbursementsProcessed", period.DisbursementsProcessed }, 
                                                                                            { "@Description", period.Description }, 
                                                                                            //{ "@StartDateUTC", period.StartDateUTC }, 
                                                                                            //{ "@EndDateUTC", period.EndDateUTC },
                                                                                            { "@LockDate", period.LockDate } };


                SqlCommand cmd = DataAccess.GetCommand("upsInsPeriod", parameters, "Core") as SqlCommand;
                cmd.Connection.Open();
                period.PeriodID = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static void Update(PeriodSearchData period)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@PeriodID", period.PeriodID }, 
                                                                                            { "@StartDate", period.StartDate },
                                                                                            { "@EndDate", period.EndDate },
                                                                                            { "@ClosedDate", period.ClosedDate },
                                                                                            { "@PlanID", period.PlanID },
                                                                                            { "@EarningsViewable", period.EarningsViewable }, 
                                                                                            { "@BackOfficeDisplayStartDate", period.BackOfficeDisplayStartDate }, 
                                                                                            { "@DisbursementsProcessed", period.DisbursementsProcessed }, 
                                                                                            { "@Description", period.Description }, 
                                                                                            //{ "@StartDateUTC", period.StartDateUTC }, 
                                                                                            //{ "@EndDateUTC", period.EndDateUTC },
                                                                                            { "@LockDate", period.LockDate } };


                SqlCommand cmd = DataAccess.GetCommand("upsUpdPeriod", parameters, "Core") as SqlCommand;
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static DateTime? GetStartDateCreatingMode(int planId)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@PlanID", planId } };

                SqlCommand cmd = DataAccess.GetCommand("upsGetStartDateCreatingMode", parameters, "Core") as SqlCommand;
                cmd.Connection.Open();

                var dateReturn = cmd.ExecuteScalar();

                if (Convert.IsDBNull(dateReturn))
                {
                    return null;
                }
                else
                {
                    return Convert.ToDateTime(dateReturn);
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static bool VerifyOverlapStartDate(int planId, DateTime startDate)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@PlanID", planId },
                                                                                           { "@StartDate", startDate }};

                SqlCommand cmd = DataAccess.GetCommand("upsVerifyOverlapStartDate", parameters, "Core") as SqlCommand;
                cmd.Connection.Open();
                return Convert.ToBoolean(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static bool VerifyOverlapDate(int periodId, int planId, DateTime startDate, DateTime endDate)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@PlanID", planId },
                                                                                           { "@PeriodID", periodId },
                                                                                           { "@EndDate", endDate },  
                                                                                           { "@StartDate", startDate }};

                SqlCommand cmd = DataAccess.GetCommand("upsVerifyOverlapDate", parameters, "Core") as SqlCommand;
                cmd.Connection.Open();
                return Convert.ToBoolean(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        //Developed by BAL - CSTI - AINI
        public static List<CatalogPeriod> SearchCatalogPeriods(int catalogId)
        {
            List<CatalogPeriod> result = null;
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@CatalogID", catalogId } };

                SqlDataReader reader = DataAccess.GetDataReader("upsGetPeriodsByCatalog", parameters, "Core");

                if (reader.HasRows)
                {
                    result = new List<CatalogPeriod>();
                    while (reader.Read())
                    {
                        result.Add(new CatalogPeriod()
                        {
                            CatalogPeriodID = Convert.ToInt32(reader["CatalogPeriodID"]),
                            CatalogID = Convert.ToInt32(reader["CatalogID"]),
                            PeriodID = Convert.ToInt32(reader["PeriodID"]),
                            PeriodDescription =  Convert.ToString(reader["Description"])
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

        public static void DeleteCatalogPeriods(int catalogId, SqlConnection connection, SqlTransaction transaction)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@CatalogID", catalogId } };

                SqlCommand cmd = DataAccess.GetCommand("upsDelCatalogPeriods", parameters,connection, transaction) as SqlCommand;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static void SaveCatalogPeriod(int catalogId, int periodId, SqlConnection connection, SqlTransaction transaction)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@CatalogID", catalogId }, 
                                                                                            { "@PeriodID", periodId } };

                SqlCommand cmd = DataAccess.GetCommand("upsInsCatalogPeriod", parameters, connection, transaction) as SqlCommand;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        //Developed by BAL - CSTI - AFIN

        #region [@01 A01]

        /// <summary>
        /// Gets Three Next Periods
        /// </summary>
        /// <param name="PeriodID"></param>
        /// <returns>(Key: PeriodID, Value: Description)</returns>
        public static List<PeriodSearchData> GetThreeNextPeriods(int PeriodID)
        {
            List<PeriodSearchData> result = null;
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@PeriodID", PeriodID } };

                SqlDataReader reader = DataAccess.GetDataReader("GetThreeNextPeriods", parameters, "Commissions");

                if (reader.HasRows)
                {
                    result = new List<PeriodSearchData>();
                    while (reader.Read())
                    {
                        PeriodSearchData period = new PeriodSearchData();
                        period.PeriodID = Convert.ToInt32(reader["PeriodID"]);
                        period.PlanID = Convert.ToInt32(reader["PlanID"]);
                        period.StartDate = Convert.ToDateTime(reader["StartDate"]);
                        period.LockDate = Convert.ToDateTime(reader["LockDate"]);
                        period.EndDate = Convert.ToDateTime(reader["EndDate"]);
                        if (!Convert.IsDBNull(reader["ClosedDate"]))
                        {
                            period.ClosedDate = Convert.ToDateTime(reader["ClosedDate"]);
                        }
                        period.Description = Convert.ToString(reader["Description"]);

                        result.Add(period);
                    }
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return result;
        }



        public static List<PeriodSearchData> GetThreeNextPeriodsReactiveAccount(int PeriodID, int AccountID)
        {
            List<PeriodSearchData> result = null;
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { 
                { "@PeriodID", PeriodID } ,
                { "@AccountID", AccountID } 
                };

                SqlDataReader reader = DataAccess.GetDataReader("GetThreeNextPeriods", parameters, "Commissions");

                if (reader.HasRows)
                {
                    result = new List<PeriodSearchData>();
                    while (reader.Read())
                    {
                        PeriodSearchData period = new PeriodSearchData();
                        period.PeriodID = Convert.ToInt32(reader["PeriodID"]);
                        period.PlanID = Convert.ToInt32(reader["PlanID"]);
                        period.StartDate = Convert.ToDateTime(reader["StartDate"]);
                        period.LockDate = Convert.ToDateTime(reader["LockDate"]);
                        period.EndDate = Convert.ToDateTime(reader["EndDate"]);
                        if (!Convert.IsDBNull(reader["ClosedDate"]))
                        {
                            period.ClosedDate = Convert.ToDateTime(reader["ClosedDate"]);
                        }
                        period.Description = Convert.ToString(reader["Description"]);

                        result.Add(period);
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
        /// Gets Open Period
        /// </summary>
        /// <returns>PeriodID</returns>
        public static int GetOpenPeriodID()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Commissions"].ConnectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT dbo.sfnGetOpenPeriod()";

                    return Convert.ToInt32(cmd.ExecuteScalar());
                }

            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        #endregion

        #region 
        /*CSTI:CS:10/03/2016*/
        public static List<CatalogPeriod> GetCatalogPeriodsOfOpenPeriod()
        {
            List<CatalogPeriod> result = null;
            try
            {
                SqlDataReader reader = DataAccess.GetDataReader("upsGetCatalogPeriodsOfOpenPeriod", null, "Core");
                if (reader.HasRows)
                {
                    result = new List<CatalogPeriod>();
                    while (reader.Read())
                    {
                        result.Add(new CatalogPeriod()
                        {
                            CatalogPeriodID = Convert.ToInt32(reader["CatalogPeriodID"]),
                            CatalogID = Convert.ToInt32(reader["CatalogID"]),
                            PeriodID = Convert.ToInt32(reader["PeriodID"]),
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

        public static PeriodSearchData GetPeriodbyID(int periodoId)
        {
            PeriodSearchData result = null;
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { 
                { "@PeriodID", periodoId } 
                };

                SqlDataReader reader = DataAccess.GetDataReader("upsGetPeriodbyId", parameters, "Core");
                if (reader.HasRows)
                {
                    result = new PeriodSearchData();
                    while (reader.Read())
                    {
                        result = new PeriodSearchData
                        {
                            PeriodID = Convert.ToInt32(reader["PeriodID"]),
                            StartDate = Convert.ToDateTime(reader["StartDate"]),
                            EndDate = Convert.ToDateTime(reader["EndDate"]),
                            ClosedDate = reader["ClosedDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["ClosedDate"]),
                            PlanID = Convert.ToInt32(reader["PlanID"]),
                            EarningsViewable = reader["EarningsViewable"] == DBNull.Value ? false : Convert.ToBoolean(reader["EarningsViewable"]),
                            BackOfficeDisplayStartDate = reader["BackOfficeDisplayStartDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["BackOfficeDisplayStartDate"]),
                            DisbursementsProcessed = reader["DisbursementsProcessed"] == DBNull.Value ? false : Convert.ToBoolean(reader["DisbursementsProcessed"]),
                            Description = reader["Description"] == DBNull.Value ? "" :  reader["Description"].ToString(),
                            StartDateUTC = reader["StartDateUTC"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["StartDateUTC"]),
                            EndDateUTC = reader["EndDateUTC"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["EndDateUTC"]),
                            LockDate = Convert.ToDateTime(reader["LockDate"])
                        };
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
    }
}
