using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.Encore.Core.Wireup;

[assembly: Wireup(typeof(NetSteps.Common.Tests.ModuleWireup))]

namespace NetSteps.Common.Tests
{
	[WireupDependency(typeof(NetSteps.Common.ModuleWireup))]
	public class ModuleWireup : WireupCommand
	{
		protected override void PerformWireup(IWireupCoordinator coordinator)
		{
		}
	}
}
