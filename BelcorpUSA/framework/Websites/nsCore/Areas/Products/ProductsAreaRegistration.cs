using System.Web.Mvc;
using nsCore.Areas.Products.Models.Promotions.Interfaces;
using nsCore.Areas.Products.Models.Promotions.ModelBinders;

namespace nsCore.Areas.Products
{
	public class ProductsAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get
			{
				return "Products";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.MapRoute("Wait", "Products/Wait", new { controller = "Catalogs", action = "Wait" });
			context.MapRoute("ReloadInventory", "Products/ReloadInventory", new { controller = "Products", action = "ReloadInventory" });
			context.MapRoute("InitiateReloadInventory", "Products/InitiateReloadInventory", new { controller = "Products", action = "InitiateReloadInventory" });
			context.MapRoute(
				"Products",
				"Products/Products/{action}/{baseProductId}/{productId}",
				new { controller = "Products", action = "Index", baseProductId = UrlParameter.Optional, productId = UrlParameter.Optional }
			);
			context.MapRoute(
				"Products_default",
				"Products/{controller}/{action}/{id}",
				new { controller = "Catalogs", action = "Index", id = UrlParameter.Optional }
			);

			ModelBinders.Binders.Add(typeof(ICartConditionModel), new CartConditionModelBinder());
			ModelBinders.Binders.Add(typeof(ICartRewardModel), new CartRewardModelBinder());
		}
	}
}
