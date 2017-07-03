using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TestMasterHelpProvider.Logging
{
	public class CsvLogWriter : ILogWriter
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

		public CsvLogWriter(string filePath)
		{
			_filePath = filePath;

			if (CsvLogWriter.TruncateLogFileOnInit)
			{
				FileInfo fileInfo = new FileInfo(_filePath);

				if (fileInfo.Exists)
				{
					fileInfo.Delete();
				}
			}

			if (CsvLogWriter.TruncateLogFileOnInit)
			{
				using (StreamWriter streamWriter = new StreamWriter(new FileStream(FilePath, FileMode.Create, FileAccess.Write)))
				{
					streamWriter.WriteLine("\"Timestamp\", \"Type\", \"Message\"");
				}
			}
		}

		#endregion

		#region Methods

        public void WriteLogMessage(LogMessageType logMessageType, string message)
		{
			using (StreamWriter streamWriter = new StreamWriter(new FileStream(FilePath, FileMode.Append, FileAccess.Write)))
			{
				streamWriter.WriteLine(String.Format("\"{0}\", \"{1}\", \"{2}\"", DateTime.Now.ToString(TestMasterHelpProviderConstants.FullTimestampToStringPattern), logMessageType, message));
			}
		}

		#endregion
	}
}
