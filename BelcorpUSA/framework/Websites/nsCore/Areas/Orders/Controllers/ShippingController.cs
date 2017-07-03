using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Helpers;
using nsCore.Areas.Orders.Models.PDF;
using nsCore.Areas.Orders.Models.Shipping;
using nsCore.Controllers;

namespace nsCore.Areas.Orders.Controllers
{
	using NetSteps.Encore.Core.IoC;
	using NetSteps.Shipping.Common;
	using ServiceUpdatePackageModel = NetSteps.Shipping.Common.Models.UpdatePackageModel;

	public class ShippingController : BaseController
	{
		#region Actions
		public virtual ActionResult Index()
		{
			var model = new IndexModel();

			return View(model);
		}

		[HttpPost]
		public virtual ActionResult GetPackages(OrderShipmentSearchParameters searchParameters)
		{
			try
			{
				var orderShipments = OrderShipment.Search(searchParameters);

				var model = new GetPackagesModel()
					 .LoadResources(
						  orderShipments,
						  Enumerable.Empty<string>(),
						  CoreContext.CurrentCultureInfo
					 );

				return Json(model);
			}
			catch (Exception ex)
			{
				return JsonError(ex.Log().PublicMessage);
			}
		}

		[HttpPost]
		public virtual ActionResult UpdatePackages(IEnumerable<UpdatePackageModel> updatePackageModels)
		{
			// Check for no models.
			if (updatePackageModels == null || !updatePackageModels.Any())
			{
				return JsonError(_errorNoOrdersWereUpdated);
			}

			try
			{
				// Get distinct order IDs and load orders.
				var orderIDs = updatePackageModels
					 .Select(x => x.OrderID)
					 .Distinct();

				var shippingService = Create.New<IShippingService>();
				List<string> errors = shippingService.UpdatePackages(UpdatePackageModelAdapter.GetUpdatePackageModels(updatePackageModels));

				// Return fresh data.
				var updatedOrders = OrderShipment.GetOrderShippingSearchData(orderIDs);
				var model = new PackagesModel()
					 .LoadResources(
						  updatedOrders,
						  errors,
						  CoreContext.CurrentCultureInfo
					 );

				return Json(model);
			}
			catch (Exception ex)
			{
				return JsonError(ex.Log().PublicMessage);
			}
		}

		/// <summary>
		/// Generates PDF Packing slip for the select orders
		/// </summary>
		/// <param name="items">OrderNumbers</param>
		[FunctionFilter("Orders", "~/Accounts")]
		public virtual ActionResult PrintPackingSlip(List<string> items)
		{
			if (items != null && items.Count > 0)
			{
				// Save current status in case of failure generating slips
				var orderInfo = items.Select(i => GetOrder(i)).ToDictionary(o => o.OrderNumber, o => o.OrderStatusID);
				try
				{
					// Change status to printed first because that's what needs to show on pdf.
					ChangeOrderStatusToPrinted(items);

					var orders = OrderShipment.GetOrderShippingDetails(items).ToList();

					Stream fileStream = PdfGenerator(orders);

					SetHttpContextHeader();

					return File(fileStream, "application/pdf");
				}
				catch
				{
					// Revert order statuses
					foreach (var kvp in orderInfo)
					{
						var o = GetOrder(kvp.Key);
						if (o.OrderStatusID != kvp.Value)
						{
							o.OrderStatusID = kvp.Value;
							o.Save();
						}
					}
				}
			}
			return RedirectToAction("Index", "Browse");
		}

		#endregion

		#region Helpers

		[NonAction]
		public virtual Stream PdfGenerator(List<Order> orders)
		{
			var slip = GetPackSlip();

			return slip.GeneratePackingSlipPdf(orders);
		}

		[NonAction]
		public virtual void SetHttpContextHeader()
		{
			HttpContext.Response.AddHeader("content-disposition", "attachment; filename=PackingSlip.pdf");
		}


		/// <summary>
		/// Custom concrete class per client that extends BasePdfMaker
		/// NOTE: For each client override this with your own implementation.
		/// </summary>
		/// <returns></returns>
		[NonAction]
		public virtual IBasePdfMaker GetPackSlip()
		{
			return new PackingSlip();
		}


		[NonAction]
		public virtual void ChangeOrderStatusToPrinted(List<string> orderNumbers)
		{
			foreach (var number in orderNumbers)
			{
				var order = GetOrder(number);

				if (order.OrderStatusID != (short)Constants.OrderStatus.Printed)
				{
					order.OrderStatusID = (short)Constants.OrderStatus.Printed;
					order.Save();
				}
			}
		}

		[NonAction]
		public virtual Order GetOrder(string orderNumber)
		{
			return Order.LoadByOrderNumber(orderNumber);
		}
		#endregion

		#region Strings
		protected virtual string _errorNoOrdersWereUpdated { get { return Translation.GetTerm("ErrorNoOrdersWereUpdated", "No orders were updated."); } }
		protected virtual string _errorUpdatingOrder(string orderNumber, string message) { return Translation.GetTerm("ErrorUpdatingOrder", "There was an error updating order {0}: {1}", orderNumber, message); }
		protected virtual string _errorRetrievingOrder(int orderID) { return Translation.GetTerm("ErrorRetrievingOrder", "There was an error retrieving OrderID {0}.", orderID); }
		protected virtual string _errorRetrievingOrderShipment(string orderNumber) { return Translation.GetTerm("ErrorRetrievingOrderShipment", "There was an error retrieving the shipment for order {0}.", orderNumber); }
		protected virtual string _errorRetrievingOrderShipmentPackage(string orderNumber) { return Translation.GetTerm("ErrorRetrievingOrderShipmentPackage", "There was an error retrieving the package info for order {0}.", orderNumber); }
		protected virtual string _errorOrderIsNotPaidInFull(string orderNumber) { return Translation.GetTerm("ErrorOrderIsNotPaidInFull", "Order {0} is not paid in full.", orderNumber); }
		#endregion


		private class UpdatePackageModelAdapter
		{
			internal static IEnumerable<ServiceUpdatePackageModel> GetUpdatePackageModels(IEnumerable<UpdatePackageModel> updatePackageModels)
			{
				return
					updatePackageModels.Select(
						model =>
						new ServiceUpdatePackageModel
							{
								DateShipped = model.DateShipped,
								OrderID = model.OrderID,
								OrderShipmentID = model.OrderShipmentID,
								OrderShipmentPackageID = model.OrderShipmentPackageID,
								TrackingNumber = model.TrackingNumber
							});
			}
		}
	}
}
