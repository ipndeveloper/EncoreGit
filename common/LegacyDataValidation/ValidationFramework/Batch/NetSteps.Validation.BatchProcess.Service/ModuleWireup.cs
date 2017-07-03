using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.Validation.BatchProcess.Common;
using NetSteps.Validation.Common;

[assembly: Wireup(typeof(NetSteps.Validation.BatchProcess.Service.ModuleWireup))]

namespace NetSteps.Validation.BatchProcess.Service
{
    [WireupDependency(typeof(NetSteps.Validation.BatchProcess.Common.ModuleWireup))]
    public class ModuleWireup : WireupCommand
    {
        protected override void PerformWireup(IWireupCoordinator coordinator)
        {
            Container.Root.ForType<IBatchValidationService>()
                     .Register<BatchValidationService>
                                                (
                                                    Param.Resolve<IRecordValidationService>()
                                                )
                     .ResolveAsSingleton()
                     .End();

            Container.Root.ForType<IRecordValidationResultManager>()
                     .Register<ValidationLogManager>()
                     .ResolveAnInstancePerRequest()
                     .End();
        }
    }
}
