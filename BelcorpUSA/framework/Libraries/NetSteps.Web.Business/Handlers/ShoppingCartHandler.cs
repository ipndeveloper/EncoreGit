using System;
using NetSteps.Objects.Business;

namespace NetSteps.WebControls
{
	internal class ShoppingCartHandler
	{
		internal static void HandleRequest(string action, System.Web.HttpContext context)
		{
			switch (action)
			{
				case "additembypartnumber":
					ShoppingCartHandler.AddItemByPartNumber(context);
					break;
				case "additembyid":
					ShoppingCartHandler.AddItemById(context);
					break;
			}
		}

		private static void AddItemByPartNumber(System.Web.HttpContext context)
		{
			if (context.Request.QueryString["cartSession"] == null ||
				context.Request.QueryString["quantity"] == null ||
				context.Request.QueryString["partnumber"] == null)
			{
				throw new ArgumentException("'cartSession', 'quantity' and 'partnumber' are required parameters");
			}

			ShoppingCart cart = context.Session[context.Request.QueryString["cartSession"]] as ShoppingCart;
			if (cart != null)
			{
				// Parse out the quantity...
				int quantity;
				if (!Int32.TryParse(context.Request.QueryString["quantity"], out quantity)) { quantity = 1; }

				ProductBase baseProduct = new ProductBase();//context.Session["CurrentSite"] as Site);
				baseProduct.BaseSKU = context.Request.QueryString["partnumber"];
				baseProduct.LoadBySKU();
				Product itemProduct = baseProduct.Products[0];

				//if (quantity > itemProduct.QuantityOnHand)
				//{
				//    context.Response.Write("nostock");
				//}
				//else
				//{
				// Add item to cart
				cart.AddItem(context.Request.QueryString["partnumber"], quantity);
				//}
			}
			else
			{
				throw new ArgumentException("Could not load site object from session");
			}
		}

		private static void AddItemById(System.Web.HttpContext context)
		{
			if (context.Request.QueryString["cartSession"] == null ||
				context.Request.QueryString["quantity"] == null ||
				context.Request.QueryString["itemid"] == null)
			{
				throw new ArgumentException("'cartSession', 'quantity' and 'itemid' are required parameters");
			}

			ShoppingCart cart = context.Session[context.Request.QueryString["cartSession"]] as ShoppingCart;
			if (cart != null)
			{
				// Parse out the quantity...
				int quantity;
				if (!Int32.TryParse(context.Request.QueryString["quantity"], out quantity)) { quantity = 1; }

				// Add item to cart
				cart.AddItem(Convert.ToInt32(context.Request.QueryString["itemid"]), quantity);
			}
			else
			{
				throw new ArgumentException("Could not load site object from session");
			}
		}
	}
}
