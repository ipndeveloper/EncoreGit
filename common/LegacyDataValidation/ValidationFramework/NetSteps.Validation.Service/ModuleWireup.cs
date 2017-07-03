using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.Validation.Common;
using NetSteps.Validation.Common.Core;
using NetSteps.Validation.Service;

[assembly:Wireup(typeof(NetSteps.Validation.Service.ModuleWireup))]

namespace NetSteps.Validation.Service
{
    [WireupDependency(typeof(NetSteps.Validation.Common.ModuleWireup))]
    public class ModuleWireup : WireupCommand
    {
        protected override void PerformWireup(IWireupCoordinator coordinator)
        {

            Container.Root.ForType<IRecordValidationService>()
                     .Register<RecordValidationService>(Param.Resolve<IRecordPropertyCalculationHandlerResolver>())
                     .ResolveAsSingleton()
                     .End();

        }
    }
}
