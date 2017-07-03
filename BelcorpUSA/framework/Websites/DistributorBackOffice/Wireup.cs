using AddressValidator.Common;
using DistributorBackOffice.Helpers;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.Data.Common;
using DistributorBackOffice.Infrastructure;

[module: WireupDependency(typeof(DistributorBackOffice.ModuleWireup))]

namespace DistributorBackOffice
{
    /// <summary>
    /// Wireup command called at bootstrap time by the wireup coordinator.
    /// </summary>
    [WireupDependency(typeof(NetSteps.Encore.Core.ModuleWireup))]
    [WireupDependency(typeof(NetSteps.Core.Cache.ModuleWireup))]
    [WireupDependency(typeof(NetSteps.Common.ModuleWireup))]
    [WireupDependency(typeof(NetSteps.Communication.Services.ModuleWireup))]
    [WireupDependency(typeof(NetSteps.Data.Entities.ModuleWireup))]
    [WireupDependency(typeof(NetSteps.OrderAdjustments.Common.ModuleWireup))]
    [WireupDependency(typeof(NetSteps.OrderAdjustments.Service.ModuleWireup))]
    [WireupDependency(typeof(NetSteps.Web.ModuleWireup))]
    [WireupDependency(typeof(NetSteps.Promotions.Plugins.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Promotions.Common.ModuleWireup))]
    [WireupDependency(typeof(NetSteps.Promotions.Service.ModuleWireup))]
    [WireupDependency(typeof(NetSteps.Promotions.UI.Service.ModuleWireUp))]
	[WireupDependency(typeof(AddressValidator.Common.ModuleWireup))]

    [WireupDependency(typeof(NetSteps.SOD.Common.ModuleWireup))]
    [WireupDependency(typeof(NetSteps.SOD.Framework.Plugin.ModuleWireup))]
    [WireupDependency(typeof(NetSteps.SOD.Wrapper.ModuleWireup))]
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

            root.ForType<ICategoryHelper>()
                .Register((c, p) => new CategoryHelper())
                .End();

            root.ForType<ISitePrincipal>()
                .Register<SitePrincipal>()
                .ResolveAnInstancePerRequest()
                .End();

        }
    }
}