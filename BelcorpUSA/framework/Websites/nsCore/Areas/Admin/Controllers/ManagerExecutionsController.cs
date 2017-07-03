using System;
using System.Text;
using System.Web.Mvc;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Business.Logic;
using NetSteps.Web.Mvc.Helpers;
using NetSteps.Web.Extensions;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Common.Globalization;
using Newtonsoft.Json;

namespace nsCore.Areas.Admin.Controllers
{
    public class ManagerExecutionsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public virtual string GetGridSubProcessStatus(string status, string page, string pageSize, string orderBy, string orderByDirection)
        {
            try
            {
                var order = (orderByDirection.ToString().ToUpper().StartsWith("ASC")) ? "asc" : "desc";
                var result = ManagerExecutionsLogic.Instance.ListSubProcessStatus(CoreContext.CurrentLanguageID, int.Parse(status),
                                                                                int.Parse(page), int.Parse(pageSize), orderBy, order);
                string json = JsonConvert.SerializeObject(result);                
                return json;
            }
            catch (Exception) {return "{ 'result' = false }";}
        }

        public virtual string GetGridMainProcessesDetail(string status, string page, string pageSize, string orderBy, string orderByDirection)
        {
            try
            {
                var order = (orderByDirection.ToString().ToUpper().StartsWith("ASC")) ? "asc" : "desc";
                var result = ManagerExecutionsLogic.Instance.ListMainProcessesDetail(CoreContext.CurrentLanguageID, int.Parse(status),
                                                                                int.Parse(page), int.Parse(pageSize), orderBy, order);
                string json = JsonConvert.SerializeObject(result);
                return json;
            }
            catch (Exception) { return "{ 'result' = false }"; }
        }

        public virtual string ListSubprocessStatuses()
        {
            try
            {
                var oListStatuses = StatusProcessMonthlyClosureLogLogic.Instance.ListStatuses(CoreContext.CurrentLanguageID);
                var json = JsonConvert.SerializeObject(oListStatuses);
                return json;
            }
            catch (Exception) {return "{ 'result' = false }";}
        }

        public ActionResult GetFailedSuprocess(string type, string id)
        {
            try
            {
                if (type == "MC")
                {
                    var result = ManagerExecutionsLogic.Instance.GetFailedSubProcess(CoreContext.CurrentLanguageID, int.Parse(id));
                    var json = new { process = result.ProcessName, subprocess = result.SubProcessName, message = result.MessageToShow, error = result.RealError };
                    return Json(json, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = ManagerExecutionsLogic.Instance.GetFailedSubProcess_PI(CoreContext.CurrentLanguageID, int.Parse(id));
                    var json = new { process = result.ProcessName, subprocess = result.SubProcessName, message = result.MessageToShow, error = result.RealError };
                    return Json(json, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
             return Json(new { result = false, message = exception.PublicMessage }, JsonRequestBehavior.AllowGet);}
        }

        public ActionResult ReProcess(string type, string id)
        {
            try
            {
                var result = ManagerExecutionsLogic.Instance.Reprocess(type, CoreContext.CurrentLanguageID, int.Parse(id));
                if (result)
                {
                    var json = new { result = true}; return Json(json, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var exception = EntityExceptionHelper.GetAndLogNetStepsException(null, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                    return Json(new { result = false, message = exception.PublicMessage }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {   var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage }, JsonRequestBehavior.AllowGet);}
        }
    }
}
