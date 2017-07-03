using System;
using System.Data.Entity;
using NetSteps.Communication.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.Promotions.Common;
using NetSteps.Promotions.UI.Common.Interfaces;
using NetSteps.Promotions.UI.Service.Context;
using NetSteps.Promotions.UI.Service.Impl;

[assembly: Wireup(typeof(NetSteps.Promotions.UI.Service.ModuleWireUp))]
namespace NetSteps.Promotions.UI.Service
{
    [WireupDependency(typeof(Encore.Core.ModuleWireup))]
    [WireupDependency(typeof(Core.Cache.ModuleWireup))]
    [WireupDependency(typeof(Promotions.Common.ModuleWireup))]
    public class ModuleWireUp : WireupCommand
    {
        protected override void PerformWireup(IWireupCoordinator coordinator)
        {
            // Disable EF migrations.
            Database.SetInitializer<PromotionsUIContext>(null);

            var root = Container.Root;
            //root.ForType<IContentProxyData>().Register<FakeProxyData>().End();
            //root.ForType<IContentProxyService>().Register<ContentProxyService>().End();
            //root.ForType<ITermLocalizedProxy>().Register<TermLocalizedProxy>().End();
            root.ForType<IDisplayInfo>().Register<DisplayInfo>().End();
            root.ForType<IAlertInfo>().Register<AlertInfo>().End();
            //root.ForType<IPromotionInfoContext>().Register<PromotionInfoContext>().End();

            root.ForType<IPromotionInfoService>()
                .Register<PromotionInfoService>(
                    Param.Resolve<IPromotionService>(),
                    Param.Resolve<IPromotionContentService>()
                )
                .End();

            root.ForType<Func<IPromotionsUIContext>>()
                .Register<Func<IPromotionsUIContext>>((c, p) =>
                    () => c.New<IPromotionsUIContext>())
                .End();

            root.ForType<IPromotionContentService>()
                .Register<PromotionContentService>(
                    Param.Resolve<Func<IPromotionsUIContext>>(),
                    Param.Resolve<IPromotionContentRepository>()
                )
                .End();

            root.ForType<IPromotionAccountAlertUIProvider>()
                .Register<PromotionAccountAlertUIProvider>(
                    Param.Resolve<IPromotionAccountAlertService>(),
                    Param.Resolve<IPromotionInfoService>()
                )
                .End();
        }
    }
}
