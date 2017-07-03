using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Security.Authentication;
using NetSteps.Encore.Core.Wireup.Meta;

[assembly: Wireup(typeof(NetSteps.Security.ModuleWireup))]

namespace NetSteps.Security
{
	public class ModuleWireup : WireupCommand
	{
		protected override void PerformWireup(IWireupCoordinator coordinator)
		{
			coordinator.WireupDependencies(typeof(IAuthorizationProviders).Assembly);	
		}
	}
}
