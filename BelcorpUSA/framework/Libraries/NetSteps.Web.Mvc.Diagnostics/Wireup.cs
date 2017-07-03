using System;
using System.Web.Routing;
using System.Web.Mvc;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;

[assembly: Wireup(typeof(NetSteps.Web.Mvc.Diagnostics.Wireup))]

namespace NetSteps.Web.Mvc.Diagnostics
{
	public class Wireup : WireupCommand
	{
		protected override void PerformWireup(IWireupCoordinator coordinator)
		{
			ControllerBuilder current = ControllerBuilder.Current;
			if (current != null)
			{
				current.DefaultNamespaces.Add("NetSteps.Web.Mvc.Diagnostics.Controllers");
			}
		}
	}
}
