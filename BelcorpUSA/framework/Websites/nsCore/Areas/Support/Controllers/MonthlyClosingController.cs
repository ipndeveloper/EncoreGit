using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetSteps.Data.Entities.Business.Logic;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Common.Globalization;

namespace nsCore.Areas.Support.Controllers
{
    public class MonthlyClosingController : Controller
    {
        //
        // GET: /Support/MonthlyClosing/

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
                var result = MonthlyClosingLogic.Instance.ProcessCampaignMonthlyClosing(Plan, Period);
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
