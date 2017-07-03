using System;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Encore.Core.IoC;
using NetSteps.EventProcessing.Common;

namespace NetSteps.Data.Entities.Repositories
{
	[ContainerRegister(typeof(IExceptionLogger), RegistrationBehaviors.IfNoneOther, ScopeBehavior = ScopeBehavior.Singleton)]
	public class ExceptionRepository : IExceptionLogger
	{
		public void LogException(Exception ex)
		{
			ex.Log(Constants.NetStepsExceptionType.NetStepsApplicationException);
		}
	}
}
