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

namespace nsCore.Areas.Logistics.Controllers
{
    public class RoutesController : BaseController
    {
        //
        // GET: /Logistics/Routes/

        public ActionResult Index()
        {

            return View();
        }

        #region Routes
        //Add a new Route
        public ActionResult NewRoute(int? id)
        {
            //StatesProvinces
            ViewData["StatesProvinces"] = RoutesRepository.SearchStates();
            //
            var zones = id.HasValue ? RoutesRepository.SearchRoutesZones().FindAll(x => x.RouteID == id.Value) : new List<RoutesData>();
            ViewData["zones"] = zones;
            return View();
        }

        [OutputCache(CacheProfile = "AutoCompleteData")]
        public virtual ActionResult searchRoutes(string query)
        {
            try
            {
                var resultado = NetSteps.Web.Mvc.Extensions.DictionaryExtensions.ToAJAXSearchResults(Routes.GetRoutesSearch(query));
                return Json(resultado);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }


        [OutputCache(CacheProfile = "PagedGridData")]
        public virtual ActionResult GetRoutes(int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection,
            int? RouteID)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                int count = 0;

                var routes = Routes.Search(new RouterParametros()
                {
                    RouteID = RouteID,
                    PageIndex = page,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    OrderByDirection = orderByDirection
                });

                foreach (var route in routes)
                {
                    builder.Append("<tr>");

                    builder
                        .AppendCheckBoxCell(value: route.RouteID.ToString())
                        .AppendLinkCell("~/Logistics/Routes/NewRoute/" + route.RouteID, route.RouteID.ToString())
                        .AppendCell(route.Name)
                        .Append("</tr>");
                    ++count;
                }

                return Json(new { result = true, totalPages = routes.TotalPages, page = routes.TotalCount == 0 ? "<tr><td colspan=\"7\">" + Translation.GetTerm("NotRoutes", "There are no routes.") + "</td></tr>" : builder.ToString() });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public virtual ActionResult ChangeStatus(List<int> items)
        {
            if (items != null && items.Count > 0)
            {
                try
                {
                    foreach (var prov in Routes.Search().FindAll(x => items.Contains(x.RouteID)))
                    {
                        RoutesRepository.upDesactive(prov.RouteID);
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
        #endregion
        #region Zonas
        [OutputCache(CacheProfile = "PagedGridData")]
        public virtual ActionResult GetZones(int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection,
            int? id)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                int count = 0;

                var zones = RoutesRepository.SearchZones(new RouterParametros()
                {
                    RouteID = id.HasValue ? id.Value : 0,
                    PageIndex = page,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    OrderByDirection = orderByDirection
                });

                foreach (var zone in zones)
                {
                    builder.Append("<tr>");
                    builder.AppendCheckBoxCell(value: zone.RouteID.ToString(), name: "");
                    builder

                        .AppendCell(zone.Name)
                        .AppendCell(zone.Value)
                        .AppendCell(zone.Except.ToString())
                        .Append("</tr>");
                    ++count;
                }

                return Json(new { result = true, totalPages = zones.TotalPages, page = zones.TotalCount == 0 ? "<tr><td colspan=\"7\">" + Translation.GetTerm("NotZones", "There are no Zones.") + " </td></tr>" : builder.ToString() });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
        //Citys KLC
        public virtual ActionResult GetCitys(string state)
        {
            try
            {
                return Json(new
                {
                    result = true,
                    Cities = (Routes.SearchCitys(state.Trim())).Select(pp => new { City = pp.City })
                });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
        #endregion
        #region saveroutes
        /// <summary>
        /// Developed By KLC - CSTI
        /// </summary>
        /// <param name="zones"></param>
        /// <param name="routeID"></param>
        /// <param name="nameRoute"></param>
        /// <returns></returns>
        public virtual ActionResult SaveRoutes(List<ZonesData> zones, int? routeID, string nameRoute, string deletedzones)
        {
            try
            {
                int RouteID = Routes.spInsertRoute(routeID, nameRoute); //Guardar Ruta

                if (deletedzones.Length > 0)
                {
                    string[] dzrow = deletedzones.Remove(deletedzones.Length - 1, 1).Split(Convert.ToChar("*"));
                    foreach (var item in dzrow)
                    {
                        string[] zone = item.Split(Convert.ToChar(","));
                        var obj = new ZonesData
                        {
                            Name = zone[0].Trim(),
                            Value = zone[1].Trim(),
                            Except = Convert.ToBoolean(zone[2].Trim()),
                            RouteID = RouteID
                        };
                        Routes.spDeleteRouteScopes(obj);
                    }
                    //Routes.spDeleteRouteScopes(RouteID); //Eliminar Zonas.
                }

                if (zones != null)
                {
                    foreach (var rzone in zones)
                    {
                        if ((rzone.Value != "null") && (rzone.Name != "null"))
                        {
                            var zone = new ZonesData
                            {
                                Name = rzone.Name.Trim(),
                                Value = Convert.ToString(rzone.Value.Trim()),
                                Except = Convert.ToBoolean(rzone.Except),
                                RouteID = RouteID
                            };
                            Routes.InsertRoutesZones(zone);
                        }
                    }
                }
                return Json(new { result = true, RouteID = RouteID });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
        #endregion

        /// <summary>
        ///  Excel Export
        /// </summary>
        /// <returns>Excel File</returns>
        public virtual ActionResult ExportRoutes(int? routeId)
        {
            try
            {

                var lstResultado = Routes.Search(new RouterParametros()
                {
                    RouteID = routeId
                });

                string fileNameSave = string.Format("{0}.xlsx", Translation.GetTerm("Routes", "Routes"));

                var columns = new Dictionary<string, string>
				{
                    {"RouteID", Translation.GetTerm("RouteCode","Route Code")} ,
                    {"Name", Translation.GetTerm("Name","Name")}  
				};

                return new NetSteps.Web.Mvc.ActionResults.ExcelResult<RoutesData>(fileNameSave, lstResultado, columns, null, columns.Keys.ToArray());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }

        }


    }
}
