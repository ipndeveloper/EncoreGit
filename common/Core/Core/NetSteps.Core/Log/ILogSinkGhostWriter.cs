using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Encore.Core.Log
{
    /// <summary>
    /// Interface for ghost writers. Writing log events are delegated to ghost writers.
    /// </summary>
    public interface ILogSinkGhostWriter
    {
        /// <summary>
        /// Delegates writing an event.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="evt"></param>
        void GhostWrite(LogEventWriter writer, LogEvent evt);
    }
}
