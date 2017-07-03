using NetSteps.Common.Globalization;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.AccountLocatorService.Common
{
	[DTO]
	public interface IAccountLocatorServiceResult
	{
		int AccountId { get; set; }
		string FirstName { get; set; }
		string LastName { get; set; }
		string City { get; set; }
		string State { get; set; }
		int? CountryId { get; set; }
		double? Distance { get; set; }
		GeoLocation.DistanceType DistanceType { get; set; }
		string PwsUrl { get; set; }
		string PhotoContent { get; set; }
		bool IsPhotoContentHtmlEncoded { get; set; }
        string EmailAddress { get; set; }
        string PhoneNumber { get; set; }

	}
}
