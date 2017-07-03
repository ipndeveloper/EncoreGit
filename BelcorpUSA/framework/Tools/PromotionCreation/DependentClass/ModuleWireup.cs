using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSteps.Data.Common.Services;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;

[assembly: Wireup(typeof(DependentClass.ModuleWireup))]

namespace DependentClass
{
    public class ModuleWireup : WireupCommand
    {
        protected override void PerformWireup(IWireupCoordinator coordinator)
        {
            Container.Root.ForType<IPriceTypeService>()
                .Register<JewelKadePriceTypeService>()
                .ResolveAsSingleton()
                .End();

            Container.Root.ForType<ITitleService>()
                .Register<TitleService>()
                .ResolveAsSingleton()
                .End();

            Container.Root.ForType<IOrderService>()
                .Register<OrderService>()
                .ResolveAsSingleton()
                .End();
        }
    }
}
