using System.Linq;
using System.Web.Mvc;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Web.Mvc.Helpers;
using nsCore.Controllers;

namespace nsCore.Areas.Sites.Controllers
{
    public abstract class BaseSitesController : BaseController
    {
        // We could load the site here if null, but for now the controllers seem to be loading it already.
        public Site CurrentSite
        {
            get
            {
                if (_currentSite == null)
                {
                    _currentSite = GetCurrentSiteFromDb();
                }
                return _currentSite;
            }
            set
            {
                _currentSite = value;
                if (value == null)
                {
                    CurrentSiteId = null;
                }
                else
                {
                    CurrentSiteId = value.SiteID;
                }
            }
        }
        private Site _currentSite;

        public static Site GetCurrentSiteFromDb()
        {
            if (CurrentSiteId.HasValue)
            {
                return Site.LoadFullWithoutContent(CurrentSiteId.Value);
            }
            else
            {
                return null;
            }
        }

        public static Site GetCurrentSiteFromCache()
        {
            var currentSiteId = CurrentSiteId;
            if (currentSiteId.HasValue)
            {
                return SiteCache.GetSiteByID(currentSiteId.Value);
            }
            else
            {
                return null;
            }
        }

        public static int? CurrentSiteId
        {
            get { return System.Web.HttpContext.Current.Session["CurrentSiteId"] as int?; }
            set { System.Web.HttpContext.Current.Session["CurrentSiteId"] = value; }
        }

        /// <summary>
        /// Make sure we always have all of the base sites in the sub navigation
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (CoreContext.CurrentUser != null)
                ViewData["Sites"] = Site.LoadBaseSites(CoreContext.CurrentMarketId, (CoreContext.CurrentUser as CorporateUser).CorporateUserID);
            base.OnActionExecuting(filterContext);
        }

        public Site LoadSiteWithSiteMapData(int? id)
        {
            if (!IsValidSiteWithSiteMap(CurrentSite) || CurrentSite.SiteID != id)
            {
                int siteID = GetSiteID(id, CurrentSite);

                return siteID > 0 ? Site.SiteWithSiteMap(siteID) : null;
            }

            return CurrentSite;
        }

        public bool IsValidSiteWithSiteMap(Site currentSite)
        {
            return currentSite.IsNotNull() && currentSite.Navigations.Any();
        }

        public Site LoadSiteWithNewsData(int? id)
        {
            if (!IsValidSiteWithNews(CurrentSite) || CurrentSite.SiteID != id)
            {
                int siteID = GetSiteID(id, CurrentSite);

                return siteID > 0 ? Site.SiteWithNews(siteID) : null;
            }
            return CurrentSite;
        }

        public bool IsValidSiteWithNews(Site currentSite)
        {
            return currentSite.IsNotNull() && currentSite.News.Any();
        }

        public Site LoadSiteWithNewsAndArchiveData(int? id)
        {
            if (!IsValidSiteWithNewsAndArchive(CurrentSite) || CurrentSite.SiteID != id)
            {
                int siteID = GetSiteID(id, CurrentSite);

                return siteID > 0 ? Site.SiteWithNewsAndArchive(siteID) : null;
            }
            return CurrentSite;
        }

        public bool IsValidSiteWithNewsAndArchive(Site currentSite)
        {
            return currentSite.IsNotNull() && currentSite.News.Any() && currentSite.Archives.Any();
        }

        public int GetSiteID(int? id, Site currentSite)
        {
            int siteID = 0;

            if (id.HasValue && id.Value > 0)
                siteID = id.Value;
            else if (currentSite.IsNotNull())
                siteID = currentSite.SiteID;

            return siteID;
        }
    }
}
