using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using TestMasterHelpProvider.Logging;

namespace TestMasterHelpProvider
{
	public class SqlQueryManager
	{
		#region Fields

        public const string SqlLogWriterConnectionStringName = "SqlLogConnection";

		private static IDictionary<string, SqlQueryManager> __instances;

		private string _connectionString;
		private PlainLogWriter _logWriter;

		#endregion

		#region Properties

		/// <summary>
		/// Gets the current instance's connection string.
		/// </summary>
		public string ConnectionString
		{
			get { return _connectionString; }
		}

		#endregion

		#region Constructor

		/// <summary>
		/// Creates an instance of SQLQueryManager.
		/// </summary>
		/// <param name="connectionString"></param>
		private SqlQueryManager(string connectionString)
		{
			_connectionString = connectionString;
			_logWriter = new PlainLogWriter(Utils.DefaultLogFileLocation);
		}

		#endregion

		#region Methdods

		/// <summary>
		/// Executes an SQL query and returns the results.
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		public DataTable ExecuteQuery(string query)
		{
			DataTable results = null;

			using (SqlConnection connection = GetConnection())
			{
				SqlCommand command = new SqlCommand(query, connection);
				command.Connection.Open();

				if (connection.State == ConnectionState.Open)
				{
					results = new DataTable();
					results.Load(command.ExecuteReader(), LoadOption.OverwriteChanges, FillErrorHandler);

					command.Connection.Close();
				}
				else
				{
					_logWriter.WriteLogMessage(LogMessageType.Exception, String.Format("Could not connect to database using {0}.", connection.ConnectionString));

					throw new Exception(String.Format("Could not connect to database using {0}.", connection.ConnectionString));
				}
			}

			return results;
		}

		/// <summary>
		/// Executes an SQL non-query and returns the results.
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		public DataTable ExecuteNonQuery(string query)
		{
			DataTable results = null;

			using (SqlConnection connection = GetConnection())
			{
				SqlCommand command = new SqlCommand(query, connection);
				command.Connection.Open();

				if (connection.State == ConnectionState.Open)
				{
					results = new DataTable();
					results.Load(command.ExecuteReader(), LoadOption.OverwriteChanges, FillErrorHandler);

					command.Connection.Close();
				}
				else
				{
					_logWriter.WriteLogMessage(LogMessageType.Exception, String.Format("Could not connect to database using {0}.", connection.ConnectionString));

					throw new Exception(String.Format("Could not connect to database using {0}.", connection.ConnectionString));
				}
			}

			return results;
		}

		/// <summary>
		/// Executes a stored procedure.
		/// </summary>
		/// <param name="storedProcedureName"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public DataTable ExecuteStoredProcedure(string storedProcedureName, IDictionary<string, string> parameters)
		{
			DataTable results = null;

			using (SqlConnection connection = GetConnection())
			{
				SqlCommand command = new SqlCommand(storedProcedureName, connection);
				command.CommandType = CommandType.StoredProcedure;

				foreach (string parameterName in parameters.Keys)
				{
					SqlParameter nextParameter = new SqlParameter(parameterName, parameters[parameterName]);

					command.Parameters.Add(nextParameter);
				}

				command.Connection.Open();

				if (connection.State == ConnectionState.Open)
				{
					results = new DataTable();
					results.Load(command.ExecuteReader(), LoadOption.OverwriteChanges, FillErrorHandler);

					command.Connection.Close();
				}
				else
				{
					_logWriter.WriteLogMessage(LogMessageType.Exception, String.Format("Could not connect to database using {0}.", connection.ConnectionString));

					throw new Exception(String.Format("Could not connect to database using {0}.", connection.ConnectionString));
				}
			}

			return results;
		}

		/// <summary>
		/// Determines whether another object is equal to the current instance.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			bool results = false;

			if (obj.GetType() == this.GetType())
			{
				if (((SqlQueryManager)obj).ConnectionString.Equals(this.ConnectionString))
				{
					results = true;
				}
			}

			return results;
		}

		/// <summary>
		/// Gets this object's hash code.
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		/// <summary>
		/// Returns a string representation of this instance.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return ConnectionString;
		}

		/// <summary>
		/// Gets a reference to an instance of SqlQueryManager for the Core database.
		/// </summary>
		/// <returns></returns>
		public static SqlQueryManager GetCoreInstance()
		{
			EnsureInstances();

			if (!SqlQueryManager.__instances.ContainsKey(TestMasterHelpProviderConstants.CoreConnectionStringSettingName))
			{
				string connectionStringName = ConfigurationManager.AppSettings[TestMasterHelpProviderConstants.CoreConnectionStringSettingName];
				ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings[connectionStringName];

				SqlQueryManager.__instances.Add(TestMasterHelpProviderConstants.CoreConnectionStringSettingName, new SqlQueryManager(connectionStringSettings.ConnectionString));
			}

			return SqlQueryManager.__instances[TestMasterHelpProviderConstants.CoreConnectionStringSettingName];
		}

		/// <summary>
		/// Gets a reference to an instance of SqlQueryManager for the Commissions database.
		/// </summary>
		/// <returns></returns>
		public static SqlQueryManager GetCommissionsInstance()
		{
			EnsureInstances();

			if (!SqlQueryManager.__instances.ContainsKey(TestMasterHelpProviderConstants.CommissionsConnectionStringSettingName))
			{
				string connectionStringName = ConfigurationManager.AppSettings[TestMasterHelpProviderConstants.CommissionsConnectionStringSettingName];
				ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings[connectionStringName];

				SqlQueryManager.__instances.Add(TestMasterHelpProviderConstants.CommissionsConnectionStringSettingName, new SqlQueryManager(connectionStringSettings.ConnectionString));
			}

			return SqlQueryManager.__instances[TestMasterHelpProviderConstants.CommissionsConnectionStringSettingName];
		}

		/// <summary>
		/// Gets a named instance of the SqlQueryManager.
		/// </summary>
		/// <param name="instanceName">Connection string name in the app configuration file.</param>
		/// <returns></returns>
		public static SqlQueryManager GetNamedInstance(string instanceName)
		{
			SqlQueryManager returnInstance = null;

			EnsureInstances();

			if (!SqlQueryManager.__instances.ContainsKey(instanceName))
			{
				ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings[instanceName];

				if (connectionStringSettings != null && !String.IsNullOrEmpty(connectionStringSettings.ConnectionString))
				{
					SqlQueryManager.__instances.Add(instanceName, new SqlQueryManager(connectionStringSettings.ConnectionString));

					returnInstance = SqlQueryManager.__instances[instanceName];
				}
			}
			else
			{
				returnInstance = SqlQueryManager.__instances[instanceName];
			}

			return returnInstance;
		}

		/// <summary>
		/// Gets a named instance for a query string not in the configuration file.
		/// </summary>
		/// <param name="instanceName"></param>
		/// <param name="connectionStringSettings"></param>
		/// <returns></returns>
		public static SqlQueryManager GetInstance(string instanceName, ConnectionStringSettings connectionStringSettings = null)
		{
			SqlQueryManager returnInstance = null;

			EnsureInstances();

			if (!SqlQueryManager.__instances.ContainsKey(instanceName))
			{
				if (connectionStringSettings != null && !String.IsNullOrEmpty(connectionStringSettings.ConnectionString))
				{
					SqlQueryManager.__instances.Add(instanceName, new SqlQueryManager(connectionStringSettings.ConnectionString));

					returnInstance = SqlQueryManager.__instances[instanceName];
				}
			}
			else
			{
				returnInstance = SqlQueryManager.__instances[instanceName];
			}

			return returnInstance;
		}

		/// <summary>
		/// Exception handler for load method.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public static void FillErrorHandler(object sender, FillErrorEventArgs e)
		{
			if (e.Errors.GetType() == typeof(Exception))
			{
				string value = e.Values[0].ToString();

				e.Continue = false;
			}

			e.Continue = true;
		}

		/// <summary>
		/// Ensures that a collection is available for query manager instances.
		/// </summary>
		private static void EnsureInstances()
		{
			if (SqlQueryManager.__instances == null)
			{
				SqlQueryManager.__instances = new Dictionary<string, SqlQueryManager>();
			}
		}

		/// <summary>
		/// Gets the SqlConnection for the connection string.
		/// </summary>
		/// <returns></returns>
		private SqlConnection GetConnection()
		{
			return new SqlConnection(ConnectionString);
		}

		#endregion
	}
}
