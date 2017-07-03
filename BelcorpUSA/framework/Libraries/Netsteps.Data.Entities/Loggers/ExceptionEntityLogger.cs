namespace NetSteps.Data.Entities.Loggers
{
	using System;

	using NetSteps.Data.Entities.Exceptions;
	using NetSteps.Diagnostics.Logging.Common;
	using NetSteps.Encore.Core.IoC;

	[ContainerRegister(typeof(ILogResolver), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
	public class ExceptionEntityLogger: ILogResolver
	{
		public void LogException(Exception ex, bool isUnhandledException)
		{
			LogException(ex, ex.Message, isUnhandledException);
		}

		public void LogException(Exception ex, string message, bool isUnhandledException)
		{
			EntityExceptionHelper.GetAndLogNetStepsException(
				ex, Constants.NetStepsExceptionType.NetStepsException, null, null, ex.Message);
		}
	}
}