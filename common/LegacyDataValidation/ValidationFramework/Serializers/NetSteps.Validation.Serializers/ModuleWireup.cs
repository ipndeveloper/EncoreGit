using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.Validation.Common;

[assembly:Wireup(typeof(NetSteps.Validation.Serializers.ModuleWireup))]

namespace NetSteps.Validation.Serializers
{
    public class ModuleWireup : WireupCommand
    {
        protected override void PerformWireup(IWireupCoordinator coordinator)
        {
            Container.Root.ForType<IRecordValidationSerializer>()
                     .RegisterWithName<SqlSerializer>("SqlSerializer")
                     .ResolveAnInstancePerRequest()
                     .End();

            Container.Root.ForType<IRecordValidationSerializer>()
                     .RegisterWithName<XmlSerializer>("XmlSerializer")
                     .ResolveAnInstancePerRequest()
                     .End();

            Container.Root.ForType<IRecordValidationSerializer>()
                     .RegisterWithName<FlatTextSerializer>("FlatTextSerializer")
                     .ResolveAnInstancePerRequest()
                     .End();

            Container.Root.ForType<IRecordValidationSerializer>()
                     .RegisterWithName<CommaDelimitedSerializer>("CommaDelimitedSerializer")
                     .ResolveAnInstancePerRequest()
                     .End();

            Container.Root.ForType<IRecordValidationSerializer>()
                     .RegisterWithName<FieldReportCommaDelimitedSerializer>("FieldReportCommaDelimitedSerializer")
                     .ResolveAnInstancePerRequest()
                     .End();

        }
    }
}
