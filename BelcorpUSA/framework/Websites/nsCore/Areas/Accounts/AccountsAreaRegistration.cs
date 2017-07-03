using System.Web.Mvc;

namespace nsCore.Areas.Accounts
{
	public class AccountsAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get
			{
				return "Accounts";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.MapRoute("Search", "Accounts/Search", new { controller = "Browse", action = "Search" });
            context.MapRoute("SearchOnAccountStatuses", "Accounts/SearchOnAccountStatuses", new { controller = "Browse", action = "SearchOnAccountStatuses" });
			context.MapRoute("SearchActive", "Accounts/SearchActive", new { controller = "Browse", action = "SearchActive" });
			context.MapRoute("SearchDistributors", "Accounts/SearchDistributors", new { controller = "Browse", action = "SearchDistributors" });
			context.MapRoute("SearchActiveDistributors", "Accounts/SearchActiveDistributors", new { controller = "Browse", action = "SearchActiveDistributors" });

            context.MapRoute("SearchActiveDistributorsCEP", "Accounts/SearchActiveDistributorsCEP", new { controller = "Browse", action = "SearchActiveDistributorsCEP" });
            context.MapRoute("GetAccounts", "AccountsWithoutSponsor/GetAccounts", new { controller = "AccountsWithoutSponsor", action = "GetAccounts" });
			context.MapRoute("Subscriptions", "Accounts/Subscriptions/{action}/{id}", new { controller = "Autoships", action = "Index", id = UrlParameter.Optional });

			context.MapRoute(
				"Accounts_default",
				"Accounts/{controller}/{action}/{id}",
				new { controller = "Landing", action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}
