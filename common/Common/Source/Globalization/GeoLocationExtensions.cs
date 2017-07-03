using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Common.Globalization
{
    public static class GeoLocationExtensions
    {
        /// <summary>
        /// Filters a sequence of <see cref="IGeoCode"/> items based on their distance from a specified location,
        /// and wraps the items in an outer class that contains their distance.
        /// </summary>
        public static IQueryable<IGeoCodeItemWithDistance<T>> FilterByDistance<T>(
            this IQueryable<T> query,
            Func<IQueryable<T>, IGeoCodeDistanceSelectorParameters, IQueryable<IGeoCodeItemWithDistance<T>>> distanceSelector,
            double latitude,
            double longitude,
            GeoLocation.DistanceType distanceType,
            double maximumDistance)
            where T : IGeoCode
        {
            Contract.Requires<ArgumentNullException>(query != null);
            Contract.Requires<ArgumentOutOfRangeException>(latitude >= GeoLocation.MIN_LAT_DEGREES && latitude <= GeoLocation.MAX_LAT_DEGREES);
            Contract.Requires<ArgumentOutOfRangeException>(longitude >= GeoLocation.MIN_LON_DEGREES && longitude <= GeoLocation.MAX_LON_DEGREES);
            Contract.Requires<ArgumentOutOfRangeException>(maximumDistance > 0);

            var geoCodeRange = GeoLocation.CalculateGeoCodeRange(
                latitude,
                longitude,
                maximumDistance,
                distanceType
            );

            return query
                .WhereInGeoCodeRange(geoCodeRange)
                .SelectWithDistance(
                    distanceSelector,
                    latitude,
                    longitude,
                    distanceType
                )
                .Where(x => x.Distance <= maximumDistance);
        }

        /// <summary>
        /// Wraps a sequence of <see cref="IGeoCode"/> items in an outer class that contains their distance from a specified location.
        /// </summary>
        public static IQueryable<IGeoCodeItemWithDistance<T>> SelectWithDistance<T>(
            this IQueryable<T> query,
            Func<IQueryable<T>, IGeoCodeDistanceSelectorParameters, IQueryable<IGeoCodeItemWithDistance<T>>> distanceSelector,
            double latitude,
            double longitude,
            GeoLocation.DistanceType distanceType)
            where T : IGeoCode
        {
            Contract.Requires<ArgumentNullException>(query != null);
            Contract.Requires<ArgumentOutOfRangeException>(latitude >= GeoLocation.MIN_LAT_DEGREES && latitude <= GeoLocation.MAX_LAT_DEGREES);
            Contract.Requires<ArgumentOutOfRangeException>(longitude >= GeoLocation.MIN_LON_DEGREES && longitude <= GeoLocation.MAX_LON_DEGREES);

            double earthRadius = distanceType == GeoLocation.DistanceType.Miles
                ? GeoLocation.EARTH_RADIUS_MILES
                : GeoLocation.EARTH_RADIUS_KILOMETERS;

            var distanceSelectorParameters = Create.New<IGeoCodeDistanceSelectorParameters>();
            distanceSelectorParameters.Latitude = latitude;
            distanceSelectorParameters.Longitude = longitude;
            distanceSelectorParameters.Radius = earthRadius;

            return distanceSelector(
                query,
                distanceSelectorParameters
            );
        }
    }
}
