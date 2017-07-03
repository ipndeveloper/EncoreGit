﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.Encore.Core.IoC;
using System.Web.Routing;
using System.Web.Mvc;

[assembly: Wireup(typeof(NetSteps.Web.API.Downline.Common.ModuleWireup))]

namespace NetSteps.Web.API.Downline.Common
{
	/// <summary>
	/// Wireup command called at bootstrap time by the wireup coordinator.
	/// </summary>
	[WireupDependency(typeof(NetSteps.Web.API.Base.Common.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Modules.Downline.Common.ModuleWireup))]
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
				"modules_downline_search",
				"account/{sponsorID}/downline",
				new { controller = "Downline", action = "Search", accountID = UrlParameter.Optional, query = UrlParameter.Optional, AccountTypeID = UrlParameter.Optional }
				);
        }
    }
}
