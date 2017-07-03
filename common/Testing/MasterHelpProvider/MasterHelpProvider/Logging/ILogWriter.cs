using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestMasterHelpProvider.Logging
{
    public interface ILogWriter
    {
        /// <summary>
        /// Writes a log message with a type to the log.
        /// </summary>
        /// <param name="logMessageType"></param>
        /// <param name="message"></param>
        void WriteLogMessage(LogMessageType logMessageType, string message);
    }
}
