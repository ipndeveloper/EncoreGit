using System;
using System.Collections.Generic;
using NetSteps.Common.Configuration;
using NetSteps.Core.Cache;
using NetSteps.Data.Common.Services;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Data.Entities.Services;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.Sites.Common.Models;
using NSCR = NetSteps.Sites.Common.Repositories;
using NetSteps.Data.Entities.Tax;

// Indicates this assembly is dependent upon its own wireup command:
[assembly: Wireup(typeof(NetSteps.Data.Entities.ModuleWireup))]

namespace NetSteps.Data.Entities
{
	/// <summary>
	/// Wireup command called at bootstrap time by the wireup coordinator.
	/// </summary>
	[WireupDependency(typeof(NetSteps.Encore.Core.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Core.Cache.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Common.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Promotions.Common.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Extensibility.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.EventProcessing.Repository.Wireup))]
	[WireupDependency(typeof(NetSteps.Events.AccountArguments.Repository.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Events.PartyArguments.Repository.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Events.EmailEventTemplate.Repository.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.AccountLocatorService.Common.ModuleWireup))]
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

			root.ForType<ITaxService>()
				.Register<NetSteps.Data.Entities.Tax.Avatax.AvataxCalculator>()
				.ResolveAnInstancePerRequest()
				.End();

			root.ForType<ITitleService>()
				.Register<TitleService>()
				.ResolveAnInstancePerRequest()
				.End();

			root.ForGenericType(typeof(INetStepsInformer<>))
				.Register(typeof(NullInformer<>))
				.ResolveAnInstancePerScope()
				.End();

			root.ForType<IOrderPromotionQualification>()
					.Register((c, p) => new OrderPromotionQualification())
					.ResolveAsSingleton()
					.End();

			root.ForType<IOrderSearchCache>()
					.Register((c, p) => new OrderSearchCache())
					.ResolveAsSingleton()
					.End();

			root.ForType<InventoryBaseRepository>()
					.Register<InventoryBaseRepository>(
							(c, p) =>
							{
								if (ConfigurationManager.UseInventoryCache)
								{
									return new InventoryCacheRepository();
								}
								else
								{
									return new InventorySlimCacheRepository();
								}
							})
					.ResolveAsSingleton()
					.End()
					.ForType<Func<InventoryBaseRepository>>()
					.Register<Func<InventoryBaseRepository>>((c, p) => c.New<InventoryBaseRepository>)
					.ResolveAnInstancePerRequest()
					.End();

			root.ForType<IProductService>()
					.Register<ProductService>()
					.ResolveAsSingleton()
					.End();

			root.ForType<IAutoshipOrderService>()
					.Register<AutoshipOrderService>()
					.ResolveAsSingleton()
					.End();

			root.ForType<IOrderService>()
					.Register<OrderService>()
					.ResolveAsSingleton()
					.End();

			root.ForType<IAccountObserver>()
				.Register<AccountObserver>()
				.ResolveAsSingleton()
				.End();

			root.ForType<IInventoryService>()
				.Register<InventoryService>()
				.ResolveAsSingleton()
				.End();

			// SiteCache
			root.ForType<ICache<int, ISite>>()
				.RegisterWithName<MruLocalMemoryCache<int, ISite>>(
					Constants.CacheNames.SiteCache,
					Param.Value(Constants.CacheNames.SiteCache)
				)
				.ResolveAsSingleton()
				.End();

			// UrlSiteLookupCache
			root.ForType<ICache<string, Tuple<int?>>>()
				.RegisterWithName<MruLocalMemoryCache<string, Tuple<int?>>>(
					Constants.CacheNames.UrlSiteLookupCache,
					Param.Value(Constants.CacheNames.UrlSiteLookupCache)
				)
				.ResolveAsSingleton()
				.End();

			// SiteSettingKindsCache
			root.ForType<ICache<int, ICollection<ISiteSetting>>>()
				.RegisterWithName<MruLocalMemoryCache<int, ICollection<ISiteSetting>>>(
					Constants.CacheNames.SiteSettingKindsCache,
					Param.Value(Constants.CacheNames.SiteSettingKindsCache)
				)
				.ResolveAsSingleton()
				.End();

			// SiteSettingsCache
			root.ForType<ICache<int, IDictionary<string, string>>>()
				.RegisterWithName<MruLocalMemoryCache<int, IDictionary<string, string>>>(
					Constants.CacheNames.SiteSettingsCache,
					Param.Value(Constants.CacheNames.SiteSettingsCache)
				)
				.ResolveAsSingleton()
				.End();

			root.ForType<NSCR.ISiteRepository>()
				.Register<SiteRepositoryCached>(
					Param.ResolveNamed<ICache<int, ISite>>(Constants.CacheNames.SiteCache),
					Param.ResolveNamed<ICache<string, Tuple<int?>>>(Constants.CacheNames.UrlSiteLookupCache)
				)
				.ResolveAsSingleton()
				.End();

			root.ForType<NSCR.ISiteSettingRepository>()
				.Register<SiteSettingRepositoryCached>(
					Param.ResolveNamed<ICache<int, ICollection<ISiteSetting>>>(Constants.CacheNames.SiteSettingKindsCache),
					Param.ResolveNamed<ICache<int, IDictionary<string, string>>>(Constants.CacheNames.SiteSettingsCache)
				)
				.ResolveAsSingleton()
				.End();
		}
	}
}
