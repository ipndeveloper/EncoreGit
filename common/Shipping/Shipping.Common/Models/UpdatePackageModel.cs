namespace NetSteps.Shipping.Common.Models
{
	using System.Collections;

	public class UpdatePackageModel
	{
		public int OrderID { get; set; }
		public int OrderShipmentID { get; set; }
		public int? OrderShipmentPackageID { get; set; }
		public string TrackingNumber { get; set; }
		public string TrackingURL { get; set; }
		public string DateShipped { get; set; }
		public object DataOriginTag { get; set; }
	}
}
