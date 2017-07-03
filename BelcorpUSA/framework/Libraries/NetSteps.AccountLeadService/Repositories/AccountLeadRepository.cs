using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;

using NetSteps.AccountLeadService.Common.Repositories;
using NetSteps.Encore.Core.IoC;
using NetSteps.Foundation.Common;

namespace NetSteps.AccountLeadService.Repositories
{
	[ContainerRegister(typeof(IAccountLeadRepository), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
	public class AccountLeadRepository : IAccountLeadRepository
	{
		#region Constants

		protected const string TableName = "dbo.AccountLeads";
		protected const string LeadColumnName = "Leads";
		protected const string AccountIdColumnName = "AccountID";

		protected static readonly string CoreConnectionStringName = ConnectionStringNames.Core;
		protected static string DbConnectionString;

		#endregion

		public AccountLeadRepository()
		{
			var dbConnectionSettings = ConfigurationManager.ConnectionStrings[CoreConnectionStringName];

			if(dbConnectionSettings == null)
			{
				throw new ConfigurationErrorsException(string.Format("Connection string not found: {0}", CoreConnectionStringName));
			}
			if (string.IsNullOrWhiteSpace(dbConnectionSettings.ConnectionString))
			{
				throw new ConfigurationErrorsException(string.Format("Missing string not found: {0}", CoreConnectionStringName));
			}

			DbConnectionString = dbConnectionSettings.ConnectionString;
		}

		#region Implementation of IAccountLeadRepository

		public virtual int? GetLeadCount(int accountId)
		{
			int? leadCount;
			string query = string.Format("SELECT {0} FROM {1} WHERE {2} = {3}", LeadColumnName, TableName, AccountIdColumnName, accountId);

			using (var connection = new SqlConnection(DbConnectionString))
			{
				connection.Open();
				using (var command = new SqlCommand(query, connection))
				{
					leadCount = this.GetLeadCountFromCommand(command);
				}
			}

			return leadCount;
		}

		public virtual void SetLeadCount(int accountId, int amount)
		{
			string ifRowCountQuery = "IF @@ROWCOUNT = 0";
			string insertQuery = string.Format("INSERT INTO {0} VALUES({1}, {2})", TableName, accountId, amount);
			string updateQuery = string.Format("UPDATE {0} SET {1} = {2} WHERE {3} = {4}\n{5}\n{6}", 
				TableName, LeadColumnName, amount, AccountIdColumnName, accountId, ifRowCountQuery, insertQuery);

			using (var connection = new SqlConnection(DbConnectionString))
			{
				connection.Open();
				using (var command = new SqlCommand(updateQuery, connection))
				{
					command.ExecuteNonQuery();
				}
			}
		}

		#endregion

		private int? GetLeadCountFromCommand(SqlCommand command)
		{
			int? leadCount;

			using (var reader = command.ExecuteReader())
			{
				leadCount = GetLeadCountFromReader(reader);
			}

			return leadCount;
		}

		private static int? GetLeadCountFromReader(DbDataReader reader)
		{
			var leadCount = new int?();

			if (reader.HasRows)
			{
				leadCount = reader.GetInt32(0);
			}

			return leadCount;
		}
	}
}
