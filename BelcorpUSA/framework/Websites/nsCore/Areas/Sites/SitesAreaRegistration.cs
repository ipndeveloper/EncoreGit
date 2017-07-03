using System.Web.Mvc;

namespace nsCore.Areas.Sites
{
	public class SitesAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get
			{
				return "Sites";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.MapRoute(
				"Sites_default",
				"Sites/{controller}/{action}/{id}",
				new { controller = "Landing", action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}
