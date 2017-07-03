using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.EntityClient;
using System.Data.Metadata.Edm;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Foundation.Common
{
    /// <summary>
    /// Utility methods for database connections.
    /// </summary>
    public static class DbExtensions
    {
        private static readonly string _dbConnectionCacheKey = "DbConnectionCache";

        /// <summary>
        /// Returns a shared connection to the data source referenced by the specified connection string.
        /// </summary>
        /// <param name="container">The container defining the scope of the requested connection.</param>
        /// <param name="connectionStringName">The name of the connection string defining the data source.</param>
        /// <returns>The shared connection to the data source.</returns>
        public static IDbConnection NewOrSharedConnection(this IContainer container, string connectionStringName)
        {
            Contract.Requires<ArgumentNullException>(container != null);
            Contract.Requires<ArgumentNullException>(connectionStringName != null);
            Contract.Requires<ArgumentException>(connectionStringName.Length > 0);

            return container
                .EnsureCache<string, ConcurrentDictionary<string, IDbConnection>>(_dbConnectionCacheKey)
                .GetOrAdd(connectionStringName, (csn) => DbExtensions.CreateConnection(csn));
        }

        /// <summary>
        /// Returns a new connection to the data source referenced by the specified connection string.
        /// </summary>
        /// <param name="connectionStringName">The name of the connection string defining the data source.</param>
        /// <returns>The connection to the data source.</returns>
        public static IDbConnection CreateConnection(string connectionStringName)
        {
            Contract.Requires<ArgumentNullException>(connectionStringName != null);
            Contract.Requires<ArgumentException>(connectionStringName.Length > 0);

            var connectionStringSettings = ConfigurationManager
                .ConnectionStrings[connectionStringName];
            if (connectionStringSettings == null)
            {
                throw new ConfigurationErrorsException(string.Format("Connection string not found: {0}", connectionStringName));
            }
            if (string.IsNullOrWhiteSpace(connectionStringSettings.ProviderName))
            {
                throw new ConfigurationErrorsException(string.Format("Missing provider name: {0}", connectionStringName));
            }
            if (string.IsNullOrWhiteSpace(connectionStringSettings.ConnectionString))
            {
                throw new ConfigurationErrorsException(string.Format("Missing connection string: {0}", connectionStringName));
            }

            return GetDbConnectionFactory(
                connectionStringSettings.ProviderName,
                connectionStringSettings.ConnectionString
            ).CreateStoreConnection();
        }

        /// <summary>
        /// Initializes a new instance of the System.Data.EntityClient.EntityConnection
        /// class with a specified System.Data.Metadata.Edm.MetadataWorkspace and connection string.
        /// </summary>
        /// <param name="metadataWorkspace">A System.Data.Metadata.Edm.MetadataWorkspace to be associated with this System.Data.EntityClient.EntityConnection.</param>
        /// <param name="connectionStringName">The name of the connection string to use when creating the underlying data source connection.</param>
        public static EntityConnection CreateEntityConnection(this MetadataWorkspace metadataWorkspace, string connectionStringName)
        {
            Contract.Requires<ArgumentNullException>(metadataWorkspace != null);
            Contract.Requires<ArgumentNullException>(connectionStringName != null);
            Contract.Requires<ArgumentException>(connectionStringName.Length > 0);


            return new EntityConnection(
                metadataWorkspace,
                (DbConnection)CreateConnection(connectionStringName)
            );
        }

        /// <summary>
        /// Unwraps an Entity Framework connection string and returns the connection string for the underlying provider.
        /// </summary>
        /// <param name="entityConnectionString">An Entity Framework connection string.</param>
        /// <returns>The inner, provider-specific connection string.</returns>
        public static string GetProviderConnectionString(string entityConnectionString)
        {
            Contract.Requires<ArgumentNullException>(entityConnectionString != null);
            Contract.Requires<ArgumentException>(entityConnectionString.Length > 0);
            Contract.Ensures(Contract.Result<string>() != null);
            Contract.Ensures(Contract.Result<string>().Length > 0);

            var entityConnectionStringBuilder = new EntityConnectionStringBuilder(entityConnectionString);
            var providerConnectionString = entityConnectionStringBuilder.ProviderConnectionString;

            if (string.IsNullOrWhiteSpace(providerConnectionString))
            {
                throw new ConfigurationErrorsException(string.Format("Missing provider connection string: {0}", entityConnectionStringBuilder.Name));
            }

            return providerConnectionString;
        }

        private static IDbConnectionFactory GetDbConnectionFactory(string providerName, string connectionString)
        {
            Contract.Requires<ArgumentNullException>(providerName != null);
            Contract.Requires<ArgumentException>(providerName.Length > 0);
            Contract.Requires<ArgumentNullException>(connectionString != null);
            Contract.Requires<ArgumentException>(connectionString.Length > 0);

            if (providerName == "System.Data.SqlClient")
            {
                return new SqlConnectionFactory(connectionString);
            }

            if (providerName == "System.Data.EntityClient")
            {
                return new EfStoreConnectionFactory(connectionString);
            }

            throw new ConfigurationErrorsException(string.Format("Provider not supported: {0}", providerName));
        }

        private interface IDbConnectionFactory
        {
            IDbConnection CreateStoreConnection();
        }

        private class SqlConnectionFactory : IDbConnectionFactory
        {
            private readonly string _connectionString;

            public SqlConnectionFactory(string connectionString)
            {
                Contract.Requires<ArgumentNullException>(connectionString != null);
                Contract.Requires<ArgumentException>(connectionString.Length > 0);

                _connectionString = connectionString;
            }

            public IDbConnection CreateStoreConnection()
            {
                return new SqlConnection(_connectionString);
            }
        }

        private class EfStoreConnectionFactory : SqlConnectionFactory
        {
            public EfStoreConnectionFactory(string connectionString)
                : base(GetProviderConnectionString(connectionString))
            {
                Contract.Requires<ArgumentNullException>(connectionString != null);
                Contract.Requires<ArgumentException>(connectionString.Length > 0);
            }
        }
    }
}
