using System;

namespace NetSteps.Data.Entities.Business.HelperObjects.OrderPackages
{
	public class OrderPackageInfoModel
	{
		public int? OrderCustomerID { get; set; }
		public string ShipMethodName { get; set; }
		public DateTime ShipDate { get; set; }
		public string BaseTrackUrl { get; set; }
		public string TrackingNumber { get; set; }
		public string TrackingUrl { get; set; }
	}
}
