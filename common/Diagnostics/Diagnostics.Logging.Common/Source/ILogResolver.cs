using System;
using NetSteps.Diagnostics.Logging.Common.Models;

namespace NetSteps.Diagnostics.Logging.Common
{
    /// <summary>
    /// Resolves logs.
    /// </summary>
    public interface ILogResolver
    {
        /// <summary>
        /// Logs and exception.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="isUnhandledException"></param>        
        /// <returns></returns>
        void LogException(Exception ex, bool isUnhandledException);

        /// <summary>
        /// Logs and exception.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="message"></param>
        /// <param name="isUnhandledException"></param>        
        /// <returns></returns>
        void LogException(Exception ex, string message, bool isUnhandledException);

    }
}
