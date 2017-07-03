using System;
using System.Collections.Generic;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Data.Entities.Repositories.Interfaces;
using System.Linq;
using System.Transactions;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;

namespace NetSteps.Data.Entities.Business
{
    /// <summary>
    /// Business Logic
    /// </summary>
    public static class LogisticShippingRules
    {
        private static ShippingRateGroupBe DtoToBo(NetSteps.Data.Entities.Dto.ShippingRateGroupDto dto)
        {
            return new ShippingRateGroupBe()
            {
                ShippingRateGroupID = dto.ShippingRateGroupID,
                Name = dto.Name,
                Description = dto.Description,
                GroupCode = dto.GroupCode,
                Active = dto.Active,
                RowTotal = dto.RowTotal
            };
        }

        private static NetSteps.Data.Entities.Dto.ShippingRateGroupDto BoToDto(ShippingRateGroupBe bo)
        {
            return new NetSteps.Data.Entities.Dto.ShippingRateGroupDto()
            {
                ShippingRateGroupID = bo.ShippingRateGroupID,
                Name = bo.Name,
                Description = bo.Description,
                GroupCode = bo.GroupCode,
                Active = bo.Active,
                RowTotal = bo.RowTotal
            };
        }

        public static Dictionary<int, string> GetShippingMethods()
        {
            try
            {
                return LogisticShippingRulesRepository.GetShippingMethods();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static Dictionary<int, string> GetStatuses()
        {
            try
            {
                return new Dictionary<int, string>() { { 1, "Active" }, { 0, "Inactive" } };
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static int getOrderTypeId(int ShippingOrderTypeID)
        {
            try
            {
                return LogisticShippingRulesRepository.getOrderTypeId(ShippingOrderTypeID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            } 
        }

        public static Dictionary<int, string> GetOrderTypes()
        {
            try
            {
                return LogisticShippingRulesRepository.GetOrderTypes();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static Dictionary<int, string> GetShippingRateGroups()
        {
            try
            {
                return LogisticShippingRulesRepository.GetShippingRateGroups();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static PaginatedList<ShippingRulesLogisticsSearchData> Search(ShippingRulesLogisticsSearchParameters shippingRulesLogisticsSearchParameters)
        {
            try
            {
                return LogisticShippingRulesRepository.Search(shippingRulesLogisticsSearchParameters);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        /// <summary>
        /// KLC
        /// Export Excel
        /// </summary>
        /// <param name="shippingRulesLogisticsSearchParameters"></param>
        /// <returns></returns>
        public static List<ShippingRulesLogisticsSearchData> SearchExport(ShippingRulesLogisticsSearchParameters shippingRulesLogisticsSearchParameters)
        {
            try
            {
                var shippingRules = LogisticShippingRulesDataAccess.Search(shippingRulesLogisticsSearchParameters);

                if (shippingRules == null)
                    shippingRules = new List<ShippingRulesLogisticsSearchData>();

                return shippingRules;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
        /// <summary>
        /// Developed KLC - CSTI
        /// </summary>
        /// <param name="searchParams"></param>
        /// <returns></returns>
        public static PaginatedList<ZonesData> SearchAreaGeographical(AreaGeographicalSearchParameters param)
        {
            try
            {
                return LogisticShippingRulesRepository.SearchAreaGeographical(param);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        /// <summary>
        /// KLC - CSTI
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static PaginatedList<GeoScopesByCodesSearchData> SearchGeoScopesByCodes(GeoScopesByCodesSearchParameters param)
        {
            try
            {
                return LogisticShippingRulesRepository.SearchGeoScopesByCodes(param);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        /// <summary>
        /// KLC - CSTI
        /// Ins. Postal Codes
        /// </summary>
        /// <param name="shippingRuleId"></param>
        public static void InsertGeoScopesByCodes(GeoScopesByCodesSearchParameters parameter)
        {
            try
            {
                LogisticShippingRulesRepository.InsertGeoScopesByCodes(parameter);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
        /// <summary>
        /// KLC - CSTI
        /// </summary>
        /// <param name="parameter"></param>
        public static void DeleGeoScopesByCodes(GeoScopesByCodesSearchParameters parameter)
        {
            try
            {
                LogisticShippingRulesRepository.upDelGeoScopesByCodes(parameter);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static void DeleGeoScopesByCodesAll(int shippingOrderTypeID)
        {
            try
            {
                LogisticShippingRulesRepository.upDelGeoScopesByCodesAll(shippingOrderTypeID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        /// <summary>
        /// KLC - CSTI
        /// </summary>
        /// <param name="parameter"></param>
        public static void InsertAreaGeographical(AreaGeographicalSearchParameters parameter)
        {
            try
            {
                LogisticShippingRulesRepository.InsertAreaGeographical(parameter);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        /// <summary>
        /// KLC - CSTI
        /// </summary>
        /// <param name="parameter"></param>
        public static void DeleAreaGeographical(AreaGeographicalSearchParameters parameter)
        {
            try
            {
                LogisticShippingRulesRepository.upDelAreaGeographical(parameter);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static void DeleAreaGeographicalAll(int shippingOrderTypeID)
        {
            try
            {
                LogisticShippingRulesRepository.upDelAreaGeographicalAll(shippingOrderTypeID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static ShippingRulesLogisticsSearchData SearchByID(int id)
        {
            try
            {
                return LogisticShippingRulesRepository.SearchByID(id);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static Dictionary<int, string> ShippingRulesLookUp(string input)
        {
            try
            {
                return LogisticShippingRulesRepository.ShippingRulesLookUp(input);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static Dictionary<int, string> WarehouseLookUp(string input)
        {
            try
            {
                return LogisticShippingRulesRepository.WarehouseLookUp(input);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static Dictionary<int, string> LogisticProviderLookUp(string input)
        {
            try
            {
                return LogisticShippingRulesRepository.LogisticProviderLookUp(input);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static int SaveRule(ShippingRulesLogisticsSearchData shippingRulesLogisticsSearchData)
        {
            try
            {
                int result;
                if (shippingRulesLogisticsSearchData.ShippingOrderTypeID == 0)
                {
                    result=LogisticShippingRulesRepository.InsertRule(shippingRulesLogisticsSearchData);
                }
                else
                {
                    LogisticShippingRulesRepository.UpdateRule(shippingRulesLogisticsSearchData);
                    result = 1;
                }
                return result;
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
                LogisticShippingRulesRepository.UpdateRuleStatus(shippingRuleId);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static List<ShippingRateGroupBe> ListShippingRates(int page, int pageSize, string column, string order,
        int shippingRuleId,
        int shippingMethodId,
        int statusId,
        int warehouseId,
        int logisticProviderId)
        {
            var data = LogisticShippingRulesRepository.ListShippingRates(page, pageSize, column, order, shippingRuleId, shippingMethodId, statusId, warehouseId, logisticProviderId);
            return (from r in data select DtoToBo(r)).ToList(); ;
        }

        /// <summary>
        /// KLC - CSTI
        /// </summary>
        /// <param name="shippingRuleId"></param>
        /// <param name="shippingMethodId"></param>
        /// <param name="statusId"></param>
        /// <param name="warehouseId"></param>
        /// <param name="logisticProviderId"></param>
        /// <returns></returns>
        public static int RowCountExportListShippingRates(int shippingRuleId, int shippingMethodId, int statusId, int warehouseId, int logisticProviderId)
        {
            return LogisticShippingRulesRepository.RowCountExport(shippingRuleId, shippingMethodId, statusId, warehouseId, logisticProviderId);
        }


        public static bool ActivateDesactivateShippingRate(List<int> oListShipping, bool status)
        {
            using (var mainScope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    var result = false;
                    foreach (var item in oListShipping)
                    {
                        result = LogisticShippingRulesRepository.ActivateDesactivateShippingRate(item,status);
                        if (result == false)
                        {
                            mainScope.Dispose();
                            return false; 
                        }
                    }
                    mainScope.Complete();
                    mainScope.Dispose();
                    return true;
                }
                catch (Exception)
                {
                    mainScope.Dispose();
                    return false;
                }
            }
        }

        public static ShippingRateGroupBe GetShippingRateGroup(int shippingRateGroupID)
        {
            var data = LogisticShippingRulesRepository.GetShippingRateGroup(shippingRateGroupID);
            return (DtoToBo(data));
        }

        public static List<ShippingRateSearchData> ListShippingRate(int shippingRateGroupID)
        {
            return LogisticShippingRulesRepository.ListShippingRate(shippingRateGroupID);
        }

        public static string ValidateCurrencyName(string currencyName, int LanguageID)
        {
            return LogisticShippingRulesRepository.ValidateCurrencyName(currencyName, LanguageID);
        }

        public static Dictionary<int, string> CurrencyTermLookUp(string CurrencyTerm, int LanguageID)
        {
            return LogisticShippingRulesRepository.CurrencyTermLookUp(CurrencyTerm, LanguageID);
        }
        

        public static bool SaveShippingRate(ShippingRateGroupBe oShippingRateGroup, List<ShippingRateSearchData> oListaShippingRatesToInsert, List<int> oListShippingRateToDelete)
        {
            var transactionOptions = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted
            };
            using (var mainScope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {               
                try
                {
                    var ShippingRateGroupID = 0;
                    if (oShippingRateGroup.ShippingRateGroupID != 0)
                    {
                        ShippingRateGroupID = oShippingRateGroup.ShippingRateGroupID;
                        var result = LogisticShippingRulesRepository.UpdateShippingRateGroup(BoToDto(oShippingRateGroup));
                        if (result == false)
                        {
                            mainScope.Dispose();
                            return false;
                        }
                    }
                    else {

                        var result = LogisticShippingRulesRepository.InsertShippingRateGroup(BoToDto(oShippingRateGroup),out ShippingRateGroupID);
                        if (result == false)
                        {
                            mainScope.Dispose();
                            return false;
                       }
                    }

                    foreach (var item in oListaShippingRatesToInsert)
                    {
                        var result = LogisticShippingRulesRepository.InsertShippingRate(item, ShippingRateGroupID);
                        if (result == false)
                        {
                            mainScope.Dispose();
                            return false;
                        }
                    }
                    foreach (var item in oListShippingRateToDelete)
                    {
                        var result = LogisticShippingRulesRepository.DeletShippingRate(item);
                        if (result == false)
                        {
                            mainScope.Dispose();
                            return false;
                        }
                    }
                    mainScope.Complete();
                    mainScope.Dispose();
                    return true;
                }
                catch (Exception ex)
                {
                    mainScope.Dispose();
                    return false;
                }
            }
        }

        public static bool SaveMassiveRate(List<ShippingRateSearchData> oListaShippingRatesToInsert,int ShippingRateGroupID)
        {
            var transactionOptions = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted
            };
            using (var mainScope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                try
                {
                    foreach (var item in oListaShippingRatesToInsert)
                    {
                        var result = LogisticShippingRulesRepository.InsertShippingRate_2(item, ShippingRateGroupID);
                        if (result == false)
                        {
                            mainScope.Dispose();
                            return false;
                        }
                    }
                    mainScope.Complete();
                    mainScope.Dispose();
                    return true;
                }
                catch (Exception ex)
                {
                    mainScope.Dispose();
                    return false;
                }
            }
        }

        public static List<ScopeLevelsSearchData> ListScopeLevels()
        {
            return LogisticShippingRulesRepository.ListScopeLevels();
        }
    }
}
