using System.Web.Mvc;
using System.Web.Routing;

namespace Encore.ApiSite.Areas.UPSQuery
{
	public class UPSQueryAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get
			{
				return "UPSQuery";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.MapRoute(
                "upsquery_getshipmentinfo",
				"ups/{orderShipmentID}",
				new { action = "GetOrderShipmentInfo", controller = "UPS", id = UrlParameter.Optional },
                new { httpMethod = new HttpMethodConstraint("GET") }
            );

			context.MapRoute(
				"upsquery_updatetrackingnum",
				"UPS/SetTrackingNum",
				new { action = "SetOrderShipmentTrackingNumber", controller = "UPS", orderShipmentID = UrlParameter.Optional, trackingNumber = UrlParameter.Optional },
				new { httpMethod = new HttpMethodConstraint("POST") }
				);
		}
	}
}
