using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Business.Logic;
using NetSteps.Web.Mvc.Helpers;

namespace nsCore.Areas.Support.Controllers
{
    public class ExecMonthlyClosingController : Controller
    {
        //
        // GET: /Support/ExecMonthlyClosing/

        public ActionResult Index()
        {
            var ListAvailablePlans = from x in MonthlyClosingLogic.Instance.ListAvailablePlans()
                                     select new SelectListItem()
                                     {
                                         Text = x.Value,
                                         Value = x.Key
                                     };

            TempData["ActivePeriod"] = MonthlyClosingLogic.Instance.GetActivePeriod();
            TempData["AvailablePlans"] = ListAvailablePlans;

            return View();
        }

        public virtual ActionResult Process(string Plan, string Period)
        {
            try
            {
                //var result = MonthlyClosingLogic.Instance.ExecuteCampaignMonthlyClosing(CoreContext.CurrentLanguageID, Plan, Period);
                var result = MonthlyClosingLogic.Instance.ExecuteCampaignMonthlyClosing(Plan, Period);
                if (result)
                    return Json(new { result = true });
                else
                    return Json(new { result = false });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
    }
}
