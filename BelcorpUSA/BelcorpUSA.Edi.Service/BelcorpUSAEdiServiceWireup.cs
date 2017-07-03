using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;

[assembly: Wireup(typeof(BelcorpUSA.Edi.Service.BelcorpUSAEdiServiceWireup))]

namespace BelcorpUSA.Edi.Service
{
	public class BelcorpUSAEdiServiceWireup : WireupCommand
	{
		protected override void PerformWireup(IWireupCoordinator coordinator)
		{
			
		}
	}
}
