using System;
using System.Configuration;
using NetSteps.Common;
using NetSteps.Common.EldResolver;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.Sites.Common;
using NetSteps.Sites.Common.Configuration;
using NetSteps.Sites.Common.Repositories;
using NetSteps.Sites.Service.Configuration;

// Indicates this assembly is dependent upon its own wireup command:
[assembly: Wireup(typeof(NetSteps.Sites.Service.ModuleWireup))]

namespace NetSteps.Sites.Service
{
	/// <summary>
	/// Wireup command called at bootstrap time by the wireup coordinator.
	/// </summary>
	[WireupDependency(typeof(NetSteps.Encore.Core.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Common.ModuleWireup))]
	public class ModuleWireup : WireupCommand
	{
		/// <summary>
		/// Wires this module.
		/// </summary>
		protected override void PerformWireup(IWireupCoordinator coordinator)
		{
			var root = Container.Root;

			root.ForType<IAnalyticsConfiguration>()
				.Register<AnalyticsConfiguration>(
					Param.Value(ConfigurationManager.GetSection("analytics") as AnalyticsConfigurationSection)
				)
				.ResolveAsSingleton()
				.End();

			root.ForType<ISitesConfiguration>()
				.Register<SitesConfigurationSection>((c, p) => ConfigurationManager.GetSection("netsteps.sites") as SitesConfigurationSection ?? new SitesConfigurationSection())
				.ResolveAsSingleton()
				.End();

			root.ForType<ISiteService>()
				.Register<SiteService>(
					Param.Resolve<ISiteRepository>(),
					Param.Resolve<ISiteSettingRepository>(),
					Param.Resolve<ISitesConfiguration>(),
					Param.Resolve<IAnalyticsConfiguration>(),
					Param.Resolve<IEldResolver>()
				)
				.End();
		}
	}
}