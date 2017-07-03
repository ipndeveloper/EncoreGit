using System.Web.Mvc;
using System.Web.Routing;

namespace DistributorBackOffice.Extensions
{
	public static class RouteCollectionExtensions
	{
		public static Route MapAreaRoute(this RouteCollection routes, string name, string url, string areaName, object defaults)
		{
			ControllerBuilder.Current.DefaultNamespaces.Clear();
			ControllerBuilder.Current.DefaultNamespaces.Add("DistributorBackOffice.Areas." + areaName + ".Controllers");

			Route route = routes.MapRoute(name, url, defaults, null, new string[] { "DistributorBackOffice.Areas." + areaName + ".Controllers" });
			if (route.DataTokens["area"] == null)
			{
				route.DataTokens["area"] = areaName;
			}

			return route;
		}
	}
}
