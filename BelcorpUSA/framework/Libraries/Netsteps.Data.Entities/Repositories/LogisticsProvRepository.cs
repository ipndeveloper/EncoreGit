using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Extensions;
using System.Data.SqlClient;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Repositories
{
    public class LogisticsProvRepository
    {
        #region Provider
        public static PaginatedList<LogisticsProviderSearData> SearchProvider(LogisticsProvParameters searchParams)
        {
            // Apply filters
            var provider = LogisticsProvRepository.SearchProviders(searchParams);
            // Apply pagination
            IQueryable<LogisticsProviderSearData> matchingItems = provider.AsQueryable<LogisticsProviderSearData>();

            var resultTotalCount = matchingItems.Count();
            matchingItems = matchingItems.ApplyPagination(searchParams);

            return matchingItems.ToPaginatedList<LogisticsProviderSearData>(searchParams, resultTotalCount);
        }

         
        public static PaginatedList<OrderLogisticProviderSearchData> SearchOrderProvider(OrderLogisticProvParameters searchParams)
        {
            // Apply filters
            var provider = LogisticsProvRepository.SearchOrdersProvider(searchParams);
            // Apply pagination
            IQueryable<OrderLogisticProviderSearchData> matchingItems = provider.AsQueryable<OrderLogisticProviderSearchData>();

            var resultTotalCount = matchingItems.Count();
            matchingItems = matchingItems.ApplyPagination(searchParams);

            return matchingItems.ToPaginatedList<OrderLogisticProviderSearchData>(searchParams, resultTotalCount);
        }
        //llenar lokoup
        public static System.Collections.Generic.Dictionary<int, string> SearchProviderByText(string text)
        {
            string firstValue = "";
            List<LogisticsProviderSearData> lenMaterial = new List<LogisticsProviderSearData>();
            try
            {

                firstValue = (text);
                using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Core"].ConnectionString))
                {

                    connection.Open();
                    var parameters = new SqlParameter[] 
                                                        {
                                                        new SqlParameter("@firstValue", firstValue )
                                                        };

                    SqlDataReader dataReader = DataAccess.queryDatabase("[dbo].[spGetProv]", connection, parameters);

                    while (dataReader.Read())
                    {
                        lenMaterial.Add(new LogisticsProviderSearData()
                        {
                            LogisticsProviderID = dataReader.IsDBNull(0) ? (int)0 : dataReader.GetInt32(0),
                            Name = dataReader.IsDBNull(1) ? "" : dataReader.GetString(1)
                        });

                    }

                    dataReader.Close();
                    var result = lenMaterial;
                    return result.ToDictionary(a => a.LogisticsProviderID, a => a.Name);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region DataAcces
        
        #region Browse Provider
        /// <summary>
        /// Developed By KLC - CSTI
        /// BR-LG-002
        /// </summary>
        /// <returns></returns>
        public static List<LogisticsProviderSearData> SearchProviders(LogisticsProvParameters param)
            {
                List<LogisticsProviderSearData> result = new List<LogisticsProviderSearData>();
                try
                {
                    Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@LogisticsProviderID",param.LogisticsProviderID },
                                                                                                { "@Name",param.Name },
                                                                                                { "@Active",param.Active }
                                                                                                };

                    SqlDataReader reader = DataAccess.GetDataReader("spBrowseProv", parameters, "Core");

                    if (reader.HasRows)
                    {
                        //result = new List<RoutesData>();
                        while (reader.Read())
                        {
                            result.Add(new LogisticsProviderSearData()
                            {
                                LogisticsProviderID = Convert.ToInt32(reader["LogisticsProviderID"]),                            
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

        public static List<LogisticsProviderSearData> SearchProviders1()
        {
            List<LogisticsProviderSearData> result = new List<LogisticsProviderSearData>();
            try
            {

                SqlDataReader reader = DataAccess.GetDataReader("spBrowseProvidersAll", null, "Core");

                if (reader.HasRows)
                {
                    //result = new List<RoutesData>();
                    while (reader.Read())
                    {
                        result.Add(new LogisticsProviderSearData()
                        {
                            LogisticsProviderID = Convert.ToInt32(reader["LogisticsProviderID"]),
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

        /// <summary>
        /// Created by KTC
        /// </summary>
        /// <param name="searchParams"></param>
            /// <returns>List</returns>
            public static List<OrderLogisticProviderSearchData> SearchOrdersProvider(OrderLogisticProvParameters searchParams)
            {
                List<OrderLogisticProviderSearchData> result = new List<OrderLogisticProviderSearchData>();
                try
                {


                    Dictionary<string, object> parameters = new Dictionary<string, object>() {  
                                                                                            { "@OrderNumber", searchParams.OrderNumber }, 
                                                                                            { "@DateStart", searchParams.DateStart==null?"1/1/2000" : searchParams.DateStart.ToDateTime().ToShortDateString()},
                                                                                            { "@DateEnd", searchParams.DateEnd==null?"1/1/2000" : searchParams.DateEnd.ToDateTime().ToShortDateString() },
                                                                                            // { "@DateStart", null},
                                                                                            //{ "@DateEnd", null },
                                                                                            { "@LogisticProviderID", searchParams.LogisticProviderID },
                                                                                            { "@PeriodID", searchParams.PeriodID }};

                    SqlDataReader reader = DataAccess.GetDataReader("upsGetOrderLogisticProvider", parameters, "Core");

                    if (reader.HasRows)
                    {
                        result = new List<OrderLogisticProviderSearchData>();
                        while (reader.Read())
                        {
                            result.Add(new OrderLogisticProviderSearchData()
                            {
                                OrderShipmentID = Convert.ToInt32(reader["OrderShipmentID"]),
                                OrderNumber = Convert.ToInt32(reader["OrderNumber"]),
                                LogisticProviderName = Convert.ToString(reader["LogisticProviderName"]),
                                OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                                OrderTotal = Convert.ToString(reader["OrderTotal"])
                               
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

            //Seccion Insert
            //Desactivar Logistics Prov.
            public static int upDesactiveProv(int LogisticsProviderID,int Active)
            {
                try
                {

                    Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@LogisticsProviderID", LogisticsProviderID },
                                                                                                { "@Active", Active }
                                                                                                };
                
                    SqlCommand cmd = DataAccess.GetCommand("spDesactiveProvider", parameters, "Core") as SqlCommand;
                    cmd.Connection.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
                }
            }
            
            //Delete Documents
            public static int upDeleteDocument(int IDTypeID)
            {
                try
                {

                    Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@IDTypeID", IDTypeID }
                                                                                                };

                    SqlCommand cmd = DataAccess.GetCommand("spDeleteDocuments", parameters, "Core") as SqlCommand;
                    cmd.Connection.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
                }
            }


            public static int upChangeLogisticProvider(OrderLogisticProvParameters param)
            {
                try
                {
                     
                    Dictionary<string, object> parameters = new Dictionary<string, object>() {  
                                                                                                { "@OrderShipmentID", param.OrderShipmentID },
                                                                                                { "@LogisticsProviderID", param.LogisticProviderID }
                                                                                                };

                    SqlCommand cmd = DataAccess.GetCommand("upsChangeLogistiProviderByOrderShipmentID", parameters, "Core") as SqlCommand;
                    cmd.Connection.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
                }
            }
        
            #endregion
        #region Details
            #region Insert
            public static int upInsertDetails(LogisticsProviderSearData detail)
                    {
                        try
                        {

                            Dictionary<string, object> parameters = new Dictionary<string, object>() {  
                                                                                                     { "@LogisticsProviderID", detail.LogisticsProviderID },
                                                                                                     //{ "@AddressID", detail.AddressID },
                                                                                                     { "@Name", detail.Name },
                                                                                                     { "@PhoneNumber", detail.PhoneNumber},
                                                                                                     { "@FaxNumber", detail.FaxNumber },
                                                                                                     { "@EmailAddress", detail.EmailAddress },
                                                                                                     { "@TermName", detail.TermName },
                                                                                                     { "@Description", detail.Description },
                                                                                                     { "@Active", detail.Active },
                                                                                                     { "@MarketID", detail.MarketID },
                                                                                                     { "@ExternalCode", detail.ExternalCode },
                                                                                                     { "@WorkInSaturdays", detail.WorkInSaturdays },
                                                                                                     { "@WorkInSundays", detail.WorkInSundays },
                                                                                                     { "@WorkInHolidays", detail.WorkInHolidays },
                                                                                                     { "@ExternalTrakingURL", detail.ExternalTrakingURL }
                                                                                                     };

                            SqlCommand cmd = DataAccess.GetCommand("spInsLogProviders", parameters, "Core") as SqlCommand;
                            cmd.Connection.Open();
                            return Convert.ToInt32(cmd.ExecuteScalar());
                        }
                        catch (Exception ex)
                        {
                            throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
                        }
                    }
                #endregion
            #region ViewDatos Provider
            public static List<LogisticsProviderSearData> SearDetails()
                {
                    List<LogisticsProviderSearData> result = new List<LogisticsProviderSearData>();
                    try
                    {
                        //Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@RouteID", RouteID.Value } };
                        SqlDataReader reader = DataAccess.GetDataReader("spGetLogProvDetails", null, "Core");

                        if (reader.HasRows)
                        {
                            //result = new List<RoutesData>();
                            while (reader.Read())
                            {
                                result.Add(new LogisticsProviderSearData()
                                {
                                    LogisticsProviderID = Convert.ToInt32(reader["LogisticsProviderID"]),
                                    Name = Convert.ToString(reader["Name"]),
                                    Active = Convert.ToBoolean(reader["Active"]),
                                    //AddressID = Convert.ToInt32(reader["AddressID"]),
                                    PhoneNumber = Convert.ToString(reader["PhoneNumber"]),
                                    FaxNumber = Convert.ToString(reader["FaxNumber"]),
                                    EmailAddress = Convert.ToString(reader["EmailAddress"]),
                                    TermName = Convert.ToString(reader["TermName"]),
                                    Description = Convert.ToString(reader["Description"]),
                                    MarketID = Convert.ToInt32(reader["MarketID"]),
                                    ExternalCode = Convert.ToString(reader["ExternalCode"]),
                                    WorkInSaturdays = Convert.ToBoolean(reader["WorkInSaturdays"]),
                                    WorkInSundays = Convert.ToBoolean(reader["WorkInSundays"]),
                                    WorkInHolidays = Convert.ToBoolean(reader["WorkInHolidays"]),
                                    ExternalTrakingURL = Convert.ToString(reader["ExternalTrakingURL"])
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
        #endregion
        #region Documents
        public static PaginatedList<LogisticProviderSuppliedIDs> SearchDocuments(LogisticsProvParameters searchParams)
        {
            var documents = LogisticsProvRepository.SearchDocuments().FindAll(x => x.LogisticsProviderID == (searchParams.LogisticsProviderID.HasValue ? searchParams.LogisticsProviderID.Value : x.LogisticsProviderID));
            // Apply pagination
            IQueryable<LogisticProviderSuppliedIDs> matchingItems = documents.AsQueryable<LogisticProviderSuppliedIDs>();

            var resultTotalCount = matchingItems.Count();
            matchingItems = matchingItems.ApplyPagination(searchParams);

            return matchingItems.ToPaginatedList<LogisticProviderSuppliedIDs>(searchParams, resultTotalCount);
        }


        public static List<LogisticProviderSuppliedIDs> SearchDocuments()
        {
            List<LogisticProviderSuppliedIDs> result = new List<LogisticProviderSuppliedIDs>();
            try
            {
               
                SqlDataReader reader = DataAccess.GetDataReader("spGetDocumentos", null, "Core");

                if (reader.HasRows)
                {                  
                    while (reader.Read())
                    {
                        result.Add(new LogisticProviderSuppliedIDs()
                        {
                            LogisticsProviderID = Convert.ToInt32(reader["LogisticsProviderID"]),
                            Name = Convert.ToString(reader["Name"]),
                            IDValue = Convert.ToString(reader["IDValue"]),
                            IDExpeditionDate = Convert.ToDateTime(reader["IDExpeditionDate"]),
                            ExpeditionEntity = Convert.ToString(reader["ExpeditionEntity"]),
                            IsPrimaryID = Convert.ToBoolean(reader["IsPrimaryID"]),
                            IDTypeID = Convert.ToInt32(reader["IdTypeID"])
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

        public static List<LogisticProviderSuppliedIDs> SearchDocumenttypes()
        {
            List<LogisticProviderSuppliedIDs> result = new List<LogisticProviderSuppliedIDs>();
            try
            {

                SqlDataReader reader = DataAccess.GetDataReader("spGetDocTypes", null, "Core");

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result.Add(new LogisticProviderSuppliedIDs()
                        {
                            IDTypeID = Convert.ToInt32(reader["IDTypeID"]),
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

        public static int upInsertDocuments(LogisticProviderSuppliedIDs document)
        {
            try
            {

                Dictionary<string, object> parameters = new Dictionary<string, object>() {
                                                                                                 { "@DocumentType", document.IDTypeID },
                                                                                                 { "@LogisticsProviderID", document.LogisticsProviderID },
                                                                                                 { "@DocumentNumber", document.IDValue },
                                                                                                 { "@ExpDate", document.IDExpeditionDate },
                                                                                                 { "@ExpEntity", document.ExpeditionEntity },
                                                                                                 { "@Primary", document.IsPrimaryID }  
                                                                                                 };

                SqlCommand cmd = DataAccess.GetCommand("spInsLogProvSuppliedIDs", parameters, "Core") as SqlCommand;
                cmd.Connection.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }


        #endregion
        #region Address
        #endregion
        #region Routes
            #region Get Routes
                public static PaginatedList<RoutesLogProvSearchData> SearchRoutesProv(LogisticsProvParameters searchParams)
                {
                    var routesprov = LogisticsProvRepository.SearchRoutesProv().FindAll(x => x.LogisticsProviderID == (searchParams.LogisticsProviderID.HasValue ? searchParams.LogisticsProviderID.Value : x.LogisticsProviderID));
                    // Apply pagination
                    IQueryable<RoutesLogProvSearchData> matchingItems = routesprov.AsQueryable<RoutesLogProvSearchData>();

                    var resultTotalCount = matchingItems.Count();
                    matchingItems = matchingItems.ApplyPagination(searchParams);

                    return matchingItems.ToPaginatedList<RoutesLogProvSearchData>(searchParams, resultTotalCount);
                }

                public static List<RoutesLogProvSearchData> SearchRoutesProv()
                {
                    List<RoutesLogProvSearchData> result = new List<RoutesLogProvSearchData>();
                    try
                    {

                        SqlDataReader reader = DataAccess.GetDataReader("spGetRoutesProv", null, "Core");

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                result.Add(new RoutesLogProvSearchData()
                                {
                                    LogisticsProviderID = Convert.ToInt32(reader["LogisticsProviderID"]),                                    
                                    RouteID = Convert.ToInt32(reader["RouteID"]),
                                    Name = Convert.ToString(reader["Name"]),
                                    Monday = Convert.ToBoolean(reader["Monday"]),
                                    Tuesday = Convert.ToBoolean(reader["Tuesday"]),
                                    Wednesday = Convert.ToBoolean(reader["Wednesday"]),
                                    Thursday = Convert.ToBoolean(reader["Thursday"]),
                                    Friday = Convert.ToBoolean(reader["Friday"]),
                                    Saturday = Convert.ToBoolean(reader["Saturday"]),
                                    Sunday = Convert.ToBoolean(reader["Sunday"])
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
            #region Insert Routes Prov. Log.
                public static int upInsertRoute(RoutesLogProvSearchData routesprov)
                {
                    try
                    {

                        Dictionary<string, object> parameters = new Dictionary<string, object>() {
                                                                                                 { "@RouteID", routesprov.RouteID },
                                                                                                 { "@LogisticsProviderID", routesprov.LogisticsProviderID },
                                                                                                 { "@Monday", routesprov.Monday },
                                                                                                 { "@Tuesday", routesprov.Tuesday },
                                                                                                 { "@Wednesday", routesprov.Wednesday },
                                                                                                 { "@Thursday", routesprov.Thursday },
                                                                                                 { "@Friday", routesprov.Friday },
                                                                                                 { "@Saturday", routesprov.Saturday },
                                                                                                 { "@Sunday", routesprov.Sunday }
                                                                                                 };

                        SqlCommand cmd = DataAccess.GetCommand("spInsertRoutesProvLog", parameters, "Core") as SqlCommand;
                        cmd.Connection.Open();
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {
                        throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
                    }
                }
            #endregion

                #region Delete Routes Prov. Log.
                public static int upDeleteRoute(int RouteID, int LogisticsProviderID)
                {
                    try
                    { 

                        Dictionary<string, object> parameters = new Dictionary<string, object>() {
                                                                                                 { "@RouteID", RouteID },
                                                                                                 { "@LogisticsProviderID", LogisticsProviderID }
                                                                                                 };

                        SqlCommand cmd = DataAccess.GetCommand("upsDeleteRoutesProvLog", parameters, "Core") as SqlCommand;
                        cmd.Connection.Open();
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {
                        throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
                    }
                }
                #endregion
        #endregion

        #endregion
    }
}
