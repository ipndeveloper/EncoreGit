using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;

[assembly: Wireup(typeof(NetSteps.Enrollment.XmlConfiguration.EnrollmentXmlConfigurationModuleWireup))]
namespace NetSteps.Enrollment.XmlConfiguration
{
	[WireupDependency(typeof(NetSteps.Enrollment.Common.EnrollmentCommonModuleWireup))]
	public class EnrollmentXmlConfigurationModuleWireup : WireupCommand
	{
		protected override void PerformWireup(IWireupCoordinator coordinator)
		{
		}
	}
}
