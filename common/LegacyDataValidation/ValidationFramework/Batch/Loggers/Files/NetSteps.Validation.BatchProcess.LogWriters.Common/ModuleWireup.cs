using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;

[assembly:Wireup(typeof(NetSteps.Validation.BatchProcess.LogWriters.Common.ModuleWireup))]

namespace NetSteps.Validation.BatchProcess.LogWriters.Common
{
    public class ModuleWireup : WireupCommand
    {
        protected override void PerformWireup(IWireupCoordinator coordinator)
        {
            
        }
    }
}
