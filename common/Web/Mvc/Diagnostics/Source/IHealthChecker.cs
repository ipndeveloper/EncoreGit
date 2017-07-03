using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Web.Mvc.Diagnostics
{
	public interface IHealthChecker
	{
		bool PerformHealthCheck();
	}
}
