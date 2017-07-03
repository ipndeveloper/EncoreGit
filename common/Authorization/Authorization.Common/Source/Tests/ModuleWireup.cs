using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Authorization.Common;

[assembly: Wireup(typeof(ModuleWireup))]

namespace NetSteps.Authorization.Common.Test
{
    public class ModuleWireup : WireupCommand
    {
        protected override void PerformWireup(IWireupCoordinator coordinator)
        {

        }
    }
}
