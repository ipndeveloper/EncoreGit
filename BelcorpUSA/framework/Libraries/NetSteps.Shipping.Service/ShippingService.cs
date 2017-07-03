namespace NetSteps.Shipping.Service
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.Linq;

	using NetSteps.Common.Globalization;
	using NetSteps.Data.Entities;
	using NetSteps.Data.Entities.Extensions;
	using NetSteps.Encore.Core.IoC;
	using NetSteps.Shipping.Common;
	using NetSteps.Shipping.Common.Models;

	[ContainerRegister(typeof(IShippingService), RegistrationBehaviors.Default)]
	public class ShippingService : IShippingService
	{
		protected virtual string _errorNoOrdersWereUpdated { get { return Translation.GetTerm("ErrorNoOrdersWereUpdated", "No orders were updated."); } }
		protected virtual string _errorUpdatingOrder(string orderNumber, string message) { return Translation.GetTerm("ErrorUpdatingOrder", "There was an error updating order {0}: {1}", orderNumber, message); }
		protected virtual string _errorRetrievingOrder(int orderID) { return Translation.GetTerm("ErrorRetrievingOrder", "There was an error retrieving OrderID {0}.", orderID); }
		protected virtual string _errorRetrievingOrderShipment(string orderNumber) { return Translation.GetTerm("ErrorRetrievingOrderShipment", "There was an error retrieving the shipment for order {0}.", orderNumber); }
		protected virtual string _errorRetrievingOrderShipmentPackage(string orderNumber) { return Translation.GetTerm("ErrorRetrievingOrderShipmentPackage", "There was an error retrieving the package info for order {0}.", orderNumber); }
		protected virtual string _errorOrderIsNotPaidInFull(string orderNumber) { return Translation.GetTerm("ErrorOrderIsNotPaidInFull", "Order {0} is not paid in full.", orderNumber); }


		public virtual List<string> UpdatePackages(IEnumerable<UpdatePackageModel> updatePackageModels)
		{
			List<string> errors = new List<string>();

			// Check for no models.
			if (updatePackageModels == null || !updatePackageModels.Any())
			{
				errors.Add(this._errorNoOrdersWereUpdated);
				return errors;
			}

			try
			{
				// Get distinct order IDs and load orders.
				var orderIDs = updatePackageModels
					 .Select(x => x.OrderID)
					 .Distinct();

				var orders = Order.LoadBatchWithShipmentDetails(orderIDs, true);

				// Process each order.
				// Continue on errors - partial success is better than no success.
				foreach (int orderID in orderIDs)
				{
					// Get order.
					var order = orders.FirstOrDefault(x => x.OrderID == orderID);

					// Sanity check.
					if (order == null)
					{
						errors.Add(this._errorRetrievingOrder(orderID));
						continue;
					}

					this.UpdatePackagesForOrder(order, updatePackageModels.Where(x => x.OrderID == orderID), errors);
				}
			}
			catch (Exception ex)
			{
				errors.Add(ex.Log().PublicMessage);
			}

			return errors;
		}


		protected void OnOrderStatusChanged(Order order, short originalOrderStatusID)
		{
			if (order.OrderStatusID == (short)Constants.OrderStatus.Shipped)
			{
				this.OnOrderStatusChangedToShipped(order);
			}
		}


		protected void OnOrderStatusChangedToShipped(Order order)
		{
			Contract.Requires(order != null);

			// Order shipped domain event
			this.FireOrderShippedDomainEvent(order);
			
			// Update the Quantity On Hand and Quantity Allocated values in the warehouse
			this.UpdateWarehouseProductsForOrder(order);
		}


		protected void RecalculateOrderStatus(Order order)
		{
			Contract.Requires(order != null);

			// Recalculate status of order.
			var newOrderStatusID = Order.CalculateOrderShippedStatus(order);

			if (order.OrderStatusID != newOrderStatusID)
			{
				// Set new order status
				var originalOrderStatusID = order.OrderStatusID;
				order.OrderStatusID = newOrderStatusID;
				this.OnOrderStatusChanged(order, originalOrderStatusID);
			}
		}


		protected void UpdatePackagesForOrder(Order order, IEnumerable<UpdatePackageModel> updatePackageModels, List<string> errors)
		{
			Contract.Requires(order != null);
			Contract.Requires(updatePackageModels != null);
			Contract.Requires(errors != null);

			try
			{
				// Don't allow modifications to UNSHIPPED orders if they aren't paid in full.
				if (order.OrderStatusID != (short)Constants.OrderStatus.Shipped
					 && !order.IsPaidInFull())
				{
					errors.Add(this._errorOrderIsNotPaidInFull(order.OrderNumber));
					return;
				}

				// A list to keep track of which order shipments are modified.
				var modifiedOrderShipments = new List<OrderShipment>();

				// Process each update for the order.
				foreach (var updatePackageModel in updatePackageModels)
				{
					// Get shipment.
					var shipment = order.OrderShipments.FirstOrDefault(x => x.OrderShipmentID == updatePackageModel.OrderShipmentID);

					// Make sure shipment exists.
					if (shipment == null)
					{
						errors.Add(this._errorRetrievingOrderShipment(order.OrderNumber));
						continue;
					}

					// Add shipment to modified list.
					if (!modifiedOrderShipments.Contains(shipment))
					{
						modifiedOrderShipments.Add(shipment);
					}

					// Scrub values.
					string trackingNumber = (updatePackageModel.TrackingNumber ?? "").Trim();
					DateTime dateShipped;
					if (!DateTime.TryParse(updatePackageModel.DateShipped, out dateShipped))
					{
						dateShipped = DateTime.Now;
					}

					// Check if this is an update for an existing package.
					if (updatePackageModel.OrderShipmentPackageID != null)
					{
						// Update existing package.
						var package = shipment.OrderShipmentPackages.FirstOrDefault(x => x.OrderShipmentPackageID == updatePackageModel.OrderShipmentPackageID.Value);

						// Make sure package exists.
						if (package == null)
						{
							errors.Add(this._errorRetrievingOrderShipmentPackage(order.OrderNumber));
							continue;
						}

						// If tracking number is blank or null, delete the package.
						if (string.IsNullOrWhiteSpace(trackingNumber))
						{
							// Delete package items.
							package.OrderShipmentPackageItems.RemoveAllAndMarkAsDeleted();

							// Delete package (it will automatically remove itself from shipment.OrderShipmentPackages).
							package.MarkAsDeleted();
						}
						else
						{
							// Set values.
							package.TrackingNumber = trackingNumber;
							package.DateShipped = dateShipped;
						}
					}
					else
					{
						// This is a new package.

						// Make sure there are no packages.
						if (shipment.OrderShipmentPackages.Any())
						{
							errors.Add(this._errorRetrievingOrderShipmentPackage(order.OrderNumber));
							continue;
						}

						// Create a new package.
						var package = new OrderShipmentPackage
						{
							TrackingNumber = trackingNumber,
							DateShipped = dateShipped,
							ShippingMethodID = shipment.ShippingMethodID
						};

						// Add the items to the package.
						package.OrderShipmentPackageItems.AddRange(
							 shipment.GetOrderItems().Select(x => new OrderShipmentPackageItem
							 {
								 OrderItemID = x.OrderItemID,
								 Quantity = x.Quantity
							 })
						);

						// Add the package to the collection.
						shipment.OrderShipmentPackages.Add(package);
					}
				}

				// Recalculate status of modified order shipments.
				modifiedOrderShipments.ForEach(x =>
					 x.OrderShipmentStatusID = OrderShipment.CalculateOrderShipmentStatus(x)
				);

				this.RecalculateOrderStatus(order);

				// Commit each order one at a time.
				order.Save();
			}
			catch (Exception ex)
			{
				errors.Add(this._errorUpdatingOrder(order.OrderNumber, ex.Log().PublicMessage));
			}
		}


		private void FireOrderShippedDomainEvent(Order order)
		{
			try
			{
				DomainEventQueueItem.AddOrderShippedEventToQueue(order.OrderID);
			}
			catch (Exception ex)
			{
				ex.Log(orderID: order.OrderID);
			}
		}


		private void UpdateWarehouseProductsForOrder(Order order, bool removeFromInventory = true)
		{
			Contract.Requires(order != null);

			foreach (OrderShipment shipment in order.OrderShipments)
			{
				this.UpdateWarehouseProductsForShipment(shipment, removeFromInventory);
			}
		}


		private void UpdateWarehouseProductsForShipment(OrderShipment orderShipment, bool removeFromInventory = true)
		{
			Contract.Requires(orderShipment != null);

			foreach (OrderItem orderItem in orderShipment.GetShippableOrderItems())
			{
				this.UpdateWarehouseProductForShipment(orderShipment, orderItem, removeFromInventory);
			}
		}


		private void UpdateWarehouseProductForShipment(OrderShipment orderShipment, OrderItem orderItem, bool removeFromInventory = true)
		{
			Contract.Requires(orderShipment != null);
			Contract.Requires(orderItem != null);
			Contract.Requires(orderItem.ProductID.HasValue);

			WarehouseProduct warehouseProduct = WarehouseProduct.GetWarehouseProduct(orderShipment, orderItem.ProductID.Value);
			warehouseProduct.StartEntityTracking();

			if (removeFromInventory)
			{
				warehouseProduct.QuantityAllocated -= orderItem.Quantity;
				warehouseProduct.QuantityOnHand -= orderItem.Quantity;
			}
			else
			{
				warehouseProduct.QuantityAllocated += orderItem.Quantity;
				warehouseProduct.QuantityOnHand += orderItem.Quantity;
			}

			warehouseProduct.Save();
		}
	}
}
