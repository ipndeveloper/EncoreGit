using System.Collections.Generic;

using NetSteps.AccountLocatorService.Common;
using NetSteps.Common.Base;
using NetSteps.Common.Globalization;

namespace NetSteps.AccountLocatorService
{
	public class AccountLocatorServiceSearchParameters : PaginatedListParameters, IAccountLocatorServiceSearchParameters
	{
		public IEnumerable<short> AccountTypeIDs { get; set; }
		public string AccountNumber { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public bool RequirePwsUrl { get; set; }

		public double? Latitude { get; set; }
		public double? Longitude { get; set; }
		public double? MaximumDistance { get; set; }
		public GeoLocation.DistanceType DistanceType { get; set; }
	}
}
