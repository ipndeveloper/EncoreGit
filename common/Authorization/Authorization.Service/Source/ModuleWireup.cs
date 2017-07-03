using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Authorization.Common;
using NetSteps.Encore.Core.IoC;

[assembly: Wireup(typeof(NetSteps.Authorization.Service.ModuleWireup))]

namespace NetSteps.Authorization.Service
{
	[WireupDependency(typeof(NetSteps.Authorization.Common.ModuleWireup))]
    public class ModuleWireup : WireupCommand
    {
        protected override void PerformWireup(IWireupCoordinator coordinator)
        {
			Container.Root.ForType<IAuthorizationService>()
				.Register<AuthorizationService>()
				.ResolveAsSingleton()
				.End();
        }
    }
}
