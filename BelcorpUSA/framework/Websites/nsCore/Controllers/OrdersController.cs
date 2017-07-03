using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using NetSteps.Common.Configuration;
using NetSteps.Common.Extensions;
using NetSteps.Common.Reflection;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Business.Interfaces;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Commissions;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Helpers;
using nsCore5.Models;

namespace nsCore.Controllers
{
	public class OrdersController : BaseController
	{
		[FunctionFilter("Orders", "~/Accounts")]
		public ActionResult Index()
		{
			CoreContext.CurrentOrder = null;
			CoreContext.CurrentAccount = null;
			return View("Index");
		}

		[FunctionFilter("Orders", "~/Accounts")]
		public ActionResult Find(string orderNumber)
		{
			List<string> ordersNumberList = new List<string>();
			var orders = Order.SearchOrders(new NetSteps.Data.Entities.Business.OrderSearchParameters()
											{
												OrderNumber = orderNumber
											});

			if (orders.TotalCount == 1)
			{
				RouteValueDictionary routeVals = new RouteValueDictionary();
				routeVals.Add("id", orderNumber.ToInt());
				return RedirectToAction("Details", routeVals);
			}
			else
			{
				return Redirect(string.Format("~/Orders/OrderSearch?orderNumber={0}", orderNumber));
			}
		}

		[FunctionFilter("Orders", "~/Accounts")]
		public ActionResult OrderSearch(string orderNumber)
		{
			var orders = Order.SearchOrders(new NetSteps.Data.Entities.Business.OrderSearchParameters()
											{
												PageIndex = 0,
												PageSize = 20,
												OrderNumber = orderNumber,
												OrderByDirection = NetSteps.Common.Constants.SortDirection.Ascending
											});

			if (orders.TotalCount == 1)
				return RedirectToAction("Details", new { id = orders[0].OrderID });
			return View(orders);
		}

		public ActionResult GetOrders(int page, int pageSize, int? status, int? type, DateTime? startDate, DateTime? endDate, string accountNumberOrName, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection, string orderNumber)
		{
			if (startDate.HasValue && startDate.Value.Year < 1900)
				startDate = null;
			if (endDate.HasValue && endDate.Value.Year < 1900)
				endDate = null;
			StringBuilder builder = new StringBuilder();
			var orders = Order.SearchOrders(new NetSteps.Data.Entities.Business.OrderSearchParameters()
											{
												OrderStatusID = status,
												OrderTypeID = type,
												StartDate = startDate,
												EndDate = endDate,
												AccountNumberOrName = accountNumberOrName,
												OrderNumber = orderNumber,
												PageIndex = page,
												PageSize = pageSize,
												OrderBy = orderBy,
												OrderByDirection = orderByDirection
											});
			if (orders.Count > 0)
			{
				int count = 0;
				foreach (var order in orders)
				{
					builder.Append("<tr class=\"").Append(count % 2 == 0 ? "GridRow" : "GridRowAlt").Append("\">")
						.AppendLinkCell("~/Orders/Details/" + order.OrderID, order.OrderNumber)
						.AppendLinkCell("~/Accounts/Overview/Index" + order.AccountNumber, order.FirstName)
						.AppendLinkCell("~/Accounts/Overview/Index" + order.AccountNumber, order.LastName)
						.AppendCell(order.OrderStatus)
						.AppendCell(order.OrderType)
						.AppendCell(order.CompleteDate.ToDateTime().Year <= 1900 ? "N/A" : order.CompleteDate.ToShortDateString())
						.AppendCell(order.DateShipped.ToDateTime().Year <= 1900 ? "N/A" : order.DateShipped.ToShortDateString())
						.AppendCell(order.SubTotal.ToString("C"), style: "text-align: right; padding-right: 10px;")
						.AppendCell(order.GrandTotal.ToString("C"), style: "text-align: right; padding-right: 10px;")
						.AppendLinkCell("~/Accounts/Overview/Index" + order.SponsorAccountNumber, order.Sponsor)
						.Append("</tr>");
					++count;
				}
				return Json(new { result = true, resultCount = orders.TotalCount, orders = builder.ToString() });
			}
			else
				return Json(new { result = true, resultCount = 0, orders = "<tr><td colspan=\"9\">There were no records found that meet that criteria.  Please try again.</td></tr>" });
		}

		public ActionResult AdvancedSearchData(int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection, string column, string query, DateTime? startDate, DateTime? endDate)
		{
			if (startDate.HasValue && startDate.Value.Year < 1900)
				startDate = null;
			if (endDate.HasValue && endDate.Value.Year < 1900)
				endDate = null;
			StringBuilder builder = new StringBuilder();
			if (!string.IsNullOrEmpty(column) && !string.IsNullOrEmpty(query))
			{
				var orders = Order.SearchOrders(new NetSteps.Data.Entities.Business.OrderSearchParameters()
											{
												StartDate = startDate,
												EndDate = endDate,
												AccountNumberOrName = query,
												PageIndex = page,
												PageSize = pageSize,
												OrderBy = orderBy,
												OrderByDirection = orderByDirection
											});
				if (orders.Count > 0)
				{
					int count = 0;
					foreach (var order in orders)
					{
						builder.Append("<tr class=\"").Append(count % 2 == 0 ? "GridRow" : "GridRowAlt").Append("\">")
							.AppendLinkCell("~/Orders/Details/" + order.OrderID, order.OrderNumber)
							.AppendLinkCell("~/Accounts/Overview/Index" + order.AccountNumber, order.FirstName)
							.AppendLinkCell("~/Accounts/Overview/Index" + order.AccountNumber, order.LastName)
							.AppendCell(order.OrderStatus)
							.AppendCell(order.OrderType)
							.AppendCell(order.CompleteDate.ToDateTime().Year <= 1900 ? "N/A" : order.CompleteDate.ToShortDateString())
							.AppendCell(order.DateShipped.ToDateTime().Year <= 1900 ? "N/A" : order.DateShipped.ToShortDateString())
							.AppendCell(order.SubTotal.ToString("C"), style: "text-align: right; padding-right: 10px;")
							.AppendCell(order.GrandTotal.ToString("C"), style: "text-align: right; padding-right: 10px;")
							.AppendLinkCell("~/Accounts/Overview/Index" + order.SponsorAccountNumber, order.Sponsor)
							.Append("</tr>");
						++count;
					}
					return Json(new { result = true, resultCount = orders.TotalCount, orders = builder.ToString() });
				}
			}

			return Json(new { resultCount = 0, orders = "<tr><td colspan=\"9\">There were no records found that meet that criteria.  Please try again.</td></tr>" }); // no data to return
		}

		#region Helpers
		private object Totals
		{
			get
			{
				Order order = CoreContext.CurrentOrder;
				if (order == null)
					return null;
				decimal paymentTotal = order.OrderCustomers[0].OrderPayments.Sum(p => p.Amount);
				return new
				{
					subtotal = order.Subtotal.ToDecimal().ToString("C"),
					taxTotal = order.TaxAmountTotal.ToDecimal().ToString("C"),
					shippingTotal = order.ShippingTotal.ToDecimal().ToString("C"),
					grandTotal = order.GrandTotal.ToDecimal().ToString("C"),
					paymentTotal = paymentTotal.ToString("C"),
					balanceDue = (order.GrandTotal - paymentTotal).ToDecimal().ToString("C"),
					balanceAmount = order.GrandTotal - paymentTotal
				};
			}
		}

		#region Shipping Methods
		private string ShippingMethods
		{
			get
			{
				Order order = CoreContext.CurrentOrder;

				OrderShipment shipment = order.GetDefaultShipment();
				OrderCustomer customer = order.OrderCustomers[0];

				if (shipment != null && customer != null)
				{
					StringBuilder builder = new StringBuilder();

					order.CalculateTotals();
					var shippingMethods = Calculations.ShippingCalculator.GetShippingMethodsWithRates(customer, shipment).OrderBy(sm => sm.ShippingAmount);
					if (!shippingMethods.Select(sm => sm.ShippingMethodID).Contains(shipment.ShippingMethodID.ToInt()) && shippingMethods.Count() > 0)
					{
						var cheapestShippingMethod = shippingMethods.First();

						shipment.ShippingMethodID = cheapestShippingMethod.ShippingMethodID;
						//shipment.ShippingMethodName = cheapestShippingMethod.Name;
						shipment.Save();

					}
					order.CalculateTotals();

					foreach (var shippingMethod in shippingMethods)
					{
						builder.Append("<div class=\"FL AddressProfile\"><input value=\"").Append(shippingMethod.ShippingMethodID).Append(shipment.ShippingMethodID == shippingMethod.ShippingMethodID ? "\" checked=\"checked" : "").Append("\" type=\"radio\" name=\"shippingMethod\" class=\"Radio\" /><b>")
							.Append(shippingMethod.Name).Append("</b><br />").Append(shippingMethod.ShippingAmount.ToString("C")).Append("</div>");
					}

					return builder.ToString();
				}

				order.CalculateTotals();
				return null;
			}
		}

		private void UpdateOrderShipmentAddress(OrderShipment shipment, int addressId)
		{
			Address address = CoreContext.CurrentAccount.Addresses.GetByAddressID(addressId);

			if (shipment == null)
			{
				// Create new shipment
				shipment = new OrderShipment();
				shipment.OrderShipmentStatusID = (int)Constants.OrderShipmentStatus.Pending;
				CoreContext.CurrentOrder.OrderShipments.Add(shipment);
			}

			shipment.Name = address.Name;
			shipment.FirstName = address.FirstName; // Regex.Replace(address.Attention, @"(\S*)\s", "$1"); //customer.FirstName;
			shipment.LastName = address.LastName; // Regex.Replace(address.Attention, @"\s*\s(\S*)", "$1"); //customer.LastName;
			shipment.Attention = address.Attention;
			shipment.Address1 = address.Address1;
			shipment.Address2 = address.Address2;
			shipment.Address3 = address.Address3;
			shipment.City = address.City;
			shipment.County = address.County;
			shipment.State = address.State;
			shipment.PostalCode = address.PostalCode;
			shipment.CountryID = address.CountryID;
			shipment.DayPhone = address.PhoneNumber;
			shipment.IsWillCall = address.IsWillCall;

			//if (shipment.OrderShipmentID > 0)
			//    shipment.Save();
		}

		private IEnumerable<ShippingMethodWithRate> GetNewShippingMethods(int addressId)
		{
			Order order = CoreContext.CurrentOrder;

			OrderShipment shipment = order.GetDefaultShipment();
			OrderCustomer customer = order.OrderCustomers[0];

			UpdateOrderShipmentAddress(shipment, addressId);

			order.CalculateTotals();

			var shippingMethods = Calculations.ShippingCalculator.GetShippingMethodsWithRates(customer, shipment).OrderBy(sm => sm.ShippingAmount).ToList();

			if (shipment.OrderShipmentID == 0 || !shippingMethods.Select(sm => sm.ShippingMethodID).Contains(shipment.ShippingMethodID.ToInt()))
			{
				// Set default shipping method
				if (shippingMethods.Count() > 0)
				{
					var cheapestShippingMethod = shippingMethods.OrderBy(sm => sm.ShippingAmount).First();

					shipment.ShippingMethodID = cheapestShippingMethod.ShippingMethodID;
					shipment.Name = cheapestShippingMethod.Name;
				}
				CoreContext.CurrentOrder.Save();
			}

			return shippingMethods;
		}
		#endregion

		private List<int> ReconcilePayments()
		{
			//TODO: remove payments until the payments are less than the balance due and return the ids of the payments removed
			Order order = CoreContext.CurrentOrder;

			if (order.Balance < order.OrderCustomers[0].OrderPayments.Sum(p => p.Amount))
			{
			}

			return null;
		}

		private void RemoveAllPayments()
		{
			// remove payments
			foreach (int paymentId in CoreContext.CurrentOrder.OrderCustomers[0].OrderPayments.Select(x => x.OrderPaymentID).ToList())
			{
				RemovePayment(paymentId);
			}
		}
		#endregion

		#region Order Entry
		[FunctionFilter("Orders-Order Entry", "~/Orders")]
		public ActionResult NewOrder()
		{
			return View();
		}

		[FunctionFilter("Orders-Order Entry", "~/Orders")]
		public ActionResult OrderEntry(int? accountId)
		{
			Account account = null;
			if (accountId.HasValue)
				account = CoreContext.CurrentAccount = Account.Load(accountId.Value);
			else if (CoreContext.CurrentAccount != null)
				account = CoreContext.CurrentAccount;

			if (account == null)
				return RedirectToAction("NewOrder");

			// Load the Customer's consultant
			Account consultant = null;
			if (account.SponsorID < 1)
				consultant = Account.Load(ConfigurationManager.GetAppSetting<int>(ConfigurationManager.VariableKey.CorporateAccountID));
			else
				consultant = Account.Load(account.SponsorID.ToInt());
			//consultant.LoadAccountInfo();


			// Create new order
			Order order = new Order(consultant);
			order.ConsultantID = account.AccountID;
			order.Consultant = account;
			//order.OrderTypeID = Constants.OrderType.NetSteps.Common.CustomConfigurationHandler.Config.IDs.StandardOrderTypeID; // Set in Business Logic called from constructor. - JHE
			//order.OrderStatusID = (int)Constants.OrderStatus.Pending; // Set in Business Logic called from constructor. - JHE
			order.SiteID = ConfigurationManager.GetAppSetting<int>(ConfigurationManager.VariableKey.NSCoreSiteID);
			order.DateCreated = DateTime.Now;
			order.Save(); // TODO: Why Save twice (here and a few lines down) - JHE

			// Add customer to order
			order.AddNewCustomer(account.AccountID);
			order.Save();

			CoreContext.CurrentOrder = order;

			// Try to get the shipping methods for the default address
			Address defaultShippingAddress = CoreContext.CurrentAccount.Addresses.GetDefaultByTypeID(Constants.AddressType.Shipping);
			ViewData["ShippingMethods"] = defaultShippingAddress != null ? GetNewShippingMethods(defaultShippingAddress.AddressID) : new List<ShippingMethodWithRate>();

			ViewData["Catalogs"] = Inventory.Catalogs;

			return View(order);
		}

		#region Product Listing
		public void LoadProducts()
		{
			if (!Inventory.InventoryLoaded)
			{
				Inventory.LoadInventoryCache();
			}
		}

		public ActionResult SearchProducts(string query)
		{
			query = query.ToLower();
			//TODO: fix the commented out section below to use currency
            return Json(Inventory.Products.Where(p => p.Active.ToBool() /*&& p.Prices.GetPriceByAccountType(CoreContext.CurrentAccount.AccountTypeId, AccountPriceTypeRelationshipTypes.Products, NetSteps.Common.CustomConfigurationHandler.Config.IDs.StoreFrontID) != null*/ && (p.SKU.ToLower().Contains(query) || p.Translations.Any(d => d.Name.ToLower().Contains(query) || d.ShortDescription.ToLower().Contains(query) || d.LongDescription.ToLower().Contains(query)))).Select(p => new { id = p.ProductID, text = p.SKU + " - " + (p.Translations.GetByLanguageIdOrDefault(CoreContext.CurrentLanguageID).Name) }));
		}

		public ActionResult GetCatalog(int catalogId)
		{
			StringBuilder builder = new StringBuilder();
			int count = 0;
			//TODO: fix the commented out section below to use currency
			foreach (Product product in Inventory.GetCatalog(catalogId).CatalogItems.Select(ci => ci.Product).Where(p => p.Active.ToBool() /*&& p.Prices.GetPriceByAccountType(CoreContext.CurrentAccount.AccountTypeId, AccountPriceTypeRelationshipTypes.Products, NetSteps.Common.CustomConfigurationHandler.Config.IDs.StoreFrontID) != null*/))
			{
				builder.Append("<tr").Append(count % 2 == 0 ? "" : " class=\"Alt\"").Append("><td style=\"width: 80px;\">").Append(product.SKU).Append("</td><td style=\"width: 120px;\">").Append(product.Name)
					.Append("</td><td style=\"width: 50px;\">").Append(product.Prices.Where(pp => pp.ProductPriceTypeID == Inventory.GetPriceType(CoreContext.CurrentAccount.AccountTypeID, Constants.PriceRelationshipType.Products, ConfigurationManager.GetAppSetting<int>(ConfigurationManager.VariableKey.StoreFrontID)).ProductPriceTypeID).First().Price.ToString("C"))
					.Append("</td><td style=\"width: 50px;\"><input type=\"hidden\" value=\"").Append(product.ProductID)
					.Append("\" class=\"productId\"/><input type=\"text\" value=\"0\" style=\"width: 20px;\" class=\"quantity\"/></td></tr>");
				++count;
			}
			return Content(builder.ToString());
		}
		#endregion

		#region Cart
		private object AddOrUpdateOrderItem(Dictionary<int, int> products, bool overrideQuantity)
		{
			OrderCustomer customer = CoreContext.CurrentOrder.OrderCustomers[0];
			OrderItem orderItem = null;
			List<object> orderItems = new List<object>();
			foreach (KeyValuePair<int, int> product in products)
			{
				if (customer.OrderItems.Any(oi => oi.Product.ProductID == product.Key))
				{
					orderItem = customer.OrderItems.GetOrderItemByProductID(product.Key);
					CoreContext.CurrentOrder.UpdateItem(customer, orderItem.OrderItemID, overrideQuantity ? product.Value : orderItem.Quantity + product.Value);
				}
				else
				{
					CoreContext.CurrentOrder.AddItem(product.Key, product.Value);
					orderItem = customer.OrderItems.First(oi => oi.Product.ProductID == product.Key);
				}

				orderItems.Add(new
				{
					orderItemId = orderItem.OrderItemID,
					orderItem = new StringBuilder().Append("<tr id=\"oi").Append(orderItem.OrderItemID)
						.Append("\"><td><a href=\"javascript:void(0);\" title=\"Remove\" onclick=\"remove(").Append(orderItem.OrderItemID).Append(");\"><img src=\"")
						.Append("~/Resources/Images/Icons/remove-trans.png".ResolveUrl()).Append("\" alt=\"Remove\" /></a></td><td><input type=\"hidden\" class=\"productId\" value=\"")
                        .Append(orderItem.Product.ProductID).Append("\" />").Append(orderItem.Product.SKU).Append("</td><td>").Append(orderItem.Product.Translations.GetByLanguageIdOrDefault(CoreContext.CurrentLanguageID).Name)
						.Append("</td><td>").Append(orderItem.ItemPrice.ToString("C")).Append("</td><td><input type=\"text\" class=\"quantity\" value=\"").Append(orderItem.Quantity)
						.Append("\" style=\"width:50px;\" /></td><td>").Append((orderItem.Quantity * orderItem.ItemPrice).ToString("C")).Append("</td></tr>").ToString()
				});
			}

			RemoveAllPayments();

			return orderItems;
		}

		[FunctionFilter("Orders-Order Entry", "~/Orders")]
		public ActionResult AddToCart(int productId, int quantity)
		{
			//1.	Get Changed Order Items
			//2.	Current Shipping Methods
			//3.	Totals
			//4.	Reconciled Payments

			try
			{

				return Json(new { result = true, orderItems = AddOrUpdateOrderItem(new Dictionary<int, int> { { productId, quantity } }, false), shippingMethods = ShippingMethods, totals = Totals });
			}
			catch (Exception ex)
			{
				EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = ex.Message });
			}
		}

		[FunctionFilter("Orders-Order Entry", "~/Orders")]
		public ActionResult BulkAddToCart(Dictionary<int, int> products)
		{
			//1.	Get Changed Order Items
			//2.	Current Shipping Methods
			//3.	Totals
			//4.	Reconciled Payments

			try
			{
				return Json(new { result = true, orderItems = AddOrUpdateOrderItem(products, false), shippingMethods = ShippingMethods, totals = Totals });
			}
			catch (Exception ex)
			{
				EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = ex.Message });
			}
		}

		[FunctionFilter("Orders-Order Entry", "~/Orders")]
		public ActionResult UpdateCart(Dictionary<int, int> products)
		{
			//1.	Get Changed Order Items
			//2.	Current Shipping Methods
			//3.	Totals
			//4.	Reconciled Payments

			try
			{
				return Json(new { result = true, orderItems = AddOrUpdateOrderItem(products, true), shippingMethods = ShippingMethods, totals = Totals });
			}
			catch (Exception ex)
			{
				EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = ex.Message });
			}
		}

		[FunctionFilter("Orders-Order Entry", "~/Orders")]
		public ActionResult RemoveFromCart(int orderItemId)
		{
			//1.	Get Changed Order Items
			//2.	Current Shipping Methods
			//3.	Totals
			//4.	Reconciled Payments
			try
			{
				CoreContext.CurrentOrder.RemoveItem(CoreContext.CurrentOrder.OrderCustomers[0], orderItemId);
				CoreContext.CurrentOrder.Save();

				RemoveAllPayments();

				return Json(new { result = true, shippingMethods = ShippingMethods, totals = Totals });
			}
			catch (Exception ex)
			{
				EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = ex.Message });
			}
		}
		#endregion

		#region Addresses
		public ActionResult GetAddresses()
		{
			StringBuilder addresses = new StringBuilder();
			StringBuilder options = new StringBuilder();

			foreach (Address address in CoreContext.CurrentAccount.Addresses.Where(a => a.AddressTypeID == Constants.AddressType.Shipping.ToInt()).OrderByDescending(a => a.IsDefault).ThenBy(a => a.AddressID))
			{
				addresses.Append("<div id=\"shippingAddress").Append(address.AddressID).Append("\" class=\"shippingAddressDisplay\">");
				addresses.Append("<b>").Append(address.Name).Append("</b> - ");
				addresses.Append("<a title=\"Edit\" style=\"cursor: pointer;\" onclick=\"editAddress(").Append(address.AddressID).Append(");\">Edit</a>");
				addresses.Append("<br />");
				addresses.Append(address.ToString().ToHtmlBreaks());
				addresses.Append("</div>");

				options.Append("<option value=\"").Append(address.AddressID).Append("\">").Append(address.Name);
				if (address.IsDefault)
					options.Append(" (default)");
				options.Append("</option>");
			}

			return Json(new { options = options.ToString(), addresses = addresses.ToString() });
		}

		[FunctionFilter("Orders-Order Entry", "~/Orders")]
		public ActionResult ChangeShippingAddress(int shippingAddressId)
		{
			try
			{
				OrderShipment shipment = CoreContext.CurrentOrder.GetDefaultShipment();
				UpdateOrderShipmentAddress(shipment, shippingAddressId);
				return Json(new { result = true, shippingMethods = ShippingMethods, totals = Totals });
			}
			catch (Exception ex)
			{
				return Json(new { result = false, message = ex.Message });
			}
		}
		#endregion

		[FunctionFilter("Orders-Order Entry", "~/Orders")]
		public ActionResult SetShippingMethod(int shippingMethodId)
		{
			try
			{
				Order order = CoreContext.CurrentOrder;
				OrderShipment shipment = order.GetDefaultShipment();
				shipment.ShippingMethodID = shippingMethodId;
				shipment.Name = SmallCollectionCache.Instance.ShippingMethods.GetById(shipment.ShippingMethodID.ToInt()).Name;
				//shipment.Save(); // Unnecessary is we save order, this gets saved. - JHE

				order.CalculateTotals();
				order.Save();

				// The below commented out code should be happening already when we call order.CalculateTotals(); - JHE
				//bool itemUpdated = false;
				//// Update the order items in the DB since the shipping value may allow true TaxAmount values to be set
				//foreach (OrderCustomer customer in order.OrderCustomers)
				//{
				//    foreach (OrderItem item in customer.OrderItems)
				//    {
				//        decimal originalTaxes = item.Taxes.TaxAmountTotal;
				//        if (item.ChargeTax && !customer.IsTaxExempt.ToBool())
				//            item.Taxes.TaxAmountTotal = TaxRecord.GetTaxablePriceForOrderItem(customer.OrderCustomerID, item.Product.ProductID) * item.Taxes.TaxPercent;
				//        else
				//            item.Taxes.TaxAmountTotal = 0;
				//        if (originalTaxes != item.Taxes.TaxAmountTotal)
				//        {
				//            itemUpdated = true;
				//            item.Save();
				//        }
				//    }
				//}
				//if (itemUpdated)
				//    order.Save();

				return Json(new { result = true, totals = Totals });
			}
			catch (Exception ex)
			{
				EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = ex.Message });
			}
		}

		#region Payments
		public ActionResult GetPaymentMethods()
		{
			StringBuilder paymentMethods = new StringBuilder();
			StringBuilder options = new StringBuilder();

			foreach (AccountPaymentMethod paymentMethod in CoreContext.CurrentAccount.AccountPaymentMethods.OrderByDescending(pm => pm.IsDefault).ThenBy(pm => pm.AccountPaymentMethodID))
			{
				paymentMethods.Append("<div id=\"paymentMethod").Append(paymentMethod.AccountPaymentMethodID).Append("\" class=\"paymentMethodDisplay\">");
				paymentMethods.Append("<b>").Append(paymentMethod.ProfileName).Append("</b> - ");
				paymentMethods.Append("<a title=\"Edit\" style=\"cursor: pointer;\" onclick=\"editPaymentMethod(").Append(paymentMethod.AccountPaymentMethodID).Append(");\">Edit</a>");
				paymentMethods.Append("<br />");
				paymentMethods.Append(paymentMethod.NameOnCard);
				paymentMethods.Append("<br />");
				paymentMethods.Append(paymentMethod.AccountNumber).Append("<br />").Append(paymentMethod.FormatedExpirationDate);
				paymentMethods.Append("</div>");

				options.Append("<option value=\"").Append(paymentMethod.AccountPaymentMethodID).Append("\">").Append(paymentMethod.ProfileName);
				if (paymentMethod.IsDefault)
					options.Append(" (default)");
				options.Append("</option>");
			}

			return Json(new { options = options.ToString(), paymentMethods = paymentMethods.ToString() });
		}

		[AcceptVerbs(HttpVerbs.Post)]
		[FunctionFilter("Orders-Order Entry", "~/Orders")]
		public ActionResult ApplyPayment(int paymentMethodId, decimal amount)
		{
			bool result = true;
			string message = string.Empty;
			StringBuilder paymentList = new StringBuilder();

			Order order = CoreContext.CurrentOrder;

			AccountPaymentMethod paymentMethod = CoreContext.CurrentAccount.AccountPaymentMethods.GetByAccountPaymentMethodID(paymentMethodId);
			Address billingAddress = paymentMethod.BillingAddress;
			Country billingCountry = SmallCollectionCache.Instance.Countries.GetById(billingAddress.CountryID);

			OrderPayment payment = null;

			try
			{
				if (amount > 0)
				{
					if (amount > order.GrandTotal - order.PaymentTotal)
					{
						message = Terms.Get(CoreContext.CurrentLanguageID == 0 ? 1 : CoreContext.CurrentLanguageID, "PaymentCannotExceedTotal", "Payment cannot exceed the total.");
						result = false;
					}
					else
					{
						// now that we know the amount is valid, let's add the payment to the order
						payment = order.OrderCustomers[0].OrderPayments.Where(x => x.PaymentMethodID == paymentMethodId).FirstOrDefault();
						if (payment == null)
						{
							payment = new OrderPayment()
							{
								Order = order,
								PaymentMethodID = paymentMethodId,
								DecryptedAccountNumber = paymentMethod.DecryptedAccountNumber,
								Amount = amount,
								//BillingCity = billingAddress.City,
								//BillingCountry = billingCountry,
								//BillingCountryID = billingAddress.CountryID,
								//BillingFirstName = billingAddress.FirstName,
								//BillingLastName = billingAddress.LastName,
								//BillingPhoneNumber = billingAddress.PhoneNumber,
								//BillingPostalCode = billingAddress.PostalCode,
								//BillingState = billingAddress.State,
								//BillingAddress1 = billingAddress.Address1,
								//BillingAddress2 = billingAddress.Address2,
								//BillingAddress3 = billingAddress.Address3,
								Cvv = paymentMethod.CVV,
								ExpirationDate = paymentMethod.ExpirationDate.ToDateTime(),
								OrderCustomer = order.OrderCustomers[0],
								PaymentName = paymentMethod.PaymentName.IsNullOrEmpty() ? paymentMethod.ProfileName : paymentMethod.PaymentName,
								PaymentTypeID = paymentMethod.PaymentTypeID,
								OrderPaymentStatusID = (int)Constants.OrderPaymentStatus.Pending
							};
							Reflection.CopyPropertiesDynamic<IAddress, IAddress>(billingAddress, payment);
							order.OrderCustomers[0].OrderPayments.Add(payment);
						}
						else
						{
							payment.Amount = payment.Amount += amount;
						}

						payment.Save();

						order.PaymentTotal = order.OrderCustomers[0].OrderPayments.Sum(x => x.Amount); // this keeps us from calling order.CalculateTotals() which is a very slow method

						// now get the html that will be displayed for the applied payment
						paymentList.Append("<tr id=\"pmt").Append(payment.OrderPaymentID).Append("\" class=\"paymentItem ").Append(order.OrderCustomers[0].OrderPayments.Count % 2 == 1 ? "Alt" : "").Append("\">");
						paymentList.Append("<td><a href=\"javascript:void(0);\" id=\"btnRemovePayment").Append(payment.OrderPaymentID).Append("\" onclick=\"removePayment('").Append(payment.OrderPaymentID)
							.Append("')\" title=\"Remove Payment\"><img src=\"").Append("~/Resources/Images/Icons/remove-trans.png".ResolveUrl()).Append("\" alt=\"\" /></a></td>");
						paymentList.Append("<td><a href=\"javascript:void(0);\" onclick=\"showPaymentModal('").Append(payment.OrderPaymentID).Append("')\">").Append(payment.AccountNumber).Append("</a></td>");
						paymentList.Append("<td>").Append(payment.Amount.ToString("C")).Append("</td>");
						paymentList.Append("</tr>");

					}
				}
				else if (amount < 0)
				{
					result = false;
					message = Terms.Get(CoreContext.CurrentLanguageID == 0 ? 1 : CoreContext.CurrentLanguageID, "PaymentCannotBeNegative", "Payment cannot be a negative amount.");
				}
			}
			catch (Exception ex)
			{
				result = false;
				message = ex.Message;
				EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
			}

			return Json(new { result = result, message = message, totals = Totals, payment = paymentList.ToString(), paymentId = payment == null ? 0 : payment.OrderPaymentID });
		}

		[AcceptVerbs(HttpVerbs.Post)]
		[FunctionFilter("Orders-Order Entry", "~/Orders")]
		public ActionResult RemovePayment(int paymentId)
		{
			bool result = true;
			string message = string.Empty;
			Order order = CoreContext.CurrentOrder;
			OrderPayment payment;

			try
			{
				payment = order.OrderCustomers[0].OrderPayments.Where(x => x.OrderPaymentID == paymentId).First();
				payment.Delete();
				order.OrderCustomers[0].OrderPayments.Remove(payment);
				order.PaymentTotal = order.OrderCustomers[0].OrderPayments.Sum(x => x.Amount); // this keeps us from calling order.CalculateTotals() which is a very slow method
			}
			catch (Exception ex)
			{
				result = false;
				message = ex.Message;
				EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
			}

			return Json(new { result = result, message = message, totals = Totals });
		}

		public ActionResult GetPayment(int paymentId)
		{
			OrderPayment payment = CoreContext.CurrentOrder.OrderCustomers[0].OrderPayments.Where(x => x.OrderPaymentID == paymentId).First();
			StringBuilder paymentDisplay = new StringBuilder();

			paymentDisplay.Append("<tr><td>Account Number:</td><td>").Append(payment.AccountNumber).Append("</td></tr>");
			paymentDisplay.Append("<tr class=\"Alt\"><td>Expiration Date:</td><td>").Append(payment.ExpirationDate.ToString("MM/yyyy")).Append("</td></tr>");
			paymentDisplay.Append("<tr><td>First Name:</td><td>").Append(payment.BillingFirstName).Append("</td></tr>");
			paymentDisplay.Append("<tr class=\"Alt\"><td>Last Name:</td><td>").Append(payment.BillingLastName).Append("</td></tr>");
			paymentDisplay.Append("<tr><td>Address:</td><td>").Append(payment.BillingAddress1).Append("</td></tr>");
			paymentDisplay.Append("<tr><td></td><td>").Append(payment.BillingCity).Append(", ").Append(payment.BillingState).Append("  ").Append(payment.BillingPostalCode).Append("</td></tr>");
			paymentDisplay.Append("<tr class=\"Alt\"><td>Country:</td><td>").Append(payment.BillingCountry).Append("</td></tr>");
			paymentDisplay.Append("<tr><td>Payment Status:</td><td>").Append(SmallCollectionCache.Instance.OrderPaymentStatuses.GetById(payment.OrderPaymentStatusID).Name).Append("</td></tr>");
			paymentDisplay.Append("<tr class=\"Alt\"><td>Transaction ID:</td><td>").Append(payment.TransactionID).Append("</td></tr>");
			paymentDisplay.Append("<tr><td></td><td>").Append("</td></tr>");

			return Content(paymentDisplay.ToString());
		}
		#endregion

		[FunctionFilter("Orders-Order Entry", "~/Orders")]
		public ActionResult SubmitOrder(string invoiceNotes)
		{
			try
			{
				Order order = CoreContext.CurrentOrder;

				// validate that a shipping address has been selected and contains enough information to ship the order
				if (order.OrderShipments.Count < 1)
				{
					throw new Exception(TermTranslation.LoadTermByTermNameAndLanguageID("NoShippingAddressMsg", CoreContext.CurrentLanguageID));
				}
				foreach (OrderShipment shipment in order.OrderShipments)
				{
					if (string.IsNullOrEmpty(shipment.Address1) ||
						string.IsNullOrEmpty(shipment.City) ||
						string.IsNullOrEmpty(shipment.State) ||
						string.IsNullOrEmpty(shipment.PostalCode))
					{
						throw new Exception(TermTranslation.LoadTermByTermNameAndLanguageID("InvalidShippingAddressMsg", CoreContext.CurrentLanguageID));
					}
				}

				var invoiceNote = order.Notes.FirstOrDefault(n => n.NotesTypeID == Constants.NoteType.OrderInvoiceNotes.ToInt());
				if (invoiceNote == null)
				{
					invoiceNote = new Note()
					{
						DateCreated = DateTime.Now,
						UserID = CoreContext.CurrentUser.UserID,
						NotesTypeID = Constants.NoteType.OrderInvoiceNotes.ToInt(),
					};
					order.Notes.Add(invoiceNote);
				}
				invoiceNote.NoteText = invoiceNotes.Trim();

				// Submit the order
				var result = order.SubmitOrder();
				if (!result.Success)
					throw new Exception(result.Message);

				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = ex.Message });
			}
		}

		#region Overrides
		public ActionResult GetOverrides()
		{
			StringBuilder html = new StringBuilder();
			foreach (OrderItem item in CoreContext.CurrentOrder.OrderCustomers[0].OrderItems)
			{
				html.Append("<tr id=\"").Append(item.OrderItemID).Append("\">")
					.Append("<td>").Append(item.Product.SKU).Append("</td>")
					.Append("<td>").Append(item.Product.Name).Append("</td>")
					.Append("<td>").Append(System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencySymbol).Append("<input type=\"text\" class=\"TextInput required price\" style=\"width: 50px;\" id=\"overridePrices")
						.Append(item.OrderItemID).Append("\" name=\"overridePrices\" value=\"")
						.Append(Math.Round(item.ItemPrice, 2)).Append("\" ></td>")
					.Append("<td>").Append(item.Quantity).Append("</td>")
					.Append("<td>").Append(System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencySymbol).Append("<input type=\"text\" class=\"TextInput required quantity\" style=\"width: 50px;\" id=\"cvAmount")
						.Append(item.OrderItemID).Append("\" name=\"overrideCVAmount\" value=\"")
						.Append(Math.Round(item.CommissionableTotal.ToDecimal(), 2)).Append("\" ></td>")
					.Append("<td>").Append(item.AdjustedPrice.ToDecimal().ToString("C")).Append("</td>")
					.Append("</tr>");
			}

			return Json(new { products = html.ToString(), totals = Totals });
		}

		[FunctionFilter("Orders-Override Order", "~/Orders/OrderEntry")]
		public ActionResult PerformOverrides(List<Overrides> items, decimal taxAmount, decimal shippingAmount)
		{
			bool result = true;
			string message = string.Empty;
			OrderCustomer customer = CoreContext.CurrentOrder.OrderCustomers[0];
			OrderItem currentOrderItem;
			List<object> orderItems = new List<object>();

			try
			{
				if (customer != null)
				{
					// apply item amounts
					CoreContext.CurrentOrder.OrderTypeID = Constants.OrderType.Override.ToInt();
					foreach (Overrides item in items)
					{
						currentOrderItem = customer.OrderItems.Where(x => x.OrderItemID == item.OrderItemID).First();
						if (currentOrderItem.ItemPrice == item.PricePerItem && currentOrderItem.CommissionableTotal == item.CommissionableValue)
						{
							continue; // no need to do anything, they didn't change the amounts
						}
						if (item.PricePerItem >= 0 && item.PricePerItem <= currentOrderItem.ItemPrice &&
							item.CommissionableValue >= 0 && item.CommissionableValue <= currentOrderItem.CommissionableTotal)
						{
							currentOrderItem.ItemPrice = item.PricePerItem;
							currentOrderItem.CommissionableTotal = item.CommissionableValue;
							currentOrderItem.Save();

							orderItems.Add(new
							{
								orderItemId = currentOrderItem.OrderItemID,
								orderItem = new StringBuilder("<tr id=\"oi").Append(currentOrderItem.OrderItemID)
									.Append("\"><td><a href=\"javascript:void(0);\" title=\"Remove\" onclick=\"remove(").Append(currentOrderItem.OrderItemID).Append(");\"><img src=\"")
									.Append("~/Resources/Images/Icons/remove-trans.png".ResolveUrl()).Append("\" alt=\"Remove\" /></a></td><td><input type=\"hidden\" class=\"productId\" value=\"")
									.Append(currentOrderItem.Product.ProductID).Append("\" />").Append(currentOrderItem.Product.SKU).Append("</td><td>").Append(currentOrderItem.Product.Name)
									.Append("</td><td>").Append(currentOrderItem.ItemPrice.ToString("C")).Append("</td><td><input type=\"text\" class=\"quantity\" value=\"").Append(currentOrderItem.Quantity)
									.Append("\" /></td><td>").Append((currentOrderItem.Quantity * currentOrderItem.ItemPrice).ToString("C")).Append("</td></tr>").ToString()
							});
						}
						else // amounts were out of range
						{
							result = false;
						}
					}
					if (result)
					{
						// apply new tax and shipping amounts
						if (taxAmount >= 0 && taxAmount <= customer.TaxAmountTotal && shippingAmount >= 0 && shippingAmount <= customer.ShippingAmount)
						{
							// no need to save the order customer if the amounts did not change
							if (customer.TaxAmountTotal != taxAmount || customer.ShippingAmount != shippingAmount)
							{
								customer.TaxAmountTotal = taxAmount;
								customer.ShippingAmount = shippingAmount;
								customer.Save();
							}
						}
						else // amounts were out of range
						{
							result = false;
						}
					}
					if (!result)
					{
						message = Terms.Get(CoreContext.CurrentLanguageID == 0 ? 1 : CoreContext.CurrentLanguageID, "InvalidOverrideValue", "Please check all of the values to make sure they are not negative or greater than the original amount.");
					}
					else
					{
						RemoveAllPayments();
					}
				}
				else
				{
					result = false;
					message = "No customers are associated with this order.";
				}
			}
			catch (Exception ex)
			{
				result = false;
				message = ex.Message;
				CancelOverrides(); // Undo whatever was done
				EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
			}

			return Json(new { result = result, message = message, orderItems = orderItems, shippingMethods = ShippingMethods, totals = Totals });
		}

		[FunctionFilter("Orders-Override Order", "~/Orders/OrderEntry")]
		public ActionResult CancelOverrides()
		{
			bool result = true;
			string message = string.Empty;
			OrderCustomer customer = CoreContext.CurrentOrder.OrderCustomers[0];
			Dictionary<int, int> items = new Dictionary<int, int>();
			object orderItems = null;

			try
			{
				if (customer != null)
				{
					CoreContext.CurrentOrder.OrderTypeID = Order.GetStandardOrderTypeID(CoreContext.CurrentOrder.OrderCustomers[0]);
					foreach (OrderItem item in customer.OrderItems)
					{
						items.Add(item.Product.ProductID, item.Quantity);
					}

					// TODO: I don't think it is good practice to remove all items and them add them back in. Don't use this method for that and
					// re-factor places in code that do, to do more of a sync instead. - JHE
					CoreContext.CurrentOrder.RemoveAllItems(customer);
					orderItems = AddOrUpdateOrderItem(items, false);

					RemoveAllPayments();
				}
				else
				{
					result = false;
					message = "No customers are associated with this order.";
				}
			}
			catch (Exception ex)
			{
				result = false;
				message = ex.Message;
				EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
			}

			return Json(new { result = result, message = message, orderItems = orderItems, shippingMethods = ShippingMethods, totals = Totals });
		}
		#endregion

		#endregion

		#region Autoship Template
		//[FunctionFilter("Orders-Order Entry", "~/Orders")]
		//public ActionResult AutoshipTemplate(int autoshipScheduleId)
		//{
		//    if (CoreContext.CurrentAccount == null)
		//        return RedirectToAction("Index", "Accounts");
		//    bool newAutoship;
		//    Order order = null;
		//    List<AutoshipOrder> autoshipOrders = AutoshipOrder.LoadAllByAccount(CoreContext.CurrentAccount.AccountID);
		//    DateTime lastCreated = DateTime.Now;
		//    if (autoshipOrders.Count(ao => ao.AutoshipScheduleId == autoshipScheduleId) > 0)
		//    {
		//        newAutoship = false;
		//        AutoshipOrder autoshipOrder = autoshipOrders.First(ao => ao.AutoshipScheduleId == autoshipScheduleId);
		//        lastCreated = autoshipOrder.DateLastCreated;
		//        order = autoshipOrder.TemplateOrder;
		//        if (order.Account == null)
		//            order.Account = CoreContext.CurrentAccount;

		//        order.IsTemplate = true;

		//    }
		//    else
		//    {
		//        // Load the Customer's consultant
		//        Consultant consultant = null;
		//        if (CoreContext.CurrentAccount.SponsorId < 1)
		//            consultant = new Consultant(NetSteps.Common.CustomConfigurationHandler.Config.IDs.CorporateAccountID);
		//        else
		//            consultant = new Consultant(CoreContext.CurrentAccount.SponsorId);
		//        consultant.LoadAccountInfo();

		//        // Create new order
		//        order = new Order(consultant, 0);
		//        //NS.Order order = new NS.Order();
		//        order.AccountID = CoreContext.CurrentAccount.AccountId;
		//        order.Account = CoreContext.CurrentAccount;

		//        order.IsTemplate = true;
		//        order.OrderTypeId = NetSteps.Common.CustomConfigurationHandler.Config.IDs.AutoshipOrderTypeIDs.GetByAccountTypeID(CoreContext.CurrentAccount.AccountTypeId).OrderTypeID;
		//        order.OrderStatusId = (int)NetSteps.Objects.Business.Constants.OrderStatus.Pending;
		//        order.SiteId = NetSteps.Common.CustomConfigurationHandler.Config.IDs.NSCoreSiteID;
		//        order.Save();

		//        // Add customer to order
		//        order.AddNewCustomer(CoreContext.CurrentAccount.AccountId);
		//        order.Save();

		//        newAutoship = true;
		//    }

		//    CoreContext.CurrentOrder = order;

		//    IEnumerable<ShippingMethod> shippingMethods;
		//    if (newAutoship || order.GetDefaultShipment() == null)
		//    {
		//        AccountAddress defaultShippingAddress = CoreContext.CurrentAccount.Addresses.GetDefaultByType(Constants.AddressType.Shipping);
		//        shippingMethods = defaultShippingAddress != null ? GetNewShippingMethods(defaultShippingAddress.Id) : new List<ShippingMethod>();
		//    }
		//    else
		//    {
		//        shippingMethods = Shipping.GetShippingMethodsWithRates(order.OrderCustomers[0], order.GetDefaultShipment()).Rows.Cast<DataRow>().Select(sm => new ShippingMethod
		//        {
		//            Id = (int)sm["ShippingMethodID"],
		//            ShippingMethodDesc = sm["ShortName"].ToString(),
		//            ShippingMethodName = sm["Name"].ToString(),
		//            TotalCharges = double.Parse(sm["ShippingAmount"].ToString())
		//        }).OrderBy(sm => sm.TotalCharges).ToList();
		//    }
		//    ViewData["ShippingMethods"] = shippingMethods;

		//    ViewData["NextDueDate"] = NetSteps.Objects.Logic.LogicFactory.Current.nsCoreLogicAdapter.GetAutoshipNextDueDate(lastCreated, autoshipScheduleId).ToShortDateString();

		//    ViewData["NewAutoship"] = newAutoship;

		//    ViewData["Catalogs"] = Inventory.Catalogs;

		//    AutoshipSchedule schedule = new AutoshipSchedule(autoshipScheduleId);
		//    schedule.Load();
		//    ViewData["AutoshipSchedule"] = schedule;

		//    return View(order);
		//}

		//[FunctionFilter("Orders-Order Entry", "~/Orders")]
		//public ActionResult SubmitAutoship(string invoiceNotes, int status, int scheduleId, int paymentMethodId)
		//{
		//    Order order = CoreContext.CurrentOrder;
		//    OrderShipment shipment = order.GetDefaultShipment();

		//    RemoveAllPayments();

		//    order.CalculateTotals();

		//    ApplyPayment(paymentMethodId, order.GrandTotal);

		//    try
		//    {
		//        //perform autoship logic
		//        if (NetSteps.Common.CustomConfigurationHandler.Config.IDs.AutoshipOrderTypeIDs.GetByOrderTypeID(order.OrderTypeID) != null)
		//        {
		//            AutoshipOrder autoshipOrder = AutoshipOrder.LoadByOrderId(order.OrderID);
		//            if (autoshipOrder == null)
		//            {
		//                order.DateModified = DateTime.Now;
		//                autoshipOrder = new AutoshipOrder(order, order.OrderCustomers[0].AccountID);
		//                autoshipOrder.DateLastCreated = DateTime.Today;
		//            }

		//            //TODO: figure out how we want to handle autoship schedules now

		//            //autoshipOrder.MonthlyInterval = 1;
		//            autoshipOrder.NextDueOn = NetSteps.Objects.Logic.LogicFactory.Current.nsCoreLogicAdapter.GetAutoshipNextDueDate(DateTime.Today, scheduleId);

		//            //if (CoreContext.CurrentAccount.AccountTypeId == NetSteps.Objects.Business.Constants.AccountTypeEnum.PreferredCustomer.ToInt())
		//            //{
		//            //    autoshipOrder.AutoshipScheduleId = NetSteps.Objects.Business.Constants.AutoshipSchedulingTypes.PC.ToInt();
		//            //}
		//            //else //consultant
		//            //{
		//            //    autoshipOrder.AutoshipScheduleId = NetSteps.Objects.Business.Constants.AutoshipSchedulingTypes.Consultant.ToInt();
		//            //}

		//            autoshipOrder.AutoshipScheduleId = scheduleId;
		//            autoshipOrder.StartDate = DateTime.Now;
		//            autoshipOrder.Save();
		//        }

		//        SaveInvoiceNotes(invoiceNotes);

		//        string result;
		//        // Submit the order
		//        if (!order.SubmitOrder(out result))
		//            return Json(new { result = false, message = result });

		//        order.OrderStatusId = status;
		//        order.Save();

		//        return Json(new { result = true });
		//    }
		//    catch (Exception ex)
		//    {
		//        return Json(new { result = false, message = ex.Message });
		//    }
		//}

		//[FunctionFilter("Orders", "~/Accounts")]
		//public ActionResult AutoshipOrders(int autoshipScheduleId)
		//{
		//    ViewData["scheduleName"] = AutoshipSchedule.GetAllSchedules().First(s => s.Id == autoshipScheduleId).ScheduleName;
		//    return View(autoshipScheduleId);
		//}

		//public ActionResult GetAutoshipOrders(int page, int pageSize, DateTime? startDate, DateTime? endDate, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection, int? schedule)
		//{
		//    if (startDate.HasValue && startDate.Value.Year < 1900)
		//        startDate = null;
		//    if (endDate.HasValue && endDate.Value.Year < 1900)
		//        endDate = null;
		//    StringBuilder builder = new StringBuilder();
		//    List<SearchOrderData> orders = Order.SearchAutoshipOrders(page, pageSize, null, null, startDate, endDate, CoreContext.CurrentAccount, orderBy, orderByDirection, schedule);
		//    if (orders.Count > 0)
		//    {
		//        int count = 0;
		//        foreach (SearchOrderData order in orders)
		//        {
		//            builder.Append("<tr class=\"").Append(count % 2 == 0 ? "GridRow" : "GridRowAlt").Append("\"><td><a href=\"").Append("/Orders/Details/".ResolveUrl()).Append(order.OrderID).Append("\">").Append(order.OrderNumber)
		//                .Append("</a></td><td>").Append(order.OrderStatus).Append("</td><td>").Append(order.OrderType).Append("</td>");
		//            if (order.CompleteDate.Year <= 1900)
		//                builder.Append("<td style=\"width: 100px;\">N/A</td>");
		//            else
		//                builder.Append("<td>").Append(order.CompleteDate.ToShortDateString()).Append("</td>");
		//            if (order.ShipDate.Year <= 1900)
		//                builder.Append("<td style=\"width: 100px;\">N/A</td>");
		//            else
		//                builder.Append("<td>").Append(order.ShipDate.ToShortDateString()).Append("</td>");
		//            builder.Append("<td style=\"text-align: right; padding-right: 10px;\">").Append(order.SubTotal.ToString("C"))
		//                .Append("</td><td style=\"text-align: right; padding-right: 10px;\">").Append(order.GrandTotal.ToString("C")).Append("</td><td><a href=\"")
		//                .Append("~/Accounts/Overview/".ResolveUrl()).Append(order.SponsorAccountNumber).Append("\">").Append(order.Sponsor).Append("</a></td></tr>");
		//            ++count;
		//        }
		//        return Json(new { result = true, resultCount = orders[0].TotalRows, orders = builder.ToString() });
		//    }
		//    else
		//    {
		//        return Json(new { result = true, resultCount = 0, orders = "<tr><td colspan=\"9\">There were no records found that meet that criteria.  Please try again.</td></tr>" });
		//    }
		//}

		//[FunctionFilter("Orders", "~/Orders")]
		//public ActionResult AutoshipRunOverview()
		//{
		//    return View();
		//}

		//public ActionResult GetAutoshipBatchLog(int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection)
		//{
		//    switch (orderBy)
		//    {
		//        case "ID":
		//            orderBy = "AutoshipBatchID";
		//            break;
		//        case "EndDate":
		//            orderBy = "EndDate";
		//            break;
		//        case "Succeeded":
		//            orderBy = "SucceededCount";
		//            break;
		//        case "Failure":
		//            orderBy = "FailureCount";
		//            break;
		//        case "UserName":
		//            orderBy = "UserName";
		//            break;
		//        case "Notes":
		//            orderBy = "Notes";
		//            break;
		//        default:
		//            orderBy = "StartDate";
		//            break;
		//    }

		//    StringBuilder builder = new StringBuilder();

		//    List<DataRow> autoshipBatches = Autoship.GetAutoshipRunReport().Tables[0].AsEnumerable().ToList<DataRow>();
		//    int totalRows = autoshipBatches.Count();

		//    // make sure none of the dates are null or the sort will not work since DateTime is not a nullable type
		//    foreach (DataRow autoshipBatch in autoshipBatches)
		//    {
		//        if (autoshipBatch["StartDate"].GetType() == typeof(System.DBNull))
		//            autoshipBatch["StartDate"] = DateTime.MinValue;
		//        if (autoshipBatch["EndDate"].GetType() == typeof(System.DBNull))
		//            autoshipBatch["EndDate"] = DateTime.MinValue;
		//    }

		//    // sort the list
		//    if (orderByDirection == Constants.SortDirection.Ascending)
		//    {
		//        autoshipBatches = (from ab in autoshipBatches
		//                           orderby ab[orderBy] ascending
		//                           select ab).ToList<DataRow>();
		//    }
		//    else
		//    {
		//        autoshipBatches = (from ab in autoshipBatches
		//                           orderby ab[orderBy] descending
		//                           select ab).ToList<DataRow>();
		//    }

		//    autoshipBatches = autoshipBatches.Skip(page * pageSize).Take(pageSize).ToList<DataRow>();

		//    if (autoshipBatches.Count > 0)
		//    {
		//        int count = 0;
		//        int languageID = CoreContext.CurrentLanguage == 0 ? 1 : CoreContext.CurrentLanguage;
		//        foreach (DataRow autoShipBatch in autoshipBatches)
		//        {
		//            builder.Append("<tr id=\"row").Append(autoShipBatch["AutoshipBatchID"]).Append("\" class=\"").Append(count % 2 == 0 ? "GridRow" : "GridRowAlt").Append("\" ")
		//                    .Append("style=\"cursor:pointer;\" onclick=\"showBatchDetails('").Append(autoShipBatch["AutoshipBatchID"])
		//                    .Append("');\" onmouseover=\"highlightRow('").Append(autoShipBatch["AutoshipBatchID"]).Append("');\" >")
		//                .Append("<td>").Append("<img id=\"row").Append(autoShipBatch["AutoshipBatchID"]).Append("img\" src=\"/Resources/Images/Icons/next-trans.png\" alt=\"\" />").Append("</td>")
		//                .Append("<td>").Append(autoShipBatch["AutoshipBatchID"]).Append("</td>")
		//                .Append("<td>").Append(Convert.ToDateTime(autoShipBatch["StartDate"]).Year < 1900 ? "N/A" : autoShipBatch["StartDate"]).Append("</td>")
		//                .Append("<td>").Append(Convert.ToDateTime(autoShipBatch["EndDate"]).Year < 1900 ? "N/A" : autoShipBatch["StartDate"]).Append("</td>")
		//                .Append("<td>").Append(autoShipBatch["SucceededCount"]).Append("</td>")
		//                .Append("<td>").Append(autoShipBatch["FailureCount"]).Append("</td>")
		//                .Append("<td>").Append(autoShipBatch["UserName"]).Append("</td>")
		//                .Append("<td>").Append(autoShipBatch["Notes"]).Append("</td>")
		//                .Append("</tr>");
		//            count++;
		//        }
		//        return Json(new { result = true, resultCount = totalRows, data = builder.ToString() });
		//    }
		//    else
		//    {
		//        return Json(new { result = true, resultCount = 0, data = "<tr><td colspan=\"9\">There were no records found that meet that criteria.  Please try again.</td></tr>" });
		//    }
		//}

		//[FunctionFilter("Orders", "~/Orders")]
		//public ActionResult AutoshipBatchView(int autoshipBatchId)
		//{
		//    ViewData["AutoshipBatchID"] = autoshipBatchId;
		//    return PartialView();
		//}

		//public ActionResult GetAutoshipLog(int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection, int autoshipBatchID)
		//{
		//    switch (orderBy)
		//    {
		//        case "TemplateID":
		//            orderBy = "TemplateOrderID";
		//            break;
		//        case "Succeeded":
		//            orderBy = "Succeeded";
		//            break;
		//        case "Results":
		//            orderBy = "Results";
		//            break;
		//        case "OrderID":
		//            orderBy = "NewOrderID";
		//            break;
		//        case "DateRan":
		//            orderBy = "DateAutoshipRan";
		//            break;
		//        default:
		//            orderBy = "AutoshipLogID";
		//            break;
		//    }

		//    StringBuilder builder = new StringBuilder();

		//    List<DataRow> autoshipLogs = Autoship.GetAutoshipLogsByAutoshipBatchId(autoshipBatchID).Tables[0].AsEnumerable().ToList<DataRow>();
		//    int totalRows = autoshipLogs.Count();

		//    // make sure none of the dates are null or the sort will not work since DateTime is not a nullable type
		//    foreach (DataRow autoshipLog in autoshipLogs)
		//    {
		//        if (autoshipLog["DateAutoshipRan"].GetType() == typeof(System.DBNull))
		//            autoshipLog["DateAutoshipRan"] = DateTime.MinValue;
		//    }

		//    // sort the list
		//    if (orderByDirection == Constants.SortDirection.Ascending)
		//    {
		//        autoshipLogs = (from al in autoshipLogs
		//                        orderby al[orderBy] ascending
		//                        select al).ToList<DataRow>();
		//    }
		//    else
		//    {
		//        autoshipLogs = (from al in autoshipLogs
		//                        orderby al[orderBy] descending
		//                        select al).ToList<DataRow>();
		//    }

		//    autoshipLogs = autoshipLogs.Skip(page * pageSize).Take(pageSize).ToList<DataRow>();

		//    if (autoshipLogs.Count > 0)
		//    {
		//        int count = 0;
		//        foreach (DataRow logRow in autoshipLogs)
		//        {
		//            builder.Append("<tr class=\"").Append(count % 2 == 0 ? "GridRow" : "GridRowAlt").Append("\" >")
		//                .Append("<td>").Append(logRow["AutoshipLogID"]).Append("</td>")
		//                .Append("<td><a href=\"").Append("/Orders/Details/".ResolveUrl()).Append(logRow["TemplateOrderID"]).Append("\">").Append(logRow["TemplateOrderID"]).Append("</a></td>")
		//                .Append("<td><a href=\"").Append("/Orders/Details/".ResolveUrl()).Append(logRow["NewOrderID"]).Append("\">").Append(logRow["NewOrderID"]).Append("</a></td>")
		//                .Append("<td>").Append(logRow["Succeeded"]).Append("</td>")
		//                .Append("<td>").Append(logRow["Results"]).Append("</td>")
		//                .Append("<td>").Append(Convert.ToDateTime(logRow["DateAutoshipRan"]).Year < 1900 ? "N/A" : logRow["DateAutoshipRan"]).Append("</td>")
		//                .Append("</tr>");
		//            count++;
		//        }
		//        return Json(new { result = true, resultCount = totalRows, data = builder.ToString() });
		//    }
		//    else
		//    {
		//        return Json(new { result = true, resultCount = 0, data = "<tr><td colspan=\"9\">There were no records found that meet that criteria.  Please try again.</td></tr>" });
		//    }
		//}

		#endregion

		#region Details
		[FunctionFilter("Orders", "~/Accounts")]
		public ActionResult Details(int? id)
		{
			Order order;
			if (id.HasValue)
			{
				order = Order.LoadFull(id.Value);
				//order.InvoiceNotes = OrderNote.GetNoteByOrderIDAndOrderNoteTypeID(id.Value, OrderNoteTypes.InvoiceNotes).FirstOrDefault(); // Not necessary with new Entities Objects? - JHE
				CoreContext.CurrentOrder = order;
			}
			else
			{
				order = CoreContext.CurrentOrder;
			}
			if (order == null)
				return RedirectToAction("Index");

			ViewData["Returnable"] = order.IsReturnOrder();

			return View(order);
		}

		[FunctionFilter("Orders", "~/Accounts")]
		public ActionResult CheckIfOrderFullyReturned()
		{
			List<Order> returnOrders = Order.FindChildOrders(CoreContext.CurrentOrder.OrderID, ConfigurationManager.GetAppSetting<int>(ConfigurationManager.VariableKey.ReturnOrderTypeID));
			bool fullyReturned = true;
			if (returnOrders.Count > 0)
			{
				Dictionary<int, int> returnedProducts = new Dictionary<int, int>();
				foreach (Order existingReturn in returnOrders.Where(o => o.OrderStatusID == (int)Constants.OrderStatus.Submitted))
				{
					foreach (OrderItem item in existingReturn.OrderCustomers[0].OrderItems)
					{
						if (returnedProducts.ContainsKey(item.Product.ProductID))
							returnedProducts[item.Product.ProductID] += item.Quantity;
						else
							returnedProducts.Add(item.Product.ProductID, item.Quantity);
					}
				}

				foreach (OrderItem item in CoreContext.CurrentOrder.OrderCustomers[0].OrderItems)
				{
					if (!returnedProducts.ContainsKey(item.Product.ProductID) || returnedProducts[item.Product.ProductID] < item.Quantity)
					{
						fullyReturned = false;
						break;
					}
				}
			}
			else
			{
				fullyReturned = false;
			}
			return Json(new { fullyReturned = fullyReturned });
		}

		[FunctionFilter("Orders", "~/Accounts")]
		public ActionResult ChangeCommissionConsultant(int commissionConsultantId)
		{
			try
			{
				Order currentOrder = CoreContext.CurrentOrder;
				Account consultant = Account.Load(commissionConsultantId);

				currentOrder.Consultant = consultant;
				currentOrder.Save();

				CoreContext.CurrentOrder = currentOrder;

				return Json(new
				{
					result = true,
					accountNumber = currentOrder.Consultant.AccountNumber,
					fullName = currentOrder.Consultant.FullName
				});
			}
			catch (Exception ex)
			{
				EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, Message = ex.Message });
			}
		}

		[FunctionFilter("Orders", "~/Accounts")]
		public ActionResult ChangeCommissionDate(DateTime commissionDate)
		{
			try
			{
				Order currentOrder = CoreContext.CurrentOrder;
				currentOrder.CommissionDate = commissionDate;
				currentOrder.Save();

				CoreContext.CurrentOrder = currentOrder;

				return Json(new
				{
					result = true
				});
			}
			catch (Exception ex)
			{
				EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { Result = false, message = ex.Message });
			}
		}


		#region Notes
		[FunctionFilter("Accounts-Notes", "~/Accounts/Overview")]
		public ActionResult AddNote(int? parentId, string subject, string noteText)
		{
			if (string.IsNullOrEmpty(subject) && string.IsNullOrEmpty(noteText))
			{
				return Json(new { result = false, message = "Note not saved.  No subject or text could be found for this note." });
			}

			Note newNote = new Note();
			CoreContext.CurrentOrder.Notes.Add(newNote);

			newNote.UserID = CoreContext.CurrentUser.UserID;
			newNote.Subject = subject;
			newNote.NotesTypeID = Constants.NoteType.OrderNotes.ToInt();
			newNote.NoteText = noteText;
			newNote.DateCreated = DateTime.Now;
			newNote.ParentID = parentId;
			CoreContext.CurrentOrder.Save();

			if (newNote.NoteID > 0)
				return Json(new { result = true });
			else
				return Json(new { result = false, message = "Note not saved" });
		}

		public ActionResult GetNotes(DateTime startDate, DateTime endDate, string searchCriteria)
		{
			List<Note> filteredList = Note.FilterNotes(startDate, endDate, searchCriteria, CoreContext.CurrentOrder.Notes.Where(n => n.NotesTypeID == Constants.NoteType.OrderInternalNotes.ToInt()).ToList());

			if (filteredList.Count == 0)
			{
				if (string.IsNullOrEmpty(searchCriteria.Trim()))
					return Content("<div style=\"margin-left:10px;\">No notes posted.</div>");
				else
					return Content("<div style=\"margin-left:10px;\">No notes found.</div>");
			}
			return Content(BuildNotes(filteredList, false));
		}

		public string BuildNotes(IList<Note> notes, bool isChild)
		{
			if (notes == null)
				return null;
			StringBuilder builder = new StringBuilder();
			notes.Each((n, i) =>
			{
				if (!isChild)
				{
					builder.Append("<div class=\"AcctNote").Append(i % 2 == 0 ? "" : " Alt").Append("\">").Append("<span class=\"FL NoteTitle\"><b>").Append(n.Subject).Append(" (#").Append(n.NoteID).Append(")</b> (")
						.Append(n.FollowupNotes.Count).Append(" Follow-up(s)) </span><span class=\"FR ExpandNote\"><a style=\"cursor: pointer\" onclick=\"createNewFollowup(").Append(n.NoteID).Append(");\" >Post Follow-up</a>");
					if (n.FollowupNotes.Count > 0)
					{
						builder.Append(" | <a class=\"toggleChildNotes\" style=\"cursor: pointer\">Collapse</a>");
					}
					builder.Append("</span><span class='ClearAll'></span><span class=\"NoteAuthor\">Posted on: ").Append(n.DateCreated.ToShortDateString()).Append("</b><br />").Append("Posted by: #")
					.Append(n.User.Username).Append("</span>").Append(n.NoteText);

					if (n.FollowupNotes.Count > 0)
					{
						builder.Append("<div class=\"ChildNotes\">").Append(BuildNotes(n.FollowupNotes, true)).Append("</div>");
					}
					builder.Append("</div>");
				}
				else
				{
					builder.Append("<div class=\"NoteReply\"><b>").Append(n.Subject).Append("</b> (").Append(n.FollowupNotes.Count).Append(" Follow-up)  <span class=\"FR ExpandNote\"><a style=\"cursor: pointer\" onclick=\"CreateNewFollowup(")
						.Append(n.NoteID).Append(");\" >Post Follow-up</a></span><span class=\"ClearAll\" />").Append("<span class=\"NoteAuthor\">Posted on:")
						.Append(n.DateCreated.ToShortDateString()).Append("<br />Posted by: #").Append(n.User.Username).Append("</span>").Append(n.NoteText);

					if (n.FollowupNotes.Count > 0)
					{
						builder.Append("<div class=\"ChildNotes\">").Append(BuildNotes(n.FollowupNotes, true)).Append("</div>");
					}
					builder.Append("</div>");
				}
			});
			return builder.ToString();
		}
		#endregion
		#endregion

		#region Return
		[FunctionFilter("Orders-Return Order", "~/Orders")]
		public ActionResult Return(int id = 0)
		{
			if (id > 0)
			{
				if (CoreContext.CurrentOrder == null || CoreContext.CurrentOrder.OrderID != id)
				{
					Order order = Order.Load(id);
					CoreContext.CurrentOrder = order;
				}
			}

			if (CoreContext.CurrentOrder == null)
				return RedirectToAction("Index");

			List<Order> returnOrders = Order.FindChildOrders(CoreContext.CurrentOrder.OrderID, Constants.OrderType.Return.ToInt()).Where(o => o.OrderStatusID == (int)Constants.OrderStatus.Submitted).ToList();
			if (returnOrders.Count > 0)
			{
				foreach (OrderItem item in CoreContext.CurrentOrder.OrderCustomers[0].OrderItems)
				{
					foreach (Order existingReturn in returnOrders)
					{
						if (existingReturn.OrderStatusID == Constants.OrderStatus.Cancelled.ToInt())
							continue;

						OrderItem existingItem = existingReturn.OrderCustomers[0].OrderItems.FirstOrDefault(t1 => t1.Product.ProductID == item.Product.ProductID);

						if (existingItem == null)
							continue;

						item.Quantity = item.Quantity - existingItem.Quantity;
					}
				}
			}

			// If there's no more quantities left to return on original order
			if (CoreContext.CurrentOrder.OrderCustomers[0].OrderItems.ToList().FindAll(oi => oi.Quantity > 0).Count == 0)
			{
				TempData["ReturnError"] = "All Items have been returned on this order.";
				return RedirectToAction("Details", new { id = CoreContext.CurrentOrder.OrderID });
			}

			//ViewData["ReturnTypes"] = SmallCollectionCache.Instance.ReturnTypes.ToList();
			//ViewData["ReturnReasons"] = SmallCollectionCache.Instance.ReturnReasons.ToList();

			return View(CoreContext.CurrentOrder);
		}

		private Order ReturnOrder
		{
			get
			{
				if (Session["ReturnOrder"] == null || (Session["ReturnOrder"] as Order).ParentOrderID != CoreContext.CurrentOrder.OrderID)
				{
					// Create the new return order with the items they selected
					Order order = new Order();
					order.Consultant = CoreContext.CurrentOrder.Consultant;
					order.ConsultantID = CoreContext.CurrentOrder.Consultant.AccountID;
					order.CommissionDate = DateTime.Today.AddDays(ConfigurationManager.GetAppSetting<double>(ConfigurationManager.VariableKey.ReturnOrderCommissionDateAddition));
					order.OrderTypeID = ConfigurationManager.GetAppSetting<int>(ConfigurationManager.VariableKey.ReturnOrderTypeID);
					order.ParentOrderID = CoreContext.CurrentOrder.OrderID;
					order.OrderStatusID = (int)Constants.OrderStatus.Pending;
					order.SiteID = ConfigurationManager.GetAppSetting<int>(ConfigurationManager.VariableKey.NSCoreSiteID);
					order.ShippingTotal = CoreContext.CurrentOrder.ShippingTotal;
					order.Save();

					// Add customer to order
					order.AddNewCustomer(CoreContext.CurrentOrder.OrderCustomers[0].AccountID);

					order.OrderCustomers[0].ShippingAmount = CoreContext.CurrentOrder.ShippingTotal;

					order.Save();

					Session["ReturnOrder"] = order;
				}
				return Session["ReturnOrder"] as Order;
			}
			set { Session["ReturnOrder"] = value; }
		}

		[FunctionFilter("Orders-Return Order", "~/Orders")]
		public ActionResult UpdateProductReturns(int originalOrderId, bool overridenShipping, decimal restockingFee, List<ProductReturn> returnedProducts)
		{
			try
			{
				Order originalOrder;
				if (CoreContext.CurrentOrder != null)
					originalOrder = CoreContext.CurrentOrder;
				else
				{
					originalOrder = Order.LoadFull(originalOrderId);
					//originalOrder = new Order(originalOrderId);
					//originalOrder.LoadAll();
					CoreContext.CurrentOrder = originalOrder;
				}

				Order order = ReturnOrder;

				//Add all of the returned products to the order
				decimal subtotal = 0;
				foreach (ProductReturn returnedProduct in returnedProducts.Where(p => p.OrderItemID > 0))
				{
					OrderItem orderItem = originalOrder.OrderCustomers[0].OrderItems.GetOrderItem(returnedProduct.OrderItemID);

					OrderItem returnedItem = order.OrderCustomers[0].OrderItems.GetOrderItemByProductID(orderItem.Product.ProductID);

					subtotal += orderItem.ItemPrice * returnedProduct.QuantityReturned;
					if (returnedItem == null)
					{
						order.AddItem(order.OrderCustomers[0], orderItem.Product, returnedProduct.QuantityReturned, Constants.OrderItemType.Retail, orderItem.ItemPrice);
						new OrderItemReturn()
						{
							OrderItemID = order.OrderCustomers[0].OrderItems.GetOrderItemByProductID(orderItem.Product.ProductID).OrderItemID,
							IsRestocked = returnedProduct.IsRestockable
						}.Save();
					}
					else
					{
						order.UpdateItem(order.OrderCustomers[0], returnedItem.OrderItemID, returnedProduct.QuantityReturned);

						OrderItemReturn itemReturnDetail = OrderItemReturn.LoadByOrderItemID(returnedItem.OrderItemID);
						if (itemReturnDetail.IsRestocked != returnedProduct.IsRestockable)
						{
							itemReturnDetail.IsRestocked = returnedProduct.IsRestockable;
							itemReturnDetail.Save();
						}
					}
				}

				string restockingFeeSku = ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.RestockingFeeSKU);

				if (!string.IsNullOrEmpty(restockingFeeSku))
				{
					// Check for the fee already existing
					OrderItem item = order.OrderCustomers[0].OrderItems.GetOrderItemBySku(restockingFeeSku);
					if (restockingFee > 0)
					{
						if (item != null)
						{
							order.UpdateItemPrice(item.OrderItemID, -1 * restockingFee);
						}
						else
						{
							Product restockingFeeProduct = Inventory.Products.FirstOrDefault(p => p.SKU == restockingFeeSku);
							if (restockingFeeProduct == default(Product))
							{
								restockingFeeProduct = CachedData.GetProduct(restockingFeeSku);
								//restockingFeeProduct = new Product(restockingFeeSku);
								//restockingFeeProduct.Load();
							}

							order.AddItem(order.OrderCustomers[0], restockingFeeProduct, 1, Constants.OrderItemType.Retail, -1 * restockingFee);
						}
					}
					else if (item != null)
					{
						// Remove existing restocking fee if there is one
						order.RemoveItem(item.OrderItemID);
					}
				}

				decimal? shippingTotal = null;
				if (!overridenShipping)
				{
					if (ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.Orders_Returns_RefundPercentOfShipping))
					{
						shippingTotal = order.OrderCustomers[0].ShippingAmount = originalOrder.ShippingTotal * (subtotal / originalOrder.Subtotal);
					}
					else
					{
						shippingTotal = order.OrderCustomers[0].ShippingAmount = originalOrder.ShippingTotal;
					}
				}

				order.CalculateTotals();
				order.Save();

				return Json(new
				{
					result = true,
					totals = new
					{
						subtotal = order.Subtotal.ToDecimal().ToString("C"),
						taxTotal = order.TaxAmountTotal.ToDecimal().ToString("C"),
						grandTotal = order.GrandTotal.ToDecimal().ToString("C"),
						shippingTotal = shippingTotal
					}
				});
			}
			catch (Exception ex)
			{
				EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = ex.Message });
			}
		}

		[FunctionFilter("Orders-Return Order", "~/Orders")]
		public ActionResult UpdateShippingRefunded(decimal shippingAmount)
		{
			try
			{
				Order order = ReturnOrder;

				order.OrderCustomers[0].ShippingAmount = shippingAmount;

				order.CalculateTotals();
				order.Save();

				return Json(new
				{
					result = true,
					grandTotal = order.GrandTotal.ToDecimal().ToString("C")
				});
			}
			catch (Exception ex)
			{
				EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = ex.Message });
			}
		}

		[FunctionFilter("Orders-Return Order", "~/Orders")]
		public ActionResult SubmitReturn(int originalOrderId, bool refundOriginalPayments, Dictionary<int, decimal> refundPayments, int returnType, int returnReason)
		{
			try
			{
				Order originalOrder;
				if (CoreContext.CurrentOrder != null)
					originalOrder = CoreContext.CurrentOrder;
				else
				{
					originalOrder = Order.LoadFull(originalOrderId);
					//originalOrder = new Order(originalOrderId);
					//originalOrder.LoadAll();
				}

				// Create the new return order with the items they selected
				Order order = ReturnOrder;

				if (order == null)
					return Json(new { result = false, message = "No items selected to return" });

				//Make ComissionableTotal zero for returns not in the same/current period
				Period period = SmallCollectionCache.Instance.Periods.GetPeriodByDate(DateTime.Today);
				bool makeNegative = false;
				if (originalOrder.CommissionDate > period.StartDate && originalOrder.CommissionDate < period.EndDate && order.CommissionDate > period.StartDate && order.CommissionDate < period.EndDate)
					makeNegative = true;

				foreach (var orderItem in order.OrderCustomers[0].OrderItems)
				{
					orderItem.OverrideCV = true;
					if (makeNegative)
					{
						orderItem.CommissionablePrice = Math.Abs(orderItem.CommissionablePrice) * -1;
						orderItem.CommissionableTotal = Math.Abs(orderItem.CommissionableTotal.ToDecimal()) * -1;
					}
					else
					{
						orderItem.CommissionablePrice = 0;
						orderItem.CommissionableTotal = 0;
					}
					orderItem.Save();
				}

				order.ReturnReasonID = returnReason;
				order.ReturnTypeID = returnType;
				//order.UpdateReturnReasons(returnReason, returnType); // Not necessary any more - JHE

				order.CalculateTotals();
				order.Save();

				// Add the payments to refund
				OrderCustomer customer = order.OrderCustomers[0];
				while (customer.OrderPayments.Count > 0)
					order.DeletePayment(customer.OrderPayments[0].OrderPaymentID);
				if (refundOriginalPayments)
				{
					foreach (KeyValuePair<int, decimal> payment in refundPayments)
					{
						if (payment.Value > 0)
						{
							OrderPayment orderPayment = customer.AddNewPayment(Constants.PaymentType.CreditCard);

							OrderPayment originalPayment = originalOrder.OrderCustomers[0].OrderPayments.FirstOrDefault(op => op.OrderPaymentID == payment.Key);
							if (originalPayment == null)
								return Json(new { result = false }); //TODO: What?

							// TODO: Test this CopyPropertiesDynamic to make sure this still works - JHE
							Reflection.CopyPropertiesDynamic<IPayment, IPayment>(originalPayment, orderPayment);
							Reflection.CopyPropertiesDynamic<IAddress, IAddress>(originalPayment, orderPayment);
							//orderPayment.PaymentName = originalPayment.PaymentName;
							//orderPayment.DecryptedAccountNumber = originalPayment.DecryptedAccountNumber;
							//orderPayment.ExpirationDate = originalPayment.ExpirationDate;
							//orderPayment.Cvv = originalPayment.Cvv;
							//orderPayment.BillingAddress1 = originalPayment.BillingAddress1;
							//orderPayment.BillingCity = originalPayment.BillingCity;
							//orderPayment.BillingState = originalPayment.BillingState;
							//orderPayment.BillingPostalCode = originalPayment.BillingPostalCode;
							//orderPayment.BillingFirstName = originalPayment.BillingFirstName;
							//orderPayment.BillingLastName = originalPayment.BillingLastName;
							//orderPayment.BillingPhoneNumber = originalPayment.BillingPhoneNumber;
							orderPayment.TransactionID = originalPayment.TransactionID;
							orderPayment.Amount = payment.Value;

							customer.UpdatePayment(orderPayment);
						}
					}
				}
				else
				{
					OrderPayment orderPayment = customer.AddNewPayment(Constants.PaymentType.Check);
					orderPayment.PaymentName = "Manual Refund";
					orderPayment.Amount = order.GrandTotal.ToDecimal();
					customer.UpdatePayment(orderPayment);
				}

				string message = string.Empty;
				bool result = true;
				foreach (OrderPayment payment in customer.OrderPayments)
				{
					if (payment.PaymentTypeID == (int)Constants.PaymentType.CreditCard)
					{
						var authResult = order.Refund(payment, payment.Amount);
						message = authResult.Message;
						result = authResult.Success;
					}
					else // For a manual check, no processing will take place.
					{
						result = true;
						payment.OrderPaymentStatusID = (int)Constants.OrderPaymentStatus.Completed;
						payment.ProcessedDate = DateTime.Now;
						payment.Save();
					}
				}

				if (result)
				{
					order.OrderStatusID = (int)Constants.OrderStatus.Submitted;
					order.CompleteDate = DateTime.Now;

					order.Save();
					return Json(new { result = true, returnOrderId = order.OrderID });
				}
				else
					return Json(new { result = false, message = message });
			}
			catch (Exception ex)
			{
				EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = ex.Message });
			}
		}
		#endregion
	}
}