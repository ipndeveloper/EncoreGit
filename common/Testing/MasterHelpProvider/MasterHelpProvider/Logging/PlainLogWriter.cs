using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TestMasterHelpProvider.Logging
{
	public class PlainLogWriter : ILogWriter
	{
		#region Fields

		private static bool _truncateLogFileOnInit = false;

		private string _filePath;

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets whether or not to truncate the log file when the LogWriter is instantiated.
		/// </summary>
		public static bool TruncateLogFileOnInit
		{
			get { return _truncateLogFileOnInit; }
			set { _truncateLogFileOnInit = value; }
		}

		/// <summary>
		/// Gets the path to the log file.
		/// </summary>
		public string FilePath
		{
			get { return _filePath; }
		}

		#endregion

		#region Constructors

		public PlainLogWriter(string filePath)
		{
			_filePath = filePath;

			if (PlainLogWriter.TruncateLogFileOnInit)
			{
				FileInfo fileInfo = new FileInfo(_filePath);

				if (fileInfo.Exists)
				{
					fileInfo.Delete();
				}
			}

			if (PlainLogWriter.TruncateLogFileOnInit)
			{
				using (StreamWriter streamWriter = new StreamWriter(new FileStream(FilePath, FileMode.Create, FileAccess.Write)))
				{					
				}
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Writes a log message to the log.
		/// </summary>
		/// <param name="logMessageType"></param>
		/// <param name="message"></param>
        public void WriteLogMessage(LogMessageType logMessageType, string message)
		{
			using (StreamWriter streamWriter = new StreamWriter(new FileStream(FilePath, FileMode.Append, FileAccess.Write)))
			{
				streamWriter.WriteLine(String.Format("[{0} {1}] {2}", DateTime.Now.ToString(TestMasterHelpProviderConstants.FullTimestampToStringPattern), logMessageType, message));
			}
		}

		#endregion
	}
}
