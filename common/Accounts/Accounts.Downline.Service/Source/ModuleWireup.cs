using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using NetSteps.Accounts.Downline.Common.Repositories;
using NetSteps.Accounts.Downline.Service.Context;
using NetSteps.Accounts.Downline.Service.Repositories;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;

[module: WireupDependency(typeof(NetSteps.Accounts.Downline.Service.ModuleWireup))]

namespace NetSteps.Accounts.Downline.Service
{
    [WireupDependency(typeof(NetSteps.Encore.Core.ModuleWireup))]
    public class ModuleWireup : WireupCommand
    {
        protected override void PerformWireup(IWireupCoordinator coordinator)
        {
            // Disable EF migrations.
            Database.SetInitializer<DownlineDbContext>(null);

            var root = Container.Root;

			root.ForType<Func<IDownlineDbContext>>()
				.Register<Func<IDownlineDbContext>>((c, p) =>
					() => c.New<IDownlineDbContext>())
                .End();

			root.ForType<IDownlineRepository>()
				.Register<DownlineRepository>(
					Param.Resolve<Func<IDownlineDbContext>>()
				)
				.End();
		}
    }
}