using System;
using NetSteps.Common.Configuration;
using NetSteps.Common.EldResolver;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;

// Indicates this assembly is dependent upon its own wireup command:
[assembly: Wireup(typeof(NetSteps.Common.ModuleWireup))]

namespace NetSteps.Common
{
    /// <summary>
    /// Wireup command called at bootstrap time by the wireup coordinator.
    /// </summary>
    [WireupDependency(typeof(NetSteps.Encore.Core.ModuleWireup))]
    public class ModuleWireup : WireupCommand
    {
        /// <summary>
        /// Wires this module.
        /// </summary>
        protected override void PerformWireup(IWireupCoordinator coordinator)
        {
            Container.Root
                .ForType<IEldResolver>()
                .Register((c, p) => new NetSteps.Common.EldResolver.DefaultEldResolver(
					ConfigurationManager.GetAppSetting(ConfigurationManager.VariableKey.EnvironmentLevelDomain, string.Empty)
				))
                .End();
        }
    }
}
