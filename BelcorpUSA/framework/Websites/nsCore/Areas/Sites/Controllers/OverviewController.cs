using System;
using System.Web.Mvc;
using NetSteps.Common.Configuration;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Web.Mvc.Attributes;

namespace nsCore.Areas.Sites.Controllers
{
    public class OverviewController : BaseSitesController
    {

        /// <summary>
        /// Shows an overview of the selected base site
        /// </summary>
        /// <param name="id">The id of the site</param>
        /// <returns></returns>
        [FunctionFilter("Sites", "~/Accounts")]
        public virtual ActionResult Index(int? id)
        {
            try
            {
                var current = NetSteps.Web.Mvc.Helpers.CoreContext.CurrentCultureInfo;


                CurrentSite = LoadSiteWithNewsAndArchiveData(id);

                if (CurrentSite == null)
                    return RedirectToAction("Index", "Landing");

                //Load up all of the types and events to show
                ViewData["CurrentSite"] = CurrentSite;
                ViewData["Events"] = CalendarEvent.LoadByDateRange(DateTime.Now, DateTime.Now.AddDays(int.Parse(ConfigurationManager.AppSettings["OverviewDays"])), CurrentSite.SiteID);
                //ViewData["CalendarEventTypes"] = AccountListValue.LoadCorporateListValuesByType(NetSteps.Data.Entities.Constants.ListValueType.CalendarEventType.ToInt());
                ViewData["Days"] = ConfigurationManager.AppSettings["OverviewDays"];

                return View(CurrentSite);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }
    }
}
