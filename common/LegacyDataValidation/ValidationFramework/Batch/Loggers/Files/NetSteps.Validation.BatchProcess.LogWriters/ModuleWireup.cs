using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.Validation.BatchProcess.LogWriters.Common;
using NetSteps.Validation.BatchProcess.Common;

[assembly:Wireup(typeof(NetSteps.Validation.BatchProcess.LogWriters.Implementation.ModuleWireup))]

namespace NetSteps.Validation.BatchProcess.LogWriters.Implementation
{
    [WireupDependency(typeof(NetSteps.Validation.BatchProcess.Common.ModuleWireup))]
    [WireupDependency(typeof(NetSteps.Validation.BatchProcess.LogWriters.Common.ModuleWireup))]
    public class ModuleWireup : WireupCommand
    {
        protected override void PerformWireup(IWireupCoordinator coordinator)
        {
            Container.Root.ForType<IMultipleValidationFileLogWriter>()
                     .Register<MultipleValidationFileLogWriter>()
                     .ResolveAnInstancePerRequest()
                     .End();

            Container.Root.ForType<ISingleValidationFileLogWriter>()
                     .Register<SingleValidationFileLogWriter>()
                     .ResolveAnInstancePerRequest()
                     .End();
        }
    }
}
