using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using NetSteps.Data.Common.Services;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.Promotions.Common;
using NetSteps.Promotions.Plugins.Tests;

[assembly: WireupDependency(WireupPhase.Immediate, typeof(PreWireup))]
[assembly: Wireup(typeof(NetSteps.Promotions.Plugins.Tests.ModuleWireup))]

namespace NetSteps.Promotions.Plugins.Tests
{
    [WireupDependency(typeof(Promotions.Plugins.ModuleWireup))]
    [WireupDependency(typeof(NetSteps.Core.Cache.ModuleWireup))]
    public class ModuleWireup : WireupCommand
    {
        protected override void PerformWireup(IWireupCoordinator coordinator)
        {
            
        }
    }

    public class PreWireup : WireupCommand
    {

        protected override void PerformWireup(IWireupCoordinator coordinator)
        {
            Container.Root.ForType<IPriceTypeService>()
                .Register<IPriceTypeService>((c, p) => { return new Mock<IPriceTypeService>().Object; })
                .ResolveAnInstancePerRequest()
                .End();

            Container.Root.ForType<ITitleService>()
                .Register<ITitleService>((c, p) => { return new Mock<ITitleService>().Object; })
                .ResolveAnInstancePerRequest()
                .End();

            Container.Root.ForType<IOrderService>()
                .Register<IOrderService>((c, p) => { return new Mock<IOrderService>().Object; })
                .ResolveAnInstancePerRequest()
                .End();

            Container.Root.ForType<IPromotionProvider>()
                .Register<IPromotionProvider>((c, p) => { return new Mock<IPromotionProvider>().Object; })
                .ResolveAnInstancePerRequest()
                .End();
        }
    }
}
