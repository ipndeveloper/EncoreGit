using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using NetSteps.Common.Base;
using NetSteps.Common.Globalization;

namespace NetSteps.Data.Entities.Business
{
    public interface IAccountLocatorSearchParameters : IPaginatedListParameters
    {
        IEnumerable<short> AccountTypeIDs { get; }
        string AccountNumber { get; }
        string FirstName { get; }
        string LastName { get; }
        bool RequirePwsUrl { get; }

        // Geolocation search
        double? Latitude { get; }
        double? Longitude { get; }
        double? MaximumDistance { get; }
        GeoLocation.DistanceType DistanceType { get; }
        DateTime? LatestDateToHaveHadACommissionPayout { get; set; }
    }

    public class AccountLocatorSearchParameters : PaginatedListParameters, IAccountLocatorSearchParameters
    {
        public IEnumerable<short> AccountTypeIDs { get; set; }
        public string AccountNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool RequirePwsUrl { get; set; }

        // Geolocation search
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double? MaximumDistance { get; set; }
        public GeoLocation.DistanceType DistanceType { get; set; }
        public DateTime? LatestDateToHaveHadACommissionPayout { get; set; }
    }

    public static class IAccountLocatorSearchParametersExtensions
    {
        public static bool IsGeoLocationSearch(this IAccountLocatorSearchParameters searchParameters)
        {
            Contract.Requires<ArgumentNullException>(searchParameters != null);

            return
                searchParameters.Latitude >= GeoLocation.MIN_LAT_DEGREES
                && searchParameters.Latitude <= GeoLocation.MAX_LAT_DEGREES
                && searchParameters.Longitude >= GeoLocation.MIN_LON_DEGREES
                && searchParameters.Longitude <= GeoLocation.MAX_LON_DEGREES
                && searchParameters.MaximumDistance > 0;
        }
    }
}
