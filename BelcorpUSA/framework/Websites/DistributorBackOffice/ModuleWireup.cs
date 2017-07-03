using AddressValidator.Common;
using DistributorBackOffice.Helpers;
using NetSteps.Addresses.UI.Common.Services;
using NetSteps.Addresses.UI.Services;
using NetSteps.Data.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.Orders.UI.Models;

[assembly: Wireup(typeof(DistributorBackOffice.ModuleWireup))]

namespace DistributorBackOffice
{
	/// <summary>
	/// Wireup command called at bootstrap time by the wireup coordinator.
	/// </summary>
	[WireupDependency(typeof(NetSteps.Commissions.Service.CommissionsServiceModuleWireup))]
	[WireupDependency(typeof(NetSteps.Data.Entities.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Communication.Services.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.OrderAdjustments.Common.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.OrderAdjustments.Service.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Promotions.Common.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Promotions.Service.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Promotions.Plugins.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Promotions.UI.Service.ModuleWireUp))]
	[WireupDependency(typeof(NetSteps.Bundles.Component.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Bundles.Repository.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Payments.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Web.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Web.Mvc.Controls.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Auth.Service.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Auth.UI.Common.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Auth.UI.Service.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Authorization.Service.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Addresses.UI.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Addresses.UI.Common.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Addresses.UI.Mvc.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Accounts.Downline.UI.Service.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.GiftCards.Service.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Sites.Service.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.NonCommissionablePaymentTypeProvider.Service.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.SSO.Service.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Events.Service.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Web.Validation.Wireup))]
	public class ModuleWireup : WireupCommand
	{
		/// <summary>
		/// Wires this module.
		/// </summary>
		/// <param name="coordinator">the coordinator</param>
		/// <seealso cref="IWireupCoordinator" />
		protected override void PerformWireup(IWireupCoordinator coordinator)
		{
			var root = Container.Root;
			root.ForType<ICategoryHelper>()
				.Register((c, p) => new CategoryHelper())
				.End();
			coordinator.WireupDependencies(typeof(IAddressValidator).Assembly);
			coordinator.WireupDependencies(typeof(OrderDetailModel).Assembly);

			root.ForType<ISitePrincipal>()
				.Register<Infrastructure.SitePrincipal>()
				.ResolveAnInstancePerRequest()
				.End();



			root.ForType<IAddressesService>()
				.Register<AddressesService>()
				.End();

			root.ForType<IAddressCountrySettingsRegistry>()
				.Register<DefaultAddressCountrySettingsRegistry>()
				.End();

			root.ForType<IAddressModelRegistry>()
				.Register<DefaultAddressModelRegistry>()
				.End();
		}
	}
}