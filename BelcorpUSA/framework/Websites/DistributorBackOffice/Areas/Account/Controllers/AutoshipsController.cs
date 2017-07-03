using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using DistributorBackOffice.Areas.Orders.Models;
using DistributorBackOffice.Helpers;
using DistributorBackOffice.Models;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Common.Context;
using NetSteps.Data.Common.Models;
using NetSteps.Data.Common.Services;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Interfaces;
using NetSteps.Encore.Core.IoC;
using NetSteps.Web;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Controls.Models.Shared.ShippingMethods;
using NetSteps.Web.Mvc.Helpers;
using ShippingMethod = NetSteps.Web.Mvc.Controls.Models.Shared.ShippingMethods.ShippingMethod;

namespace DistributorBackOffice.Areas.Account.Controllers
{
    public class AutoshipsController : BaseAccountsController
    {
        #region Order Context

        private readonly Lazy<IOrderService> _orderServiceFactory = new Lazy<IOrderService>(Create.New<IOrderService>);
        protected IOrderService OrderService { get { return _orderServiceFactory.Value; } }

        #endregion

        #region Edit Template

        #region Properties

        protected virtual object Totals
        {
            get
            {
                try
                {
                    Order order = OrderContext.Order.AsOrder();
					if (order == null)
		                return null;
                    decimal paymentTotal = order.OrderCustomers[0].OrderPayments.Sum(p => p.Amount);
                    return new
                    {
                        subtotal = order.Subtotal.ToDecimal().ToString(order.CurrencyID),
                        commissionableTotal = order.CommissionableTotal.ToDecimal().ToString(order.CurrencyID),
                        taxTotal = (order.TaxAmountTotalOverride != null) ? order.TaxAmountTotalOverride.ToString(order.CurrencyID) : order.TaxAmountTotal.ToString(order.CurrencyID),
                        shippingTotal = (order.ShippingTotalOverride != null) ? order.ShippingTotalOverride.ToString(order.CurrencyID) : order.ShippingTotal.ToString(order.CurrencyID),
                        grandTotal = order.GrandTotal.ToDecimal().ToString(order.CurrencyID),
                        paymentTotal = paymentTotal.ToString(order.CurrencyID),
                        balanceDue = (order.GrandTotal - paymentTotal).ToDecimal().ToString(order.CurrencyID),
                        balanceAmount = order.GrandTotal - paymentTotal
                    };
                }
                catch (Exception ex)
                {
					throw EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                }
            }
        }

        protected virtual string ShippingMethodsHtml
        {
            get
            {
                return GetShippingMethodsHtml();
            }
        }

        protected virtual ShippingMethodModel ShippingMethods { get; set; }

        public InventoryBaseRepository Inventory { get { return Create.New<InventoryBaseRepository>(); } }
        public IShippingCalculator ShippingCalculator { get { return Create.New<IShippingCalculator>(); } }

        #endregion

        #region Helper methods

        protected virtual IEnumerable<string> GetOutOfStockItemsString(Order o)
        {
            return o.OrderCustomers.SelectMany(oi => oi.ParentOrderItems)
                .Where(oi => Inventory.IsOutOfStock(oi.ProductID.Value))
				.Select(oi => oi.SKU + " - " + oi.ProductName);
        }

        protected virtual void UpdateOrderShipmentAddress(OrderShipment shipment, int addressId)
        {
            try
            {
                CoreContext.CurrentAutoship.StartEntityTracking();
                OrderContext.Order.AsOrder().UpdateOrderShipmentAddress(shipment, addressId);
                CoreContext.CurrentAutoship.Order = OrderContext.Order.AsOrder();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }
        }

        protected virtual IEnumerable<ShippingMethodWithRate> GetNewShippingMethods(int addressId)
        {
			var shippingMethods = OrderContext.Order.AsOrder().UpdateOrderShipmentAddressAndDefaultShipping(addressId);
			OrderService.UpdateOrder(OrderContext);

            return shippingMethods;
        }

        protected virtual List<object> GetOrderItemsHtml(Order order)
        {
            try
            {
                var orderItems = new List<object>();
				OrderCustomer orderCustomer = order.OrderCustomers[0];
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
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }
        }

        protected virtual object GetOrderItemHtml(Order order, OrderItem orderItem)
        {
            var autoshipSchedule = (Session["AutoshipSchedule"] as AutoshipSchedule);
            bool fixedAutoship = autoshipSchedule != null && autoshipSchedule.AutoshipScheduleProducts.Count > 0;
            decimal commissionableTotal = (orderItem.CommissionableTotalOverride != null) ? orderItem.CommissionableTotalOverride.ToDecimal() : orderItem.CommissionableTotal.ToDecimal();
            var product = Inventory.GetProduct(orderItem.ProductID.ToInt());

            return new StringBuilder().Append("<tr id=\"oi").Append(orderItem.Guid.ToString("N")).Append("\">")
				.AppendCell(!fixedAutoship ? "<a href=\"javascript:void(0);\" title=\"" + Translation.GetTerm("Remove") + "\" onclick=\"removeItem('" + orderItem.Guid.ToString("N") + "');\" class=\"UI-icon icon-x\" />" : string.Empty)
				.AppendCell("<input type=\"hidden\" class=\"productId\" value=\"" + orderItem.ProductID + "\" />" + orderItem.SKU)
				.AppendCell(product.Translations.Name())
				.AppendCell(orderItem.GetAdjustedPrice().ToString(order.CurrencyID))
				.AppendCell(commissionableTotal.ToString(order.CurrencyID))
				.AppendCell("<input type=\"text\" class=\"quantity\" value=\"" + orderItem.Quantity + "\" style=\"width:50px;\" />")
				.AppendCell((orderItem.GetAdjustedPrice() * orderItem.Quantity).ToString(order.CurrencyID))
				.Append("</tr>").ToString();
        }

        protected virtual BasicResponseItem<OrderPayment> ApplyPayment(int paymentMethodId, decimal amount)
        {
			var paymentMethod = NetSteps.Data.Entities.Account.LoadPaymentMethodAndVerifyAccount(paymentMethodId, OrderContext.Order.AsOrder().OrderCustomers[0].AccountID);
			var response = OrderContext.Order.AsOrder().ApplyPaymentToCustomer(paymentMethod.PaymentTypeID,amount,paymentMethod.NameOnCard,paymentMethod); 
            return response;
        }

        protected virtual Address SelectedShippingAddress(Order order)
        {
            var shippingAddresses = CurrentAccount.Addresses.Where(a => a.AddressTypeID == Constants.AddressType.Shipping.ToInt()).ToList();
            var defaultShipment = order.GetDefaultShipment();
            Address selectedShippingAddress = null;

            if (shippingAddresses.Count > 0 && order.IsTemplate && defaultShipment != null)
            {
                OrderShipment shipment = defaultShipment;
                if (shipment.SourceAddressID.HasValue)
				{
					selectedShippingAddress = NetSteps.Data.Entities.Account.LoadAddressAndVerifyAccount(shipment.SourceAddressID.Value, order.OrderCustomers[0].AccountID);
				}
                else
				{
                    selectedShippingAddress = shippingAddresses.FirstOrDefault(sa => sa.Address1 == shipment.Address1 && sa.City == shipment.City);
				}
			}

            return selectedShippingAddress;
        }

        public virtual IBulkAddModel GetBulkAddModel(int? categoryID = null)
        {
            var model = Create.New<IBulkAddModel>();
            //set options
            model.GetProductsUrl = Url.Action("GetCategoryProducts");
            model.AddProductsUrl = Url.Action("BulkAddToCart");
            //get data
            model.Data = GetBulkAddModelData(categoryID);

            return model;
        }

        protected virtual IBulkAddModelData GetBulkAddModelData(int? categoryId = null)
        {
            var data = Create.New<IBulkAddModelData>();
            var helper = Create.New<ICategoryHelper>();
            var categories = helper.GetDwsActiveCategories(CurrentAccount).Select(c =>
            {
                var category = Create.New<ICategoryInfoModel>();
                category.CategoryID = c.CategoryID;
                category.Name = c.Translations.Name();

                return category;
            });
            data.Categories = categories.ToList();
            int selectedCategory = categoryId.HasValue ? categoryId.Value : categories.First().CategoryID;
            data.Products = GetProductsForCategory(selectedCategory, CurrentAccount.AccountTypeID, OrderContext.Order.CurrencyID, OrderContext.Order.OrderTypeID);

            return data;
        }

        List<IBulkProductInfoModel> GetProductsForCategory(int categoryId, int accountTypeId, int currencyId, int orderTypeId)
        {
            var helper = Create.New<ICategoryHelper>();

            return helper.GetValidProductsForCategory(categoryId, accountTypeId, currencyId)
                .Select(p =>
                {
                    var prod = Create.New<IBulkProductInfoModel>();
                    prod.Name = p.Name;
                    prod.Price = p.Prices.First(pp => pp.ProductPriceTypeID == AccountPriceType.GetPriceType(accountTypeId, Constants.PriceRelationshipType.Products, orderTypeId).ProductPriceTypeID && pp.CurrencyID == currencyId).Price.ToString(currencyId);
                    prod.ProductID = p.ProductID;
                    prod.Quantity = 0;
                    prod.SKU = p.SKU;
                    return prod;
                }).ToList();
        }

        #endregion

        [FunctionFilter("Accounts-Autoship Template", "~/Account", Constants.SiteType.BackOffice)]
        public virtual ActionResult Edit(int? id, int? autoshipScheduleId)
        {
            try
            {
                if (!id.HasValue && !autoshipScheduleId.HasValue)
                {
                    throw new ArgumentException("Either id or autoshipScheduleId must be set");
                }

                Address defaultShippingAddress = CurrentAccount.Addresses.GetDefaultByTypeID(Constants.AddressType.Shipping);
                if (defaultShippingAddress == null)
                {
                    TempData["Error"] = Translation.GetTerm("NoDefaultShippingAddress", "Please add a default shipping address before enrolling for an autoship");
                    return RedirectToAction("Index", "Landing");
                }

                bool isNewAutoship = false;
                AutoshipOrder autoshipOrder = id.HasValue ? AutoshipOrder.LoadFull(id.Value) : AutoshipOrder.LoadFullByAccountIDAndAutoshipScheduleID(CurrentAccount.AccountID, autoshipScheduleId.Value);
                AutoshipSchedule autoshipSchedule = AutoshipSchedule.LoadFull(autoshipScheduleId.HasValue ? autoshipScheduleId.Value : autoshipOrder.AutoshipScheduleID);

	            if(autoshipOrder != null && autoshipOrder.AccountID != CurrentAccount.AccountID)
	            {
		            return RedirectToAction("Index", "Landing");
	            }

                if (autoshipOrder == null)
                {
                    isNewAutoship = true;
                    autoshipOrder = AutoshipOrder.GenerateTemplateFromSchedule
                    (
                        autoshipScheduleId.Value,
                        CurrentAccount,
                        CoreContext.CurrentMarketId,
                        saveAndChargeNewOrder: false
                    );

                    OrderContext.Order = autoshipOrder.Order;
                    OrderContext.Order.AsOrder().UpdateOrderShipmentAddressAndDefaultShipping(defaultShippingAddress);
                    autoshipOrder.Order = OrderContext.Order.AsOrder();
                }

                CoreContext.CurrentAutoship = autoshipOrder;
                OrderContext.Order = autoshipOrder.Order;

                // Sync items for a fixed autoship
                if (autoshipOrder.HasFixedTemplateItems())
                {
                    autoshipOrder.SyncFixedOrderItems(false);
                    if (!isNewAutoship)
                    {
                        autoshipOrder.Save();
                    }
                }

                if (!autoshipSchedule.IsVirtualSubscription)
                {
                    GetShippingMethods();
                    ViewData["ShippingMethods"] = ShippingMethods;
                }
                else
                {
                    ViewData["ShippingMethods"] = new ShippingMethodModel();

                    if (isNewAutoship || OrderContext.Order.AsOrder().GetDefaultShipment() == null)
                    {
                        UpdateOrderShipmentAddress(OrderContext.Order.AsOrder().GetDefaultShipment(), defaultShippingAddress.AddressID);
                    }
                    OrderContext.Order.AsOrder().SetShippingMethod(null);
                }

                OrderService.UpdateOrder(OrderContext);

                if (isNewAutoship)
                {
                    var payment = CurrentAccount.AccountPaymentMethods.GetDefaultAccountPaymentMethod();
                    if (payment == null)
                    {
                        TempData["Error"] = "There are no payment profiles on your account.";
                        return RedirectToAction("Index", "Landing");
                    }

					if (OrderContext.Order.AsOrder().GrandTotal.HasValue && OrderContext.Order.AsOrder().GrandTotal > 0)
                    {
						var paymentMethod = NetSteps.Data.Entities.Account.LoadPaymentMethodAndVerifyAccount(payment.AccountPaymentMethodID, OrderContext.Order.AsOrder().OrderCustomers[0].AccountID);
						var response = OrderContext.Order.AsOrder().ApplyPaymentToCustomer(paymentMethod.PaymentTypeID, OrderContext.Order.AsOrder().GrandTotal.HasValue ? OrderContext.Order.AsOrder().GrandTotal.Value : 0,paymentMethod.NameOnCard,paymentMethod);
						OrderContext.Order.AsOrder().OrderPendingState = Constants.OrderPendingStates.Open;
						OrderService.UpdateOrder(OrderContext);
						OrderContext.Order.AsOrder().OrderPendingState = Constants.OrderPendingStates.Quote;
                        if (!response.Success)
                        {
                            TempData["Error"] = response.Message;
                            return RedirectToAction("Index", "Landing");
                        }
                    }
                }

                ViewData["Schedules"] = SmallCollectionCache.Instance.AutoshipSchedules.ToList(); // TODO: Change this later to be correctly filtered list. - JHE
                ViewData["NextDueDate"] = autoshipOrder.NextRunDate.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo);
                ViewData["NewAutoship"] = isNewAutoship;
                ViewData["AutoshipSchedule"] = autoshipSchedule;
                ViewData["AutoshipDay"] = autoshipOrder.Day;

                var categoryHelper = Create.New<ICategoryHelper>();

                ViewBag.Categories = categoryHelper.GetDwsActiveCategories(CurrentAccount).ToList();
                ViewBag.BulkAddModel = GetBulkAddModel();

                Session["AutoshipSchedule"] = autoshipSchedule; // Using Session var to access this for item updates. - JHE

                string viewName = "Edit";

                if (autoshipSchedule.AutoshipScheduleTypeID == (int)Constants.AutoshipScheduleType.SiteSubscription)
                {
                    viewName += "Subscription";
                    var site = (autoshipOrder.AutoshipOrderID > 0 ? Site.LoadByAutoshipOrderID(autoshipOrder.AutoshipOrderID) : null) ?? new Site();
                    ViewData["Site"] = site;
                    SetDomains(site);
                }

                CoreContext.CurrentAutoship = autoshipOrder;
                ViewBag.SelectedShippingAddress = SelectedShippingAddress(OrderContext.Order.AsOrder());
                autoshipOrder.Order = OrderContext.Order.AsOrder();

                return View(viewName, autoshipOrder.Order);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                TempData["Error"] = exception.PublicMessage;
                return RedirectToAction("Index", "Landing");
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Accounts-Autoship Template", "~/Account", Constants.SiteType.BackOffice)]
        public virtual ActionResult GetCategoryProducts(int categoryID = 0)
        {
            try
            {
                return Json(new { result = true, BulkAddModelData = GetBulkAddModelData(categoryID) });
            }
            catch (Exception ex)
            {
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException, (OrderContext.Order != null) ? OrderContext.Order.OrderID.ToIntNullable() : null, accountID: (CurrentAccount != null) ? CurrentAccount.AccountID.ToIntNullable() : null);
                throw exception;
            }
        }

        private void GetShippingMethods()
        {
			GetShippingMethods(OrderContext.Order.AsOrder());
        }

        private void GetShippingMethods(Order order)
        {
            var shippingRates = ShippingCalculator.GetShippingMethodsWithRates(order).OrderBy(x => x.ShippingAmount);
            ShippingMethods = new ShippingMethodModel();
            ShippingMethods.LoadResources(shippingRates, order.CurrencyID);

            // if shippingMethodID for the shipment is null, set the shipping method to the cheapest shipping rate from the list
            if (order.OrderShipments[0].ShippingMethodID == null)
            {
                if (shippingRates.Any())
                {
                    order.OrderShipments[0].ShippingMethodID = shippingRates.FirstOrDefault().ShippingMethodID;
                }
            }
            else
            {
                order.GetDefaultShippingMethodID();
                ShippingMethods.LoadValues(order.OrderShipments[0].ShippingMethodID != null ? (int)order.OrderShipments[0].ShippingMethodID : (int)order.GetDefaultShipment().ShippingMethodID);
            }
        }

        private string GetShippingMethodsHtml()
        {
            if (ShippingMethods == null)
            {
                GetShippingMethods();
            }

            if (!ShippingMethods.ShippingMethodList.Any())
            {
                return "<span>No shipping methods currently available.</span>";
            }

            var returnString = new StringBuilder();
            foreach (ShippingMethod method in ShippingMethods.ShippingMethodList)
            {
                returnString.AppendLine(String.Format(@"<span><label><input id=""SelectedShippingMethod_ShippingMethodID"" name=""SelectedShippingMethod.ShippingMethodID"" type=""radio"" value=""{0}"" {1}>", method.ShippingMethodID, method.ShippingMethodID == ShippingMethods.SelectedShippingMethod.ShippingMethodID ? "checked" : ""));
                returnString.AppendLine(String.Format("{0} : {1}", method.Name, method.ShippingAmount));
                returnString.AppendLine("</label><br></span>");
            }

            return returnString.ToString();
        }

        public void UpdateOrderSelectedShippingMethod(Order order, int? selectedShippingMethodID)
        {
            if (selectedShippingMethodID != null && selectedShippingMethodID != order.OrderShipments[0].ShippingMethodID)
            {
                order.SetShippingMethod(selectedShippingMethodID);
            }

            GetShippingMethods(order);
        }

        private void SetDomains(Site site)
        {
            if (site == null || site.SiteID == 0)
            {
                site = Site.LoadBaseSiteForNewPWS(CoreContext.CurrentMarketId);
            }

            var domains = site != null ? site.GetDomains() : new List<string>();

            ViewData["Domains"] = domains.ToArray();
        }

        [OutputCache(CacheProfile = "AutoCompleteData")]
        [FunctionFilter("Accounts-Autoship Template", "~/Account", Constants.SiteType.BackOffice)]
        public virtual ActionResult SearchProducts(string query)
        {
            try
            {
				//    return Json(Inventory.SearchProducts(NetSteps.Data.Entities.ApplicationContext.Instance.StoreFrontID, query).Select(p => new
				//    {
				//        id = p.ProductID,
				//        text = p.SKU + " - " + p.Translations.Name()
				//    }));

				return Json(Inventory.SearchProducts(NetSteps.Data.Entities.ApplicationContext.Instance.StoreFrontID, query, CurrentAccount.AccountTypeID)
                    .Where(p => Inventory.IsAvailable(p)).Select(p => new
                    {
                        id = p.ProductID,
						text = p.SKU + " - " + p.Translations.Name(),
						needsBackOrderConfirmation = Inventory.IsOutOfStock(p) && p.ProductBackOrderBehaviorID == Constants.ProductBackOrderBehavior.AllowBackorderInformCustomer.ToInt()
					}));
            }
            catch (Exception ex)
            {
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException, (OrderContext.Order != null) ? OrderContext.Order.OrderID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        #region Cart

        protected virtual object AddOrUpdateOrderItems(Dictionary<int, int> products, bool overrideQuantity)
        {
            try
            {
                OrderContext.Order.AsOrder().AddOrUpdateOrderItem(OrderContext.Order.AsOrder().OrderCustomers[0], products.Select(p => new OrderItemUpdateInfo { ProductID = p.Key, Quantity = p.Value }), overrideQuantity);
                OrderService.UpdateOrder(OrderContext);
                var orderItems = GetOrderItemsHtml(OrderContext.Order.AsOrder());

                return orderItems;
            }
            catch (Exception ex)
            {
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Accounts-Autoship Template", "~/Account", Constants.SiteType.BackOffice)]
        public virtual ActionResult AddToCart(int productId, int quantity, int? selectedShippingMethodID)
        {
            //1.	Get Changed Order Items
            //2.	Current Shipping Methods
            //3.	Totals
            //4.	Reconciled Payments

            try
            {
                bool allow = true;
                bool showOutOfStockMessage = false;
                var product = Inventory.GetProduct(productId);
				if (Product.CheckStock(productId, OrderContext.Order.AsOrder().GetDefaultShipment()).IsOutOfStock)
                {
                    switch (product.ProductBackOrderBehaviorID)
                    {
                        case (int)Constants.ProductBackOrderBehavior.AllowBackorder:
                            break;
                        case (int)Constants.ProductBackOrderBehavior.AllowBackorderInformCustomer:
                            showOutOfStockMessage = true;
                            break;
                        case (int)Constants.ProductBackOrderBehavior.ShowOutOfStockMessage:
                            allow = false;
                            showOutOfStockMessage = true;
                            break;
                        case (int)Constants.ProductBackOrderBehavior.Hide:
                            allow = false;
                            break;
                    }
                }

				UpdateOrderSelectedShippingMethod(OrderContext.Order.AsOrder(), selectedShippingMethodID);
                var orderItemsObject = AddOrUpdateOrderItems(new Dictionary<int, int> { { productId, quantity } }, false);

                OrderContext.Order.AsOrder().OrderPendingState = Constants.OrderPendingStates.Quote;
                OrderService.UpdateOrder(OrderContext);

                return Json(new
                {
                    result = true,
                    allow,
                    orderItems = orderItemsObject,
					outOfStockItems = GetOutOfStockItemsString(OrderContext.Order.AsOrder()),
                    showOutOfStockMessage,
                    shippingMethods = ShippingMethods,
                    totals = Totals,
					message = CoreContext.CurrentAutoship.GetWarningMessageForAutoshipChange()
                });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Accounts-Autoship Template", "~/Account", Constants.SiteType.BackOffice)]
        public virtual ActionResult BulkAddToCart(List<ProductQuantityContainer> products)
        {
            //1.	Get Changed Order Items
            //2.	Current Shipping Methods
            //3.	Totals
            //4.	Reconciled Payments

            try
            {
				var validProducts = products.Where(p => p.Quantity > 0).ToDictionary(p => p.ProductID, p => p.Quantity);
                if (validProducts.Count > 0)
                {
                    var orderItems = AddOrUpdateOrderItems(validProducts, false);
                    OrderContext.Order.AsOrder().OrderPendingState = Constants.OrderPendingStates.Quote;
                    OrderService.UpdateOrder(OrderContext);

                    var message = CoreContext.CurrentAutoship.GetWarningMessageForAutoshipChange();

                    return Json(new
                    {
                        result = true,
                        orderItems = orderItems,
						outOfStockItems = GetOutOfStockItemsString(OrderContext.Order.AsOrder()),
                        shippingMethods = ShippingMethods,
                        totals = Totals,
                        message = message
                    });
                }
                return JsonError(Translation.GetTerm("InvalidProductQuantity", "Invalid product quantity"));
            }
            catch (Exception ex)
            {
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Accounts-Autoship Template", "~/Account", Constants.SiteType.BackOffice)]
        public virtual ActionResult UpdateCart(Dictionary<int, int> products)
        {
            //1.	Get Changed Order Items
            //2.	Current Shipping Methods
            //3.	Totals
            //4.	Reconciled Payments

            try
            {
                if (products == null)
				{
                    return Json(new { result = false, message = Translation.GetTerm("PleaseAddItemsToOrderBeforeUpdating", "Please add items to Order before updating.") });
				}

                var orderItems = AddOrUpdateOrderItems(products, true);
				// AdOrUpdateOrderItems calls UpdateOrder once it is done doing it's thing.
                var message = CoreContext.CurrentAutoship.GetWarningMessageForAutoshipChange();

                return Json(new
                {
                    result = true,
                    orderItems = orderItems,
					outOfStockItems = GetOutOfStockItemsString(OrderContext.Order.AsOrder()),
                    shippingMethods = ShippingMethods,
                    totals = Totals,
                    message = message
                });
            }
            catch (Exception ex)
            {
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Accounts-Autoship Template", "~/Account", Constants.SiteType.BackOffice)]
        public virtual ActionResult RemoveFromCart(string orderItemGuid, string parentGuid = null, int? quantity = null, int? selectedShippingMethodID = null)
        {
            //1.	Get Changed Order Items
            //2.	Current Shipping Methods
            //3.	Totals
            //4.	Reconciled Payments
            try
            {
				var customer = OrderContext.Order.AsOrder().OrderCustomers.FirstOrDefault();
                var item = customer.OrderItems.FirstOrDefault(oi => oi.Guid.ToString("N") == orderItemGuid);

                if (item == null)
                    return Json(new
                    {
                        result = false,
                        message = Translation.GetTerm("ItemDoesNotExist", "That item does not exist.")
                    });
                if (quantity.HasValue)
                {
                    item.Quantity = item.Quantity - quantity.Value;
                    if (item.Quantity <= 0)
                    {
						OrderContext.Order.AsOrder().RemoveItem(customer, item);
                    }
                }
                else
                {
					OrderContext.Order.AsOrder().RemoveItem(customer, item);
                }

				UpdateOrderSelectedShippingMethod(OrderContext.Order.AsOrder(), selectedShippingMethodID);
				// We modified the order. The totals must be calculated again.
				OrderService.UpdateOrder(OrderContext);

                return Json(new
                {
                    result = true,
					outOfStockItems = GetOutOfStockItemsString(OrderContext.Order.AsOrder()),
                    shippingMethods = ShippingMethods,
                    totals = Totals,
                    message = CoreContext.CurrentAutoship.GetWarningMessageForAutoshipChange()
                });
            }
            catch (Exception ex)
            {
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException, (OrderContext != null && OrderContext.Order != null) ? OrderContext.Order.OrderID.ToIntNullable() : null, accountID: (CurrentAccount != null) ? CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
        #endregion

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Accounts-Autoship Template", "~/Account", Constants.SiteType.BackOffice)]
        public virtual ActionResult SetShippingMethod(int shippingMethodId)
        {
            try
            {
				OrderContext.Order.AsOrder().SetShippingMethod(shippingMethodId);
				OrderService.UpdateOrder(OrderContext);

                return Json(new { result = true, totals = Totals });
            }
            catch (Exception ex)
            {
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Accounts-Autoship Template", "~/Account", Constants.SiteType.BackOffice)]
        public virtual ActionResult GetAddresses()
        {
            try
            {
				StringBuilder addresses = new StringBuilder();
				StringBuilder options = new StringBuilder();

                foreach (Address address in CurrentAccount.Addresses.Where(a => a.AddressTypeID == Constants.AddressType.Shipping.ToInt()).OrderByDescending(a => a.IsDefault).ThenBy(a => a.AddressID))
                {
                    addresses.Append("<div id=\"shippingAddress").Append(address.AddressID).Append("\" class=\"shippingAddressDisplay\">")
                        .Append("<b>").Append(address.ProfileName).Append("</b> - ")
                        .Append("<a title=\"Edit\" style=\"cursor: pointer;\" onclick=\"editAddress(").Append(address.AddressID).Append(");\">Edit</a>")
                        .Append("<br />")
                        .Append(address.ToString().ToHtmlBreaks())
                        .Append("</div>");

                    options.Append("<option value=\"").Append(address.AddressID).Append("\">").Append(address.ProfileName);
                    if (address.IsDefault)
					{
                        options.Append(" (default)");
					}
                    options.Append("</option>");
                }

                return Json(new { options = options.ToString(), addresses = addresses.ToString() });
            }
            catch (Exception ex)
            {
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Accounts-Autoship Template", "~/Account", Constants.SiteType.BackOffice)]
        public virtual ActionResult GetPaymentMethods()
        {
            try
            {
				StringBuilder paymentMethods = new StringBuilder();
				StringBuilder options = new StringBuilder();

                foreach (AccountPaymentMethod paymentMethod in CurrentAccount.AccountPaymentMethods.OrderByDescending(pm => pm.IsDefault).ThenBy(pm => pm.AccountPaymentMethodID))
                {
                    paymentMethods.Append("<div id=\"paymentMethod").Append(paymentMethod.AccountPaymentMethodID).Append("\" class=\"paymentMethodDisplay\">")
                        .Append("<b>").Append(paymentMethod.ProfileName).Append("</b> - ")
                        .Append("<a title=\"Edit\" style=\"cursor: pointer;\" onclick=\"editPaymentMethod(").Append(paymentMethod.AccountPaymentMethodID).Append(");\">Edit</a>")
                        .Append("<br />")
                        .Append(paymentMethod.NameOnCard)
                        .Append("<br />")
                        .Append(paymentMethod.DecryptedAccountNumber.MaskString(4)).Append("<br />").Append(paymentMethod.FormatedExpirationDate)
                        .Append("</div>");

                    options.Append("<option value=\"").Append(paymentMethod.AccountPaymentMethodID).Append("\">").Append(paymentMethod.ProfileName);
                    if (paymentMethod.IsDefault)
					{
                        options.Append(" (default)");
					}
                    options.Append("</option>");
                }

                return Json(new { options = options.ToString(), paymentMethods = paymentMethods.ToString() });
            }
            catch (Exception ex)
            {
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Accounts-Autoship Template", "~/Account", Constants.SiteType.BackOffice)]
        public virtual ActionResult ChangeShippingAddress(int shippingAddressId)
        {
            try
            {
				OrderShipment shipment = OrderContext.Order.AsOrder().GetDefaultShipment();
                UpdateOrderShipmentAddress(shipment, shippingAddressId);

                return Json(new { result = true, shippingMethods = ShippingMethodsHtml, totals = Totals });
            }
            catch (Exception ex)
            {
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public virtual ActionResult ToggleStatus()
        {
            try
            {
	            Order order = OrderContext.Order.AsOrder();
                order.OrderStatusID = order.OrderStatusID == (short)Constants.OrderStatus.Paid ? (short)Constants.OrderStatus.Cancelled : (short)Constants.OrderStatus.Paid;

                OrderContext.Order = order;
                CoreContext.CurrentAutoship.Order = order;
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Accounts-Autoship Template", "~/Account", Constants.SiteType.BackOffice)]
        public virtual ActionResult Save(int scheduleId, int paymentMethodId,
            int? autoshipScheduleDay, int? siteUrlId, string url, int? siteId, int? selectedShippingMethodID, int? shippingAddressId)
        {
            AutoshipOrder autoshipOrder = CoreContext.CurrentAutoship;
            autoshipOrder.StartEntityTracking();
			OrderShipment shipment = OrderContext.Order.AsOrder().GetDefaultShipment();

            if (paymentMethodId > 0)
            {
                OrderContext.Order.AsOrder().OrderPendingState = Constants.OrderPendingStates.Open;
                OrderContext.Order.AsOrder().OrderCustomers[0].RemoveAllPayments();
                OrderService.UpdateOrder(OrderContext);

                var paymentMethod = NetSteps.Data.Entities.Account.LoadPaymentMethodAndVerifyAccount(paymentMethodId, OrderContext.Order.AsOrder().OrderCustomers[0].AccountID);
                //var response = OrderContext.Order.AsOrder().ApplyPaymentToCustomer(paymentMethod.PaymentTypeID, amount, paymentMethod.NameOnCard, paymentMethod); 
                var response = OrderContext.Order.AsOrder().ApplyPaymentToCustomer(paymentMethod.PaymentTypeID, OrderContext.Order.AsOrder().GrandTotal.HasValue ? OrderContext.Order.AsOrder().GrandTotal.Value : 0,paymentMethod.NameOnCard,paymentMethod);
                OrderService.UpdateOrder(OrderContext);
                OrderContext.Order.AsOrder().OrderPendingState = Constants.OrderPendingStates.Quote;

                if (!response.Success)
                {
                    return Json(new
                    {
                        result = false,
                        message = response.Message
                    });
                }

                // OrderID needs to be set in order to save the order - JHE
                // TODO: Discuss changing the OrderID column on OrderPayment to nullable later when we do party orders. - JHE
				foreach (var orderCustomer in OrderContext.Order.AsOrder().OrderCustomers)
                {
                    foreach (var orderPayment in orderCustomer.OrderPayments)
                    {
						orderPayment.OrderID = OrderContext.Order.AsOrder().OrderID;
                    }
                }
            }

            //Autoships always need a shippingmethod because of order validation.
			UpdateOrderSelectedShippingMethod(OrderContext.Order.AsOrder(), selectedShippingMethodID);

            // Though the shipping address is updated upon the drop down list change, there is a use case where there was only one selection, and therefore no triggering of that change event
            if (shippingAddressId.HasValue)
            {
                UpdateOrderShipmentAddress(shipment, shippingAddressId.Value);
            }

            try
            {
				autoshipOrder.Order = OrderContext.Order.AsOrder();
                autoshipOrder.Save();

                OrderContext.Order.AsOrder().OrderPendingState = Constants.OrderPendingStates.Quote;
                OrderService.SubmitOrder(OrderContext);

                autoshipOrder.AutoshipScheduleID = scheduleId;
                if (autoshipOrder.StartDate == null || autoshipOrder.StartDate > DateTime.Now)
                {
                    autoshipOrder.StartDate = DateTime.Now;
                }
                autoshipOrder.AccountID = CurrentAccount.AccountID;

                if (OrderContext.Order.AsOrder().SiteID == 0)
                {
                    return Json(new { result = false, message = "NSCoreSiteID is not set in the config. Please set this to a valid id." });
                }

	            if(autoshipScheduleDay.HasValue)
	            {
		            autoshipOrder.Day = autoshipScheduleDay.Value;
	            }
                autoshipOrder.Order = OrderContext.Order.AsOrder();
                CoreContext.CurrentAutoship = autoshipOrder;

                AutoshipSchedule schedule = AutoshipSchedule.Load(scheduleId);

                ReloadAutoships();

                JsonResult taken = null;
                if (schedule.AutoshipScheduleTypeID == (int)Constants.AutoshipScheduleType.SiteSubscription)
                {
					Site site = Site.LoadByAutoshipOrderID(autoshipOrder.AutoshipOrderID)
						?? new Site
						{
							AccountID = CurrentAccount.AccountID,
							AccountNumber = CurrentAccount.AccountNumber,
							CreatedByUserID = ApplicationContext.Instance.CurrentUserID,
							AutoshipOrderID = autoshipOrder.AutoshipOrderID,
							BaseSiteID = schedule.BaseSiteID,
							MarketID = CoreContext.CurrentMarketId,
							IsBase = false,
							DateCreated = DateTime.Now,
							DateSignedUp = DateTime.Now,
							SiteTypeID = (int)Constants.SiteType.Replicated,
							Name = url,
							DefaultLanguageID = 1,
							SiteStatusID = (short)Constants.SiteStatus.Active
						};
                    site.Save();
                    if (SiteUrl.IsAvailable(site.SiteID, url))
                    {
						if (site.SiteUrls.Count == 0)
		                    site.SiteUrls.Add(new SiteUrl());

                        site.SiteUrls[0].StartEntityTracking();
                        site.SiteUrls[0].Url = url;
                        site.SiteUrls[0].Save();
                    }
                    else
                    {
	                    if(string.IsNullOrEmpty(url))
	                    {
		                    url = "blank";
	                    }
                        taken = Json(new { result = false, message = string.Format("The site url '{0}' is unavailable please enter one that is.", url) });
                    }
                }
				return taken ?? Json(new { result = true, orderId = OrderContext.Order.AsOrder().OrderNumber });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        [AcceptVerbs(HttpVerbs.Post)]
        public virtual JsonResult SetAutoshipDay(int day)
        {
            AutoshipOrder autoshipOrder = CoreContext.CurrentAutoship;
            autoshipOrder.Day = day;
            autoshipOrder.Save();

            return Json(new { result = true });
        }

        #endregion

        [FunctionFilter("Accounts-Autoship Template", "~/Account", Constants.SiteType.BackOffice)]
        public virtual ActionResult View(int autoshipScheduleId)
        {
            return View(SmallCollectionCache.Instance.AutoshipSchedules.GetById(autoshipScheduleId));
        }

        public virtual ActionResult SelectedAutoshipOrder(int id)
        {
            var autoshipOrder = AutoshipOrder.Load(id);
            if (autoshipOrder.AccountID != CurrentAccountId)
            {
                return Redirect("~/Accounts");
            }
            return View(autoshipOrder);
        }

        [OutputCache(CacheProfile = "PagedGridData")]
        [FunctionFilter("Accounts-Autoship Template", "~/Account", Constants.SiteType.BackOffice)]
        public virtual ActionResult Get(int? id, int? autoshipScheduleId, DateTime? startDate, DateTime? endDate,
            int page, int pageSize, string orderBy, Constants.SortDirection orderByDirection)
        {
            try
            {
				if (startDate.HasValue && startDate.Value.Year < 1900)
		            startDate = null;
				if (endDate.HasValue && endDate.Value.Year < 1900)
		            endDate = null;
				StringBuilder builder = new StringBuilder();

                PaginatedList<OrderSearchData> orders = Order.Search(new OrderSearchParameters
                {
                    CustomerAccountID = CurrentAccount.AccountID,
                    AutoshipScheduleID = autoshipScheduleId,
                    AutoshipOrderID = id,
                    PageIndex = page,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    OrderByDirection = orderByDirection,
                    StartDate = startDate,
                    EndDate = endDate
                });
                if (orders.Count > 0)
                {
					int count = 0;
                    foreach (OrderSearchData order in orders)
                    {
                        builder.Append("<tr>")
                            .AppendLinkCell("~/Orders/Details/Index/" + order.OrderNumber, order.OrderNumber)
                            .AppendCell(order.FirstName)
                            .AppendCell(order.LastName)
                            .AppendCell(order.OrderStatus)
                            .AppendCell(order.OrderType)
                            .AppendCell(!order.CompleteDate.HasValue ? "N/A" : order.CompleteDate.ToShortDateString())
                            .AppendCell(!order.DateShipped.HasValue ? "N/A" : order.DateShipped.ToShortDateString())
                            .AppendCell(order.Subtotal.ToString("C"), style: "text-align: right; padding-right: 10px;")
                            .AppendCell(order.GrandTotal.ToString("C"), style: "text-align: right; padding-right: 10px;")
                            .AppendCell(!order.CommissionDate.HasValue ? "N/A" : order.CommissionDate.ToShortDateString())
                            .AppendLinkCell("~/Account/Overview/Index/" + order.SponsorAccountNumber, order.Sponsor)
                            .Append("</tr>");
						++count;
                    }
                    return Json(new { result = true, totalPages = orders.TotalPages, page = builder.ToString() });
                }
				else
				{
				return Json(new { result = true, totalPages = 0, page = "<tr><td colspan=\"9\">" + Translation.GetTerm("ThereWereNoRecordsFoundThatMeetThatCriteriaPleaseTryAgain", "There were no records found that meet that criteria. Please try again.") + "</td></tr>" });
				}
			}
            catch (Exception ex)
            {
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Accounts-Autoship Template", "~/Account", Constants.SiteType.BackOffice)]
        public virtual ActionResult CheckIfAvailableUrl(int siteID, string url)
        {
            try
            {
                return Json(new { result = true, url = url, available = SiteUrl.IsAvailable(siteID, url) });
            }
            catch (Exception ex)
            {
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (CurrentAccount != null) ? CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public ActionResult RedirectToPWSEnrollment()
        {
            string url = PWSURL;

            if (!string.IsNullOrWhiteSpace(url))
            {
                return Redirect(string.Format("{0}Enroll", url));
            }

            return Redirect("/Home");
        }

        public ActionResult RedirectToPWSEnrollmentPC()
        {
            string url = PWSURL;

            if (!string.IsNullOrWhiteSpace(url))
            {
                return Redirect(string.Format("{0}Enroll/PC", url));
            }

            return Redirect("/Home");
        }

        public string PWSURL
        {
            get
            {
                string url = string.Empty;

                var sites = Site.LoadByAccountID(CurrentAccount.AccountID);
                if (sites.Count > 0)
                {
                    var defaultSite = sites.FirstOrDefault(s => s.SiteStatusID == (int)Constants.SiteStatus.Active);
                    if (defaultSite != null)
                    {
                        //Get the localhost url if we are on dev boxes - DES
                        var defaultUrl = Request.Url.Authority.Contains("localhost") && defaultSite.SiteUrls.Any(su => su.Url.Contains("localhost")) ? defaultSite.SiteUrls.FirstOrDefault(su => su.Url.Contains("localhost")) : defaultSite.SiteUrls.FirstOrDefault(su => su.IsPrimaryUrl);
                        if (defaultUrl == null && defaultSite.SiteUrls.Count > 0)
                        {
                            defaultUrl = defaultSite.SiteUrls.FirstOrDefault();
                        }
                        if (defaultUrl != null)
                        {
                            url = defaultUrl.Url.AppendForwardSlash();
                        }
                    }
                }

                return url;
            }
        }
    }
}