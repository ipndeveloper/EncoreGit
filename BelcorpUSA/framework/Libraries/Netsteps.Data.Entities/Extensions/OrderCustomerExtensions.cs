using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Cache;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities
{
	/// <summary>
	/// Author: John Egbert
	/// Description: OrderCustomer Extensions
	/// Created: 11-21-2011
	/// </summary>
	public static class OrderCustomerExtensions
	{
		public static bool ContainsShippableItems(this OrderCustomer orderCustomers)
		{
			bool containsShippableItems = false;

			var inventory = Create.New<InventoryBaseRepository>();

			foreach (OrderItem orderItem in orderCustomers.OrderItems)
			{
				var product = inventory.GetProduct(orderItem.ProductID.ToInt());
				if (product.ProductBase.IsShippable)
					return containsShippableItems = true;
			}

			return containsShippableItems;
		}
	}
}
