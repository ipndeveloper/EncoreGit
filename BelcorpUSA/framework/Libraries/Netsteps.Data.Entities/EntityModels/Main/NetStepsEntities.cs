using System;
using System.Configuration;
using System.Data.Metadata.Edm;
using System.Data.Objects.DataClasses;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using NetSteps.Common.Globalization;
using NetSteps.Foundation.Common;
using System.Data.Objects;

namespace NetSteps.Data.Entities
{
    public partial class NetStepsEntities
    {
        /// <summary>
        /// The MetadataWorkspace for this context, used to initialize the context using a SQL connection string.
        /// Static-Lazy because it is costly to instantiate one of these.
        /// </summary>
        private static readonly Lazy<MetadataWorkspace> _metadataWorkspace = new Lazy<MetadataWorkspace>(() =>
            new MetadataWorkspace(
                new[]
                {
                    "res://*/EntityModels.Main.NetStepsDB.csdl",
                    "res://*/EntityModels.Main.NetStepsDB.ssdl",
                    "res://*/EntityModels.Main.NetStepsDB.msl"
                },
                new[] { Assembly.GetExecutingAssembly() }
            )
        );

        //INI - GR_Encore-07
        public ObjectSet<AccountConsistencyStatus> AccountConsistencyStatuses
        {
            get { return _accountConsistencyStatuses ?? (_accountConsistencyStatuses = CreateObjectSet<AccountConsistencyStatus>("AccountConsistencyStatus")); }
        }

        private ObjectSet<AccountConsistencyStatus> _accountConsistencyStatuses;
        //FIN - GR_Encore-07

        /// <summary>
        /// Retrieves the "Core" connection string from config.
        /// </summary>
        private static readonly Lazy<string> _coreConnectionString = new Lazy<string>(() =>
            ConfigurationManager.ConnectionStrings[ConnectionStringNames.Core].ConnectionString
        );

        /// <summary>
        /// Initializes a new <see cref="NetStepsEntities"/> object using the "Core" connection string.
        /// </summary>
        public NetStepsEntities() : this(_metadataWorkspace.Value.CreateEntityConnection(ConnectionStringNames.Core)) { }

        /// <summary>
        /// The connection string for the "Core" database.
        /// </summary>
        public static string CoreConnectionString { get { return _coreConnectionString.Value; } }

        [EdmFunction("ZriiDbModel.Store", "CalculateGeoDistance")]
        public static double CalculateGeoDistance(double latitude1, double longitude1, double latitude2, double longitude2, double radius)
        {
            throw new NotSupportedException("Direct calls are not supported.");
        }

        [EdmFunction("ZriiDbModel.Store", "ToBigInt")]
        public static long ToBigInt(string value)
        {
            throw new NotSupportedException("Direct calls are not supported.");
        }

        [EdmFunction("ZriiDbModel.Store", "nsDecrypt")]
        public static string NsDecrypt(string value)
        {
            throw new NotSupportedException("Direct calls are not supported.");
        }

        /// <summary>
        /// Wraps a sequence of <see cref="IGeoCode"/> items in an outer class that contains their distance from a specified location.
        /// </summary>
        public static IQueryable<IGeoCodeItemWithDistance<T>> SelectWithDistance<T>(
            IQueryable<T> query,
            IGeoCodeDistanceSelectorParameters distanceSelectorParameters)
            where T : IGeoCode
        {
            Contract.Requires<ArgumentNullException>(query != null);
            Contract.Requires<ArgumentNullException>(distanceSelectorParameters != null);

            return ((IQueryable<IGeoCode>)query)
                .Select(x => new GeoCodeItemWithDistance<T>
                {
                    GeoCodeItem = (T)x,
                    Distance = CalculateGeoDistance(
                        distanceSelectorParameters.Latitude,
                        distanceSelectorParameters.Longitude,
                        x.Latitude.Value,
                        x.Longitude.Value,
                        distanceSelectorParameters.Radius
                    )
                });
        }
    }
}
