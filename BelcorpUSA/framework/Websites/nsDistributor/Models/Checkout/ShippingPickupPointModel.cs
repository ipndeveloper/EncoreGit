using NetSteps.Addresses.PickupPoints.Common.Models;

namespace nsDistributor.Models.Checkout
{
	public class ShippingPickupPointModel
	{
		#region Properties
		public string PickupPointCode { get; set; }
		public int Distance { get; set; }
		public string Name { get; set; }
		public string Location { get; set; }
		#endregion

		#region Constructors
		public ShippingPickupPointModel() : this(null) { }
		public ShippingPickupPointModel(IPickupPointModel copyFrom)
		{
			if (copyFrom != default(IPickupPointModel))
				CopyFrom(copyFrom);
		}
		#endregion

		#region Infrastructure
		public void CopyFrom(IPickupPointModel pickupPoint)
		{
			this.PickupPointCode = pickupPoint.PickupPointCode;
			this.Name = pickupPoint.Name;
			this.Distance = pickupPoint.Distance;
			this.Location = 
				string.Join(" ", string.Format("{0}{1}{2},{3},{4}{5}",
					pickupPoint.PickupPointAddress.Address1.Trim(),
					string.IsNullOrWhiteSpace(pickupPoint.PickupPointAddress.Address2) ? string.Empty : " " + pickupPoint.PickupPointAddress.Address2.Trim(),
					string.IsNullOrWhiteSpace(pickupPoint.PickupPointAddress.Address3) ? string.Empty : " " + pickupPoint.PickupPointAddress.Address3.Trim(),
					string.IsNullOrWhiteSpace(pickupPoint.PickupPointAddress.City) ? string.Empty : " " + pickupPoint.PickupPointAddress.City.Trim(),
					string.IsNullOrWhiteSpace(pickupPoint.PickupPointAddress.State) ? string.Empty : " " + pickupPoint.PickupPointAddress.State.Trim(),
					string.IsNullOrWhiteSpace(pickupPoint.PickupPointAddress.PostalCode) ? string.Empty : " " + pickupPoint.PickupPointAddress.PostalCode.Trim())
				.Split(' '));
		}
		#endregion
	}
}