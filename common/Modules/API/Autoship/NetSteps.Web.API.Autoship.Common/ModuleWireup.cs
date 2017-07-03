using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.Encore.Core.IoC;
using System.Web.Routing;
using System.Web.Mvc;

[assembly: Wireup(typeof(NetSteps.Web.API.Autoship.Common.ModuleWireup))]
namespace NetSteps.Web.API.Autoship.Common
{
	/// <summary>
	/// Wireup command called at bootstrap time by the wireup coordinator.
	/// </summary>
	[WireupDependency(typeof(NetSteps.Web.API.Base.Common.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Modules.Autoship.Common.ModuleWireup))]
	public class ModuleWireup : WireupCommand 
	{
		/// <summary>
		/// Wires this module.
		/// </summary>
		/// <param name="coordinator">the coordinator</param>
		/// <seealso cref="IWireupCoordinator"/>
		protected override void PerformWireup(IWireupCoordinator coordinator)
		{
			RouteTable.Routes.MapRoute(
				"modules_autoships_search",
				"account/{accountID}/autoships/{autoshipScheduleID}",
				new { controller = "Autoship", action = "Search", autoshipScheduleID = UrlParameter.Optional, ActiveAutoshipsOnly = UrlParameter.Optional }
				);

			RouteTable.Routes.MapRoute(
				"modules_autoships_cancel",
				"account/{accountID}/autoship/{autoshipID}",
				new { controller = "Autoship", action = "Cancel" }
				);
		}
	}
}
