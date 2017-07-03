using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Repositories.Interfaces;
using NetSteps.Data.Entities.Dto;
using System.Data;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Common.Globalization;

namespace NetSteps.Data.Entities.Repositories
{
    public static class LogisticShippingRulesRepository 
    {
        public static Dictionary<int, string> ShippingRulesLookUp(string input)
        {
            return LogisticShippingRulesDataAccess.ShippingRulesLookUp(input);
        }

        public static Dictionary<int, string> WarehouseLookUp(string input)
        {
            return LogisticShippingRulesDataAccess.WarehouseLookUp(input);
        }

        public static int getOrderTypeId(int input)
        {
            return LogisticShippingRulesDataAccess.getOrderTypeId(input);
        }
         
        public static Dictionary<int, string> LogisticProviderLookUp(string input)
        {
            return LogisticShippingRulesDataAccess.LogisticProviderLookUp(input);
        }

        public static Dictionary<int, string> GetShippingMethods()
        {
            return LogisticShippingRulesDataAccess.GetShippingMethods();
        }

        public static Dictionary<int, string> GetOrderTypes()
        {
            return LogisticShippingRulesDataAccess.GetOrderTypes();
        }

        public static Dictionary<int, string> GetShippingRateGroups()
        {
            return LogisticShippingRulesDataAccess.GetShippingRateGroups();
        }

        public static PaginatedList<ShippingRulesLogisticsSearchData> Search(ShippingRulesLogisticsSearchParameters parameters)
        {
            // Apply filters
            var shippingRules = LogisticShippingRulesDataAccess.Search(parameters);

            if (shippingRules == null)
                shippingRules = new List<ShippingRulesLogisticsSearchData>();

            // Apply pagination
            IQueryable<ShippingRulesLogisticsSearchData> matchingItems = shippingRules.AsQueryable<ShippingRulesLogisticsSearchData>();

            var resultTotalCount = matchingItems.Count();
            matchingItems = matchingItems.ApplyPagination(parameters);

            return matchingItems.ToPaginatedList<ShippingRulesLogisticsSearchData>(parameters, resultTotalCount);
        }

        public static ShippingRulesLogisticsSearchData SearchByID(int id)
        {
            return LogisticShippingRulesDataAccess.Search(new ShippingRulesLogisticsSearchParameters() { StatusID = -1, ShippingOrderTypeID = id }).First();
        }

        public static int InsertRule(ShippingRulesLogisticsSearchData shippingRulesLogisticsSearchData)
        {
            return LogisticShippingRulesDataAccess.InsertRule(shippingRulesLogisticsSearchData);
        }

        public static void UpdateRule(ShippingRulesLogisticsSearchData shippingRulesLogisticsSearchData)
        {
            LogisticShippingRulesDataAccess.UpdateRule(shippingRulesLogisticsSearchData);
        }

        public static void UpdateRuleStatus(int shippingRuleId)
        {
            LogisticShippingRulesDataAccess.UpdateRuleStatus(shippingRuleId);
        }

        /// <summary>
        /// Returns the Shipping Rates
        /// </summary>
        /// <param name="LanguageID">current language</param>
        /// <returns>A generic list of ShippingRateGroupDto class</returns>
        public static List<ShippingRateGroupDto> ListShippingRates(int page, int pageSize, string column, string order,
        int shippingRuleId,
        int shippingMethodId,
        int statusId,
        int warehouseId,
        int logisticProviderId)
        {
            var result = new List<ShippingRateGroupDto>();
            try
            {
                object[] parameters = {
                                        new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize },
                                        new SqlParameter("@PageNumber", SqlDbType.Int) { Value = page },
                                        new SqlParameter("@Colum", SqlDbType.VarChar) { Value = column },
                                        new SqlParameter("@Order", SqlDbType.VarChar) { Value = order },
                                        new SqlParameter("@Active", SqlDbType.Int) { Value = statusId },
                                        new SqlParameter("@ShippingOrderType", SqlDbType.Int) { Value = shippingRuleId },
                                        new SqlParameter("@LogisticProvider", SqlDbType.Int) { Value = logisticProviderId },
                                        new SqlParameter("@ShippingMethod", SqlDbType.Int) { Value = shippingMethodId },
                                        new SqlParameter("@Warehouse", SqlDbType.Int) { Value = warehouseId }
                                      };

                using (var DbContext = new EntityDBContext(ConnectionStrings.BelcorpCore))
                {
                    result = DbContext.Database.SqlQuery<ShippingRateGroupDto>
                            (GenerateQueryString("spSearchShippingRates", parameters), parameters).ToList();
                }

                return result;
            }
            catch (Exception) { throw new Exception("An error has occurred, please try again."); }
        }

        /// <summary>
        /// Add @ as pref of parameters
        /// </summary>
        /// <param name="query">Query or store procedure</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>string format Query @parameter ...</returns>
        private static string GenerateQueryString(string query, params object[] parameters)
        {
            if (!query.Contains("@") && parameters != null)
            {
                var parameterNames = from p in parameters select ((System.Data.SqlClient.SqlParameter)p).ParameterName;
                query = string.Format("{0} {1}", query, string.Join(", ", parameterNames));
            }

            return query;
        }

        public static bool ActivateDesactivateShippingRate(int shippingRateGroupID, bool status)
        {
            try
            {
                object[] parameters = {
                                        new SqlParameter("@ShippingRateGroupID", SqlDbType.Int) { Value = shippingRateGroupID },
                                        new SqlParameter("@Status", SqlDbType.Bit) { Value = status }                                       
                                      };

                using (var DbContext = new EntityDBContext(ConnectionStrings.BelcorpCore))
                {
                   var result = DbContext.Database.SqlQuery<int>
                            (GenerateQueryString("spActivateDesactivateShippingRates", parameters), parameters).ToList();
                }

                return  true;
            }
            catch (Exception) {
                throw new Exception("An error has occurred, please try again.");
            }
        }

        public static ShippingRateGroupDto GetShippingRateGroup(int shippingRateGroupID)
        {
            try
            {
                var obj = new ShippingRateGroupDto();
                object[] parameters = {
                                        new SqlParameter("@ShippingRateGroupID", SqlDbType.Int) { Value = shippingRateGroupID }                                     
                                      };

                using (var DbContext = new EntityDBContext(ConnectionStrings.BelcorpCore))
                {
                    obj = DbContext.Database.SqlQuery<ShippingRateGroupDto>
                             (GenerateQueryString("spGetShippingRateGroup", parameters), parameters).First();
                }
                return obj;
            }
            catch (Exception)
            {
                throw new Exception("An error has occurred, please try again.");
            }
        }

        //public static ShippingRateGroupDto GetShippingRateGroup(int shippingRateGroupID)
        //{
        //    try
        //    {
        //        var obj = new ShippingRateGroupDto();
        //        object[] parameters = {
        //                                new SqlParameter("@ShippingRateGroupID", SqlDbType.Int) { Value = shippingRateGroupID }                                     
        //                              };

        //        using (var DbContext = new EntityDBContext(ConnectionStrings.BelcorpCore))
        //        {
        //            obj = DbContext.Database.SqlQuery<ShippingRateGroupDto>
        //                     (GenerateQueryString("spGetShippingRateGroup", parameters), parameters).First();
        //        }
        //        return obj;
        //    }
        //    catch (Exception)
        //    {
        //        throw new Exception("An error has occurred, please try again.");
        //    }
        //}

        public static List<ShippingRateSearchData> ListShippingRate(int shippingRateGroupID)
        {
            try
            {
                var oList = new List<ShippingRateSearchData>();
                object[] parameters = {                                           
                                            new SqlParameter("@ShippingRateGroupID", SqlDbType.Int) { Value = shippingRateGroupID }
                                      };

                using (var DbContext = new EntityDBContext(ConnectionStrings.BelcorpCore))
                {
                    oList = DbContext.Database.SqlQuery<ShippingRateSearchData>
                             (GenerateQueryString("spListShippingRates", parameters), parameters).ToList();
                }
                return oList;
            }
            catch (Exception)
            {
                throw new Exception("An error has occurred, please try again.");
            }
        }

        public static string ValidateCurrencyName(string currencyName, int LanguageID)
        {
            try
            {
                var obj = 0;
                object[] parameters = {
                                        new SqlParameter("@CurrencyName", SqlDbType.VarChar) { Value = currencyName },
                                        new SqlParameter("@LanguageID", SqlDbType.Int) { Value = LanguageID }
                                      };

                using (var DbContext = new EntityDBContext(ConnectionStrings.BelcorpCore))
                {
                    obj = DbContext.Database.SqlQuery<int>
                             (GenerateQueryString("spValidateCurrency", parameters), parameters).First();
                }
                return obj.ToString();
            }
            catch (Exception)
            {
                throw new Exception("An error has occurred, please try again.");
            }
        }

        public static Dictionary<int, string> CurrencyTermLookUp(string CurrencyTerm, int LanguageID )
        {
            Dictionary<int, string> result = null;
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@CurrencyTerm", CurrencyTerm }, { "@LanguageID", LanguageID } };

                SqlDataReader reader = DataAccess.GetDataReader("spCurrencyLookUp", parameters, "Core");

                if (reader.HasRows)
                {
                    result = new Dictionary<int, string>();
                    while (reader.Read())
                    {
                        result.Add(Convert.ToInt32(reader["CurrencyID"]), Convert.ToString(reader["CurrencyTerm"]));
                    }
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return result;
        }

        public static bool InsertShippingRate(ShippingRateSearchData objShipping, int ShippingRateGroupID)
        {
            try
            {
                var obj = 0;
                object[] parameters = {
                                        new SqlParameter("@ShippingRateGroupID", SqlDbType.Int) {Value = ShippingRateGroupID },
                                        new SqlParameter("@Currency", SqlDbType.VarChar) { Value = objShipping.Currency },
                                        new SqlParameter("@ValueName", SqlDbType.VarChar) { Value = objShipping.ValueName },
                                        new SqlParameter("@ValueFrom", SqlDbType.Decimal) { Value = objShipping.ValueFrom },
                                        new SqlParameter("@ValueTo", SqlDbType.Decimal) { Value = objShipping.ValueTo },
                                        new SqlParameter("@ShippingAmount", SqlDbType.Decimal) { Value = objShipping.ShippingAmount }
                                      };

                using (var DbContext = new EntityDBContext(ConnectionStrings.BelcorpCore))
                {
                    obj = DbContext.Database.SqlQuery<int>
                             (GenerateQueryString("spInsertShippingRate", parameters), parameters).First();
                }
                return true;
            }
            catch (Exception)
            {
                throw new Exception("An error has occurred, please try again.");
            }
        }

        public static bool InsertShippingRate_2(ShippingRateSearchData objShipping, int ShippingRateGroupID)
        {
            try
            {
                var obj = 0;
                object[] parameters = {
                                        new SqlParameter("@ShippingRateGroupID", SqlDbType.Int) {Value = ShippingRateGroupID },
                                        new SqlParameter("@CurrencyCode", SqlDbType.VarChar) { Value = objShipping.CurrencyCode },
                                        new SqlParameter("@ValueName", SqlDbType.VarChar) { Value = objShipping.ValueName },
                                        new SqlParameter("@ValueFrom", SqlDbType.Decimal) { Value = objShipping.ValueFrom },
                                        new SqlParameter("@ValueTo", SqlDbType.Decimal) { Value = objShipping.ValueTo },
                                        new SqlParameter("@ShippingAmount", SqlDbType.Decimal) { Value = objShipping.ShippingAmount }
                                      };

                using (var DbContext = new EntityDBContext(ConnectionStrings.BelcorpCore))
                {
                    obj = DbContext.Database.SqlQuery<int>
                             (GenerateQueryString("spInsertShippingRate_2", parameters), parameters).First();
                }
                return true;
            }
            catch (Exception)
            {
                throw new Exception("An error has occurred, please try again.");
            }
        }

        public static bool DeletShippingRate(int ShippingRateID)
        {
            try
            {
                var obj = 0;
                object[] parameters = {
                                        new SqlParameter("@ShippingRateID", SqlDbType.Int) { Value = ShippingRateID }                                       
                                      };

                using (var DbContext = new EntityDBContext(ConnectionStrings.BelcorpCore))
                {
                    obj = DbContext.Database.SqlQuery<int>
                             (GenerateQueryString("spDeleteShippingRate", parameters), parameters).First();
                }
                return true;
            }
            catch (Exception)
            {
                throw new Exception("An error has occurred, please try again.");
            }
        }

        public static bool InsertShippingRateGroup(ShippingRateGroupDto objShippingRateGroup, out int ShippingRateGroupID)
        {
            try
            {
                var obj = 0;
                object[] parameters = {
                                        new SqlParameter("@Name", SqlDbType.VarChar) {Value = objShippingRateGroup.Name },
                                        new SqlParameter("@Description", SqlDbType.VarChar) { Value = objShippingRateGroup.Description },
                                        new SqlParameter("@GroupCode", SqlDbType.VarChar) { Value = objShippingRateGroup.GroupCode },
                                        new SqlParameter("@Active", SqlDbType.Bit) { Value = objShippingRateGroup.Active }                                        
                                      };

                using (var DbContext = new EntityDBContext(ConnectionStrings.BelcorpCore))
                {
                    obj = DbContext.Database.SqlQuery<int>
                             (GenerateQueryString("spInsertShippingRateGroup", parameters), parameters).First();
                    ShippingRateGroupID = obj;
                }
                return true;
            }
            catch (Exception)
            {
                throw new Exception("An error has occurred, please try again.");
            }
        }

        public static bool UpdateShippingRateGroup(ShippingRateGroupDto objShippingRateGroup)
        {
            try
            {
                var obj = 0;
                object[] parameters = {
                                        new SqlParameter("@ShippingRateGroupID", SqlDbType.Int) {Value = objShippingRateGroup.ShippingRateGroupID},
                                        new SqlParameter("@Name", SqlDbType.VarChar) {Value = objShippingRateGroup.Name },
                                        new SqlParameter("@Description", SqlDbType.VarChar) { Value = objShippingRateGroup.Description },
                                        new SqlParameter("@GroupCode", SqlDbType.VarChar) { Value = objShippingRateGroup.GroupCode },
                                        new SqlParameter("@Active", SqlDbType.Bit) { Value = objShippingRateGroup.Active }                                        
                                      };

                using (var DbContext = new EntityDBContext(ConnectionStrings.BelcorpCore))
                {
                    obj = DbContext.Database.SqlQuery<int>
                             (GenerateQueryString("spUpdateShippingRateGroup", parameters), parameters).First();
                }
                return true;
            }
            catch (Exception)
            {
                throw new Exception("An error has occurred, please try again.");
            }
        }

        public static List<ScopeLevelsSearchData> ListScopeLevels()
        {
            try
            {
                var oList = new List<ScopeLevelsSearchData>();
                object[] parameters = {};

                using (var DbContext = new EntityDBContext(ConnectionStrings.BelcorpCore))
                {
                    oList = DbContext.Database.SqlQuery<ScopeLevelsSearchData>
                             (GenerateQueryString("spListScopeLevels", parameters), parameters).ToList();
                }
                return oList;
            }
            catch (Exception)
            {
                throw new Exception("An error has occurred, please try again.");
            }
        }

        /// <summary>
        /// Developed By KLC
        /// Rows Count - Export
        /// </summary>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        public static int RowCountExport(int shippingRuleId,int shippingMethodId,int statusId,int warehouseId,int logisticProviderId)
        {
            try
            {
                return DataAccess.ExecWithStoreProcedureSaveIdentity("Core", "spSearchShippingRates2",
                new SqlParameter("Active", SqlDbType.Int) { Value = statusId },
                new SqlParameter("ShippingOrderType", SqlDbType.Int) { Value = shippingRuleId },
                new SqlParameter("LogisticProvider", SqlDbType.Int) { Value = logisticProviderId },
                new SqlParameter("ShippingMethod", SqlDbType.Int) { Value = shippingMethodId },
                new SqlParameter("Warehouse", SqlDbType.Int) { Value = warehouseId }
                );
            }
            catch (Exception ex)
            {
                throw new Exception(Translation.GetTerm("An error has occurred, please try again."));
            }
            
        }

        /// <summary>
        /// KLC - CSTI
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static PaginatedList<ZonesData> SearchAreaGeographical(AreaGeographicalSearchParameters param)
        {
            object RowsCount;
            List<ZonesData> paginatedResult = DataAccess.ExecWithStoreProcedureListParam<ZonesData>("Core", "spGetAreaGeographical", out RowsCount,
                new SqlParameter("ShippingOrderTypeID", SqlDbType.Int) { Value = param.ShippingOrderTypeID }
                ).ToList();

            IQueryable<ZonesData> matchingItems = paginatedResult.AsQueryable<ZonesData>();

            var resultTotalCount = matchingItems.Count();
            matchingItems = matchingItems.ApplyPagination(param);

            return matchingItems.ToPaginatedList<ZonesData>(param, resultTotalCount);
        }
        /// <summary>
        /// KLC - CSTI
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static PaginatedList<GeoScopesByCodesSearchData> SearchGeoScopesByCodes(GeoScopesByCodesSearchParameters param)
        {
            object RowsCount;
            List<GeoScopesByCodesSearchData> paginatedResult = DataAccess.ExecWithStoreProcedureListParam<GeoScopesByCodesSearchData>("Core", "spGetShippingOrderTypesGeoScopesByCodes", out RowsCount,
                new SqlParameter("ShippingOrderTypeID", SqlDbType.Int) { Value = param.ShippingOrderTypeID }
                ).ToList();

            IQueryable<GeoScopesByCodesSearchData> matchingItems = paginatedResult.AsQueryable<GeoScopesByCodesSearchData>();

            var resultTotalCount = matchingItems.Count();
            matchingItems = matchingItems.ApplyPagination(param);

            return matchingItems.ToPaginatedList<GeoScopesByCodesSearchData>(param, resultTotalCount);
        }

        /// <summary>
        /// KLC - CSTI
        /// Save Geo Scopes By Codes
        /// </summary>
        /// <param name="parameter"></param>
        public static void InsertGeoScopesByCodes(GeoScopesByCodesSearchParameters parameter)
        {
            var result = DataAccess.ExecWithStoreProcedureSave("Core", "spInsertGeoScopesByCodes",
                new SqlParameter("ShippingOrderTypeID", SqlDbType.Int) { Value = parameter.ShippingOrderTypeID },
                new SqlParameter("ShippingOrderTypesGeoScopesByCodeID", SqlDbType.Int) { Value = parameter.ShippingOrderTypesGeoScopesByCodeID },
                new SqlParameter("ValueFrom", SqlDbType.VarChar) { Value = parameter.ValueFrom },
                new SqlParameter("ValueTo", SqlDbType.VarChar) { Value = parameter.ValueTo },
                new SqlParameter("Except", SqlDbType.Bit) { Value = parameter.Except }               
                );
        }

        /// <summary>
        /// KLC - CSTI
        /// Delete GeoScopes By Codes
        /// </summary>
        /// <param name="parameter"></param>
        public static void upDelGeoScopesByCodes(GeoScopesByCodesSearchParameters parameter)
        {
            var result = DataAccess.ExecWithStoreProcedureSave("Core", "spDeleteGeoScopesByCodes",
                new SqlParameter("ShippingOrderTypesGeoScopesByCodeID", SqlDbType.Int) { Value = parameter.ShippingOrderTypesGeoScopesByCodeID }
                );
        }

        public static void upDelGeoScopesByCodesAll(int shippingOrderTypeID)
        {
            var result = DataAccess.ExecWithStoreProcedureSave("Core", "spDeleteGeoScopesByCodesAll",
                new SqlParameter("ShippingOrderTypeID", SqlDbType.Int) { Value = shippingOrderTypeID }
                );
        }

        /// <summary>
        /// KLC - CSTI
        /// Save Zones
        /// </summary>
        /// <param name="parameter"></param>
        public static void InsertAreaGeographical(AreaGeographicalSearchParameters parameter)
        {
            var result = DataAccess.ExecWithStoreProcedureSave("Core", "spInsertAreaGeographical",
                new SqlParameter("ShippingOrderTypeID", SqlDbType.Int) { Value = parameter.ShippingOrderTypeID },
                new SqlParameter("GeoScopeID", SqlDbType.Int) { Value = parameter.GeoScopeID },
                new SqlParameter("Value", SqlDbType.VarChar) { Value = parameter.Value },
                new SqlParameter("ScopeLevel", SqlDbType.VarChar) { Value = parameter.Name },
                new SqlParameter("Except", SqlDbType.Bit) { Value = parameter.Except }
                );
        }

        /// <summary>
        /// KLC - CSTI
        /// </summary>
        /// <param name="parameter"></param>
        public static void upDelAreaGeographical(AreaGeographicalSearchParameters parameter)
        {
            var result = DataAccess.ExecWithStoreProcedureSave("Core", "spDeleteAreaGeographical",
                new SqlParameter("GeoScopeID", SqlDbType.Int) { Value = parameter.GeoScopeID }
                );
        }

        public static void upDelAreaGeographicalAll(int shippingOrderTypeID)
        {
            var result = DataAccess.ExecWithStoreProcedureSave("Core", "spDeleteAreaGeographicalAll",
                new SqlParameter("ShippingOrderTypeID", SqlDbType.Int) { Value = shippingOrderTypeID }
                );
        }

    }

    class LogisticShippingRulesDataAccess
    {
        public static List<ShippingRulesLogisticsSearchData> Search(ShippingRulesLogisticsSearchParameters searchParams)
        {
            List<ShippingRulesLogisticsSearchData> result = null;
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@SelectedShippingRuleID", searchParams.ShippingOrderTypeID }, 
                                                                                            { "@SelectedShippingMethodID", searchParams.ShippingMethodID },
                                                                                            { "@SelectedStatus", searchParams.StatusID },
                                                                                            { "@SelectedWarehouseID", searchParams.WarehouseID },
                                                                                            { "@SelectedLogisticsProviderID", searchParams.LogisticsProviderID }};

                SqlDataReader reader = DataAccess.GetDataReader("upsGetShippingRules", parameters, "Core");

                if (reader.HasRows)
                {
                    result = new List<ShippingRulesLogisticsSearchData>();
                    while (reader.Read())
                    {
                        result.Add(new ShippingRulesLogisticsSearchData()
                        {
                            ShippingOrderTypeID = Convert.ToInt32(reader["ShippingOrderTypeID"]),
                            ShippingRuleName = Convert.ToString(reader["Name"]),
                            WarehouseID = Convert.ToInt32(reader["WarehouseID"]),
                            Warehouse = Convert.ToString(reader["Warehouse"]),
                            ShippingMethodID = Convert.ToInt32(reader["ShippingMethodID"]),
                            ShippingMethod = Convert.ToString(reader["ShippingMethod"]),
                            LogisticProviderID = Convert.ToInt32(reader["LogisticsProviderID"]),
                            LogisticProvider = Convert.ToString(reader["LogisticProvider"]),
                            Status = Convert.ToBoolean(reader["Active"]),
                            CountryID = Convert.ToInt32(reader["CountryID"]),
                            ShippingRateGroupID = Convert.ToInt32(reader["ShippingRateGroupID"]),
                            DaysForDelivey = Convert.ToInt32(reader["DaysForDelivery"])
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

        public static Dictionary<int, string> ShippingRulesLookUp(string input)
        {
            Dictionary<int, string> result = null;
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@firstValue", input } };

                SqlDataReader reader = DataAccess.GetDataReader("upsShippingRulesLookUp", parameters, "Core");

                if (reader.HasRows)
                {
                    result = new Dictionary<int, string>();
                    while (reader.Read())
                    {
                        result.Add(Convert.ToInt32(reader["ShippingOrderTypeID"]), Convert.ToString(reader["DisplayText"]));
                    }
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return result;
        }

        public static Dictionary<int, string> WarehouseLookUp(string input)
        {
            Dictionary<int, string> result = null;
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@firstValue", input } };

                SqlDataReader reader = DataAccess.GetDataReader("upsWarehouseLookUp", parameters, "Core");

                if (reader.HasRows)
                {
                    result = new Dictionary<int, string>();
                    while (reader.Read())
                    {
                        result.Add(Convert.ToInt32(reader["WarehouseID"]), Convert.ToString(reader["DisplayText"]));
                    }
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return result;
        }

        public static Dictionary<int, string> LogisticProviderLookUp(string input)
        {
            Dictionary<int, string> result = null;
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@firstValue", input } };

                SqlDataReader reader = DataAccess.GetDataReader("upsLogisticProviderLookUp", parameters, "Core");

                if (reader.HasRows)
                {
                    result = new Dictionary<int, string>();
                    while (reader.Read())
                    {
                        result.Add(Convert.ToInt32(reader["LogisticsProviderID"]), Convert.ToString(reader["DisplayText"]));
                    }
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return result;
        }

        public static Dictionary<int, string> GetShippingMethods()
        {
            Dictionary<int, string> result = null;
            try
            {
                SqlDataReader reader = DataAccess.GetDataReader("upsGetShippingMethods", null, "Core");

                if (reader.HasRows)
                {
                    result = new Dictionary<int, string>();
                    while (reader.Read())
                    {
                        result.Add(Convert.ToInt32(reader["ShippingMethodID"]), Convert.ToString(reader["Name"]));
                    }
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return result;
        }

        public static Dictionary<int, string> GetOrderTypes()
        {
            Dictionary<int, string> result = null;
            try
            {
                SqlDataReader reader = DataAccess.GetDataReader("upsGetOrderTypes", null, "Core");

                if (reader.HasRows)
                {
                    result = new Dictionary<int, string>();
                    while (reader.Read())
                    {
                        result.Add(Convert.ToInt32(reader["OrderTypeID"]), Convert.ToString(reader["Name"]));
                    }
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return result;
        }


        public static int getOrderTypeId(int ShippingOrderTypeID)
        {
            return   DataAccess.ExecWithStoreProcedureScalarType<int>("Core", "uspGetOrderTypes",
                new SqlParameter("ShippingOrderTypeID", SqlDbType.Int) { Value = ShippingOrderTypeID }
                ); 
        }

        public static Dictionary<int, string> GetShippingRateGroups()
        {
            Dictionary<int, string> result = null;
            try
            {
                SqlDataReader reader = DataAccess.GetDataReader("upsGetShippingRateGroup", null, "Core");

                if (reader.HasRows)
                {
                    result = new Dictionary<int, string>();
                    while (reader.Read())
                    {
                        result.Add(Convert.ToInt32(reader["ShippingRateGroupID"]), Convert.ToString(reader["Name"]));
                    }
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return result;
        }

        public static int InsertRule(ShippingRulesLogisticsSearchData shippingRules)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {  
                                                                                            { "@ShippingRuleName", shippingRules.ShippingRuleName },
                                                                                            { "@CountryID", shippingRules.CountryID },
                                                                                            { "@OrderTypeID", shippingRules.OrderTypeID },
                                                                                            { "@ShippingMethodID", shippingRules.ShippingMethodID },
                                                                                            { "@WarehouseID", shippingRules.WarehouseID },
                                                                                            { "@LogisticProviderID", shippingRules.LogisticProviderID },
                                                                                            { "@DaysForDelivey", shippingRules.DaysForDelivey },
                                                                                            { "@ShippingRateGroupID", shippingRules.ShippingRateGroupID },
                                                                                            { "@IsDefaultShippingMethod", shippingRules.IsDefaultShippingMethod },
                                                                                            { "@AllowDirectShipments", shippingRules.AllowDirectShipments },
                                                                                            { "@Active", shippingRules.Status } };

                SqlCommand cmd = DataAccess.GetCommand("upsInsertRule", parameters, "Core") as SqlCommand;
                cmd.Connection.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static void UpdateRule(ShippingRulesLogisticsSearchData shippingRules)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@ShippingOrderTypeID", shippingRules.ShippingOrderTypeID }, 
                                                                                            { "@ShippingRuleName", shippingRules.ShippingRuleName },
                                                                                            { "@CountryID", shippingRules.CountryID },
                                                                                            { "@OrderTypeID", shippingRules.OrderTypeID },
                                                                                            { "@ShippingMethodID", shippingRules.ShippingMethodID },
                                                                                            { "@WarehouseID", shippingRules.WarehouseID },
                                                                                            { "@LogisticProviderID", shippingRules.LogisticProviderID },
                                                                                            { "@DaysForDelivey", shippingRules.DaysForDelivey },
                                                                                            { "@ShippingRateGroupID", shippingRules.ShippingRateGroupID },
                                                                                            { "@IsDefaultShippingMethod", shippingRules.IsDefaultShippingMethod } };

                SqlCommand cmd = DataAccess.GetCommand("upsUpdateRule", parameters, "Core") as SqlCommand;
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static void UpdateRuleStatus(int shippingRuleId)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ShippingOrderTypeID", shippingRuleId } };

                SqlCommand cmd = DataAccess.GetCommand("upsUpdateRuleStatus", parameters, "Core") as SqlCommand;
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
    }
}
