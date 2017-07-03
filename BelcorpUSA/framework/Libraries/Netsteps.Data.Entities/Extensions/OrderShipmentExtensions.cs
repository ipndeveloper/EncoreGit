using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Cache;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities
{
    /// <summary>
    /// Author: John Egbert
    /// Description: OrderShipment Extensions
    /// Created: 04-14-2010
    /// </summary>
    public static class OrderShipmentExtensions
    {
        public static bool ContainsShippableItems(this IEnumerable<OrderCustomer> orderCustomers)
        {
            bool containsShippableItems = false;

            var inventory = Create.New<InventoryBaseRepository>();

            foreach (OrderCustomer oc in orderCustomers)
            {
                foreach (OrderItem orderItem in oc.OrderItems)
                {
                    var product = inventory.GetProduct(orderItem.ProductID.ToInt());
                    if (product.ProductBase.IsShippable)
                        return containsShippableItems = true;
                }
            }

            return containsShippableItems;
        }

        public static IEnumerable<OrderItem> GetOrderItems(this OrderShipment orderShipment)
        {
            if (orderShipment != null)
            {
                if (orderShipment.OrderCustomer != null)
                {
                    return orderShipment.OrderCustomer.OrderItems;
                }

                return orderShipment.Order.OrderCustomers
                    .Where(x => !x.OrderShipments.Any())
                    .SelectMany(x => x.OrderItems);
            }
            return Enumerable.Empty<OrderItem>();
        }

		public static IEnumerable<OrderItem> GetShippableOrderItems(this OrderShipment orderShipment)
		{
			if (orderShipment != null)
			{

				var inventory = Create.New<InventoryBaseRepository>();
				//Only return orderItems that have a product marked as IsShippable
				List<OrderCustomer> orderCustomers = new List<OrderCustomer>();

				//Identify the orderCustomers that we need to ship items for
				if (orderShipment.OrderCustomer != null)
				{
					orderCustomers.Add(orderShipment.OrderCustomer);
				}
				else
				{
					orderCustomers.AddRange(orderShipment.Order.OrderCustomers.Where(x => !x.OrderShipments.Any()));
				}

				List<OrderItem> orderItems = new List<OrderItem>();
				//Identify the shippable order items via the productBase
				foreach (OrderCustomer orderCustomer in orderCustomers)
				{
					foreach (OrderItem item in orderCustomer.OrderItems)
					{
						var product = inventory.GetProduct(item.ProductID.Value);
						if (product.ProductBase.IsShippable)
						{
							orderItems.Add(item);
						}
					}
				}

				return orderItems;

			}
			return Enumerable.Empty<OrderItem>();
		}
    }
}
