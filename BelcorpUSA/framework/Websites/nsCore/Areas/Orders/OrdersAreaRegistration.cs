using System.Web.Mvc;

namespace nsCore.Areas.Orders
{
	public class OrdersAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get
			{
				return "Orders";
			}
		}
        // dd
		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.MapRoute(
				"Orders_default",
				"Orders/{controller}/{action}/{id}",
				new { controller = "Landing", action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}
