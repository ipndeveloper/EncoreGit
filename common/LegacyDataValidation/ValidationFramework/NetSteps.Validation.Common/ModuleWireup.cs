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

[assembly : Wireup(typeof(ModuleWireup))]

namespace NetSteps.Validation.Common
{
    public class ModuleWireup : WireupCommand
    {
        protected override void PerformWireup(IWireupCoordinator coordinator)
        {
            Func<string, string, bool> hasRegisteredHandler = (recordKind, propertyName) => Container.Current.Registry.IsTypeRegisteredWithName<IRecordPropertyCalculationHandler>(String.Format("{0}.{1}", recordKind, propertyName));
            Func<string, string, IRecordPropertyCalculationHandler> handlerFactory = (recordKind, propertyName) => Create.NewNamed<IRecordPropertyCalculationHandler>(String.Format("{0}.{1}", recordKind, propertyName));

            Container.Root.ForType<IRecordPropertyCalculationHandlerResolver>()
                     .Register<RecordPropertyCalculationHandlerResolver>(Param.Value(handlerFactory), Param.Value(hasRegisteredHandler))
                     .ResolveAsSingleton()
                     .End();

            
        }
    }
}
