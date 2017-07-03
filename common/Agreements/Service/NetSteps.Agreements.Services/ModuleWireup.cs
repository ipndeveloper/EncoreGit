using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;
using System.Data.Entity;
using NetSteps.Encore.Core.IoC;
using NetSteps.Agreements.Common;
using NetSteps.Agreements.Services.Agreements;
using System;

[module: WireupDependency(typeof(NetSteps.Agreements.Services.ModuleWireup))]

namespace NetSteps.Agreements.Services
{
    [WireupDependency(typeof(NetSteps.Encore.Core.ModuleWireup))]
    public class ModuleWireup : WireupCommand
    {
        protected override void PerformWireup(IWireupCoordinator coordinator)
        {
            Database.SetInitializer<AgreementsContext>(null);

            var root = Container.Root;

            root.ForType<Func<IAgreementsContext>>()
                .Register<Func<IAgreementsContext>>((c, p) =>
                    () => c.New<IAgreementsContext>())
                .End();

            root.ForType<IAgreementsService>()
                .Register<AgreementsService>(
                    Param.Resolve<Func<IAgreementsContext>>(),
                    Param.Resolve<IAgreementsRepository>())
                .End();
        }
    }
}
