using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;

[assembly : Wireup(typeof(JewelKade.OrderValidation.ConsoleApplication.ModuleWireup))]

namespace JewelKade.OrderValidation.ConsoleApplication
{
    [WireupDependency(typeof(NetSteps.Validation.Common.ModuleWireup))]
    [WireupDependency(typeof(NetSteps.Validation.Handlers.ModuleWireup))]
    [WireupDependency(typeof(NetSteps.Validation.Service.ModuleWireup))]
    [WireupDependency(typeof(NetSteps.Validation.BatchProcess.Common.ModuleWireup))]
    [WireupDependency(typeof(NetSteps.Validation.BatchProcess.Service.ModuleWireup))]
    [WireupDependency(typeof(NetSteps.Validation.BatchProcess.LogWriters.Implementation.ModuleWireup))]
    [WireupDependency(typeof(NetSteps.Validation.BatchProcess.TempTableWriters.Implementation.ModuleWireup))]
    [WireupDependency(typeof(NetSteps.Validation.Serializers.ModuleWireup))]
    [WireupDependency(typeof(JewelKade.Orders.ModuleWireup))]
    public class ModuleWireup : WireupCommand
    {
        protected override void PerformWireup(IWireupCoordinator coordinator)
        {
            
        }
    }
}
