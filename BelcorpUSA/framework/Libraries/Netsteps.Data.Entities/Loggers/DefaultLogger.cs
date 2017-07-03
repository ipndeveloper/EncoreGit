using NetSteps.Common.Interfaces;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities.Loggers
{
	[ContainerRegister(typeof(ILogger), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
	public class DefaultLogger : ILogger
	{
		public void Debug(string msg)
		{
		}

		public void Debug(string format, params object[] args)
		{
		}

		public void Info(string msg)
		{
		}

		public void Info(string format, params object[] args)
		{
		}

		public void Error(string msg)
		{
		}

		public void Error(string format, params object[] args)
		{
		}

		public void AttemptAtBlockedPage(string url, string referringUrl)
		{
		}
	}
}
