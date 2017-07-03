using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DistributorBackOffice.Areas.Orders.Models;
using DistributorBackOffice.Areas.Orders.Models.OrderEntry;
using DistributorBackOffice.Areas.Orders.Models.Shared;
using DistributorBackOffice.Helpers;
using DistributorBackOffice.Models;
using NetSteps.Common.Base;
using NetSteps.Common.Configuration;
using NetSteps.Common.Dynamic;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Common.Models;
using NetSteps.Data.Common.Context;
using NetSteps.Data.Common.Services;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Business.Interfaces;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Context;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Generated;
using NetSteps.Data.Entities.Interfaces;
using NetSteps.Encore.Core.IoC;
using NetSteps.Events.Common.Services;
using NetSteps.GiftCards.Common;
using NetSteps.OrderAdjustments.Common.Model;
using NetSteps.Promotions.Common;
using NetSteps.Promotions.Plugins.Common.Helpers;
using NetSteps.Promotions.Plugins.Common.Qualifications;
using NetSteps.Promotions.Plugins.Common.Rewards.Effects.Components;
using NetSteps.Promotions.Service;
using NetSteps.Promotions.UI.Common.Interfaces;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Controls.Models;
using NetSteps.Web.Mvc.Helpers;
using NetSteps.Commissions.Common;
using NetSteps.Promotions.Plugins.Common.Rewards.Concrete;
using NetSteps.Promotions.Common.ModelConcrete;
using NetSteps.Promotions.Plugins.Common.Rewards.Effects;
using NetSteps.Data.Entities.TokenValueProviders;
using System.IO;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Data.Entities.Business.Logic;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using OrderRules.Service.Interface;
using OrderRules.Service.DTO;
using OrderRules.Service.DTO.Converters;
using OrderRules.Core.Model;
using iTextSharp.text.pdf;
using System.Data;
using Microsoft.Reporting.WebForms;
using CodeBarGeerator;
using System.Globalization;
using NetSteps.Commissions.Common.Models;
using System.Web.Routing;
using DistributorBackOffice.Areas.Orders.Models.Paypal;
using System.Data.SqlClient;
using NetSteps.Data.Entities.Business.Common;
using NetSteps.Data.Common.Entities;

//using DistributorBackOffice.Areas.Orders.Helpers;


namespace DistributorBackOffice.Areas.Orders.Controllers
{
    [OutputCache(CacheProfile = "DontCache")]
    public class OrderEntryController : OrdersBaseController
    {
        private IProductCreditLedgerService _productCreditLedgerService = Create.New<IProductCreditLedgerService>();
        private ICommissionsService _commissionsService = Create.New<ICommissionsService>();
        public int wareHouseId;
        public static Boolean controlGenshipping;
        public static string valShiping;

        #region Properties

        protected virtual object Totals
        {
            get
            {
                Order order = OrderContext.Order.AsOrder();

                CalculateQualificationTotal();
                if (order == null)
                    return null;
                decimal paymentTotal = order.OrderCustomers[0].OrderPayments.Where(p => p.OrderPaymentStatusID != NetSteps.Data.Entities.Constants.OrderPaymentStatus.Cancelled.ToShort()).Sum(p => p.Amount);
                decimal shippin = 0;
                shippin = (order.ShippingTotalOverride.HasValue) ? order.HandlingTotal.ToDecimal() : 0;

                decimal totalAPagar = (paymentTotal + shippin);
                decimal balance = order.GrandTotal.GetRoundedNumber() - totalAPagar;
                if (paymentTotal > order.GrandTotal.GetRoundedNumber())
                {
                    if (balance < 0)
                        balance = balance * (-1);
                }
                else
                {
                    if (balance > 0)
                        balance = balance * (-1);
                }

                int numberofPayment = OrderContext.Order.AsOrder().OrderPayments.Where(x => x.BankName != "Product Credit").ToList().Count();

                return new
                {
                    // COMENTADO POR HUNDRED => INICIO = 10/05/2017  => RESOLVER EL FORMATO DECIMAL
                    //subtotalAdjusted = order.OrderCustomers[0].AdjustedSubTotal.ToString(order.CurrencyID),
                    //subtotal = order.Subtotal.ToDecimal().GetRoundedNumber().ToString(order.CurrencyID),
                    //commissionableTotal = order.CommissionableTotal.ToDecimal().ToString(order.CurrencyID),
                    //qualificationTotal = order.QualificationTotal.ToDecimal().ToString(order.CurrencyID),
                    //taxTotal = (order.TaxAmountTotalOverride != null) ? order.TaxAmountTotalOverride.ToString(order.CurrencyID) : order.TaxAmountTotal.ToString(order.CurrencyID),
                    //shippingTotal = (order.ShippingTotalOverride != null) ? order.ShippingTotalOverride.ToString(order.CurrencyID) : order.ShippingTotal.ToString(order.CurrencyID),
                    //handlingTotal = order.HandlingTotal.ToString(order.CurrencyID),
                    //grandTotal = order.GrandTotal.ToDecimal().ToString(order.CurrencyID),
                    //paymentTotal = order.OrderCustomers[0].OrderPayments.Where(p => p.OrderPaymentStatusID != NetSteps.Data.Entities.Constants.OrderPaymentStatus.Cancelled.ToShort() && p.OrderPaymentStatusID != (int)Constants.OrderPaymentStatus.Declined && p.Amount > 0).Sum(p => p.Amount).ToString(order.CurrencyID),
                    //balanceDue = balance.ToString(order.CurrencyID),
                    // COMENTADO POR HUNDRED => FIN = 10/05/2017 



                    subtotalAdjusted = order.OrderCustomers[0].AdjustedSubTotal.ToString("C",CoreContext.CurrentCultureInfo),
                    subtotal = order.Subtotal.ToDecimal().GetRoundedNumber().ToString("C", CoreContext.CurrentCultureInfo),
                    commissionableTotal = order.CommissionableTotal.ToDecimal().ToString("C", CoreContext.CurrentCultureInfo),
                    qualificationTotal = order.QualificationTotal.ToDecimal().ToString("C", CoreContext.CurrentCultureInfo),
                    taxTotal = (order.TaxAmountTotalOverride != null) ? order.TaxAmountTotalOverride.ToString(CoreContext.CurrentCultureInfo) : order.TaxAmountTotal.ToString(CoreContext.CurrentCultureInfo),
                    shippingTotal = (order.ShippingTotalOverride != null) ? order.ShippingTotalOverride.ToString(order.CurrencyID) : order.ShippingTotal.ToString (CoreContext.CurrentCultureInfo),
                    handlingTotal = order.HandlingTotal.ToString(CoreContext.CurrentCultureInfo),
                    grandTotal = order.GrandTotal.ToDecimal().ToString("C", CoreContext.CurrentCultureInfo),
                    paymentTotal = order.OrderCustomers[0].OrderPayments.Where(p => p.OrderPaymentStatusID != NetSteps.Data.Entities.Constants.OrderPaymentStatus.Cancelled.ToShort() && p.OrderPaymentStatusID != (int)Constants.OrderPaymentStatus.Declined && p.Amount > 0).Sum(p => p.Amount).ToString("C", CoreContext.CurrentCultureInfo),
                    balanceDue = balance.ToString(order.CurrencyID),
                    balanceAmount = balance.GetRoundedNumber(),
                    numberOfItems = (order.OrderCustomers.Sum(oc => oc.ParentOrderItems.Sum(oi => oi.Quantity))),
                    numberofPayment = numberofPayment
                };
            }
        }


        void calculateDispatch()
        {
            var dispatchs = OrderExtensions.GetDispatchByOrder(OrderContext.Order.OrderID);
            foreach (var dispa in dispatchs)
            {
                foreach (var oi in OrderContext.Order.AsOrder().OrderCustomers[0].OrderItems.Where(x => x.OrderItemID == dispa.OrderItemID))
                {
                    RemoverItems(oi.OrderItemID);
                    break;
                }
            }
        }

        void calculateTotalDispatch()
        {
            var dispatchs = OrderExtensions.GetDispatchByOrder(OrderContext.Order.OrderID);
            int orderItems = 0;
            decimal totalOrder = 0;
            if (dispatchs.Count > 0)
            {
                foreach (var dispa in dispatchs)
                {
                    foreach (var oi in OrderContext.Order.AsOrder().OrderCustomers[0].OrderItems.Where(x => x.OrderItemID != dispa.OrderItemID))
                    {
                        if (orderItems == 0)
                        {
                            orderItems = oi.OrderItemID;
                            totalOrder = totalOrder + (oi.ItemPrice * oi.Quantity);
                            break;
                        }
                        else if (orderItems == oi.OrderItemID)
                        {
                            break;
                        }
                        else if (orderItems != oi.OrderItemID)
                        {
                            totalOrder = totalOrder + (oi.ItemPrice * oi.Quantity);
                            break;
                        }
                    }
                }
            }

            OrderContext.Order.Subtotal = totalOrder;
            OrderContext.Order.GrandTotal = totalOrder;
            OrderContext.Order.AsOrder().OrderCustomers[0].AdjustedSubTotal = totalOrder;
            //OrderContext.Order.AsOrder().OrderCustomers[0].ProductSubTotal =  OrderContext.Order.AsOrder().OrderCustomers[0].ProductSubTotal - totalDispatch;

        }

        void RemoverItems(int orderItemId)
        {
            var customer = (OrderCustomer)OrderContext.Order.OrderCustomers.FirstOrDefault();
            var item = customer.OrderItems.FirstOrDefault(oi => oi.OrderItemID == orderItemId);
            int SiteID = ConfigurationManager.GetAppSetting<int>(ConfigurationManager.VariableKey.NSCoreSiteID);
            var dynamicKitGroupId = (item as OrderItem).DynamicKitGroupID;
            int Valuequantity = 0;
            List<OrderItemUpdate> objOrderItemsOld = new List<OrderItemUpdate>();
            Valuequantity = item.Quantity;
            var removeModification = Create.New<IOrderItemQuantityModification>();
            removeModification.Customer = customer;
            removeModification.ModificationKind = OrderItemQuantityModificationKind.Delete;
            removeModification.ProductID = item.ProductID.Value;
            removeModification.Quantity = 0;
            removeModification.ExistingItem = item;
            var changes = new IOrderItemQuantityModification[] { removeModification };
            OrderService.UpdateOrderItemQuantities(OrderContext, changes);
            OrderService.UpdateOrder(OrderContext);
        }

        protected virtual string ShippingMethods
        {
            get
            {

                OrderShipment shipment = OrderContext.Order.AsOrder().OrderShipments[0]; //OrderContext.Order.AsOrder().GetDefaultShipment();

                OrderCustomer customer = OrderContext.Order.AsOrder().OrderCustomers[0];
                var shipmentAdjustmentAmount = customer.ShippingAdjustmentAmount;

                if (shipment != null && customer != null)
                {
                    StringBuilder builder = new StringBuilder();

                    IEnumerable<ShippingMethodWithRate> shippingMethods = null;

                    try
                    {
                        // WV: Si ya se proceso una vez el Shipping no lo vuelve a procesar => Aqui se debe incluir si es bra o usa
                        Boolean vGenShipping = Convert.ToBoolean(Session["vControlGenShipping"]);
                        if (vGenShipping == false)
                        {
                            if (valShiping == "N")
                            {
                                controlGenshipping = true;
                                Session["vControlGenShipping"] = controlGenshipping;
                            }
                            shippingMethods = OrderContext.Order.AsOrder().GetShippingMethods(shipment);
                            if (shippingMethods != null)
                                shippingMethods = shippingMethods.OrderBy(sm => sm.ShippingAmount).ToList();
                            if (shippingMethods != null && !shippingMethods.Select(sm => sm.ShippingMethodID).Contains(shipment.ShippingMethodID.ToInt()) && shippingMethods.Count() > 0)
                            {
                                var cheapestShippingMethod = shippingMethods.First();
                                shipment.ShippingMethodID = cheapestShippingMethod.ShippingMethodID;
                                OrderService.UpdateOrder(OrderContext);
                            }
                            if (shippingMethods != null && shippingMethods.Count() > 0)
                            {
                                foreach (var shippingMethod in shippingMethods)
                                {
                                    Session["DateEstimated"] = shippingMethod.DateEstimated;
                                    builder.Append("<li class=\"AddressProfile\"><input value=\"").Append(shippingMethod.ShippingMethodID)
                                        .Append("\"")
                                        .Append(" id=\"shippingMethod").Append(shippingMethod.ShippingMethodID)
                                        .Append("\"")
                                        .Append(shipment.ShippingMethodID == shippingMethod.ShippingMethodID ? " checked=\"checked\"" : "")
                                        .Append("type=\"radio\" name=\"shippingMethod\" class=\"Radio\" />")
                                        .Append("<label for=\"shippingMethod")
                                        .Append(shippingMethod.ShippingMethodID)
                                        .Append("\"><b class=\"mr10\">")
                                        .Append(shippingMethod.Name)
                                        .Append("</b>");

                                    if (shipmentAdjustmentAmount != 0)
                                    {
                                        builder.Append("<span class=\"shipMethodPrice originalPrice strikethrough\">")
                                            .Append(shippingMethod.ShippingAmount.ToString(OrderContext.Order.AsOrder().CurrencyID))
                                            .Append("</span>&nbsp;")
                                            .Append("<span class=\"shipMethodPrice discountPrice\">")
                                            .Append((shippingMethod.ShippingAmount - shipmentAdjustmentAmount).ToString(OrderContext.Order.AsOrder().CurrencyID))
                                            .Append("</span>");
                                    }
                                    else
                                    {
                                        builder.Append(shippingMethod.ShippingAmount.ToString(OrderContext.Order.AsOrder().CurrencyID))
                                        .Append("</label></li>");
                                    }
                                    Session["vbuilder"] = builder.ToString();
                                }
                            }
                            else
                            {
                                builder.Append("<li class=\"AddressProfile\">").Append(Translation.GetTerm("NoShippingRatesAvailable", "No shipping rates available. Please enter a shipping address if one is not yet added to order.")).Append("</li>");
                            }
                            return builder.ToString();
                        }
                        else
                        {
                            builder.Append(Session["vbuilder"]);
                            return builder.ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        var productShippingExcludedShippingException = ex as ProductShippingExcludedShippingException;
                        if (productShippingExcludedShippingException != null)
                        {
                            builder.Append("<li class=\"AddressProfile\"><div>").Append(Translation.GetTerm("InvalidShippingForProducts", "No available shipping methods, try changing your shipping address, updating your order subtotal or call customer service.")).Append("</div><ul>");

                            foreach (var product in (productShippingExcludedShippingException).ProductsThatHaveExcludedShipping)
                                builder.Append(string.Format("<li>{0}</li>", product.Name));

                            builder.Append("</ul></li>");
                            return builder.ToString();
                        }

                        throw new Exception(ex.Message, ex);
                    }
                }
                return null;
            }
        }

        public InventoryBaseRepository Inventory { get { return Create.New<InventoryBaseRepository>(); } }
        public IShippingCalculator ShippingCalculator { get { return Create.New<IShippingCalculator>(); } }
        private int _availableBundleCount { get; set; }

        #endregion

        #region Helper Methods

        protected virtual object GetApplicablePromotions(IOrderContext orderContext)
        {
            var promotionAdjustments = orderContext.Order.AsOrder().OrderAdjustments.Where(adjustment => adjustment.ExtensionProviderKey == PromotionProvider.ProviderKey);
            var adjustments = promotionAdjustments.Where(adjustment => adjustment.OrderAdjustmentOrderLineModifications.Any() || adjustment.OrderAdjustmentOrderModifications.Any() || adjustment.InjectedOrderSteps.Any());
            var inventoryService = Create.New<IInventoryService>();
            return adjustments.Select(adj =>
            {

                var giftStep = adj.InjectedOrderSteps.FirstOrDefault(os => os is IUserProductSelectionOrderStep &&
                    (os.Response == null || (os.Response is IUserProductSelectionOrderStepResponse && (os.Response as IUserProductSelectionOrderStepResponse).SelectedOptions.Count == 0)));
                var promotionOutOfStock = false;
                if (giftStep == null)
                {
                    promotionOutOfStock = adj.OrderAdjustmentOrderModifications.Any(mod => mod.ModificationOperationID == (int)OrderAdjustmentOrderOperationKind.Message);
                }
                else
                {
                    var productSelectionStep = (IUserProductSelectionOrderStep)giftStep;
                    promotionOutOfStock = !(productSelectionStep.AvailableOptions.Any(option => inventoryService.GetProductAvailabilityForOrder(orderContext, option.ProductID, option.Quantity).CanAddNormally == option.Quantity));
                }
                return new { Description = adj.Description, StepID = giftStep == null ? null : giftStep.OrderStepReferenceID.ToString(), PromotionOutOfStock = promotionOutOfStock };
            }
            );
        }

        protected virtual IEnumerable<string> GetOutOfStockItemsString(Order order)
        {
            return order.OrderCustomers.SelectMany(oi => oi.ParentOrderItems)
                .Where(oi => Inventory.IsOutOfStock(Inventory.GetProduct(oi.ProductID.Value))/* && !Inventory.IsAvailable(oi.ProductID ?? 0)*/ && !oi.HasChildOrderItems)
                .Select(oi => oi.SKU + " - " + oi.ProductName);
        }

        protected virtual void UpdateOrderShipmentAddress(OrderShipment shipment, int addressId)
        {
            OrderContext.Order.AsOrder().UpdateOrderShipmentAddress(shipment, addressId);
            OrderService.UpdateOrder(OrderContext);
        }

        protected virtual IEnumerable<ShippingMethodWithRate> GetNewShippingMethods(int addressId)
        {
            var shippingMethods = OrderContext.Order.AsOrder().UpdateOrderShipmentAddressAndDefaultShipping(addressId);
            OrderService.UpdateOrder(OrderContext);

            return shippingMethods;
        }

        protected virtual List<IDisplayInfo> GetPromotionsDisplayInfo()
        {
            return new List<IDisplayInfo>();
        }

        protected virtual string GetPromotionsHtml(Order order)
        {
            List<IDisplayInfo> promoDisplayInfos = GetPromotionsDisplayInfo();
            string promotionsHtml = RenderRazorPartialViewToString("~/Views/Promotions/_AppliedPromotions.cshtml", promoDisplayInfos);
            return promotionsHtml;
        }

        /// <summary>
        /// Allows client-specific code to override whether or not an order can 
        /// be shipped to an address that is outside the distributor's market
        /// </summary>
        /// <returns></returns>
        public virtual bool IsDistributorMarketSameAsShippingAddressMarket()
        {
            return true;
        }
        #endregion

        public ActionResult OrderLock()
        {
            return View();
        }

        [FunctionFilter("Orders", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public virtual ActionResult Index(short? orderTypeID, bool? gd, int? orderID, string promotionCode, bool newOrder = false)
        {
            var account = CurrentAccount;
            /*CS.11JUL2016.Inicio.Validar Bloqueo de Consultora */
            List<AccountBlocking> blockingSts = new List<AccountBlocking>();
            blockingSts = NetSteps.Data.Entities.Account.AccountBlockingStatus(account.AccountID, "BLKORDE");
            if (blockingSts.Count > 0)
            {
                if (blockingSts[0].Status == true)
                {
                    //return Json(new { result = false, message = blockingSts[0].Description });
                    MessageHandler.AddError(blockingSts[0].Description);
                    return RedirectToAction("Index", "OrderHistory");
                }
            }
            /*CS.11JUL2016.Fin.Validar Bloqueo de Consultora*/

            /*CS.09JUL2016.Inicio.Retoma de Primer Pedido a PWS*/
            if (account != null && account.AccountStatusID == (short)NetSteps.Data.Entities.Constants.AccountStatus.BegunEnrollment && account.EnrollmentDateUTC == null)
            {
                Tuple<int, int, int> result = new OrderBusinessLogic().CheckOrdersByAccountID(account.AccountID);
                var otherOrders = result.Item2;
                if (otherOrders == 0)
                {
                    string token = "";
                    token = NetSteps.Data.Entities.Account.GetSingleSignOnToken(account.AccountID);

                    if (token != "")
                    {
                        Response.Redirect(ProxyLink.Load(2).URL + "?token=" + token + "&kitInicio=true");
                    }
                }
            }
            /*CS.09JUL2016.Fin.Retoma de Primer Pedido a PWS*/

            try
            {
                //var parames = OrderExtensions.GetGeneralParameterVEA();
                Session["ParameterVEA"] = OrderExtensions.GetGeneralParameterVEA();
                //ViewBag.EnableShippingSel = OrderExtensions.GeneralParameterVal(Convert.ToInt32(CoreContext.CurrentMarketId), "CSA");
                //ViewBag.EnableShippingAdd = OrderExtensions.GeneralParameterVal(Convert.ToInt32(CoreContext.CurrentMarketId), "ESA");
                Session.Remove("listDispatChProducts");
                ValidatePaymentByMarket();
                ViewBag.Cancel = true;
                Session["vbuilder"] = null;
                Session["vControlGenShipping"] = null;
                // wv: 20160504 => Determina si el pais realiza o no validacion x linea del shipping en el pedido
                valShiping = OrderExtensions.GeneralParameterVal(Convert.ToInt32(CoreContext.CurrentMarketId), "SHP");
                if (string.IsNullOrEmpty(valShiping))
                {
                    valShiping = "N";
                }
                //Definicion del periodo

                Dictionary<int, bool> result = new Dictionary<int, bool>();
                result = Periods.GetPeriodByDate(DateTime.Now);
                bool bloqueadoPorPeriodo = result.ElementAt(0).Value;
                ViewBag.bloqueadoPorPeriodo = bloqueadoPorPeriodo;
                if (bloqueadoPorPeriodo)
                {
                    return RedirectToAction("OrderLock", "OrderEntry");
                }
                ViewBag.Period = result.ElementAt(0).Key;


                NetSteps.Data.Entities.Business.CTE objN = new NetSteps.Data.Entities.Business.CTE();
                // Load requested order.
                if (orderID.HasValue && (OrderContext.Order == null || OrderContext.Order.OrderID != orderID.Value))
                {
                    var requestedOrder = Order.LoadFull(orderID.Value);
                    if (!CurrentAccountCanAccessOrder(requestedOrder)) RedirectToSafePage();
                    OrderContext.Order = requestedOrder;
                }
                //var siteId = CurrentAccount.Sites[0].SiteID;
                int SiteIDs = 2;
                string orderNumber = Order.GetOrderPending(CurrentAccount.AccountID, SiteIDs);
                if (orderNumber != "")
                {
                    ViewBag.SessionPreOrderId = Order.GetPreOrderPending(orderNumber, SiteIDs);
                    Session["PreOrder"] = Order.GetPreOrderPending(orderNumber, SiteIDs);
                    var preorder = Session["PreOrder"];
                    return RedirectToAction("Edit", new { orderNumber = orderNumber });
                }
                List<PreOrderCondition> orderCondition = Order.GetPreOrderConditions(CurrentAccount.AccountID, CoreContext.CurrentLanguageID);
                foreach (var oCondition in orderCondition)
                {
                    if (!oCondition.Exist)
                    {
                        TempData["Error"] = oCondition.Descriptions;
                        return RedirectToAction("Index", "OrderHistory");
                    }
                }
                //csti-mescobar-EB433-20160218
                //Se comento este codigo ya que al actualizar la pagina donde se agrega los productos 
                //esto me envia a la pagina index.No se supo porque razon realizo se agrego este codigo.Lo dejo comentado temporalmente.
                //if (OrderContext.Order != null && OrderContext.Order.OrderCustomers[0].AccountTypeID == (short)ConstantsGenerated.AccountType.Distributor)
                //{
                //    string messageOutput;
                //    //if (! Order.ValidateOrderRulesPreCondition(OrderContext.Order.OrderCustomers[0].AccountID, out messageOutput))
                //    //{
                //    //TempData["Error"] = "You cannot create an order, because you have exceeded the maximum order without payment";
                //    //TempData["Error"] = messageOutput;
                //    return RedirectToAction("Index", "OrderHistory");
                //    //}
                //}
                int SiteID = 2; // ConfigurationManager.GetAppSetting<int>(ConfigurationManager.VariableKey.ns);

                if (newOrder || OrderContext.Order == null || !OrderContext.Order.OrderCustomers.Any() || OrderContext.Order.OrderCustomers[0].AccountID != CurrentAccountId)
                {
                    OrderContext.Clear();
                    short newOrderTypeID = orderTypeID ?? (short)NetSteps.Data.Entities.Constants.OrderType.WorkstationOrder;


                    OrderContext.Order = new Order(CurrentAccount, newOrderTypeID);

                    OrderContext.Order.AsOrder().SiteID = CurrentSite.SiteID;
                    OrderContext.Order.AsOrder().DateCreated = DateTime.Now;
                    OrderContext.Order.AsOrder().CurrencyID = SmallCollectionCache.Instance.Markets.GetById(CoreContext.CurrentMarketId).GetDefaultCurrencyID();
                    // OrderContext.Order.AsOrder().OrderTypeID = newOrderTypeID;
                    OrderContext.Order.OrderTypeID = NetSteps.Data.Entities.Constants.OrderType.PortalOrder.ToShort(); ////  yo  puse

                    //OrderContext.Order.AsOrder().SetConsultantID(CurrentAccount);
                    // Due to the single page flow of order entry in Core we must always calculate shipping and taxes.
                    OrderContext.Order.AsOrder().OrderPendingState = NetSteps.Data.Entities.Constants.OrderPendingStates.Quote;
                    Session["PreOrder"] = Order.getPreOrder(account.AccountID, SiteID, null);
                }
                else
                {
                    Session["PreOrder"] = Order.GetProOrderUpdate(account.AccountID, SiteID);
                    var preOrder = Order.GetProOrderUpdate(account.AccountID, SiteID);
                } 
                int wareHouseId = WarehouseExtensions.WareHouseByAddresID(CurrentAccount.AccountID);
                Session["WareHouseId"] = wareHouseId; 
                OrderContext.Order.ParentOrderID = Convert.ToInt32(Session["WareHouseId"]);
                OrderContext.Order.WareHouseID = Convert.ToInt32(Session["WareHouseId"]);

                Address defaultShippingAddress = CurrentAccount.Addresses.GetDefaultByTypeID(NetSteps.Data.Entities.Constants.AddressType.Shipping);
                ViewData["ShippingMethods"] = defaultShippingAddress != null ? GetNewShippingMethods(defaultShippingAddress.AddressID) : new List<ShippingMethodWithRate>();
                var shippingMethodWithRates = ViewData["ShippingMethods"] as IEnumerable<ShippingMethodWithRate>;
                Session["ExisShippingMetods"] = true;
                if (shippingMethodWithRates.Count() == 0)
                {
                    Session["ExisShippingMetods"] = false;
                }

                Session["Edit"] = "0";
                if (!String.IsNullOrWhiteSpace(promotionCode))
                {
                    try
                    {
                        ApplyPromotionCode(promotionCode);
                    }
                    catch (Exception ex)
                    {
                        var message = ex.Log(accountID: CurrentAccountId, orderID: OrderContext.Order.OrderID).PublicMessage;
                        MessageHandler.AddError(message);
                    }
                }

                OrderService.UpdateOrder(OrderContext);

                if (ApplicationContext.Instance.UseDefaultBundling)
                {
                    ViewBag.DynamicKitUpSaleHTML = GetDynamicBundleUpSale();
                    ViewBag.AvailableBundleCount = this._availableBundleCount;
                }

                //Address defaultShippingAddress = CurrentAccount.Addresses.GetDefaultByTypeID(NetSteps.Data.Entities.Constants.AddressType.Shipping);
                //ViewData["ShippingMethods"] = defaultShippingAddress != null ? GetNewShippingMethods(defaultShippingAddress.AddressID) : new List<ShippingMethodWithRate>();

                var categoryHelper = Create.New<ICategoryHelper>();

                //ViewBag.Categories = categoryHelper.GetDwsActiveCategories(CurrentAccount).ToList();/*CS:04AB2016:Comenté línea*/
                SetProductCreditViewData();

                if (gd.GetValueOrDefault())
                {
                    ProductPriceType productPriceType = AccountPriceType.GetPriceType(CurrentAccount.AccountTypeID, NetSteps.Data.Entities.Generated.ConstantsGenerated.PriceRelationshipType.Products, OrderContext.Order.AsOrder().OrderTypeID);
                    OrderShipment orderShipment = OrderContext.Order.AsOrder().OrderShipments.FirstOrDefault();
                    var outOfStockProducts = Product.GetOutOfStockProductIDs(orderShipment);
                    var products = Inventory.SearchProducts(NetSteps.Data.Entities.ApplicationContext.Instance.StoreFrontID, String.Empty, CurrentAccount.AccountTypeID, startsWith: "", productPriceTypeID: productPriceType.ProductPriceTypeID.ToIntNullable())
                        .Where(p => !outOfStockProducts.Contains(p.ProductID) || p.ProductBackOrderBehaviorID != (int)NetSteps.Data.Entities.Constants.ProductBackOrderBehavior.Hide).ToList();
                    ViewData["Products"] = products;
                }
                else
                {
                    ViewData["Products"] = null;
                }
                foreach (var item in GetNewShippingMethods(defaultShippingAddress.AddressID).Where(x => x.ShippingMethodID == OrderContext.Order.AsOrder().OrderShipments.FirstOrDefault().ShippingMethodID))
                {
                    Session["DateEstimated"] = item.DateEstimated;
                }
                var localizationInfo = Create.New<ILocalizationInfo>();
                localizationInfo.CultureName = CoreContext.CurrentCultureInfo.Name;
                localizationInfo.LanguageId = CoreContext.CurrentLanguageID;
                ViewBag.ExistingPromoDisplayInfos = GetPromotionsDisplayInfo();
                var promotions = new List<IDisplayInfo>();
                var promotionService = Create.New<IPromotionService>();
                var accountPromotions = promotionService.GetPromotions(x => x.ValidFor<int>("AccountID", CurrentAccount.AccountID));
                foreach (var promotion in accountPromotions)
                {
                    if (!promotion.PromotionQualifications.Values.Any(qualification =>
                        typeof(IPromotionCodeQualificationExtension).IsAssignableFrom(qualification.GetType())
                        && OrderContext.CouponCodes.Any(addedCoupon => CurrentAccount.AccountID == addedCoupon.AccountID
                            && (((IPromotionCodeQualificationExtension)qualification).PromotionCode.Equals(addedCoupon.CouponCode, StringComparison.InvariantCultureIgnoreCase)))))
                    {
                        IPromotionInfoService promoInfoService = Create.New<IPromotionInfoService>();
                        promotions.AddRange(promoInfoService.GetPromotionDisplayInfo(new List<NetSteps.Promotions.Common.Model.IPromotion>() { promotion }, localizationInfo));
                    }
                }

                //========================================================================================
                TempData["sPaymentMethod"] = from x in Order.SelectPaymentMethod(CurrentAccount.AccountID, OrderContext.Order.AsOrder().OrderTypeID)
                                             orderby x.Key
                                             select new SelectListItem()
                                             {
                                                 Text = x.Value,
                                                 Value = x.Key
                                             };
                //========================================================================================

                ViewData["Promotions"] = promotions;
                ViewData["AvailablePromotionsList"] = GetAvailablePromotionsList();
                //ViewBag.BulkAddModel = GetBulkAddModel(); /*CS:04AB2016:Comenté línea*/
                ViewBag.BulkAddModel = GetBulkAddModel2();
                //int SiteID = Convert.ToInt32(ConfigurationManager.GetAppSetting<int>(ConfigurationManager.VariableKey.NSCoreSiteID));
                //Session["PreOrder"] = Order.getPreOrder(CurrentAccount.AccountID, SiteID, null);
                ApplyPaymentPreviosBalance();//  se coloca en este orden , ya que estan generando el preorderid a este nivel
                actBalanceDue();
                var model = Index_CreateModel(OrderContext.Order.AsOrder());
                Session["AccountId"] = CurrentAccount.AccountID;
                ViewBag.AccountTypeId = Convert.ToInt32(CurrentAccount.AccountTypeID);

                OrderCustomer orderCustomer = OrderContext.Order.AsOrder().OrderCustomers[0];
                /*
                 * wv:20160606 Validacion de los Dispatch para ser asignados al account
                 */
                //int edicion = Convert.ToInt32(Session["Edit"]);
                //if (edicion == 0)
                //{
                //    List<NetSteps.Data.Entities.Business.HelperObjects.SearchData.DispatchProducts> itemsProductsDispatch = new List<NetSteps.Data.Entities.Business.HelperObjects.SearchData.DispatchProducts>();
                //    var lstProductsVal = (List<DispatchProducts>)Session["listDispatChProducts"];
                //    if (!(lstProductsVal != null))
                //    {
                //        itemsProductsDispatch = NetSteps.Data.Entities.Order.getDispatchProducts(CurrentAccount.AccountID, 0, CurrentAccount.AccountTypeID, Convert.ToInt32(Session["WareHouseId"]), Convert.ToInt32(Session["PreOrder"]), 1, 22, false, true, false);
                //        Session["listDispatChProducts"] = itemsProductsDispatch;
                //    }
                //}


                //var productClains = GetClains(CurrentAccount.AccountID);
                //if (productClains != null)
                //{
                //    if (productClains.Count > 0)
                //    {
                //        Session["IsClains"] = true;
                //        Session["OrderClaimID"] = productClains[0].OrderClaimID;
                //        //Session["ProductClains"] = GetClains(account.AccountID);
                //    }
                //    else
                //    {
                //        Session["IsClains"] = false;
                //    }
                //}
                //Session["WareHouseId"] = 1;
                CalculateTotal(); /*CS:04AB2016:Comenté línea*/
                OrderShipment shipment = OrderContext.Order.AsOrder().GetDefaultShipment();
                // ------------------------------------------------------------------------------------------------------------
                // Valida si ya se tiene la lista de los productos Restringidos, en el caso de no tenerlos los carga para 
                // validarlos en la captura de de cada producto.
                // wv: 20160422

                //if (Session["listProductRestrictionxAccount"] == null)
                //{
                //    List<ProductsRestriction> lstProdRestr = new List<ProductsRestriction>();
                //    List<ListProducts> listProductsRes = new List<ListProducts>();
                //    List<int> tmplistRestriction = new List<int>();

                //    lstProdRestr = ProductQuotasRepository.getProductRestriction(CurrentAccount.AccountID, Convert.ToInt32(ViewBag.Period));
                //    int aplica = 0;
                //    if (lstProdRestr != null)
                //    {
                //        var DistinctItems = lstProdRestr.GroupBy(x => x.RestrictionID).Select(y => y.First());

                //        foreach (var fill in DistinctItems)
                //        {
                //            aplica = 0;
                //            foreach (var tmpValor in lstProdRestr.Where(x => x.RestrictionID == fill.RestrictionID))
                //            {
                //                if (tmpValor.AccountTypeID == 0)
                //                {
                //                    aplica = 1;
                //                    break;
                //                }
                //                else
                //                {
                //                    if (tmpValor.AccountTypeID == ViewBag.AccountTypeId)
                //                    {
                //                        aplica = 1;
                //                        break;
                //                    }

                //                }
                //            }
                //            if (aplica == 1)
                //            {
                //                foreach (var tmpValor in lstProdRestr.Where(x => x.RestrictionID == fill.RestrictionID))
                //                {
                //                    if (tmpValor.TitleID == 0)
                //                    {
                //                        aplica = 1;
                //                        break;
                //                    }
                //                    else
                //                    {
                //                        if (tmpValor.TitleTypeID == 1)
                //                        {
                //                            if (tmpValor.TitleID == tmpValor.TituloPago)
                //                            {
                //                                aplica = 1;
                //                                break;
                //                            }
                //                            else
                //                            {
                //                                if (tmpValor.TitleID == tmpValor.TituloCarrera)
                //                                {
                //                                    aplica = 1;
                //                                    break;
                //                                }
                //                            }
                //                        }
                //                    }
                //                }
                //                if (aplica == 1)
                //                {
                //                    tmplistRestriction.Add(fill.RestrictionID);
                //                }
                //            }
                //        }

                //        if (tmplistRestriction.Count > 0)
                //        {
                //            foreach (var tmpRestr in tmplistRestriction.Distinct())
                //            {
                //                var lista = ProductQuotasRepository.getRestrictionxAccountProduct(tmpRestr, Convert.ToInt32(ViewBag.Period), CurrentAccount.AccountID);
                //                if (lista != null)
                //                { listProductsRes.Add(lista[0]); }
                //            }
                //            try
                //            {
                //                if (listProductsRes[0].ProductID != 0)
                //                {
                //                    Session["listProductRestrictionxAccount"] = listProductsRes;
                //                }

                //            }
                //            catch
                //            {
                //                // No hace nada;
                //            }
                //        }
                //    }
                //}
                // ------------------------------------------------------------------------------------------------------------

                return View(model);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, (OrderContext.Order != null) ? OrderContext.Order.OrderID.ToIntNullable() : null, accountID: (CurrentAccount != null) ? CurrentAccount.AccountID.ToIntNullable() : null);
                TempData["Error"] = exception.Message;
                return RedirectToAction("Index", "OrderHistory");
            }
        }

        private List<ProductNameClains> GetClains(int accountId)
        {
            //Found ClainsItems

            var pClains = OrderExtensions.GetProductClains(accountId);
            return pClains;
        }

        public ActionResult ValidPostalCode(string postalCode)
        {
            try
            {
                if (!OrderExtensions.GetStatesByAccountID(OrderContext.Order.OrderCustomers[0].AccountID, postalCode))
                {

                    return Json(new { result = false });
                }
                else
                {

                    return Json(new { result = true });
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        private void ValidatePaymentByMarket()
        {
            // N : BRASIL
            // S : USA
            Session["GeneralParameterVal"] = OrderExtensions.GeneralParameterVal(CoreContext.CurrentMarketId, "ANM");

        }

        //public ActionResult CargarOpenLista()
        //{
        //    var categoryHelper = Create.New<ICategoryHelper>();
        //    ViewBag.Categories = categoryHelper.GetDwsActiveCategories(CurrentAccount).ToList();
        //    ViewBag.BulkAddModel = GetBulkAddModel();

        //    return PartialView("AddBulkOrder", (DistributorBackOffice.Models.IBulkAddModel)ViewBag.BulkAddModel);
        //}

        public virtual ActionResult GetPreOrder(int? accountId)
        {
            int SiteID = ConfigurationManager.GetAppSetting<int>(ConfigurationManager.VariableKey.DistributorWorkstationUrl);
            int PreOrderId = Order.GetPreOders(Convert.ToInt32(Session["AccountId"]), SiteID);
            if (PreOrderId > 0)
            {
                return Json(new { result = true });
            }
            else
            {
                return Json(new { result = false });
            }
        }

        private void actBalanceDue()
        {

            var comisionableTotal = Totals.GetType().GetProperty("balanceAmount").GetValue(Totals, null);

            decimal val_payments = 0;

            var KeyDecimals = ConfigurationManager.AppSettings["CultureDecimal"];
            if (KeyDecimals == "ES")
            {
                var culture = CultureInfoCache.GetCultureInfo("En");

                try
                {
                    //  valor positivo

                    //val_payments = Convert.ToDecimal(comisionableTotal, culture);
                    val_payments = NetSteps.Data.Entities.Extensions.DecimalExtensions.FormatGlobalizationDecimal(comisionableTotal.ToString());

                }
                catch
                {
                    //  valor negativo
                    var d = comisionableTotal.ToString().Substring(0, comisionableTotal.ToString().Length - 1);
                    //val_payments = Convert.ToDecimal(d, culture);
                    val_payments = NetSteps.Data.Entities.Extensions.DecimalExtensions.FormatGlobalizationDecimal(d.ToString());
                    val_payments = val_payments * (-1);
                }

            }
            OrderContext.Order.AsOrder().Balance = val_payments;
        }
        /*
        public virtual ActionResult IsCreditCard(int CollectionEntityID)
        {
            var IsTarget = NetSteps.Data.Entities.Business.PaymentMethods.IsTargetCreditByPaymentConfiguration(CollectionEntityID);
            var numberTarget = NetSteps.Data.Entities.Business.PaymentMethods.GetNumberCuotasByPaymentConfigurationID(CollectionEntityID);
            int PaymentTypeID = Order.GetApplyPaymentType(CollectionEntityID);
            if (!IsTarget)
            {
                if (PaymentTypeID == 11 && OrderContext.Order.OrderStatusID == 1)
                {
                    return Json(new { result = true, IsBoleta = true, totals = Totals }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { result = true, numberTarget = numberTarget }, JsonRequestBehavior.AllowGet);
            }
        }
        */
        private int IsCreditCard(int PaymentConfigurationID)
        {

            var IsTarget = NetSteps.Data.Entities.Business.PaymentMethods.IsTargetCreditByPaymentConfiguration(PaymentConfigurationID);
            var numberTarget = NetSteps.Data.Entities.Business.PaymentMethods.GetNumberCuotasByPaymentConfigurationID(PaymentConfigurationID);
            int PaymentTypeID = Order.GetApplyPaymentType(PaymentConfigurationID);
            if (IsTarget)
            {
                return numberTarget;
            }
            else
            {
                return 0;
            }

        }

        [OutputCache(CacheProfile = "DontCache")]
        //[FunctionFilter("Orders-Order Entry", "~/Orders")]
        public virtual ActionResult CanEditShippingAddress()
        {
            string canEdit = OrderExtensions.GeneralParameterVal(CoreContext.CurrentMarketId, "CSA");
            return Json(new { result = canEdit });
        }

        #region CSTI - FHP (PreOrder)

        //public int Period(DateTime date)
        //{
        //    return Order.ListPeriod(dataBase, date);
        //}
        //public int CreatePreOrder(int AccountID, int SiteID)
        //{
        //    Session["AccountID"] = AccountID;
        //    OrderType = "PreOrder";
        //    wareHouseId = Order.WareHouseXAddresID(dataBase, AccountID);
        //    getShippingDate = GetShippingMethods(wareHouseId, 1);
        //    return Order.CreatePreOrder(dataBase, AccountID, SiteID);

        //}

        private int ValidateAccountCredit(int PaymentConfiguration)
        {
            Order order = OrderContext.Order.AsOrder();
            int resp = 0;
            LoadAccountCredit();
            var tabl = NetSteps.Data.Entities.Business.CTE.creditPayment;
            decimal? ValorComparacion = 0;

            var AfectaCredito = AccountPropertiesBusinessLogic.GetValueByID(2, PaymentConfiguration).PaymentCredit;

            if (AfectaCredito == "S")
            {
                NetSteps.Data.Entities.Business.CTE.creditPayment.AfectaCredito = "S";
                if (tabl.EstadoCredito.Equals("S"))
                {
                    resp = 1;
                }

                if (tabl.EstadoCredito.Equals("N"))
                {
                    switch (tabl.TipoCredito)
                    {
                        case "P":
                            ValorComparacion = order.QualificationTotal.ToDecimal();
                            break;
                        case "D":
                            ValorComparacion = order.GrandTotal.ToDecimal();// + CreditBalanceDue =  Balance
                            break;
                        default:
                            break;

                    }

                    NetSteps.Data.Entities.Business.CTE.creditPayment.ValorComparacion = ValorComparacion.ToString().Replace(",", ".");
                    if (tabl.CreditoDisponible.ToDecimal() < ValorComparacion)
                        resp = 2;
                }
            }
            else
            {
                NetSteps.Data.Entities.Business.CTE.creditPayment.AfectaCredito = "N";
            }
            return resp;
        }

        private void LoadAccountCredit()
        {
            var rep = AccountPropertiesBusinessLogic.GetValueByID(4, CurrentAccount.AccountID);
            var tb = new CreditPaymentTable()
            {
                //Posibles valores P = Puntos , D= Dinero
                TipoCredito = OrderExtensions.GeneralParameterVal(CoreContext.CurrentMarketId, "TPC"),
                ValorComparacion = "0",
                CreditoDisponible = (rep == null) ? "0.00" : rep.AccountCreditDis.ToString(),
                EstadoCredito = (rep == null) ? "N" : rep.AccountCreditEst

            };

            NetSteps.Data.Entities.Business.CTE.creditPayment = tb;

            NetSteps.Data.Entities.Business.CTE.creditPayment.CreditoDisponible.Replace(",", ".");

        }

        public void CalculateTotal()
        {
            try
            {
                LoadAccountCredit();
                var comisionableTotal = Totals.GetType().GetProperty("commissionableTotal").GetValue(Totals, null);
                string[] separatorCommissionable = Convert.ToString(comisionableTotal).Split('$');
                ViewBag.CreditAvailable = NetSteps.Data.Entities.Business.CTE.creditPayment.CreditoDisponible;// Order.GetCredit().ToString('S');
                ViewBag.EstadoCredito = NetSteps.Data.Entities.Business.CTE.creditPayment.EstadoCredito;
                ViewBag.CommisionableTotal = Convert.ToDecimal(separatorCommissionable[1]).ToString('S');
                ViewBag.QualificationTotal = Convert.ToDecimal(separatorCommissionable[1]).ToString('S');
                Session["ProductCredit"] = Order.GetProductCreditByAccount(OrderContext.Order.AsOrder().ConsultantID);//Convert.ToInt32(Session["AccountId"]));
                var ProductCredit = NetSteps.Data.Entities.Extensions.DecimalExtensions.FormatGlobalizationDecimal(Convert.ToDecimal(Session["ProductCredit"]));

                ViewBag.ProductCredit = ProductCredit;// (ProductCredit < 0) ? (ProductCredit * -1).ToString('S') : ProductCredit.ToString('S');
                ViewBag.ProductCreditStatus = ProductCredit;

                //ViewBag.ProductCredit = Order.GetProductCreditByAccount(Convert.ToInt32(Session["AccountId"])).ToString('S');


            }
            catch (Exception)
            {

            }
        }

        private void CalculateQualificationTotal()
        {
            decimal sum = 0;
            foreach (var item in OrderContext.Order.AsOrder().OrderCustomers[0].OrderItems)
            {
                foreach (var price in item.OrderItemPrices)
                {
                    if (price.ProductPriceTypeID == 21) sum += price.UnitPrice * item.Quantity;
                }
            }
            OrderContext.Order.AsOrder().QualificationTotal = sum;
        }

        public DateTime GetShippingMethods(int WareHouse, int PostalCode)
        {
            return DateTime.Now;
        }
        #endregion

        public virtual IBulkAddModel GetBulkAddModel(int? categoryID = null)
        {
            var model = Create.New<DistributorBackOffice.Models.IBulkAddModel>();
            //set options
            model.GetProductsUrl = Url.Action("GetCategoryProducts");
            model.AddProductsUrl = Url.Action("BulkAddToCart");
            //get data
            model.Data = GetBulkAddModelData(categoryID);
            return model;
        }

        public virtual IBulkAddModel GetBulkAddModel2(int? categoryID = null)
        {
            var model = Create.New<DistributorBackOffice.Models.IBulkAddModel>();
            //set options
            model.GetProductsUrl = Url.Action("GetCategoryProducts");
            model.AddProductsUrl = Url.Action("BulkAddToCart");
            //get data
            var data = Create.New<DistributorBackOffice.Models.IBulkAddModelData>();
            model.Data = GetBulkAddModelData2(categoryID);
            return model;
        }

        protected virtual IBulkAddModelData GetBulkAddModelData(int? categoryID = null)
        {
            var data = Create.New<DistributorBackOffice.Models.IBulkAddModelData>();
            var helper = Create.New<ICategoryHelper>();
            var categories = helper.GetDwsActiveCategories(CurrentAccount).Select(c =>
            {
                var category = Create.New<DistributorBackOffice.Models.ICategoryInfoModel>();
                category.CategoryID = c.CategoryID;
                category.Name = c.Translations.Name();

                return category;
            });
            data.Categories = categories.ToList();
            int selectedCategory = categoryID.HasValue ? categoryID.Value : (categories.Any() ? categories.First().CategoryID : 0);
            data.Products = GetProductsForCategory(selectedCategory, CurrentAccount.AccountTypeID, OrderContext.Order.CurrencyID, OrderContext.Order.OrderTypeID);

            return data;
        }

        protected virtual IBulkAddModelData GetBulkAddModelData2(int? categoryID = null)
        {
            var data = Create.New<DistributorBackOffice.Models.IBulkAddModelData>();
            var helper = Create.New<ICategoryHelper>();
            var categories = helper.GetDwsActiveCategories(CurrentAccount).Select(c =>
            {
                var category = Create.New<DistributorBackOffice.Models.ICategoryInfoModel>();
                category.CategoryID = c.CategoryID;
                category.Name = c.Translations.Name();

                return category;
            });
            data.Categories = categories.ToList();
            //int selectedCategory = categoryID.HasValue ? categoryID.Value : (categories.Any() ? categories.First().CategoryID : 0);
            //data.Products = GetProductsForCategory(selectedCategory, CurrentAccount.AccountTypeID, OrderContext.Order.CurrencyID, OrderContext.Order.OrderTypeID);

            return data;
        }

        List<IBulkProductInfoModel> GetProductsForCategory(int categoryID, int accountTypeId, int currencyId, int orderTypeID)
        {
            var helper = Create.New<ICategoryHelper>();

            return helper.GetValidProductsForCategory(categoryID, accountTypeId, currencyId).OrderBy(o => o.SKU)
                .Select(p =>
                {
                    var prod = Create.New<DistributorBackOffice.Models.IBulkProductInfoModel>();
                    prod.Name = p.Name;
                    prod.Price = p.Prices.First(pp => pp.ProductPriceTypeID == AccountPriceType.GetPriceType(accountTypeId, NetSteps.Data.Entities.Constants.PriceRelationshipType.Products, orderTypeID).ProductPriceTypeID && pp.CurrencyID == currencyId).Price.ToString(currencyId);
                    prod.ProductID = p.ProductID;
                    prod.Quantity = 0;
                    prod.SKU = p.SKU;
                    return prod;
                }).ToList();
        }

        [NonAction]
        public virtual void SetProductCreditViewData()
        {
            var productCreditBalance = _productCreditLedgerService.GetCurrentBalanceLessPendingPayments(CurrentAccount.AccountID);
            ViewData["ProductCreditBalance"] = productCreditBalance.ToMoneyString();
        }

        [NonAction]
        public virtual string GetDynamicBundleUpSale()
        {
            var order = OrderContext.Order.AsOrder();
            if (order == null)
            {
                return string.Empty;
            }
            var customer = order.OrderCustomers[0];
            if (customer.IsTooBigForBundling())
            {
                return string.Empty;
            }

            var possibleDynamicKitProducts = order.GetPotentialDynamicKitUpSaleProducts(customer, OrderContext.SortedDynamicKitProducts).ToList();
            this._availableBundleCount = possibleDynamicKitProducts.Count();

            var sb = new StringBuilder();
            var sbi = new StringBuilder();

            for (int i = 0; i < _availableBundleCount; i++)
            {
                Guid guid = Guid.NewGuid();
                var newID = guid.ToString("N");

                //truncate += product.Translations.Name() + ", ";
                if (possibleDynamicKitProducts.Count() <= 1 || i != 0)
                {
                    continue;
                }
                //sb.Append("<a href=\"#\" id=\"BundleTrigger" + newID + "\" class=\"jqModal bold\">");
                //sb.Append(possibleDynamicKitProducts.Count() + " bundles available");
                //sb.Append("</a>");

                //sbi.Append("<div id=\"BundleModal" + newID + "\" class=\"jqmWindow LModal bundleModal\" style=\"display:none;\">" +
                sbi.Append("<div class=\"mContent\">" +
                            "<h2>" + Translation.GetTerm("BundlesAvailableModal_Heading", "Bundles Available") + "</h2>" +
                            "<p class=\"mb10 bundleAvailableTerm\">" + Translation.GetTerm("BundlesAvailableModal_Text", "Click on an available bundle below to save on your order!") + "</p>" +
                            "<ul class=\"flatList bundleList\">");
                for (var j = 0; j < possibleDynamicKitProducts.Count(); j++)
                {
                    var product = possibleDynamicKitProducts[j];
                    sbi.Append("<li class=\"UI-lightBg brdrAll bundleOption\">" +
                                "<input type=\"hidden\" class=\"dynamicKitProductSuggestion\" value=\"" + product.ProductID + "\" />" +
                                "<a href=\"javascript:void(0);\" class=\"block pad5 brdrAll CreateBundle\">" + product.Translations.Name() + "</a></li>");
                }

                sbi.Append(
                    "</ul>"
                    +
                    "<p class=\"mt10\"><a href=\"javascript:void(0);\" class=\"jqmClose FL cancel close\">Close</a></p>"
                    + "<span class=\"clr\"></span>" + "</div>"); //</div>");

                //sbi.Append("<script type=\"text/javascript\">$(function() { $('#BundleModal" + newID + "').jqm({ trigger: '#BundleTrigger" + newID + "'," +
                //                                "onShow: function (h) {h.w.css({" +
                //                                                "top: Math.floor(parseInt($(window).height() / 2)) - Math.floor(parseInt(h.w.height() / 2)) + ($(window).scrollTop() - 20) + 'px'," +
                //                                                "left: Math.floor(parseInt($(window).width() / 2)) + 'px'" +
                //                                        "}).fadeIn(); } }) });</script>");
            }

            return sb.ToString() + sbi.ToString();
        }


        public ActionResult CancelOrder()
        {
            try
            {
                bool result = true;
                string message = null;
                if (OrderContext.Order.OrderID != 0)
                {
                    if (OrderContext.Order != null)
                    {
                        Order o = Order.LoadFull(OrderContext.Order.OrderID);
                        if (o != null)
                        {
                            result = OrderService.TryCancel(o, out message);
                        }
                    }
                }


                string sessionEdit = Session["Edit"].ToString();
                if (sessionEdit == "1")
                {
                    //Eliminar los dispatch en modo edit
                    int RowDispatchControlDel = ShippingCalculatorExtensions.DispatchItemControlsDel(OrderContext.Order.OrderCustomers[0].AccountID, OrderContext.Order.OrderID);
                }

                if (OrderContext.Order != null && result)
                {
                    //Actualizar inventario
                    int count = 0;

                    int ppt = OrderContext.Order.AsOrder().OrderCustomers[0].ProductPriceTypeID;

                    foreach (var item in OrderContext.Order.AsOrder().OrderCustomers[0].OrderItems)
                    {
                        var ne = Order.GenerateAllocation(item.ProductID.Value,
                            item.Quantity, OrderContext.Order.AsOrder().OrderID, Convert.ToInt32(Session["WareHouseId"]), EntitiesEnums.MaintenanceMode.Delete,
                            Convert.ToInt32(Session["PreOrder"]), CoreContext.CurrentAccount.AccountTypeID, false);
                    }

                    OrderContext.Clear();
                }
                return RedirectToAction("Index", "OrderHistory");
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, orderID: OrderContext.Order.OrderID.ToIntNullable());
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }


        [NonAction]
        private string ConvertToDynamicKit(Product kitProduct)
        {
            if (OrderContext.Order == null || kitProduct == null)
            {
                return string.Empty;
            }

            var customer = OrderContext.Order.AsOrder().OrderCustomers[0];
            var parentGuid = OrderContext.Order.AsOrder().ConvertToDynamicKit(customer, kitProduct);
            OrderService.UpdateOrder(OrderContext);

            return parentGuid;
        }

        [FunctionFilter("Orders", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public ActionResult CreateDynamicBundleUpSale(int productId)
        {
            var clonedOrder = OrderContext.Order.AsOrder().Clone();
            var customer = OrderContext.Order.AsOrder().OrderCustomers[0];

            try
            {
                var product = Inventory.GetProduct(productId);
                string kitGuid = ConvertToDynamicKit(product);

                if (kitGuid.IsNullOrEmpty())
                {
                    return Json(new
                    {
                        result = false,
                        guid = kitGuid,
                        message = Translation.GetTerm("BundleCouldNotBeCreated", "The bundle could not be created.  Please try again.")
                    });
                }

                return Json(new
                {
                    result = true,
                    guid = kitGuid,
                    orderCustomerId = customer.OrderCustomerID
                });
            }
            catch (Exception ex)
            {
                //something went wrong in conversion to kit so revert
                OrderContext.Order = clonedOrder;
                var message = ex.Log(accountID: CurrentAccount.AccountID, orderID: OrderContext.Order.OrderID).PublicMessage;
                return JsonError(message);
            }
        }

        [FunctionFilter("Orders", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public virtual ActionResult Edit(string orderNumber)
        {
            try
            {
                /*
                En el edit nunca debe de haber un metodo de pago registrado( en orderpayments),
                este error se estaba presentando(en el boton de realizar el pedido) ya que el usuario sale del form. de paypal de una manera inusual                 
                Con esto nos aseguramos que al editar su pedido pendiente  este lo habra y no nuestre ningun pago( salvo que sea residual  pero eso se controla al finalizar el edit) .*/
                AccountPropertiesBusinessLogic.GetValueByID(NetSteps.Data.Entities.Constants.OrderPayments.Delete.ToInt(), orderNumber.ToInt());

                //ViewBag.ParameterVEA = OrderExtensions.GetGeneralParameterVEA();
                ViewBag.Cancel = true;
                Session["vbuilder"] = null;
                Session["vControlGenShipping"] = null;
                Session["PreOrder"] = null;
                OrderContext.Order = null;
                ValidatePaymentByMarket();
                if (!String.IsNullOrWhiteSpace(orderNumber))
                    OrderContext.Order = Order.LoadByOrderNumberFull(orderNumber);

                if (OrderContext.Order == null) return RedirectToAction("NewOrder");
                if (OrderContext.Order.OrderStatusID != (short)Constants.OrderStatus.Pending)
                {
                    return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Details", action = "Index", id = orderNumber }));
                }

                if (OrderContext.Order.AsOrder().SiteID == 0)
                    OrderContext.Order.AsOrder().SiteID = ConfigurationManager.GetAppSetting<int>(ConfigurationManager.VariableKey.NSCoreSiteID);

                //if (!CurrentAccountCanAccessOrder(OrderContext.Order.AsOrder())) RedirectToSafePage();

                //int SiteID = ConfigurationManager.GetAppSetting<int>(ConfigurationManager.VariableKey.NSCoreSiteID);
                OrderContext.Order.AsOrder().SiteID = ConfigurationManager.GetAppSetting<int>(ConfigurationManager.VariableKey.NSCoreSiteID);

                OrderCustomer customer = OrderContext.Order.AsOrder().OrderCustomers.FirstOrDefault();

                OrderShipment shipment = OrderContext.Order.AsOrder().GetDefaultShipmentNoDefault();

                NetSteps.Data.Entities.Account account = null;
                if (customer != null && customer.AccountID != 0)
                {
                    account = NetSteps.Data.Entities.Account.LoadFull(customer.AccountID);
                }

                int PreOrderId = Convert.ToInt32(Session["PreOrder"]);
                Session["WareHouseId"] = WarehouseExtensions.WareHouseByAddresID(account.AccountID);
                Session["Period"] = OrderExtensions.GetPeriodByOrderId(orderNumber, account.AccountID);

                OrderContext.Order.ParentOrderID = Convert.ToInt32(Session["WareHouseId"]);
                var o = Session["PreOrder"]; if (o != null) OrderContext.Order.PreorderID = (int)o;
                OrderService.UpdateOrder(OrderContext);

                Address defaultShippingAddres = CurrentAccount.Addresses.GetDefaultByTypeID(Constants.AddressType.Shipping);
                //Shipping address Logic
                IEnumerable<ShippingMethodWithRate> NewShippingMethodsList =
                    GetNewShippingMethods(defaultShippingAddres.AddressID); //RECENTLY ADDED BY WCS

                ViewData["ShippingMethods"] = defaultShippingAddres != null ? NewShippingMethodsList : new List<ShippingMethodWithRate>(); //RECENTLY ADDED BY WCS
                var shippingMethodWithRate = ViewData["ShippingMethods"] as IEnumerable<ShippingMethodWithRate>;
                Session["ExisShippingMetods"] = true;
                if (shippingMethodWithRate == null)
                {
                    Session["ExisShippingMetods"] = false;
                }
                //END - Shipping address Logic
                IEnumerable<ShippingMethodWithRate> NewShippingMethodsList1 =
                    NewShippingMethodsList.Where(x => x.ShippingMethodID == shipment.ShippingMethodID);
                //RECENTLY ADDED BY WCS
                foreach (var item in NewShippingMethodsList1) Session["DateEstimated"] = item.DateEstimated; //RECENTLY ADDED BY WCS

                TempData["sPaymentMethod"] = from x in Order.SelectPaymentMethod(OrderContext.Order.ConsultantID, OrderContext.Order.AsOrder().OrderTypeID)
                                             orderby x.Key
                                             select new SelectListItem()
                                             {
                                                 Text = x.Value,
                                                 Value = x.Key
                                             };
                ViewData["Catalogs"] = Inventory.GetActiveCatalogs(NetSteps.Data.Entities.ApplicationContext.Instance.StoreFrontID);
                ViewBag.BulkAddModel = GetBulkAddModel();

                int SiteID = ConfigurationManager.GetAppSetting<int>(ConfigurationManager.VariableKey.NSCoreSiteID);

                Session["PreOrder"] = OrderExtensions.GetPreOrderByOrderID(OrderContext.Order.OrderID);

                if (PreOrderId > 0)
                {
                    //Recorre el WareHouseMaterialAllocation
                    foreach (var objWMA in Order.GetMaterialWareHouseMaterial(customer.OrderID))
                    {
                        bool Exist = false;
                        //Si los campos son iguales en OrderItems
                        foreach (var objOI in Order.GetMaterialOrderItem(customer.OrderCustomerID).Where(x => x.MaterialID == objWMA.MaterialID))
                        {
                            Exist = true;
                        }
                        if (!Exist)
                        {
                            //Borrar y actualizar el stock 
                            PreOrderExtension.UpdWarehouseMaterial(objWMA.MaterialID, objWMA.Quantity, objWMA.WareHouseID);
                            //Elimino del HouseMaterialsAllocations
                            PreOrderExtension.DelWareHouseMaterialsAllocationsXPreOrder(objWMA.ProductID, Convert.ToInt32(Session["PreOrder"]));
                        }
                    }
                }
                ViewBag.AccountTypeId = Convert.ToInt32(account.AccountTypeID);
                calculateDispatch();
                ApplyPaymentPreviosBalance();
                Session["Edit"] = "1";
                var indexModel = Index_CreateModel(OrderContext.Order.AsOrder());
                CalculateTotal();
                actBalanceDue();
                return View("Index", indexModel);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, (OrderContext.Order != null) ? OrderContext.Order.OrderID.ToIntNullable() : null, accountID: (CurrentAccount != null) ? CurrentAccount.AccountID.ToIntNullable() : null);
                throw exception;
            }
        }

        #region Product Listing


        [FunctionFilter("Orders", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public virtual ActionResult SearchProducts(string query, bool includeDynamicKits = true)
        {
            try
            {
                List<Product> ProductList = FindProducts(query, includeDynamicKits).Where(p => !p.IsVariantTemplate).Take(10).ToList();
                //ProductRepository repository = new ProductRepository();
                //foreach (Product product in ProductList)
                //{
                //    product.Active = repository.LoadOne(product.ProductID).Active;
                //}
                return Json(ProductList.Where(p => p.Active).Select(p => new
                {
                    id = p.ProductID,
                    text = p.SKU + " - " + p.Translations.Name(),
                    isDynamicKit = p.IsDynamicKit(),
                    needsBackOrderConfirmation = Inventory.IsOutOfStock(p) && p.ProductBackOrderBehaviorID == NetSteps.Data.Entities.Constants.ProductBackOrderBehavior.AllowBackorderInformCustomer.ToInt(),
                    customizationType = p.CustomizationType()
                }));


            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, OrderContext.Order != null ? OrderContext.Order.OrderID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }

        }

        public virtual ActionResult ApplyPaymentPreviosBalance()
        {
            if (OrderContext.Order.AsOrder().OrderStatusID != 4)
            {
                //NetSteps.Data.Entities.Business.CTE.paymentTables.RemoveAll();
                RemovePaymentsTable(0);
                BasicResponseItem<OrderPayment> response = new BasicResponseItem<OrderPayment>();
                OrderContext.Order.AsOrder().OrderCustomers[0].RemoveAllPayments();
                decimal amount = Order.GetProductCreditByAccount(OrderContext.Order.AsOrder().ConsultantID);

                int existPB = 0;
                existPB = OrderContext.Order.AsOrder().OrderPayments.Where(x => x.BankName == "Product Credit").ToList().Count();


                if (amount == 0 || existPB > 0)
                {
                    return Json(new { message = "" });
                }
                else
                {
                    try
                    {
                        int paymentMethodId = 60;


                        IPayment payment = new Payment()
                        {
                            DecryptedAccountNumber = null,
                            CVV = null,
                            PaymentType = ConstantsGenerated.PaymentType.EFT,
                            NameOnCard = null,
                            BankName = "Product Credit"//"PREVIOS BALANCE"

                        };

                        payment.PaymentTypeID = Order.GetApplyPaymentType(paymentMethodId);
                        //payment = NetSteps.Data.Entities.Account.LoadPaymentMethodAndVerifyAccount(paymentMethodId, CurrentAccount.AccountID);
                        response = OrderContext.Order.AsOrder().ApplyPaymentToCustomerPreviosBalance(payment.PaymentTypeID, amount, payment, user: CoreContext.CurrentUser);



                        ApplyPaymentSearchData objE = new ApplyPaymentSearchData();
                        objE.OrderPaymentId = response.Item.OrderPaymentID;
                        objE.Amount = amount;
                        objE.PaymentConfigurationID = paymentMethodId;
                        objE.NameOnCard = "";
                        objE.PreOrderID = Convert.ToInt32(Session["PreOrder"]);
                        NetSteps.Data.Entities.Business.CTE.ApplyPayment(objE);


                        if (response.Success)
                        {
                            OrderService.UpdateOrder(OrderContext);
                            var paymentResponse = response.Item;
                            return Json(new
                            {
                                //result = true,
                                //message = string.Empty,
                                totals = Totals,
                                OrderEntryModelData = GetOrderEntryModelData(OrderContext),
                                paymentsGrid = RenderRazorPartialViewToString("OEPaymentsGrid", OrderContext.Order.AsOrder()),
                                //paymentId = paymentResponse == null ? 0 : paymentResponse.OrderPaymentID
                            });
                        }
                        else
                        {
                            return Json(new { result = false, message = response.Message });
                        }
                    }
                    catch (Exception ex)
                    {
                        var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, (OrderContext.Order != null) ? OrderContext.Order.OrderID.ToIntNullable() : null, accountID: (CurrentAccount != null) ? CurrentAccount.AccountID.ToIntNullable() : null);
                        return Json(new { result = false, message = exception.PublicMessage });
                    }
                }

            }
            return Json(new { message = "" });
        }

        protected virtual IEnumerable<Product> FindProducts(string query, bool includeDynamicKits)
        {
            //return Inventory.SearchProducts(NetSteps.Data.Entities.ApplicationContext.Instance.StoreFrontID, query, CoreContext.CurrentAccount.AccountTypeID)
            //     .Where(p => (!Inventory.IsOutOfStock(p) || p.ProductBackOrderBehaviorID != NetSteps.Data.Entities.Constants.ProductBackOrderBehavior.Hide.ToInt()));

            return Product.SearchProductForOrder(query).Where(p => (!Inventory.IsOutOfStock(p) || p.ProductBackOrderBehaviorID != NetSteps.Data.Entities.Constants.ProductBackOrderBehavior.Hide.ToInt()));
        }

        protected virtual IEnumerable<int> GetCatalogsToSearch()
        {
            // Filter out these catalog types.
            var catalogTypesToFilter = new[] { (int)NetSteps.Data.Entities.Constants.CatalogType.EnrollmentKits };

            // Filter out rewards catalogs.
            var rewardCatalogs = HostessRewardType.GetAvailableCatalogs(SmallCollectionCache.Instance.HostessRewardTypes.Select(r => r.HostessRewardTypeID));

            var activeCatalogs = Inventory.GetActiveCatalogs(ApplicationContext.Instance.StoreFrontID)
                .Where(c => !catalogTypesToFilter.Contains(c.CatalogTypeID))
                .Select(c => c.CatalogID)
                .Except(rewardCatalogs);

            return activeCatalogs;
        }

        protected virtual IEnumerable<Product> CustomFilterProducts(IEnumerable<Product> products, string query, bool includeDynamicKits = true)
        {
            return products;
        }

        [FunctionFilter("Orders", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public virtual ActionResult InPageSearch(string query = null, int? kitProductId = null, int? dynamicKitGroupId = null, string sku = null)
        {
            try
            {
                OrderShipment orderShipment = null;
                if (OrderContext.Order != null)
                    orderShipment = OrderContext.Order.AsOrder().OrderShipments.FirstOrDefault();
                var outOfStockProducts = Product.GetOutOfStockProductIDs(orderShipment);

                List<Product> products = new List<Product>();
                if (sku != null)
                {
                    products.Add(Inventory.GetProduct(sku));
                }
                else
                {
                    //filter other dynamic kits
                    if (query == null)
                    {
                        products = Inventory.SearchProducts(NetSteps.Data.Entities.ApplicationContext.Instance.StoreFrontID, query, accountTypeId: CurrentAccount.AccountTypeID, includeDynamicKits: false)
                                .Where(p => !p.IsDynamicKit()).ToList();
                    }
                    else
                    {
                        products = Inventory.SearchProducts(NetSteps.Data.Entities.ApplicationContext.Instance.StoreFrontID, query)
                                .Where(p => !p.IsDynamicKit()).ToList();
                    }

                    if (kitProductId != null && dynamicKitGroupId != null)
                    {
                        products = products.Where(p => p.CanBeAddedToDynamicKitGroup(kitProductId.Value, dynamicKitGroupId.Value)).ToList();
                    }
                }

                return Json(products.Where(p => !outOfStockProducts.Contains(p.ProductID) || p.ProductBackOrderBehaviorID != (int)NetSteps.Data.Entities.Constants.ProductBackOrderBehavior.Hide).ToList()
                        .Select(p => new
                        {
                            id = p.ProductID,
                            text = p.SKU + " - " + p.Translations.Name(),
                            isDynamicKit = p.IsDynamicKit(),
                            needsBackOrderConfirmation = Product.CheckStock(p.ProductID, orderShipment).IsOutOfStock && p.ProductBackOrderBehaviorID == NetSteps.Data.Entities.Constants.ProductBackOrderBehavior.AllowBackorderInformCustomer.ToShort(),
                            customizationType = p.CustomizationType()
                        }));
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, (OrderContext.Order != null) ? OrderContext.Order.OrderID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [FunctionFilter("Orders", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public virtual ActionResult GetCategoryProducts(int categoryID = 0)
        {
            try
            {
                return Json(new { result = true, BulkAddModelData = GetBulkAddModelData(categoryID) });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, OrderContext.Order != null ? OrderContext.Order.OrderID.ToIntNullable() : null, accountID: CurrentAccount != null ? CurrentAccount.AccountID.ToIntNullable() : null);
                throw exception;
            }
        }
        #endregion

        #region Cart
        protected virtual IEnumerable<NetSteps.Data.Common.Entities.IOrderItem> AddOrUpdateOrderItems(IEnumerable<IOrderItemQuantityModification> changes)
        {
            IEnumerable<NetSteps.Data.Common.Entities.IOrderItem> results = null;
            try
            {
                results = OrderService.UpdateOrderItemQuantities(OrderContext, changes);
                //OrderService.UpdateOrder(OrderContext); /*CS:12AB2016:Comentado*/
            }
            catch (NetStepsBusinessException bsbEx)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(bsbEx, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, OrderContext.Order != null ? OrderContext.Order.OrderID.ToIntNullable() : null, accountID: CurrentAccount != null ? CurrentAccount.AccountID.ToIntNullable() : null);
            }

            return results;
        }

        public virtual ActionResult GetGiftStepInfo(string stepId)
        {
            try
            {
                var allSteps = OrderContext.InjectedOrderSteps.Union(OrderContext.Order.OrderAdjustments.SelectMany(oa => oa.InjectedOrderSteps));
                var step = (IUserProductSelectionOrderStep)allSteps.Single(os => os is IUserProductSelectionOrderStep && os.OrderStepReferenceID.ToString() == stepId);
                var inventoryService = Create.New<IInventoryService>();
                var options = step.AvailableOptions.Select(o =>
                {
                    var product = Inventory.GetProduct(o.ProductID);
                    int currencyID = OrderContext.Order.AsOrder().CurrencyID;
                    return new GiftModel
                    {
                        Name = product.Name,
                        Image = product.MainImage != null ? product.MainImage.FilePath.ReplaceFileUploadPathToken() : String.Empty,
                        Description = product.GetShortDescriptionDisplay(),
                        ProductID = product.ProductID,
                        Value = product.GetPriceByPriceType((int)NetSteps.Data.Entities.Constants.ProductPriceType.Retail, currencyID).ToString(currencyID),
                        IsOutOfStock = inventoryService.GetProductAvailabilityForOrder(OrderContext, o.ProductID, o.Quantity).CanAddNormally != o.Quantity,
                    };
                });

                bool? especial = false;
                PromotionOrderAdjustment orderAdjustment = (PromotionOrderAdjustment)OrderContext.Order.AsOrder().OrderAdjustments.Where(p => p.InjectedOrderSteps.Where(s => s.OrderStepReferenceID == stepId).Count() == 1).First().Extension;
                var promo = Create.New<IPromotionService>().GetPromotion(orderAdjustment.PromotionID);
                var reward = (SelectFreeItemsFromListReward)promo.PromotionRewards.First().Value;
                var effect = reward.Effects["ProductID Selector"];
                var extension = (IUserProductSelectionRewardEffect)(effect.Extension);
                especial = extension.IsEspecialPromotion;

                var giftSelectionModel = new GiftSelectionModel(Url.Action("GetGiftStepInfo"), Url.Action("AddGifts"), stepID: stepId, callbackFunctionName: "updateCartAndTotals", isEspecialPromo: especial);

                giftSelectionModel.MaxQuantity = step.MaximumOptionSelectionCount;

                var selections = step.Response == null ? Enumerable.Empty<IProductOption>() : ((IUserProductSelectionOrderStepResponse)step.Response).SelectedOptions;
                giftSelectionModel.SelectedOptions = selections.Select(p => options.First(o => o.ProductID == p.ProductID)).ToList();
                giftSelectionModel.AvailableOptions = options.ToList();
                return Json(new { result = true, GiftSelectionModel = giftSelectionModel });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public virtual ActionResult AddGifts(List<int> productIds, string stepId)
        {
            try
            {
                var response = Create.New<IUserProductSelectionOrderStepResponse>();
                if (productIds != null)
                {
                    foreach (var productId in productIds)
                    {
                        var option = Create.New<IProductOption>();
                        option.ProductID = productId;
                        option.Quantity = 1;
                        response.SelectedOptions.Add(option);
                    }
                }
                var allSteps = OrderContext.InjectedOrderSteps.Union(OrderContext.Order.OrderAdjustments.SelectMany(oa => oa.InjectedOrderSteps));
                var step = (IUserProductSelectionOrderStep)allSteps.Single(os => os is IUserProductSelectionOrderStep && os.OrderStepReferenceID.ToString() == stepId);
                step.Response = response;
                OrderService.UpdateOrder(OrderContext);

                return Json(new
                {
                    result = true,
                    OrderEntryModelData = GetOrderEntryModelData(OrderContext),
                    totals = Totals,
                    promotions = GetApplicablePromotions(OrderContext),
                    shippingMethods = ShippingMethods,
                });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        /// <summary>
        /// Valida si el producto ya esta agregado en la orden
        /// </summary>
        /// <param name="productId">Id del producto que se esta agregando</param>
        /// <returns></returns>
        public ActionResult ExistsProductInOrder(int productId)
        {
            var exists = OrderContext.Order.AsOrder().OrderCustomers[0].OrderItems.FirstOrDefault(x => x.ProductID.Equals(productId));
            return Json(new { result = exists == null ? false : true, message = Translation.GetTerm("ExistsProductInOrder", "This product already exists in the order. You want to add the amount ?") });
        }

        [FunctionFilter("Orders", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public virtual ActionResult AddToCart(int productId, int quantity, string parentGuid = null, int? dynamicKitGroupId = null)
        {
            //Validar valores negativos 
            if (quantity <= 0)
            {
                return Json(new { result = false, restricted = true, message = Translation.GetTerm("InvalidProductQuantity", "Quantity Negative") });
            }

            #region ExistsProductInOrder & Product Quota

            bool ExistsProductInOrder = false;
            int NewQuantity = 0;
            var itemAdded = OrderContext.Order.AsOrder().OrderCustomers[0].OrderItems.Count > 0 ? OrderContext.Order.AsOrder().OrderCustomers[0].OrderItems.FirstOrDefault(x => x.ProductID.Equals(productId)) : null;
            if (itemAdded != null) { ExistsProductInOrder = true; NewQuantity = itemAdded.Quantity + quantity; }

            #endregion

            Session["vControlGenShipping"] = false;
            if (ProductQuotasRepository.ProductIsRestricted(productId, ExistsProductInOrder ? NewQuantity : quantity, CurrentAccount.AccountID, CurrentAccount.AccountTypeID))
                return Json(new { result = false, restricted = true, message = Translation.GetTerm("ProductCantPurchased", "The added product can not be purchased") });

            #region Valida Precios Activos del Producto agregado

            int currendyid = OrderContext.Order.AsOrder().CurrencyID;
            int ppt = OrderContext.Order.AsOrder().OrderCustomers[0].ProductPriceTypeID;

            if (ProductPricesExtensions.GetPriceByPriceType(productId, ppt, currendyid) <= 0)
                return Json(new { result = false, restricted = true, message = Translation.GetTerm("ProductNotActive", "The added product is not active") });

            #endregion
            calculateTotalDispatch();
            return PerformAddToCart(productId, quantity, parentGuid, dynamicKitGroupId);
        }



        public virtual void ConsolidateOrderItemsKits(int productId)
        {
            var orderItems = OrderContext.Order.OrderCustomers.First().OrderItems.Where(p => p.ProductID == productId && ((OrderItem)p).MaterialID == null);
            var orderItemsList = orderItems as IList<IOrderItem> ?? orderItems.ToList();
            var listToRemove = new List<Guid>();
            if (orderItemsList.Count() == 2 && ((OrderItem)orderItemsList.First()).ChildOrderItems.Count() > 0)
            {
                var queryFirst = ((OrderItem)orderItemsList.ToList()[0]);
                var querySecond = ((OrderItem)orderItemsList.ToList()[1]);
                queryFirst.Quantity += querySecond.Quantity;
                listToRemove.Add(querySecond.Guid);
                foreach (var child in querySecond.ChildOrderItems)
                {
                    var childtemp = child;
                    var queryChids = OrderContext.Order.OrderCustomers.First()
                        .OrderItems.Where(o => o.ProductID == childtemp.ProductID && ((OrderItem)o).MaterialID == childtemp.MaterialID).ToList();
                    var firstChild = (OrderItem)queryChids[0];
                    var secondChild = (OrderItem)queryChids[1];
                    firstChild.Quantity += secondChild.Quantity;
                    listToRemove.Add(secondChild.Guid);
                }
                OrderContext.Order.OrderCustomers.First().OrderItems.RemoveWhere(o => listToRemove.Contains(((OrderItem)o).Guid));
            }
        }


        [NonAction]
        protected ActionResult PerformAddToCart(int productId, int quantity, string parentGuid = null, int? dynamicKitGroupId = null, IDictionary<string, string> itemProperties = null)
        {
            
            //1.	Get Changed Order Items
            //2.	Current Shipping Methods
            //3.	Totals
            //4.	Reconciled Payments
            try
            {
                if (quantity == 0)
                {
                    return Json(new
                    {
                        result = false,
                        message = string.Format(Translation.GetTerm("TheItemCouldNotBeAddedInvalidQuantityOf", "The product could not be added. Invalid quantity of {0}."), "0")
                    });
                }

                int wareHouseID = Convert.ToInt32(Session["WareHouseId"]);
                int preOrder = Convert.ToInt32(Session["PreOrder"]);

                string messageValidKit = "";
                int insert = 0;

                var itemAdded = OrderContext.Order.AsOrder().OrderCustomers[0].OrderItems.Count > 0 ? OrderContext.Order.AsOrder().OrderCustomers[0].OrderItems.FirstOrDefault(
               x => x.ProductID.Equals(productId)) : null;

                var hasChangedKit = HasChangedKit(productId, quantity);
                if (hasChangedKit != null) return hasChangedKit;

                var currentQuantity = (itemAdded != null && itemAdded.Quantity > 0) ? itemAdded.Quantity + quantity : quantity;

                OrderContext.Order.WareHouseID = wareHouseID;
                if (itemAdded != null)
                {
                    Order.RemoveLineOrder(productId, wareHouseID, preOrder, CoreContext.CurrentAccount.AccountTypeID, false);
                }
                //Ingreso Nuevo 
                
                bool showOutOfStockMessage = false;
                string outOfStockMessage = "";
                var product = Inventory.GetProduct(productId);
                product.ProductBackOrderBehaviorID = 4;

                //This is only for the current product
                var queryAllocations = Order.GenerateAllocation(productId, currentQuantity,
                   OrderContext.Order.AsOrder().OrderID, wareHouseID, EntitiesEnums.MaintenanceMode.Add, preOrder,
                   CoreContext.CurrentAccount.AccountTypeID, false);
                foreach (var item in queryAllocations)
                {
                    if (!item.Estado) { return Json(new { result = false, restricted = true, message = item.Message }); }

                    if ((item.EstatusNewQuantity) || (item.Estado && string.IsNullOrEmpty(item.Message) && item.NewQuantity > 0))
                    {
                        messageValidKit = item.Message;
                        quantity = item.NewQuantity;
                    }
                }

                List<OrderItemUpdate> objOrderItemsOld = new List<OrderItemUpdate>{
                    new OrderItemUpdate() {Product = productId, Quantity = quantity, status = false}
                };

                foreach (var productRemove in OrderContext.Order.AsOrder().OrderCustomers[0].OrderItems)
                {
                    if (productRemove.ProductID != null)
                        objOrderItemsOld.Add(new OrderItemUpdate()
                        {
                            Product = productRemove.ProductID.Value,
                            Quantity = productRemove.Quantity,
                            status = false
                        });
                }

                bool isDynamicKit = product.IsDynamicKit();
                bool isBundleGroupComplete = false;
                var customer = OrderContext.Order.AsOrder().OrderCustomers[0];
                OrderItem bundleItem = null;
                Product bundleProduct = null;
                DynamicKitGroup group = null;

                Guid pguid;
                if (!String.IsNullOrWhiteSpace(parentGuid) && Guid.TryParse(parentGuid, out pguid) && dynamicKitGroupId.HasValue)
                {
                    bundleItem = customer.OrderItems.Cast<OrderItem>().FirstOrDefault(oc => oc.Guid == pguid);
                    bundleProduct = Inventory.GetProduct(bundleItem.ProductID.Value);
                    group = bundleProduct.DynamicKits[0].DynamicKitGroups.Where(g => g.DynamicKitGroupID == dynamicKitGroupId).FirstOrDefault();
                    int currentCount = bundleItem.ChildOrderItems.Where(oi => oi.DynamicKitGroupID == dynamicKitGroupId).Sum(oi => oi.Quantity);
                    if ((currentCount + quantity) > group.MinimumProductCount)
                    {
                        return Json(new
                        {
                            result = false,
                            message = string.Format(Translation.GetTerm("TheItemCouldNotBeAddedInvalidQuantityOf", "The product could not be added. Invalid quantity of {0}."), quantity)
                        });
                    }
                }

                IEnumerable<IOrderItem> addedItems = null;
                var mod = Create.New<IOrderItemQuantityModification>();
                   mod.ProductID = productId;
                   mod.Quantity = quantity;
                   mod.ModificationKind = OrderItemQuantityModificationKind.Add;
                   mod.Customer = OrderContext.Order.OrderCustomers[0];
                   RemovePaymentsTable(0);
                   OrderContext.Order.AsOrder().OrderCustomers[0].RemoveAllPayments();

                    if (bundleItem != null && bundleProduct != null && group != null)
                    {
                        OrderService.AddOrderItemsToOrderBundle(OrderContext, bundleItem, new IOrderItemQuantityModification[] { mod }, dynamicKitGroupId.Value);
                        int currentCount = bundleItem.ChildOrderItems.Where(i => i.DynamicKitGroupID == dynamicKitGroupId).Sum(i => i.Quantity);
                        isBundleGroupComplete = currentCount == group.MinimumProductCount;
                    }
                    else
                    {
                        OrderContext.Order.PreorderID = preOrder;
                        AddOrUpdateOrderItems(new[] { mod });
                        ConsolidateOrderItemsKits(productId);
                    }
                

                ApplyPaymentPreviosBalance();
                OrderContext.Order.ParentOrderID = wareHouseID;                
                OrderService.UpdateOrder(OrderContext);
                
          
                CalculateTotal();
                if (messageValidKit != "")
                {
                    outOfStockMessage = messageValidKit;
                }
                string DateEstimated = "";

                return Json(new
                {
                    result = true,
                    OrderEntryModelData = GetOrderEntryModelData(OrderContext),
                    promotions = GetApplicablePromotions(OrderContext),
                    applicablePromotionsHTML = this.GetPromotionsHtml(OrderContext.Order.AsOrder()),
                    outOfStockItems = GetOutOfStockItemsString(OrderContext.Order.AsOrder()),
                    shippingMethods = ShippingMethods,
                    allow= true,
                    showOutOfStockMessage,
                    isBundle = isDynamicKit,
                    bundleGuid = isDynamicKit ? customer.OrderItems.Cast<OrderItem>().FirstOrDefault(i => i.ProductID == product.ProductID && !i.HasChildOrderItems).Guid.ToString("N") : string.Empty,
                    productId = product.ProductID,
                    groupItemsHtml = parentGuid == null ? "" : GetGroupItemsHtml(parentGuid, dynamicKitGroupId.Value).ToString(),
                    isBundleGroupComplete = isBundleGroupComplete,
                    childItemCount = parentGuid == null ? 0 : customer.OrderItems.Cast<OrderItem>().GetByGuid(parentGuid).ChildOrderItems.Sum(c => c.Quantity),
                    orderCustomerId = ((OrderCustomer)customer).OrderCustomerID,
                    BundleOptionsSpanHTML = GetDynamicBundleUpSale(),
                    AvailableBundleCount = _availableBundleCount,
                    resultQualificationTotal = Totals.GetType().GetProperty("handlingTotal").GetValue(Totals, null),
                    resultCommisionableTotal = Totals.GetType().GetProperty("handlingTotal").GetValue(Totals, null),
                    //productCredit = productCredit,
                    //productCreditResult = productCreditResult,
                    message = outOfStockMessage,
                    dateEstimated = Convert.ToString(Session["DateEstimated"]),
                    paymentsGrid = RenderRazorPartialViewToString("OEPaymentsGrid", OrderContext.Order.AsOrder()),
                    totals = Totals
                });
            }
            catch (ProductShippingExcludedShippingException ex)
            {
                var excludedProducts = ex.ProductsThatHaveExcludedShipping.Select(prod => prod.Name);
                return Json(new { result = false, message = ex.PublicMessage, products = excludedProducts });
            }
            catch (Exception ex)
            {
                NetStepsException exception;
                if (!(ex is NetStepsBusinessException))
                    exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, (OrderContext.Order != null) ? OrderContext.Order.OrderID.ToIntNullable() : null, accountID: (CurrentAccount != null) ? CurrentAccount.AccountID.ToIntNullable() : null);
                else
                    exception = ex as NetStepsException;
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        private JsonResult HasChangedKit(int productId, int newQuantity)
        {
            var queryOrderITems = OrderContext.Order.OrderCustomers.First().OrderItems;

            var validationKit = PreOrderExtension.GetKitComposition(productId);
            var OrderItemQuantity = 0;
            var queryOrderItem = queryOrderITems.FirstOrDefault(l => l.ProductID == productId && l.ParentOrderItem == null);

            if (queryOrderItem == null)
                return null;

            OrderItemQuantity = queryOrderItem.Quantity;
            var hasChangedQuantity = (newQuantity != OrderItemQuantity);
            if (hasChangedQuantity)
            {
                foreach (var kitChild in ((OrderItem)queryOrderItem).ChildOrderItems)
                {
                    var queryKit = validationKit.Find(o => kitChild.MaterialID != null && ((o.MaterialID == kitChild.MaterialID.Value) &&
                                                                                           (kitChild.Quantity / queryOrderItem.Quantity) == o.Quantity));
                    if (queryKit == null)
                    {
                        return Json(new { result = false, message = Translation.GetTerm("KitChanged", "The kit ({0}) has changed. Please remove and add it again.", ((OrderItem)queryOrderItem).SKU) });
                    }
                }
            }
            return null;
        }

        [FunctionFilter("Orders-Bulk Add To Cart", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public virtual ActionResult BulkAddToCart(List<ProductQuantityContainer> products)
        {
            try
            {
                #region Variables

                string msgError = "";
                int SiteID = ConfigurationManager.GetAppSetting<int>(ConfigurationManager.VariableKey.NSCoreSiteID);
                int ppt = OrderContext.Order.AsOrder().OrderCustomers[0].ProductPriceTypeID;

                #endregion

                foreach (var objProduct in products)
                {
                    #region ExistsProductInOrder & Product Quota

                    bool ExistsProductInOrder = false;
                    int NewQuantity = 0;
                    var itemAdded = OrderContext.Order.AsOrder().OrderCustomers[0].OrderItems.Count > 0 ? OrderContext.Order.AsOrder().OrderCustomers[0].OrderItems.FirstOrDefault(x => x.ProductID.Equals(objProduct.ProductID)) : null;
                    if (itemAdded != null)
                    {
                        //Order.RemoveLineOrder(objProduct.ProductID, itemAdded.Quantity, OrderContext.Order.AsOrder().OrderID, Convert.ToString(OrderContext.Order.AsOrder().OrderTypeID), Convert.ToInt32(Session["WareHouseId"]), Convert.ToInt32(Session["PreOrder"]), ppt, CoreContext.CurrentAccount.AccountTypeID, false);
                        ExistsProductInOrder = true; NewQuantity = itemAdded.Quantity + objProduct.Quantity;
                    }

                    if (ProductQuotasRepository.ProductIsRestricted(objProduct.ProductID, ExistsProductInOrder ? NewQuantity : objProduct.Quantity, CurrentAccount.AccountID, CurrentAccount.AccountTypeID))
                        return Json(new { result = false, restricted = true, message = Translation.GetTerm("ProductCantPurchased", "The added product can not be purchased") });

                    #endregion

                    #region GenerateAllocation

                    foreach (var item in Order.GenerateAllocation(objProduct.ProductID,
                                                                  objProduct.Quantity,
                                                                  OrderContext.Order.AsOrder().OrderID,
                                                                  Convert.ToInt32(Session["WareHouseId"]),
                                                                  EntitiesEnums.MaintenanceMode.Update,
                                                                  Convert.ToInt32(Session["PreOrder"]),
                                                                  CoreContext.CurrentAccount.AccountTypeID,
                                                                  false))
                    {
                        if (!item.Estado)
                        {
                            msgError = msgError + " The Product: " + objProduct.ProductID + " " + item.Message;
                            objProduct.Quantity = item.NewQuantity;
                        }
                        else if (item.EstatusNewQuantity)
                        {
                            msgError = msgError + " The Product: " + objProduct.ProductID + " " + item.Message;
                            objProduct.Quantity = item.NewQuantity;
                        }
                        else if (item.Estado && item.Message != "" && item.NewQuantity > 0)
                        {
                            msgError = msgError + " The Product: " + objProduct.ProductID + " " + item.Message;
                            objProduct.Quantity = item.NewQuantity;
                        }
                    }

                    #endregion
                }

                #region UpdateOrder

                var validProducts = products.Where(p => p.Quantity > 0).ToList();
                AddOrUpdateOrderItems(validProducts.Select(item =>
                {
                    var mod = Create.New<IOrderItemQuantityModification>();
                    mod.Customer = OrderContext.Order.OrderCustomers[0];
                    mod.ProductID = item.ProductID;
                    mod.Quantity = item.Quantity;
                    mod.ModificationKind = OrderItemQuantityModificationKind.Add;

                    return mod;
                }));

                #endregion

                #region Return

                bool result = true;
                if (msgError != "") result = false;
                if (validProducts.Count > 0)
                    return Json(new
                    {
                        totals = Totals,
                        result = result,
                        OrderEntryModelData = GetOrderEntryModelData(OrderContext),
                        shippingMethods = ShippingMethods,
                        outOfStockItems = GetOutOfStockItemsString(OrderContext.Order.AsOrder()),
                        message = msgError
                    });
                return JsonError(Translation.GetTerm("InvalidProductQuantity", "Invalid product quantity"));

                #endregion
            }
            catch (Exception ex)
            {
                NetStepsException exception;
                if (!(ex is NetStepsBusinessException))
                    exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, (OrderContext.Order != null) ? OrderContext.Order.OrderID.ToIntNullable() : null, accountID: (CurrentAccount != null) ? CurrentAccount.AccountID.ToIntNullable() : null);
                else
                    exception = ex as NetStepsException;
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public virtual ActionResult ListWarehouseMaterialLacks()
        {
            var listWarehouseMaterialLacks = PreOrderExtension.GetWarehouseMaterialLacksByPreOrder(Convert.ToInt32(Session["PreOrder"]), 5);//CurrentAccount.Language.LanguageID
            return Json(new { result = true, listWarehouseMaterialLacks = listWarehouseMaterialLacks });
        }


        [FunctionFilter("Orders", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public virtual ActionResult UpdateCart(List<ProductQuantityContainer> products)
        {
            OrderContext.Order.PreorderID = Convert.ToInt32(Session["PreOrder"]);
            foreach (var item in products)
            {
                if (item.Quantity <= 0)
                {
                    return Json(new { result = false, restricted = true, message = Translation.GetTerm("InvalidProductQuantity", "Quantity Negative") });
                }
            }

            foreach (var item in products)
            {
                if (ProductQuotasRepository.ProductIsRestricted(item.ProductID, item.Quantity, CurrentAccount.AccountID, CurrentAccount.AccountTypeID))
                    return Json(new { result = false, restricted = true, message = Translation.GetTerm("ProductCantPurchased", "The added product can not be purchased") });
            }

            try
            {
                string messageValidKit = "";
                int productID = 0;
                int quantity = 0;
                var queryOrderITems = OrderContext.Order.OrderCustomers.First().OrderItems;

                products.Each(u => u.HasQuantityChange = true);
                foreach (var prod in queryOrderITems.Where(i => ((OrderItem)i).ChildOrderItems.Count > 0))
                {
                    var queryNewProducts = products.FirstOrDefault(l => l.ProductID == prod.ProductID.Value);
                    if (queryNewProducts != null)
                    {
                        var hasChangedKit = HasChangedKit(prod.ProductID.Value, queryNewProducts.Quantity);
                        if (hasChangedKit != null) return hasChangedKit;                  
                    }                    
                }

                foreach (var noKitOrderItem in queryOrderITems.Where(i => ((OrderItem)i).ChildOrderItems.Count == 0 && ((OrderItem)i).ParentOrderItemID == null))
                {
                    var queryProducts = products.FirstOrDefault(o => o.ProductID == noKitOrderItem.ProductID.Value);
                    var queryOrderItemData = queryOrderITems.FirstOrDefault(l => noKitOrderItem.ProductID != null && l.ProductID == noKitOrderItem.ProductID.Value);
                    if (queryOrderItemData != null && (queryProducts != null))
                        queryProducts.HasQuantityChange = (queryProducts.Quantity != queryOrderItemData.Quantity);
                }

                var changes = products.Where(n => n.HasQuantityChange)
                    .Select(item =>
                    {
                        var mod = Create.New<IOrderItemQuantityModification>();
                        mod.Customer = OrderContext.Order.OrderCustomers[0];
                        Guid oig;
                        if (!String.IsNullOrWhiteSpace(item.OrderItemGuid) && Guid.TryParse(item.OrderItemGuid, out oig))
                        {
                            var existingItem = mod.Customer.OrderItems.FirstOrDefault(oi => ((OrderItem)oi).Guid == oig);
                            if (existingItem != null)
                            {
                                mod.ExistingItem = existingItem;
                            }
                        }

                        mod.ProductID = item.ProductID;
                        mod.Quantity = item.Quantity;
                        mod.ModificationKind = OrderItemQuantityModificationKind.SetToQuantity;

                        return mod;
                    });

                List<OrderItemUpdate> objOrderItemsOld = new List<OrderItemUpdate>();
                foreach (var p in products.Where(p => p.HasQuantityChange))
                {
                    foreach (var item in Order.GenerateAllocation(p.ProductID,
                                                                  p.Quantity,
                                                                  OrderContext.Order.AsOrder().OrderID,
                                                                  Convert.ToInt32(Session["WareHouseId"]),
                                                                  EntitiesEnums.MaintenanceMode.Update,
                                                                  Convert.ToInt32(Session["PreOrder"]),
                                                                  CoreContext.CurrentAccount.AccountTypeID, false))
                    {
                        if (!item.Estado)
                        {
                            return Json(new { result = false, restricted = true, message = item.Message });
                        }
                        else if (item.EstatusNewQuantity)
                        {
                            messageValidKit = item.Message;
                            p.Quantity = item.NewQuantity;
                        }
                        else if (item.Estado && item.Message != "" && item.NewQuantity > 0)
                        {
                            messageValidKit = item.Message;
                            p.Quantity = item.NewQuantity;
                        }
                    }
                }

                foreach (var productRemove in OrderContext.Order.AsOrder().OrderCustomers[0].OrderItems)
                {
                    objOrderItemsOld.Add(new OrderItemUpdate() { Product = productRemove.ProductID.Value, Quantity = productRemove.Quantity, status = false });
                }
                string outOfStockMessage = "";
                if (messageValidKit != "")
                {
                    outOfStockMessage = messageValidKit;
                }

                RemovePaymentsTable(0);
                OrderContext.Order.AsOrder().OrderCustomers[0].RemoveAllPayments();

                OrderService.UpdateOrderItemQuantities(OrderContext, changes);
                ApplyPaymentPreviosBalance();

                int WareHouseId = Convert.ToInt32(Session["WareHouseId"]);
                OrderContext.Order.ParentOrderID = WareHouseId;
                OrderService.UpdateOrder(OrderContext);

                //Optener los order item despues de actualizar el producto
                List<ConfigKit> objOrderItemsNew = new List<ConfigKit>();

                foreach (var productRemove in OrderContext.Order.AsOrder().OrderCustomers[0].OrderItems)
                {
                    objOrderItemsNew.Add(new ConfigKit() { MaterialID = productRemove.ProductID.Value, Available = productRemove.Quantity });
                }
                //Recorrer los OrderItem Actual
                foreach (var item in objOrderItemsNew)
                {
                    //Consultar y actualizar los orden item antiguos
                    foreach (var ItemStart in objOrderItemsOld.Where(x => x.Product == item.MaterialID))
                    {
                        ItemStart.status = true;
                    }
                }
                foreach (var item in objOrderItemsNew)
                {
                    if (!objOrderItemsOld.Any(X => X.Product == item.MaterialID))
                    {
                        //Insertar el nuevo items
                        Order.GenerateAllocation(item.MaterialID,
                                                              item.Available,
                                                              OrderContext.Order.AsOrder().OrderID,
                                                              Convert.ToInt32(Session["WareHouseId"]),
                                                              EntitiesEnums.MaintenanceMode.Add,
                                                              Convert.ToInt32(Session["PreOrder"]),
                                                              CoreContext.CurrentAccount.AccountTypeID,
                                                              false);

                    }
                }
                foreach (var itemOld in objOrderItemsOld)
                {
                    if (!itemOld.status)
                    {
                        //Eliminarlo
                        Order.GenerateAllocation(itemOld.Product,
                                                 itemOld.Quantity,
                                                 OrderContext.Order.AsOrder().OrderID,
                                                 Convert.ToInt32(Session["WareHouseId"]),
                                                 EntitiesEnums.MaintenanceMode.Delete,
                                                 Convert.ToInt32(Session["PreOrder"]),
                                                 CoreContext.CurrentAccount.AccountTypeID,
                                                 false);
                    }
                }
                //OrderContext.Order.AsOrder().Balance = OrderContext.Order.AsOrder().GrandTotal; 
                //calculateTotalDispatch();
                return Json(new
                {
                    result = true,
                    OrderEntryModelData = GetOrderEntryModelData(OrderContext),
                    promotions = GetApplicablePromotions(OrderContext),
                    shippingMethods = ShippingMethods,
                    BundleOptionsSpanHTML = GetDynamicBundleUpSale(),
                    AvailableBundleCount = _availableBundleCount,
                    outOfStockItems = GetOutOfStockItemsString(OrderContext.Order.AsOrder()),
                    message = outOfStockMessage,
                    paymentsGrid = RenderRazorPartialViewToString("OEPaymentsGrid", OrderContext.Order.AsOrder()),
                    totals = Totals
                });
            }
            catch (Exception ex)
            {
                var message = ex.Log(orderID: OrderContext.Order.OrderID, accountID: CurrentAccount.AccountID).PublicMessage;
                return JsonError(message);
            }
        }

        [FunctionFilter("Orders", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public virtual ActionResult RemoveFromCart(string orderItemGuid, string parentGuid = null, int? quantity = null)
        {
            try
            {
                Guid oiGuid = Guid.Parse(orderItemGuid);
                var customer = (OrderCustomer)OrderContext.Order.OrderCustomers.First();

                var item = customer.OrderItems.First(oi => ((OrderItem)oi).Guid == oiGuid);
                var dynamicKitGroupId = ((OrderItem)item).DynamicKitGroupID;
                int Valuequantity = 0;

                Valuequantity = item.Quantity;
                List<OrderItemUpdate> objOrderItemsOld = new List<OrderItemUpdate>();

                foreach (var productRemove in OrderContext.Order.AsOrder().OrderCustomers[0].OrderItems)
                {
                    objOrderItemsOld.Add(new OrderItemUpdate() { Product = productRemove.ProductID.Value, Quantity = productRemove.Quantity, status = false });
                }
                var removeModification = Create.New<IOrderItemQuantityModification>();
                removeModification.Customer = customer;
                removeModification.ModificationKind = OrderItemQuantityModificationKind.Delete;
                removeModification.ProductID = item.ProductID.Value;
                removeModification.Quantity = 0;
                removeModification.ExistingItem = item;

                var changes = new IOrderItemQuantityModification[] { removeModification };
                OrderService.UpdateOrderItemQuantities(OrderContext, changes);
                RemovePaymentsTable(0);
                OrderContext.Order.AsOrder().OrderCustomers[0].RemoveAllPayments();
                ApplyPaymentPreviosBalance();
                OrderService.UpdateOrder(OrderContext);

                OrderItem bundleItem = null;

                Guid pGuid;
                if (dynamicKitGroupId.HasValue && !String.IsNullOrWhiteSpace(parentGuid) && Guid.TryParse(parentGuid, out pGuid))
                {
                    bundleItem = customer.OrderItems.FirstOrDefault(oi => oi.Guid == pGuid);
                }

                bool isBundleGroupComplete = false;
                if (bundleItem != null)
                {
                    var bundleProduct = Inventory.GetProduct(bundleItem.ProductID.Value);
                    var group = bundleProduct.DynamicKits[0].DynamicKitGroups.Where(g => g.DynamicKitGroupID == dynamicKitGroupId).First();
                    int currentCount = bundleItem.ChildOrderItems.Where(oi => oi.DynamicKitGroupID == dynamicKitGroupId).Sum(oi => oi.Quantity);
                    isBundleGroupComplete = currentCount == group.MinimumProductCount;
                }

                int ppt = OrderContext.Order.AsOrder().OrderCustomers[0].ProductPriceTypeID;

                foreach (var ItemsActual in OrderContext.Order.AsOrder().OrderCustomers[0].OrderItems)
                {
                    foreach (var items in objOrderItemsOld.Where(X => X.Product == ItemsActual.ProductID))
                    {
                        items.status = true;
                    }
                }

                List<ConfigKit> objOrderItemsNew = new List<ConfigKit>();

                foreach (var productRemove in OrderContext.Order.AsOrder().OrderCustomers[0].OrderItems)
                {
                    objOrderItemsNew.Add(new ConfigKit() { MaterialID = productRemove.ProductID.Value, Available = productRemove.Quantity });
                }

                foreach (var items in objOrderItemsNew)
                {
                    if (objOrderItemsOld.Where(X => X.Product == items.MaterialID).Count() == 0)
                    {
                        //Insertar el nuevo items
                        Order.GenerateAllocation(items.MaterialID,
                                                              items.Available,
                                                              OrderContext.Order.AsOrder().OrderID,
                                                              Convert.ToInt32(Session["WareHouseId"]),
                                                              EntitiesEnums.MaintenanceMode.Add,
                                                              Convert.ToInt32(Session["PreOrder"]),
                                                              CoreContext.CurrentAccount.AccountTypeID,
                                                              false);

                    }
                }

                foreach (var productRemove in objOrderItemsOld)
                {
                    if (!productRemove.status)
                    {
                        Order.GenerateAllocation(productRemove.Product,
                                                 productRemove.Quantity,
                                                 OrderContext.Order.AsOrder().OrderID,
                                                 Convert.ToInt32(Session["WareHouseId"]),
                                                 EntitiesEnums.MaintenanceMode.Delete,
                                                 Convert.ToInt32(Session["PreOrder"]),
                                                 CoreContext.CurrentAccount.AccountTypeID,
                                                 false);

                    }
                }
                return Json(new
                {
                    result = true,
                    OrderEntryModelData = GetOrderEntryModelData(OrderContext),
                    promotions = GetApplicablePromotions(OrderContext),
                    outOfStockItems = GetOutOfStockItemsString(OrderContext.Order.AsOrder()),
                    shippingMethods = ShippingMethods,
                    groupItemsHtml = parentGuid == null ? "" : GetGroupItemsHtml(parentGuid, dynamicKitGroupId.Value).ToString(),
                    isBundleGroupComplete = isBundleGroupComplete,
                    childItemCount = parentGuid == null ? 0 : customer.OrderItems.GetByGuid(parentGuid).ChildOrderItems.Sum(c => c.Quantity),
                    orderCustomerId = customer.OrderCustomerID,
                    BundleOptionsSpanHTML = GetDynamicBundleUpSale(),
                    AvailableBundleCount = _availableBundleCount,
                    paymentsGrid = RenderRazorPartialViewToString("OEPaymentsGrid", OrderContext.Order.AsOrder()),
                    totals = Totals
                });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, (OrderContext.Order != null) ? OrderContext.Order.OrderID.ToIntNullable() : null, accountID: (CurrentAccount != null) ? CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [FunctionFilter("Orders", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public ActionResult SaveBundle(string bundleGuid)
        {
            try
            {
                var customer = OrderContext.Order.AsOrder().OrderCustomers.FirstOrDefault();
                var orderItem = customer.OrderItems.GetByGuid(bundleGuid);
                var kitProduct = Inventory.GetProduct(orderItem.ProductID.Value);
                if (kitProduct == null)
                    return Json(new { result = false, message = string.Format("Could not find a product with SKU '{0}'", kitProduct.SKU) });

                if (!Order.IsDynamicKitValid(orderItem))
                    return Json(new { result = false, message = Translation.GetTerm("TheBundleYouTriedToOrderIsNotComplete", "The bundle you tried to order ({0}) is not complete.", kitProduct.Translations.Name()) });

                OrderService.UpdateOrder(OrderContext);

                return Json(new
                {
                    result = true,
                    itemsInCart = customer.OrderItems.Count,
                    total = OrderContext.Order.Subtotal.ToString(OrderContext.Order.CurrencyID),
                    productName = kitProduct.Translations.Name(),
                    image = kitProduct.MainImage == null ? "" : kitProduct.MainImage.FilePath.ReplaceFileUploadPathToken()//,
                    //orderItems = GetFormattedCartPreview().ToString()
                });
            }
            catch (Exception ex)
            {
                NetStepsException exception;
                if (!(ex is NetStepsBusinessException))
                    exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                else
                    exception = ex as NetStepsException;
                return Json(new { result = false, message = exception.PublicMessage });
            }

        }
        #endregion

        #region Addresses
        [FunctionFilter("Orders", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public virtual ActionResult GetAddresses()
        {
            try
            {
                StringBuilder addresses = new StringBuilder();
                StringBuilder options = new StringBuilder();

                foreach (Address address in CurrentAccount.Addresses.Where(a => a.AddressTypeID == NetSteps.Data.Entities.Constants.AddressType.Shipping.ToInt()).OrderByDescending(a => a.IsDefault).ThenBy(a => a.AddressID))
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
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, (OrderContext.Order != null) ? OrderContext.Order.OrderID.ToIntNullable() : null, accountID: (CurrentAccount != null) ? CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [FunctionFilter("Orders", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public virtual ActionResult ChangeShippingAddress(int shippingAddressId, int paymentMethodID = 0)
        {
            try
            {
                Session["vControlGenShipping"] = false;
                ValidatePaymentByMarket();
                OrderContext.Order.AsOrder().OrderCustomers[0].RemoveAllPayments();
                //NetSteps.Data.Entities.Business.CTE.paymentTables.RemoveAll();
                RemovePaymentsTable(0);
                ApplyPaymentPreviosBalance();
                //if (NetSteps.Data.Entities.Business.CTE.paymentTables.Count > 0)  ----  pendiente
                //    OrderService.UpdateOrder(OrderContext);

                TempData["sPaymentMethod"] = from x in Order.SelectPaymentMethod(CurrentAccount.AccountID, OrderContext.Order.AsOrder().OrderTypeID)
                                             orderby x.Key
                                             select new SelectListItem()
                                             {
                                                 Text = x.Value,
                                                 Value = x.Key
                                             };
                //string valueTotal = Totals.GetType().GetProperty("commissionableTotal").GetValue(Totals, null).ToString();
                string valueTotal = Totals.GetType().GetProperty("subtotalAdjusted").GetValue(Totals, null).ToString();
                bool isPopup = false;
                int WareHouseIdSession = Convert.ToInt32(Session["WareHouseId"]);
                int newWareHouseIdSession = Order.WareHouseByAccountAddress(Convert.ToInt32(Session["AccountID"]), shippingAddressId);
                OrderShipment shipment = OrderContext.Order.AsOrder().GetDefaultShipment();
                if (valueTotal != "$0.00")
                {
                    //if (Convert.ToInt32(Session["WareHouseId"]) != Order.WareHouseByAccountAddress(Convert.ToInt32(Session["AccountID"]), shippingAddressId))
                    if (WareHouseIdSession != newWareHouseIdSession) isPopup = true;
                }
                var viewData = new ViewDataDictionary();
                var paymentMethodsBlock = RenderRazorPartialViewToString("_PaymentMethods", OrderContext.Order.AsOrder(), viewData);

                if (OrderContext.Order.AsOrder().OrderCustomers[0].OrderItems.Count() > 0)
                {
                    viewData["PaymentMethodID"] = paymentMethodID;
                    if (WareHouseIdSession == newWareHouseIdSession)
                    {
                        UpdateOrderShipmentAddress(shipment, shippingAddressId);
                        Session["WareHouseId"] = newWareHouseIdSession;
                        return Json(new { result = true, shippingMethods = ShippingMethods, totals = Totals, paymentMethodsBlock = paymentMethodsBlock, isPopup = isPopup, paymentsGrid = RenderRazorPartialViewToString("OEPaymentsGrid", OrderContext.Order.AsOrder()) });
                        //return Json(new { result = true, isPopup = isPopup, shippingMethods = ShippingMethods });
                    }
                    else
                    {
                        var shippingAddresID = OrderContext.Order.AsOrder().OrderCustomers[0].OrderShipments;
                        int SourceAddressID = 0;
                        foreach (var item in shippingAddresID)
                        {
                            SourceAddressID = item.SourceAddressID.Value;
                        }
                        return Json(new { result = false, isPopup = isPopup, shippingMethods = ShippingMethods, totals = Totals, paymentMethodsBlock = paymentMethodsBlock, sourceAddressID = SourceAddressID, paymentsGrid = RenderRazorPartialViewToString("OEPaymentsGrid", OrderContext.Order.AsOrder()) });
                    }
                }
                Session["WareHouseId"] = newWareHouseIdSession;
                UpdateOrderShipmentAddress(shipment, shippingAddressId);
                return Json(new { result = true, shippingMethods = ShippingMethods, totals = Totals, paymentMethodsBlock = paymentMethodsBlock, isPopup = isPopup, paymentsGrid = RenderRazorPartialViewToString("OEPaymentsGrid", OrderContext.Order.AsOrder()) });


            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, (OrderContext.Order != null) ? OrderContext.Order.OrderID.ToIntNullable() : null, accountID: (CurrentAccount != null) ? CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
        #endregion

        #region Shipping Method
        [FunctionFilter("Orders", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public virtual ActionResult SetShippingMethod(int shippingMethodId)
        {
            try
            {
                IEnumerable<ShippingMethodWithRate> shippingMethods = null;
                OrderShipment shipment = OrderContext.Order.AsOrder().GetDefaultShipment();
                shippingMethods = OrderContext.Order.AsOrder().GetShippingMethods(shipment);
                foreach (var item in shippingMethods.Where(x => x.ShippingMethodID == shippingMethodId))
                {
                    Session["DateEstimated"] = item.DateEstimated;
                }
                OrderContext.Order.AsOrder().SetShippingMethod(shippingMethodId);
                OrderService.UpdateOrder(OrderContext);

                //TODO: Fix json returns.
                return Json(new { result = true, totals = Totals, dateEstimate = Convert.ToString(Session["DateEstimated"]) });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, (OrderContext.Order != null) ? OrderContext.Order.OrderID.ToIntNullable() : null, accountID: (CurrentAccount != null) ? CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
        #endregion

        #region Payments
        [FunctionFilter("Orders", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public virtual ActionResult GetPaymentMethods()
        {
            try
            {
                SetProductCreditViewData();

                return Json(new { paymentMethodsBlock = RenderRazorPartialViewToString("_PaymentMethods", OrderContext.Order.AsOrder()) });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, (OrderContext.Order != null) ? OrderContext.Order.OrderID.ToIntNullable() : null, accountID: (CurrentAccount != null) ? CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [HttpPost]
        [FunctionFilter("Orders", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public virtual ActionResult ApplyPayment(string NameOnCard, int paymentMethodId, string amountConfiguration,
              string giftCardCode = null)
        {
            try
            {
                Order order = OrderContext.Order.AsOrder();
                var numberOfItems = (order.OrderCustomers.Sum(oc => oc.ParentOrderItems.Sum(oi => oi.Quantity)));
                if (numberOfItems == 0)
                    return JsonError(Translation.GetTerm("Payment_ValidCantProduct", "Debe de tener al menos un producto"));

                var IsCredit = ValidateAccountCredit(paymentMethodId);

                if (IsCredit > 0)
                {
                    if (IsCredit == 1)
                        return JsonError(Translation.GetTerm("Payment_ValidCanNotMethod", "No es posible utilizar el crédito, seleccione otro medio de pago"));

                    if (IsCredit == 2)
                        return JsonError(Translation.GetTerm("Payment_ValidOrderExceeds", "La compra excede su crédito disponible. Modifique la orden o seleccione otro medio de pago"));

                }

                //==========================================================//               
                decimal amount = 0;
                amount = NetSteps.Data.Entities.Extensions.DecimalExtensions.FormatGlobalizationDecimal(amountConfiguration);
                //==========================================================//
                BasicResponseItem<OrderPayment> response = new BasicResponseItem<OrderPayment>();
                IPayment payment = new Payment();

                payment.BankName = NameOnCard;
                //==========================================================//
                // Para Brasil('N') el flujo sigue su curso normal , mientras que para USA('S')  tiene que ser siempre el PaymentTypeID = 1;
                //if (Session["GeneralParameterVal"].ToString() == "N")
                //    payment.PaymentTypeID = Order.GetApplyPaymentType(paymentMethodId);
                //else
                //    payment.PaymentTypeID = 1;
                payment.PaymentTypeID = Order.GetApplyPaymentType(paymentMethodId);
                //==========================================================//
                response = OrderContext.Order.AsOrder().ApplyPaymentToCustomers(payment.PaymentTypeID, amount, payment, user: CoreContext.CurrentUser);

                if (response.Success)
                {

                    //==========================================================//
                    ApplyPaymentSearchData objE = new ApplyPaymentSearchData();
                    objE.OrderPaymentId = OrderContext.Order.AsOrder().OrderPayments.Count() * (-1);//response.Item.OrderPaymentID;
                    objE.Amount = amount;
                    objE.PaymentConfigurationID = paymentMethodId;
                    objE.NameOnCard = "";
                    if (payment.PaymentTypeID > 1)
                    {
                        objE.NumberCuota = null;
                    }
                    else
                    {
                        var numberTarget = NetSteps.Data.Entities.Business.PaymentMethods.GetNumberCuotasByPaymentConfigurationID(paymentMethodId);
                        objE.NumberCuota = numberTarget;  //tarjeta de una cuota , pero puede ser de2 cuotas 
                    }

                    objE.PreOrderID = Convert.ToInt32(Session["PreOrder"]);
                    NetSteps.Data.Entities.Business.CTE.ApplyPayment(objE);
                    //==========================================================//
                    OrderService.UpdateOrder(OrderContext);
                    var paymentResponse = response.Item;

                    var NumCuotas = IsCreditCard(paymentMethodId);
                    return Json(new
                    {
                        result = true,
                        message = string.Empty,
                        totals = Totals,
                        OrderEntryModelData = GetOrderEntryModelData(OrderContext),
                        paymentsGrid = RenderRazorPartialViewToString("OEPaymentsGrid", OrderContext.Order.AsOrder()),
                        paymentId = paymentResponse == null ? 0 : paymentResponse.OrderPaymentID,
                        numberTarget = NumCuotas,
                        amount = amountConfiguration
                    });
                }
                else
                {
                    return Json(new { result = false, message = response.Message });
                }
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, (OrderContext.Order != null) ? OrderContext.Order.OrderID.ToIntNullable() : null, accountID: (CurrentAccount != null) ? CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [HttpPost]
        [FunctionFilter("Orders", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public virtual ActionResult RemovePayment(string paymentId, int indice)
        {
            try
            {
                //NetSteps.Data.Entities.Business.CTE.paymentTables.RemoveWhere(x => x.OrderPaymentId == indice);
                RemovePaymentsTable(indice);
                //var customer = OrderContext.Order.AsOrder().OrderCustomers.FirstOrDefault();
                //var orderPayment = customer.OrderPayments.FirstOrDefault(oi => oi.Guid.ToString("N") == paymentId);
                //if (orderPayment == null)
                //    return Json(new
                //    {
                //        result = false,
                //        message = Translation.GetTerm("PaymentDoesNotExist", "That payment does not exist.")
                //    });
                //customer.RemovePayment(orderPayment);
                //OrderService.UpdateOrder(OrderContext);
                var customer = OrderContext.Order.AsOrder().OrderCustomers.FirstOrDefault();
                customer.RemovePayment(paymentId);
                OrderService.UpdateOrder(OrderContext);
                return Json(new
                {
                    result = true,
                    totals = Totals,
                    OrderEntryModelData = GetOrderEntryModelData(OrderContext)
                });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, (OrderContext.Order != null) ? OrderContext.Order.OrderID.ToIntNullable() : null, accountID: (CurrentAccount != null) ? CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public void RemovePaymentsTable(int Indice)
        {
            PaymentsTable objE = new PaymentsTable();
            objE.ubic = 0;
            objE.PreOrderID = Convert.ToInt32(Session["PreOrder"]);
            objE.OrderPaymentId = Indice;
            PaymentsMethodsExtensions.UpdPaymentsTable(objE);
        }

        [HttpPost]
        public virtual ActionResult LookupGiftCardBalance(string giftCardCode)
        {
            try
            {
                var gcService = Create.New<IGiftCardService>();

                decimal? balance = gcService.GetBalanceWithPendingPayments(giftCardCode, OrderContext.Order);
                return !balance.HasValue
                    ? this.Json(new { result = false, message = Translation.GetTerm("GiftCardNotFound", "Gift card not found") })
                    : this.Json(new { result = true, balance = balance.Value.ToString(((Order)this.OrderContext.Order).CurrencyID), amountToApply = Math.Min(balance.Value, ((Order)this.OrderContext.Order).Balance ?? 0m) });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, (OrderContext.Order != null) ? OrderContext.Order.OrderID.ToIntNullable() : null, accountID: (CurrentAccount != null) ? CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
        #endregion

        [FunctionFilter("Orders", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public virtual ActionResult SubmitOrder(int paymentMethodId)
        {
            try
            {
                /* PayPal_001
                 * Se agrega la funcionalidad para validar que el pago con tarjeta de credito se halla realizado
                 * Author : JMorales
                 * Date   : 01/09/2016
                 */

                if (!validaPayPal_Paid())
                {
                    return Json(new { result = false, message = Translation.GetTerm("NoPayPalPaid", "Erro de pagamento processamento, tente novamente") });
                }

                /* End PayPal_001*/


                ////csti(mescoobar)-EB-486-Fin

                OrderContext.Order.AsOrder().StartEntityTracking();

                // Remove all the Negative OrderItemIDs used in UI - JHE
                foreach (var orderItem in OrderContext.Order.AsOrder().OrderCustomers[0].OrderItems.Where(oi => oi.OrderItemID < 0))
                {
                    orderItem.StopTracking();
                    orderItem.OrderItemID = 0;
                    orderItem.StartTracking();
                }


                OrderShipment shipments = OrderContext.Order.AsOrder().OrderShipments[0]; //OrderContext.Order.AsOrder().GetDefaultShipment();

                var shippingMethods = OrderContext.Order.AsOrder().GetShippingMethods(shipments);
                if (shippingMethods != null)
                    shippingMethods = shippingMethods.OrderBy(sm => sm.ShippingAmount).ToList();
                if (shippingMethods.Count() == 0)
                {
                    return Json(new { result = false, message = Translation.GetTerm("NoShippingAddress", "There is no shipping address.  Please choose an address to ship to.") });
                }

                OrderContext.Order.AsOrder().OrderPendingState = NetSteps.Data.Entities.Constants.OrderPendingStates.Quote;


                PaymentsTable objE = new PaymentsTable();
                objE.PreOrderID = Convert.ToInt32(Session["PreOrder"]);
                var paymentTables = Order.GetPaymentsTable(objE);

                int contadorMenos = 0;
                foreach (var item in OrderContext.Order.AsOrder().OrderPayments)
                {
                    contadorMenos--;

                    foreach (var paymentTable in paymentTables)
                    {


                        if (contadorMenos == paymentTable.OrderPaymentId)
                        {


                            if (paymentTable.PaymentStatusID == 4)
                            {
                                item.OrderPaymentStatusID = 2;
                            }
                            else if (paymentTable.PaymentStatusID == 11 || paymentTable.PaymentStatusID == 18)
                            {
                                item.OrderPaymentStatusID = 1;
                            }


                            if (item.TransactionID == null)
                                item.TransactionID = paymentTable.AutorizationNumber;

                            item.PaymentTypeID = paymentTable.PaymentType;
                            item.ExpirationDateUTC = paymentTable.ExpirationDate;
                            item.DateLastModifiedUTC = DateTime.Now;
                            item.CreditCardTypeID = null;
                            item.NameOnCard = null;
                            item.BillingCity = null;
                            item.BillingCountryID = null;
                            item.BillingState = null;
                            item.BillingStateProvinceID = null;
                            item.BillingPostalCode = null;
                            item.BillingCountry = null;
                            item.BillingPostalCode = null;
                            item.BillingPhoneNumber = null;
                            item.IdentityNumber = null;
                            item.IdentityState = null;
                            item.RoutingNumber = null;
                            item.DeferredAmount = (paymentTable.NumberCuota.HasValue) ? paymentTable.NumberCuota : 0;
                            item.DeferredTransactionID = null;
                            //item.DataVersion = null;
                            item.Request = null;
                            item.AccountNumberLastFour = null;

                            item.SourceAccountPaymentMethodID = null;
                            item.BankAccountTypeID = null;
                            item.BankName = null;
                            item.NachaClassType = null;
                            item.NachaSentDate = null;
                            item.ExpirationStatusID = (int)ConstantsGenerated.ExpirationStatuses.Unexpired;
                        }
                    }
                }
                actBalanceDue();

                int shippingOrderType = OrderContext.Order.AsOrder().OrderShipments[0].ShippingMethodID.Value;
                OrderContext.Order.AsOrder().OrderShipments[0].ShippingMethodID = Order.GetShippingMethodID(shippingOrderType);
                OrderContext.Order.AsOrder().ParentOrderID = null;
                var result = OrderService.SubmitOrder(OrderContext);

                if (!result.Success)
                {
                    // CSTI(mescobar)-02/02/2016-Inicio
                    //Se agregro el parametro validrule = false
                    return Json(new { result = false, validrule = false, message = result.Message, paymentsGrid = RenderRazorPartialViewToString("OEPaymentsGrid", OrderContext.Order.AsOrder()) });
                    // CSTI(mescobar)-02/02/2016-Fin
                }
                else
                {
                    try
                    {
                        //========================================================================================
                        //Actualizar los campos nuevos  en la tabla de OrderPayments
                        contadorMenos = 0;
                        foreach (var item in OrderContext.Order.AsOrder().OrderPayments)
                        {

                            contadorMenos--;

                            foreach (var paymentTable in paymentTables)
                            {

                                if (contadorMenos == paymentTable.OrderPaymentId)
                                {
                                    OrderPaymentsParameters objEP = new OrderPaymentsParameters();
                                    objEP.InitialAmount = paymentTable.AppliedAmount.ToDecimal();
                                    objEP.ProcessedDateUTC = null; objEP.ProcessOnDateUTC = null;
                                    if (paymentTable.PaymentStatusID.Equals(4))
                                    {
                                        objEP.ProcessedDateUTC = DateTime.Now;
                                        objEP.ProcessOnDateUTC = DateTime.Now;
                                    }
                                    objEP.ModifiedByUserID = CoreContext.CurrentUser == null ? CurrentAccount.UserID.ToInt() : CoreContext.CurrentUser.UserID;
                                    objEP.ProductCredit = decimal.Parse(Session["ProductCredit"].ToString());


                                    //===================================================================//
                                    //Actualizamos los datos 
                                    int filasAfectadas = Order.UPDPaymentConfigurations(item.OrderPaymentID,
                                        item.OrderID,
                                        paymentTable.PaymentConfigurationID.Value,
                                        paymentTable.NumberCuota
                                        , objEP, OrderExtensions.GeneralParameterVal(CoreContext.CurrentMarketId, "ANM")
                                       );

                                    //--------------------------------------------------------------------//
                                    if ((decimal.Parse(Session["ProductCredit"].ToString()) != 0) && paymentTable.PaymentConfigurationID.Value == 60)
                                    {
                                        decimal productCredit = Order.GetProductCreditByAccount(OrderContext.Order.AsOrder().ConsultantID);
                                        var endingAmount = CreditApplied();
                                        decimal EntryAmount = endingAmount - productCredit;

                                        int EntryReasonID = (endingAmount > 0) ? 5 : 10;
                                        int EntryTypeID = Constants.LedgerEntryOrigins.OrderEntry.ToInt();
                                        int EntryOrigin = Constants.LedgerEntryOrigins.OrderEntry.ToInt();

                                        NetSteps.Data.Entities.Business.CTE.ApplyCredit(CurrentAccount.AccountID, EntryReasonID, EntryOrigin, EntryTypeID, EntryAmount, item.Order.AsOrder().OrderID, item.OrderPaymentID);
                                    }

                                    ValidateAccountCredit(paymentTable.PaymentConfigurationID.Value);
                                    var tb = NetSteps.Data.Entities.Business.CTE.creditPayment;
                                    if (tb.ValorComparacion.ToDecimal() > 0 && tb.AfectaCredito == "S")
                                    {
                                        decimal amount = 0;
                                        var KeyDecimals = ConfigurationManager.AppSettings["CultureDecimal"];
                                        if (KeyDecimals == "ES")
                                        {
                                            var culture = CultureInfoCache.GetCultureInfo("En");
                                            amount = Convert.ToDecimal(tb.ValorComparacion, culture);

                                        }
                                        Order.UPDAccountCredit(CurrentAccount.AccountID, amount);
                                    }
                                }
                            }
                        }

                        //========================================================================================
                        // SPOnLineMLM ( BDcommissions) solo cuando la Orden ya esta Pagada ( status: 4 )
                        var OrderStatusID = AccountPropertiesBusinessLogic.GetValueByID(11, OrderContext.Order.OrderID).OrderStatusID;
                        if (OrderStatusID == Constants.OrderStatus.Paid.ToShort())
                        {
                            PersonalIndicardorAsynExtensions personalIndicardorAsyn = new PersonalIndicardorAsynExtensions();
                            personalIndicardorAsyn.UpdatePersonalIndicatorAsyn(OrderContext.Order.OrderID, OrderStatusID);
                            //OrderExtensions.UpdatePersonalIndicator(OrderContext.Order.OrderID, OrderStatusID);
                            //INI - GR6356 - CDAS              
                            var account = NetSteps.Data.Entities.Account.Load(OrderContext.Order.OrderCustomers[0].AccountID);
                            if (account != null && account.AccountStatusID != (short)Constants.AccountStatus.Active)
                            {
                                account.AccountStatusID = (short)Constants.AccountStatus.Active;
                                account.EnrollmentDateUTC = DateTime.Now;
                                account.Save();
                            }
                            //FIN - GR6356 - CDAS
                        }
                        //========================================================================================

                        //INI - GR6356 - CDAS
                        if (OrderStatusID == (short)Constants.OrderStatus.PendingPerPaidConfirmation)
                        {
                            var account = NetSteps.Data.Entities.Account.Load(OrderContext.Order.OrderCustomers[0].AccountID);
                            if (account != null && account.AccountStatusID == (short)Constants.AccountStatus.BegunEnrollment)
                            {
                                account.EnrollmentDateUTC = null;
                                account.Save();
                            }
                        }
                        //FIN - GR6356 - CDAS

                        var orderID = OrderContext.Order.AsOrder().OrderID;
                        var postalCode = OrderContext.Order.AsOrder().OrderShipments[0].PostalCode;
                        var warehouseID = Convert.ToInt32(Session["WareHouseId"]);
                        var dateEstimated = Convert.ToString(Session["DateEstimated"]);

                        SetKitItemPrices(OrderContext.Order);

                        var resultOrderShipments = PaymentsMethodsExtensions.ManagementKit(postalCode,
                            warehouseID,
                            orderID,
                            dateEstimated);

                        insertDispatchProductsFill(); // wv: 20160603 inserta los Dispatch de productos filtrados para el account
                    }
                    catch (Exception ex)
                    {
                        throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
                    }
                }

                //Registrar los claims
                var listInventoryByClaims = OrderExtensions.GetInventoryByClaims(Convert.ToInt32(Session["PreOrder"]));
                foreach (var item in listInventoryByClaims)
                {
                    int OrderItemID = OrderExtensions.GetOrderItemClaims(item.ProductID, Convert.ToInt32(Session["OrderClaimID"]));
                    int OrderItemClaimsID = OrderExtensions.InsOrderItemByClaims(OrderItemID, OrderContext.Order.AsOrder().OrderCustomers[0].OrderCustomerID);
                    int resultOrderItem = Order.UPDOrderItemClaimsProduct(OrderItemClaimsID, OrderContext.Order.AsOrder().OrderID);
                }

                //===================================================================//
                //Now that this order has been submitted. Remove the order from session.
                var scheduler = Create.New<IEventScheduler>();
                scheduler.ScheduleOrderCompletionEvents(OrderContext.Order.AsOrder().OrderID);

                var orderNumber = OrderContext.Order.AsOrder().OrderNumber;
                var paymentsGrid = RenderRazorPartialViewToString("OEPaymentsGrid", OrderContext.Order.AsOrder());

                OrderContext.Clear();

                //Order.AsignarReservasAOrden(); 
                return Json(new { result = true, orderNumber = orderNumber, paymentsGrid = paymentsGrid });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, OrderContext.Order != null ? OrderContext.Order.OrderID.ToIntNullable() : null, accountID: CurrentAccount != null ? CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, validrule = false, message = exception.PublicMessage, paymentsGrid = RenderRazorPartialViewToString("OEPaymentsGrid", OrderContext.Order.AsOrder()) });
            }
        }


        private void SetKitItemPrices(IOrder order)
        {
            OrderService.AddKitItemPrices(order);
        }

        //
        //BR-CC-007 KTC
        private decimal CreditApplied()
        {

            var KeyDecimals = ConfigurationManager.AppSettings["CultureDecimal"];

            var getBalanceDue = Totals.GetType().GetProperty("balanceDue").GetValue(Totals, null);
            string[] strBalanceDue = Convert.ToString(getBalanceDue).Split('$');
            decimal balancDue = 0;
            balancDue = NetSteps.Data.Entities.Extensions.DecimalExtensions.FormatGlobalizationDecimal(Convert.ToDecimal(strBalanceDue[1].ToString()));


            return balancDue;
        }
        //private void CreateTickets()
        //{
        //    var _CreditApplied = Order.GetCredit();
        //    var _orderID = OrderContext.Order.AsOrder().OrderID;
        //    var _AccountID = CurrentAccount.AccountID;
        //    var payments = CTE.paymentTables;


        //    var _orderPayment = OrderPayment.LoadOrderPaymentByOrderId(_orderID);
        //    OrderPaymentsParameters parameters = new OrderPaymentsParameters();
        //    DateTime _dateCurrent = DateTime.Now;
        //    int _orderPaymentStatusID = 0;
        //    foreach (var item in payments.Where(x => x.PaymentConfigurationID > 0).ToList())
        //    {
        //        if (item.PaymentStatusID == 4)
        //            _orderPaymentStatusID = Constants.OrderPaymentStatus.Completed.ToInt();
        //        if (item.PaymentStatusID == 12 || item.PaymentStatusID == 9)
        //            _orderPaymentStatusID = Constants.OrderPaymentStatus.Pending.ToInt();

        //        var _paymentConfigurations = AccountPropertiesBusinessLogic.GetValueByID(2, item.PaymentConfigurationID.ToInt());
        //        var ExpirationDate = (item.ExpirationDate.HasValue) ? item.ExpirationDate.ToDateTime() : DateTime.Now;

        //        parameters = new OrderPaymentsParameters();
        //        parameters.PaymentConfigurationID = item.PaymentConfigurationID.ToInt();
        //        parameters.FineAndInterestRulesID = _paymentConfigurations.FineAndInterestRulesID;
        //        //parameters.TicketNumber = 0;  este campo debe ser el mismo que su Id del registro generado
        //        parameters.OrderID = _orderID;
        //        parameters.OrderCustomerID = int.Parse(_orderPayment.OrderCustomerID.ToString());
        //        parameters.CurrencyID = OrderContext.Order.AsOrder().CurrencyID;
        //        parameters.OrderPaymentStatusID = _orderPaymentStatusID;
        //        parameters.OriginalExpirationDateUTC = ExpirationDate;
        //        parameters.CurrentExpirationDateUTC = ExpirationDate;
        //        parameters.InitialAmount = item.AppliedAmount.ToDecimal();
        //        parameters.TotalAmount = item.AppliedAmount.ToDecimal();
        //        parameters.DateLastTotalAmountUTC = DateTime.Now;
        //        parameters.ExpirationStatusID = Constants.ExpirationStatus.Unexpired.ToInt();
        //        parameters.NegotiationLevelID = Constants.NegotiationLevel.Original.ToInt();
        //        parameters.IsDeferred = true;
        //        parameters.ProcessOnDateUTC = _dateCurrent;
        //        parameters.ProcessedDateUTC = _dateCurrent;
        //        parameters.TransactionID = "";
        //        parameters.ModifiedByUserID = CoreContext.CurrentUser.UserID;
        //        parameters.PaymentGatewayID = AccountPropertiesBusinessLogic.GetValueByID(1, _paymentConfigurations.CollectionEntityID).PaymentGateway;
        //        parameters.DateCreatedUTC = _dateCurrent;
        //        parameters.DateLastModifiedUTC = _dateCurrent;
        //        parameters.PaymentTypeID = Constants.PaymentType.ProductCredit.ToInt();

        //        OrderPaymentsBusinessLogic.Insert(parameters);

        //        if (_CreditApplied > 0)
        //        {
        //            parameters = new OrderPaymentsParameters();
        //            //parameters.TicketNumber = 0;  este campo debe ser el mismo que su Id del registro generado
        //            parameters.OrderID = _orderID;
        //            parameters.OrderCustomerID = int.Parse(_orderPayment.OrderCustomerID.ToString());
        //            parameters.CurrencyID = OrderContext.Order.AsOrder().CurrencyID;
        //            parameters.OrderPaymentStatusID = Constants.OrderPaymentStatus.Completed.ToInt();
        //            parameters.OriginalExpirationDateUTC = ExpirationDate;
        //            parameters.CurrentExpirationDateUTC = _dateCurrent;
        //            parameters.InitialAmount = item.AppliedAmount.ToDecimal();
        //            parameters.TotalAmount = item.AppliedAmount.ToDecimal();
        //            parameters.DateLastTotalAmountUTC = DateTime.Now;
        //            parameters.ExpirationStatusID = Constants.ExpirationStatus.Unexpired.ToInt();
        //            parameters.NegotiationLevelID = Constants.NegotiationLevel.Original.ToInt();
        //            parameters.IsDeferred = true;
        //            parameters.ProcessOnDateUTC = _dateCurrent;
        //            parameters.ProcessedDateUTC = _dateCurrent;
        //            parameters.ModifiedByUserID = CoreContext.CurrentUser.UserID;
        //            parameters.DateCreatedUTC = _dateCurrent;
        //            parameters.DateLastModifiedUTC = _dateCurrent;
        //            parameters.PaymentTypeID = Constants.PaymentType.ProductCredit.ToInt();
        //            var _OrderPaymentID = OrderPaymentsBusinessLogic.Insert(parameters);

        //            int EntryTypeID = Constants.LedgerEntryTypes.EnrrollmentCredit.ToInt();
        //            int EntryReasonID = Constants.LedgerEntryReasons.ProductCredit.ToInt();
        //            int EntryOrigin = Constants.LedgerEntryOrigins.OrderEntry.ToInt();
        //            CTE.ApplyCredit(_AccountID, EntryReasonID, EntryOrigin, EntryTypeID, item.AppliedAmount.ToDecimal(), _orderID, _OrderPaymentID);
        //        }
        //    }
        //}

        // lo tuve que crear(aunque no deberia; se creo una copia de 'acctionResult ApplyPayment'); solo para obtener el estado porq eso deberia hacer como indica el requerimiento BR-PD-002
        public int _ApplyPayment(int paymentMethodId, decimal amount, string giftCardCode = null)
        {
            int return_StatusIDPayment = 0;
            try
            {
                BasicResponseItem<OrderPayment> response = new BasicResponseItem<OrderPayment>();
                IPayment payment = null;

                if (paymentMethodId < 0)
                {
                    var paymentType = (NetSteps.Data.Entities.Constants.PaymentType)Math.Abs(paymentMethodId);
                    switch (paymentType)
                    {
                        case NetSteps.Data.Entities.Constants.PaymentType.GiftCard:
                            payment = new NonAccountPaymentMethod()
                            {
                                DecryptedAccountNumber = giftCardCode ?? string.Empty,
                                PaymentTypeID = (int)paymentType
                            };
                            break;
                        case NetSteps.Data.Entities.Constants.PaymentType.ProductCredit:
                            payment = new NonAccountPaymentMethod()
                            {
                                PaymentTypeID = (int)paymentType
                            };
                            break;
                        default:
                            throw new Exception(Translation.GetTerm("InvalidPaymentType", "Invalid payment type"));
                    }
                }
                else
                {
                    payment = NetSteps.Data.Entities.Account.LoadPaymentMethodAndVerifyAccount(paymentMethodId, CurrentAccount.AccountID);
                }

                response = OrderContext.Order.AsOrder().ApplyPaymentToCustomer(payment.PaymentTypeID, amount, payment.NameOnCard, payment);

                return_StatusIDPayment = response.Item.OrderPaymentStatusID;
            }
            catch
            {
                return_StatusIDPayment = 0;
            }
            return return_StatusIDPayment;

        }

        //BR-CC-007 END 
        protected virtual void ValidateCrossBorderShipping()
        {
        }

        [FunctionFilter("Orders", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public virtual ActionResult SaveOrder(string invoiceNotes)
        {
            try
            {
                OrderContext.Order.AsOrder().StartEntityTracking();

                // Remove all the Negative OrderItemIDs used in UI - JHE
                foreach (var orderItem in OrderContext.Order.AsOrder().OrderCustomers[0].OrderItems.Where(oi => oi.OrderItemID < 0))
                {
                    orderItem.StopTracking();
                    orderItem.OrderItemID = 0;
                    orderItem.StartTracking();
                }

                foreach (var payment in OrderContext.Order.AsOrder().OrderCustomers[0].OrderPayments.Where(p => p.OrderPaymentID < 0))
                {
                    payment.StopTracking();
                    payment.OrderPaymentID = 0;
                    payment.StartTracking();
                }

                // Validate that a shipping address has been selected and contains enough information to ship the order
                if (OrderContext.Order.AsOrder().OrderShipments.Count < 1)
                {
                    throw new Exception(Translation.GetTerm("NoShippingAddress", "There is no shipping address.  Please choose an address to ship to."));
                }

                OrderShipment shipments = OrderContext.Order.AsOrder().OrderShipments[0]; //OrderContext.Order.AsOrder().GetDefaultShipment();

                var shippingMethods = OrderContext.Order.AsOrder().GetShippingMethods(shipments);
                if (shippingMethods != null)
                    shippingMethods = shippingMethods.OrderBy(sm => sm.ShippingAmount).ToList();
                if (shippingMethods.Count() == 0)
                {
                    return Json(new { result = false, message = Translation.GetTerm("NoShippingAddress", "There is no shipping address.  Please choose an address to ship to.") });
                }


                foreach (OrderShipment shipment in OrderContext.Order.AsOrder().OrderShipments)
                {
                    if (string.IsNullOrEmpty(shipment.Address1) ||
                            string.IsNullOrEmpty(shipment.City) ||
                            string.IsNullOrEmpty(shipment.State) ||
                            string.IsNullOrEmpty(shipment.PostalCode))
                    {
                        throw new Exception(Translation.GetTerm("InvalidShippingAddress", "The shipping address is invalid."));
                    }
                }

                if (!invoiceNotes.ToCleanString().IsNullOrEmpty())
                    OrderContext.Order.AsOrder().InvoiceNotes = invoiceNotes;
                try
                {
                    int shippingOrderType = OrderContext.Order.AsOrder().OrderShipments[0].ShippingMethodID.Value;
                    OrderContext.Order.AsOrder().OrderShipments[0].ShippingMethodID = Order.GetShippingMethodID(shippingOrderType);

                }
                catch (Exception)
                {
                    return Json(new { result = false, message = Translation.GetTerm("NoShippingAddress", "There is no shipping address.  Please choose an address to ship to.") });
                }

                OrderContext.Order.AsOrder().OrderPayments.Each(o => o.ExpirationStatusID = (int)ConstantsGenerated.ExpirationStatuses.Unexpired);
                OrderContext.Order.AsOrder().ParentOrderID = null;
                OrderContext.Order.AsOrder().Save();

                //Se recorre para actualizar los campos que faltan  

                var orderID = OrderContext.Order.AsOrder().OrderID;
                var postalCode = OrderContext.Order.AsOrder().OrderShipments[0].PostalCode;
                var warehouseID = Convert.ToInt32(Session["WareHouseId"]);
                var dateEstimated = Convert.ToString(Session["DateEstimated"]);

                PaymentsMethodsExtensions.ManagementKit(postalCode, warehouseID, orderID,
                    dateEstimated);


                insertDispatchProductsFill(); // wv: 20160603 inserta los Dispatch de productos filtrados para el account

                var orderNumber = OrderContext.Order.AsOrder().OrderNumber;

                string session = Session["Edit"].ToString();
                OrderContext.Clear();
                return Json(new { result = true, orderNumber = orderNumber });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, OrderContext.Order != null ? OrderContext.Order.OrderID.ToIntNullable() : null, accountID: CurrentAccount != null ? CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public virtual ActionResult ReloadInventory()
        {
            try
            {
                Inventory.ExpireCache();
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [FunctionFilter("Orders", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public virtual ActionResult BundlePackItems(int productId, string bundleGuid, string orderCustomerId = null)
        {
            var product = Inventory.GetProduct(productId);
            //var product = Product.LoadFull(productId);
            var dynamicKit = new DynamicKit();
            var dynamicKitGroups = new NetSteps.Data.Entities.TrackableCollection<NetSteps.Data.Entities.DynamicKitGroup>();
            if (product.DynamicKits.Count != 0)
            {
                dynamicKit = product.DynamicKits[0];
                dynamicKitGroups = product.DynamicKits[0].DynamicKitGroups;
            }
            var customer = ((OrderCustomer)OrderContext.Order.OrderCustomers[0]);
            ViewBag.DynamicKit = dynamicKit;
            ViewBag.DynamicKitGroups = dynamicKitGroups;
            ViewBag.MaxItemsInBundle = dynamicKitGroups.Sum(g => g.MinimumProductCount);
            ViewBag.ProductId = productId;
            ViewBag.BundleGuid = bundleGuid;
            ViewBag.OrderCustomerId = customer.Guid.ToString("N");
            ViewBag.OrderItem = customer.OrderItems.GetByGuid(bundleGuid);

            return View();
        }

        [NonAction]
        public virtual HtmlString GetGroupItemsHtml(string parentGuid, int groupId)
        {
            if (OrderContext.Order.AsOrder() == null)
                return new HtmlString("");

            var customer = OrderContext.Order.AsOrder().OrderCustomers.FirstOrDefault();
            StringBuilder builder = new StringBuilder();

            var orderItem = customer.OrderItems.GetByGuid(parentGuid);
            var product = Inventory.GetProduct(orderItem.ProductID.ToInt());
            var dynamicKit = product.DynamicKits[0];
            var groupItems = orderItem.ChildOrderItems.Where(index => index.DynamicKitGroupID == groupId);

            foreach (var item in groupItems)
            {
                Product childProduct = Inventory.GetProduct(item.ProductID.Value);
                for (int q = 0; q < item.Quantity; q++)
                {
                    TagBuilder span = new TagBuilder("span");
                    span.AddCssClass("block");

                    span.InnerHtml = new StringBuilder()
                            .Append("<input type=\"hidden\" value=\"" + item.Guid.ToString("N") + "\" class=\"orderItemGuid\" />")
                            .Append("<a href=\"javascript:void(0)\" class=\"UI-icon icon-x RemoveItem\"></a>&nbsp;" + childProduct.SKU + " " + childProduct.Translations.Name()).ToString();

                    builder.Append(span.ToString());
                }
            }

            var results = builder.ToString();

            return new HtmlString(string.IsNullOrEmpty(results) ? "" : results);
        }

        [FunctionFilter("Orders", "~/", NetSteps.Data.Entities.Constants.SiteType.Replicated)]
        public virtual ActionResult ApplyPromotionCode(string promotionCode)
        {
            try
            {
                int accountID = OrderContext.Order.OrderCustomers.First().AccountID;
                if (string.IsNullOrWhiteSpace(promotionCode))
                    return JsonError(_errorInvalidPromotionCode(promotionCode));

                if (!OrderService.GetActivePromotionCodes(accountID).Contains(promotionCode))
                {
                    return JsonError(Translation.GetTerm("Promotions_PromotionCodeNotFound", "Promotion Code Not Found or Already Used"));
                }

                if (OrderContext.CouponCodes.Any(existing => existing.AccountID == accountID && existing.CouponCode.Equals(promotionCode, StringComparison.InvariantCultureIgnoreCase)))
                {
                    return JsonError(Translation.GetTerm("Promotions_PromotionCodeAlreadyApplied", "Promotion Code Already Applied"));
                }

                var newCode = Create.New<ICouponCode>();
                newCode.CouponCode = promotionCode;
                newCode.AccountID = accountID;
                OrderContext.CouponCodes.Add(newCode);
                OrderService.UpdateOrder(OrderContext);

                return Json(new
                {
                    result = true,
                    totals = Totals,
                    OrderEntryModelData = GetOrderEntryModelData(OrderContext),
                    shippingMethods = ShippingMethods,
                    outOfStockItems = GetOutOfStockItemsString(OrderContext.Order.AsOrder()),
                });
            }
            catch (Exception ex)
            {
                var message = ex.Log(orderID: OrderContext.Order.OrderID, accountID: CurrentAccount.AccountID).PublicMessage;
                return JsonError(message);
            }
        }

        [FunctionFilter("Orders", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public virtual ActionResult RemovePromotionCode(string promotionCode)
        {
            if (string.IsNullOrWhiteSpace(promotionCode))
            {
                return JsonError(_errorInvalidPromotionCode(promotionCode));
            }

            try
            {
                var orderCustomer = OrderContext.Order.AsOrder().OrderCustomers.Single(customer => customer.AccountID == CurrentAccount.AccountID);
                var found = OrderContext.CouponCodes.SingleOrDefault(existing => existing.AccountID == orderCustomer.AccountID && existing.CouponCode.Equals(promotionCode, StringComparison.InvariantCultureIgnoreCase));
                if (found != null)
                {
                    OrderContext.CouponCodes.Remove(found);
                    orderCustomer.RemoveAllPayments();
                    OrderService.UpdateOrder(OrderContext);
                }

                return Json(new
                {
                    totals = Totals,
                    result = true,
                    OrderEntryModelData = GetOrderEntryModelData(OrderContext),
                    promotions = GetApplicablePromotions(OrderContext),
                    shippingMethods = ShippingMethods,
                    outOfStockItems = GetOutOfStockItemsString(OrderContext.Order.AsOrder()),
                });
            }
            catch (Exception ex)
            {
                var message = ex.Log(orderID: OrderContext.Order.OrderID, accountID: CurrentAccount.AccountID).PublicMessage;
                return JsonError(message);
            }
        }

        #region Helper methods that belong in a UI service.
        /// <summary>
        /// Creates the model for the initial page load.
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        protected virtual IIndexModel Index_CreateModel(Order order)
        {
            Contract.Requires<ArgumentNullException>(order != null);

            var model = Create.New<IIndexModel>();
            model.OrderEntryModel = Create.New<IOrderEntryModel>();
            LoadOrderEntryModelOptions(model.OrderEntryModel.Options);
            LoadOrderEntryModelData(model.OrderEntryModel.Data, OrderContext);
            model.OrderEntryModel.Order = order;

            return model;
        }

        /// <summary>
        /// Returns the data to send to the client-side viewmodel during AJAX calls.
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        protected virtual IDictionary<string, object> GetOrderEntryModelData(IOrderContext orderContext)
        {
            Contract.Requires<ArgumentNullException>(orderContext != null);

            return LoadOrderEntryModelData(new DynamicDictionary(), orderContext).AsDictionary();
        }

        /// <summary>
        /// Loads the options bag for the client-side viewmodel.
        /// </summary>
        protected virtual dynamic LoadOrderEntryModelOptions(dynamic options)
        {
            // Code contracts rewriter doesn't work with dynamics
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }

            options.RemoveUrl = Url.Action("RemoveFromCart");
            options.RemoveErrorMessage = Translation.GetTerm("ErrorRemovingProduct", "The product could not be removed");
            options.UpdateQuantitiesUrl = Url.Action("UpdateCart");
            options.UpdateQuantitiesErrorMessage = Translation.GetTerm("ErrorUpdatingCart", "The cart could not be updated");

            return options;
        }

        /// <summary>
        /// Returns subtotal, formatted for the client-side viewmodel.
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        protected virtual string GetSubtotal(Order order)
        {
            Contract.Requires<ArgumentNullException>(order != null);

            return order.Subtotal.ToString(order.CurrencyID);
        }

        /// <summary>
        /// Returns order items, formatted for the client-side viewmodel.
        /// </summary>
        protected virtual IList<IOrderItemModel> GetOrderItemModels(Order order)
        {
            Contract.Requires<ArgumentNullException>(order != null);
            Contract.Requires<ArgumentException>(order.OrderCustomers != null);
            Contract.Requires<ArgumentException>(order.OrderCustomers.Count > 0);

            List<getDispatchByOrder> listDispatch = OrderExtensions.GetDispatchByOrder(order.OrderID); //(List<getDispatchByOrder>)Session["loadDispatchProcessOrder"];
            IList<IOrderItemModel> listTemporal = new List<IOrderItemModel>();
            IList<IOrderItemModel> listRetorno = new List<IOrderItemModel>();

            listTemporal = order.OrderCustomers[0].ParentOrderItems
                .Where(x => !x.OrderAdjustmentOrderLineModifications.Any(y => y.ModificationOperationID == (int)OrderAdjustmentOrderLineOperationKind.AddedItem))
                .Select(GetOrderItemModel)
                .ToList();

            foreach (var items in listTemporal)
            {
                if (!(listDispatch.Exists(x => x.OrderItemID == items.OrderItemID)))
                {
                    listRetorno.Add(items);
                }
            }

            return listRetorno;

            //return order.OrderCustomers[0].ParentOrderItems
            //    .Where(x => !x.OrderAdjustmentOrderLineModifications.Any(y => y.ModificationOperationID == (int)OrderAdjustmentOrderLineOperationKind.AddedItem))
            //    .Select(GetOrderItemModel)
            //    .ToList();
        }

        /// <summary>
        /// Returns an order item, formatted for the client-side viewmodel.
        /// </summary>
        protected virtual IOrderItemModel GetOrderItemModel(OrderItem orderItem)
        {
            Contract.Requires<ArgumentNullException>(orderItem != null);
            Contract.Requires<ArgumentNullException>(orderItem.OrderCustomer != null);
            Contract.Requires<ArgumentNullException>(orderItem.OrderCustomer.Order != null);

            var orderCustomer = orderItem.OrderCustomer;
            var order = orderCustomer.Order;
            var currencyId = order.CurrencyID;
            var orderItemModel = Create.New<IOrderItemModel>();

            var orderItemProduct = Inventory.GetProduct(orderItem.ProductID.Value);
            var preAdjustmentUnitPrice = orderItem.GetPreAdjustmentPrice();
            var finalUnitPrice = orderItem.ItemPriceActual ?? orderItem.GetAdjustedPrice();

            orderItemModel.Guid = orderItem.Guid.ToString("N");
            orderItemModel.ProductID = orderItem.ProductID ?? 0;
            orderItemModel.SKU = orderItem.SKU;
            orderItemModel.ProductName = orderItemProduct.Translations.Name();
            orderItemModel.retailPricePerItem = NetSteps.Data.Entities.ProductPricesExtensions.GetPriceByPriceType(orderItem.ProductID.Value, 1, currencyId).GetRoundedNumber(2).ToString(currencyId);
            orderItemModel.AdjustedUnitPrice = finalUnitPrice.ToString(currencyId);
            orderItemModel.OriginalUnitPrice = preAdjustmentUnitPrice.ToString(currencyId);

            orderItemModel.AdjustedTotal = (finalUnitPrice * orderItem.Quantity).ToString(currencyId);
            orderItemModel.OriginalTotal = (preAdjustmentUnitPrice * orderItem.Quantity).ToString(currencyId);

            orderItemModel.Quantity = orderItem.Quantity;
            orderItemModel.OrderItemID = orderItem.OrderItemID;

            orderItemModel.OriginalCommissionableTotal = (orderItem.GetPreAdjustmentPrice(orderCustomer.CommissionablePriceTypeID) * orderItem.Quantity).ToString(currencyId);
            orderItemModel.AdjustedCommissionableTotal = (orderItem.GetAdjustedPrice(orderCustomer.CommissionablePriceTypeID) * orderItem.Quantity).ToString(currencyId);

            foreach (var message in orderItem.OrderItemMessages
                .Select(m => m.Message))
            {
                orderItemModel.Messages.Add(message);
            }

            // Hostess rewards show the discount amount next total.
            if (orderItem.IsHostReward)
            {
                // when hostess rewards are an order adjustment type we can refactor this.
                orderItemModel.AdjustedTotal = string.Format("{0} {1}",
                    orderItemModel.AdjustedTotal,
                    Translation.GetTerm("HostRewardDiscount", "(discounted {0})",
                        (orderItem.Discount ?? (orderItem.DiscountPercent.HasValue ? (orderItem.ItemPrice * orderItem.Quantity) * orderItem.DiscountPercent.Value : 0)).ToString(currencyId)
                    )
                );
            }

            orderItemModel.IsStaticKit = orderItemProduct.IsStaticKit();
            orderItemModel.IsDynamicKit = orderItemProduct.IsDynamicKit();
            if (orderItemModel.IsDynamicKit)
            {
                orderItemModel.IsDynamicKitFull = orderItem.ChildOrderItems.Sum(oi => oi.Quantity) >= orderItemProduct.DynamicKits[0].DynamicKitGroups.Sum(g => g.MinimumProductCount);
            }
            orderItemModel.IsHostReward = orderItem.IsHostReward;
            orderItemModel.BundlePackItemsUrl = Url.Action("BundlePackItems", new { productId = orderItem.ProductID, bundleGuid = orderItem.Guid.ToString("N"), orderCustomerId = orderItem.OrderCustomer.Guid.ToString("N") });

            orderItemModel.KitItemsModel = Create.New<IKitItemsModel>();
            if (orderItemProduct.IsStaticKit())
            {
                //orderItemModel.KitItemsModel.KitItemModels obj = new  
                List<MaterialName> objList = new List<MaterialName>();
                foreach (var objE in Order.GetMaterialWithMaterialID(Convert.ToInt32(Session["WareHouseId"]), Convert.ToInt32(Session["PreOrder"])).Where(x => x.ProductId == orderItem.ProductID))
                {
                    objList.Add(objE);
                }
                //foreach (var item in Order.productRelations.Where(x => x.ProductID == orderItem.ProductID).ToList())
                //{
                //    foreach (var objM in Order.GetMaterialWithMaterialID(item.MaterialId))
                //    {
                //        MaterialName objE = new MaterialName();
                //        objE.Name = objM.Name;
                //        objE.Quantity = item.Quantity;
                //        objE.SKU = objM.SKU;
                //        objList.Add(objE);
                //    }
                //} 
                //Is Kit
                orderItemModel.KitItemsModel.KitItemModels = objList
                    .Select(k =>
                    {
                        var kitItemModel = Create.New<IKitItemModel>();

                        kitItemModel.ProductName = k.Name;
                        kitItemModel.Quantity = k.Quantity;
                        kitItemModel.SKU = k.SKU;
                        return kitItemModel;
                    }).ToList();

            }
            //if (orderItemProduct.IsStaticKit() || orderItemProduct.IsDynamicKit())
            //{
            //    orderItemModel.KitItemsModel.KitItemModels = orderItem.ChildOrderItems
            //        .Select(k =>
            //        {
            //            var kitItemModel = Create.New<IKitItemModel>();
            //            var kitItemProduct = Inventory.GetProduct(k.ProductID.Value);
            //            kitItemModel.ProductName = kitItemProduct.Translations.Name();
            //            kitItemModel.Quantity = k.Quantity;
            //            kitItemModel.SKU = kitItemProduct.SKU;
            //            return kitItemModel;
            //        })
            //        .ToList();
            //}
            else
            {
                // Non-kits still need an empty list.
                orderItemModel.KitItemsModel.KitItemModels = new List<IKitItemModel>();
            }

            //CGI(CMR)-29/10/2014-Inicio
            decimal totalQV = 0;
            totalQV = orderItem.OrderItemPrices
                                    .Where(ip => ip.ProductPriceTypeID == (int)NetSteps.Data.Entities.Constants.ProductPriceType.QV)
                                    .Sum(ip => ((ip.UnitPrice == null ? 0 : ip.UnitPrice) * (orderItem.Quantity == null ? 0 : (decimal)orderItem.Quantity)));

            orderItemModel.TotalQV = totalQV;
            //orderItemModel.TotalQV_Currency = totalQV.ToString(currencyId);
            orderItemModel.TotalQV_Currency = Convert.ToString(totalQV.GetRoundedNumber(2)); //EL QV NO DEBE TENER SIGNO $
            //CGI(CMR)-29/10/2014-Inicio

            return orderItemModel;
        }

        protected virtual IEnumerable<object> GetFreeGiftModels(Order order)
        {
            OrderCustomer orderCustomer = OrderContext.Order.AsOrder().OrderCustomers[0];
            int addedItemOperationID = (int)OrderAdjustmentOrderLineOperationKind.AddedItem;
            var nonPromotionalItems = orderCustomer.ParentOrderItems.Where(x => !x.OrderAdjustmentOrderLineModifications.Any(y => y.ModificationOperationID == addedItemOperationID));
            var promotionalItems = orderCustomer.ParentOrderItems.Except(nonPromotionalItems).ToList();
            var adjustments = promotionalItems.GroupBy(x => x.OrderAdjustmentOrderLineModifications.Single(y => y.ModificationOperationID == addedItemOperationID).OrderAdjustment);

            return adjustments.Select(grp => new
            {
                Description = grp.Key.Description,
                StepID = grp.Key.InjectedOrderSteps.Any() ? grp.Key.InjectedOrderSteps.First().OrderStepReferenceID : null,
                Selections = grp.Select(i => new { SKU = i.SKU, Name = i.ProductName, Quantity = i.Quantity })
            });
        }

        /// <summary>
        /// Loads the data bag for the client-side viewmodel.
        /// For consistency, this method should be used for
        /// both the initial page load and for AJAX calls.
        /// </summary>
        /// 


        public decimal FormatDecimalByCulture(string valor)
        {
            bool correcto = false;
            decimal numero = 0;

            var formatos = new System.Collections.Generic.List<CultureInfo>
            {
                new System.Globalization.CultureInfo("pt-BR"),
                new System.Globalization.CultureInfo("en-US"),
                new System.Globalization.CultureInfo("es-US")

            };

            if (!formatos.Any(x => x.Name == CoreContext.CurrentCultureInfo.Name))
                formatos.Add(CoreContext.CurrentCultureInfo);



            if (string.IsNullOrEmpty(valor))
                return numero;

            var style = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands;

            foreach (var item in formatos)
            {

                if (decimal.TryParse(valor, style, item, out numero) == true)
                {
                    correcto = true;
                    break;
                }

            }
            if (correcto)
            {

                return numero;

            }
            else
                return decimal.Zero;
        }
        protected virtual dynamic LoadOrderEntryModelData(dynamic data, IOrderContext orderContext)
        {
            Contract.Requires<ArgumentNullException>(orderContext != null);
            // Code contracts rewriter doesn't work with dynamics
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            data.Subtotal = orderContext.Order.AsOrder().OrderCustomers[0].Subtotal.ToString(orderContext.Order.CurrencyID);
            data.SubtotalAdjusted = orderContext.Order.OrderCustomers[0].AdjustedSubTotal.ToString(orderContext.Order.CurrencyID);
            //CGI(CMR)-29/10/2014-Inicio
            //data.OrderItemModels = GetOrderItemModels(orderContext.Order.AsOrder()); //comentado
            IList<IOrderItemModel> orderItemModels = GetOrderItemModels(orderContext.Order.AsOrder());
            data.OrderItemModels = orderItemModels;
            //data.TotalQV_Sum = orderItemModels.Sum(x => (x.TotalQV != null ? x.TotalQV : 0)).ToString(orderContext.Order.CurrencyID); //EL QV NO DEBE TENER SIGNO $
            data.TotalQV_Sum = orderItemModels.Sum(x => (x.TotalQV != null ? x.TotalQV : 0)).GetRoundedNumber(2);
            //CGI(CMR)-29/10/2014-Fin

            //CGI(CMR)-07/04/2015-Inicio
            //CV
           

            // inicio 09052017   comentado por hundred  para generalizar el formato númerico.
            //string cultureInfo = Currency.Load((int)ConstantsGenerated.Currency.BrazilianReal).CultureInfo.ToString();

            //data.OriginalCommissionableTotal_Sum = orderItemModels.Sum(x => (x.OriginalCommissionableTotal.Length > 0 ? decimal.Parse(x.OriginalCommissionableTotal, NumberStyles.Currency, CultureInfo.GetCultureInfo(cultureInfo)) : 0)).ToString(orderContext.Order.CurrencyID);
            //data.AdjustedCommissionableTotal_Sum = orderItemModels.Sum(x => (x.AdjustedCommissionableTotal.Length > 0 ? decimal.Parse(x.AdjustedCommissionableTotal, NumberStyles.Currency, CultureInfo.GetCultureInfo(cultureInfo)) : 0)).ToString(orderContext.Order.CurrencyID);
            // fin  09052017   
            // inicio 09052017   comentado por hundred  para generalizar el formato númerico.
            string cultureInfo = CoreContext.CurrentCultureInfo.Name;


            data.OriginalCommissionableTotal_Sum = orderItemModels.Sum(x => (x.OriginalCommissionableTotal.Length > 0 ? FormatDecimalByCulture(x.OriginalCommissionableTotal) : 0)).ToString("C", CoreContext.CurrentCultureInfo);
            data.AdjustedCommissionableTotal_Sum = orderItemModels.Sum(x => (x.AdjustedCommissionableTotal.Length > 0 ? FormatDecimalByCulture(x.AdjustedCommissionableTotal) : 0)).ToString("C", CoreContext.CurrentCultureInfo);

            //data.OriginalCommissionableTotal_Sum = orderItemModels.Sum(x => (x.OriginalCommissionableTotal.Length > 0 ? decimal.Parse(x.OriginalCommissionableTotal, NumberStyles.Currency, CultureInfo.GetCultureInfo(cultureInfo)) : 0)).ToString("C",CoreContext.CurrentCultureInfo);
            //data.AdjustedCommissionableTotal_Sum = orderItemModels.Sum(x => (x.AdjustedCommissionableTotal.Length > 0 ? decimal.Parse(x.AdjustedCommissionableTotal, NumberStyles.Currency, CultureInfo.GetCultureInfo(cultureInfo)) : 0)).ToString("C",CoreContext.CurrentCultureInfo);

            // fin  09052017 




           

            //Qty
            data.CountItems = orderItemModels.Sum(x => x.Quantity);
            //SubTotal

            //data.OriginalTotal_Sum = orderItemModels.Sum(x => (x.OriginalTotal.Length > 0 ? decimal.Parse(x.OriginalTotal, NumberStyles.Currency, CultureInfo.GetCultureInfo(cultureInfo)) : 0)).ToString(orderContext.Order.CurrencyID);
            //data.AdjustedTotal_Sum = orderItemModels.Sum(x => (x.AdjustedTotal.Length > 0 ? decimal.Parse(x.AdjustedTotal, NumberStyles.Currency, CultureInfo.GetCultureInfo(cultureInfo)) : 0)).ToString(orderContext.Order.CurrencyID);


            //data.OriginalTotal_Sum = orderItemModels.Sum(x => (x.OriginalTotal.Length > 0 ? decimal.Parse(x.OriginalTotal, NumberStyles.Currency, CultureInfo.GetCultureInfo(cultureInfo)) : 0)).ToString(CoreContext.CurrentCultureInfo);
            //data.AdjustedTotal_Sum = orderItemModels.Sum(x => (x.AdjustedTotal.Length > 0 ? decimal.Parse(x.AdjustedTotal, NumberStyles.Currency, CultureInfo.GetCultureInfo(cultureInfo)) : 0)).ToString(CoreContext.CurrentCultureInfo);



            data.OriginalTotal_Sum = orderItemModels.Sum(x => (x.OriginalTotal.Length > 0 ? FormatDecimalByCulture(x.OriginalTotal) : 0)).ToString(CoreContext.CurrentCultureInfo);
            data.AdjustedTotal_Sum = orderItemModels.Sum(x => (x.AdjustedTotal.Length > 0 ? FormatDecimalByCulture(x.AdjustedTotal) : 0)).ToString(CoreContext.CurrentCultureInfo);



            //***************************** start only test *********************************
            //foreach (var item in orderItemModels)
            //{
            //    string OriginalTotal = item.OriginalTotal;
            //    string AdjustedTotal = item.AdjustedTotal;

            //    decimal OriginalTotal_dec = Convert.ToDecimal(item.OriginalTotal.Replace("$", "")).FormatGlobalizationDecimal();
            //    decimal AdjustedTotal_dec = Convert.ToDecimal(item.AdjustedTotal.Replace("$", "")).FormatGlobalizationDecimal();

            //    decimal OriginalTotal_dec1 = Convert.ToDecimal(item.OriginalTotal.Replace("$", ""));
            //    decimal AdjustedTotal_dec1 = Convert.ToDecimal(item.AdjustedTotal.Replace("$", ""));

            //    decimal OriginalTotal_dec2 = item.OriginalTotal.Replace("$", "").FormatGlobalizationDecimal();
            //    decimal AdjustedTotal_dec2 = item.AdjustedTotal.Replace("$", "").FormatGlobalizationDecimal();

            //    decimal a = OriginalTotal_dec2.GetRoundedNumber(2);
            //}

            //***************************** end only test *********************************

            data.OriginalTotal_Texto = data.OriginalTotal_Sum + "(" + Translation.GetTerm("Subtotal", "Sub total") + ")";
            data.AdjustedTotal_Texto = data.AdjustedTotal_Sum + "(" + Translation.GetTerm("Subtotaldco", "Sub total dco") + ")";

            //CGI(CMR)-07/04/2015-Fin
            data.ApplicablePromotions = GetApplicablePromotions(orderContext);
            data.FreeGiftModels = GetFreeGiftModels(orderContext.Order.AsOrder());

            return data;
        }

        /// <summary>
        /// Returns a sequence of promotion models to display in the sidebar,
        /// or returns null to hide the sidebar.
        /// </summary>
        protected virtual IEnumerable<IDisplayInfo> GetAvailablePromotionsList()
        {
            return null;
        }
        #endregion

        protected virtual string _errorInvalidPromotionCode(string promotionCode) { return Translation.GetTerm("ErrorInvalidPromotionCode", "The promotion could not be applied. Invalid promotion code: '{0}'.", promotionCode); }
        protected virtual string _errorNoItemsInOrder { get { return Translation.GetTerm("PleaseAddItemsToOrderBeforeUpdating", "Please add items to Order before updating."); } }

        public ActionResult RedirectPage()
        {
            if (OrderContext.Order.OrderStatusID == 1)
            {
                OrderContext.Order.OrderStatusID = 5;
                OrderContext.Order.Save();
            }
            return RedirectToAction("NewOrder", "OrderEntry");
        }

        #region Envio de Boleto



        public virtual ActionResult ExportarBoleta()
        {
            try
            {
                int OrderPaymentID = 0; int BankCode = 0;
                string BankName = string.Empty;
                Byte[] ResponseFile = null;
                if (Request.QueryString["OrderPaymentID"] != null)
                {
                    OrderPaymentID = Convert.ToInt32(Request.QueryString["OrderPaymentID"]);
                }
                if (Request.QueryString["BankName"] != null)
                {
                    BankName = Request.QueryString["BankName"];
                }
                if (Request.QueryString["BankCode"] != null)
                {
                    BankCode = Convert.ToInt32(Request.QueryString["BankCode"]);
                }
                switch (BankCode)
                {
                    case 1:// "Banco Do Brasil":
                        ResponseFile = CrearTicketBB(OrderPaymentID);
                        break;
                    case 104://"Caixa":
                        ResponseFile = CrearTicketCaixa(OrderPaymentID);
                        break;
                    case 341://"Itaú":
                        ResponseFile = CrearTicketItau(OrderPaymentID);
                        break;
                    default:
                        break;
                }
                byte[] Libro = ExtractPages(ResponseFile);
                string nameFile = string.Format("Ticket{0}-{1}{2}{3}.pdf", OrderPaymentID.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString(), DateTime.Now.Year.ToString());
                byte[] output = Libro;
                var ruta = ConfigurationManager.AppSettings["FileUploadWebPath"];

                var path = ruta + nameFile; //@"\\10.12.6.183\FileUploads\ReportsPDF\" + nameFile;//Server.MapPath("~/Reports/FilesTemp/" + nameFile);  //   se coloca  o exporta el archivo
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                System.IO.File.WriteAllBytes(path, output);
                File(path, "application/pdf", Path.GetFileName(path));


                AccountPropertiesBusinessLogic.GetEmailTemplate(OrderPaymentID, nameFile);

                return File(Libro, "application/pdf", string.Format("Ticket{0}-{1}{2}{3}.pdf", OrderPaymentID.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString(), DateTime.Now.Year.ToString()));
            }
            catch (Exception ex)
            {
                //throw ex;
                string msg = Translation.GetTerm("PDFNotSentForData", "You can not export values by format");
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = msg });// exception.PublicMessage });
            }
        }
        #region crear codigo de barra
        #region TicketBB
        private string CreateCodeBarTicketBB(DataTable dtOrder, DataTable dtInfoBank)
        {
            string Code = "";
            try
            {
                Code = dtInfoBank.Rows[0]["BankCode"].ToString();//(Fields!BankCode.Value, "dtsInfoBank") & 
                Code += dtInfoBank.Rows[0]["CurrencieBankID"].ToString(); // First(Fields!CurrencyCode.Value, "dtsInfoBank") &  
                Code += CodeBarFormulaTicketBB.DVTCalculate
                        (
                            dtInfoBank.Rows[0]["BankCode"].ToString(),//First(Fields!BankCode.Value, "dtsInfoBank"),
                            dtInfoBank.Rows[0]["CurrencieBankID"].ToString(),// First(Fields!CurrencyCode.Value, "dtsInfoBank"),/* 
                            dtOrder.Rows[0]["FactorVencimientoCalculate"].ToString(), // cstr(First(Fields!FactorVencimientoCalculate.Value, "dtsOrder")) ,
                            dtOrder.Rows[0]["AmountTotal"].ToString(),  //First(Fields!AmountTotal.Value, "dtsOrder"),
                            "000000",
                            dtInfoBank.Rows[0]["CodigoConvenio"].ToString(),//  First(Fields!CodigoConvenio.Value, "dtsInfoBank"),
                            dtInfoBank.Rows[0]["NumeroTitulo"].ToString(),//  First(Fields!NumeroTitulo.Value, "dtsInfoBank"),
                            dtInfoBank.Rows[0]["Cartera"].ToString()// First(Fields!Cartera.Value, "dtsInfoBank")
                       );

                Code += dtOrder.Rows[0]["FactorVencimientoCalculate"].ToString();// CSTR(FIRST(Fields!FactorVencimientoCalculate.Value, "dtsOrder")) 
                Code += dtOrder.Rows[0]["AmountTotal"].ToString();// First(Fields!AmountTotal.Value, "dtsOrder") 
                Code += "000000";
                Code += dtInfoBank.Rows[0]["CodigoConvenio"].ToString();// First(Fields!CodigoConvenio.Value, "dtsInfoBank")
                Code += dtInfoBank.Rows[0]["NumeroTitulo"].ToString();// First(Fields!NumeroTitulo.Value, "dtsInfoBank");
                Code += dtInfoBank.Rows[0]["Cartera"].ToString(); //First(Fields!Cartera.Value, "dtsInfoBank") 
                return Code;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion
        #region TicketItau
        private string CreateCodebarTicketItau(DataTable dtOrder, DataTable dtInfoBank)
        {

            string Code = string.Empty;

            try
            {
                Code = dtInfoBank.Rows[0]["BankCode"].ToString();//(Fields!BankCode.Value, "dtsInfoBank") & 
                Code += dtInfoBank.Rows[0]["CurrencyCode"].ToString();//(Fields!BankCode.Value, "dtsInfoBank") & 

                Code += CodeBarFormulaTicketItau.DVTCalculate
                    (
                        dtInfoBank.Rows[0]["BankCode"].ToString(),
                        dtInfoBank.Rows[0]["CurrencyCode"].ToString(),
                        "",
                        dtOrder.Rows[0]["FactorVencimientoCalculate"].ToString(),
                        dtOrder.Rows[0]["AmountTotal"].ToString(),
                            (
                                dtInfoBank.Rows[0]["Cartera"].ToString() +//First(Fields!Cartera.Value, "dtsInfoBank")
                                dtInfoBank.Rows[0]["NumeroTitulo"].ToString() +//  First(Fields!NumeroTitulo.Value, "dtsInfoBank")

                                CodeBarFormulaTicketItau.DVNNCalculate
                                (
                                    dtInfoBank.Rows[0]["BankAgence"].ToString(),// First(Fields!Cartera.Value, "dtsInfoBank"),
                                    dtInfoBank.Rows[0]["Cuenta"].ToString(),// cstr(First(Fields!Cuenta.Value, "dtsInfoBank")),
                                    dtInfoBank.Rows[0]["Cartera"].ToString(),// First(Fields!Cartera.Value, "dtsInfoBank"),
                                    dtInfoBank.Rows[0]["NumeroTitulo"].ToString()// First(Fields!NumeroTitulo.Value, "dtsInfoBank")
                                )
                        ),
                          dtInfoBank.Rows[0]["BankAgence"].ToString(),
                          dtInfoBank.Rows[0]["Cuenta"].ToString(),
                          "000");
                Code += dtOrder.Rows[0]["FactorVencimientoCalculate"].ToString();
                Code += dtOrder.Rows[0]["AmountTotal"].ToString();

                Code += dtInfoBank.Rows[0]["Cartera"].ToString();//First(Fields!Cartera.Value, "dtsInfoBank")
                Code += dtInfoBank.Rows[0]["NumeroTitulo"].ToString();//  First(Fields!NumeroTitulo.Value, "dtsInfoBank")

                Code += CodeBarFormulaTicketItau.DVNNCalculate
                                  (
                                      dtInfoBank.Rows[0]["BankAgence"].ToString(),// First(Fields!Cartera.Value, "dtsInfoBank"),
                                      dtInfoBank.Rows[0]["Cuenta"].ToString(),// cstr(First(Fields!Cuenta.Value, "dtsInfoBank")),
                                      dtInfoBank.Rows[0]["Cartera"].ToString(),// First(Fields!Cartera.Value, "dtsInfoBank"),
                                      dtInfoBank.Rows[0]["NumeroTitulo"].ToString()// First(Fields!NumeroTitulo.Value, "dtsInfoBank")
                                  );
                Code += dtInfoBank.Rows[0]["BankAgence"].ToString().Substring(0, 4);
                Code += dtInfoBank.Rows[0]["Cuenta"].ToString();
                Code += "000";
                //

                return Code;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion
        #region Caixa
        private static string DVT3Calculate(DataTable dtOrder, DataTable dtInfoBank)
        {

            string code = "";
            code += CodeBarFormulaTicketCaixa.DVT3Calculate
                   (
                            dtInfoBank.Rows[0]["BankCode"].ToString(),//A
                            dtInfoBank.Rows[0]["CurrencyCode"].ToString(),//B
                            "",//C
                            dtOrder.Rows[0]["FactorVencimientoCalculate"].ToString(),//D
                            dtOrder.Rows[0]["AmountTotal"].ToString(),//E
                            dtInfoBank.Rows[0]["Cuenta"].ToString(),//F
                            dtInfoBank.Rows[0]["Cuenta"].ToString(),//G
                            ("000000000000000" + "80000" + dtInfoBank.Rows[0]["Cuenta"].ToString()).Substring(0, 3),//H
                            "1",//I
                            ("000000000000000" + "80000" + dtInfoBank.Rows[0]["Cuenta"].ToString()).Substring(3, 3),//J
                            "4",//K
                            ("000000000000000" + "80000" + dtInfoBank.Rows[0]["Cuenta"].ToString()).Substring(6, 9),//M

                          CodeBarFormulaTicketCaixa.DVCLCalculate(//L
                               ("000000" + dtInfoBank.Rows[0]["Cuenta"].ToString().Substring(0, dtInfoBank.Rows[0]["Cuenta"].ToString().Length - 1)).Substring(0, 6),
                               ("000000" + dtInfoBank.Rows[0]["Cuenta"].ToString().Substring(dtInfoBank.Rows[0]["Cuenta"].ToString().Length - 1, 1)).Substring(0, 6),
                               ("000000000000000" + "80000" + dtInfoBank.Rows[0]["Cuenta"].ToString()).Substring(0, 3),
                               "1",
                               ("000000000000000" + "80000" + dtInfoBank.Rows[0]["Cuenta"].ToString()).Substring(3, 3),
                               "4",
                                ("000000000000000" + "80000" + dtInfoBank.Rows[0]["Cuenta"].ToString()).Substring(6, 9)
                          )
                          );
            return code;
        }
        private static string CreateCodebarCaixa(DataTable dtOrder, DataTable dtInfoBank)
        {
            string Code = "";
            try
            {

                Code += dtInfoBank.Rows[0]["BankCode"].ToString().Substring(0, 3);
                Code += dtInfoBank.Rows[0]["CurrencyCode"].ToString().Substring(0, 1);
                Code += CodeBarFormulaTicketCaixa.DVT3Calculate
                    (
                             dtInfoBank.Rows[0]["BankCode"].ToString(),//A
                             dtInfoBank.Rows[0]["CurrencyCode"].ToString(),//B
                             "",//C
                             dtOrder.Rows[0]["FactorVencimientoCalculate"].ToString(),//D
                             dtOrder.Rows[0]["AmountTotal"].ToString(),//E

                           ("0" + dtInfoBank.Rows[0]["Cuenta"].ToString().Substring(0, 5)),//F
                             dtInfoBank.Rows[0]["Cuenta"].ToString().Substring(5, 1),//G


                             ("80000" + dtInfoBank.Rows[0]["NumeroTitulo"].ToString()).Substring(0, 3),//H
                             "1",//I
                             ("80000" + dtInfoBank.Rows[0]["NumeroTitulo"].ToString()).Substring(3, 3),//J
                             "4",//K
                             ("80000" + dtInfoBank.Rows[0]["NumeroTitulo"].ToString()).Substring(6, 9),//M

                           CodeBarFormulaTicketCaixa.DVCLCalculate(//L
                    //("000000" + dtInfoBank.Rows[0]["Cuenta"].ToString().Substring(0, dtInfoBank.Rows[0]["Cuenta"].ToString().Length - 1)).Substring(0, 6),
                    //("000000" + dtInfoBank.Rows[0]["Cuenta"].ToString().Substring(dtInfoBank.Rows[0]["Cuenta"].ToString().Length - 1, 1)).Substring(0, 6),
                                 ("0" + dtInfoBank.Rows[0]["Cuenta"].ToString().Substring(0, 5)),
                                dtInfoBank.Rows[0]["Cuenta"].ToString().Substring(5, 1),
                                ("80000" + dtInfoBank.Rows[0]["NumeroTitulo"].ToString()).Substring(0, 3),
                                "1",
                                ("80000" + dtInfoBank.Rows[0]["NumeroTitulo"].ToString()).Substring(3, 3),
                                "4",
                                ("80000" + dtInfoBank.Rows[0]["NumeroTitulo"].ToString()).Substring(6, 9)
                           )
                           );



                Code += dtOrder.Rows[0]["FactorVencimientoCalculate"].ToString();
                Code += dtOrder.Rows[0]["AmountTotal"].ToString();
                //Code += ("000000" + dtInfoBank.Rows[0]["Cuenta"].ToString().Substring(0, dtInfoBank.Rows[0]["Cuenta"].ToString().Length - 1)).Substring(0, 6);
                //Code += dtInfoBank.Rows[0]["Cuenta"].ToString().Substring(4, 1);
                Code += ("0" + dtInfoBank.Rows[0]["Cuenta"].ToString().Substring(0, 5));
                Code += dtInfoBank.Rows[0]["Cuenta"].ToString().Substring(5, 1);
                Code += ("80000" + dtInfoBank.Rows[0]["NumeroTitulo"].ToString()).Substring(0, 3);
                Code += "1";
                Code += ("80000" + dtInfoBank.Rows[0]["NumeroTitulo"].ToString()).Substring(3, 3);
                Code += "4";
                Code += ("80000" + dtInfoBank.Rows[0]["NumeroTitulo"].ToString()).Substring(6, 9);
                Code += CodeBarFormulaTicketCaixa.DVCLCalculate(//L
                    //("000000" + dtInfoBank.Rows[0]["Cuenta"].ToString().Substring(0, dtInfoBank.Rows[0]["Cuenta"].ToString().Length - 1)).Substring(0, 6),
                    //("000000" + dtInfoBank.Rows[0]["Cuenta"].ToString().Substring(dtInfoBank.Rows[0]["Cuenta"].ToString().Length - 1, 1)).Substring(0, 6),
                                ("0" + dtInfoBank.Rows[0]["Cuenta"].ToString().Substring(0, 5)),
                                dtInfoBank.Rows[0]["Cuenta"].ToString().Substring(5, 1),
                    //"000152", "4","800","1","000","4","000033282"
                                ("80000" + dtInfoBank.Rows[0]["NumeroTitulo"].ToString()).Substring(0, 3),
                                "1",
                                ("80000" + dtInfoBank.Rows[0]["NumeroTitulo"].ToString()).Substring(3, 3),
                                "4",
                                ("80000" + dtInfoBank.Rows[0]["NumeroTitulo"].ToString()).Substring(6, 9)
                           );


                return Code;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion
        private static Byte[] CodeBar(string text)
        {
            Barcode128 code128 = new Barcode128();
            code128.CodeType = Barcode.CODE128;
            code128.ChecksumText = true;
            code128.GenerateChecksum = true;
            code128.StartStopText = true;
            code128.Code = text;
            var bm = new System.Drawing.Bitmap(code128.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White));
            Byte[] bitmapData = null;
            using (var ms = new System.IO.MemoryStream())
            {
                bm.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                bitmapData = ms.ToArray();
            }
            return bitmapData;
        }

        #endregion

        private byte[] CrearTicketBB(int TicketNumber)
        {
            try
            {
                DataSet dsData = new DataSet();
                dsData = PaymetTycketsReportBusinessLogic.GenerateTicketBB(TicketNumber);

                string extension;
                string encoding;
                string mimeType;
                string[] streams;
                Warning[] warnings;
                LocalReport report = new LocalReport();
                report.ReportPath = Server.MapPath("~/Reports/GenerateTicketBB.rdlc");

                ReportDataSource rdsInfoBank = new ReportDataSource();
                rdsInfoBank.Name = "dtsInfoBank";//This refers to the dataset name in the RDLC file  
                rdsInfoBank.Value = dsData.Tables[0];


                ReportDataSource rdsdtOrder = new ReportDataSource();
                rdsdtOrder.Name = "dtsOrder";//This refers to the dataset name in the RDLC file  
                rdsdtOrder.Value = dsData.Tables[1];

                ReportDataSource rdsdtDetails = new ReportDataSource();
                rdsdtDetails.Name = "dtsDetailsBank";
                rdsdtDetails.Value = dsData.Tables[2];

                Byte[] bitmapData = CodeBar(CreateCodeBarTicketBB(dsData.Tables[1], dsData.Tables[0]));

                DataTable dtCodeBar = new DataTable("dtImage");
                DataColumn dc = new DataColumn("CodeBar");
                dc.DataType = typeof(byte[]);
                dtCodeBar.Columns.Add(dc);
                dtCodeBar.Rows.Add(new Object[] { bitmapData });

                ReportDataSource rdsdstImage = new ReportDataSource();
                rdsdstImage.Name = "dstImage";
                rdsdstImage.Value = dtCodeBar;

                report.DataSources.Add(rdsInfoBank);
                report.DataSources.Add(rdsdtOrder);
                report.DataSources.Add(rdsdtDetails);
                report.DataSources.Add(rdsdstImage);

                Byte[] mybytes = report.Render("pdf", null,
                                    out extension, out encoding,
                                    out mimeType, out streams, out warnings); //for exporting to PDF  
                return mybytes;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private byte[] CrearTicketCaixa(int TicketNumber)
        {
            try
            {
                DataSet dsData = new DataSet();
                dsData = PaymetTycketsReportBusinessLogic.GenerateTicketCaixa(TicketNumber);

                string extension;
                string encoding;
                string mimeType;
                string[] streams;
                Warning[] warnings;
                LocalReport report = new LocalReport();
                report.ReportPath = Server.MapPath("~/Reports/GenerateTicketCaixa.rdlc");

                ReportDataSource rdsOrdenProductos = new ReportDataSource();
                rdsOrdenProductos.Name = "dtsInfoBank";
                rdsOrdenProductos.Value = dsData.Tables[0];


                ReportDataSource rdsdtOrders = new ReportDataSource();
                rdsdtOrders.Name = "dtsOrder";
                rdsdtOrders.Value = dsData.Tables[1];

                ReportDataSource rdsdtDetails = new ReportDataSource();
                rdsdtDetails.Name = "dtsDetailsBank";
                rdsdtDetails.Value = dsData.Tables[2];

                string PmDVT3Calculate = DVT3Calculate(dsData.Tables[1], dsData.Tables[0]);


                Byte[] bitmapData = CodeBar(CreateCodebarCaixa(dsData.Tables[1], dsData.Tables[0]));

                DataTable dtCodeBar = new DataTable("dtImage");
                DataColumn dc = new DataColumn("CodeBar");
                dc.DataType = typeof(byte[]);
                dtCodeBar.Columns.Add(dc);
                dtCodeBar.Rows.Add(new Object[] { bitmapData });

                ReportDataSource rdsdstImage = new ReportDataSource();
                rdsdstImage.Name = "dstImage";
                rdsdstImage.Value = dtCodeBar;

                report.DataSources.Add(rdsOrdenProductos);
                report.DataSources.Add(rdsdtOrders);
                report.DataSources.Add(rdsdtDetails);
                report.DataSources.Add(rdsdstImage);

                report.SetParameters(new List<ReportParameter>() { new ReportParameter("PmDVT3Calculate", PmDVT3Calculate) });

                Byte[] mybytes = report.Render("pdf", null,
                                    out extension, out encoding,
                                    out mimeType, out streams, out warnings); //for exporting to PDF  


                return mybytes;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private byte[] CrearTicketItau(int TicketNumber)
        {
            try
            {
                DataSet dsData = new DataSet();
                dsData = PaymetTycketsReportBusinessLogic.GenerateTicketItau(TicketNumber);

                string extension;
                string encoding;
                string mimeType;
                string[] streams;
                Warning[] warnings;
                LocalReport report = new LocalReport();
                report.ReportPath = Server.MapPath("~/Reports/GenerateTicketItau.rdlc");

                ReportDataSource rdsInfoBank = new ReportDataSource();
                rdsInfoBank.Name = "dtsInfoBank";//This refers to the dataset name in the RDLC file  
                rdsInfoBank.Value = dsData.Tables[0];


                ReportDataSource rdsdtOrder = new ReportDataSource();
                rdsdtOrder.Name = "dtsOrder";//This refers to the dataset name in the RDLC file  
                rdsdtOrder.Value = dsData.Tables[1];

                ReportDataSource rdsdtDetails = new ReportDataSource();
                rdsdtDetails.Name = "dtsDetailsBank";
                rdsdtDetails.Value = dsData.Tables[2];

                Byte[] bitmapData = CodeBar(CreateCodebarTicketItau(dsData.Tables[1], dsData.Tables[0]));

                DataTable dtCodeBar = new DataTable("dtImage");
                DataColumn dc = new DataColumn("CodeBar");
                dc.DataType = typeof(byte[]);
                dtCodeBar.Columns.Add(dc);
                dtCodeBar.Rows.Add(new Object[] { bitmapData });
                ReportDataSource rdsdstImage = new ReportDataSource();
                rdsdstImage.Name = "dstImage";
                rdsdstImage.Value = dtCodeBar;

                report.DataSources.Add(rdsInfoBank);
                report.DataSources.Add(rdsdtOrder);
                report.DataSources.Add(rdsdtDetails);
                report.DataSources.Add(rdsdstImage);


                Byte[] mybytes = report.Render("pdf", null,
                                    out extension, out encoding,
                                    out mimeType, out streams, out warnings); //for exporting to PDF  
                return mybytes;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static byte[] ExtractPages(Byte[] sourcePdfPath)
        {
            iTextSharp.text.pdf.PdfReader reader = null;
            iTextSharp.text.Document sourceDocument = null;
            iTextSharp.text.pdf.PdfCopy pdfCopyProvider = null;
            iTextSharp.text.pdf.PdfImportedPage importedPage = null;
            System.IO.MemoryStream target = new System.IO.MemoryStream();
            reader = new iTextSharp.text.pdf.PdfReader(sourcePdfPath);
            int numberOfPages = reader.NumberOfPages;

            sourceDocument = new iTextSharp.text.Document(reader.GetPageSizeWithRotation(1));
            pdfCopyProvider = new iTextSharp.text.pdf.PdfCopy(sourceDocument, target);
            sourceDocument.Open();
            for (int i = 1; i <= numberOfPages; i++)
            {
                String pageText = iTextSharp.text.pdf.parser.PdfTextExtractor.GetTextFromPage(reader, i);

                if (pageText.Equals(""))
                    continue;

                importedPage = pdfCopyProvider.GetImportedPage(reader, i);
                pdfCopyProvider.AddPage(importedPage);
            }

            sourceDocument.Close();
            reader.Close();

            return target.ToArray();
        }

        #endregion

        public virtual ActionResult GetProductCredit(int page, int pageSize)
        {
            try
            {
                var accountLedgers = _productCreditLedgerService.RetrieveLedger(CurrentAccount.AccountID);
                if (accountLedgers != null && accountLedgers.Any())
                {
                    var entries = accountLedgers.OrderByDescending(le => le.EffectiveDate).ThenByDescending(le => le.EntryId);
                    StringBuilder builder = new StringBuilder();

                    int count = 0;
                    foreach (var entry in entries.Skip(page * pageSize).Take(pageSize))
                    {
                        var currency = SmallCollectionCache.Instance.Currencies.FirstOrDefault(c => c.CurrencyID == entry.CurrencyTypeId);
                        var origin = _productCreditLedgerService.GetEntryOrigin(entry.EntryOrigin.EntryOriginId);

                        IBonusKind bonusKind = null;
                        if (entry.BonusTypeId.GetValueOrDefault() > 0)
                        {
                            bonusKind = _commissionsService.GetBonusKind(entry.BonusTypeId.Value);
                        }
                        builder.Append("<tr class=\"").Append(count % 2 == 1 ? "Alt" : String.Empty).Append(entry.EntryAmount < 0 ? " Negative" : " Positive").Append("\">")

                            .AppendCell(Translation.GetTerm(entry.EntryReason.TermName))
                            .AppendCell(origin != null ? origin.TermName : String.Empty)
                            .AppendCell(Translation.GetTerm(entry.EntryKind.TermName))
                            .AppendCell(entry.EffectiveDate.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo))
                            .AppendCell(entry.EntryAmount.ToString("C", currency.Culture))
                            .AppendCell(entry.EndingBalance.GetValueOrDefault().ToString("C", currency.Culture))
                           .AppendCell(entry.OrderPaymentId.ToString())
                             .AppendCell(entry.OrderId.ToString())
                            .AppendCell(bonusKind != null ? Translation.GetTerm(bonusKind.TermName) : String.Empty)
                            .Append("</td></tr>");
                        ++count;
                    }
                    return Json(new { totalPages = Math.Ceiling(entries.Count() / (double)pageSize), page = builder.ToString() });
                }
                return Json(new { totalPages = 0, page = "<tr><td colspan=\"7\">" + Translation.GetTerm("NoLedgerEntries", "No ledger entries.") + "</td></tr>" });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (CurrentAccount != null) ? CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        private void insertDispatchProductsFill()
        {
            // ------------------------------------------------------------------------------------------------------------------------------------------------
            /*
             * Recorre la variable de session registrada con los Dispatch filtrados y los almacena en la base de datos dentro de la tabla [DispatchItemControls]
             * wv: 20160531
             * Session => listDispatChProducts
             * 
             */

            var lstProductsVal = (List<DispatchProducts>)Session["listDispatChProducts"];
            bool endInsert = false;
            Dictionary<int, bool> periodIDSel = Periods.GetPeriodByDate(DateTime.Now);
            int periodVal = Convert.ToInt32(periodIDSel.Keys.ElementAt(0));
            int resultado;
            if (lstProductsVal != null)
            {
                foreach (var item in lstProductsVal)
                {
                    try
                    {
                        resultado = OrderExtensions.insertDispatchProducts(item.OrderDispatchID, OrderContext.Order.OrderCustomers[0].AccountID, item.Quantity, OrderContext.Order.OrderID, periodVal, item.OrderItemID, item.ProductId, item.Name, item.SKU);
                    }
                    catch
                    {
                        endInsert = true;
                    }
                }
            }

            // ------------------------------------------------------------------------------------------------------------------------------------------------
        }

        #region Valida Pago con PayPal Correcto

        private bool validaPayPal_Paid()
        {

            bool valida = false;
            int paymentTypeID = 0;
            string payPal_PaidID = string.Empty;
            PaymentDeclinedModel paymentDecline = new PaymentDeclinedModel();

            PaymentsTable objE = new PaymentsTable();
            objE.PreOrderID = Convert.ToInt32(Session["PreOrder"]);
            var ct = Order.GetPaymentsTable(objE);

            if (OrderContext.Order.AsOrder().OrderPayments.Count > 1)
            {
                for (int i = 0; i < OrderContext.Order.AsOrder().OrderPayments.Count; i++)
                {
                    if (OrderContext.Order.AsOrder().OrderPayments[i].PaymentTypeID == 1)
                    {
                        paymentTypeID = OrderContext.Order.AsOrder().OrderPayments[i].PaymentTypeID;
                        payPal_PaidID = OrderContext.Order.AsOrder().OrderPayments[i].TransactionID;

                        var paymentTable = ct[i];
                        paymentDecline.PaymentDecMon = paymentTable.AppliedAmount.ToString();
                        paymentDecline.PaymentDecCuo = paymentTable.NumberCuota.ToInt();
                    }
                }
            }
            else
            {
                paymentTypeID = OrderContext.Order.AsOrder().OrderPayments[0].PaymentTypeID;
                payPal_PaidID = OrderContext.Order.AsOrder().OrderPayments[0].TransactionID;

                if (paymentTypeID == 1)
                {
                    var paymentTable = ct[0];
                    paymentDecline.PaymentDecMon = paymentTable.AppliedAmount.ToString();
                    paymentDecline.PaymentDecCuo = paymentTable.NumberCuota.ToInt();
                }
            }


            if (paymentTypeID == 1 && !payPal_PaidID.IsNull())
            {
                valida = true;
            }
            else if (paymentTypeID != 1)
            {
                valida = true;
            }
            else
            {
                paymentDecline.TypeError = "{Type: No IFrame PayPal, Message: [No se Activo la Pantalla de Pasarela de pago con PayPal] }";
                paymentDecline.PaymentGatewayID = '0';
                paymentDecline.AccountId = OrderContext.Order.AsOrder().OrderCustomers[0].AccountID != null ? OrderContext.Order.AsOrder().OrderCustomers[0].AccountID : 0;
                paymentDecline.OrderID = OrderContext.Order.AsOrder().OrderNumber != null ? OrderContext.Order.AsOrder().OrderNumber.ToInt() : 0;

                var statusID = DataAccess.ExecWithStoreProcedureSave("Core", "InsertPaymentDeclined",
                                                                new SqlParameter("TypeError", SqlDbType.VarChar) { Value = paymentDecline.TypeError },
                                                                new SqlParameter("OrderID", SqlDbType.Int) { Value = paymentDecline.OrderID },
                                                                new SqlParameter("PaymentDecMon", SqlDbType.Decimal) { Value = paymentDecline.PaymentDecMon },
                                                                new SqlParameter("PaymentDecCuo", SqlDbType.Int) { Value = paymentDecline.PaymentDecCuo },
                                                                new SqlParameter("AccountId", SqlDbType.Int) { Value = paymentDecline.AccountId },
                                                                new SqlParameter("PaymentGatewayID", SqlDbType.SmallInt) { Value = paymentDecline.PaymentGatewayID });

                valida = false;
            }

            return valida;
        }

        #endregion

    }
}//

