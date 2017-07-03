using System;
using System.Web.Mvc;
using NetSteps.Encore.Core.IoC;
using NetSteps.Web.Mvc;
using NetSteps.Web.Mvc.Restful;
using UPSQuery.Common;

namespace Encore.ApiSite.Areas.UPSQuery.Controllers
{
	public class UPSController : Controller
	{
		public IUPSQueryProcessor UPSQueryProcessorQuery { get { return Create.New<IUPSQueryProcessor>(); } }

		public ActionResult GetOrderShipmentInfo(int orderShipmentID)
		{
			if (orderShipmentID <= 0)
				throw new HttpRequestException(HttpConstants.HttpStatusCodes.UnproccessableEntity, "OrderShipmentID must be greater than 0");

			var results = UPSQueryProcessorQuery.GetUPSOrderShipmentInfo(orderShipmentID);
			if (results != null)
				return Content(results.ToXml(), "text/xml");

			return Content("No order found for Order Number: " + orderShipmentID.ToString());
		}

		public ActionResult SetOrderShipmentTrackingNumber(int orderShipmentID, string trackingNumber)
		{
			try
			{
				if (orderShipmentID <= 0)
					throw new HttpRequestException(HttpConstants.HttpStatusCodes.UnproccessableEntity, "OrderShipmentID must be greater than 0");

				if (string.IsNullOrEmpty(trackingNumber))
					throw new HttpRequestException(HttpConstants.HttpStatusCodes.UnproccessableEntity, "Tracking Number must not be blank");

				UPSQueryProcessorQuery.SetUPSOrderShippingTrackingNumber(orderShipmentID, trackingNumber);
				return this.Result_200_OK();
			}
			catch (Exception ex)
			{
				return Content(ex.Message);
			}
		}
	}
}
