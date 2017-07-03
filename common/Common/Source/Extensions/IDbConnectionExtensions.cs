using System.Collections.Generic;
using System.Data;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Common.Data;

namespace NetSteps.Common.Extensions
{
    /// <summary>
    /// Author: John Egbert
    /// Description: IDbConnection Extensions to extend the functionality of LINQ to Entities
    /// Created: 06-09-2010
    /// </summary>
    public static class IDbConnectionExtensions
    {
        public static ConnectionStringInfo GetConnectionStringInfo(this IDbConnection conn)
        {
            return GetConnectionStringInfo(conn.ConnectionString);
        }

        public static ConnectionStringInfo GetConnectionStringInfo(this string entityFrameworkConnectionString)
        {
            var nameValues = entityFrameworkConnectionString.GetConnectionStringNameValues();

            ConnectionStringInfo connectionStringInfo = new ConnectionStringInfo();
            foreach (var item in nameValues)
            {
                if (item.Name == "Data Source")
                    connectionStringInfo.DataSource = item.Value;
                else if (item.Name == "Initial Catalog")
                    connectionStringInfo.InitialCatalog = item.Value;
                else if (item.Name == "User ID")
                    connectionStringInfo.UserID = item.Value;
                else if (item.Name == "Password")
                    connectionStringInfo.Password = item.Value;
                else if (item.Name == "Application Name")
                    connectionStringInfo.ApplicationName = item.Value;
                else if (item.Name == "MultipleActiveResultSets")
                    connectionStringInfo.MultipleActiveResultSets = item.Value;
                else if (item.Name == "Max Pool Size")
                    connectionStringInfo.MaxPoolSize = item.Value;
            }

            return connectionStringInfo;
        }

        private static List<NameValue<string, string>> GetConnectionStringNameValues(this IDbConnection conn)
        {
            List<NameValue<string, string>> nameValues = new List<NameValue<string, string>>();
            string entityFrameworkConnectionString = conn.ConnectionString.Replace("\"", "&quot;");
            foreach (var item in entityFrameworkConnectionString.Split(new char[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries))
            {
                nameValues.Add(new NameValue<string, string>()
                {
                    Name = item.Split('=').ToList()[0].Trim(),
                    Value = item.Split('=').ToList()[1].Trim()
                });
            }
            return nameValues;
        }

        public static EntityConnection GetEntityConnection(this string entityFrameworkConnectionString)
        {
            var connectionStringInfo = entityFrameworkConnectionString.GetEntityConnectionStringInfo();

            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();
            sqlBuilder.DataSource = connectionStringInfo.ProviderConnectionString.DataSource;
            sqlBuilder.InitialCatalog = connectionStringInfo.ProviderConnectionString.InitialCatalog;
            sqlBuilder.MultipleActiveResultSets = connectionStringInfo.ProviderConnectionString.MultipleActiveResultSets.ToBool();
            sqlBuilder.UserID = connectionStringInfo.ProviderConnectionString.UserID;
            sqlBuilder.Password = connectionStringInfo.ProviderConnectionString.Password;
            sqlBuilder.MaxPoolSize = connectionStringInfo.ProviderConnectionString.MaxPoolSize.ToInt();

            EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder();
            entityBuilder.Provider = connectionStringInfo.Provider;
            entityBuilder.ProviderConnectionString = sqlBuilder.ToString();
            entityBuilder.Metadata = connectionStringInfo.Metadata;

            EntityConnection entityConnection = new EntityConnection(entityBuilder.ToString());

            return entityConnection;
        }

        public static EntityConnectionStringInfo GetEntityConnectionStringInfo(this string entityFrameworkConnectionString)
        {
            var nameValues = entityFrameworkConnectionString.GetConnectionStringNameValues();

            EntityConnectionStringInfo connectionStringInfo = new EntityConnectionStringInfo();
            foreach (var item in nameValues)
            {
                if (item.Name.ToLower() == "Metadata".ToLower())
                    connectionStringInfo.Metadata = item.Value;
                else if (item.Name.ToLower() == "Provider".ToLower())
                    connectionStringInfo.Provider = item.Value;
            }

            string searchTerm = "provider connection string=";
            var connectionString = entityFrameworkConnectionString
                .Substring(entityFrameworkConnectionString.IndexOf(searchTerm) + searchTerm.Length)
                .Replace("\"", "");
            connectionStringInfo.ProviderConnectionString = GetConnectionStringInfo(connectionString);

            return connectionStringInfo;
        }
        private static List<NameValue<string, string>> GetConnectionStringNameValues(this string entityFrameworkConnectionString)
        {
            var entityFrameworkConnectionString2 = entityFrameworkConnectionString.Replace("&quot;", "\"");
            List<NameValue<string, string>> nameValues = new List<NameValue<string, string>>();
            foreach (var item in entityFrameworkConnectionString2.Split(';'))
            {
                nameValues.Add(new NameValue<string, string>()
                {
                    Name = item.Split('=').ToList()[0].Trim(),
                    Value = item.Split('=').ToList()[1].Trim()
                });
            }
            return nameValues;
        }
    }
}
