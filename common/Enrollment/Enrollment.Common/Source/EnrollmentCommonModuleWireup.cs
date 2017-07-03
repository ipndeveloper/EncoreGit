using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.Encore.Core.IoC;
using NetSteps.Enrollment.Common.Models.Config;

[assembly: Wireup(typeof(NetSteps.Enrollment.Common.EnrollmentCommonModuleWireup))]

namespace NetSteps.Enrollment.Common
{
	[WireupDependency(typeof(NetSteps.Encore.Core.ModuleWireup))]
	public class EnrollmentCommonModuleWireup : WireupCommand
	{
		protected override void PerformWireup(IWireupCoordinator coordinator)
		{
		}
	}
}
