namespace NetSteps.Data.Common.Orders
{
	using System;

	/// <summary>
	/// The order package info model.
	/// </summary>
	public class OrderPackageInfoModel
	{
		/// <summary>
		/// Gets or sets the ship method name.
		/// </summary>
		public string ShipMethodName { get; set; }

		/// <summary>
		/// Gets or sets the ship date.
		/// </summary>
		public DateTime ShipDate { get; set; }

		/// <summary>
		/// Gets or sets the base track url.
		/// </summary>
		public string BaseTrackUrl { get; set; }

		/// <summary>
		/// Gets or sets the tracking number.
		/// </summary>
		public string TrackingNumber { get; set; }

		/// <summary>
		/// Gets or sets the tracking url.
		/// </summary>
		public string TrackingUrl { get; set; }
	}
}
