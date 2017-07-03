using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.Auth.Common;
using NetSteps.Encore.Core.Wireup;

[assembly: Wireup(typeof(ModuleWireup))]

namespace NetSteps.Auth.Common
{

	public class ModuleWireup : WireupCommand
	{
		protected override void PerformWireup(IWireupCoordinator coordinator)
		{

		}
	}
}
