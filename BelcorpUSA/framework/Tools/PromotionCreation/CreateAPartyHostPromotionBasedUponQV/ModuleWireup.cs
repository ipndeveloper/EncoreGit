using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;

[assembly: Wireup(typeof(CreateAPartyHostPromotionBasedUponQV.ModuleWireup))]

namespace CreateAPartyHostPromotionBasedUponQV
{
    [WireupDependency(typeof(DependentClass.ModuleWireup))]
    [WireupDependency(typeof(NetSteps.Promotions.Common.ModuleWireup))]
    [WireupDependency(typeof(NetSteps.Promotions.Service.ModuleWireup))]
    [WireupDependency(typeof(NetSteps.Promotions.Plugins.ModuleWireup))]
    [WireupDependency(typeof(NetSteps.Promotions.Caching.ModuleWireup))]
    public class ModuleWireup : WireupCommand
    {
        protected override void PerformWireup(IWireupCoordinator coordinator)
        {
            Console.WriteLine("Wiring up....");
        }
    }
}
