using System.Web.Routing;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;

namespace NetSteps.Web.Mvc.Business.Routing
{
    public class PwsRouteConstraint : IRouteConstraint
    {
        public bool Match(System.Web.HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            string siteUrl = httpContext.Request.Url.ToString().GetSiteUrl();
            string host = httpContext.Request.Url.Host;

            // No siteUrl, return false.
            if (siteUrl.IsNullOrEmpty())
            {
                return false;
            }

            var site = SiteCache.GetSiteByUrl(siteUrl);

            if (site == null || site.SiteTypeID != (int)Constants.SiteType.Corporate)
            {
                return false;
            }

            if (!siteUrl.Contains("."))
            {
                return false;
            }

            var subdomain = values["subdomain"].ToString();

            var oldSubdomain = host.Substring(0, host.IndexOf("."));

            var url = siteUrl.Replace(oldSubdomain, subdomain);

            var pwsSite = SiteCache.GetSiteByUrl(url);

            if (pwsSite == null)
            {
                return false;
            }

            values["pwsurl"] = url;

            return true;
        }
    }
}
