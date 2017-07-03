using System.Collections.Generic;

using NetSteps.Common.Base;
using NetSteps.Common.Globalization;

namespace NetSteps.AccountLocatorService.Common
{
	public interface IAccountLocatorServiceSearchParameters : IPaginatedListParameters
	{
		IEnumerable<short> AccountTypeIDs { get; }
		string AccountNumber { get; }
		string FirstName { get; }
		string LastName { get; }
		bool RequirePwsUrl { get; }

		// Location search
		double? Latitude { get; }
		double? Longitude { get; }
		double? MaximumDistance { get; }
		GeoLocation.DistanceType DistanceType { get; }
	}
}
