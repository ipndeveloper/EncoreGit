using System.Web.Mvc;

namespace Cart.DemoWebsite.Areas.Cart
{
	public class CartAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get
			{
				return "Cart";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.MapRoute(
				"Cart",
				"Cart/{controller}/{action}/{id}",
				new { controller = "Cart", action = "Cart", id = UrlParameter.Optional }
			);
		}
	}
}
