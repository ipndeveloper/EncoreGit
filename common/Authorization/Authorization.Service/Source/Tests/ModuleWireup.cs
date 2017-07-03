using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.Encore.Core.Wireup;

[assembly: Wireup(typeof(NetSteps.Authorization.Service.Test.ModuleWireup))]

namespace NetSteps.Authorization.Service.Test
{
	[WireupDependency(typeof(NetSteps.Authorization.Service.ModuleWireup))]
	public class ModuleWireup : WireupCommand
	{
		protected override void PerformWireup(IWireupCoordinator coordinator)
		{
		}
	}
}
