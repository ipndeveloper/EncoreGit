using NetSteps.Common.EldResolver;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;

[assembly: Wireup(typeof(nsCore.ModuleWireup))]

namespace nsCore
{
	/// <summary>
	/// Wireup command called at bootstrap time by the wireup coordinator.
	/// </summary>
	[WireupDependency(typeof(NetSteps.Commissions.Service.CommissionsServiceModuleWireup))]
	[WireupDependency(typeof(NetSteps.Data.Entities.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Sites.Service.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Communication.Services.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.OrderAdjustments.Common.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.OrderAdjustments.Service.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Promotions.Common.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Promotions.Service.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Promotions.Plugins.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Promotions.UI.Service.ModuleWireUp))]
	[WireupDependency(typeof(AddressValidator.Common.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Auth.Service.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Auth.UI.Service.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Payments.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.GiftCards.Service.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Authorization.Service.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Web.Mvc.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Shipping.Service.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.NonCommissionablePaymentTypeProvider.Service.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.SSO.Service.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Enrollment.Service.EnrollmentServiceModuleWireup))]
	[WireupDependency(typeof(NetSteps.Enrollment.XmlConfiguration.EnrollmentXmlConfigurationModuleWireup))]
	[WireupDependency(typeof(NetSteps.Events.Service.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Web.Validation.Wireup))]
	[WireupDependency(typeof(NetSteps.Enrollment.Common.EnrollmentCommonModuleWireup))]
	public class ModuleWireup : WireupCommand
	{
		/// <summary>
		/// Wires this module.
		/// </summary>
		/// <param name="coordinator">the coordinator</param>
		/// <seealso cref="IWireupCoordinator"/>
		protected override void PerformWireup(IWireupCoordinator coordinator)
		{
			var root = Container.Root;

			var eld = System.Configuration.ConfigurationManager.AppSettings["EnvironmentLevelDomain"];
			root.ForType<IEldResolver>()
				.Register((c, p) => new DefaultEldResolver(eld))
				.End();
		}
	}
}