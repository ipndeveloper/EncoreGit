using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.Validation.BatchProcess.Common;
using NetSteps.Validation.BatchProcess.TempTableWriters.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly : Wireup(typeof(NetSteps.Validation.BatchProcess.TempTableWriters.Implementation.ModuleWireup))]

namespace NetSteps.Validation.BatchProcess.TempTableWriters.Implementation
{
    public class ModuleWireup : WireupCommand
    {
        protected override void PerformWireup(IWireupCoordinator coordinator)
        {
            Container.Root.ForType<ITempTableOutputWriter>()
                     .Register<TempTableOutputWriter>()
                     .ResolveAnInstancePerRequest()
                     .End();
        }
    }
}
