using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Data;

namespace TestMasterHelpProvider.Logging.Sql
{
	public class Test : ISqlLoggable
    {
        #region Fields

        public const string DatabaseName = "TestAutomation";
        public const string TableName = "dbo.Tests";
        public const string TestIdColumnName = "TestID";
        public const string TestGuidColumnName = "TestGuid";
        public const string NameColumnName = "Name";
        public const string ClientIdColumnName = "ClientID";

        private Client _client;
        private string _className;
        private string _testMethodName;
        private int _testId;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the GUID for this test.
        /// </summary>
        public string Guid
        {
            get { return GenerateGuid(); }
        }

        /// <summary>
        /// Gets the Client for this test.
        /// </summary>
        public Client Client
        {
            get { return _client; }
        }

        /// <summary>
        /// Gets the Client ID for this test's client.
        /// </summary>
        public int ClientId
        {
            get { return (int)_client; }
        }

        /// <summary>
        /// Gets this test's name.
        /// </summary>
        public string TestMethodName
        {
            get { return _testMethodName; }
        }

        /// <summary>
        /// Gets this test's test ID.
        /// </summary>
        public int TestId
        {
            get { return _testId; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an instance of Test using the specified client, class name, and test method name. Also this checks 
        /// whether or not this test is already listed in the database, and if it is gets the ID for this test, and if 
        /// it isn't create a new record for this test.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="className"></param>
        /// <param name="testMethodName"></param>
        public Test(Client client, string className, string testMethodName)
        {
            _client = client;
            _className = className;
            _testMethodName = testMethodName;
        }

        #endregion

        #region Methods

		/// <summary>
		/// Serializes the Test to the database.
		/// </summary>
		public void SerializeToDatabase()
		{
			SqlQueryManager queryManager = SqlQueryManager.GetNamedInstance(SqlQueryManager.SqlLogWriterConnectionStringName);

			if (queryManager != null)
			{
				string query = String.Format("select * from {0}.{1} where {2} = '{3}'", Test.DatabaseName, Test.TableName, Test.TestGuidColumnName, Guid);
				DataTable queryResults = queryManager.ExecuteQuery(query);

				if (queryResults != null && queryResults.Rows.Count == 0)
				{
					string insert = String.Format("insert into {0}.{1} ({2}, {3}, {4}) values ('{5}', '{6}', {7})", Test.DatabaseName, Test.TableName, Test.TestGuidColumnName, Test.NameColumnName, Test.ClientIdColumnName, Guid, TestMethodName, ClientId);

					queryManager.ExecuteNonQuery(insert);

					queryResults = queryManager.ExecuteQuery(query);
				}

				_testId = (int)(queryResults.Rows[0][Test.TestIdColumnName]);
			}
			else
			{
				throw new NullReferenceException(String.Format("SqlQueryManager was null. Please check the configuration file for the connection string named {0}.", SqlQueryManager.SqlLogWriterConnectionStringName));
			}
		}

        /// <summary>
        /// Generates the test's GUID (as seen in the test lists).
        /// </summary>
        /// <returns></returns>
        private string GenerateGuid()
        {
            SHA1Managed crypto = new SHA1Managed();
            byte[] bytes = new byte[16];
            Array.Copy(crypto.ComputeHash(Encoding.Unicode.GetBytes(String.Format("{0}.{1}", _className, _testMethodName))), bytes, 16);
            Guid guid = new Guid(bytes);

            return guid.ToString();
        }

        #endregion
    }
}
