using System;
using System.Configuration;
using System.Data;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Cache;
using NetSteps.Foundation.Common;

namespace NetSteps.Data.Entities
{
	/// <summary>
	/// Author: John Egbert
	/// Description: Helper methods for use with the Entity Framework.
	/// Created: 02-05-2010
	/// </summary>
	public static class EntitiesHelpers
	{
		/// <summary>
		/// The get application id from connection string.
		/// </summary>
		/// <returns>
		/// The <see cref="short"/>.
		/// </returns>
		public static short GetApplicationIdFromConnectionString()
		{
			ConnectionStringSettings setting = ConfigurationManager.ConnectionStrings[ConnectionStringNames.Core];
			Contract.Assert(setting != null, String.Format("The {0} connection string is missing from the application configuration", ConnectionStringNames.Core));
			string connectionString = setting.ConnectionString;
			Application application = null;
			if (!string.IsNullOrEmpty(connectionString))
			{
				var sqlConnectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
				var applicationName = sqlConnectionStringBuilder.ApplicationName;
				if (!string.IsNullOrEmpty(applicationName))
				{
					application = ApplicationCache.GetByName(applicationName.ToCleanString());
				}
			}
			return (application != null) ? application.ApplicationID : (short)0;
		}

		/// <summary>
		/// Attempts to get you an ado friendly connection string of the object context.
		/// </summary>
		/// <typeparam name="T">
		/// Type of context 
		/// <example>
		/// NetStepsEntities
		/// </example>
		/// </typeparam>
		/// <returns>
		/// The <see cref="string"/>.
		/// </returns>
		public static string GetAdoConnectionString<T>()
		{
			if (typeof(T).BaseType == typeof(ObjectContext))
			{
				using (var context = (ObjectContext)typeof(T).New())
				{
					var entityConnection = context.Connection as EntityConnection;
					if (entityConnection != null)
					{
						IDbConnection conn = entityConnection.StoreConnection;
						return conn.ConnectionString;
					}
				}
			}

			return string.Empty;
		}
	}
}