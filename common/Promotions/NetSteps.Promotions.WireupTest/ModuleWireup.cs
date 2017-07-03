using Moq;
using NetSteps.Data.Common.Services;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSteps.Promotions.WireupTest;

// Indicates this assembly is dependent upon its own wireup command:
[assembly: WireupDependency(WireupPhase.Immediate, typeof(PreWireup))]
[assembly: Wireup(typeof(NetSteps.Promotions.WireupTest.ModuleWireup))]

namespace NetSteps.Promotions.WireupTest
{
    [WireupDependency(typeof(NetSteps.Promotions.WireupTest.MockDependencies.ModuleWireup))]
    [WireupDependency(typeof(Promotions.Common.ModuleWireup))]
    [WireupDependency(typeof(Promotions.Service.ModuleWireup))]
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
        }
    }
}
