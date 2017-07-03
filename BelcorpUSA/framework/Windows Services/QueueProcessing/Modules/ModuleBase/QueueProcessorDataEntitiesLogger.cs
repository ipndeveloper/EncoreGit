using NetSteps.Common.Interfaces;
using NetSteps.Encore.Core.IoC;
using NetSteps.QueueProcessing.Common;

namespace NetSteps.QueueProcessing.Modules.ModuleBase
{
	[ContainerRegister(typeof(ILogger), RegistrationBehaviors.DefaultOrOverrideDefault, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
	public class QueueProcessorDataEntitiesLogger : ILogger
	{
		IQueueProcessorLogger logger = Create.New<IQueueProcessorLogger>();

		public void Info(string msg)
		{
			logger.Info(msg);
		}

		public void Info(string format, params object[] args)
		{
			logger.Info(format, args);
		}

		public void Error(string msg)
		{
			logger.Error(msg);
		}

		public void Error(string format, params object[] args)
		{
			logger.Error(format, args);
		}

		public void Debug(string msg)
		{
			logger.Debug(msg);
		}

		public void Debug(string format, params object[] args)
		{
			logger.Debug(format, args);
		}

		public void AttemptAtBlockedPage(string url, string referringUrl)
		{
			// do nothing
		}
	}
}
