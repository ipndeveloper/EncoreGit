using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BelcorpUSA.Edi.Common.Orders;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Diagnostics.Utilities;

namespace BelcorpUSA.Edi.Service
{
	internal class ShipNoticeProcessor
	{
		public void ProcessShipNotice(Edi856ShipNotice notice)
		{
			using (NetStepsEntities nse = new NetStepsEntities())
			{
				var shipment = nse.OrderShipments.FirstOrDefault(s => s.OrderShipmentID == notice.ShipmentId);
				var orderShipmentShipped = SmallCollectionCache.Instance.OrderShipmentStatuses.First(f => f.Name == "Shipped");
				if (shipment != null)
				{
					var shippingMethod = SmallCollectionCache.Instance.ShippingMethods.FirstOrDefault(sm => sm.ShortName == notice.ShipmentDetails.IdentificationCode);
					int? shippingMethodId = null;
					if (shippingMethod != null)
					{
						shippingMethodId = shippingMethod.ShippingMethodID;
					}
					shipment.OrderShipmentStatusID = orderShipmentShipped.OrderShipmentStatusID;
					shipment.TrackingNumber = String.Join(", ", notice.ShipmentDetails.TrackingNumbers);
					shipment.DateShippedUTC = notice.ShippedDateUTC;
					if (shippingMethodId.HasValue)
					{
						shipment.ShippingMethodID = shippingMethodId;
					}

					int p = 0;
					foreach (var pack in notice.Containers)
					{
						var osPack = nse.OrderShipmentPackages.CreateObject();
						osPack.StartEntityTracking();
						if (notice.ShipmentDetails.TrackingNumbers.Count > p)
						{
							osPack.TrackingNumber = notice.ShipmentDetails.TrackingNumbers[p++];
						}
						osPack.ShippingMethodID = shipment.ShippingMethodID;
						osPack.DateShippedUTC = notice.ShippedDateUTC;
						osPack.DateCreatedUTC = DateTime.UtcNow;
						osPack.DateLastModifiedUTC = osPack.DateCreatedUTC;
						osPack.OrderShipmentID = shipment.OrderShipmentID;

						foreach (var item in pack.Items)
						{
							foreach (var ospi in CreateOrderShipmentPackageItemForShipmentItem(nse, item, shipment.OrderShipmentID))
							{
								osPack.OrderShipmentPackageItems.Add(ospi);
							}
							UpdateWarehouseProductCounts(nse, item, notice.ShipTo);
						}

						nse.OrderShipmentPackages.AddObject(osPack);
					}

					nse.SaveChanges();

					var order = nse.Orders.FirstOrDefault(o => o.OrderID == shipment.OrderID);
					bool allShipped = false;
					if (order != null)
					{
						var ostatExpected = SmallCollectionCache.Instance.OrderStatuses.Where(os => os.Name == "Printed" || os.Name == "PartiallyShipped");
						if (!ostatExpected.Any(os => order.OrderStatusID == os.OrderStatusID))
						{
							var ostatActual = SmallCollectionCache.Instance.OrderStatuses.Where(os => os.OrderStatusID == order.OrderStatusID).First();
							var msg = String.Format("Order '{0}' has recieved a shipping notice, but the order status is '{1}', expected '{2}'... this could indicate an error state.  Order will be updated with shipping information. Shipping notice filename: {3}", order.OrderID, ostatActual.Name, String.Join(" or ", ostatExpected.Select(os => os.Name)), notice.FileName);
							this.TraceWarning(msg);
							var noteTypeId = SmallCollectionCache.Instance.NoteTypes.Where(nt => nt.Name == "Order Internal Notes").First().NoteTypeID;
							var note = nse.Notes.CreateObject();
							note.DateCreatedUTC = DateTime.UtcNow;
							note.NoteText = msg;
							note.NoteTypeID = noteTypeId;
							order.Notes.Add(note);
						}

						if (nse.OrderShipments.Where(oship => oship.OrderID == order.OrderID).Any(oship => oship.OrderShipmentStatusID == orderShipmentShipped.OrderShipmentStatusID))
						{
							if (nse.OrderShipments.Where(oship => oship.OrderID == order.OrderID).All(oship => oship.OrderShipmentStatusID == orderShipmentShipped.OrderShipmentStatusID))
							{
								order.OrderStatusID = SmallCollectionCache.Instance.OrderStatuses.First(f => f.Name == "Shipped").OrderStatusID;
								allShipped = true;
							}
							else
							{
								order.OrderStatusID = SmallCollectionCache.Instance.OrderStatuses.First(f => f.Name == "PartiallyShipped").OrderStatusID;
							}
						}

						nse.SaveChanges();
						if (allShipped)
						{
							DomainEventQueueItem.AddOrderShippedEventToQueue(order.OrderID);
						}
					}
					else
					{
						this.TraceError(String.Format("Recieved Ship Notice for Order ID: '{0}' and OrderShipmentID: '{1}'.  Unable to load an order by that ID!", notice.OrderId, notice.ShipmentId));
					}
				}
				else
				{
					this.TraceError(String.Format("Recieved Ship Notice for Order ID: '{0}' and OrderShipmentID: '{1}'.  Unable to load a shippment by those IDs!", notice.OrderId, notice.ShipmentId));
				}
			}
		}

		protected IEnumerable<OrderShipmentPackageItem> CreateOrderShipmentPackageItemForShipmentItem(NetStepsEntities nse, Edi856ShipNotice.EdiShipmentContainer.EdiShipmentItem item, int shipmentId)
		{
			List<OrderShipmentPackageItem> ospItems = new List<OrderShipmentPackageItem>();
			var ttlShipped = item.QuantityShipped;
			var orderItems = (from oi in nse.OrderItems
							  join oc in nse.OrderCustomers on oi.OrderCustomerID equals oc.OrderCustomerID
							  join o in nse.Orders on oc.OrderID equals o.OrderID
							  join os in nse.OrderShipments on o.OrderID equals os.OrderID
							  join p in nse.Products on oi.ProductID equals p.ProductID
							  join pp in nse.ProductProperties on p.ProductID equals pp.ProductID
							  join ppt in nse.ProductPropertyTypes on pp.ProductPropertyTypeID equals ppt.ProductPropertyTypeID
							  where ppt.Name == "SAP Code"
							  && pp.PropertyValue == item.ProductCode
							  && os.OrderShipmentID == shipmentId
							  select oi);
			foreach (var oi in orderItems)
			{
				if (ttlShipped >= oi.Quantity)
				{
					var ospItem = nse.OrderShipmentPackageItems.CreateObject();
					ospItem.StartEntityTracking();
					ospItem.Quantity = oi.Quantity;
					ospItem.OrderItem = oi;
					ospItem.DateCreatedUTC = DateTime.UtcNow;
					ospItem.DateLastModifiedUTC = ospItem.DateCreatedUTC;
					ospItems.Add(ospItem);

					ttlShipped -= oi.Quantity;
				}
			}

			return ospItems;
		}

		protected void UpdateWarehouseProductCounts(NetStepsEntities nse, Edi856ShipNotice.EdiShipmentContainer.EdiShipmentItem item, EdiGeographicLocation shipTo)
		{
			var wareProd = (from wp in nse.WarehouseProducts
							join w in nse.Warehouses on wp.WarehouseID equals w.WarehouseID
							join sr in nse.ShippingRegions on w.WarehouseID equals sr.WarehouseID
							join sp in nse.StateProvinces on sr.ShippingRegionID equals sp.ShippingRegionID
							join p in nse.Products on wp.ProductID equals p.ProductID
							join pp in nse.ProductProperties on p.ProductID equals pp.ProductID
							join ppt in nse.ProductPropertyTypes on pp.ProductPropertyTypeID equals ppt.ProductPropertyTypeID
							where ppt.Name == "SAP Code"
							&& pp.PropertyValue == item.ProductCode
							&& sp.StateAbbreviation == shipTo.StateProvinceCode
							select wp).FirstOrDefault();
			if (wareProd != null)
			{
				wareProd.StartEntityTracking();
				var newAllocation = wareProd.QuantityAllocated - item.QuantityShipped;
				if (newAllocation < 0)
				{
					this.TraceWarning(String.Format("Shipping adjustment for Warehouse Product (Id: {0}) allocation resulted in a negative value (v: {1})... Missed update, or out of bounds shipment occured, setting value to 0.", wareProd.WarehouseProductID, newAllocation));
					newAllocation = 0;
				}
				wareProd.QuantityAllocated = newAllocation;
				wareProd.QuantityOnHand -= item.QuantityShipped;
			}
		}
	}
}
