using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Diagnostics.Utilities;
using System.IO;
using BelcorpUSA.Edi.Common.Orders;

namespace BelcorpUSA.Edi.Service.Data
{
	public static class EdiInterchangeTrackerExtensions
	{
		public static void TerminateInterchangeTracking(this EdiInterchangeTracker interchangeTracking, string reason)
		{
			interchangeTracking.TraceWarning(String.Format("The Interchange Tracking (Id:{0}) started for Order Type '{1}' at '{2:yyyy/MM/dd H:mm:ss}' is being terminated for reason: '{3}'.  See previous Trace information for details.", interchangeTracking.EdiInterchangeTrackerId, interchangeTracking.OrderType, interchangeTracking.DateCreatedUtc, reason));
			using (EdiTrackingDbContext ctx = new EdiTrackingDbContext())
			{
				ctx.EdiInterchangeTrackers.Attach(interchangeTracking);

				if (interchangeTracking.EdiPartnerTrackers.Any())
				{
					foreach (var part in interchangeTracking.EdiPartnerTrackers.ToArray())
					{
						if (File.Exists(part.FilePath))
						{
							File.Delete(part.FilePath);
						}
						ctx.EdiPartnerTrackers.Remove(part);
					}
				}

				if (interchangeTracking.EdiShipmentTrackers.Any())
				{
					foreach (var ship in interchangeTracking.EdiShipmentTrackers.ToArray())
					{
						ctx.EdiShipmentTrackers.Remove(ship);
					}
				}

				ctx.EdiInterchangeTrackers.Remove(interchangeTracking);
				ctx.SaveChanges();
			}
		}

		public static void AddPartnerTrackingToInterchangeTracking(this EdiInterchangeTracker interchangeTracking, string partnerName, string filePath)
		{
			using (EdiTrackingDbContext ctx = new EdiTrackingDbContext())
			{
				ctx.EdiInterchangeTrackers.Attach(interchangeTracking);
				var trackPartner = ctx.EdiPartnerTrackers.Create();
				trackPartner.PartnerName = partnerName;
				trackPartner.FilePath = filePath;
				interchangeTracking.EdiPartnerTrackers.Add(trackPartner);
				ctx.SaveChanges();
			}
		}

		public static void AddShipmentTrackingToInterchangeTracking(this EdiInterchangeTracker interchangeTracking, Edi850PurchaseOrderList orderList)
		{
			using (EdiTrackingDbContext ctx = new EdiTrackingDbContext())
			{
				ctx.EdiInterchangeTrackers.Attach(interchangeTracking);
				foreach (var item in orderList)
				{
					var trackItem = ctx.EdiShipmentTrackers.Create();
					trackItem.OrderId = item.OrderId;
					trackItem.OrderShipmentId = item.ShipmentId;
					interchangeTracking.EdiShipmentTrackers.Add(trackItem);
				}
				ctx.SaveChanges();
			}
		}
	}
}
