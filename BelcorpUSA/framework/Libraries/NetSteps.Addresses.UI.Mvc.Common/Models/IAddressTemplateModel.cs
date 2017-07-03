using System.Collections.Generic;
using NetSteps.Data.Entities;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Addresses.UI.Mvc.Common.Models
{
	[DTO]
	public interface IAddressTemplateModel
	{
		IAddressEditorModel AddressEditor { get; set; }
		IEnumerable<Country> AvailableCountries { get; set; }
	}
}
