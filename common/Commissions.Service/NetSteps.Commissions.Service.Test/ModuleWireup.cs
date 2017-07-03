using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly:Wireup(typeof(NetSteps.Commissions.Service.Test.ModuleWireup))]

namespace NetSteps.Commissions.Service.Test
{
	[WireupDependency(typeof(NetSteps.Commissions.Service.CommissionsServiceModuleWireup))]
	public class ModuleWireup : WireupCommand
	{
		protected override void PerformWireup(IWireupCoordinator coordinator)
		{
			
		}
	}
}
