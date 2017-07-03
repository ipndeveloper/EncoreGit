using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.Encore.Core.Wireup;

// Indicates this assembly is dependent upon its own wireup command:
[assembly: Wireup(typeof(NetSteps.Data.Common.ModuleWireup))]

namespace Data.Common.Tests
{
	[WireupDependency(typeof(NetSteps.Data.Common.ModuleWireup))]
	public class ModuleWireup : WireupCommand
	{
		protected override void PerformWireup(IWireupCoordinator coordinator)
		{
			
		}
	}
}
