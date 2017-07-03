using System.Collections.Generic;
using AddressValidation.Common;
using NetSteps.Addresses.UI.Common.Models;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Addresses.UI.Common.Services
{
	[DTO]
	public interface IAddressValidationResult2
	{
		string Message { get; set; }
		AddressInfoResultState Status { get; set; }
		IEnumerable<IAddressUIModel> ValidAddresses { get; set; }
	}
}
