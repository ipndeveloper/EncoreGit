using NetSteps.Addresses.UI.Common.Models;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Addresses.UI.Mvc.Common.Models
{
	[DTO]
	public interface IAddressEditorModel
	{
		IAddressUIModel Address { get; set; }
		IAddressCountrySettingsModel Settings { get; set; }
	}
}
