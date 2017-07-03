using System;
using NetSteps.Data.Common.Context;
using NetSteps.Data.Common.Entities;
using NetSteps.Data.Common.Services;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities.Services
{
	public class InventoryService : IInventoryService
	{
		public IInventoryProductCheckResponse GetProductAvailabilityForOrder(IOrderContext orderContext, int productID, int quantity)
		{
			return GetProductAvailabilityForOrder(orderContext.Order, productID, quantity);
		}

		public IInventoryProductCheckResponse GetProductAvailabilityForOrder(IOrder order, int productID, int quantity)
		{
			var response = new InventoryProductCheckResponse();

			try
			{
				// get the product
				var product = getProduct(productID);

				// return if the product is inactive
				if (!product.Active)
				{
					response.CannotAddInactive = quantity;
					return response;
				}

				bool allowBackorder = productAllowsBackorders(product);

				InventoryLevels inventoryLevel = Product.CheckStock(productID);
				var inventoryCount = inventoryLevel.QuantityAvailable ?? 0;
				if (inventoryLevel.IsOutOfStock)
				{
					response.CannotAddOutOfStock = quantity;
					return response;
				}
				response.CanAddNormally = Math.Min((int)inventoryCount, quantity);
				if (allowBackorder)
				{
					response.CanAddBackorder = quantity - response.CanAddNormally;
				}
				else
				{
					response.CannotAddOutOfStock = quantity - response.CanAddNormally;
				}
			}
			catch
			{
				response.CannotAddInvalid = quantity;
			}

			return response;
		}

		private bool productAllowsBackorders(Product product)
	    {
	        switch (product.ProductBackOrderBehaviorID)
	        {
	            case (int)Constants.ProductBackOrderBehavior.AllowBackorder:
	            case (int)Constants.ProductBackOrderBehavior.AllowBackorderInformCustomer:
	                return true;
	            default:
	                return false;
	        }
	    }

		private Product getProduct(int productID)
		{
			var product = Create.New<InventoryBaseRepository>().GetProduct(productID);
			return product;
		}

		private class InventoryProductCheckResponse : IInventoryProductCheckResponse
		{
			public int CanAddBackorder { get; set; }

			public int CanAddNormally { get; set; }

			public int CannotAddInactive { get; set; }

			public int CannotAddInvalid { get; set; }

			public int CannotAddOutOfStock { get; set; }
		}
	}
}