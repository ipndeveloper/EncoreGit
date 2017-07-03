using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TestMasterHelpProvider.Logging
{
	public sealed class Logger
	{
		#region Fields

        private static Logger __instance;

		private ILogWriter _logWriter;

		#endregion

        #region Properties

        /// <summary>
        /// Gets or sets the LogWriter for the Logger.
        /// </summary>
        public ILogWriter LogWriter
        {
            get { return _logWriter; }
            set { _logWriter = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
		/// Creates an instance of AbstractFileLogger.
		/// </summary>
        /// <param name="logWriter"></param>
		private Logger(ILogWriter logWriter)
		{
			_logWriter = logWriter;
		}

		#endregion

		#region Methods

        /// <summary>
        /// Gets the singleton instance of the Logger.
        /// </summary>
        /// <param name="logWriter"></param>
        /// <returns></returns>
        public static Logger GetInstance(ILogWriter logWriter)
        {
            if (Logger.__instance == null)
            {
                Logger.__instance = new Logger(logWriter);
            }

            return Logger.__instance;
        }

        /// <summary>
        /// Gets the singleton instance of the Logger with a NullLogWriter.
        /// </summary>
        /// <returns></returns>
        public static Logger GetInstance()
        {
            if (Logger.__instance == null)
            {
                Logger.__instance = new Logger(new NullLogWriter());
            }

            return Logger.__instance;
        }

		/// <summary>
		/// Writes a debug message to the log.
		/// </summary>
		/// <param name="message"></param>
		public void WriteDebugMessage(string message)
		{
			_logWriter.WriteLogMessage(LogMessageType.Debug, message);
		}

		/// <summary>
		/// Writes a pass message to the log.
		/// </summary>
		/// <param name="message"></param>
		public void WritePassMessage(string message)
		{
			_logWriter.WriteLogMessage(LogMessageType.Pass, message);
		}

		/// <summary>
		/// Writes a fail message to the log.
		/// </summary>
		/// <param name="message"></param>
		public void WriteFailMessage(string message)
		{
			_logWriter.WriteLogMessage(LogMessageType.Fail, message);
		}

		/// <summary>
		/// Writes an exception message to the log.
		/// </summary>
		/// <param name="message"></param>
		public void WriteExceptionMessage(string message)
		{
			_logWriter.WriteLogMessage(LogMessageType.Exception, message);
		}

        /// <summary>
        /// Writes an informational message to the log.
        /// </summary>
        /// <param name="message"></param>
        public void WriteInfoMessage(string message)
        {
            _logWriter.WriteLogMessage(LogMessageType.Info, message);
        }

		/// <summary>
		/// Writes a test start message to the log, marking the start time of the test.
		/// </summary>
		/// <param name="message"></param>
		public void WriteTestStartMessage(string message = null)
		{
			_logWriter.WriteLogMessage(LogMessageType.TestStart, message);
		}

		/// <summary>
		/// Writes a test end message to the log, marking the end time of the test.
		/// </summary>
		/// <param name="message"></param>
		public void WriteTestEndMessage(string message = null)
		{
			_logWriter.WriteLogMessage(LogMessageType.TestEnd, message);
		}

		#endregion
	}
}
