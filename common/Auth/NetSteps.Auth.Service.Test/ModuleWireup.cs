using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.Encore.Core.Wireup;

[module: WireupDependency(typeof(NetSteps.Auth.Service.Test.ModuleWireup))]

namespace NetSteps.Auth.Service.Test
{
	[WireupDependency(typeof(NetSteps.Auth.Service.ModuleWireup))]
	public class ModuleWireup : WireupCommand
	{
		protected override void PerformWireup(IWireupCoordinator coordinator)
		{
			
		}
	}
}
