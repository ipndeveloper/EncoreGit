using System.Web.Mvc;

namespace DistributorBackOffice.Areas.Orders
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

		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.MapRoute("ReloadInventory", "Products/ReloadInventory", new { controller = "OrderEntry", action = "ReloadInventory" });
			context.MapRoute(
				"Orders_default",
				"Orders/{controller}/{action}/{id}",
				new { controller = "OrderHistory", action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}
