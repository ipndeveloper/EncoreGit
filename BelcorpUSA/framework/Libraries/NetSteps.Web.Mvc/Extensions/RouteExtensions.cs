using System.Web.Mvc;
using System.Web.Routing;

namespace NetSteps.Web.Mvc.Extensions
{
    public static class RouteExtensions
    {
        public static Route MapRouteWithName(this RouteCollection routes,
           string name, string url, object defaults = null, object constraints = null)
        {
            Route route = routes.MapRoute(name, url, defaults, constraints);
            route.DataTokens = route.DataTokens ?? new RouteValueDictionary();
            route.DataTokens.Add("RouteName", name);

            return route;
        }

        public static Route MapRouteWithName(this AreaRegistrationContext routes,
          string name, string url, object defaults = null, object constraints = null)
        {
            Route route = routes.MapRoute(name, url, defaults, constraints);
            route.DataTokens = route.DataTokens ?? new RouteValueDictionary();
            route.DataTokens.Add("RouteName", name);

            return route;
        }
    }
}
