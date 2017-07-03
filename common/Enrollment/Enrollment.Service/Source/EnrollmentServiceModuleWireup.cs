using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;

[assembly: Wireup(typeof(NetSteps.Enrollment.Service.EnrollmentServiceModuleWireup))]

namespace NetSteps.Enrollment.Service
{
	[WireupDependency(typeof(NetSteps.Enrollment.Common.EnrollmentCommonModuleWireup))]
	public class EnrollmentServiceModuleWireup : WireupCommand
	{
		protected override void PerformWireup(IWireupCoordinator coordinator)
		{
		}
	}
}
