using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using NetstepsDataAccess.DataEntities;

namespace NetSteps.Integrations.Service.Business
{
    internal class LoggingTimer : IDisposable
    {
        ILog Logger { get; set; }

        string Description { get; set; }

        DateTime StartTime { get; set; }

        int? TimeOut { get; set; }

        NetStepsEntities Entities { get; set; }

        IList<string> ExitMessages { get; set; }

        /// <summary>
        /// Creates an object for timing the duration of an operation
        /// </summary>
        /// <param name="description">description of what is being timed</param>
        /// <param name="timeout">optional field to issue a warning if the specified amount of time has been exceeded</param>
        /// <param name="logger">optional, the logger to use</param>
        /// <param name="db">optional, entities collection to write timeout warning</param>
        public LoggingTimer(string description, ILog logger, NetStepsEntities entities = null, int? timeout = null)
        {
            if (logger == null)
            {
                logger = LogManager.GetLogger(typeof(IntegrationsService));
                log4net.Config.XmlConfigurator.Configure();
            }

            ExitMessages = new List<string>();

            Logger = logger;
            Description = description;
            TimeOut = timeout;
            Entities = entities;

            LogMessage("started timing");

            StartTime = DateTime.Now;
        }

        public void AddExitMessage(string message)
        {
            ExitMessages.Add(message);
        }

        public void Error(string message)
        {
            LogError(message);
        }

        public void Message(string message)
        {
            LogMessage(message);
        }

        public void Dispose()
        {
            foreach (string message in ExitMessages)
            {
                LogMessage(message);
            }

            TimeSpan elapsedTime = DateTime.Now - StartTime;

            if (TimeOut.HasValue && elapsedTime.TotalSeconds > TimeOut.Value)
            {
                string message = string.Format("{0}: Timeout of {1} seconds exceeded", Description, TimeOut.Value);
                Logger.Error(message);
            }

            LogMessage(string.Format("finished timing: elapsed time {0} seconds", elapsedTime.TotalSeconds.ToString("0.000")));
        }

        private void LogMessage(string message)
        {
            Logger.Debug(string.Format("{0}: {1}", Description, message));
        }

        private void LogError(string message)
        {
            Logger.Error(string.Format("{0}: {1}", Description, message));
            if (Entities != null)
            {
                Entities.UspErrorLogsInsert(DateTime.Now, "LogisticsCommunicationError", Description, message);
            }
        }

    }
}
