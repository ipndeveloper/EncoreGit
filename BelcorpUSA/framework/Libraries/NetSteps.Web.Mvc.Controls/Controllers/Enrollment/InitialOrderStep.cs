using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Xml.Linq;
using NetSteps.Common.Base;
using NetSteps.Common.Configuration;
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

using OrderRules.Service.Interface;
using OrderRules.Core.Model;
using OrderRules.Service.DTO;
using OrderRules.Service.DTO.Converters;

namespace NetSteps.Web.Mvc.Controls.Controllers.Enrollment
{
	public class InitialOrderStep : BaseEnrollmentStep
	{
		public enum EnrollmentOrderType
		{
			Fixed,
			Variable,
			FixedAndVariable
		}

		private static EnrollmentOrderType _orderType;
		protected static decimal _minimumCV;
		protected static Dictionary<string, int> _products = new Dictionary<string, int>();
		protected static int _annualScheduleId;
		protected readonly static object _lock = new object();

		static InitialOrderStep()
		{
			LoadConfigValues();

			EnrollmentConfigHandler.ConfigUpdated -= new EventHandler(EnrollmentConfigHandler_ConfigUpdated);
			EnrollmentConfigHandler.ConfigUpdated += new EventHandler(EnrollmentConfigHandler_ConfigUpdated);
		}

		#region Helper Methods
		private object Totals
		{
			get
			{
				Order order = InitialOrder;
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
				OrderShipment shipment = InitialOrder.GetDefaultShipment();
				OrderCustomer customer = InitialOrder.OrderCustomers[0];

				if (shipment != null && customer != null)
				{
					if (!InitialOrder.OrderCustomers.Any(oc => oc.ContainsShippableItems()))
					{
						return String.Format("<b>{0}</b>", Translation.GetTerm("NotApplicable", "N/A"));
					}

					var shippingMethods = ShippingCalculator.GetShippingMethodsWithRates(customer, shipment).OrderBy(x => x.ShippingAmount);

					// If shipping methods have changed, this will make sure the selected shipping method is still valid.
					InitialOrder.ValidateOrderShipmentShippingMethod(shipment, shippingMethods);

					var builder = new StringBuilder();
					foreach (var shippingMethod in shippingMethods)
					{
						builder.Append("<div class=\"FL AddressProfile\"><input value=\"")
							.Append(shippingMethod.ShippingMethodID)
							.Append(shipment.ShippingMethodID == shippingMethod.ShippingMethodID ? "\" checked=\"checked" : "")
							.Append("\" type=\"radio\" name=\"shippingMethod\" class=\"Radio\" /><b>")
							.Append(shippingMethod.Name)
							.Append("</b><br />")
							.Append(shippingMethod.ShippingAmount.ToString(InitialOrder.CurrencyID))
							.Append("</div>");
					}

					return builder.ToString();
				}

				return null;
			}
		}

		private object AddOrUpdateOrderItem(OrderCustomer customer, Dictionary<int, int> products, bool overrideQuantity)
		{
			InitialOrder.AddOrUpdateOrderItem(customer, products.Select(p => new OrderItemUpdateInfo()
				{
					ProductID = p.Key,
					Quantity = p.Value
				}), overrideQuantity);

			// We've added or updated an OrderItem. We need to Calculate the totals before generating the HTML for the items.
			InitialOrder = this.TotalOrder(InitialOrder);

			List<object> orderItems = GetOrderItemsHtml(InitialOrder);
			return orderItems;
		}

		private IEnumerable<ShippingMethodWithRate> GetNewShippingMethods(int addressId)
		{
			var shippingMethods = InitialOrder.UpdateOrderShipmentAddressAndDefaultShipping(addressId);

			InitialOrder = this.TotalOrder(InitialOrder);

			return shippingMethods;
		}

		private List<object> GetOrderItemsHtml(Order order)
		{
			List<object> orderItems = new List<object>();
			OrderCustomer orderCustomer = InitialOrder.OrderCustomers[0];
			foreach (OrderItem orderItem in orderCustomer.OrderItems)
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
			string guid = item.Guid.ToString("N");

			var enrollmentProducts = _products.Where(prod => prod.Key == item.SKU);
			if (enrollmentProducts.Any())
			{
				return new StringBuilder().Append("<tr id=\"oi").Append(guid).Append("\">")
								.AppendCell("")
								.AppendCell("<input type=\"hidden\" class=\"productId\" value=\"" + item.ProductID + "\" />" + product.SKU)
								.AppendCell(product.Translations.Name())
								.AppendCell(item.ItemPrice.ToString(order.CurrencyID))
								.AppendCell(item.Quantity.ToString())
								.AppendCell((item.Quantity * item.ItemPrice).ToString(order.CurrencyID))
								.Append("</tr>").ToString();

			}
			else
			{

                var addedItemOperationID = 1;
			    
                if (item.ParentOrderItemID != null || item.OrderAdjustmentOrderLineModifications.Where(ol => ol.ModificationOperationID==addedItemOperationID).Count()>0)
                {
                    return new StringBuilder().Append("<tr id=\"oi").Append(guid).Append("\">")
                                      .AppendCell("<a> </a>")
                                      .AppendCell("<input type=\"hidden\" class=\"productId\" value=\"" + item.ProductID + "\" />" + product.SKU)
                                      .AppendCell(product.Translations.Name())
                                      .AppendCell(item.ItemPrice.ToString(order.CurrencyID))
                                      .AppendCell(item.Quantity.ToString())
                                      .AppendCell((item.Quantity * item.ItemPrice).ToString(order.CurrencyID))
                                      .Append("</tr>").ToString();
                }else
                {

                    return new StringBuilder().Append("<tr id=\"oi").Append(guid).Append("\">")
                                         .AppendCell("<a href=\"javascript:void(0);\" title=\"Remove\" class=\"initialOrderRemoveItem\" guid=\"" + guid + "\" onclick=\"remove(" + "'" + guid + "'" + ");\"><img src=\"" + "~/Resource/Content/Images/Icons/remove-trans.png".ResolveUrl() + "\" alt=\"Remove\" /></a>")
                                         .AppendCell("<input type=\"hidden\" class=\"productId\" value=\"" + item.ProductID + "\" />" + product.SKU)
                                         .AppendCell(product.Translations.Name())
                                         .AppendCell(item.ItemPrice.ToString(order.CurrencyID))
                                         .AppendCell(item.Quantity.ToString())
                                         .AppendCell((item.Quantity * item.ItemPrice).ToString(order.CurrencyID))
                                         .Append("</tr>").ToString();
                }
            }
		}

		#endregion

		public virtual ActionResult Index()
		{
			var inventory = Create.New<InventoryBaseRepository>();

			Account account = EnrollingAccount;
			Order order = this.CreateInitialOrder(account);

			InitialOrder = order;
			EnrollingAccount = account;
			ApplicationContext.Instance.CurrentAccount = account;
			AddOrUpdateOrderItem(order.OrderCustomers[0], _products.ToDictionary(p => inventory.GetProduct(p.Key).ProductID, p => p.Value), true);

			order.OrderCustomers[0].OrderItems.ToList().ForEach(oi => oi.IsEditable = false);

			var shippingMethods = order.UpdateOrderShipmentAddressAndDefaultShipping(EnrollingAccount.Addresses.GetDefaultByTypeID(Constants.AddressType.Shipping));
			_controller.ViewData["ShippingMethods"] = shippingMethods;

			_controller.ViewData["RequiresShippingMethod"] = order.OrderCustomers.Any(oc => oc.ContainsShippableItems());

			_controller.ViewData["AccountTypeID"] = (int)_enrollmentContext.AccountTypeID;
			_controller.ViewData["IsSkippable"] = this.IsSkippable;
			_controller.ViewData["StepCounter"] = _enrollmentContext.StepCounter;

			_controller.ViewData["OrderType"] = _orderType;
			_controller.ViewData["MinimumCV"] = _minimumCV;
			_controller.ViewData["Products"] = _products;

			_controller.ViewData["Subtotal"] = order.Subtotal.ToDecimal().ToString(order.CurrencyID);
			_controller.ViewData["Commissionable"] = order.CommissionableTotal.ToDecimal().ToString(order.CurrencyID);
			_controller.ViewData["Tax"] = order.TaxAmountTotal.ToDecimal().ToString(order.CurrencyID);
			_controller.ViewData["Shipping"] = order.ShippingTotal.ToDecimal().ToString(order.CurrencyID);
			_controller.ViewData["GrandTotal"] = order.GrandTotal.ToDecimal().ToString(order.CurrencyID);

			return PartialView();
		}

		public virtual ActionResult SearchProducts(string query)
		{
			try
			{
				query = query.ToCleanString();

				var products = Inventory.SearchProducts(NetSteps.Data.Entities.ApplicationContext.Instance.StoreFrontID, query, EnrollingAccount.AccountTypeID)
							.Where(p => (!Inventory.IsOutOfStock(p) || p.ProductBackOrderBehaviorID != Constants.ProductBackOrderBehavior.Hide.ToInt()));

				
				return Json(products.Select(p => new { id = p.ProductID, text = p.SKU + " - " + p.Name }));
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, (InitialOrder != null) ? InitialOrder.OrderID.ToIntNullable() : null);
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
				if (InitialOrder == null)
				{
					InitialOrder = this.CreateInitialOrder(EnrollingAccount);
				}

				var orderItems = AddOrUpdateOrderItem(InitialOrder.OrderCustomers[0], new Dictionary<int, int> { { productId, quantity } }, false);

				var shippingMethods = ShippingMethods;
				var totals = Totals;
				return Json(new
				{
					result = true,
					orderItems,
					shippingMethods,
					totals
				});
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, (InitialOrder != null) ? InitialOrder.OrderID.ToIntNullable() : null);
				return JsonError(exception.PublicMessage);
			}
		}

		public ActionResult RemoveFromCart(string guid)
		{
			//1.	Get Changed Order Items
			//2.	Current Shipping Methods
			//3.	Totals
			//4.	Reconciled Payments
			try
			{
				if (InitialOrder == null)
				{
					InitialOrder = this.CreateInitialOrder(EnrollingAccount);
				}

				var kitChildOrderItem = new List<string>();
				var staticProduct = InitialOrder.OrderCustomers[0].OrderItems.FirstOrDefault(oi => oi.Guid.ToString("N") == guid && !oi.IsEditable);
				if (staticProduct != null)
				{
					InitialOrder.UpdateItem(InitialOrder.OrderCustomers[0], staticProduct, _products.Where(p => p.Key == staticProduct.SKU).Select(p => p.Value).Single());
				}
				else
				{
					//find all child order item and remove from cart
					var orderItem = InitialOrder.OrderCustomers[0].OrderItems.FirstOrDefault(o => o.Guid.ToString("N") == guid);
        	
                    if (orderItem != null && orderItem.HasChildOrderItems)
					{
						kitChildOrderItem = orderItem.ChildOrderItems.Select(o => o.Guid.ToString("N")).ToList();
                    }
					foreach (string o in kitChildOrderItem)
                        InitialOrder.RemoveItem(InitialOrder.OrderCustomers[0], o);
                    InitialOrder.RemoveItem(InitialOrder.OrderCustomers[0], guid);
                    
                }
				InitialOrder.OrderCustomers[0].OrderPayments.RemoveAll();

				InitialOrder = this.TotalOrder(InitialOrder);

				return Json(new
				{
					result = true,
					shippingMethods = ShippingMethods,
					childsItemGUID = kitChildOrderItem,
					totals = Totals
				});
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException, (InitialOrder != null) ? InitialOrder.OrderID.ToIntNullable() : null);
				return JsonError(exception.PublicMessage);
			}
		}

		public ActionResult SetShippingMethod(int shippingMethodId)
		{
			try
			{
				InitialOrder.SetShippingMethod(shippingMethodId);

				return Json(new { result = true, totals = Totals });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException, (InitialOrder != null) ? InitialOrder.OrderID.ToIntNullable() : null);
				return JsonError(exception.PublicMessage);
			}
		}

		public virtual ActionResult SubmitStep()
		{
			try
			{
				if (InitialOrder.OrderCustomers[0].OrderItems.Count > 0)
				{
					// Set the right order sponsor ID
					InitialOrder.SetConsultantID(EnrollingAccount);

					var orderContext = Create.New<IOrderContext>();
					orderContext.Order = InitialOrder;
					OrderService.UpdateOrder(orderContext);

					if (InitialOrder.CommissionableTotal < _minimumCV)
					{
						return JsonError("The initial order does not meet the minimum commissionable value requirements.");
					}

					if (InitialOrder.GrandTotal != null)
					{
                        int paymentType = 0;
                        //InitialOrder.ApplyPaymentToCustomer(paymentType,InitialOrder.GrandTotal.Value,"",1);
						OrderService.UpdateOrder(orderContext);
					}

					BasicResponse response = OrderService.SubmitOrder(orderContext);

					if (!response.Success)
					{
						return JsonError(response.Message);
					}

					return NextStep();
				}
				return JsonError(Translation.GetTerm("InitialOrderIsRequired", "Initial order is required to proceed."));
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException, (InitialOrder != null) ? InitialOrder.OrderID.ToIntNullable() : null);
				return JsonError(exception.PublicMessage);
			}
		}

		public ActionResult SkipStep()
		{
			try
			{
				// Note that we null out the InitialOrder, but if the following page fails to load correctly
				// the user will remain on this page, still prompted to add/remove items.
				// This can cause problems with a null InitialOrder.

				var account = EnrollingAccount;
				account.Orders.RemoveAll();

				EnrollingAccount = account;
				InitialOrder = null;

				return NextStep();
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
				return JsonError(exception.PublicMessage);
			}
		}

		public InitialOrderStep(IEnrollmentStepConfig stepConfig, Controller controller, IEnrollmentContext enrollmentContext)
			: base(stepConfig, controller, enrollmentContext) { }

		protected virtual Order CreateInitialOrder(Account account)
		{
			return new Order(account)
			{
				SiteID = ConfigurationManager.GetAppSetting<int>(ConfigurationManager.VariableKey.NSCoreSiteID),
				DateCreated = DateTime.Now,
				CurrencyID = _enrollmentContext.CurrencyID,
				OrderTypeID = (short)Constants.OrderType.EnrollmentOrder,
				OrderStatusID = (short)Constants.OrderStatus.Pending,
				OrderPendingState = Constants.OrderPendingStates.Quote
			};
		}

        // CSTI(mescobar)-EB-486-Inicio
        public ActionResult ValidateOrderRules()
        {
            OrderCustomer orderCustomer = InitialOrder.AsOrder().OrderCustomers[0];
            var addedItemOperationID = 1;
            var nonPromotionalItems = orderCustomer.ParentOrderItems.Where(x => !x.OrderAdjustmentOrderLineModifications.Any(y => y.ModificationOperationID == addedItemOperationID));

            if (nonPromotionalItems == null || nonPromotionalItems.Count() == 0)
            {
                return Json(new { result = false });
            }

            /*Reglas de Salida*/
            decimal qvRule = 0;
            decimal retailRule = 0;
            decimal subtotalRule = (decimal)InitialOrder.Subtotal;
            /*Reglas de Entrada*/
            List<int> productsRule = new List<int>();
            List<int> productTypesRule = new List<int>();
            int constQV = (int)NetSteps.Data.Entities.Constants.ProductPriceType.QV;
            int constRetail = (int)NetSteps.Data.Entities.Constants.ProductPriceType.Retail;
            foreach (OrderItem orderItem in nonPromotionalItems)
            {
                var productInfo = Product.Load((int)orderItem.ProductID);
                productsRule.Add(productInfo.ProductID);
                productTypesRule.Add(ProductBase.Load(productInfo.ProductBaseID).ProductTypeID);
                qvRule += (orderItem.Quantity * (decimal)orderItem.OrderItemPrices
                                .Where(x => x.ProductPriceTypeID == constQV)
                                    .Select(y => y.OriginalUnitPrice).FirstOrDefault());
                retailRule += (orderItem.Quantity * (decimal)orderItem.OrderItemPrices
                                .Where(x => x.ProductPriceTypeID == constRetail)
                                    .Select(y => y.OriginalUnitPrice).FirstOrDefault());
            }
            int storeFrontRule = ApplicationContext.Instance.StoreFrontID;
            short orderTypeRule = InitialOrder.OrderTypeID;
            int accountRule = orderCustomer.AccountID;
            short accountTypeRule = orderCustomer.AccountTypeID;

            var ruleBasicFilter = Create.New<IOrderRulesService>().GetRules().Where(x => x.RuleStatus == (int)RuleStatus.Active &&
                                                                (x.StartDate.IsNullOrEmpty() ? true : x.StartDate <= DateTime.Now ? true : false) &&
                                                                (x.EndDate.IsNullOrEmpty() ? true : x.EndDate >= DateTime.Now ? true : false)).ToList();

            List<RulesDTO> dtoRuleComparer = new List<RulesDTO>();
            var ordeRuleConverter = Create.New<OrderRuleConverter<Rules, RulesDTO>>();
            foreach (var item in ruleBasicFilter)
            {
                dtoRuleComparer.Add(ordeRuleConverter.Convert(item));
            }

            /*Filtrar Reglas a las que aplica la order*/
            var appliedRules = dtoRuleComparer.Where(x => (x.RuleValidationsDTO.Where(y => (y.AccountIDs.Count == 0 ? true : y.AccountIDs.Contains(accountRule) ? true : false)
                                                                                && (y.AccountTypeIDs.Count == 0 ? true : y.AccountTypeIDs.Contains(accountTypeRule) ? true : false)
                                                                                && (y.OrderTypeIDs.Count == 0 ? true : y.OrderTypeIDs.Contains(orderTypeRule) ? true : false)
                                                                                && (y.StoreFrontIDs.Count == 0 ? true : y.StoreFrontIDs.Contains(storeFrontRule) ? true : false)
                                                                                && (y.ProductIDs.Count == 0 ? true : productsRule.Distinct().Intersect(y.ProductIDs).Any() ? true : false)
                                                                                && (y.ProductTypeIDs.Count == 0 ? true : productTypesRule.Distinct().Intersect(y.ProductTypeIDs).Any() ? true : false)).Any())).ToList();

            /*Filtrar Reglas no cumplidas*/
            var unfulfilledRules = appliedRules.Where(x => (x.RuleValidationsDTO.Where(y => (y.CustomerPriceSubTotalDTO.Count == 0 ? false : y.CustomerPriceSubTotalDTO.FirstOrDefault().MinimumAmount > subtotalRule ? true : false)
                                                                                        || (y.CustomerPriceTotalDTO.Count == 0 ? false : (y.CustomerPriceTotalDTO
                                                                    .Where(z => z.ProductPriceTypeID == constQV && z.MinimumAmount > qvRule).Any() || y.CustomerPriceTotalDTO
                                                                    .Where(z => z.ProductPriceTypeID == constRetail && z.MinimumAmount > retailRule).Any()))).Any())).ToList();

            /*Concatenar mensajes*/
            var messageRule = string.Empty;
            foreach (var faildRule in unfulfilledRules)
            {
                TermTranslation translation = TermTranslation.Repository.FirstOrDefault(tt => tt.TermName == faildRule.TermName && tt.LanguageID == EnrollingAccount.DefaultLanguageID);
                if (translation == default(TermTranslation))
                {
                    translation = TermTranslation.Repository.FirstOrDefault(tt => tt.TermName == faildRule.TermName);
                    messageRule += translation.Term;
                }
                else
                {
                    messageRule += translation.Term;
                }
            }

            if (!messageRule.IsEmpty())
            {
                return Json(new { result = true, message = messageRule });
            }
            else
            {
                return Json(new { result = false });
            }
        }
        // CSTI(mescobar)-EB-486-Fin


		#region Helper Methods

		private static void EnrollmentConfigHandler_ConfigUpdated(object sender, EventArgs e)
		{
			LoadConfigValues();
		}

		private static void LoadConfigValues()
		{
			lock (_lock)
			{
				IEnumerable<dynamic> initialOrderProperties = EnrollmentConfigHandler.GetProperties("InitialOrder");
				_orderType = (EnrollmentOrderType)Enum.Parse(typeof(EnrollmentOrderType), (string)initialOrderProperties.First(p => p.Name == "OrderType"), true);
				_minimumCV = decimal.Parse((string)initialOrderProperties.First(p => p.Name == "MinimumCV"));
				string temp = initialOrderProperties.FirstOrDefault(p => p.Name == "AutoshipscheduleID");
				if (!string.IsNullOrEmpty(temp))
				{
					_annualScheduleId = int.Parse(temp);
				}
				_products = ((XElement)initialOrderProperties.First(p => p.Name == "Products").Products())
					 .Elements("Product").ToDictionary(p => p.Attribute<string>("SKU", ""), p => p.Attribute<int>("Quantity"));
				string found = initialOrderProperties.FirstOrDefault(p => p.Name == "AutoshipscheduleID");
				if (!string.IsNullOrEmpty(found))
				{
					int.TryParse(found, out _annualScheduleId);
				}
			}
		}

		#endregion
	}
}