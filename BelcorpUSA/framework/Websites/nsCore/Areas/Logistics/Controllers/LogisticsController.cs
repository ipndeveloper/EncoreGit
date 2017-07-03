using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetSteps.Common.Base;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Business.Controllers;
using NetSteps.Common.Globalization;
using System.Text;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Web.Mvc.Helpers;


namespace nsCore.Areas.Logistics.Controllers
{
    public class LogisticsController : BaseController
    {
        //
        // GET: /Logistics/Logistics/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult BrowseProvider()
        {
            return View();
        }

        /// <summary>
        ///  Excel Export
        /// </summary>
        /// <returns>Excel File</returns>
        public virtual ActionResult Exportproviders(int? logisticsProviderID, string name)
        {
            try
            {

                var lstResultado = LogisticsProv.SearchProvider(new LogisticsProvParameters()
                {
                    LogisticsProviderID = logisticsProviderID,
                    Name = name
                });

                string fileNameSave = string.Format("{0}.xlsx", Translation.GetTerm("Providers", "Providers"));

                var columns = new Dictionary<string, string>
				{
                    {"LogisticsProviderID", Translation.GetTerm("LogisticProviderCode","Logistic Provider Code")} ,
                    {"Name", Translation.GetTerm("Name","Name")}  
				};

                return new NetSteps.Web.Mvc.ActionResults.ExcelResult<LogisticsProviderSearData>(fileNameSave, lstResultado, columns, null, columns.Keys.ToArray());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }

        }

        public ActionResult NewProvider()
        {
            return View();
        }

        public ActionResult ProviderDetails(int? id)
        {
            var details = id.HasValue ? LogisticsProvRepository.SearDetails().FindAll(x => x.LogisticsProviderID == id.Value) : new List<LogisticsProviderSearData>();
            TempData["LogisticsProviderID"] = id;
            ViewData["details"] = details;

            return View();
        }
        public ActionResult Documents(int? id)
        {
            var details = id.HasValue ? LogisticsProvRepository.SearDetails().FindAll(x => x.LogisticsProviderID == id.Value) : new List<LogisticsProviderSearData>();
            ViewData["details"] = details;

            ViewData["DocumentType"] = LogisticsProvRepository.SearchDocumenttypes();
            return View();
        }
        public ActionResult Address(int? id)
        {
            var details = id.HasValue ? LogisticsProvRepository.SearDetails().FindAll(x => x.LogisticsProviderID == id.Value) : new List<LogisticsProviderSearData>();
            ViewData["details"] = details;

            return View();
        }
        public ActionResult Routes(int? id)
        {
            var details = id.HasValue ? LogisticsProvRepository.SearDetails().FindAll(x => x.LogisticsProviderID == id.Value) : new List<LogisticsProviderSearData>();
            ViewData["details"] = details;
            return View();
        }
        [OutputCache(CacheProfile = "AutoCompleteData")]
        public virtual ActionResult searchRoutes(string query)
        {
            try
            {
                //var resultado = NetSteps.Web.Mvc.Extensions.DictionaryExtensions.ToAJAXSearchResults(NetSteps.Data.Entities.Routes.GetRoutesSearch(query));
                //return Json(resultado);}
                return Json(new { result = false, message = "hhh" });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
        #region Browse
        [OutputCache(CacheProfile = "AutoCompleteData")]
        public virtual ActionResult searchProv(string query)
        {
            try
            {
                var resultado = NetSteps.Web.Mvc.Extensions.DictionaryExtensions.ToAJAXSearchResults(LogisticsProv.GetSearchProvider(query));
                return Json(resultado);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }


        [OutputCache(CacheProfile = "PagedGridData")]
        public virtual ActionResult GetLogisticsProv(int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection,
            int? LogisticsProviderID, string Name, int active)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                int count = 0;

                var providers = LogisticsProv.SearchProvider(new LogisticsProvParameters()
                {
                    LogisticsProviderID = LogisticsProviderID,
                    Name = Name,
                    Active = active,
                    PageIndex = page,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    OrderByDirection = orderByDirection
                });

                foreach (var provider in providers)
                {
                    builder.Append("<tr>");

                    builder
                        .AppendCheckBoxCell(value: provider.LogisticsProviderID.ToString())
                        .AppendLinkCell("~/Logistics/Logistics/ProviderDetails/" + provider.LogisticsProviderID, provider.LogisticsProviderID.ToString())
                        .AppendCell(provider.Name)
                        .Append("</tr>");
                    ++count;
                }

                return Json(new { result = true, totalPages = providers.TotalPages, page = providers.TotalCount == 0 ? "<tr><td colspan=\"7\">There are no Providers</td></tr>" : builder.ToString() });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        //Delete Documents
        public virtual ActionResult DeleteDocuments(List<int> items)
        {
            if (items != null && items.Count > 0)
            {
                try
                {
                    foreach (var prov in LogisticsProvRepository.SearchDocuments().FindAll(x => items.Contains(x.IDTypeID)))
                    {
                        LogisticsProvRepository.upDeleteDocument(prov.IDTypeID);
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

        //Desactive       
        public virtual ActionResult ChangeStatusProv(List<int> items, bool active)
        {
            if (items != null && items.Count > 0)
            {
                try
                {

                    foreach (var prov in LogisticsProv.SearchProvider().FindAll(x => items.Contains(x.LogisticsProviderID)))
                    {
                        int Active = Convert.ToInt32(active);
                        LogisticsProvRepository.upDesactiveProv(prov.LogisticsProviderID, Active);
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
        /// <summary>
        /// Developed By KLC -  CSTI
        /// </summary>
        /// <param name="Details"></param>
        /// <returns></returns>
        public virtual ActionResult ToggleStatus(int LogisticsProviderID, int active)
        {
            try
            {
                LogisticsProvRepository.upDesactiveProv(LogisticsProviderID, active);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }

            return Json(new { result = true });
        }

        #endregion
        #region Details
        #region Save Details
        public virtual ActionResult SaveDetails(LogisticsProviderSearData Details)
        {
            try
            {
                //KLC -CSTI 
                var details = new LogisticsProviderSearData
                {
                    LogisticsProviderID = Convert.ToInt32(Details.LogisticsProviderID),
                    Name = Convert.ToString(Details.Name),
                    PhoneNumber = Convert.ToString(Details.PhoneNumber),
                    FaxNumber = Convert.ToString(Details.FaxNumber),
                    EmailAddress = Convert.ToString(Details.EmailAddress),
                    TermName = Convert.ToString(Details.TermName),
                    Description = Convert.ToString(Details.Description),
                    Active = Convert.ToBoolean(Details.Active),
                    MarketID = Convert.ToInt32(Details.MarketID),
                    ExternalCode = Convert.ToString(Details.ExternalCode),
                    WorkInSaturdays = Convert.ToBoolean(Details.WorkInSaturdays),
                    WorkInSundays = Convert.ToBoolean(Details.WorkInSundays),
                    WorkInHolidays = Convert.ToBoolean(Details.WorkInHolidays),
                    ExternalTrakingURL = Convert.ToString(Details.ExternalTrakingURL)
                };

                int logisticsProviderID = LogisticsProvRepository.upInsertDetails(details);

                return Json(new { result = true, logisticsProviderID = logisticsProviderID });

            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
        #endregion
        #endregion
        #region Documents
        [OutputCache(CacheProfile = "PagedGridData")]
        public virtual ActionResult GetDocuments(int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection,
            int? id)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                int count = 0;

                var documents = LogisticsProvRepository.SearchDocuments(new LogisticsProvParameters()
                {
                    LogisticsProviderID = id,
                    PageIndex = page,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    OrderByDirection = orderByDirection
                });

                foreach (var document in documents)
                {
                    builder.Append("<tr>");
                    builder.AppendCheckBoxCell(value: document.IDTypeID.ToString(), name: "");
                    builder

                        .AppendCell(document.Name)
                        .AppendCell(document.IDValue)
                        .AppendCell(document.IDExpeditionDate.ToString())
                        .AppendCell(document.ExpeditionEntity.ToString())
                        .AppendCell(document.IsPrimaryID.ToString())
                        .Append("</tr>");
                    ++count;
                }

                return Json(new { result = true, totalPages = documents.TotalPages, page = documents.TotalCount == 0 ? "<tr><td colspan=\"7\">There are no Documents</td></tr>" : builder.ToString() });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
        #region SaveDoc
        public virtual ActionResult SaveDocuments(LogisticProviderSuppliedIDs documents)
        {
            try
            {
                var exist = LogisticsProvRepository.SearchDocuments().Where(r => r.IDTypeID == documents.IDTypeID && r.LogisticsProviderID == documents.LogisticsProviderID).FirstOrDefault();
                if (exist != null)
                    return Json(new { result = false, message = Translation.GetTerm("docisAssociated", "This Document is already associated.") });

                //KLC -CSTI 
                var document = new LogisticProviderSuppliedIDs
                {
                    IDTypeID = Convert.ToInt32(documents.IDTypeID),
                    LogisticsProviderID = Convert.ToInt32(documents.LogisticsProviderID),
                    IDValue = Convert.ToString(documents.IDValue),
                    IDExpeditionDate = Convert.ToDateTime(documents.IDExpeditionDate),
                    ExpeditionEntity = Convert.ToString(documents.ExpeditionEntity),
                    IsPrimaryID = Convert.ToBoolean(documents.IsPrimaryID)
                };

                LogisticsProvRepository.upInsertDocuments(document);

                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
        #endregion
        #endregion
        #region Routes Logistics
        #region Get Routes
        [OutputCache(CacheProfile = "PagedGridData")]
        public virtual ActionResult GetRoutesProv(int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection,
            int? id)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                int count = 0;

                var routes = LogisticsProvRepository.SearchRoutesProv(new LogisticsProvParameters()
                {
                    LogisticsProviderID = id,
                    PageIndex = page,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    OrderByDirection = orderByDirection
                });

                foreach (var route in routes)
                {
                    builder.Append("<tr>");
                    builder.AppendCheckBoxCell(value: route.RouteID.ToString(), name: "");
                    builder

                        .AppendCell(route.RouteID.ToString())
                        .AppendCell(route.Name)
                        .AppendCell(route.Monday.ToString())
                        .AppendCell(route.Tuesday.ToString())
                        .AppendCell(route.Wednesday.ToString())
                        .AppendCell(route.Thursday.ToString())
                        .AppendCell(route.Friday.ToString())
                        .AppendCell(route.Saturday.ToString())
                        .AppendCell(route.Sunday.ToString())

                        .Append("</tr>");
                    ++count;
                }

                return Json(new { result = true, totalPages = routes.TotalPages, page = routes.TotalCount == 0 ? "<tr><td colspan=\"7\">There are no Routes</td></tr>" : builder.ToString() });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
        #endregion
        #region Save Routes Logist.
        public virtual ActionResult SaveRoutes(RoutesLogProvSearchData routesprov)
        {
            try
            {
                var exist = LogisticsProvRepository.SearchRoutesProv().Where(r => r.RouteID == routesprov.RouteID && r.LogisticsProviderID == routesprov.LogisticsProviderID).FirstOrDefault();
                if (exist != null)
                    return Json(new { result = false, message = "This route is already associated." });

                //KLC -CSTI 
                var routes = new RoutesLogProvSearchData
                {
                    RouteID = Convert.ToInt32(routesprov.RouteID),
                    LogisticsProviderID = Convert.ToInt32(routesprov.LogisticsProviderID),
                    Monday = Convert.ToBoolean(routesprov.Monday),
                    Tuesday = Convert.ToBoolean(routesprov.Tuesday),
                    Wednesday = Convert.ToBoolean(routesprov.Wednesday),
                    Thursday = Convert.ToBoolean(routesprov.Thursday),
                    Friday = Convert.ToBoolean(routesprov.Friday),
                    Saturday = Convert.ToBoolean(routesprov.Saturday),
                    Sunday = Convert.ToBoolean(routesprov.Sunday)
                };

                LogisticsProvRepository.upInsertRoute(routes);


                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
        #endregion

        public virtual ActionResult DeleteRoutes(List<int> items)
        {
            try
            {
                //List<LogisticsProviderSearData> details = ViewData["details"] as List<LogisticsProviderSearData>;
                int LogisticsProviderID = 0;
                LogisticsProviderID = TempData["LogisticsProviderID"] == null? 0: Convert.ToInt32(TempData["LogisticsProviderID"]);
                //if (details.Count > 0) LogisticsProviderID = details[0].LogisticsProviderID;

                if (items != null)
                {
                    foreach (var itemId in items)
                    {
                        LogisticsProvRepository.upDeleteRoute(itemId, LogisticsProviderID);
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
        #endregion

        public ActionResult BrowseOrdersAllocate()
        {
            return View();
        }

        [OutputCache(CacheProfile = "PagedGridData")]
        public virtual ActionResult GetOrdersAllocate(int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection,
          int? PeriodID = 0, int? OrderNumber = 0, DateTime? startDate = null, DateTime? endDate = null, int? LogisticsProviderID = 0)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                int count = 0;

                var providers = LogisticsProv.SearchOrderProvider(new OrderLogisticProvParameters()
                {
                    OrderNumber = OrderNumber,
                    PeriodID = PeriodID,
                    StartDate = startDate,
                    EndDate = endDate,
                    LogisticProviderID = LogisticsProviderID,
                    PageIndex = page,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    OrderByDirection = orderByDirection
                });

                foreach (var provider in providers)
                {
                    builder.Append(string.Format("<tr data-id=\"{0}\">", provider.OrderShipmentID))
                        .AppendLinkCell("javascript:void(0);", "Change Logistic", linkCssClass: "btnEdit")
                        .AppendCell(provider.OrderNumber.ToString())
                        .AppendCell(provider.LogisticProviderName)
                        .AppendCell(provider.OrderDate.Date.ToShortDateString())
                        .AppendCell(Convert.ToDecimal(provider.OrderTotal).ToString("C",CoreContext.CurrentCultureInfo))
                        .Append("</tr>");
                    ++count;
                }

                return Json(new { result = true, totalPages = providers.TotalPages, page = providers.TotalCount == 0 ? "<tr><td colspan=\"7\">There are no Providers</td></tr>" : builder.ToString() });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }



        public virtual ActionResult EditChangeLogisticProviderModal(int OrderShipmentID)
        {
            OrderLogisticProviderSearchData data = new NetSteps.Data.Entities.Business.OrderLogisticProviderSearchData();
            data.OrderShipmentID = OrderShipmentID;
            return View(data);
        }

        public virtual ActionResult UpdateChangeLogisticProvider(int orderShipmentID, int logisticsProviderID)
        {
            try
            {
                var providers = LogisticsProv.ChangeLogisticProvider(new OrderLogisticProvParameters()
                {
                    OrderShipmentID = orderShipmentID,
                    LogisticProviderID = (int)logisticsProviderID
                });

                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
    }
}
