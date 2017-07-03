using System;
using NetSteps.Encore.Core.Dto;


namespace NetSteps.Modules.Autoship.Common
{
	/// <summary>
	/// Autoship Order Model
	/// </summary>
	[DTO]
	public interface ISiteAutoship
	{
		/// <summary>
		/// AccountID
		/// </summary>
		int AccountID { get; set; }
		/// <summary>
		/// OrderID
		/// </summary>
		int OrderID { get; set; }
		/// <summary>
		/// Date the order was created
		/// </summary>
		DateTime OrderDate { get; set; }
		/// <summary>
		/// Grand total for the order.
		/// </summary>
		decimal OrderTotal { get; set; }
		/// <summary>
		/// Bonus Volume
		/// </summary>
		decimal BonusVolume { get; set; }
		/// <summary>
		/// Autoship Schedule for the autoship
		/// </summary>
		int AutoShipScheduleID { get; set; }
	}
}
