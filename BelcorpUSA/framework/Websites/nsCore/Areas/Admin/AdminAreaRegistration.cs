using System.Web.Mvc;

namespace nsCore.Areas.Admin
{
	public class AdminAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get
			{
				return "Admin";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
            context.MapRoute("ShowConfig", "Admin/ShowConfig", new { controller = "Configuration", action = "ShowConfig" });
            context.MapRoute("PingAvataxUrl", "Admin/PingAvataxUrl", new { controller = "Configuration", action = "PingAvataxUrl" });
			context.MapRoute("Admin_default", "Admin/{controller}/{action}/{id}", new { controller = "Users", action = "Index", id = UrlParameter.Optional });
		}
	}
}
