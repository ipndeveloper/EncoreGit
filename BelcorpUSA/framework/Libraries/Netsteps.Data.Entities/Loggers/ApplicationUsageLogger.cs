using System;
using System.Diagnostics;
using NetSteps.Common.Utility;
using NetSteps.Encore.Core;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Parallel;
using Threading = System.Threading;

namespace NetSteps.Data.Entities.Business
{
    /// <summary>
    /// Author: John Egbert
    /// Description: Class to be used in a Using statement wrapping method calls to measure and log the 
    ///     frequency and duration of method calls to the DB.
    /// Example Usage:   using (new ApplicationUsageLogger(new ExecutionContext(this)))
    ///                 {
    ///                     // Actually method logic here
    ///                 }
    /// Created: 03-11-2010
    /// </summary>
    public class ApplicationUsageLogger : IDisposable
    {
        class LogAndLogger
        {
            public ApplicationUsageLog Log { get; set; }
            public IApplicationUsageLoggerAction Logger { get; set; }

        }

        #region Static Members

        private static Reactor<LogAndLogger> __innerLogger;

        #endregion

        #region Members

        private Int64 _startTime;

        #endregion

        /// <summary>
        /// Gets and sets whether logger is enabled.
        /// </summary>
        public static bool IsEnabled
        {
            get
            {
                return CurrentLogger != null;
            }

            set
            {
                if (CurrentLogger == null)
                {
                    if (value)
                    {
                        __innerLogger = new Reactor<LogAndLogger>((self, it) =>
                        {
                            if (it != null)
                            {
                                it.Logger.DoAction(it.Log);
                            }
                        });
                    }
                }
                else if (!value)
                {
                    Util.Dispose(ref __innerLogger);
                }
            }
        }

        internal static bool IsEmpty
        {
            get
            {
                var current = CurrentLogger;
                return current == null || current.IsEmpty;
            }
        }

        private static Reactor<LogAndLogger> CurrentLogger
        {
            get
            {
                Threading.Thread.MemoryBarrier();
                var current = __innerLogger;
                Threading.Thread.MemoryBarrier();

                return current;
            }
        }

        #region Constructors
        public ApplicationUsageLogger(ExecutionContext executionContext)
        {
            this.ExecutionContext = executionContext;

            // This should be the last statement in this method to keep timing as accurate as possible.
            _startTime = Stopwatch.GetTimestamp();
        }
        #endregion

        #region Methods

        public ExecutionContext ExecutionContext { get; private set; }

        public virtual void Dispose()
        {
            var log = GetApplicationUsageLog();

            var current = CurrentLogger;
            if (current != null)
            {
                current.Push(new LogAndLogger
                {
                    Log = log,
                    Logger = Create.New<IApplicationUsageLoggerAction>()
                });
            }
        }

        public virtual ApplicationUsageLog GetApplicationUsageLog()
        {
            var context = this.ExecutionContext;
            if (context == null
                || !ApplicationContext.Instance.EnableApplicationUsageLogging
                || ExecutionContext.ClassName == "ApplicationUsageLogRepository")
            {
                return null;
            }

            var timeSpanMilliseconds = (Stopwatch.GetTimestamp() - _startTime) / (double)Stopwatch.Frequency * 1000;

            ApplicationUsageLog applicationUsageLog = new ApplicationUsageLog
                {
                    UsageDate = DateTime.Now,
                    ApplicationID = context.ApplicationID,
                    UserID = null,
                    AssemblyName = context.AssemblyName,
                    MachineName = context.MachineName,
                    ClassName = context.ClassName,
                    MethodName = context.MethodName,
                    MillisecondDuration = timeSpanMilliseconds
                };

            if (context.UserID != 0)
            {
                applicationUsageLog.UserID = context.UserID;
            }

            return applicationUsageLog;
        }
       
        #endregion
    }

    public interface IApplicationUsageLoggerAction
    {
        void DoAction(ApplicationUsageLog log);
    }

    [ContainerRegister(typeof(IApplicationUsageLoggerAction), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
    public class ApplucationUsageLoggerAction : IApplicationUsageLoggerAction
    {
        public void DoAction(ApplicationUsageLog log)
        {
            log.Save();
        }
    }
}
