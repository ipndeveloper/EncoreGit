using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Xml.Linq;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Common.Models;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Encore.Core.IoC;
using NetSteps.Enrollment.Common.Models.Config;
using NetSteps.Enrollment.Common.Models.Context;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Controls.Infrastructure;
using NetSteps.Data.Common.Context;

namespace NetSteps.Web.Mvc.Controls.Controllers.Enrollment
{
	public class AutoshipStep : BaseEnrollmentStep
	{
		public class PrebuiltAutoship
		{
			public string Name { get; set; }
			public string Description { get; set; }
			public AutoshipSchedule AutoshipSchedule { get; set; }
			public int AccountTypeId { get; set; }
			private Dictionary<string, int> _products = new Dictionary<string, int>();
			public Dictionary<string, int> Products
			{
				get { return _products; }
				set { _products = value; }
			}
		}

		protected static List<PrebuiltAutoship> _prebuiltAutoships = new List<PrebuiltAutoship>();
		protected static decimal _minimumCV;
		private static bool _showCustom = true;
		protected readonly static object _lock = new object();

		static AutoshipStep()
		{
			LoadConfigValues();

			EnrollmentConfigHandler.ConfigUpdated -= new EventHandler(EnrollmentConfigHandler_ConfigUpdated);
			EnrollmentConfigHandler.ConfigUpdated += new EventHandler(EnrollmentConfigHandler_ConfigUpdated);
		}

		private static void EnrollmentConfigHandler_ConfigUpdated(object sender, EventArgs e)
		{
			LoadConfigValues();
		}

		protected static void LoadConfigValues()
		{
			lock (_lock)
			{
				IEnumerable<dynamic> autoshipProperties = EnrollmentConfigHandler.GetProperties("Autoship").ToList();

				_prebuiltAutoships.Clear();

				foreach (var autoshipEntry in autoshipProperties.First(p => p.Name == "PreBuiltAutoships").Autoship())
				{
					_prebuiltAutoships.Add(new PrebuiltAutoship
					{
						Name = autoshipEntry.Name,
						Description = autoshipEntry.Description(),
						AutoshipSchedule = AutoshipSchedule.LoadFull(int.Parse(autoshipEntry.AutoshipScheduleID)),
						AccountTypeId = int.Parse(autoshipEntry.AccountTypeID == null || autoshipEntry.AccountTypeID == string.Empty ? "0" : autoshipEntry.AccountTypeID),
						Products = ((XElement)autoshipEntry.Products()).Elements("Product").ToDictionary(p => p.Attribute<string>("SKU", ""), p => p.Attribute<int>("Quantity"))
					});
				}
				_minimumCV = decimal.Parse((string)autoshipProperties.First(p => p.Name == "MinimumCV"));
				string found = autoshipProperties.FirstOrDefault(p => p.Name == "ShowCustom");
				if (!string.IsNullOrEmpty(found))
				{
					bool.TryParse(found, out _showCustom);
				}
			}
		}

		#region Helper Methods
		public virtual AutoshipOrder GetAutoshipOrder()
		{
			return AutoshipOrder;
		}

		public virtual void SetAutoshipOrder(AutoshipOrder autoshipOrder)
		{
			AutoshipOrder = autoshipOrder;
		}

		private object Totals
		{
			get
			{
				AutoshipOrder autoshipOrder = GetAutoshipOrder();
				Order order = autoshipOrder.Order;
				if (order == null)
					return null;
				return new
				{
					subtotal = order.Subtotal.ToDecimal().ToString(order.CurrencyID),
					commissionableTotal = order.CommissionableTotal.ToDecimal().ToString(order.CurrencyID),
					taxTotal = (order.TaxAmountTotalOverride != null) ? order.TaxAmountTotalOverride.ToString(order.CurrencyID) : order.TaxAmountTotal.ToString(order.CurrencyID),
					shippingTotal = (order.ShippingTotalOverride != null) ? order.ShippingTotalOverride.ToString(order.CurrencyID) : order.ShippingTotal.ToString(order.CurrencyID),
					grandTotal = order.GrandTotal.ToDecimal().ToString(order.CurrencyID)
				};
			}
		}

		private string ShippingMethods
		{
			get
			{
				AutoshipOrder autoshipOrder = GetAutoshipOrder();
				Order order = autoshipOrder.Order;

				OrderShipment shipment = order.GetDefaultShipment();
				OrderCustomer customer = order.OrderCustomers[0];

				if (shipment != null && customer != null)
				{
					var builder = new StringBuilder();

					if (!order.OrderCustomers.Any(oc => oc.ContainsShippableItems()))
					{
						return String.Format("<b>{0}</b>", Translation.GetTerm("NotApplicable", "N/A"));
					}
					var shippingMethods = ShippingCalculator.GetShippingMethodsWithRates(customer, shipment).OrderBy(sm => sm.ShippingAmount);
					if (!shippingMethods.Select(sm => sm.ShippingMethodID).Contains(shipment.ShippingMethodID.ToInt()) && shippingMethods.Any())
					{
						var cheapestShippingMethod = shippingMethods.First();
						shipment.ShippingMethodID = cheapestShippingMethod.ShippingMethodID;
						autoshipOrder.Order = this.TotalOrder(autoshipOrder.Order);
						SetAutoshipOrder(autoshipOrder);
					}

					foreach (var shippingMethod in shippingMethods)
					{
						builder.Append("<div class=\"FL AddressProfile\"><input value=\"").Append(shippingMethod.ShippingMethodID).Append(shipment.ShippingMethodID == shippingMethod.ShippingMethodID ? "\" checked=\"checked" : "").Append("\" type=\"radio\" name=\"shippingMethod\" class=\"Radio\" /><b>")
							.Append(shippingMethod.Name).Append("</b><br />").Append(shippingMethod.ShippingAmount.ToString(order.CurrencyID)).Append("</div>");
					}

					return builder.ToString();
				}

				return null;
			}
		}

		private object AddOrUpdateOrderItem(OrderCustomer customer, Dictionary<int, int> products, bool overrideQuantity)
		{
			var autoshipOrder = GetAutoshipOrder();
			autoshipOrder.Order.AddOrUpdateOrderItem(customer, products.Select(p => new OrderItemUpdateInfo() { ProductID = p.Key, Quantity = p.Value }), overrideQuantity);
			SetAutoshipOrder(autoshipOrder);

			var orderItems = GetOrderItemsHtml(autoshipOrder.Order);

			return orderItems;
		}

		protected IEnumerable<ShippingMethodWithRate> GetNewShippingMethods(int addressId)
		{
			var autoshipOrder = GetAutoshipOrder();
			var shippingMethods = autoshipOrder.Order.UpdateOrderShipmentAddressAndDefaultShipping(addressId);
			autoshipOrder.Order = this.TotalOrder(autoshipOrder.Order);
			SetAutoshipOrder(autoshipOrder);

			return shippingMethods;
		}

		private List<object> GetOrderItemsHtml(Order order)
		{
			var orderItems = new List<object>();
			var orderCustomer = GetAutoshipOrder().Order.OrderCustomers[0];

			foreach (var orderItem in orderCustomer.OrderItems)
			{
				orderItems.Add(new
				{
					orderItemId = orderItem.Guid.ToString("N"),
					orderItem = GetOrderItemHtml(order, orderItem)
				});
			}

			return orderItems;
		}
		private string GetOrderItemHtml(Order order, OrderItem item)
		{
			var inventory = Create.New<InventoryBaseRepository>();

			var product = inventory.GetProduct(item.ProductID.ToInt());
			return new StringBuilder().Append("<tr id=\"oi").Append(item.Guid.ToString("N")).Append("\">")
											.AppendCell("<a href=\"javascript:void(0);\" title=\"Remove\" class=\"removeItem\" onclick=\"remove(" + "'" + item.Guid.ToString("N") + "'" + ");\"><img src=\"" + "~/Resource/Content/Images/Icons/remove-trans.png".ResolveUrl() + "\" alt=\"Remove\" /></a>")
											.AppendCell("<input type=\"hidden\" class=\"productId\" value=\"" + item.ProductID + "\" />" + product.SKU)
											.AppendCell(product.Translations.Name())
											.AppendCell(item.ItemPrice.ToString(order.CurrencyID))
											.AppendCell(item.Quantity.ToString())
											.AppendCell((item.Quantity * item.ItemPrice).ToString(order.CurrencyID))
											.Append("</tr>").ToString();
		}
		#endregion

		public ActionResult ShowCustom()
		{
			return Json(_showCustom);
		}

		public virtual ActionResult Index()
		{
			Account account = EnrollingAccount;
			AutoshipOrder autoshipOrder = GetAutoshipOrder();
			var inventory = Create.New<InventoryBaseRepository>();

			PrebuiltAutoship prebuiltAutoship = new PrebuiltAutoship();

			if (_enrollmentContext.AccountTypeID == (int)Constants.AccountType.PreferredCustomer)
			{
				prebuiltAutoship = _prebuiltAutoships.FirstOrDefault(a => a.AutoshipSchedule.AccountTypes.Any(x => x.AccountTypeID == (int)Constants.AccountType.PreferredCustomer));
			}
			else if (_enrollmentContext.AccountTypeID == (int)Constants.AccountType.Distributor)
			{
				prebuiltAutoship = _prebuiltAutoships.FirstOrDefault(a => a.AutoshipSchedule.AccountTypes.Any(x => x.AccountTypeID == (int)Constants.AccountType.Distributor));
			}

			if (prebuiltAutoship == null)
			{
				throw new NetSteps.Common.Exceptions.NetStepsBusinessException(String.Format(
						"No autoship schedules have been configured for account type: {0}",
						SmallCollectionCache.Instance.AccountTypes.GetById((short)_enrollmentContext.AccountTypeID).GetTerm()));
			}

			var autoshipScheduleID = prebuiltAutoship.AutoshipSchedule.AutoshipScheduleID;

			if (autoshipOrder == null)
			{
				autoshipOrder = AutoshipOrder.GenerateTemplateFromSchedule(
					autoshipScheduleID,
					account,
					_enrollmentContext.MarketID,
					saveAndChargeNewOrder: false
				);
			}

			autoshipOrder.StartTracking();
			autoshipOrder.AccountID = account.AccountID;
			SetAutoshipOrder(autoshipOrder);
			EnrollingAccount = account;

			AddOrUpdateOrderItem(autoshipOrder.Order.OrderCustomers[0], prebuiltAutoship.Products.ToDictionary(p => inventory.GetProduct(p.Key).ProductID, p => p.Value), true);

			IEnumerable<ShippingMethodWithRate> shippingMethods;
			Address defaultShippingAddress = EnrollingAccount.Addresses.GetDefaultByTypeID(Constants.AddressType.Shipping);
			shippingMethods = defaultShippingAddress != null ? GetNewShippingMethods(defaultShippingAddress.AddressID) : new List<ShippingMethodWithRate>();
			_controller.ViewData["ShippingMethods"] = shippingMethods;
			_controller.ViewData["AccountTypeID"] = (int)_enrollmentContext.AccountTypeID;
			_controller.ViewData["IsSkippable"] = this.IsSkippable;
			_controller.ViewData["StepCounter"] = _enrollmentContext.StepCounter;

			Order order = autoshipOrder.Order;
			_controller.ViewData["RequiresShippingMethod"] = order.OrderCustomers.Any(oc => oc.ContainsShippableItems());
			_controller.ViewData["Subtotal"] = order.Subtotal.ToDecimal().ToString(order.CurrencyID);
			_controller.ViewData["Commissionable"] = order.CommissionableTotal.ToDecimal().ToString(order.CurrencyID);
			_controller.ViewData["Tax"] = order.TaxAmountTotal.ToDecimal().ToString(order.CurrencyID);
			_controller.ViewData["Shipping"] = order.ShippingTotal.ToDecimal().ToString(order.CurrencyID);
			_controller.ViewData["GrandTotal"] = order.GrandTotal.ToDecimal().ToString(order.CurrencyID);

			var cheapestShippingMethod = shippingMethods.OrderBy(sm => sm.ShippingAmount).FirstOrDefault();

			if (cheapestShippingMethod != null)
				autoshipOrder.Order.SetShippingMethod(cheapestShippingMethod.ShippingMethodID);

			return PartialView(_prebuiltAutoships.FindAll(s => s.AutoshipSchedule.AccountTypes.Any(a => a.AccountTypeID == _enrollmentContext.AccountTypeID)));
		}


		public virtual ActionResult SearchProducts(string query)
		{
			try
			{
				query = query.ToCleanString();
				return Json(Product.SlimSearch(query).Select(p => new { id = p.ProductID, text = p.SKU + " - " + p.Name }));
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				return JsonError(exception.PublicMessage);
			}
		}

		public ActionResult AddToCart(int productId, int quantity)
		{
			//1.	Get Changed Order Items
			//2.	Current Shipping Methods
			//3.	Totals
			//4.	Reconciled Payments
			try
			{
				return Json(new
				{
					result = true,
					orderItems = AddOrUpdateOrderItem(AutoshipOrder.Order.OrderCustomers[0], new Dictionary<int, int> { { productId, quantity } }, false),
					shippingMethods = ShippingMethods,
					totals = Totals
				});
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				return JsonError(exception.PublicMessage);
			}
		}

		public ActionResult RemoveFromCart(string orderItemGuid)
		{
			//1.	Get Changed Order Items
			//2.	Current Shipping Methods
			//3.	Totals
			//4.	Reconciled Payments
			try
			{
				AutoshipOrder autoshipOrder = GetAutoshipOrder();
				autoshipOrder.Order.RemoveItem(autoshipOrder.Order.OrderCustomers[0], orderItemGuid);
				autoshipOrder.Order.OrderCustomers[0].OrderPayments.RemoveAll();
				autoshipOrder.Order = this.TotalOrder(autoshipOrder.Order);
				SetAutoshipOrder(autoshipOrder);

				return Json(new
				{
					result = true,
					shippingMethods = ShippingMethods,
					totals = Totals
				});
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				return JsonError(exception.PublicMessage);
			}
		}

		public ActionResult SetShippingMethod(int shippingMethodId)
		{
			try
			{
				AutoshipOrder autoshipOrder = GetAutoshipOrder();
				autoshipOrder.Order.SetShippingMethod(shippingMethodId);
				autoshipOrder.Order = this.TotalOrder(autoshipOrder.Order);
				SetAutoshipOrder(autoshipOrder);

				return Json(new { result = true, totals = Totals });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				return JsonError(exception.PublicMessage);
			}
		}

		[NonAction]
		private Dictionary<int, int> GetCustomOrderItems(IEnrollmentContext context)
		{
			Dictionary<int, int> customOrderItems = context.CustomOrderItems;
			if (customOrderItems == null)
			{
				customOrderItems = new Dictionary<int, int>();
				context.CustomOrderItems = customOrderItems;
			}
			return customOrderItems;
		}

		public ActionResult SetSelectedAutoship(string autoshipName, IEnrollmentContext context)
		{
			try
			{
				var inventory = Create.New<InventoryBaseRepository>();

				AutoshipOrder autoshipOrder = GetAutoshipOrder();
				Order order = autoshipOrder.Order;

				var customOrderItems = GetCustomOrderItems(context);

				if (!string.IsNullOrEmpty(autoshipName))
				{
					customOrderItems.Clear();

					// Save custom-orders if any before removing them
					foreach (var orderCustomer in order.OrderCustomers)
					{
						foreach (var orderItem in orderCustomer.OrderItems)
						{
							customOrderItems.Add(orderItem.ProductID.Value, orderItem.Quantity);
						}
					}

					order.RemoveAllOrderItems();

					PrebuiltAutoship autoship = _prebuiltAutoships.FirstOrDefault(a => a.Name == autoshipName);
					if (autoship != default(PrebuiltAutoship))
					{
						AddOrUpdateOrderItem(order.OrderCustomers[0], autoship.Products.ToDictionary(p => inventory.GetProduct(p.Key).ProductID, p => p.Value), true);
						autoshipOrder.AutoshipScheduleID = autoship.AutoshipSchedule.AutoshipScheduleID;
					}
				}
				else
				{
					order.RemoveAllOrderItems();

					// Load previously saved custom-orders if any
					foreach (var item in customOrderItems)
						order.AddItem(item.Key, item.Value);
				}

				order = this.TotalOrder(order);

				SetAutoshipOrder(autoshipOrder);

				return Json(new { result = true, totals = Totals, orderItems = GetOrderItemsHtml(order) });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				return JsonError(exception.PublicMessage);
			}
		}

		public ActionResult ChooseAutoship(string autoshipName)
		{
			try
			{
				var inventory = Create.New<InventoryBaseRepository>();

				AutoshipOrder autoshipOrder = GetAutoshipOrder();
				Order order = autoshipOrder.Order;

				order.RemoveAllOrderItems();

				if (!string.IsNullOrEmpty(autoshipName))
				{
					PrebuiltAutoship autoship = _prebuiltAutoships.FirstOrDefault(a => a.Name == autoshipName);
					if (autoship != default(PrebuiltAutoship))
					{
						AddOrUpdateOrderItem(autoshipOrder.Order.OrderCustomers[0], autoship.Products.ToDictionary(p => inventory.GetProduct(p.Key).ProductID, p => p.Value), true);
						autoshipOrder.AutoshipScheduleID = autoship.AutoshipSchedule.AutoshipScheduleID;
					}
				}

				order = this.TotalOrder(order);

				SetAutoshipOrder(autoshipOrder);

				return Json(new { result = true, totals = Totals });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				return JsonError(exception.PublicMessage);
			}
		}

		[OutputCache(CacheProfile = "DontCache")]
		public virtual ActionResult SubmitStep(int? autoshipScheduleId, int? autoshipDay)
		{
			try
			{
				AutoshipOrder autoshipOrder = GetAutoshipOrder();

				autoshipOrder.Order = this.TotalOrder(autoshipOrder.Order);

				if (autoshipOrder.Day == default(int))
				{

					if (autoshipOrder.Order.CommissionableTotal < _minimumCV)
						return JsonError("The autoship does not meet the minimum commissionable value requirements.");

					if (autoshipScheduleId.HasValue)
					{
						autoshipOrder.AutoshipScheduleID = autoshipScheduleId.Value;
					}

					AutoshipSchedule schedule = autoshipDay.HasValue ? AutoshipSchedule.LoadFull(autoshipOrder.AutoshipScheduleID) : AutoshipSchedule.Load(autoshipOrder.AutoshipScheduleID);

					autoshipOrder.Order.OrderTypeID = schedule.OrderTypeID;

					if (autoshipDay.HasValue)
					{
						var day = schedule.AutoshipScheduleDays.FirstOrDefault(asd => asd.Day == autoshipDay.Value);

						if (day == default(AutoshipScheduleDay) && schedule.AutoshipScheduleDays.Count > 0)
						{
							day = schedule.AutoshipScheduleDays.First();
						}

						autoshipOrder.Day = day.Day;
					}
				}

				ApplyAutoshipPayment(autoshipOrder);

				return NextStep();
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
				return JsonError(exception.PublicMessage);
			}
		}

		public void ApplyAutoshipPayment(AutoshipOrder autoshipOrder)
		{
			var newAccount = EnrollingAccount;
			var accountPaymentMethod = newAccount.AccountPaymentMethods.FirstOrDefault();

			Order order = new Order();

			if (autoshipOrder != null)
			{
				order = autoshipOrder.Order;
				autoshipOrder.UpdateAutoshipAccount(newAccount);

				order = autoshipOrder.Order;

				order.SetConsultantID(newAccount);

				OrderCustomer orderCustomer = null;
				if (autoshipOrder.Order.OrderCustomers.Count > 0)
					orderCustomer = autoshipOrder.Order.OrderCustomers[0];

				if (orderCustomer != null && orderCustomer.OrderItems.Count > 0)
				{
					if (accountPaymentMethod != null)
					{
						orderCustomer.RemoveAllPayments();
						//var paymentResult = order.ApplyPaymentToCustomer(newAccount.AccountPaymentMethods.First(), order.GrandTotal.Value);
                        //var paymentResult = true;
                        //if (!paymentResult.Success)
                        //    throw new Exception(paymentResult.Message);
					}
					else
						throw new Exception("Error applying payment. There is no account payment method to use.");

					var orderContext = Create.New<IOrderContext>();
					orderContext.Order = autoshipOrder.Order;
					OrderService.UpdateOrder(orderContext);

					autoshipOrder.Save();
					OrderService.SubmitOrder(orderContext);

					SetAutoshipOrder(autoshipOrder);
					EnrollingAccount = newAccount;
				}
			}
		}

		public virtual ActionResult SkipStep()
		{
			try
			{
				var account = EnrollingAccount;
				account.AutoshipOrders.RemoveAll();
				account.Orders.RemoveAll();

				EnrollingAccount = account;
				SetAutoshipOrder(null);

				return NextStep();
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
				return JsonError(exception.PublicMessage);
			}
		}

		public AutoshipStep(IEnrollmentStepConfig stepConfig, Controller controller, IEnrollmentContext enrollmentContext)
			: base(stepConfig, controller, enrollmentContext) { }
	}
}
