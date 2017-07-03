using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TestMasterHelpProvider.Logging.Sql
{
	public class TestRunResults
	{
		#region Fields

		public const string ClientNameColumnName = "Client Name";
		public const string TestRunColumnName = "Test Run";
		public const string MachineNameColumnName = "Machine Name";
		public const string TestStartTimeColumnName = "Test Start Time";
		public const string TestEndTimeColumnName = "Test End Time";
		public const string TimestampColumnName = "Timestamp";
		public const string TestNameColumnName = "Test Name";
		public const string MesssageColumnName = "Message";
		public const string MessageTypeColumnName = "Message Type";

		private const string GetTestRunStoredProcedureName = "TestAutomation.dbo.sp_get_testrun";

		private string _clientName;
		private int _testRunId;
		private string _machineName;
		private DateTime _testStartTime;
		private DateTime _testEndTime;
		private IList<TestMessage> _messages;

		#endregion

		#region Properties

		/// <summary>
		/// Gets the client name for this test run results object.
		/// </summary>
		public string ClientName
		{
			get { return _clientName; }
		}

		/// <summary>
		/// Gets the test run ID for this test run results object.
		/// </summary>
		public int TestRunId
		{
			get { return _testRunId; }
		}

		/// <summary>
		/// Gets the start time for this test run results object.
		/// </summary>
		public DateTime StartTime
		{
			get { return _testStartTime; }
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Creates an instance of TestRunResults using the details of a TestRun object.
		/// </summary>
		/// <param name="testRun"></param>
		public TestRunResults(TestRun testRun)
		{
			_messages = new List<TestMessage>();

			SqlQueryManager queryManager = SqlQueryManager.GetNamedInstance(SqlQueryManager.SqlLogWriterConnectionStringName);

			if (queryManager != null)
			{
				IDictionary<string, string> sprocParameters = new Dictionary<string, string>()
				{
					{ "@TestRunID", testRun.TestRunId.ToString() }
				};

				DataTable results = queryManager.ExecuteStoredProcedure(TestRunResults.GetTestRunStoredProcedureName, sprocParameters);

				if (results.Rows.Count > 0)
				{
					_clientName = (string)results.Rows[0][TestRunResults.ClientNameColumnName];
					_testRunId = (int)results.Rows[0][TestRunResults.TestRunColumnName];
					_machineName = (string)results.Rows[0][TestRunResults.MachineNameColumnName];
					_testStartTime = (DateTime)results.Rows[0][TestRunResults.TestStartTimeColumnName];
					_testEndTime = (DateTime)results.Rows[0][TestRunResults.TestEndTimeColumnName];

					foreach (DataRow nextRow in results.Rows)
					{
						TestMessage nextTestMessage = new TestMessage(nextRow);

						if (!nextTestMessage.IsNull)
						{
							_messages.Add(nextTestMessage);
						}
					}
				}
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Creates a string representation of the test run results for the email message body.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			sb.Append(String.Format("<html><body><div>Client Name: {0}</div>", _clientName));
			sb.Append(String.Format("<div>Test Run ID: {0}</div>", _testRunId));
			sb.Append(String.Format("<div>Machine Test Was Run On: {0}</div>", _machineName));
			sb.Append(String.Format("<div>Test Run Start Time: {0}</div>", _testStartTime));
			sb.Append(String.Format("<div>Test Run End Time: {0}</div>", _testEndTime));
			sb.Append(String.Format("<div>Total test run duration: {0}</div><div><ol>", (_testEndTime - _testStartTime)));

			string testName = String.Empty;
			DateTime startTime = default(DateTime);

			foreach (TestMessage nextMessage in _messages)
			{
				if (String.IsNullOrEmpty(testName) || !testName.Equals(nextMessage.TestName, StringComparison.InvariantCultureIgnoreCase))
				{
					testName = nextMessage.TestName;

					sb.Append(String.Format("<li>Test '{0}':<ul>", testName));
				}

				if (nextMessage.MessageType == LogMessageType.TestStart)
				{
					startTime = nextMessage.Timestamp;
				}
				else if (nextMessage.MessageType == LogMessageType.TestEnd)
				{
					if (startTime != default(DateTime))
					{
						sb.Append(String.Format("<li style=\"text-decoration: underline;\">Total Run Time: {0}</li></ul></li>", (nextMessage.Timestamp - startTime)));
					}
				}
				else
				{
					sb.AppendLine(String.Format("<li>{0}</li>", nextMessage));
				}
			}

			sb.Append("</ol></div></body></html>");

			return sb.ToString();
		}

		#endregion

		#region TestMessage

		public class TestMessage
		{
			#region Fields

			private string _testName;
			private DateTime _timestamp;
			private string _message;
			private LogMessageType _messageType;

			#endregion

			#region Properties

			/// <summary>
			/// Gets the test name related to this test message.
			/// </summary>
			public string TestName
			{
				get { return _testName; }
			}

			/// <summary>
			/// Gets the timestamp for this test message.
			/// </summary>
			public DateTime Timestamp
			{
				get { return _timestamp; }
			}

			/// <summary>
			/// Gets the message for this test message.
			/// </summary>
			public string Message
			{
				get { return _message; }
			}

			/// <summary>
			/// Gets the message type for this test message.
			/// </summary>
			public LogMessageType MessageType
			{
				get { return _messageType; }
			}

			/// <summary>
			/// Gets whether this object is null.
			/// </summary>
			public bool IsNull
			{
				get { return (String.IsNullOrEmpty(_testName) &&  String.IsNullOrEmpty(_message) && _timestamp == default(DateTime) && _messageType == default(LogMessageType)); }
			}

			#endregion

			#region Constructors

			/// <summary>
			/// Creates an instance of TestMessage.
			/// </summary>
			/// <param name="row"></param>
			public TestMessage(DataRow row)
			{
				if (row[TestRunResults.TestNameColumnName] != null && row[TestRunResults.TimestampColumnName] != null && row[TestRunResults.MesssageColumnName] != null && row[TestRunResults.MessageTypeColumnName] != null)
				{
					_testName = (string)row[TestRunResults.TestNameColumnName];
					_timestamp = (DateTime)row[TestRunResults.TimestampColumnName];
					_message = (string)row[TestRunResults.MesssageColumnName];
					_messageType = (LogMessageType)Enum.Parse(typeof(LogMessageType), (string)row[TestRunResults.MessageTypeColumnName]);
				}
			}

			#endregion

			#region Methods

			/// <summary>
			/// Returns a string representation of TestMessage.
			/// </summary>
			/// <returns></returns>
			public override string ToString()
			{
				return String.Format("[{0} -- {1}] {2}", Timestamp.ToString("yyyy-MM-dd HH:mm:ss"), _messageType, _message);
			}

			#endregion
		}

		#endregion
	}
}
