using System;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common.Configuration;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;

namespace nsCore.Areas.Sites.Controllers
{
    public class ChildSitesController : BaseSitesController
    {/// <summary>
        /// Show the child sites of this base site
        /// </summary>
        /// <param name="id">The id of the base site</param>
        /// <returns></returns>
        [FunctionFilter("Sites-Replicated Sites", "~/Sites/Overview")]
        public virtual ActionResult Index(int? id)
        {
            try
            {
                if (id.HasValue && id.Value > 0)
                {
                    CurrentSite = Site.LoadFull(id.Value);
                }
                if (CurrentSite != null)
                {
                    ViewData["BaseSiteId"] = CurrentSite.SiteID;
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Landing");
                }
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        /// <summary>
        /// Checks if the url is available
        /// </summary>
        /// <param name="url">The url to check for</param>
        /// <returns></returns>
        public virtual ActionResult CheckIfUrlAvailable(string url)
        {
            return Content(SiteUrl.IsAvailable(url).ToString());
        }

        /// <summary>
        /// Get the sites with pagination
        /// </summary>
        /// <param name="baseSiteId">The id of the base site</param>
        /// <param name="page">The current page</param>
        /// <param name="pageSize">The size of the current page</param>
        /// <param name="status">A possible status to check for</param>
        /// <param name="siteName">A possible site name to check for</param>
        /// <param name="orderBy">The column to order by</param>
        /// <param name="orderByDirection">The direction of the sort</param>
        /// <returns></returns>
        [OutputCache(CacheProfile = "PagedGridData")]
        public virtual ActionResult Get(int baseSiteId, int? status, string siteName, string url, int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection)
        {
            try
            {
                SiteSearchParameters searchParams = new SiteSearchParameters()
                  {
                      BaseSiteID = baseSiteId,
                      SiteStatusID = status,
                      SiteName = siteName,
                      Url = url,
                      PageIndex = page,
                      PageSize = pageSize,
                      OrderBy = orderBy,
                      OrderByDirection = orderByDirection
                  };
                var sites = Site.Search(searchParams);
                StringBuilder builder = new StringBuilder();
                int count = 0;
                foreach (SiteSearchData site in sites)
                {
                    builder.Append("<tr>");
                    var payForSites = ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.PayForSites);
                    if (payForSites)
                    {
                        if (site.AutoshipOrderID.HasValue)
                            builder.AppendLinkCell("~/Accounts/Subscriptions/Edit/" + site.AccountNumber + "?autoshipOrderId=" + site.AutoshipOrderID, site.SiteName);
                        else
                            builder.AppendCell(site.SiteName);
                    }
                    else
                        builder.AppendLinkCell("~/Accounts/SiteSubscriptions/Index/" + site.AccountNumber + "?baseSiteId=" + CurrentSite.SiteID, site.SiteName);

                    builder.AppendLinkCell(site.Url, site.Url)
                    .AppendCell(SmallCollectionCache.Instance.SiteStatuses.GetById(site.SiteStatusID).GetTerm())
                    .AppendCell(site.Enrolled.ToString())
                    .Append("</tr>");
                    ++count;
                }
                return Json(new { totalPages = sites.TotalPages, page = builder.ToString() });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
    }
}
