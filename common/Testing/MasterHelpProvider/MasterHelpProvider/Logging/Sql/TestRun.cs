using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TestMasterHelpProvider.Logging.Sql
{
    public class TestRun : ISqlLoggable
    {
        #region Fields

        public const string DatabaseName = "TestAutomation";
        public const string TableName = "dbo.TestRuns";
        public const string TestRunIdColumnName = "TestRunID";
        public const string StartTimeColumnName = "StartTime";
		public const string EndTimeColumnName = "EndTime";
        public const string MachineNameColumnName = "MachineName";

        private static Test __test;

        private DateTime _startTime;
		private DateTime _endTime;
        private int _testRunId;
        private string _machineName;
		private EmailMessage _emailMessage;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the current test for this TestRun.
        /// </summary>
        public static Test Test
        {
            get { return TestRun.__test; }
            set
			{
				TestRun.__test = value;
				TestRun.__test.SerializeToDatabase();
			}
        }

        /// <summary>
        /// Gets the test run ID for this test run.
        /// </summary>
        public int TestRunId
        {
            get { return _testRunId; }
        }

		/// <summary>
		/// Gets the email message related to this TestRun.
		/// </summary>
		public EmailMessage EmailMessage
		{
			get { return _emailMessage; }
		}

        #endregion

        #region Constructor

        public TestRun()
        {
            _startTime = DateTime.Now;
            _machineName = System.Environment.MachineName;
			_emailMessage = new EmailMessage();
        }

        #endregion

		#region Methods

		/// <summary>
		/// Serializes TestRun to the database.
		/// </summary>
		public void SerializeToDatabase()
		{
			SqlQueryManager queryManager = SqlQueryManager.GetNamedInstance(SqlQueryManager.SqlLogWriterConnectionStringName);

			if (queryManager != null)
			{
				string insert = String.Format("insert into {0}.{1} ({2}, {3}, {4}) values ('{5}', '{6}', '{7}')", TestRun.DatabaseName, TestRun.TableName, TestRun.StartTimeColumnName, TestRun.EndTimeColumnName, TestRun.MachineNameColumnName, _startTime, _startTime, _machineName);

				queryManager.ExecuteNonQuery(insert);

				string select = String.Format("select {0} from {1}.{2} where {3} = '{4}' and {5} = '{6}'", TestRun.TestRunIdColumnName, TestRun.DatabaseName, TestRun.TableName, TestRun.StartTimeColumnName, _startTime, TestRun.MachineNameColumnName, _machineName);

				DataTable results = queryManager.ExecuteQuery(select);

				if (results != null && results.Rows.Count > 0)
				{
					_testRunId = (int)results.Rows[0][TestRun.TestRunIdColumnName];
				}
				else
				{
					throw new Exception("Could not retrieve ID for newly entered test run.");
				}
			}
			else
			{
				throw new NullReferenceException(String.Format("SqlQueryManager was null. Please check the configuration file for the connection string named {0}.", SqlQueryManager.SqlLogWriterConnectionStringName));
			}
		}

		/// <summary>
		/// Marks the end time for a test run.
		/// </summary>
		public void MarkEndTime()
		{
			_endTime = DateTime.Now;

			SqlQueryManager queryManager = SqlQueryManager.GetNamedInstance(SqlQueryManager.SqlLogWriterConnectionStringName);

			if (queryManager != null)
			{
				string update = String.Format("update {0}.{1} set {2} = '{3}' where {4} = {5}", TestRun.DatabaseName, TestRun.TableName, TestRun.EndTimeColumnName, _endTime, TestRun.TestRunIdColumnName, _testRunId);

				queryManager.ExecuteNonQuery(update);
			}
			else
			{
				throw new NullReferenceException(String.Format("SqlQueryManager was null. Please check the configuration file for the connection string named {0}.", SqlQueryManager.SqlLogWriterConnectionStringName));
			}
		}

		#endregion
	}
}
