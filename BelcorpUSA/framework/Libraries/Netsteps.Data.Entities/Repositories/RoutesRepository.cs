using System;
using System.Data;
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
    public class RoutesRepository
    {
        #region Routes

        public static PaginatedList<RoutesData> Search(RouterParametros searchParams)
        {
            // Apply filters
            var routes = RoutesRepository.Search().FindAll(x => x.RouteID == (searchParams.RouteID.HasValue ? searchParams.RouteID.Value : x.RouteID));
            // Apply pagination
            IQueryable<RoutesData> matchingItems = routes.AsQueryable<RoutesData>();

            var resultTotalCount = matchingItems.Count();
            matchingItems = matchingItems.ApplyPagination(searchParams);

            return matchingItems.ToPaginatedList<RoutesData>(searchParams, resultTotalCount);
        }

        public static System.Collections.Generic.Dictionary<int, string> SearchRoutesByText(string text)
        {
            string firstValue = "";
            List<RoutesData> lenMaterial = new List<RoutesData>();
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

                    SqlDataReader dataReader = DataAccess.queryDatabase("[dbo].[spGetRoutes]", connection, parameters);

                    while (dataReader.Read())
                    {
                        lenMaterial.Add(new RoutesData()
                        {
                            RouteID = dataReader.IsDBNull(0) ? (int)0 : dataReader.GetInt32(0),
                            Name = dataReader.IsDBNull(1) ? "" : dataReader.GetString(1)
                        });
                    }
                    dataReader.Close();
                    var result = lenMaterial;
                    return result.ToDictionary(a => a.RouteID, a => a.Name);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<RoutesData> Search()
        {
            List<RoutesData> result = new List<RoutesData>();
            try
            {
                SqlDataReader reader = DataAccess.GetDataReader("upsGetRoutes", null, "Core");

                if (reader.HasRows)
                {
                    result = new List<RoutesData>();
                    while (reader.Read())
                    {
                        result.Add(new RoutesData()
                        {
                            RouteID = Convert.ToInt32(reader["RouteID"]),
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

        public static List<RoutesData> SearchRoutesZones()
        {
            List<RoutesData> result = new List<RoutesData>();
            try
            {
                SqlDataReader reader = DataAccess.GetDataReader("spGetRouteZone", null, "Core");

                if (reader.HasRows)
                {
                    //result = new List<RoutesData>();
                    while (reader.Read())
                    {
                        result.Add(new RoutesData()
                        {
                            RouteID = Convert.ToInt32(reader["RouteID"]),
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

        public static PaginatedList<ZonesData> SearchZones(RouterParametros searchParams)
        {
            var zones = RoutesRepository.SearchZones().FindAll(x => x.RouteID == (searchParams.RouteID.HasValue ? searchParams.RouteID.Value : x.RouteID));
            // Apply pagination
            IQueryable<ZonesData> matchingItems = zones.AsQueryable<ZonesData>();

            var resultTotalCount = matchingItems.Count();
            matchingItems = matchingItems.ApplyPagination(searchParams);

            return matchingItems.ToPaginatedList<ZonesData>(searchParams, resultTotalCount);
        }

        #endregion
        #region DataAcces
        public static int upDesactive(int RouteID)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@RouteID", RouteID }
                                                                                         };

                SqlCommand cmd = DataAccess.GetCommand("spDesactiveRoute", parameters, "Core") as SqlCommand;
                cmd.Connection.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        //Llenar Tabla
        public static List<ZonesData> SearchZones()
        {
            List<ZonesData> result = new List<ZonesData>();
            try
            {
                //Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@RouteID", RouteID.Value } };
                SqlDataReader reader = DataAccess.GetDataReader("spGetZones", null, "Core");

                if (reader.HasRows)
                {
                    //result = new List<RoutesData>();
                    while (reader.Read())
                    {
                        result.Add(new ZonesData()
                        {
                            RouteID = Convert.ToInt32(reader["RouteID"]),
                            GeoScopeID = Convert.ToInt32(reader["GeoScopeID"]),
                            Name = Convert.ToString(reader["Name"]),
                            Value = Convert.ToString(reader["Value"]),
                            Except = Convert.ToBoolean(reader["Except"])
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
        //Llenar combo Status
        public static List<StateProvincesData> SearchStates()
        {
            List<StateProvincesData> result = new List<StateProvincesData>();
            try
            {
                SqlDataReader reader = DataAccess.GetDataReader("spStatesProvinces", null, "Core");

                if (reader.HasRows)
                {
                    //result = new List<RoutesData>();
                    while (reader.Read())
                    {
                        result.Add(new StateProvincesData()
                        {
                            StateProvinceID = Convert.ToInt32(reader["StateProvinceID"]),
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
        //llenar combo Citys
        public static List<StateProvincesData> SearchCitys(string state)
        {
            List<StateProvincesData> result = new List<StateProvincesData>();
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@State", state }
                                                                                           };

                SqlDataReader reader = DataAccess.GetDataReader("spStatesCitys", parameters, "Core");

                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        StateProvincesData PricesPerCatalags = new StateProvincesData();
                        PricesPerCatalags.City = Convert.ToString(reader["City"]);

                        result.Add(PricesPerCatalags);
                    }
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return result;
        }
        //Insertar Zonas
        public static int InsertRoutesZones(ZonesData Zone)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>() {  //{ "@Name", Route.Name },
                                                                                            { "@Value", Zone.Value },
                                                                                            { "@Except", Zone.Except },
                                                                                            { "@ScopeLevel", Zone.Name },
                                                                                            { "@RouteID", Zone.RouteID }
                                                                                            };
            SqlCommand cmd = DataAccess.GetCommand("spInsertRoutesZone", parameters, "Core") as SqlCommand;
            try
            {
                cmd.Connection.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            finally {
                if (cmd.Connection.State == ConnectionState.Open) cmd.Connection.Close();
            }
        }

        public static int spInsertRoute(int? routeID, string nameRoute)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>() {  {"@RouteID", routeID },{"@Name", nameRoute }};
            SqlCommand cmd = DataAccess.GetCommand("spInsertRoute", parameters, "Core") as SqlCommand;
            try
            {
                cmd.Connection.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            finally {
                if (cmd.Connection.State == ConnectionState.Open) cmd.Connection.Close();
            }
        }

        //public static int spDeleteRouteScopes(int routeID)
        public static int spDeleteRouteScopes(ZonesData Zone)
        {
            //Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@RouteID", routeID } };
            //SqlCommand cmd = DataAccess.GetCommand("spDeleteRouteGeoScope", parameters, "Core") as SqlCommand;
            Dictionary<string, object> parameters = new Dictionary<string, object>() {  //{ "@Name", Route.Name },
                                                                                            { "@Value", Zone.Value },
                                                                                            { "@Except", Zone.Except },
                                                                                            { "@ScopeLevel", Zone.Name },
                                                                                            { "@RouteID", Zone.RouteID }
                                                                                            };
            SqlCommand cmd = DataAccess.GetCommand("spDeleteRouteGeoScope", parameters, "Core") as SqlCommand;
            try
            {
                cmd.Connection.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            finally
            {
                if (cmd.Connection.State == ConnectionState.Open) cmd.Connection.Close();
            }
        }

        //End
        #endregion
    }
}
