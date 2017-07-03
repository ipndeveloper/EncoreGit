using Microsoft.Practices.EnterpriseLibrary.Logging;
using NetSteps.Encore.Core.IoC;
using NetSteps.QueueProcessing.Common;

namespace NetSteps.QueueProcessing.Service
{
    [ContainerRegister(typeof(IQueueProcessorLogger), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public class QueueProcessorLogger : IQueueProcessorLogger
    {
        public QueueProcessorLogger()
        {
        }

        public void Debug(string msg)
        {
            Logger.Write(msg, "Debug");
        }

        public void Debug(string format, params object[] args)
        {
            Logger.Write(string.Format(format, args), "Debug");
        }

        public void Error(string msg)
        {
            Logger.Write(msg, "Error");
        }

        public void Error(string format, params object[] args)
        {
            Logger.Write(string.Format(format, args), "Error");
        }

        public void Info(string msg)
        {
            Logger.Write(msg, "Info");
        }

        public void Info(string format, params object[] args)
        {
            Logger.Write(string.Format(format, args), "Info");
        }
    }
}