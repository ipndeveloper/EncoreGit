using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestMasterHelpProvider.Logging
{
    public class NullLogWriter : ILogWriter
    {
        #region Methods

        /// <summary>
        /// Writes a log message to stdout.
        /// </summary>
        /// <param name="logMessageType"></param>
        /// <param name="message"></param>
        public void WriteLogMessage(LogMessageType logMessageType, string message)
        {
            Console.WriteLine(String.Format("[{0} {1}] {2}", DateTime.Now.ToString(TestMasterHelpProviderConstants.FullTimestampToStringPattern), logMessageType, message));
        }

        #endregion
    }
}
