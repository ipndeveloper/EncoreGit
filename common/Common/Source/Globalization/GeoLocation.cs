using System;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using NetSteps.Encore.Core.Dto;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Common.Globalization
{
    /// <summary>
    /// Identifies an object with Latitude/Longitude coordinates.
    /// </summary>
    public interface IGeoCode
    {
        double? Latitude { get; }
        double? Longitude { get; }
    }

    /// <summary>
    /// A wrapper class for "appending" a distance property to any <see cref="IGeoCode"/> object.
    /// </summary>
    public interface IGeoCodeItemWithDistance<T>
        where T : IGeoCode
    {
        T GeoCodeItem { get; set; }
        double? Distance { get; set; }
    }

    [DTO]
    public interface IGeoCodeDistanceSelectorParameters
    {
        double Latitude { get; set; }
        double Longitude { get; set; }
        double Radius { get; set; }
    }

    [DTO]
    public interface IGeoCodeRange
    {
        double MinimumLatitude { get; set; }
        double MaximumLatitude { get; set; }
        double MinimumLongitude { get; set; }
        double MaximumLongitude { get; set; }
    }

    public class GeoCodeItemWithDistance<T> : IGeoCodeItemWithDistance<T>
        where T : IGeoCode
    {
        public T GeoCodeItem { get; set; }
        public double? Distance { get; set; }
    }

    /// <summary>
    /// Contains various geo-location utility methods.
    /// The mathematical calculations were adapted from Jan Philip Matuschek's article found here: http://janmatuschek.de/LatitudeLongitudeBoundingCoordinates
    /// </summary>
    public static class GeoLocation
    {
        // http://en.wikipedia.org/wiki/Earth_radius
        public static readonly double EARTH_RADIUS_KILOMETERS = 6371;
        public static readonly double EARTH_RADIUS_MILES = 3959;

        public static readonly double TO_RADIANS_FACTOR = Math.PI / 180d;
        public static readonly double TO_DEGREES_FACTOR = 180d / Math.PI;

        public enum DistanceType
        {
            Miles,
            Kilometers
        }

        public static readonly double MIN_LAT_DEGREES = -90;
        public static readonly double MAX_LAT_DEGREES = 90;
        public static readonly double MIN_LON_DEGREES = -180;
        public static readonly double MAX_LON_DEGREES = 180;
        public static readonly double MIN_LAT_RADIANS = -Math.PI / 2d;
        public static readonly double MAX_LAT_RADIANS = Math.PI / 2d;
        public static readonly double MIN_LON_RADIANS = -Math.PI;
        public static readonly double MAX_LON_RADIANS = Math.PI;

        /// <summary>
        /// Converts degrees to radians.
        /// </summary>
        public static double ToRadians(this double degrees)
        {
            return degrees * TO_RADIANS_FACTOR;
        }

        /// <summary>
        /// Converts radians to degrees.
        /// </summary>
        public static double ToDegrees(this double radians)
        {
            return radians * TO_DEGREES_FACTOR;
        }

        /// <summary>
        /// Converts distance (mi. or km.) to radians.
        /// </summary>
        public static double ToDistanceRadians(this double distance, DistanceType distanceType)
        {
            return distance / (distanceType == DistanceType.Miles ? EARTH_RADIUS_MILES : EARTH_RADIUS_KILOMETERS);
        }

        /// <summary>
        /// Converts radians to distance (mi. or km.).
        /// </summary>
        public static double ToDistance(this double distanceRadians, DistanceType distanceType)
        {
            return distanceRadians * (distanceType == DistanceType.Miles ? EARTH_RADIUS_MILES : EARTH_RADIUS_KILOMETERS);
        }

        /// <summary>
        /// Computes the bounding coordinates of all points on the surface of the earth that have a great circle distance
        /// to the point represented by the specified location that is less than or equal to the distance argument.
        /// </summary>
        public static IGeoCodeRange CalculateGeoCodeRange(
            double latitude,
            double longitude,
            double distance,
            DistanceType distanceType)
        {
            Contract.Requires<ArgumentOutOfRangeException>(latitude >= MIN_LAT_DEGREES && latitude <= MAX_LAT_DEGREES);
            Contract.Requires<ArgumentOutOfRangeException>(longitude >= MIN_LON_DEGREES && longitude <= MAX_LON_DEGREES);
            Contract.Requires<ArgumentOutOfRangeException>(distance > 0);

            double latRadians = latitude.ToRadians();
            double lonRadians = longitude.ToRadians();
            double distanceRadians = distance.ToDistanceRadians(distanceType);

            double minLatRadians = latRadians - distanceRadians;
            double maxLatRadians = latRadians + distanceRadians;

            double minLonRadians, maxLonRadians;
            if (minLatRadians > MIN_LAT_RADIANS && maxLatRadians < MAX_LAT_RADIANS)
            {
                double deltaLon = Math.Asin(Math.Sin(distanceRadians) / Math.Cos(latRadians));

                minLonRadians = lonRadians - deltaLon;
                if (minLonRadians < MIN_LON_RADIANS)
                {
                    minLonRadians += 2d * Math.PI;
                }

                maxLonRadians = lonRadians + deltaLon;
                if (maxLonRadians > MAX_LON_RADIANS)
                {
                    maxLonRadians -= 2d * Math.PI;
                }
            }
            else
            {
                // a pole is within the distance
                minLatRadians = Math.Max(minLatRadians, MIN_LAT_RADIANS);
                maxLatRadians = Math.Min(maxLatRadians, MAX_LAT_RADIANS);
                minLonRadians = MIN_LON_RADIANS;
                maxLonRadians = MAX_LON_RADIANS;
            }

            var geoCodeRange = Create.New<IGeoCodeRange>();
            geoCodeRange.MinimumLatitude = minLatRadians.ToDegrees();
            geoCodeRange.MaximumLatitude = maxLatRadians.ToDegrees();
            geoCodeRange.MinimumLongitude = minLonRadians.ToDegrees();
            geoCodeRange.MaximumLongitude = maxLonRadians.ToDegrees();

            return geoCodeRange;
        }

        /// <summary>
        /// Returns true if the <see cref="IGeoCodeRange"/> spans the 180th meridian.
        /// </summary>
        public static bool IncludesMeridian180(this IGeoCodeRange geoCodeRange)
        {
            Contract.Requires<ArgumentNullException>(geoCodeRange != null);

            return geoCodeRange.MinimumLongitude > geoCodeRange.MaximumLongitude;
        }

        /// <summary>
        /// Filters a sequence of <see cref="IGeoCode"/> items based on a specified <see cref="IGeoCodeRange"/>.
        /// Useful for database queries for better performance.
        /// </summary>
        public static IQueryable<T> WhereInGeoCodeRange<T>(
            this IQueryable<T> query,
            IGeoCodeRange geoCodeRange)
            where T : IGeoCode
        {
            Contract.Requires<ArgumentNullException>(query != null);
            Contract.Requires<ArgumentNullException>(geoCodeRange != null);

            // The logic for bounding coordinates is different depending on whether geoCodeRange includes the 180th meridian.
            Expression<Func<IGeoCode, bool>> geoCodeRangePredicate = geoCodeRange.IncludesMeridian180()
                ? (Expression<Func<IGeoCode, bool>>)(x =>
                    (x.Latitude >= geoCodeRange.MinimumLatitude && x.Latitude <= geoCodeRange.MaximumLatitude)
                    && (x.Longitude >= geoCodeRange.MinimumLongitude || x.Longitude <= geoCodeRange.MaximumLongitude)
                )
                : x =>
                    (x.Latitude >= geoCodeRange.MinimumLatitude && x.Latitude <= geoCodeRange.MaximumLatitude)
                    && (x.Longitude >= geoCodeRange.MinimumLongitude && x.Longitude <= geoCodeRange.MaximumLongitude);

            return ((IQueryable<IGeoCode>)query)
                .Where(geoCodeRangePredicate)
                .Cast<T>();
        }

        /// <summary>
        /// Calculates the distance between two locations.
        /// </summary>
        public static double CalculateDistance(
            double latitude1,
            double longitude1,
            double latitude2,
            double longitude2,
            DistanceType distanceType)
        {
            double lat1Radians = latitude1.ToRadians();
            double lon1Radians = longitude1.ToRadians();
            double lat2Radians = latitude2.ToRadians();
            double lon2Radians = longitude2.ToRadians();
            double earthRadius = distanceType == DistanceType.Miles
                ? EARTH_RADIUS_MILES
                : EARTH_RADIUS_KILOMETERS;

            return Math.Acos(
                Math.Sin(lat1Radians) * Math.Sin(lat2Radians) +
                Math.Cos(lat1Radians) * Math.Cos(lat2Radians) * Math.Cos(lon1Radians - lon2Radians)
            ) * earthRadius;
        }
    }
}
