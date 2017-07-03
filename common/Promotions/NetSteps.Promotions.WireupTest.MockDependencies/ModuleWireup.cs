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

// Indicates this assembly is dependent upon its own wireup command:
[assembly: Wireup(typeof(NetSteps.Promotions.WireupTest.MockDependencies.ModuleWireup))]

namespace NetSteps.Promotions.WireupTest.MockDependencies
{
    public class ModuleWireup : WireupCommand
    {
        protected override void PerformWireup(IWireupCoordinator coordinator)
        {
            Container.Root.ForType<IPriceTypeService>()
                .Register<IPriceTypeService>((c, p) => { return new Mock<IPriceTypeService>().Object; })
                .ResolveAnInstancePerRequest()
                .End();
        }
    }
}
