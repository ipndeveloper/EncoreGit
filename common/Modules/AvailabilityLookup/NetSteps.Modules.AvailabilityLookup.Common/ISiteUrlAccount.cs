using NetSteps.Encore.Core.Dto;

namespace NetSteps.Modules.AvailabilityLookup.Common
{
	/// <summary>
	/// Site Info
	/// </summary>
	[DTO]
	public interface ISiteUrlAccount
	{
		/// <summary>
		/// Site ID
		/// </summary>
		int SiteID { get; set; }
		/// <summary>
		/// Market ID
		/// </summary>
		int MarketID { get; set; }
		/// <summary>
		/// Account ID
		/// </summary>
		int AccountID { get; set; }
		/// <summary>
		/// URL
		/// </summary>
		string Url { get; set; }
	}
}
