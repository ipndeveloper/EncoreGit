using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using NetSteps.Communication.Common;
using NetSteps.Communication.UI.Common;
using NetSteps.Communication.UI.Services;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;

[module: WireupDependency(typeof(NetSteps.Communication.Services.ModuleWireup))]

namespace NetSteps.Communication.Services
{
    [WireupDependency(typeof(NetSteps.Encore.Core.ModuleWireup))]
    public class ModuleWireup : WireupCommand
    {
        protected override void PerformWireup(IWireupCoordinator coordinator)
        {
            // Disable EF migrations.
            Database.SetInitializer<CommunicationContext>(null);

            var root = Container.Root;

            // Context
            root.ForType<Func<ICommunicationContext>>()
                .Register<Func<ICommunicationContext>>((c, p) =>
                    () => c.New<ICommunicationContext>())
                .End();

            // AccountAlert
            root.ForType<IAccountAlertService>()
                .Register<AccountAlertService>(
                    Param.Resolve<IAccountAlertProviderCollection>(),
                    Param.Resolve<Func<ICommunicationContext>>()
                )
                .End();
            root.ForType<IAccountAlertProviderCollection>()
                .Register<AccountAlertProviderCollection>(
                    Param.Resolve<IAccountAlertConfiguration>()
                )
                .ResolveAsSingleton()
                .End();

            // PromotionAccountAlert
            root.ForType<IPromotionAccountAlertService>()
                .Register<PromotionAccountAlertService>(
                    Param.Resolve<Func<ICommunicationContext>>(),
                    Param.Resolve<IPromotionAccountAlertRepository>()
                )
                .End();
            root.ForType<IPromotionAccountAlertProvider>()
                .Register<PromotionAccountAlertProvider>(
                    Param.Resolve<IPromotionAccountAlertService>()
                )
                .End();

            // MessageAccountAlert
            root.ForType<IMessageAccountAlertService>()
                .Register<MessageAccountAlertService>(
                    Param.Resolve<Func<ICommunicationContext>>(),
                    Param.Resolve<IMessageAccountAlertRepository>()
                )
                .End();
            root.ForType<IMessageAccountAlertProvider>()
                .Register<MessageAccountAlertProvider>(
                    Param.Resolve<IMessageAccountAlertService>()
                )
                .End();

            // AccountAlertUI
            root.ForType<IAccountAlertUIService>()
                .Register<AccountAlertUIService>(
                    Param.Resolve<IAccountAlertUIProviderCollection>(),
                    Param.Resolve<IAccountAlertService>()
                )
                .End();
            root.ForType<IAccountAlertUIProviderCollection>()
                .Register<AccountAlertUIProviderCollection>(
                    Param.Resolve<IAccountAlertUIConfiguration>()
                )
                .ResolveAsSingleton()
                .End();

            // MessageAccountAlertUI
            root.ForType<IMessageAccountAlertUIProvider>()
                .Register<MessageAccountAlertUIProvider>(
                    Param.Resolve<IMessageAccountAlertService>()
                )
                .End();
        }
    }
}