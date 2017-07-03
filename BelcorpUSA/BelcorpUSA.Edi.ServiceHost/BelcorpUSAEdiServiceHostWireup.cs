using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;

[assembly: Wireup(typeof(BelcorpUSA.Edi.ServiceHost.BelcorpUSAEdiServiceHostWireup))]

namespace BelcorpUSA.Edi.ServiceHost
{
	public class BelcorpUSAEdiServiceHostWireup : WireupCommand
	{
		protected override void PerformWireup(IWireupCoordinator coordinator)
		{

		}
	}
}