using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Web.Mvc.Diagnostics
{
	[ContainerRegister(typeof(IHealthChecker), RegistrationBehaviors.IfNoneOther)]
	public class DefaultHealthChecker : IHealthChecker
	{
		public bool PerformHealthCheck()
		{
			return true;
		}
	}
}
