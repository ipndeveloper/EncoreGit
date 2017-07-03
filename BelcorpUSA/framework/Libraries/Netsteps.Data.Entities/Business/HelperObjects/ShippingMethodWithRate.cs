using System;

namespace NetSteps.Data.Entities.Business
{
	[Serializable]
	public class ShippingMethodWithRate
	{
		public int ShippingMethodID { get; set; }
		public string Name { get; set; }
		public string DisplayName { get; set; }
		public string Description { get; set; }
		public string ShortName { get; set; }
		public bool AllowDirectShipments { get; set; }
		public bool IsDefaultShippingMethod { get; set; }
		public decimal ShippingAmount { get; set; }
		public decimal? ShippingPercentage { get; set; }
		public decimal DirectShipmentFee { get; set; }
		public decimal HandlingFee { get; set; }
		public short? ShippingRateTypeID { get; set; }
		public int ShippingRateID { get; set; }
        public string DateEstimated { get; set; }
        public int ShippingMethodID2 { get; set; }
	}
}
