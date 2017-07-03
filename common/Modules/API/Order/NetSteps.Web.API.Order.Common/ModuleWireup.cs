using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.Encore.Core.IoC;
using System.Web.Routing;
using System.Web.Mvc;

[assembly: Wireup(typeof(NetSteps.Web.API.Order.Common.ModuleWireup))]

namespace NetSteps.Web.API.Order.Common
{
	/// <summary>
	/// Wireup command called at bootstrap time by the wireup coordinator.
	/// </summary>
	[WireupDependency(typeof(NetSteps.Web.API.Base.Common.ModuleWireup))]
	[WireupDependency(typeof(NetSteps.Modules.Order.Common.ModuleWireup))]
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
                "modules_create_order",
                "orders",
                new { controller = "Order", action = "CreateOrder" }
            );

			RouteTable.Routes.MapRoute(
				"modules_orders_load",
				"account/{accountID}/orders/",
				new { controller = "Order", action = "LoadOrders", NumberOfRecords = UrlParameter.Optional, OrderDate = UrlParameter.Optional }
				);

			RouteTable.Routes.MapRoute(
				"modules_orders_move",
				"order/{orderID}/{accountID}",
				new { controller = "Order", action = "MoveOrder" }
				);
        }
    }
}

