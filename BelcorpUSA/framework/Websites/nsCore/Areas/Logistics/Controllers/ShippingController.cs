using System;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Business.Controllers;
using NetSteps.Web.Mvc.Extensions;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using System.Web;
using Excel;
using System.Data;
using System.Text.RegularExpressions;
using NetSteps.Web.Mvc.Helpers;

namespace nsCore.Areas.Logistics.Controllers
{
    public class ShippingController : BaseController
    {
        #region Shipping Rules
        
        //
        // GET: /Logistics/Shipping/Rules

        public ActionResult Rules()
        {
            ShippingRulesLogisticsSearchParameters parameters = new ShippingRulesLogisticsSearchParameters();
            // 0: Inactive, 1: Active
            parameters.StatusID = -1;

            TempData["ShippingMethods"] = LogisticShippingRules.GetShippingMethods();
            TempData["ShippingStatuses"] = LogisticShippingRules.GetStatuses();

            return View(parameters);
        }

        

        [OutputCache(CacheProfile = "PagedGridData")]
        public virtual ActionResult GetRules(int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection,
            int shippingRuleId,
            int shippingMethodId,
            int statusId,
            int warehouseId,
            int logisticProviderId)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                int count = 0;

                var shippingRules = LogisticShippingRules.Search(new ShippingRulesLogisticsSearchParameters()
                {
                    ShippingOrderTypeID = shippingRuleId,
                    ShippingMethodID = shippingMethodId,
                    StatusID = statusId,
                    WarehouseID = warehouseId,
                    LogisticsProviderID = logisticProviderId,
                    PageIndex = page,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    OrderByDirection = orderByDirection
                });

                foreach (var shippingRule in shippingRules)
                {
                    builder.Append("<tr>");
                    builder.AppendCheckBoxCell(value: shippingRule.ShippingOrderTypeID.ToString(), name: "");
                    builder
                        .AppendLinkCell("~/Logistics/Shipping/RuleDetail/" + shippingRule.ShippingOrderTypeID, Translation.GetTerm(shippingRule.ShippingOrderTypeID.ToString(), shippingRule.ShippingOrderTypeID.ToString()))
                        .AppendLinkCell("~/Logistics/Shipping/RuleDetail/" + shippingRule.ShippingOrderTypeID, Translation.GetTerm(shippingRule.ShippingRuleName, shippingRule.ShippingRuleName))
                        .AppendCell(shippingRule.Warehouse)
                        .AppendCell(shippingRule.ShippingMethod)
                        .AppendLinkCell("~/Logistics/Logistics/ProviderDetails/" + shippingRule.LogisticProviderID, Translation.GetTerm(shippingRule.LogisticProvider, shippingRule.LogisticProvider))
                        .AppendCell(shippingRule.Status ? Translation.GetTerm("Active", "Active") : Translation.GetTerm("Inactive", "Inactive"))
                        .Append("</tr>");
                    ++count;
                }

                var emptyResults = "<tr><td colspan=\"5\">" + Translation.GetTerm("ThereAreNotShippingRules", "There are not shipping rules") + "</td></tr>";

                return Json(new
                {
                    result = true,
                    totalPages = shippingRules.TotalPages,
                    page = shippingRules.TotalCount == 0 ? emptyResults : builder.ToString()
                });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "AutoCompleteData")]
        public virtual ActionResult ShippingRulesLookUp(string query)
        {
            try
            {
                var shippingRules = LogisticShippingRules.ShippingRulesLookUp(query);

                if (shippingRules == null)
                {
                    return Json(new { result = false, message = "No results" });
                }
                else
                {
                    return Json(DictionaryExtensions.ToAJAXSearchResults(shippingRules));
                }
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "AutoCompleteData")]
        public virtual ActionResult WarehouseLookUp(string query)
        {
            try
            {
                var warehouses = LogisticShippingRules.WarehouseLookUp(query);

                if (warehouses == null)
                {
                    return Json(new { result = false, message = "No results" });
                }
                else
                {
                    return Json(DictionaryExtensions.ToAJAXSearchResults(warehouses));
                }
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "AutoCompleteData")]
        public virtual ActionResult LogisticProviderLookUp(string query)
        {
            try
            {
                var logisticProviders = LogisticShippingRules.LogisticProviderLookUp(query);

                if (logisticProviders == null)
                {
                    return Json(new { result = false, message = "No results" });
                }
                else
                {
                    return Json(DictionaryExtensions.ToAJAXSearchResults(logisticProviders));
                }
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "AutoCompleteData")]
        public virtual ActionResult CurrencyLookUp(string query)
        {
            try
            {
                var warehouses = LogisticShippingRules.CurrencyTermLookUp(query, CoreContext.CurrentLanguageID);

                if (warehouses == null)
                {
                    return Json(new { result = false, message = "No results" });
                }
                else
                {
                    return Json(DictionaryExtensions.ToAJAXSearchResults(warehouses));
                }
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
        public virtual ActionResult ChangeStatus(List<int> items, bool active)
        {
            if (items != null && items.Count > 0)
            {
                try
                {
                    Plan.ChangeStatusShippingOrderTypes(items, active);
                }
                catch (Exception ex)
                {
                    var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                    return Json(new { result = false, message = exception.PublicMessage });
                }
            }
            return Json(new { result = true });
        }

        //
        // GET: /Logistics/Shipping/RuleDetail

        public ActionResult RuleDetail(int? id)
        {
            try
            {
                var shippingRule = id.HasValue ? LogisticShippingRules.SearchByID(id.Value) : new ShippingRulesLogisticsSearchData();
                int orderTypesId = LogisticShippingRules.getOrderTypeId(id.HasValue ? id.Value : 0);

                #region TempData 

                TempData["OrderTypes"] = from orderTypes in LogisticShippingRules.GetOrderTypes()
                                    select new SelectListItem()
                                    {
                                        Text = orderTypes.Value,
                                        Value = orderTypes.Key.ToString(),
                                        Selected = orderTypes.Key == orderTypesId ? true : false
                                    };

                TempData["ShippingMethods"] = from shippingMethod in LogisticShippingRules.GetShippingMethods()
                                         select new SelectListItem()
                                         {
                                             Text = shippingMethod.Value,
                                             Value = shippingMethod.Key.ToString(),
                                             Selected = shippingMethod.Key == shippingRule.ShippingMethodID ? true : false
                                         };
                
                TempData["ShippingRateGroups"] = from shippingRateGroup in LogisticShippingRules.GetShippingRateGroups()
                                              select new SelectListItem()
                                              {
                                                  Text = shippingRateGroup.Value,
                                                  Value = shippingRateGroup.Key.ToString(),
                                                  Selected = shippingRateGroup.Key == shippingRule.ShippingRateGroupID ? true : false
                                              };
                #endregion

                return View(shippingRule);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult SaveRule(
            int shippingRuleId,
            string name,
            int countryId,
            int orderTypeId,
            int shippingMethodId,
            int warehouseId,
            int logisticProviderId,
            int daysForDelivery,
            int shippingRateGroupId,
            bool isDefaultShippingMethod)
        {
            try
            {
                var shippingRule = shippingRuleId != 0 ? LogisticShippingRules.SearchByID(shippingRuleId) : new ShippingRulesLogisticsSearchData();

                shippingRule.ShippingRuleName = name;
                shippingRule.CountryID = countryId;
                shippingRule.OrderTypeID = orderTypeId;
                shippingRule.ShippingMethodID = shippingMethodId;
                shippingRule.WarehouseID = warehouseId;
                shippingRule.LogisticProviderID = logisticProviderId;
                shippingRule.DaysForDelivey = daysForDelivery;
                shippingRule.ShippingRateGroupID = shippingRateGroupId;
                shippingRule.IsDefaultShippingMethod = isDefaultShippingMethod;
                shippingRule.AllowDirectShipments = true;
                shippingRule.Status = shippingRuleId != 0 ? shippingRule.Status : true;

                int result;
                result=LogisticShippingRules.SaveRule(shippingRule);
                if (shippingRuleId == 0)
                    shippingRule.ShippingOrderTypeID = result;

                return Json(new { result = true, shippingRuleId = shippingRule.ShippingOrderTypeID });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public virtual ActionResult ToggleStatus(int Id)
        {
            try
            {
                LogisticShippingRules.UpdateRuleStatus(Id);
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public ActionResult DeliveryZone(int? id)
        {
            var shippingRule = id.HasValue ? LogisticShippingRules.SearchByID(id.Value) : new ShippingRulesLogisticsSearchData();
            ViewData["StatesProvinces"] = Routes.SearchStates();

            ViewData["AreaGeographical"] = LogisticShippingRules.SearchAreaGeographical(new AreaGeographicalSearchParameters()
                {
                    ShippingOrderTypeID = id.Value
                });

            ViewData["PostalCodes"] = LogisticShippingRules.SearchGeoScopesByCodes(new GeoScopesByCodesSearchParameters()
            {
                ShippingOrderTypeID = id.Value
            });

            return View(shippingRule);
        }

        [OutputCache(CacheProfile = "PagedGridData")]
        public virtual ActionResult GetRuleZones(int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection, int id)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                int count = 0;

                var shippingRules = LogisticShippingRules.Search(new ShippingRulesLogisticsSearchParameters()
                {
                    ShippingOrderTypeID = id,
                    StatusID = -1,
                    PageIndex = page,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    OrderByDirection = orderByDirection
                });

                foreach (var shippingRule in shippingRules)
                {
                    builder.Append("<tr>");
                    builder.AppendCheckBoxCell(value: shippingRule.ShippingOrderTypeID.ToString(), name: "");
                    builder
                        //.AppendLinkCell("~/Logistics/Shipping/RuleDetail/" + shippingRule.ShippingOrderTypeID, shippingRule.ShippingOrderTypeID.ToString())
                        //.AppendLinkCell("~/Logistics/Shipping/RuleDetail/" + shippingRule.ShippingOrderTypeID, shippingRule.ShippingRuleName)
                        //.AppendCell(shippingRule.Warehouse)
                        //.AppendCell(shippingRule.ShippingMethod)
                        //.AppendLinkCell("~/Logistics/Providers/Edit/" + shippingRule.LogisticProviderID, shippingRule.LogisticProvider)
                        //.AppendCell(shippingRule.Status ? Translation.GetTerm("Active", "Active") : Translation.GetTerm("Inactive", "Inactive"))
                        .AppendCell(shippingRule.Warehouse)
                        .AppendCell(shippingRule.ShippingMethod)
                        .Append("</tr>");
                    ++count;
                }

                var emptyResults = "<tr><td colspan=\"5\">There are no shipping rules</td></tr>";

                return Json(new
                {
                    result = true,
                    totalPages = shippingRules.TotalPages,
                    page = shippingRules.TotalCount == 0 ? emptyResults : builder.ToString()
                });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        #endregion

        #region Shipping Rutes

        [OutputCache(CacheProfile = "PagedGridData")]
        public virtual ActionResult GetRutes(int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection,
            int shippingRuleId,
            int shippingMethodId,
            int statusId,
            int warehouseId,
            int logisticProviderId)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                var order = (orderByDirection.ToString().ToUpper().StartsWith("ASC")) ? "asc" : "desc";
                int rowCount = 0;
                var result = LogisticShippingRules.ListShippingRates(page, pageSize, orderBy, order, shippingRuleId, shippingMethodId, statusId, warehouseId, logisticProviderId);
                
                foreach (var shippingRule in result)
                {
                    builder.Append("<tr>");
                    builder.AppendCheckBoxCell(value: shippingRule.ShippingRateGroupID.ToString(), name: "");
                    builder
                        .AppendLinkCell("~/Logistics/Shipping/RateDetail/" + shippingRule.ShippingRateGroupID, shippingRule.ShippingRateGroupID.ToString())
                        .AppendLinkCell("~/Logistics/Shipping/RateDetail/" + shippingRule.ShippingRateGroupID, shippingRule.Name)
                        .AppendCell(shippingRule.Description)
                        .AppendCell(shippingRule.GroupCode)
                        .AppendCell(shippingRule.Active == true ? Translation.GetTerm("Active", "Active") : Translation.GetTerm("Inactive", "Inactive"))                        
                        .Append("</tr>");
                    rowCount = int.Parse(shippingRule.RowTotal);
                }

                decimal totalPages = (Convert.ToDecimal(rowCount) / Convert.ToDecimal(pageSize));
                totalPages = Math.Floor(totalPages) == 0 ? 1 : (Math.Floor(totalPages) == Math.Ceiling(totalPages) ? Math.Floor(totalPages) : Math.Floor(totalPages) + 1);

                return Json(new { totalPages = totalPages, page = builder.ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public ActionResult Rates()
        {
            ShippingRulesLogisticsSearchParameters parameters = new ShippingRulesLogisticsSearchParameters();
            // 0: Inactive, 1: Active
            parameters.StatusID = -1;

            TempData["ShippingMethods"] = LogisticShippingRules.GetShippingMethods();
            TempData["ShippingStatuses"] = LogisticShippingRules.GetStatuses();

            
            //    
            return View(parameters);
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
        public virtual ActionResult getCountRowExportRates(int shippingRuleId, int shippingMethodId, int statusId, int warehouseId, int logisticProviderId)
        {
            try
            {
                int RowCountTotal = LogisticShippingRules.RowCountExportListShippingRates(shippingRuleId, shippingMethodId, statusId, warehouseId, logisticProviderId);
                return Json(new { result = true, RowCountTotal = RowCountTotal });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public virtual ActionResult ChangeStatusRate(List<int> items, bool active)
        {
            if (items != null && items.Count > 0)
            {
                try
                {
                    var result = LogisticShippingRules.ActivateDesactivateShippingRate(items, active);
                    if (result == false)
                    {
                         var exception = EntityExceptionHelper.GetAndLogNetStepsException(null , NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                        return Json(new { result = false, message = exception.PublicMessage });
                    }
                }
                catch (Exception ex)
                {
                    var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                    return Json(new { result = false, message = exception.PublicMessage });
                }
            }
            return Json(new { result = true });
        }

        //public ActionResult RuteDetail(int? id)
        //{
        //    try
        //    {
        //        var shippingRule = id.HasValue ? LogisticShippingRules.GetShippingRateGroup(id.Value) : new ShippingRateGroupBe();
        //        return View(shippingRule);
        //    }
        //    catch (Exception ex)
        //    {
        //        var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
        //        throw exception;
        //    }
        //}

        public virtual string ListShippingRate(string shippingRateGroupID)
        {
            try
            {
                var result = LogisticShippingRules.ListShippingRate(int.Parse(shippingRateGroupID));
                string json = JsonConvert.SerializeObject(result);
                return json;
            }
            catch (Exception) { return "{ 'result' = false }"; }
        }

        public virtual string ValidateCurrency(string currencyName)
        {
            try
            {
                var result = LogisticShippingRules.ValidateCurrencyName(currencyName,CoreContext.CurrentLanguageID);
                string json = JsonConvert.SerializeObject(result);
                return json;
            }
            catch (Exception) { return "{ 'result' = 0 }"; }
        }

        public ActionResult RateDetail(int? id)
        {
            try
            {
                var shippingRule = id.HasValue ? LogisticShippingRules.GetShippingRateGroup(id.Value) : new ShippingRateGroupBe();
                return View(shippingRule);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        public ActionResult SaveRateDetail(string newShipping, string newShippingRates, string shippingRateToDelete)
        {
            try
            {
                var oShippingRateGroup = GetShippingRateGroup(newShipping);
                var dataNew = GetListShippingToInsert(newShippingRates);
                var dataToDelete = GetListShippingToDelete(shippingRateToDelete);
                var result = LogisticShippingRules.SaveShippingRate(oShippingRateGroup, dataNew, dataToDelete);

                if (result)
                    return Json(new { result = true });
                else
                {
                    var exception = EntityExceptionHelper.GetAndLogNetStepsException(null, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                    return Json(new { result = false, message = exception.PublicMessage });
                }

            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        private List<int> GetListShippingToDelete(string valor)
        {
            var datos = JsonConvert.DeserializeObject<List<int>>(valor);
            return datos;
        }

        private List<ShippingRateSearchData> GetListShippingToInsert(string valor)
        {
            var datos = JsonConvert.DeserializeObject<List<ShippingRateSearchData>>(valor);
            return datos;
        }

        private ShippingRateGroupBe GetShippingRateGroup(string valor)
        {
            var datos = JsonConvert.DeserializeObject<ShippingRateGroupBe>(valor);
            return datos;
        }

        public virtual ActionResult AddNewRate(int id)
        {
            try
            {
                var shippingRule = LogisticShippingRules.GetShippingRateGroup(id);
                return View(shippingRule);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        [HttpPost]
        public ActionResult LoadExcelFile()
        {
            string message = "";
            HttpPostedFileBase file = null;
            List<ShippingRateSearchData> oListShippingRates = new List<ShippingRateSearchData>();
            oListShippingRates = null;
            try
            {
                if (Request.Files.Count > 0) { file = Request.Files[0]; }
                if (file != null)
                {
                    string extension = System.IO.Path.GetExtension(file.FileName);
                    if (extension.Equals(".xlsx") || extension.Equals(".xls"))
                    {
                        if (GetListShippingRates(file, extension, out oListShippingRates, out message) == false)
                        {
                            return Json(new { result = false, message = message });
                        }
                        else { return Json(new { result = true, message = message, data = oListShippingRates }); }
                    }
                    else { return Json(new { result = false, message = Translation.GetTerm("YouMustSelectAExcelFile", "You must select a excel file") }); }
                }
                else {
                    return Json(new { result = false, message = Translation.GetTerm("YouMustSelectAFile", "You must select a file") });
                }                
            }
            catch (Exception)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(null, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }           
        }

        private static bool GetListShippingRates(HttpPostedFileBase file, string fileExtension, out List<ShippingRateSearchData> oListShippingRates, out string message)
        {
            oListShippingRates = new List<ShippingRateSearchData>();
            message = "";
            try
            {
                using (IExcelDataReader reader = (fileExtension.Equals(".xlsx")) ? ExcelReaderFactory.CreateOpenXmlReader(file.InputStream) : ExcelReaderFactory.CreateBinaryReader(file.InputStream))
                {
                    reader.IsFirstRowAsColumnNames = true;
                    DataSet dataset = reader.AsDataSet();
                    DataTable dt = dataset.Tables[0];

                    if (ValidateExcelData(dt, out message) == true)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            ShippingRateSearchData oShippingRate = new ShippingRateSearchData();
                            oShippingRate.ValueName = dr["NAME"].ToString().Trim();
                            oShippingRate.ValueFrom = decimal.Parse(dr["VALUEFROM"].ToString().Trim());
                            oShippingRate.ValueTo = decimal.Parse(dr["VALUETO"].ToString());
                            oShippingRate.ShippingAmount = decimal.Parse(dr["SHIPPINGAMOUNT"].ToString());
                            oShippingRate.CurrencyCode = dr["CURRENCY"].ToString().Trim();
                            oListShippingRates.Add(oShippingRate);
                        }
                        return true;
                    }
                    else {
                        return false;
                    }
                }
            }
            catch (Exception) { message += Translation.GetTerm("AnErrorHasOcurredInDataValidation", "An error has ocurred in data validation"); return false; }
        }

        private static bool ValidateExcelData(DataTable dt, out string message)
        {
            message = "";
            try 
	        {                                
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dataRow in dt.Rows)
                    {
                        var index = 0;
                        foreach (DataColumn dataColumn in dt.Columns)
                        {
                            if (index >= 1 && index <= 3)
                            {                                
                                if (IsValidateDecimal(dataRow[dataColumn].ToString()) == false)
                                {
                                    message += Translation.GetTerm("ThisFieldMustContainDecimalNumber", "This field must contain decimal number");
                                    return false;
                                }
                            }
                            index += 1;
                        }
                    }
                    return true;
                }
                else {
                    message += Translation.GetTerm("TheFielHasNotDataToProcess", "The fiel has not data to process");
                    return false;
                }                                
	        }
	        catch (Exception)
	        {
                message += Translation.GetTerm("AnErrorHasOcurredInDataValidation", "An error has ocurred in data validation");
                return false;
	        }
        }

        public static bool IsValidateDecimal(string input)
        {
            try
            {
                var valor = float.Parse(input);
                return true;
            }
            catch (Exception)
            {
                return false;
            }            
        }

        public ActionResult SaveMassiveRate(string arrShippingRates, string ShippingRateGroupID)
        {
            var dataNew = GetListShippingToInsert(arrShippingRates);
            var result = LogisticShippingRules.SaveMassiveRate(dataNew, int.Parse(ShippingRateGroupID));

            if (result)
                return Json(new { result = true });
            else
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(null, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public ActionResult DownloadTemplate()
        {
            var fileName = Translation.GetTerm("ShippingRatesTemplate", "Shipping rates template");
            return new ExcelResult
            {
                FileName = (fileName == "" ? "-" : fileName) + ".xlsx",
                Path = "~/FileUploads/Templates/ShippingRatesTemplate.xlsx"
            };
        }

        #endregion

        public virtual string ListScopeLevels()
        {
            try
            {
                var result = LogisticShippingRules.ListScopeLevels();
                string json = JsonConvert.SerializeObject(result);
                return json;
            }
            catch (Exception) { return "{ 'result' = false }"; }
        }

        public virtual ActionResult ExportShippingRules(
            string shippingRuleId,
            string shippingMethodId,
            string statusId,
            string warehouseId,
            string logisticProviderId)
        {
            try
            {                
                shippingRuleId = (shippingRuleId == "" ? "0" : shippingRuleId.Split(' ')[0].ToString());
                warehouseId = (warehouseId == "" ? "0" : warehouseId.Split(' ')[0].ToString());
                logisticProviderId = (logisticProviderId == "" ? "0" : logisticProviderId.Split(' ')[0].ToString());

                var lstResultado = LogisticShippingRules.SearchExport(new ShippingRulesLogisticsSearchParameters()
                {
                    ShippingOrderTypeID = int.Parse(shippingRuleId),
                    ShippingMethodID = int.Parse(shippingMethodId),
                    StatusID = int.Parse(statusId),
                    WarehouseID = int.Parse(warehouseId),
                    LogisticsProviderID = int.Parse(logisticProviderId)
                });
                                
                string fileNameSave = string.Format("{0}.xlsx", Translation.GetTerm("ShippingRules", "Shipping Rules"));

                var columns = new Dictionary<string, string>
				{                   
                    {"ShippingOrderTypeID", Translation.GetTerm("Shipping Rule Code")} ,
                    {"ShippingRuleName", Translation.GetTerm("Name")} ,
                    {"Warehouse", Translation.GetTerm("Warehouse")} ,
                    {"ShippingMethod", Translation.GetTerm("Shipping Method")} ,
                    {"LogisticProvider", Translation.GetTerm("Logistic Provider")} ,
                    {"Status", Translation.GetTerm("Status")}
				};

                return new NetSteps.Web.Mvc.ActionResults.ExcelResult<ShippingRulesLogisticsSearchData>(fileNameSave, lstResultado, columns, null, columns.Keys.ToArray());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }

        }

        public virtual ActionResult ExportShippingRates(int page, int pageSize, string orderBy, string orderByDirection,
            int shippingRuleId,
            int shippingMethodId,
            int statusId,
            int warehouseId,
            int logisticProviderId)
            {
            try
            {
                var order = (orderByDirection.ToString().ToUpper().StartsWith("ASC")) ? "asc" : "desc";                
                var lstResultado = LogisticShippingRules.ListShippingRates(page, pageSize, orderBy, order, shippingRuleId, shippingMethodId, statusId, warehouseId, logisticProviderId); 
                
                string fileNameSave = string.Format("{0}.xlsx", Translation.GetTerm("ShippingRates", "Shipping Rates"));

                var columns = new Dictionary<string, string>
				{                   
                    {"ShippingRateGroupID", Translation.GetTerm("ShippingRateCode", "Shipping Rate Code")} ,
                    {"Name", Translation.GetTerm("Name", "Name")} ,
                    {"Description", Translation.GetTerm("Description", "Description")} ,
                    {"GroupCode", Translation.GetTerm("ExternalCode", "External Code")} ,
                    {"Active", Translation.GetTerm("Status", "Status")}
				};

                return new NetSteps.Web.Mvc.ActionResults.ExcelResult<ShippingRateGroupBe>(fileNameSave, lstResultado, columns, null, columns.Keys.ToArray());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }

        }

        /// <summary>
        /// Developed By KLC
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="orderByDirection"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [OutputCache(CacheProfile = "PagedGridData")]
        public virtual ActionResult GetAreaGeographical(int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection,
            int? id)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                int count = 0;

                var zones = LogisticShippingRules.SearchAreaGeographical(new AreaGeographicalSearchParameters()
                {
                    ShippingOrderTypeID = id.HasValue ? id.Value : 0,
                    PageIndex = page,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    OrderByDirection = orderByDirection
                });

                foreach (var zone in zones)
                {
                    builder.Append("<tr>");
                    builder.AppendCheckBoxCell(value: zone.GeoScopeID.ToString(), name: "GeoScopesCodeID");
                    builder
                        .AppendCell(zone.Name)
                        .AppendCell(zone.Value)
                        .AppendCell(zone.Except.ToString())
                        .Append("</tr>");
                    ++count;
                }

                return Json(new { result = true, totalPages = zones.TotalPages, page = zones.TotalCount == 0 ? "<tr id='NotItems'><td colspan=\"7\">" + Translation.GetTerm("NotZones", "There are no Zones.") + " </td></tr>" : builder.ToString() });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "PagedGridData")]
        public virtual ActionResult GetPostalCodeRanges(int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection,
            int? id)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                int count = 0;

                var postalcodes = LogisticShippingRules.SearchGeoScopesByCodes(new GeoScopesByCodesSearchParameters()
                {
                    ShippingOrderTypeID = id.HasValue ? id.Value : 0,
                    PageIndex = page,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    OrderByDirection = orderByDirection
                });

                foreach (var postalcode in postalcodes)
                {
                    builder.Append("<tr>");
                    builder.AppendCheckBoxCell(value: postalcode.ShippingOrderTypesGeoScopesByCodeID.ToString(), name: "ShippingOrderTypesGeoScopesByCodeID");
                    builder
                        .AppendCell(postalcode.ValueFrom)
                        .AppendCell(postalcode.ValueTo)
                        .AppendCell(postalcode.Except.ToString())
                        .Append("</tr>");
                    ++count;
                }

                return Json(new { result = true, totalPages = postalcodes.TotalPages, page = postalcodes.TotalCount == 0 ? "<tr id='NotItemsPostalCodes'><td colspan=\"4\">" + Translation.GetTerm("NotPostalCodes", "There are no Postal Codes.") + " </td></tr>" : builder.ToString() });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public virtual ActionResult SearchPostalCode(string postalCode)
        {
            try
            {
                return Json(new
                {
                    result = true,
                    postalcode = (NetSteps.Data.Entities.TaxCache.SearchPostalCode(postalCode)),

                });
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        /// <summary>
        /// KLC - CSTI
        /// Save Postal Code Ranges
        /// </summary>
        /// <param name="postalcodes"></param>
        /// <param name="postalcodesdel"></param>
        /// <returns></returns>
        public virtual ActionResult SavePostalCodes(List<GeoScopesByCodesSearchParameters> postalcodes, List<GeoScopesByCodesSearchParameters> postalcodesdel, int shippingOrderTypeID)
        {
            try
            {
                LogisticShippingRules.DeleAreaGeographicalAll(shippingOrderTypeID);
                if (postalcodesdel != null)
                {
                    foreach (var postalDel in postalcodesdel)
                    {                       
                        var postalcoderangesDele = new GeoScopesByCodesSearchParameters
                        {
                            ShippingOrderTypesGeoScopesByCodeID = postalDel.ShippingOrderTypesGeoScopesByCodeID
                        };
                        LogisticShippingRules.DeleGeoScopesByCodes(postalcoderangesDele);
                    }
                }
                if (postalcodes != null)
                {
                    foreach (var postalcode in postalcodes)
                    {
                        if (postalcode.ValueFrom != "null")
                        {
                            var postalcoderanges = new GeoScopesByCodesSearchParameters
                            {
                                ShippingOrderTypeID = postalcode.ShippingOrderTypeID,
                                ShippingOrderTypesGeoScopesByCodeID = postalcode.ShippingOrderTypesGeoScopesByCodeID,
                                ValueFrom = postalcode.ValueFrom.ToString(),
                                ValueTo = postalcode.ValueTo.ToString(),
                                Except = Convert.ToBoolean(postalcode.Except)
                            };
                            LogisticShippingRules.InsertGeoScopesByCodes(postalcoderanges);
                        }
                    }
                }
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public virtual ActionResult SaveAreaGeographical(List<AreaGeographicalSearchParameters> AreasGeographical, List<AreaGeographicalSearchParameters> areaGeographicalDel,
            int shippingOrderTypeID)
        {
            try
            {
                LogisticShippingRules.DeleGeoScopesByCodesAll(shippingOrderTypeID);
                if (areaGeographicalDel != null)
                {
                    foreach (var areaGeolDel in areaGeographicalDel)
                    {                       
                        var areasDele = new AreaGeographicalSearchParameters
                        {
                            GeoScopeID = areaGeolDel.GeoScopeID
                        };
                        LogisticShippingRules.DeleAreaGeographical(areasDele);
                    }
                }
                if (AreasGeographical != null)
                {
                    foreach (var areaGeo in AreasGeographical)
                    {
                        if (areaGeo.Name != "null" && areaGeo.Value != "null")
                        {
                            var areasGeographical = new AreaGeographicalSearchParameters
                            {
                                GeoScopeID = areaGeo.GeoScopeID,
                                ShippingOrderTypeID = areaGeo.ShippingOrderTypeID,
                                Value = areaGeo.Value.ToString(),
                                Name = areaGeo.Name.ToString(),
                                Except = Convert.ToBoolean(areaGeo.Except)
                            };
                            LogisticShippingRules.InsertAreaGeographical(areasGeographical);
                        }
                    }
                }
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        /// <summary>
        ///  Excel Export Postal Code
        /// </summary>
        /// <returns>Excel File</returns>
        public virtual ActionResult ExportPostalCodes(int shippingOrderTypeID)
        {
            try
            {

                var lstResultado = LogisticShippingRules.SearchGeoScopesByCodes(new GeoScopesByCodesSearchParameters()
                {
                    ShippingOrderTypeID = shippingOrderTypeID
                });

                string fileNameSave = string.Format("{0}.xlsx", Translation.GetTerm("PostalCodes", "Postal Codes") + '_' + shippingOrderTypeID.ToString());

                var columns = new Dictionary<string, string>
				{
                    {"ValueFrom", Translation.GetTerm("PostalCodeFrom","Postal Code From")} ,
                    {"ValueTo", Translation.GetTerm("PostalCodeTo", "Postal Code To")} ,
                    {"Except", Translation.GetTerm("Except", "Except")} 
				};

                return new NetSteps.Web.Mvc.ActionResults.ExcelResult<GeoScopesByCodesSearchData>(fileNameSave, lstResultado, columns, null, columns.Keys.ToArray());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }

        }

        /// <summary>
        /// KLC - CSTI
        /// </summary>
        /// <param name="shippingOrderTypeID"></param>
        /// <returns></returns>
        public virtual ActionResult ExportAreasgeographic(int shippingOrderTypeID)
        {
            try
            {

                var lstResultado = LogisticShippingRules.SearchAreaGeographical(new AreaGeographicalSearchParameters()
                {
                    ShippingOrderTypeID = shippingOrderTypeID
                });

                string fileNameSave = string.Format("{0}.xlsx", Translation.GetTerm("AreaGeographical", "Area Geographical") +'_' + shippingOrderTypeID.ToString());

                var columns = new Dictionary<string, string>
				{
                    {"Name", Translation.GetTerm("AreaLevel", "Area Level")} ,
                    {"Value", Translation.GetTerm("Name", "Name")} ,
                    {"Except", Translation.GetTerm("Except", "Except")} 
				};

                return new NetSteps.Web.Mvc.ActionResults.ExcelResult<ZonesData>(fileNameSave, lstResultado, columns, null, columns.Keys.ToArray());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }

        }

    }
    public class ExcelResult : ActionResult
    {
        public string FileName { get; set; }
        public string Path { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.Buffer = true;
            context.HttpContext.Response.Clear();
            context.HttpContext.Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
            context.HttpContext.Response.ContentType = "application/vnd.ms-excel";
            context.HttpContext.Response.WriteFile(context.HttpContext.Server.MapPath(Path));
        }
    }
}
