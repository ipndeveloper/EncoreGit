using System;
using System.Linq;
using System.Web.Routing;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;

namespace NetSteps.Web.Mvc.Business.Routing
{
    using NetSteps.Common.Extensions;

    public class CmsPageRouteConstraint : IRouteConstraint
    {
        /// <summary>
        /// An array of page types that are handled by CMS.
        /// </summary>
        private static readonly short[] _cmsPageTypeIDs =
        {
            (short)Constants.PageType.User,
            (short)Constants.PageType.External
        };

        public bool Match(System.Web.HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            // No path, return false.
            if (string.IsNullOrWhiteSpace(httpContext.Request.Path) || httpContext.Request.Path == "/")
            {
                return false;
            }

            string siteUrl;
            if (httpContext.Request.Url != null)
            {
                siteUrl = httpContext.Request.Url.ToString().GetSiteUrl();
            }
            else
            {
                return false;
            }

            // No siteUrl, return false.
            if (string.IsNullOrWhiteSpace(siteUrl))
            {
                return false;
            }

            var site = SiteCache.GetSiteByUrl(siteUrl);
            var baseSite = site;

            // No site, return false.
            if (site == null)
            {
                return false;
            }

            if (!site.IsBase)
            {
                baseSite = SiteCache.GetSiteByID(site.BaseSiteID.Value);
            }

            // Search active CMS pages.
            return baseSite.Pages
                .Any(p =>
                    p.Active
                    && _cmsPageTypeIDs.Contains(p.PageTypeID)
                    && string.Equals(p.Url, httpContext.Request.Path, StringComparison.OrdinalIgnoreCase));
        }
    }
}
