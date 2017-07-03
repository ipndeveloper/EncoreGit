using NetSteps.Addresses.UI.Common.Services;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;

[assembly: Wireup(typeof(NetSteps.Addresses.UI.Mvc.ModuleWireup))]

namespace NetSteps.Addresses.UI.Mvc
{
	/// <summary>
	/// Wireup command called at bootstrap time by the wireup coordinator.
	/// </summary>
	[WireupDependency(typeof(NetSteps.Encore.Core.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Core.Cache.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Common.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Data.Entities.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Addresses.UI.ModuleWireup))]
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

			root.ForType<IAddressCountrySettingsRegistry>()
				.Register((c, p) => new DefaultAddressCountrySettingsRegistry())
				.ResolveAsSingleton()
				.End();

			root.ForType<IAddressModelRegistry>()
				.Register((c, p) => new DefaultAddressModelRegistry())
				.ResolveAsSingleton()
				.End();
		}
	}
}