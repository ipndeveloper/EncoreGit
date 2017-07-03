using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.Encore.Core.IoC;
using NetSteps.Data.Common.Entities;
using NetSteps.OrderAdjustments.Service.Test.Mocks;

// Indicates this assembly is dependent upon its own wireup command:
[assembly: Wireup(typeof(NetSteps.OrderAdjustments.Service.Test.ModuleWireup))]

namespace NetSteps.OrderAdjustments.Service.Test
{
    [WireupDependency(typeof(NetSteps.OrderAdjustments.Service.ModuleWireup))]
    public class ModuleWireup : WireupCommand
    {
        protected override void PerformWireup(IWireupCoordinator coordinator)
        {
            Container.Root.ForType<IOrderAdjustmentOrderLineModification>()
                .Register<MockOrderAdjustmentOrderLineModification>()
                .ResolveAnInstancePerRequest()
                .End();

            Container.Root.ForType<IOrderAdjustmentOrderModification>()
                .Register<MockOrderAdjustmentOrderModification>()
                .ResolveAnInstancePerRequest()
                .End();

            Container.Root.ForType<IOrderAdjustment>()
                .Register<MockOrderAdjustment>()
                .ResolveAnInstancePerRequest()
                .End();
        }
    }
}
