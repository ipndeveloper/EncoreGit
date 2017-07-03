using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common.Configuration;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Common.Models;
using NetSteps.Data.Common.Services;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Interfaces;
using NetSteps.Encore.Core.IoC;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Controls.Models;
using NetSteps.Web.Mvc.Helpers;

namespace nsCore.Areas.Orders.Controllers
{
	public class ReplacementController : OrdersBaseController
	{
		public ActionResult Index(int baseOrderID, int accountID)
		{
			CoreContext.CurrentAccount = Account.LoadForSession(accountID);
			Order order = Order.LoadFull(baseOrderID);
			Order replacementOrder = CreateReplacementOrder(order);
			OrderContext.Order = replacementOrder;

			// Try to get the shipping methods for the default address
			Address defaultShippingAddress = CoreContext.CurrentAccount.Addresses.GetDefaultByTypeID(Constants.AddressType.Shipping);
			ViewData["ShippingMethods"] = defaultShippingAddress != null ? GetNewShippingMethods(defaultShippingAddress.AddressID) : new List<ShippingMethodWithRate>();

			OrderEntryModel orderModel = CreateOrderEntryModel(replacementOrder);

			return View(orderModel);
		}

		public ActionResult Edit(string orderNumber)
		{
			try
			{
				Order replacementOrder = null;
				if (!string.IsNullOrEmpty(orderNumber))
				{
					replacementOrder = Order.LoadFull(Convert.ToInt32(orderNumber));
					if (replacementOrder.ParentOrderID.HasValue)
						replacementOrder.ParentOrder = Order.LoadFull(replacementOrder.ParentOrderID.Value);
				}

				if (replacementOrder == null || replacementOrder.ParentOrder == null)
					return RedirectToAction("Index");

				if (replacementOrder.OrderStatusID == Constants.OrderStatus.Paid.ToInt())
				{
					TempData["Error"] = Translation.GetTerm("ReturnOrderCannotBeEditedAfterItHasEnteredSubmittedStatus", "Return order cannot be edited after it has entered Submitted status.");
					return RedirectToAction("Index", "Details", new { id = replacementOrder.OrderID });
				}

				OrderContext.Order = replacementOrder;

				OrderCustomer customer = (replacementOrder.OrderCustomers != null && replacementOrder.OrderCustomers.Count > 0) ? replacementOrder.OrderCustomers[0] : null;

				Account account = null;
				if (customer != null && customer.AccountID != 0)
				{
					account = Account.LoadForSession(customer.AccountID);
					CoreContext.CurrentAccount = account.Clone();
					CoreContext.CurrentAccount.StartEntityTracking();
				}

				Address defaultShippingAddress = null;
				if (account != null)
				{
					defaultShippingAddress = CoreContext.CurrentAccount.Addresses.GetDefaultByTypeID(Constants.AddressType.Shipping);
				}

				ViewData["ShippingMethods"] = defaultShippingAddress != null ? GetNewShippingMethods(defaultShippingAddress.AddressID) : new List<ShippingMethodWithRate>();

				return View("Index", CreateOrderEntryModel((Order)OrderContext.Order));
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				throw exception;
			}
		}

		[OutputCache(CacheProfile = "DontCache")]
		public virtual ActionResult GetOverrides()
		{
			try
			{
				StringBuilder html = new StringBuilder();
				foreach (OrderItem item in ((Order)OrderContext.Order).OrderCustomers[0].ParentOrderItems)
				{
					var product = Inventory.GetProduct(item.ProductID.ToInt());
					var currency = SmallCollectionCache.Instance.Currencies.GetById(OrderContext.Order.CurrencyID);
					var guidString = item.Guid.ToString("N");
					html.Append("<tr id=\"").Append(guidString).Append("\">")
						.AppendCell(product.SKU)
						.AppendCell(product.Name)
						.AppendCell(currency.CurrencySymbol + "<input type=\"text\" class=\"TextInput required price\" style=\"width: 4.167em;\" id=\"overridePrices" + guidString + "\" name=\"overridePrices\" value=\"" + "0" + "\" >")
						.AppendCell(item.Quantity.ToString())
						.AppendCell(currency.CurrencySymbol + "<input type=\"text\" class=\"TextInput required quantity\" style=\"width: 4.167em;\" id=\"cvAmount" + guidString + "\" name=\"overrideCVAmount\" value=\"" + "0" + "\" >")
						.AppendCell((item.GetAdjustedPrice() * item.Quantity).ToString(currency))
						.Append("</tr>");
				}

				return Json(new { products = html.ToString(), totals = Totals });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, (OrderContext.Order != null) ? OrderContext.Order.OrderID.ToIntNullable() : null, accountID: (CoreContext.CurrentAccount != null) ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		public ActionResult UpdateOrder(int[] OrderItemIDArray, string[] QuantityArray, int[] OriginalQuantityArray, int[] ReplacementReasons, string[] Notes)
		{
			try
			{
				if (OrderItemIDArray == null && QuantityArray == null && OriginalQuantityArray == null && ReplacementReasons == null && Notes == null)
				{
					// If they're all null then the user has selected nothing, and empty arrays are passed to MVC.
					OrderItemIDArray = new int[0];
					QuantityArray = new string[0];
					OriginalQuantityArray = new int[0];
					ReplacementReasons = new int[0];
					Notes = new string[0];
				}

				if (OrderItemIDArray == null || QuantityArray == null || OriginalQuantityArray == null)
					return Json(new { result = false, message = Translation.GetTerm("ErrorOccurredWhenReadingOrderItems", "Error occurred when reading order items") });

				//Make sure there is a corresponding quantity for each orderitem
				if (OrderItemIDArray.Length != QuantityArray.Length)
					return Json(new { result = false, message = Translation.GetTerm("ErrorOccurredWhenReadingOrderItems", "Error occurred when reading order items") });

				//Make sure all items in QuantityArray are numeric
				if (QuantityArray.Any(q => !q.IsNumberOnly(false) && OrderItemIDArray.Length > 0 && OrderItemIDArray[0] != 0))
					return Json(new { result = false, message = Translation.GetTerm("PleaseFillOutQuantityForEachCheckedItem", "Please fill out quantity for each checked item") });

				List<ReplacementReason> reasons = ReplacementReason.GetAllReasons();
				if (reasons.Count == 0)
					return Json(new { result = false, message = Translation.GetTerm("ThereAreNoReplacementReasons", "There are no replacement reasons") });

				//Get and clear the order
				OrderContext.Order.AsOrder().RemoveAllOrderItems();

				if (OrderItemIDArray.Length > 0)
				{
				//Iterate through the passed in order items, creating OrderItemUpdateInfo objects for each one
				List<OrderItemUpdateInfo> orderItems = new List<OrderItemUpdateInfo>();
				for (int i = 0; i < OrderItemIDArray.Length; i++)
				{
					if (OrderItemIDArray[0] == 0 || QuantityArray[0] == "0" || !QuantityArray[i].IsNumberOnly(false))
						continue;   //Skip any blank products

						OrderItemUpdateInfo updateItem = CreateOrderItemUpdateInfo(
							OrderItemIDArray[i], OriginalQuantityArray[i], QuantityArray[i]);
					orderItems.Add(updateItem);
				}

				//Add our new list of items to the order
				OrderContext.Order.AsOrder().AddOrUpdateOrderItem((OrderCustomer)OrderContext.Order.OrderCustomers[0], orderItems, false);

				//Create entries linking replacement reasons to the order
				CreateOrderItemReplacements(OrderItemIDArray, ReplacementReasons, Notes, OrderContext.Order.AsOrder());
				}

				//Saving ShippingMethods text before setting the overrides, because the overrides affect the text of ShippingMethods
				string shippingMethods = ShippingMethods;
				SetOrderOverridesAndTotalsToZero(OrderContext.Order.AsOrder());

				OrderService.UpdateOrder(OrderContext);

				return Json(new
				{
					result = true,
					shippingMethods = shippingMethods,
					returnItemsHtml = GetReplacementTableHtml(OrderContext.Order.AsOrder(), reasons),
					totals = Totals,
					orderItems = GetOrderItemsHtml(OrderContext.Order.AsOrder())
				});
			}
			catch (Exception ex)
			{
				return Json(new { result = false, message = ex.Message });
			}
		}

		public ActionResult InitialReplacementGrid()
		{
			try
			{
				IEnumerable<ReplacementReason> reasons = ReplacementReason.GetAllReasons();

				return Json(new
				{
					result = true,
					shippingMethods = ShippingMethods,
					returnItemsHtml = GetReplacementTableHtml(OrderContext.Order.AsOrder(), reasons),
					totals = Totals,
					orderItems = GetOrderItemsHtml(OrderContext.Order.AsOrder())
				});
			}
			catch (Exception)
			{
				return Json(new { result = false, message = Translation.GetTerm("ErrorOccurredWhenReadingOrderItems", "Error occurred when reading order items") });
			}
		}

		[OutputCache(CacheProfile = "DontCache")]
		public override ActionResult GetAddresses()
		{
			try
			{
				StringBuilder addresses = new StringBuilder();
				StringBuilder options = new StringBuilder();

				foreach (Address address in CoreContext.CurrentAccount.Addresses.Where(a => a.AddressTypeID == Constants.AddressType.Shipping.ToInt()).OrderByDescending(a => a.IsDefault).ThenBy(a => a.AddressID))
				{
					addresses.Append("<div id=\"shippingAddress").Append(address.AddressID).Append("\" class=\"shippingAddressDisplay\">")
						.Append("<b>").Append(address.ProfileName).Append("</b> - ")
						.Append("<a title=\"Edit\" style=\"cursor: pointer;\" onclick=\"editAddress(").Append(address.AddressID).Append(");\">" + Translation.GetTerm("Edit") + "</a>")
						.Append("<br />")
						.Append(address.ToString().ToHtmlBreaks())
						.Append("</div>");

					options.Append("<option value=\"").Append(address.AddressID).Append("\">").Append(address.ProfileName);
					if (address.IsDefault)
					{
						options.Append(" (" + Translation.GetTerm("default") + ")");
					}
					options.Append("</option>");
				}

				return Json(new { options = options.ToString(), addresses = addresses.ToString() });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, orderID: (OrderContext.Order != null) ? OrderContext.Order.OrderID.ToIntNullable() : null, accountID: (CoreContext.CurrentAccount != null) ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		#region Order Creation

		private OrderEntryModel CreateOrderEntryModel(Order replacementOrder)
		{
			OrderEntryModel orderModel = new OrderEntryModel(replacementOrder);
			orderModel.BulkAddModal = false;
			orderModel.AddProduct = false;
			orderModel.ReplacementTables = true;
			orderModel.MarkAsAutoship = false;
			orderModel.GetOverridesLocation = "/Orders/Replacement/GetOverrides";

			return orderModel;
		}

		private Order CreateReplacementOrder(Order parentOrder)
		{
			Order replacementOrder = new Order(CoreContext.CurrentAccount) { OrderTypeID = (int)Constants.OrderType.ReplacementOrder };

			replacementOrder.OrderPendingState = Constants.OrderPendingStates.Quote;
			replacementOrder.SiteID = ConfigurationManager.GetAppSetting<int>(ConfigurationManager.VariableKey.NSCoreSiteID);
			replacementOrder.DateCreated = DateTime.Now;
			replacementOrder.CurrencyID = CoreContext.CurrentMarket.GetDefaultCurrencyID();
			replacementOrder.ParentOrderID = parentOrder.OrderID;
			replacementOrder.ParentOrder = parentOrder;
			parentOrder.ChildOrders.Add(replacementOrder);
			parentOrder.Save();
			replacementOrder.Save();

			return replacementOrder;
		}

		protected virtual List<OrderItemUpdateInfo> GetOrderItemUpdateInfoFromOrder(Order order)
		{
			List<OrderItemUpdateInfo> returnValue = new List<OrderItemUpdateInfo>();
			foreach (OrderCustomer customer in order.OrderCustomers)
			{
				foreach (OrderItem item in customer.OrderItems)
				{
					returnValue.Add(new OrderItemUpdateInfo()
					{
						Quantity = item.Quantity,
						ProductID = item.ProductID ?? 0
					});
				}
			}

			return returnValue;
		}

		protected virtual void SetOrderOverridesAndTotalsToZero(Order order)
		{
			List<OrderItemOverride> overrides = new List<OrderItemOverride>();
			foreach (OrderCustomer customer in order.OrderCustomers)
			{
				foreach (OrderItem item in customer.OrderItems)
				{
					overrides.Add(new OrderItemOverride()
					{
						CommissionableValue = 0,
						OrderItemGuid = item.Guid.ToString("N"),
						PricePerItem = 0,
						OrderItemID = 0
					});
				}
			}

			order.SetReplacementOrderPrices(overrides, 0, 0);
		}

		private void CreateOrderItemReplacements(int[] OrderItemIDArray, int[] ReplacementReasons, string[] Notes, Order order)
		{
			foreach (OrderCustomer customer in order.OrderCustomers)
			{
				foreach (OrderItem item in customer.OrderItems)
				{
					//int index = GetIntIndexInIntArray(OrderItemIDArray, item.ProductID ?? 0);
					int index = OrderItemIDArray.IndexOf(o => o == item.ProductID);
					if (index == -1)
						throw new Exception(string.Format("{0} ID: {1} did not exist in OrderItemIDArray", item.ProductName, item.ProductID));
					OrderItem parentItem = order.ParentOrder.OrderCustomers.First(oc => oc.OrderItems.Any(oi => oi.ProductID == item.ProductID)).OrderItems.FirstOrDefault(oi => oi.ProductID == item.ProductID);
					item.OrderItemReplacement = new OrderItemReplacement()
					{
						OrderItemID = parentItem.OrderItemID,
						ReplacementReasonID = ReplacementReasons[index].ToShort(),
						Notes = Notes[index]
					};
				}
			}
		}

		#region protected virtual OrderItemUpdateInfo CreateOrderItemUpdateInfo

		protected virtual OrderItemUpdateInfo CreateOrderItemUpdateInfo(int orderItemID, int originalQuantity, string quantity)
		{
			if (!quantity.IsNumberOnly(false))
				throw new Exception("Quantity needs to be a numeric string");

			return CreateOrderItemUpdateInfo(orderItemID, originalQuantity, Convert.ToInt32(quantity));
		}

		protected virtual OrderItemUpdateInfo CreateOrderItemUpdateInfo(int orderItemID, int originalQuantity, int quantity)
		{
			if (quantity > originalQuantity)
				quantity = originalQuantity;

			return new OrderItemUpdateInfo()
			{
				ProductID = orderItemID,
				Quantity = quantity
			};
		}

		#endregion

		#endregion

		#region Replacement Table Html
		protected virtual string GetReplacementTableHtml(Order order, IEnumerable<ReplacementReason> reasons)
		{
			StringBuilder builder = new StringBuilder();
			builder.AppendLine(CreateHtmlForProductHeader());
			foreach (OrderCustomer customer in order.OrderCustomers)
			{
				foreach (OrderItem item in customer.OrderItems.Where(oi => !oi.HasChildOrderItems))
				{
					builder.AppendLine(GetHtmlForProduct(item, reasons));
				}
			}
			return builder.ToString();
		}

		protected virtual string GetHtmlForProduct(OrderItem orderItem, IEnumerable<ReplacementReason> reasons)
		{
			ReplacementReason reason = reasons.First(r => r.ReplacementReasonID == orderItem.OrderItemReplacement.ReplacementReasonID);

			//Product product = Product.LoadFull(orderItem.ProductID ?? 0);
			Product product = Inventory.GetProduct(orderItem.ProductID.ToInt());
			return string.Format("<tr><td></td><td>{0}</td><td>{1}</td><td>{2:0.00}</td><td>{3}</td><td>{4}</td><td>{5}</td></tr>",
				product.SKU,
				product.Name,
				product.Prices.First(p => p.ProductPriceTypeID == CoreContext.CurrentAccount.ProductPriceTypeID).Price,
				orderItem.Quantity,
				Translation.GetTerm(reason.TermName, reason.Name),
				orderItem.OrderItemReplacement.Notes);
		}

		protected virtual string CreateHtmlForProductHeader()
		{
			return string.Format("<tr class=\"GridColHead\"><th class=\"GridCheckBox\"></th><th>{0}</th><th>{1}</th><th>{2}</th><th style=\"width: 9.091em;\">{3}</th><th>{4}</th><th>{5}</th></tr>",
				Translation.GetTerm("SKU", "SKU"),
				Translation.GetTerm("Product", "Product"),
				Translation.GetTerm("PricePerItem", "Price Per Item"),
				Translation.GetTerm("QuantityToReturn", "Quantity to Return"),
				Translation.GetTerm("ReplacementReason", "Replacement Reason"),
				Translation.GetTerm("Notes", "Notes"));
		}
		#endregion

	}
}