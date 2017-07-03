using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestMasterHelpProvider.Logging.Sql;

namespace TestMasterHelpProvider.Logging
{
    public class SqlLogWriter : ILogWriter
    {
        #region Fields

        private static SqlQueryManager __queryManager;

        private TestRun _testRun;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the TestRun for this SqlLogWriter.
        /// </summary>
        public TestRun TestRun
        {
            get { return _testRun; }
            set { _testRun = value; }
        }

        #endregion

        #region Constructor

        public SqlLogWriter()
        {
            if (SqlLogWriter.__queryManager == null)
            {
                SqlLogWriter.__queryManager = SqlQueryManager.GetNamedInstance(SqlQueryManager.SqlLogWriterConnectionStringName);
            }
        }

        #endregion

        #region Methods

		/// <summary>
		/// Writes the log message to the database.
		/// </summary>
		/// <param name="logMessageType"></param>
		/// <param name="message"></param>
        public void WriteLogMessage(LogMessageType logMessageType, string message)
        {
			message = message.Replace("'", "''");

			Message nextMessage = new Message(_testRun, TestRun.Test, logMessageType, message);
			nextMessage.SerializeToDatabase();
        }

        #endregion
    }
}
