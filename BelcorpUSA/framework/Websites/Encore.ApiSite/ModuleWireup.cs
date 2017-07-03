using AddressValidator.Common;
using EFTQuery.Common;
using EFTQuery.Service;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;
using UPSQuery.Common;
using UPSQuery.Service;

[assembly: Wireup(typeof(Encore.ApiSite.ModuleWireup))]

namespace Encore.ApiSite
{
	/// <summary>
	/// Wireup command called at bootstrap time by the wireup coordinator.
	/// </summary>
	[WireupDependency(typeof(EFTQuery.Service.ModuleWireup))]
	[WireupDependency(typeof(UPSQuery.Service.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Promotions.Service.ModuleWireup))]
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

			root.ForType<IEFTQueryProcessor>()
					.Register<NachaQueryProcessor>(Param.Resolve<IOrderPaymentRepository>())
					.ResolveAnInstancePerRequest()
					.End();

			root.ForType<IAddressValidator>()
				.Register<NullAddressValidator>()
				.ResolveAsSingleton()
				.End();

			root.ForType<IUPSQueryProcessor>()
				.Register<UPSQueryProcessor>(Param.Resolve<IUpsWorldshipOrderRepository>())
				.ResolveAnInstancePerRequest()
				.End();
		}
	}
}