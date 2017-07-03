using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WatiN.Core.Logging;

namespace TestMasterHelpProvider.Logging.Sql
{
	public class Message : ISqlLoggable
	{
		#region Fields

		public const string DatabaseName = "TestAutomation";
		public const string TableName = "dbo.Messages";
		public const string TestRunIDColumnName = "TestRunID";
		public const string TestIDColumnName = "TestID";
		public const string TimestampColumnName = "Timestamp";
        public const string MessageColumnName = "Message";
        public const string MessageTypeIDColumnName = "MessageTypeID";

        private int _testRunId;
        private int _testId;
        private string _timestamp;
        private string _messageText;
        private LogMessageType _messageType;

		#endregion

        #region Constructor

        /// <summary>
        /// Creates an instance of Message requiring the test run, test, message type, and message text.
        /// </summary>
        /// <param name="testRun"></param>
        /// <param name="test"></param>
        /// <param name="messageType"></param>
        /// <param name="messageText"></param>
        public Message(TestRun testRun, Test test, LogMessageType messageType, string messageText)
        {
            _testRunId = testRun.TestRunId;
            _testId = test.TestId;
            _messageText = messageText;
            _messageType = messageType;
            _timestamp = DateTime.Now.ToString();
        }

        #endregion

		#region Methods

		/// <summary>
		/// Serializes this message to the database.
		/// </summary>
		public void SerializeToDatabase()
		{
			SqlQueryManager queryManager = SqlQueryManager.GetNamedInstance(SqlQueryManager.SqlLogWriterConnectionStringName);

			if (queryManager != null)
			{
				string insert = String.Format("insert into {0}.{1} ({2}, {3}, {4}, {5}, {6}) values ({7}, {8}, '{9}', '{10}', {11})", Message.DatabaseName, Message.TableName, Message.TestRunIDColumnName, Message.TestIDColumnName, Message.TimestampColumnName, Message.MessageColumnName, Message.MessageTypeIDColumnName, _testRunId, _testId, _timestamp, _messageText, (int)_messageType);

				queryManager.ExecuteNonQuery(insert);
			}
			else
			{
				throw new NullReferenceException(String.Format("SqlQueryManager was null. Please check the configuration file for the connection string named {0}.", SqlQueryManager.SqlLogWriterConnectionStringName));
			}
		}

		#endregion
	}
}
