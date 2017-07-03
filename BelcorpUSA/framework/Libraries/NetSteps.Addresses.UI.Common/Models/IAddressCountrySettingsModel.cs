using NetSteps.Encore.Core.Dto;

namespace NetSteps.Addresses.UI.Common.Models
{
	[DTO]
	public interface IAddressCountrySettingsModel
	{
		string ForCountryCode { get; set; }
		bool RequiresScrubbing { get; set; }
		bool PostalCodeLookupEnabled { get; set; }
	}
}
