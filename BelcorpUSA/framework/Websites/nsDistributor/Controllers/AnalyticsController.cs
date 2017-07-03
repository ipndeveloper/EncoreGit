using System;
using System.Web.Mvc;
using NetSteps.Data.Entities.Exceptions;

namespace nsDistributor.Controllers
{
    public class AnalyticsController : BaseController
    {
        public virtual ActionResult Google()
        {
            return View();
        }

        [HttpPost]
        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult UpdateGoogleTracker(string trackerID)
        {
            try
            {
                if (!CurrentSite.IsBase)
                    CurrentSite.SaveSiteSetting("GoogleAnalyticsTrackerID", trackerID);

                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsBusinessException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
    }
}
