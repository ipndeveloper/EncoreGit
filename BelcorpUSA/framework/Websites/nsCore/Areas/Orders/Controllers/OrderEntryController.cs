using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common.Base;
using NetSteps.Common.Configuration;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Common.Context;
using NetSteps.Data.Common.Models;
using NetSteps.Data.Common.Services;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Business.HelperObjects.PaymentTypes;
using NetSteps.Data.Entities.Business.HelperObjects.PaymentTypes.Interfaces;
using NetSteps.Data.Entities.Business.Interfaces;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Generated;
using NetSteps.Data.Entities.Services;
using NetSteps.Encore.Core.IoC;
using NetSteps.Events.Common.Services;
using NetSteps.GiftCards.Common;
using NetSteps.Promotions.Plugins.Common.Helpers;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Promotions.Plugins.Common.Rewards.Effects.Components;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Controls.Models;
using NetSteps.Web.Mvc.Helpers;
using nsCore.Areas.Orders.Models;
using NetSteps.Commissions.Common;
using NetSteps.Promotions.Common.ModelConcrete;
using NetSteps.Promotions.Common;
using NetSteps.Promotions.Plugins.Common.Rewards.Concrete;
using NetSteps.Promotions.Plugins.Common.Rewards.Effects;
using System.IO;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Data.Entities.Business.Logic;
using System.Data;
using Microsoft.Reporting.WebForms;
using iTextSharp.text.pdf;
using CodeBarGeerator;
using System.Web.Routing;
using System.Data.SqlClient;
using nsCore.Areas.Orders.Models.Paypal;
using nsCore.nsCoreWebConstants;
using NetSteps.Data.Common.Entities;
using NetSteps.Data.Entities.Business.Common;
using OfficeOpenXml.ConditionalFormatting;

namespace nsCore.Areas.Orders.Controllers
{
    public class OrderEntryController : OrdersBaseController
    {
        private readonly IProductCreditLedgerService _productCreditLedgerService =
            Create.New<IProductCreditLedgerService>();

        //public int OrderID; 
        public int wareHouseId;

        [FunctionFilter("Orders-Order Entry", "~/Orders")]
        public virtual ActionResult NewOrder(int? accountId)
        {
            OrderContext.Clear();
            if (accountId.HasValue)
            {
                return RedirectToAction("Index", new { accountId = accountId.Value });
            }
            if (accountId.HasValue)
            {
                return RedirectToAction("Index", new { accountId = accountId.Value });
            }
            Order.paymentReturn = null;
            return View();
        }

        public virtual ActionResult ValidPreOrder()
        {
            if (Session["PreOrder"] == null)
            {
                return Json(new { result = true });
            }
            else
            {
                return Json(new { result = false });
            }
        }

        public virtual ActionResult DeletePreOrder()
        {
            //int SiteID = ConfigurationManager.GetAppSetting<int>(ConfigurationManager.VariableKey.NSCoreSiteID); 
            PreOrder preOrder = Session["PreOrder"] as PreOrder;
            if (preOrder.AccountID > 0)
            {
                int SiteID = ConfigurationManager.GetAppSetting<int>(ConfigurationManager.VariableKey.NSCoreSiteID);
                int PreOrderId = Order.GetPreOders(preOrder.AccountID, SiteID);
                Session["PreOrder"] = null;
            }
            return Json(new { result = true });
        }

        public virtual ActionResult IsCreditCard(int CollectionEntityID)
        {
            var IsTarget = PaymentMethods.IsTargetCreditByPaymentConfiguration(CollectionEntityID);
            var numberTarget = PaymentMethods.GetNumberCuotasByPaymentConfigurationID(CollectionEntityID);
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

        public virtual ActionResult GetPreOrder(int? accountId)
        {
            int SiteID = ConfigurationManager.GetAppSetting<int>(ConfigurationManager.VariableKey.NSCoreSiteID);
            int PreOrderId = Order.GetPreOders(accountId.Value, SiteID);
            if (PreOrderId > 0)
            {
                return Json(new { result = true });
            }
            else
            {
                return Json(new { result = false });
            }
        }

        /// <summary>
        /// This happedn when you clic on new order
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="periodId"></param>
        /// <param name="getPreOrder"></param>
        /// <returns></returns>
        [FunctionFilter("Orders-Order Entry", "~/Orders")]
        public virtual ActionResult Index(int? accountId, int? periodId, bool? getPreOrder)
        {
            try
            {
                /*.14JUL2016.Inicio.Validar Bloqueo de Consultora */
                List<AccountBlocking> blockingSts = new List<AccountBlocking>();
                blockingSts = Account.AccountBlockingStatus(accountId ?? 0, "BLKOGMP");
                if (blockingSts.Count > 0)
                {
                    if (blockingSts[0].Status == true)
                    {
                        MessageHandler.AddError(blockingSts[0].Description);
                        return RedirectToAction("NewOrder", "OrderEntry");
                    }
                }
                /*.14JUL2016.Fin.Validar Bloqueo de Consultora*/
                Session.Remove(SessionConstants.ItemsProductsDispatch);// Wv:20160603 Elimina la Session de los Dispatch filtrados para la señora.
                Session.Remove(SessionConstants.LoadDispatchProcessOrder);// Wv:20160603 Elimina la Session de los Dispatch filtrados para la señora.
                Session[SessionConstants.Vbuilder] = null;
                Session[SessionConstants.VControlGenShipping] = null;

                string pvalShiping = OrderExtensions.GeneralParameterVal(Convert.ToInt32(CoreContext.CurrentMarketId), "SHP");
                Session[SessionConstants.ValorShiping] = pvalShiping;
                if (string.IsNullOrEmpty(pvalShiping))
                {
                    pvalShiping = "N";
                }

                ValidatePaymentByMarket();

                Session[SessionConstants.PreOrder] = null;
                NetSteps.Data.Entities.Business.CTE objN = new NetSteps.Data.Entities.Business.CTE();
                //int statusId = 0;
                int SiteID = ConfigurationManager.GetAppSetting<int>(ConfigurationManager.VariableKey.NSCoreSiteID);
                string orderNumber = Order.GetOrderPending(accountId.Value, SiteID);
                List<PreOrderCondition> orderCondition = Order.GetPreOrderConditions(accountId.Value, CoreContext.CurrentLanguageID);

                #region GetAccountInfo

                Account account = null;
                if (accountId.HasValue)
                {
                    account = Account.LoadForSession(accountId.Value);
                    CoreContext.CurrentAccount = account.Clone();
                    CoreContext.CurrentAccount.StartEntityTracking();
                }
                else if (CoreContext.CurrentAccount != null)
                {
                    account = Account.LoadForSession(accountId.Value);
                }
                // Create a default User so Account can order - JHE
                if (!account.UserID.HasValue)
                {
                    account.User = new User();
                    account.User.Username = Guid.NewGuid().ToString();
                    account.User.Password = NetSteps.Data.Entities.User.GeneratePassword();
                    account.User.UserTypeID = (int)ConstantsGenerated.UserType.Distributor;
                    account.User.UserStatusID = (int)ConstantsGenerated.UserStatus.Active;
                    account.User.DefaultLanguageID = CoreContext.CurrentAccount.DefaultLanguageID;
                    account.Save();
                    account = Account.LoadForSession(accountId.Value);
                    CoreContext.CurrentAccount = account.Clone();
                    CoreContext.CurrentAccount.StartEntityTracking();
                }
                ViewBag.AccountTypeId = Convert.ToInt32(account.AccountTypeID);

                #endregion

                if (orderNumber != "")
                {
                    Session[SessionConstants.PreOrder] = Order.GetPreOrderPending(orderNumber, SiteID);
                    return RedirectToAction("Edit", new { orderNumber = orderNumber });
                }

                //Obtener OrderClains
                foreach (var oCondition in orderCondition)
                {
                    if (!oCondition.Exist)
                    {
                        TempData["Error"] = oCondition.Descriptions;
                        return RedirectToAction("NewOrder", "OrderEntry");
                    }
                }

                // Create new Order
                if (OrderContext.Order == null)
                {
                    OrderContext.Order = new Order(account);
                    OrderContext.Order.OrderTypeID = ConstantsGenerated.OrderType.PortalOrder.ToShort();
                    OrderContext.Order.AsOrder().SiteID = ConfigurationManager.GetAppSetting<int>(ConfigurationManager.VariableKey.NSCoreSiteID);
                    OrderContext.Order.AsOrder().DateCreated = DateTime.Now;

                    var accountMarket = SmallCollectionCache.Instance.Markets.FirstOrDefault(m => m.MarketID == CurrentAccount.MarketID);
                    OrderContext.Order.CurrencyID = accountMarket != null ? accountMarket.GetDefaultCurrencyID() : CoreContext.CurrentMarket.GetDefaultCurrencyID();

                    // Due to the single page flow of order entry in Core we must always calculate shipping and taxes.
                    OrderContext.Order.AsOrder().OrderPendingState = NetSteps.Data.Entities.Constants.OrderPendingStates.Quote;
                    // here created the getPreOrder created the PreOrder
                    Session[SessionConstants.PreOrder] = Order.getPreOrder(account.AccountID, SiteID, null);
                }
                else
                {
                    Session[SessionConstants.PreOrder] = Order.GetProOrderUpdate(account.AccountID, SiteID);
                }
                OrderContext.Order.PreorderID = (int)Session[SessionConstants.PreOrder];

                //========================================================================================= 
                TempData["sPaymentMethod"] = from x in Order.SelectPaymentMethod(account.AccountID, OrderContext.Order.AsOrder().OrderTypeID)
                                             orderby x.Key
                                             select new SelectListItem()
                                             {
                                                 Text = x.Value,
                                                 Value = x.Key
                                             };
                //=========================================================================================
                // Try to get the shipping methods for the default address
                var queryOrderCustomer = OrderContext.Order.AsOrder().OrderCustomers.FirstOrDefault();
                if (queryOrderCustomer != null)
                    queryOrderCustomer.WarehouseID = WarehouseExtensions.WareHouseByAddresID(accountId.Value);
                //TODO: To eliminated the use of WareHouseSession
                Session[SessionConstants.WareHouseId] = WarehouseExtensions.WareHouseByAddresID(accountId.Value);
                OrderContext.Order.WareHouseID = Convert.ToInt32(Session[SessionConstants.WareHouseId]);
                //TODO: This is for Order Adjustments - To Review this
                OrderContext.Order.ParentOrderID = Convert.ToInt32(Session[SessionConstants.WareHouseId]);
                //End

                Address defaultShippingAddress = account.Addresses.GetDefaultByTypeID(ConstantsGenerated.AddressType.Shipping);

                var queryShippingMethod = new List<ShippingMethodWithRate>();
                if (defaultShippingAddress != null)
                {
                    queryShippingMethod = GetNewShippingMethods(defaultShippingAddress.AddressID).ToList();
                }
                ViewData[ViewDataConstants.ShippingMethods] = queryShippingMethod;
                Session[SessionConstants.ExisShippingMetods] = queryShippingMethod.Any();

                //*CS:05ABR2016.Inicio
                ViewData[ViewDataConstants.Catalogs] = Inventory.GetActiveCatalogs(ApplicationContext.Instance.StoreFrontID,
                    ApplicationContext.Instance.CurrentLanguageID);
                /*CS:05ABR2016.Fin*/

                if (ApplicationContext.Instance.UseDefaultBundling)
                {
                    ViewBag.DynamicKitUpSaleHTML = GetDynamicBundleUpSale();
                    ViewBag.AvailableBundleCount = _availableBundleCount;
                }

                SetProductCreditViewData(account.AccountID);
                var viewModel = new OrderEntryModel(OrderContext.Order.AsOrder());

                //Definicion del periodo
                ViewBag.Period = Periods.GetNextPeriodsByAccountType(account.AccountTypeID, 2, OrderContext.Order.OrderID, true);
                Session[SessionConstants.AccountId] = accountId.Value;
                Session[SessionConstants.Edit] = "0";

                SetClaimValues(account.AccountID);

                //CalculateTotal();
                /*CS*/
                LoadAccountCredit();
                ViewBag.CreditAvailable = NetSteps.Data.Entities.Business.CTE.creditPayment.CreditoDisponible;
                ViewBag.EstadoCredito = NetSteps.Data.Entities.Business.CTE.creditPayment.EstadoCredito;

                Session[SessionConstants.ProductCredit] = Order.GetProductCreditByAccount(OrderContext.Order.AsOrder().ConsultantID);

                ViewBag.ProductCredit = (Convert.ToDecimal(Session[SessionConstants.ProductCredit])) < 0
                    ? (Convert.ToDecimal(Session[SessionConstants.ProductCredit]) * -1)
                    : Convert.ToDecimal(Session[SessionConstants.ProductCredit]);
                ViewBag.ProductCreditStatus = Convert.ToDecimal(Session[SessionConstants.ProductCredit]);
                /*CS*/

                ApplyPaymentPreviosBalance();
                OrderService.UpdateOrder(OrderContext);
                actBalanceDue();

                Json(new
                {
                    result = true,
                    message = string.Empty,
                    totals = Totals,
                    paymentsGrid = RenderRazorPartialViewToString("PaymentsGrid", OrderContext.Order.AsOrder()),
                    paymentId = 0
                });

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("NewOrder", "OrderEntry");
            }
        }

        private void SetClaimValues(int accountID)
        {
            var productClains = GetClains(accountID);
            if (productClains != null)
            {
                Session[SessionConstants.IsClains] = productClains.Any();
                if (productClains.Count > 0)
                {
                    Session[SessionConstants.OrderClaimID] = productClains[0].OrderClaimID;
                }

            }
        }

        private List<ProductNameClains> GetClains(int accountId)
        {
            //Found ClainsItems

            var pClains = OrderExtensions.GetProductClains(accountId);
            return pClains;
        }

        private void ValidatePaymentByMarket()
        {
            // N : BRASIL
            // S : USA
            Session["GeneralParameterVal"] = OrderExtensions.GeneralParameterVal(CoreContext.CurrentMarketId, "ANM");
        }


        //Session["ProductClains"] = objProductClains;
        public void RemovePaymentsTable(int Indice)
        {
            PaymentsTable objE = new PaymentsTable();
            objE.ubic = 0;
            objE.PreOrderID = Convert.ToInt32(Session["PreOrder"]);
            objE.OrderPaymentId = Indice;
            PaymentsMethodsExtensions.UpdPaymentsTable(objE);
        }

        private void actBalanceDue()
        {
            var paymentTotal = Totals.GetType().GetProperty("paymentTotal").GetValue(Totals, null);
            string[] separatorPayments = Convert.ToString(paymentTotal).Split('$');

            var grandTotal = Totals.GetType().GetProperty("grandTotal").GetValue(Totals, null);
            string[] separatorProducts = Convert.ToString(grandTotal).Split('$');

            var comisionableTotal = Totals.GetType().GetProperty("balanceAmount").GetValue(Totals, null);
            string[] separatorCommissionable = Convert.ToString(comisionableTotal).Split('$');
            //Session["balanceDue"] = Convert.ToDecimal(separatorCommissionable[1]);
            //var viewModel = new OrderEntryModel(OrderContext.Order.AsOrder());
            decimal balanceDue = 0;
            decimal val_payments = 0;
            decimal val_products = 0;


            val_payments =
                NetSteps.Data.Entities.Extensions.DecimalExtensions.FormatGlobalizationDecimal(
                    Convert.ToDecimal(separatorCommissionable[0]));

            OrderContext.Order.AsOrder().Balance = val_payments;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Orders-Order Entry", "~/Orders")]
        public virtual ActionResult ApplyPaymentPreviosBalance()
        {
            try
            {
                if (OrderContext.Order.AsOrder().OrderStatusID != 4)
                {
                    RemovePaymentsTable(0);
                    decimal Amount = Order.GetProductCreditByAccount(OrderContext.Order.AsOrder().ConsultantID);
                    OrderContext.Order.AsOrder().OrderCustomers[0].RemoveAllPayments();
                    int exitPB = 0;
                    exitPB =
                        OrderContext.Order.AsOrder()
                            .OrderPayments.Where(x => x.BankName == "PREVIOS BALANCE")
                            .ToList()
                            .Count();

                    if (Amount == 0 || exitPB > 0)
                    {
                        return Json(new { result = false, message = "" });
                    }
                    else
                    {
                        PaymentTypeModel paymentTypeModel = new PaymentTypeModel();
                        paymentTypeModel.PaymentMethodID = 60;
                        paymentTypeModel.NameOnCard = "PREVIOS BALANCE";
                        paymentTypeModel.Amount = Amount;

                        IPayment payment = new Payment()
                        {
                            DecryptedAccountNumber = paymentTypeModel.AccountNumber,
                            CVV = paymentTypeModel.Cvv,
                            PaymentType = ConstantsGenerated.PaymentType.EFT,
                            NameOnCard = paymentTypeModel.AccountNumber,
                            BankName = paymentTypeModel.NameOnCard
                        };

                        payment.PaymentTypeID = Order.GetApplyPaymentType(paymentTypeModel.PaymentMethodID.Value);

                        BasicResponseItem<OrderPayment> response =
                            OrderContext.Order.AsOrder()
                                .ApplyPaymentToCustomerPreviosBalance(paymentTypeModel.PaymentType,
                                    paymentTypeModel.Amount, payment, user: CoreContext.CurrentUser);

                        if (!response.Success)
                        {
                            return Json(new { result = false, message = response.Message });
                        }
                        OrderService.UpdateOrder(OrderContext);
                        ApplyPaymentSearchData objE = new ApplyPaymentSearchData();
                        objE.OrderPaymentId = response.Item.OrderPaymentID;
                        objE.Amount = paymentTypeModel.Amount;
                        objE.PaymentConfigurationID = paymentTypeModel.PaymentMethodID;
                        objE.NameOnCard = "";
                        objE.PaymentConfigurationID = paymentTypeModel.PaymentMethodID;

                        if (payment.PaymentTypeID > 1)
                        {
                            objE.NumberCuota = null;
                        }
                        else
                        {
                            if (paymentTypeModel.NumberCuota != null)
                                objE.NumberCuota = paymentTypeModel.NumberCuota.Value;
                        }
                        NetSteps.Data.Entities.Business.CTE.paymentTables = new List<PaymentsTable>();
                        objE.PreOrderID = Convert.ToInt32(Session["PreOrder"]);
                        Session["FinalPaymentStatusID"] = NetSteps.Data.Entities.Business.CTE.ApplyPayment(objE);

                        int OrderStatusId = Order.GetApplyPayment(objE);

                        return Json(new { result = false, message = "" });
                    }
                }
                return Json(new { result = false, message = "" });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex,
                    Constants.NetStepsExceptionType.NetStepsApplicationException,
                    OrderContext.Order != null ? OrderContext.Order.OrderID.ToIntNullable() : null,
                    accountID:
                    CoreContext.CurrentAccount != null ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

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
                if (tabl.EstadoCredito == "S")
                {
                    resp = 1;
                }

                if (tabl.EstadoCredito == "N")
                {
                    switch (tabl.TipoCredito)
                    {
                        case "P":
                            ValorComparacion = order.QualificationTotal.ToDecimal();
                            break;
                        case "D":
                            ValorComparacion = order.GrandTotal.ToDecimal(); // + CreditBalanceDue =  Balance
                            break;
                        default:
                            break;
                    }

                    NetSteps.Data.Entities.Business.CTE.creditPayment.ValorComparacion =
                        ValorComparacion.ToString().Replace(",", ".");
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
                CreditoDisponible = (rep == null) ? "0" : rep.AccountCreditDis.ToString(),
                EstadoCredito = (rep == null) ? "N" : rep.AccountCreditEst
            };
            NetSteps.Data.Entities.Business.CTE.creditPayment = tb;
        }

        public void CalculateTotal()
        {
            try
            {
                LoadAccountCredit();
                var comisionableTotal = Totals.GetType()
                    .GetProperty("subTotalPreadjustedItemPrice")
                    .GetValue(Totals, null);
                string[] separatorCommissionable = Convert.ToString(comisionableTotal).Split('$');
                ViewBag.CreditAvailable = NetSteps.Data.Entities.Business.CTE.creditPayment.CreditoDisponible;
                // Order.GetCredit().ToString('S');
                ViewBag.EstadoCredito = NetSteps.Data.Entities.Business.CTE.creditPayment.EstadoCredito;

                decimal valseparatorCommissionable = 0;
                var KeyDecimals = ConfigurationManager.AppSettings["CultureDecimal"];
                if (KeyDecimals == "ES")
                {
                    var culture = CultureInfoCache.GetCultureInfo("En");
                    valseparatorCommissionable = Convert.ToDecimal(separatorCommissionable[1], culture);
                }
                else
                {
                    valseparatorCommissionable = Convert.ToDecimal(separatorCommissionable[1]);
                }

                ViewBag.CommisionableTotal = valseparatorCommissionable.ToString('S');
                ViewBag.QualificationTotal = valseparatorCommissionable.ToString('S');
                Session["ProductCredit"] = Order.GetProductCreditByAccount(OrderContext.Order.AsOrder().ConsultantID);

                ViewBag.ProductCredit = (Convert.ToDecimal(Session["ProductCredit"])) < 0
                    ? (Convert.ToDecimal(Session["ProductCredit"]) * -1)
                    : Convert.ToDecimal(Session["ProductCredit"]);
                ViewBag.ProductCreditStatus = Convert.ToDecimal(Session["ProductCredit"]);
            }
            catch (Exception)
            {
            }
        }

        protected virtual void SetProductCreditViewData(int? accountID = null)
        {
            if (!accountID.HasValue)
                accountID = CoreContext.CurrentAccount.AccountID;
            var productCreditBalance = _productCreditLedgerService.GetCurrentBalanceLessPendingPayments(accountID.Value);

            ViewData["ProductCreditBalance"] = productCreditBalance.ToMoneyString();
        }

        [FunctionFilter("Orders-Order Entry", "~/Orders")]
        public virtual ActionResult Edit(string orderNumber)
        {
            try
            {
                /*
               En el edit nunca debe de haber un metodo de pago registrado( en orderpayments),
               este error se estaba presentando(en el boton de realizar el pedido) ya que el usuario sale del form. de paypal de una manera inusual                 
               Con esto nos aseguramos que al editar su pedido pendiente  este lo habra y no nuestre ningun pago( salvo que sea residual  pero eso se controla al finalizar el edit) .*/
                AccountPropertiesBusinessLogic.GetValueByID(Constants.OrderPayments.Delete.ToInt(), orderNumber.ToInt());

                Session["vbuilder"] = null;
                Session["vControlGenShipping"] = null;
                Session["AccountNumber"] = 0; //Inicializar la variable de sessionTest 
                NetSteps.Data.Entities.Business.CTE objN = new NetSteps.Data.Entities.Business.CTE();
                OrderContext.Clear();
                if (!orderNumber.IsNullOrEmpty())
                {
                    OrderContext.Order = Order.LoadByOrderNumberFull(orderNumber);
                }

                if (OrderContext.Order == null) return RedirectToAction("NewOrder");
                if (OrderContext.Order.OrderStatusID != (short)ConstantsGenerated.OrderStatus.Pending)
                {
                    return RedirectToAction("Index",
                        new RouteValueDictionary(new { controller = "Details", action = "Index", id = orderNumber }));
                }

                if (OrderContext.Order.AsOrder().SiteID == 0)
                    OrderContext.Order.AsOrder().SiteID =
                        ConfigurationManager.GetAppSetting<int>(ConfigurationManager.VariableKey.NSCoreSiteID);

                OrderCustomer customer = (OrderContext.Order.AsOrder().OrderCustomers != null &&
                                          OrderContext.Order.AsOrder().OrderCustomers.Count > 0)
                    ? OrderContext.Order.AsOrder().OrderCustomers[0]
                    : null;

                Account account = null;
                if (customer != null && customer.AccountID != 0)
                {
                    account = Account.LoadFull(customer.AccountID);
                    CoreContext.CurrentAccount = account.Clone();
                    CoreContext.CurrentAccount.StartEntityTracking();
                }

                ViewBag.AccountTypeId = Convert.ToInt32(account.AccountTypeID);

                OrderShipment shipment = OrderContext.Order.AsOrder().GetDefaultShipmentNoDefault();
                if (account != null)
                {
                    // Try to get the shipping methods for the default address
                    Address defaultShippingAddress =
                        account.Addresses.GetDefaultByTypeID(ConstantsGenerated.AddressType.Shipping);

                    // Default address if not set from loaded order - JHE
                    // TODO: We should probably put something in the UI to indicate that some un-saved changed are being displayed. - JHE
                    if (shipment == null || shipment.IsEmpty())
                        OrderContext.Order.AsOrder()
                            .UpdateOrderShipmentAddress(shipment, defaultShippingAddress.AddressID);

                    try
                    {
                        TempData["sPaymentMethod"] =
                            from x in
                                Order.SelectPaymentMethod(OrderContext.Order.ConsultantID,
                                    OrderContext.Order.AsOrder().OrderTypeID)
                            orderby x.Key
                            select new SelectListItem()
                            {
                                Text = x.Value,
                                Value = x.Key
                            };
                    }
                    catch (ShippingMethodNotAvailableException)
                    {
                        // Default to available shipping method if the method that used to be on the order is no longer available - JHE
                        var shippingMethods = defaultShippingAddress != null
                            ? OrderContext.Order.AsOrder()
                                .UpdateOrderShipmentAddressAndDefaultShipping(defaultShippingAddress.AddressID)
                            : new List<ShippingMethodWithRate>();
                        ViewData["ShippingMethods"] = shippingMethods;
                        var shippingMethodWithRates = ViewData["ShippingMethods"] as IEnumerable<ShippingMethodWithRate>;
                        Session["ExisShippingMetods"] = true;
                        if (shippingMethodWithRates.Count() == 0)
                        {
                            Session["ExisShippingMetods"] = false;
                        }
                        if (shippingMethods.Count() > 0)
                        {
                            OrderContext.Order.AsOrder()
                                .SetShippingMethod(
                                    shippingMethods.OrderBy(s => s.ShippingAmount).FirstOrDefault().ShippingMethodID);
                        }
                        TempData["Error"] =
                            "The existing shipping method is no longer available for selection. The order was defaulted to cheapest method available now.";
                    }
                }
                Session[SessionConstants.WareHouseId] = WarehouseExtensions.WareHouseByAddresID(account.AccountID);
                OrderContext.Order.ParentOrderID = Convert.ToInt32(Session[SessionConstants.WareHouseId]);
                OrderContext.Order.PreorderID = (int)Session[SessionConstants.PreOrder];
                OrderService.UpdateOrder(OrderContext);

                Address defaultShippingAddres =
                    account.Addresses.GetDefaultByTypeID(ConstantsGenerated.AddressType.Shipping);

                IEnumerable<ShippingMethodWithRate> NewShippingMethodsList =
                    GetNewShippingMethods(defaultShippingAddres.AddressID); //RECENTLY ADDED BY WCS

                ViewData["ShippingMethods"] = defaultShippingAddres != null
                    ? NewShippingMethodsList
                    : new List<ShippingMethodWithRate>(); //RECENTLY ADDED BY WCS
                var shippingMethodWithRate = ViewData["ShippingMethods"] as IEnumerable<ShippingMethodWithRate>;
                Session["ExisShippingMetods"] = true;
                if (shippingMethodWithRate == null)
                {
                    Session["ExisShippingMetods"] = false;
                }

                IEnumerable<ShippingMethodWithRate> NewShippingMethodsList1 =
                        NewShippingMethodsList.Where(x => x.ShippingMethodID == shipment.ShippingMethodID);
                //RECENTLY ADDED BY WCS
                foreach (var item in NewShippingMethodsList1) Session["DateEstimated"] = item.DateEstimated; //RECENTLY ADDED BY WCS

                int SiteID = ConfigurationManager.GetAppSetting<int>(ConfigurationManager.VariableKey.NSCoreSiteID);
                int PreOrderId = Convert.ToInt32(Session[SessionConstants.PreOrder]);

                string pvalShiping = OrderExtensions.GeneralParameterVal(Convert.ToInt32(CoreContext.CurrentMarketId), "SHP");
                Session["ValorShiping"] = pvalShiping;

                ValidatePaymentByMarket();
                if (string.IsNullOrEmpty(pvalShiping))
                {
                    pvalShiping = "N";
                }
                if (PreOrderId > 0)
                {
                    // wv: 20160606 Validacion si tiene o no dispatch para ser visualizado
                    List<int> getListDispatchOrder = new List<int>();
                    getListDispatchOrder = OrderExtensions.getListDispatchOrderItems(customer.OrderID);
                    // wv:20160606 Se incluye lista de Dispatch x orderitemsID
                    //Recorre el WareHouseMaterialAllocation
                    foreach (var objWMA in Order.GetMaterialWareHouseMaterial(customer.OrderID))
                    {
                        if (getListDispatchOrder.Exists(x => x.Equals(objWMA.ProductID)))
                        // wv: 20160606 Validación de visualización del producto Dispatch
                        {
                            continue;
                        }
                        bool Exist = false;
                        //Si los campos son iguales en OrderItems
                        foreach (
                            var objOI in
                            Order.GetMaterialOrderItem(customer.OrderCustomerID)
                                .Where(x => x.MaterialID == objWMA.MaterialID))
                        {
                            Exist = true;
                        }
                        if (!Exist)
                        {
                            //Borrar y actualizar el stock 
                            PreOrderExtension.UpdWarehouseMaterial(objWMA.MaterialID, objWMA.Quantity,
                                objWMA.WareHouseID);
                            //Elimino del HouseMaterialsAllocations
                            PreOrderExtension.DelWareHouseMaterialsAllocationsXPreOrder(objWMA.ProductID,
                                Convert.ToInt32(Session["PreOrder"]));
                        }
                    }
                }
                ViewBag.Period = Periods.GetNextPeriodsByAccountType(account.AccountTypeID, 2,
                    OrderContext.Order.OrderID, true); //Periods.GetPeriodByDate(DateTime.Now);
                // TODO: Change back to the normal Inventory cache
                ViewData["Catalogs"] = Inventory.GetActiveCatalogs(ApplicationContext.Instance.StoreFrontID,
                    ApplicationContext.Instance.CurrentLanguageID);

                if (ApplicationContext.Instance.UseDefaultBundling)
                {
                    ViewBag.DynamicKitUpSaleHTML = GetDynamicBundleUpSale();
                }

                int ppt = OrderContext.Order.AsOrder().OrderCustomers[0].ProductPriceTypeID;
                //Si fueron eliminadas el WareHouseMaterialAllocations
                if (Order.GetMaterialWareHouseMaterial(customer.OrderID).Count == 0)
                {
                    //Actualizar los stocks
                    foreach (var objOI in Order.uspGetOrderItemsStock(customer.OrderCustomerID))
                    {
                        foreach (var objMsg in Order.AddLineOrder(objOI.ProductID,
                                                                  objOI.Quantity,
                            Convert.ToInt32(Session[SessionConstants.WareHouseId]),
                            Convert.ToInt32(Session[SessionConstants.PreOrder]),
                                                                  account.AccountTypeID,
                                                                  false))
                        {
                            //Si no hay stock eliminar el Item
                            if (!objMsg.Estado)
                            {
                            }
                        }
                    }
                }

                calculateDispatch();
                ApplyPaymentPreviosBalance();
                OrderEntryModel viewModel = new OrderEntryModel(OrderContext.Order.AsOrder());
                Session[SessionConstants.Edit] = "1";
                CalculateTotal();
                actBalanceDue();
                return View("Index", viewModel);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex,
                    Constants.NetStepsExceptionType.NetStepsApplicationException,
                    OrderContext.Order != null ? OrderContext.Order.OrderID.ToIntNullable() : null,
                    accountID:
                    CoreContext.CurrentAccount != null ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
                throw exception;
            }
        }

        void calculateDispatch()
        {
            var dispatchs = OrderExtensions.GetDispatchByOrder(OrderContext.Order.OrderID);
            int countDispatch = 0;
            decimal totalDispatch = 0;
            foreach (var dispa in dispatchs)
            {
                foreach (
                    var oi in
                    OrderContext.Order.AsOrder().OrderCustomers[0].OrderItems.Where(
                        x => x.OrderItemID == dispa.OrderItemID))
                {
                    RemoverItems(oi.OrderItemID);
                    break;
                    //totalDispatch = totalDispatch + (oi.ItemPrice * oi.Quantity);
                    //countDispatch++;
                    //oi.ShippingTotal = 0;
                    //oi.TaxAmount = 0;
                    //oi.ShippingTotalOverride = 0;
                    //oi.ItemPrice = 0;
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
                    foreach (
                        var oi in
                        OrderContext.Order.AsOrder().OrderCustomers[0].OrderItems.Where(
                            x => x.OrderItemID != dispa.OrderItemID))
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
                OrderContext.Order.Subtotal = totalOrder;
                OrderContext.Order.GrandTotal = totalOrder;
                OrderContext.Order.AsOrder().OrderCustomers[0].AdjustedSubTotal = totalOrder;
                //OrderContext.Order.AsOrder().OrderCustomers[0].ProductSubTotal =  OrderContext.Order.AsOrder().OrderCustomers[0].ProductSubTotal - totalDispatch;
            }
        }

        [FunctionFilter("Orders-Order Entry", "~/Orders")]
        public virtual ActionResult RecalculateOrder(string orderNumber)
        {
            try
            {
                if (OrderContext.Order == null || OrderContext.Order.OrderID.ToString() != orderNumber)
                {
                    OrderContext.Clear();
                    if (!orderNumber.IsNullOrEmpty())
                    {
                        OrderContext.Order = Order.LoadByOrderNumberFull(orderNumber);
                    }
                }

                if (OrderContext.Order != null)
                {
                    OrderContext.Order.AsOrder().OrderPendingState = Constants.OrderPendingStates.Quote;
                    if (ApplicationContext.Instance.UseDefaultBundling)
                    {
                        ((OrderService)OrderService).AutoBundleItems(OrderContext);
                    }
                    OrderService.UpdateOrder(OrderContext);
                    OrderContext.Order.AsOrder().Save();
                }
                else
                {
                    TempData["Error"] = "Order " + orderNumber + " not found.";
                }

                return RedirectToAction("Index", "Details", new { id = orderNumber });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index", "Details", new { id = orderNumber });
            }
        }

        [FunctionFilter("Orders-Order Entry", "~/Orders")]
        public virtual ActionResult ChangeToPaidStatus(string orderNumber)
        {
            try
            {
                Order order = null;
                if (!string.IsNullOrEmpty(orderNumber))
                    order = Order.LoadByOrderNumberFull(orderNumber);

                if (order != null)
                {
                    OrderService.UpdateOrder(OrderContext);

                    if (order.CanChangeToPaidStatus())
                    {
                        order.ChangeToPaidStatus();
                    }
                    else
                    {
                        TempData["Error"] = "This order cannot be changed to Paid status";
                        return RedirectToAction("Index", "Details", new { id = orderNumber });
                    }
                }
                return RedirectToAction("Index", "Details", new { id = orderNumber });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index", "Details", new { id = orderNumber });
            }
        }

        #region Order Type

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Orders-Order Entry", "~/Orders")]
        public virtual ActionResult UpdateMarkAsAutoship(bool markAsAutoship)
        {
            try
            {
                UpdateOrderType(markAsAutoship);
                OrderContext.Order.AsOrder().SetConsultantID(CurrentAccount);
                return Json(new { success = true, shippingMethods = ShippingMethods, totals = Totals });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex,
                    Constants.NetStepsExceptionType.NetStepsApplicationException,
                    orderID: OrderContext.Order != null ? OrderContext.Order.OrderID.ToIntNullable() : null,
                    accountID:
                    CoreContext.CurrentAccount != null ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { success = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Orders-Order Entry", "~/Orders")]
        public virtual ActionResult GetStatusMarkAsAutoship()
        {
            try
            {
                string StatusMarkAsAutoship = OrderExtensions.GeneralParameterVal(CoreContext.CurrentMarketId, "FTO");
                return Json(new { StatusMarkAsAutoship = StatusMarkAsAutoship.Equals("P") ? 1 : 0 });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex,
                    Constants.NetStepsExceptionType.NetStepsApplicationException,
                    orderID: OrderContext.Order != null ? OrderContext.Order.OrderID.ToIntNullable() : null,
                    accountID:
                    CoreContext.CurrentAccount != null ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { success = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Orders-Order Entry", "~/Orders")]
        public virtual ActionResult GetStatusMarkOrderDate()
        {
            try
            {
                string StatusMarkOrderDate = OrderExtensions.GeneralParameterVal(CoreContext.CurrentMarketId, "DPD");
                return Json(new { StatusMarkOrderDate = StatusMarkOrderDate.Equals("P") ? 1 : 0 });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex,
                    Constants.NetStepsExceptionType.NetStepsApplicationException,
                    orderID: OrderContext.Order != null ? OrderContext.Order.OrderID.ToIntNullable() : null,
                    accountID:
                    CoreContext.CurrentAccount != null ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { success = false, message = exception.PublicMessage });
            }
        }

        #endregion

        #region Product Listing

        [OutputCache(CacheProfile = "AutoCompleteData")]
        public virtual ActionResult SearchProducts(string query, bool includeDynamicKits = true)
        {
            try
            {
                List<Product> ProductList =
                    FindProducts(query, includeDynamicKits).Where(p => !p.IsVariantTemplate).Take(10).ToList();
                return Json(ProductList.Select(p => new
                {
                    id = p.ProductID,
                    text = p.SKU + " - " + p.Translations.Name(),
                    isDynamicKit = p.IsDynamicKit(),
                    needsBackOrderConfirmation =
                    Inventory.IsOutOfStock(p) &&
                    p.ProductBackOrderBehaviorID ==
                    ConstantsGenerated.ProductBackOrderBehavior.AllowBackorderInformCustomer.ToInt()
                }));
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex,
                    Constants.NetStepsExceptionType.NetStepsApplicationException,
                    OrderContext.Order != null ? OrderContext.Order.OrderID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        protected virtual IEnumerable<Product> FindProducts(string query, bool includeDynamicKits)
        {
            //return Inventory.SearchProducts(NetSteps.Data.Entities.ApplicationContext.Instance.StoreFrontID, query, CoreContext.CurrentAccount.AccountTypeID)
            //       .Where(p => (!Inventory.IsOutOfStock(p) || p.ProductBackOrderBehaviorID != NetSteps.Data.Entities.Constants.ProductBackOrderBehavior.Hide.ToInt()));

            //.Where(p => p.Active && (!Inventory.IsOutOfStock(p) || p.ProductBackOrderBehaviorID != NetSteps.Data.Entities.Constants.ProductBackOrderBehavior.Hide.ToInt()));

            return
                Product.SearchProductForOrder(query)
                    .Where(
                        p =>
                            (!Inventory.IsOutOfStock(p) ||
                             p.ProductBackOrderBehaviorID !=
                             ConstantsGenerated.ProductBackOrderBehavior.Hide.ToInt()));
        }


        [OutputCache(CacheProfile = "AutoCompleteData")]
        public virtual ActionResult InPageSearch(string query, int? kitProductId = null, int? dynamicKitGroupId = null,
            string sku = null)
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
                        products =
                            Inventory.SearchProducts(ApplicationContext.Instance.StoreFrontID, query,
                                    CoreContext.CurrentAccount.AccountTypeID, includeDynamicKits: false)
                                .Where(p => !p.IsDynamicKit()).ToList();
                    }
                    else
                    {
                        products =
                            Inventory.SearchProducts(ApplicationContext.Instance.StoreFrontID, query,
                                    includeDynamicKits: false)
                                .Where(p => !p.IsDynamicKit()).ToList();
                    }


                    if (kitProductId != null && dynamicKitGroupId != null)
                    {
                        products =
                            products.Where(
                                    p => p.CanBeAddedToDynamicKitGroup(kitProductId.Value, dynamicKitGroupId.Value))
                                .ToList();
                    }
                }

                return
                    Json(products.Where(
                            p =>
                                !outOfStockProducts.Contains(p.ProductID) ||
                                p.ProductBackOrderBehaviorID != (int)ConstantsGenerated.ProductBackOrderBehavior.Hide)
                        .ToList()
                        .Select(p => new
                        {
                            id = p.ProductID,
                            text = p.SKU + " - " + p.Translations.Name(),
                            isDynamicKit = p.IsDynamicKit(),
                            needsBackOrderConfirmation =
                            Product.CheckStock(p.ProductID, orderShipment).IsOutOfStock &&
                            p.ProductBackOrderBehaviorID ==
                            ConstantsGenerated.ProductBackOrderBehavior.AllowBackorderInformCustomer.ToShort()
                        }));
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex,
                    Constants.NetStepsExceptionType.NetStepsApplicationException,
                    OrderContext.Order != null ? OrderContext.Order.OrderID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult GetCatalog(int catalogId, int? orderTypeID = null)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                int count = 0;

                //var productIDs = Inventory.GetCatalog(catalogId).CatalogItems.Select(ci => ci.ProductID);
                //var products = Inventory.GetProducts(productIDs).Where(p => p.Active && p.ContainsPrice(CoreContext.CurrentAccount.AccountTypeID));

                var products =
                    Inventory.GetActiveProductsForCatalog(ApplicationContext.Instance.StoreFrontID, catalogId)
                        .OrderBy(x => x.SKU)
                        .Where(
                            p =>
                                p.ContainsPrice(CoreContext.CurrentAccount.AccountTypeID,
                                    OrderContext.Order.CurrencyID, orderTypeID));

                foreach (Product product in products)
                {
                    //bool IsOutOfStock = IsProductAvailable(product.ProductID);
                    bool needsBOConfirmation = product.ProductBackOrderBehaviorID ==
                                               (short)
                                               ConstantsGenerated.ProductBackOrderBehavior.AllowBackorderInformCustomer &&
                                               Inventory.IsOutOfStock(product);
                    //if (IsOutOfStock)
                    //{
                    builder.Append("<tr").Append(count % 2 == 0 ? "" : " class=\"Alt\"").Append(">")
                        .AppendCell(product.SKU, style: "width: 80px;")
                        .AppendCell(product.Name, style: "width: 120px;")
                        .AppendCell(
                            product.Prices.First(
                                pp =>
                                    pp.ProductPriceTypeID ==
                                    AccountPriceType.GetPriceType(CoreContext.CurrentAccount.AccountTypeID,
                                            ConstantsGenerated.PriceRelationshipType.Products, orderTypeID)
                                        .ProductPriceTypeID).Price.ToString(OrderContext.Order.CurrencyID),
                            style: "width: 50px;")
                        .AppendCell(
                            "<input type=\"hidden\" value=\"" + product.ProductID +
                            "\" class=\"productId\"/><input type=\"hidden\" class=\"needsBackOrderConfirmation\" value=\"" +
                            needsBOConfirmation.ToJavascriptBool() +
                            "\" /><input type=\"text\" value=\"0\" style=\"width: 20px;\" class=\"quantity\"/>",
                            style: "width: 50px;")
                        .Append("</tr>");
                    //}
                    //else
                    //{
                    //    //<input type="hidden" class="needsBackOrderConfirmation" value="@needsBOConfirmation.ToJavascriptBool()" />
                    //    //<input type="text" class="quantity" style="width: 1.667em;" disabled="disabled" value="0" />
                    //    builder.Append("<tr").Append(count % 2 == 0 ? "" : " class=\"Alt\"").Append(">")
                    //        .AppendCell(product.SKU, style: "width: 80px;")
                    //        .AppendCell(product.Name, style: "width: 120px;")
                    //        .AppendCell(product.Prices.First(pp => pp.ProductPriceTypeID == AccountPriceType.GetPriceType(CoreContext.CurrentAccount.AccountTypeID, NetSteps.Data.Entities.Constants.PriceRelationshipType.Products, orderTypeID).ProductPriceTypeID).Price.ToString(OrderContext.Order.CurrencyID), style: "width: 50px;")
                    //        .AppendCell("<input type=\"hidden\" value=\"" + product.ProductID + "\" class=\"productId\"/><input type=\"hidden\" class=\"needsBackOrderConfirmation\" value=\"" + needsBOConfirmation.ToJavascriptBool() + "\" /><input type=\"text\" disabled=\"disabled\" value=\"0\" style=\"width: 20px;\" class=\"quantity\"/>", style: "width: 50px;")
                    //        .Append("</tr>");
                    //}
                    ++count;
                }
                return Content(builder.ToString());
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex,
                    Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        #endregion

        #region Cart

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

        public ActionResult CreateDynamicBundleUpSale(int productId)
        {
            Order clonedOrder = OrderContext.Order.AsOrder().Clone();
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
                        message =
                        Translation.GetTerm("BundleCouldNotBeCreated",
                            "The bundle could not be created.  Please try again.")
                    });
                }

                return Json(new
                {
                    result = true,
                    guid = kitGuid,
                    orderCustomerId = customer.OrderCustomerID,
                    orderItems = GetOrderItemsHtml(OrderContext.Order.AsOrder()),
                    shippingMethods = ShippingMethods,
                    totals = Totals,
                    BundleOptionsSpanHTML = GetDynamicBundleUpSale()
                });
            }
            catch (Exception ex)
            {
                //something went wrong in conversion to kit so revert
                OrderContext.Order = clonedOrder;

                NetStepsException exception;
                if (!(ex is NetStepsBusinessException))
                    exception = EntityExceptionHelper.GetAndLogNetStepsException(ex,
                        Constants.NetStepsExceptionType.NetStepsApplicationException);
                else
                    exception = ex as NetStepsException;
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public virtual ActionResult GetGiftStepInfo(string stepId)
        {
            try
            {
                var allSteps =
                    OrderContext.InjectedOrderSteps.Union(
                        OrderContext.Order.OrderAdjustments.SelectMany(oa => oa.InjectedOrderSteps));
                var step =
                    (IUserProductSelectionOrderStep)
                    allSteps.Single(
                        os => os is IUserProductSelectionOrderStep && os.OrderStepReferenceID.ToString() == stepId);
                var inventoryService = Create.New<IInventoryService>();
                var options = step.AvailableOptions.Select(o =>
                {
                    var product = Inventory.GetProduct(o.ProductID);
                    var inventoryResult = inventoryService.GetProductAvailabilityForOrder(OrderContext,
                        product.ProductID, 1);
                    int currencyID = OrderContext.Order.AsOrder().CurrencyID;
                    return new GiftModel
                    {
                        Name = product.Name,
                        Image =
                            product.MainImage != null
                                ? product.MainImage.FilePath.ReplaceFileUploadPathToken()
                                : String.Empty,
                        Description = GetProductShortDescriptionDisplay(product),
                        ProductID = product.ProductID,
                        Value =
                            product.GetPriceByPriceType((int)ConstantsGenerated.ProductPriceType.Retail, currencyID)
                                .ToString(currencyID),
                        IsOutOfStock = inventoryResult.CanAddBackorder == 0 && inventoryResult.CanAddNormally == 0
                    };
                });
                bool? especial = false;
                PromotionOrderAdjustment orderAdjustment =
                    (PromotionOrderAdjustment)
                    OrderContext.Order.AsOrder()
                        .OrderAdjustments.Where(
                            p => p.InjectedOrderSteps.Where(s => s.OrderStepReferenceID == stepId).Count() == 1)
                        .First()
                        .Extension;
                var promo = Create.New<IPromotionService>().GetPromotion(orderAdjustment.PromotionID);
                var reward = (SelectFreeItemsFromListReward)promo.PromotionRewards.First().Value;
                var effect = reward.Effects["ProductID Selector"];
                var extension = (IUserProductSelectionRewardEffect)(effect.Extension);
                especial = extension.IsEspecialPromotion;

                var giftSelectionModel = new GiftSelectionModel(Url.Action("GetGiftStepInfo"), Url.Action("AddGifts"),
                    stepID: stepId, callbackFunctionName: "updateCartAndTotals", isEspecialPromo: especial);

                giftSelectionModel.MaxQuantity = step.MaximumOptionSelectionCount;

                var selections = step.Response == null
                    ? Enumerable.Empty<IProductOption>()
                    : ((IUserProductSelectionOrderStepResponse)step.Response).SelectedOptions;
                giftSelectionModel.SelectedOptions =
                    selections.Select(p => options.First(o => o.ProductID == p.ProductID)).ToList();
                giftSelectionModel.AvailableOptions = options.ToList();

                return Json(new { result = true, GiftSelectionModel = giftSelectionModel });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex,
                    Constants.NetStepsExceptionType.NetStepsApplicationException);
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
                    foreach (var id in productIds)
                    {
                        var option = Create.New<IProductOption>();
                        option.ProductID = id;
                        option.Quantity = 1;
                        response.SelectedOptions.Add(option);
                    }
                }
                var allSteps =
                    OrderContext.InjectedOrderSteps.Union(
                        OrderContext.Order.OrderAdjustments.SelectMany(oa => oa.InjectedOrderSteps));
                var step =
                    (IUserProductSelectionOrderStep)
                    allSteps.Single(
                        os => os is IUserProductSelectionOrderStep && os.OrderStepReferenceID.ToString() == stepId);
                step.Response = response;

                OrderContext.Order.AsOrder().OrderCustomers[0].RemoveAllPayments();
                OrderService.UpdateOrder(OrderContext);

                var showFreeGiftModal = false;
                foreach (var adjustment in OrderContext.Order.OrderAdjustments)
                {
                    foreach (var orderStep in adjustment.InjectedOrderSteps)
                    {
                        showFreeGiftModal = StepHasAnItemInStockToBeChosen(orderStep.OrderStepReferenceID);
                        if (showFreeGiftModal)
                            break;
                    }
                    if (showFreeGiftModal)
                        break;
                }

                return Json(new
                {
                    result = true,
                    orderItems = GetOrderItemsHtml(OrderContext.Order.AsOrder()),
                    totals = Totals,
                    promotions = GetApplicablePromotions(OrderContext.Order.AsOrder()),
                    shippingMethods = ShippingMethods,
                    hasFreeGiftSteps = OrderContext.Order.OrderAdjustments.Any(oa => oa.InjectedOrderSteps.Any()),
                    showFreeGiftModal,
                });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex,
                    Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }


        public virtual ActionResult ListWarehouseMaterialLacks()
        {
            var listWarehouseMaterialLacks =
                    PreOrderExtension.GetWarehouseMaterialLacksByPreOrder(Convert.ToInt32(Session["PreOrder"]), 5);
            //CurrentAccount.Language.LanguageID
            return Json(new { result = true, listWarehouseMaterialLacks = listWarehouseMaterialLacks });
        }


        /// <summary>
        /// Valida si el producto ya esta agregado en la orden
        /// </summary>
        /// <param name="productId">Id del producto que se esta agregando</param>
        /// <returns></returns>
        public ActionResult ExistsProductInOrder(int productId)
        {
            var exists =
                OrderContext.Order.AsOrder().OrderCustomers[0].OrderItems.FirstOrDefault(
                    x => x.ProductID.Equals(productId));
            //return exists == null ? false : true;
            return
                Json(
                    new
                    {
                        result = exists == null ? false : true,
                        message =
                        Translation.GetTerm("ExistsProductInOrder",
                            "This product already exists in the order. You want to add the amount ?")
                    });
        }

        private JsonResult ValidateOrderQuantityToAdd(int quantity)
        {

            if (quantity == 0)
            {
                return Json(new
                {
                    result = false,
                    message =
                    string.Format(
                        Translation.GetTerm("TheItemCouldNotBeAddedInvalidQuantityOf", "The product could not be added. Invalid quantity of {0}."), "0")
                });
            }

            //Validar montos negativos
            if (quantity < 0)
            {
                return Json(new
                {
                    result = false,
                    restricted = true,
                    message = Translation.GetTerm("InvalidProductQuantity", "Quantity Negative")
                });
            }

            return null;
        }

        private JsonResult ValidateProductIsRestricted(int productId, int quantity)
        {
            if (ProductQuotasRepository.ProductIsRestricted(productId, quantity, CurrentAccount.AccountID, CurrentAccount.AccountTypeID))
                return
                    Json(new
                    {
                        result = false,
                        restricted = true,
                        message =
                        Translation.GetTerm("ProductCantPurchased", "The added product can not be purchased")
                    });

            return null;
        }


        private JsonResult ValidateActiveProductPrice(int productId, int ppt)
        {
            #region Valida Precios Activos del Producto agregado
            int currendyid = OrderContext.Order.AsOrder().CurrencyID;
            if (ProductPricesExtensions.GetPriceByPriceType(productId, ppt, currendyid) <= 0)
                return
                    Json(
                        new
                        {
                            result = false,
                            restricted = true,
                            message = Translation.GetTerm("ProductNotActive", "The added product is not active")
                        });

            #endregion

            return null;
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

        private JsonResult validateShippingAddressType()
        {

            if (!CoreContext.CurrentAccount.Addresses.Any(a => a.AddressTypeID == (short)ConstantsGenerated.AddressType.Shipping))
            {
                return Json(new
                {
                    result = false,
                    message = Translation.GetTerm("PleaseAddaShippingAddressToThisAccount", "Please add a shipping address to this account.")
                });
            }
            return null;
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



        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Orders-Order Entry", "~/Orders")]
        public virtual ActionResult AddToCart(int productId, int quantity, int? IsRecuperable, string parentGuid = null,
            int? dynamicKitGroupId = null)
        {

            int ppt = OrderContext.Order.AsOrder().OrderCustomers[0].ProductPriceTypeID;
            int wareHouseID = Convert.ToInt32(Session[SessionConstants.WareHouseId]);
            int preOrder = Convert.ToInt32(Session[SessionConstants.PreOrder]);

            var validateQuantity = ValidateOrderQuantityToAdd(quantity);
            if (validateQuantity != null) { return validateQuantity; }

            // Valida Precios Activos del Producto agregado
            var validateActivePrice = ValidateActiveProductPrice(productId, ppt);
            if (validateActivePrice != null) return validateActivePrice;

            var isValidShippingAddressType = validateShippingAddressType();
            if (isValidShippingAddressType != null) return isValidShippingAddressType;

            OrderContext.Order.WareHouseID = wareHouseID;
            // Developed by BAL - FHP - A03
            /*CS.17MAY2016.Inicio*/
            #region ExistsProductInOrder & Product Quota

            var itemAdded = OrderContext.Order.AsOrder().OrderCustomers[0].OrderItems.Count > 0
                ? OrderContext.Order.AsOrder().OrderCustomers[0].OrderItems.FirstOrDefault(
                    x => x.ProductID.Equals(productId))
                : null;

            var hasChangedKit = HasChangedKit(productId, quantity);
            if (hasChangedKit != null) return hasChangedKit;

            var currentQuantity = (itemAdded != null && itemAdded.Quantity > 0) ? itemAdded.Quantity + quantity : quantity;

            var validateRetrictions = ValidateProductIsRestricted(productId, currentQuantity);
            if (validateRetrictions != null) return validateRetrictions;

            if (itemAdded != null)
            {
                Order.RemoveLineOrder(productId, wareHouseID, preOrder, CoreContext.CurrentAccount.AccountTypeID, false);
            }
            #endregion

            int SiteID = ConfigurationManager.GetAppSetting<int>(ConfigurationManager.VariableKey.NSCoreSiteID);
            //1.	Get Changed Order Items //2.	Current Shipping Methods //3.	Totals //4.	Reconciled Payments
            try
            {
                bool showOutOfStockMessage = true;
                string outOfStockMessage = "";
                var product = Inventory.GetProduct(productId);
                string messageValidKit = "";

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
                if (!parentGuid.IsNullOrEmpty() && dynamicKitGroupId.HasValue)
                {
                    bundleItem = customer.OrderItems.FirstOrDefault(oc => oc.Guid.ToString("N") == parentGuid);
                    bundleProduct = Inventory.GetProduct(bundleItem.ProductID.Value);
                    group =
                        bundleProduct.DynamicKits[0].DynamicKitGroups.Where(
                            g => g.DynamicKitGroupID == dynamicKitGroupId).FirstOrDefault();
                    int currentCount =
                        bundleItem.ChildOrderItems.Where(oi => oi.DynamicKitGroupID == dynamicKitGroupId)
                            .Sum(oi => oi.Quantity);
                    if ((currentCount + quantity) > group.MinimumProductCount)
                    {
                        return Json(new
                        {
                            result = false,
                            message = string.Format(
                                Translation.GetTerm("TheItemCouldNotBeAddedInvalidQuantityOf",
                                    "The product could not be added. Invalid quantity of {0}."), quantity)
                        });
                    }
                }


                var mod = Create.New<IOrderItemQuantityModification>();
                mod.ProductID = productId;
                mod.Quantity = quantity;
                mod.ModificationKind = OrderItemQuantityModificationKind.Add;
                mod.Customer = OrderContext.Order.OrderCustomers[0];

                RemovePaymentsTable(0);
                OrderContext.Order.AsOrder().OrderCustomers[0].RemoveAllPayments();

                if (bundleItem != null && bundleProduct != null && @group != null)
                {
                    OrderService.AddOrderItemsToOrderBundle(OrderContext, bundleItem, new IOrderItemQuantityModification[] { mod }, dynamicKitGroupId.Value);
                    int currentCount = bundleItem.ChildOrderItems.Where(i => i.DynamicKitGroupID == dynamicKitGroupId).Sum(i => i.Quantity);
                    isBundleGroupComplete = currentCount == @group.MinimumProductCount;
                }
                else
                {
                    OrderContext.Order.PreorderID = preOrder;
                    AddOrUpdateOrderItems(new[] { mod });
                    ConsolidateOrderItemsKits(productId);
                }

                ApplyPaymentPreviosBalance();
                OrderContext.Order.ParentOrderID = wareHouseID;//To sent to adjusment module to validate inventory

                OrderService.UpdateOrder(OrderContext);
                int productsId;

                var showFreeGiftModal = false;
                foreach (var adjustment in OrderContext.Order.OrderAdjustments)
                {
                    foreach (var orderStep in adjustment.InjectedOrderSteps)
                    {
                        showFreeGiftModal = StepHasAnItemInStockToBeChosen(orderStep.OrderStepReferenceID);
                        if (showFreeGiftModal)
                            break;
                    }
                    if (showFreeGiftModal)
                        break;
                }

                //This code was causing a null reference exception when isDynamicKit was true and the order item did not have child order items.
                //Unsure if bundleGuid should always have a value if isDynamicKit is true or if it should be an empty string if the item does not have child items.
                OrderItem bundleGuidItem = isDynamicKit
                    ? customer.OrderItems.FirstOrDefault(i => i.ProductID == product.ProductID && i.HasChildOrderItems)
                    : null;
                var bundleGuid = bundleGuidItem == null ? string.Empty : bundleGuidItem.Guid.ToString("N");

                CalculateTotal();


                //int productId, int quantity, string parentGuid = null, int? dynamicKitGroupId = null
                if (messageValidKit != "")
                {
                    outOfStockMessage = messageValidKit;
                }
                string DateEstimated = "";
                OrderShipment shipment = OrderContext.Order.AsOrder().GetDefaultShipment();


                var shippingMethods =
                    ShippingCalculator.GetShippingMethodsWithRates(customer, shipment).OrderBy(x => x.ShippingAmount);

                foreach (var item in OrderContext.Order.AsOrder().OrderShipments)
                {
                    foreach (var sp in shippingMethods.Where(x => x.ShippingMethodID == item.ShippingMethodID))
                    {
                        Session["DateEstimated"] = sp.DateEstimated;
                    }
                }
                   
                return Json(new
                {
                    orderItems = GetOrderItemsHtml(OrderContext.Order.AsOrder()),
                    result = true,
                    OrderEntryModelData = GetOrderEntryModelData(OrderContext.Order.AsOrder()),
                    promotions = GetApplicablePromotions(OrderContext.Order.AsOrder()),
                    outOfStockItems = GetOutOfStockItemsString(OrderContext.Order.AsOrder()),
                    shippingMethods = ShippingMethods,
                    allow = true,
                    showOutOfStockMessage,
                    isBundle = isDynamicKit,
                    bundleGuid = bundleGuid,
                    productId = product.ProductID,
                    groupItemsHtml =
                    parentGuid == null ? "" : GetGroupItemsHtml(parentGuid, dynamicKitGroupId.Value).ToString(),
                    isBundleGroupComplete = isBundleGroupComplete,
                    childItemCount =
                    parentGuid == null
                        ? 0
                        : customer.OrderItems.GetByGuid(parentGuid).ChildOrderItems.Sum(c => c.Quantity),
                    orderCustomerId = customer.OrderCustomerID,
                    BundleOptionsSpanHTML = GetDynamicBundleUpSale(),
                    AvailableBundleCount = _availableBundleCount,
                    message = outOfStockMessage,
                    hasFreeGiftSteps = OrderContext.Order.OrderAdjustments.Any(oa => oa.InjectedOrderSteps.Any()),
                    resultQualificationTotal =
                    Totals.GetType().GetProperty("subTotalPreadjustedItemPrice").GetValue(Totals, null),
                    resultCommisionableTotal =
                    Totals.GetType().GetProperty("subTotalPreadjustedItemPrice").GetValue(Totals, null),
                    showFreeGiftModal,
                    paymentsGrid = RenderRazorPartialViewToString("PaymentsGrid", OrderContext.Order.AsOrder()),
                    dateEstimated = Convert.ToString(Session["DateEstimated"]),
                    totals = Totals
                });
            }
            catch (ProductShippingExcludedShippingException ex)
            {
                return
                    Json(
                        new
                        {
                            result = false,
                            shippingMessageEx = true,
                            excludedProducts = ex.ProductsThatHaveExcludedShipping.Select(m => m.Name)
                        });
            }
            catch (Exception ex)
            {
                NetStepsException exception;
                if (!(ex is NetStepsBusinessException))
                    exception = EntityExceptionHelper.GetAndLogNetStepsException(ex,
                        Constants.NetStepsExceptionType.NetStepsApplicationException,
                        OrderContext.Order != null ? OrderContext.Order.OrderID.ToIntNullable() : null,
                        accountID:
                        CoreContext.CurrentAccount != null ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
                else
                    exception = ex as NetStepsException;
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }



        private void ApplyConfigurationCumulative(IOrderContext orderContext)
        {
            int promotionID = 0;
            bool cumulative = false;
            foreach (var mod in orderContext.Order.AsOrder().OrderCustomers
                .SelectMany(oi => oi.OrderItems
                    .SelectMany(oaolm => oaolm.OrderAdjustmentOrderLineModifications)))
            {
                if (mod.OrderAdjustment.Extension is PromotionOrderAdjustment)
                {
                    promotionID = ((PromotionOrderAdjustment)mod.OrderAdjustment.Extension).PromotionID;
                    var promotionQualification =
                        PromotionQualificationCustomerPriceTypeTotalRangeCurrencyAmountBusinessLogic.Instance
                            .GetByPromotionID(promotionID);
                    cumulative = promotionQualification != null ? promotionQualification.Cumulative ?? false : false;
                }
            }

            if (cumulative)
            {
                Order order = OrderContext.Order.AsOrder();
                int accountId = order.OrderCustomers[0].AccountID;
                decimal total =
                    order.OrderCustomers.Select(m => m.OrderItems.Select(oi => oi.ItemPrice * oi.Quantity).Sum()).Sum();

                PromoPromotionLogic.Instance.ApplyConfigurationCumulative(accountId, total, promotionID);
            }

            if (PromoPromotionTypeConfigurationsLogic.Instance.GetActiveID() ==
                (int)ConstantsGenerated.PromotionEngineType.DiscountType4)
            {
                if (promotionID > 0)
                    PromotionConfigurationControlBusinessLogic.Instance.UpdatePromotion(4, promotionID);
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Orders-Order Entry", "~/Orders")]
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
                    #region Validating Quantity

                    if (objProduct.Quantity <= 0) return Json(new { result = false, restricted = true, message = Translation.GetTerm("InvalidProductQuantity", "Quantity Negative") });

                    #endregion

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
                        Convert.ToInt32(Session[SessionConstants.WareHouseId]),
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
                OrderContext.Order.AsOrder().OrderCustomers[0].RemoveAllPayments();
                AddOrUpdateOrderItems(validProducts.Select(item =>
                {
                    var mod = Create.New<IOrderItemQuantityModification>();
                    mod.ProductID = item.ProductID;
                    mod.Quantity = item.Quantity;
                    mod.ModificationKind = OrderItemQuantityModificationKind.Add;
                    mod.Customer = OrderContext.Order.OrderCustomers[0];

                    return mod;
                }));

                OrderService.UpdateOrder(OrderContext);

                #endregion

                #region Return

                if (msgError != "")
                {
                    return Json(new
                    {
                        result = false,
                        totals = Totals,
                        OrderEntrymodelData = GetOrderEntryModelData(OrderContext.Order.AsOrder()),
                        orderItems = GetOrderItemsHtml(OrderContext.Order.AsOrder()),
                        shippingMethods = ShippingMethods,
                        outOfStockItems = GetOutOfStockItemsString(OrderContext.Order.AsOrder()),
                        promotions = GetApplicablePromotions(OrderContext.Order.AsOrder()),
                        message = msgError
                    });
                }
                else
                {
                    return Json(new
                    {
                        result = true,
                        totals = Totals,
                        OrderEntrymodelData = GetOrderEntryModelData(OrderContext.Order.AsOrder()),
                        orderItems = GetOrderItemsHtml(OrderContext.Order.AsOrder()),
                        shippingMethods = ShippingMethods,
                        outOfStockItems = GetOutOfStockItemsString(OrderContext.Order.AsOrder()),
                        promotions = GetApplicablePromotions(OrderContext.Order.AsOrder()),
                        message = msgError
                    });
                }

                #endregion

            }
            catch (Exception ex)
            {
                NetStepsException exception;
                if (!(ex is NetStepsBusinessException))
                    exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, OrderContext.Order != null ? OrderContext.Order.OrderID.ToIntNullable() : null, accountID: CoreContext.CurrentAccount != null ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
                else
                    exception = ex as NetStepsException;
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Orders-Order Entry", "~/Orders")]
        public virtual ActionResult AddToDynamicKitGroup(string parentOrderItemGuid, int dynamicKitGroupId, int productId, int quantity)
        {
            //1.	Get Changed Order Items
            //2.	Current Shipping Methods
            //3.	Totals
            //4.	Reconciled Payments

            /*  
             * 1. Update quantities
             * 2. Clear order adjustments
             * 3. Consolidate Order Items
             * 4. Apply Promotions
             * 5. Apply Bundling
             * 6. Calculate Totals
             * 7. Return data
             */

            var originalOrder = OrderContext.Order.AsOrder().Clone();
            try
            {
                OrderContext.Order.AsOrder().OrderCustomers[0].RemoveAllPayments();
                OrderContext.Order.AsOrder().AddOrUpdateOrderItem((OrderCustomer)OrderContext.Order.OrderCustomers[0], new List<OrderItemUpdateInfo>() { new OrderItemUpdateInfo() { ProductID = productId, Quantity = quantity } }, false, parentOrderItemGuid, dynamicKitGroupId);
                OrderService.UpdateOrder(OrderContext);

                var orderItems = GetOrderItemsHtml(OrderContext.Order.AsOrder());

                return Json(new
                {
                    result = true,
                    orderItems = orderItems,
                    promotions = GetApplicablePromotions(OrderContext.Order.AsOrder()),
                    totals = Totals,
                    shippingMethods = ShippingMethods,
                });
            }
            catch (Exception ex)
            {
                OrderContext.Order = originalOrder; // Revert to original party object (before these modifications) to avoid a possibly corrupted object graph - JHE
                NetStepsException exception;
                if (!(ex is NetStepsBusinessException))
                    exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, OrderContext.Order != null ? OrderContext.Order.OrderID.ToIntNullable() : null, accountID: CoreContext.CurrentAccount != null ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
                else
                    exception = ex as NetStepsException;
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Orders-Order Entry", "~/Orders")]
        public virtual ActionResult UpdateCart(List<ProductQuantityContainer> products)
        {
            //1.	Get Changed Order Items
            //2.	Current Shipping Methods
            //3.	Totals
            //4.	Reconciled Payments
            foreach (var item in products)
            {
                if (item.Quantity <= 0)
                {
                    return
                        Json(
                            new
                            {
                                result = false,
                                restricted = true,
                                message = Translation.GetTerm("InvalidProductQuantity", "Quantity Negative")
                            });
                }
            }


            if (products == null)
            {
                return JsonError(_errorNoItemsInOrder);
            }
            int SiteID = ConfigurationManager.GetAppSetting<int>(ConfigurationManager.VariableKey.NSCoreSiteID);
            foreach (var item in products)
            {
                if (ProductQuotasRepository.ProductIsRestricted(item.ProductID, item.Quantity, CurrentAccount.AccountID, CurrentAccount.AccountTypeID))
                    return Json(new { result = false, restricted = true, message = Translation.GetTerm("ProductCantPurchased", "The added product can not be purchased") });
            }

            try
            {
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

                foreach (var noKitOrderItem in queryOrderITems.Where(i => ((OrderItem)i).ChildOrderItems.Count == 0))
                {
                    var queryProducts = products.FirstOrDefault(o => o.ProductID == noKitOrderItem.ProductID.Value);
                    var queryOrderItemData = queryOrderITems.FirstOrDefault(l => noKitOrderItem.ProductID != null && l.ProductID == noKitOrderItem.ProductID.Value);
                    if (queryOrderItemData != null && (queryProducts != null))
                        queryProducts.HasQuantityChange = (queryProducts.Quantity != queryOrderItemData.Quantity);
                }

                var changes = products.Where(n => n.HasQuantityChange)
                    .Select(item =>
                    {
                        if (item.HasQuantityChange) { }
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
                    }).ToList();
                string messageValidKit = "";


                var productThatChanged = new List<ProductQuantityContainer>();// products.Where(i => changes.Contains(ProductID == i.ProductID));
                //Optener los order item antes de actualizar el producto
                List<OrderItemUpdate> objOrderItemsOld = new List<OrderItemUpdate>();
                foreach (var p in products.Where(p => p.HasQuantityChange))
                {
                    foreach (var item in Order.GenerateAllocation(p.ProductID,
                                                                  p.Quantity,
                        OrderContext.Order.AsOrder().OrderID,
                        Convert.ToInt32(Session[SessionConstants.WareHouseId]),
                        EntitiesEnums.MaintenanceMode.Update,
                        Convert.ToInt32(Session[SessionConstants.PreOrder]),
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
                    objOrderItemsOld.Add(new OrderItemUpdate()
                    {
                        Product = productRemove.ProductID.Value,
                        Quantity = productRemove.Quantity,
                        status = false
                    });
                }


                RemovePaymentsTable(0);
                OrderContext.Order.AsOrder().OrderCustomers[0].RemoveAllPayments();

                OrderService.UpdateOrderItemQuantities(OrderContext, changes);
                ApplyPaymentPreviosBalance();

                OrderContext.Order.ParentOrderID = Convert.ToInt32(Session["WareHouseId"]);
                OrderService.UpdateOrder(OrderContext);

                //Optener los order item despues de actualizar el producto
                List<ConfigKit> objOrderItemsNew = new List<ConfigKit>();

                foreach (var productRemove in OrderContext.Order.AsOrder().OrderCustomers[0].OrderItems)
                {
                    objOrderItemsNew.Add(new ConfigKit()
                    {
                        MaterialID = productRemove.ProductID.Value,
                        Available = productRemove.Quantity
                    });
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
                    if (objOrderItemsOld.Where(X => X.Product == item.MaterialID).Count() == 0)
                    {
                        //Insertar el nuevo items
                        Order.GenerateAllocation(item.MaterialID,
                                                              item.Available,
                                                              OrderContext.Order.AsOrder().OrderID,
                            Convert.ToInt32(Session[SessionConstants.WareHouseId]),
                            EntitiesEnums.MaintenanceMode.Add,
                            Convert.ToInt32(Session[SessionConstants.PreOrder]),
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
                            Convert.ToInt32(Session[SessionConstants.WareHouseId]),
                            EntitiesEnums.MaintenanceMode.Delete,
                            Convert.ToInt32(Session[SessionConstants.PreOrder]),
                                                 CoreContext.CurrentAccount.AccountTypeID,
                                                 false);
                    }
                }

                CalculateTotal();
                string outOfStockMessage = "";
                if (messageValidKit != "")
                {
                    outOfStockMessage = messageValidKit;
                }
                return Json(new
                {
                    paymentsGrid = RenderRazorPartialViewToString("PaymentsGrid", OrderContext.Order.AsOrder()),
                    result = true,
                    orderItems = GetOrderItemsHtml(OrderContext.Order.AsOrder()),
                    OrderEntryModelData = GetOrderEntryModelData(OrderContext.Order.AsOrder()),
                    promotions = GetApplicablePromotions(OrderContext.Order.AsOrder()),
                    shippingMethods = ShippingMethods,
                    BundleOptionsSpanHTML = GetDynamicBundleUpSale(),
                    AvailableBundleCount = _availableBundleCount,
                    //resultCreditAvailable = resultCreditAvailable.ToString('S'),
                    resultQualificationTotal =
                    Totals.GetType().GetProperty("subTotalPreadjustedItemPrice").GetValue(Totals, null),
                    resultCommisionableTotal =
                    Totals.GetType().GetProperty("subTotalPreadjustedItemPrice").GetValue(Totals, null),
                    outOfStockItems = GetOutOfStockItemsString(OrderContext.Order.AsOrder()),
                    message = outOfStockMessage,
                    totals = Totals
                });
            }
            catch (Exception ex)
            {
                var message =
                    ex.Log(orderID: OrderContext.Order.AsOrder().OrderID,
                        accountID: CoreContext.CurrentAccount.AccountID).PublicMessage;
                return JsonError(message);
            }
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


        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Orders-Order Entry", "~/Orders")]
        public virtual ActionResult RemoveFromCart(string orderItemId, string parentGuid = null, int? quantity = null)
        {
            //1.	Get Changed Order Items
            //2.	Current Shipping Methods
            //3.	Totals
            //4.	Reconciled Payments
            try
            {
                var customer = (OrderCustomer)OrderContext.Order.OrderCustomers.FirstOrDefault();
                var item = customer.OrderItems.FirstOrDefault(oi => (oi as OrderItem).Guid.ToString("N") == orderItemId);
                int SiteID = ConfigurationManager.GetAppSetting<int>(ConfigurationManager.VariableKey.NSCoreSiteID);
                var dynamicKitGroupId = (item as OrderItem).DynamicKitGroupID;
                int Valuequantity = 0;
                List<OrderItemUpdate> objOrderItemsOld = new List<OrderItemUpdate>();

                foreach (var productRemove in OrderContext.Order.AsOrder().OrderCustomers[0].OrderItems)
                {
                    objOrderItemsOld.Add(new OrderItemUpdate()
                    {
                        Product = productRemove.ProductID.Value,
                        Quantity = productRemove.Quantity,
                        status = false
                    });
                }

                Valuequantity = item.Quantity;
                var removeModification = Create.New<IOrderItemQuantityModification>();
                removeModification.Customer = customer;
                removeModification.ModificationKind = OrderItemQuantityModificationKind.Delete;
                removeModification.ProductID = item.ProductID.Value;
                removeModification.Quantity = 0;
                removeModification.ExistingItem = item;

                RemovePaymentsTable(0);
                OrderContext.Order.AsOrder().OrderCustomers[0].RemoveAllPayments();
                ApplyPaymentPreviosBalance();
                var changes = new IOrderItemQuantityModification[] { removeModification };

                int ppt = OrderContext.Order.AsOrder().OrderCustomers[0].ProductPriceTypeID;

                OrderService.UpdateOrderItemQuantities(OrderContext, changes);
                OrderContext.Order.ParentOrderID = Convert.ToInt32(Session[SessionConstants.WareHouseId]);
                OrderService.UpdateOrder(OrderContext);

                OrderItem bundleItem = null;

                if (!parentGuid.IsNullOrEmpty() && dynamicKitGroupId.HasValue)
                {
                    bundleItem = customer.OrderItems.FirstOrDefault(oc => oc.Guid.ToString("N") == parentGuid);
                }

                bool isBundleGroupComplete = false;
                if (bundleItem != null)
                {
                    var bundleProduct = Inventory.GetProduct(bundleItem.ProductID.Value);
                    var group =
                        bundleProduct.DynamicKits[0].DynamicKitGroups.Where(
                            g => g.DynamicKitGroupID == dynamicKitGroupId).FirstOrDefault();
                    int currentCount =
                        bundleItem.ChildOrderItems.Where(oi => oi.DynamicKitGroupID == dynamicKitGroupId)
                            .Sum(oi => oi.Quantity);
                    isBundleGroupComplete = currentCount == group.MinimumProductCount;
                }
                CalculateTotal();

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
                    objOrderItemsNew.Add(new ConfigKit()
                    {
                        MaterialID = productRemove.ProductID.Value,
                        Available = productRemove.Quantity
                    });
                }

                foreach (var items in objOrderItemsNew)
                {
                    if (objOrderItemsOld.Where(X => X.Product == items.MaterialID).Count() == 0)
                    {
                        //Insertar el nuevo items
                        Order.GenerateAllocation(items.MaterialID,
                                                              items.Available,
                                                              OrderContext.Order.AsOrder().OrderID,
                            Convert.ToInt32(Session[SessionConstants.WareHouseId]),
                            EntitiesEnums.MaintenanceMode.Add,
                            Convert.ToInt32(Session[SessionConstants.PreOrder]),
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
                            Convert.ToInt32(Session[SessionConstants.WareHouseId]),
                            EntitiesEnums.MaintenanceMode.Delete,
                            Convert.ToInt32(Session[SessionConstants.PreOrder]),
                                                 CoreContext.CurrentAccount.AccountTypeID,
                                                 false);
                    }
                }
                return Json(new
                {
                    paymentsGrid = RenderRazorPartialViewToString("PaymentsGrid", OrderContext.Order.AsOrder()),
                    result = true,
                    OrderEntryModelData = GetOrderEntryModelData(OrderContext.Order.AsOrder()),
                    orderItems = GetOrderItemsHtml(OrderContext.Order.AsOrder()),
                    promotions = GetApplicablePromotions(OrderContext.Order.AsOrder()),
                    outOfStockItems = GetOutOfStockItemsString(OrderContext.Order.AsOrder()),
                    shippingMethods = ShippingMethods,
                    groupItemsHtml =
                    parentGuid == null ? "" : GetGroupItemsHtml(parentGuid, dynamicKitGroupId.Value).ToString(),
                    isBundleGroupComplete = isBundleGroupComplete,
                    childItemCount =
                    parentGuid == null
                        ? 0
                        : customer.OrderItems.GetByGuid(parentGuid).ChildOrderItems.Sum(c => c.Quantity),
                    orderCustomerId = customer.OrderCustomerID,
                    BundleOptionsSpanHTML = GetDynamicBundleUpSale(),
                    //resultCreditAvailable = resultCreditAvailable.ToString('S'),
                    resultQualificationTotal =
                    Totals.GetType().GetProperty("subTotalPreadjustedItemPrice").GetValue(Totals, null),
                    resultCommisionableTotal =
                    Totals.GetType().GetProperty("subTotalPreadjustedItemPrice").GetValue(Totals, null),
                    AvailableBundleCount = _availableBundleCount,
                    totals = Totals
                });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex,
                    Constants.NetStepsExceptionType.NetStepsApplicationException,
                    OrderContext.Order != null ? OrderContext.Order.OrderID.ToIntNullable() : null,
                    accountID:
                    CoreContext.CurrentAccount != null ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Orders-Order Entry", "~/Orders")]
        public virtual ActionResult RemoveFromBundle(string orderItemGuid, string parentGuid, int? quantity = null)
        {
            //1.	Get Changed Order Items
            //2.	Current Shipping Methods
            //3.	Totals
            //4.	Reconciled Payments

            var originalOrder = OrderContext.Order.AsOrder().Clone();
            try
            {
                OrderContext.Order.AsOrder().StartEntityTracking();
                var customer = OrderContext.Order.AsOrder().OrderCustomers.FirstOrDefault();
                var item = customer.OrderItems.FirstOrDefault(oi => oi.Guid.ToString("N") == orderItemGuid);
                var parentItem = customer.OrderItems.FirstOrDefault(oi => oi.Guid.ToString("N") == parentGuid);
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

                OrderContext.Order.AsOrder().OrderCustomers[0].RemoveAllPayments();
                OrderService.UpdateOrder(OrderContext);

                var orderItems = GetOrderItemsHtml(OrderContext.Order.AsOrder());

                return Json(new
                {
                    result = true,
                    orderItems = orderItems,
                    promotions = GetApplicablePromotions(OrderContext.Order.AsOrder()),
                    totals = Totals
                });
            }
            catch (Exception ex)
            {
                OrderContext.Order = originalOrder; // Revert to original party object (before these modifications) to avoid a possibly corrupted object graph - JHE
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, OrderContext.Order != null ? OrderContext.Order.OrderID.ToIntNullable() : null, accountID: CoreContext.CurrentAccount != null ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [NonAction]
        public virtual string GetDynamicBundleUpSale()
        {
            if (OrderContext.Order == null)
            {
                return string.Empty;
            }

            var customer = OrderContext.Order.AsOrder().OrderCustomers[0];
            if (customer.IsTooBigForBundling())
            {
                return string.Empty;
            }

            var possibleDynamicKitProducts = OrderContext.Order.AsOrder().GetPotentialDynamicKitUpSaleProducts(customer, OrderContext.SortedDynamicKitProducts).ToList();
            this._availableBundleCount = possibleDynamicKitProducts.Count();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < possibleDynamicKitProducts.Count(); i++)
            {
                var product = possibleDynamicKitProducts[i];
                sb.Append("<input type=\"hidden\" class=\"dynamicKitProductSuggestion\" value=\"" + product.ProductID + "\" /><a href=\"javascript:void(0);\" class=\"CreateBundle\">" + product.Translations.Name() + "</a>");
                if (i != possibleDynamicKitProducts.Count() - 1)
                {
                    sb.Append(", ");
                }
            }
            return sb.ToString();
        }

        public ActionResult AddBundle(string bundleGuid)
        {
            var originalOrder = OrderContext.Order.AsOrder().Clone();
            try
            {
                if (OrderContext.Order == null)
                {
                    return Json(new
                    {
                        result = false,
                        message = Translation.GetTerm("OrderDoesNotExist", "The order does not exist.")
                    });
                }

                OrderItem orderItem = OrderContext.Order.AsOrder().OrderCustomers[0].OrderItems.GetByGuid(bundleGuid);
                if (orderItem.ParentOrderItemID == null)
                {
                    var kitProduct = Inventory.GetProduct(orderItem.ProductID.Value);
                    OrderContext.Order.AsOrder().OrderCustomers[0].RemoveAllPayments();

                    if (!Order.IsDynamicKitValid(orderItem))
                        return
                            Json(
                                new
                                {
                                    result = false,
                                    message =
                                    Translation.GetTerm("TheBundleYouTriedToOrderIsNotComplete",
                                        "The bundle you tried to order ({0}) is not complete.",
                                        kitProduct.Translations.Name())
                                });
                }

                OrderService.UpdateOrder(OrderContext);

                List<object> orderItems = GetOrderItemsHtml(OrderContext.Order.AsOrder());

                return Json(new
                {
                    result = true,
                    orderItems = orderItems,
                    shippingMethods = ShippingMethods,
                    totals = Totals,
                    promotions = GetApplicablePromotions(OrderContext.Order.AsOrder()),
                    BundleOptionsSpanHTML = GetDynamicBundleUpSale()
                });
            }
            catch (Exception ex)
            {
                OrderContext.Order = originalOrder; // Revert to original party object (before these modifications) to avoid a possibly corrupted object graph - JHE
                NetStepsException exception;
                if (!(ex is NetStepsBusinessException))
                    exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                else
                    exception = ex as NetStepsException;
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult GetDynamicKitContents(string orderItemGuid)
        {
            try
            {
                var customer = OrderContext.Order.AsOrder().OrderCustomers.FirstOrDefault();
                var kitItem = customer.OrderItems.GetByGuid(orderItemGuid);
                if (kitItem == null)
                    return Json(new
                    {
                        result = false,
                        message = Translation.GetTerm("ItemDoesNotExist", "That item does not exist.")
                    });
                var product = Inventory.GetProduct(kitItem.ProductID.Value);
                if (!product.IsDynamicKit())
                    return Json(new
                    {
                        result = false,
                        message = Translation.GetTerm("ItemIsNotABundle", "That item is not a bundle.")
                    });
                StringBuilder sb = new StringBuilder();
                var dynamicKit = product.DynamicKits[0];
                sb.Append("<h2>" + kitItem.SKU + " - " + product.Translations.Name() + "</h2>");
                foreach (var group in dynamicKit.DynamicKitGroups)
                {
                    var childItemsInGroup = kitItem.ChildOrderItems.Where(oi => oi.DynamicKitGroupID == group.DynamicKitGroupID);
                    var childItemsInGroupTotalCount = childItemsInGroup.Sum(ci => ci.Quantity);
                    string groupDescriptionSpan = string.Empty;
                    if (!group.Translations.ShortDescription().IsNullOrEmpty())
                    {
                        groupDescriptionSpan = string.Format("<span class=\"{0}\">{1}</span>", "groupDescription", "-- " + group.Translations.ShortDescription().SubstringSafe(0, 50));
                    }
                    sb.AppendFormat("<input type=\"hidden\" class=\"parentOrderItemGuid\" value=\"{0}\" />", kitItem.Guid.ToString("N"))
                    .Append("<div id=\"groupId" + group.DynamicKitGroupID.ToString() + "\" class=\"bundleSection overflow\">")
                        .AppendFormat("<h3 class=\"mt10\">{0}&nbsp;<span class=\"bold\">{1}/{2}</span>{3}</h3>", group.Translations.Name(), childItemsInGroupTotalCount, group.MinimumProductCount, groupDescriptionSpan);

                    if (!Order.IsDynamicKitGroupValid(kitItem, group))
                    {
                        sb.Append("<div class=\"errorMessageBubble\">" + Translation.GetTerm("GroupIncomplete", "Group is incomplete") + "</div>");
                    }
                    else
                    {
                        sb.Append("<div class=\"successMessageBubble\">" + Translation.GetTerm("Success", "Success") + "</div>");
                    }

                    sb.Append("<div class=\"FL mr10\">")
                            .Append("<span class=\"block\">" + Translation.GetTerm("SKUOrName", "SKU or product name") + "</span>")
                            .Append("<input type=\"text\" class=\"txtDynamicKitQuickAddSearch\" style=\"width: 240px;\" />")
                    .Append("</div>")
                    .Append("<div class=\"FL\">")
                            .Append("<span class=\"block\">" + Translation.GetTerm("Qty", "Qty") + "</span>")
                            .Append("<input type=\"text\" class=\"Short quantity required\" style=\"width: 50px\" value=\"1\" />")
                            .Append("<input type=\"hidden\" class=\"productId\" value=\"\" />")
                            .Append("<input type=\"hidden\" class=\"needsBackOrderConfirmation\" value=\"\" />")
                    .Append("</div>")
                    .Append("<a href=\"javascript:void(0);\" class=\"DTL Update FR\"><span>" + Translation.GetTerm("Add/Update", "Add/Update") + "</span></a>")
                    .Append("<span class=\"ClearAll mb10\"></span>");

                    foreach (var childItem in childItemsInGroup)
                    {
                        var childProduct = Inventory.GetProduct(childItem.ProductID.Value);
                        sb.Append("<p class=\"addedBundleItem\">")
                                .Append("<a id=\"childGuid" + childItem.Guid.ToString("N") + "\" title=\"Remove\" href=\"javascript:void(0);\" class=\"FL mr10 removeFromBundle\"><img alt=\"Remove\" src=\"/Content/Images/Icons/remove-trans.png\"></a>")
                                .Append("<span class=\"FL bold\">" + childProduct.Translations.Name() + "</span>")
                                .Append("<span class=\"FR\">" + Translation.GetTerm("Qty", "Qty") + ": " + childItem.Quantity.ToString() + "</span>")
                        .Append("</p>");
                    }

                    sb.Append("</div>");
                }

                return Json(new { result = true, bundleContentsHTML = sb.ToString(), kitProductId = product.ProductID });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        #endregion

        #region Addresses

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Orders-Order Entry", "~/Orders")]
        public virtual ActionResult ChangeShippingAddress(int shippingAddressId)
        {
            try
            {
                int ItemCount = OrderContext.Order.AsOrder().OrderCustomers[0].OrderItems.Count();
                RemovePaymentsTable(0);
                string valueTotal = Totals.GetType().GetProperty("subTotalPreadjustedItemPrice").GetValue(Totals, null).ToString();
                bool isPopup = false;
                int WareHouseIdSession = Convert.ToInt32(Session[SessionConstants.WareHouseId]);
                int newWareHouseIdSession = Order.WareHouseByAccountAddress(Convert.ToInt32(Session["AccountID"]),
                    shippingAddressId);
                if (valueTotal != "$0.00")
                {
                    //if (Convert.ToInt32(Session["WareHouseId"]) != Order.WareHouseByAccountAddress(Convert.ToInt32(Session["AccountID"]), shippingAddressId))
                    if (WareHouseIdSession != newWareHouseIdSession && ItemCount > 0) isPopup = true;
                }
                if (ShippingMethods != "")
                {
                    if (WareHouseIdSession == newWareHouseIdSession)
                    {
                        UpdateOrderShipmentAddress(shippingAddressId);
                        ApplyPaymentPreviosBalance();
                        if (NetSteps.Data.Entities.Business.CTE.paymentTables.Count > 0)
                            OrderService.UpdateOrder(OrderContext);

                        Session["WareHouseId"] = newWareHouseIdSession;
                        return
                            Json(
                                new
                                {
                                    result = true,
                                    shippingMethods = ShippingMethods,
                                    totals = Totals,
                                    orderItems = GetOrderItemsHtml(OrderContext.Order.AsOrder()),
                                    isPopup = isPopup,
                                    paymentsGrid =
                                    RenderRazorPartialViewToString("PaymentsGrid", OrderContext.Order.AsOrder())
                                });
                        //return Json(new { result = true, isPopup = isPopup, shippingMethods = ShippingMethods });
                    }
                    else
                    {
                        var shippingAddresID = OrderContext.Order.AsOrder().OrderCustomers[0].OrderShipments;
                        int SourceAddressID = 0;
                        foreach (var item in shippingAddresID) SourceAddressID = item.SourceAddressID.Value;

                        ApplyPaymentPreviosBalance();
                        if (NetSteps.Data.Entities.Business.CTE.paymentTables.Count > 0)
                            OrderService.UpdateOrder(OrderContext);

                        return Json(new { result = false, isPopup = isPopup, shippingMethods = ShippingMethods, sourceAddressID = SourceAddressID, paymentsGrid = RenderRazorPartialViewToString("PaymentsGrid", OrderContext.Order.AsOrder()) });
                    }
                    int wareHouseXAddres = Order.WareHouseByAccountAddress(Convert.ToInt32(Session["AccountID"]), shippingAddressId);
                    if (wareHouseXAddres != 0)
                    {
                        Session["WareHouseId"] = wareHouseXAddres;
                    }

                    UpdateOrderShipmentAddress(shippingAddressId);

                    ApplyPaymentPreviosBalance();
                    if (NetSteps.Data.Entities.Business.CTE.paymentTables.Count > 0)
                        OrderService.UpdateOrder(OrderContext);

                    return Json(new { result = true, shippingMethods = ShippingMethods, totals = Totals, orderItems = GetOrderItemsHtml(OrderContext.Order.AsOrder()), isPopup = isPopup, paymentsGrid = RenderRazorPartialViewToString("PaymentsGrid", OrderContext.Order.AsOrder()) });
                }
                else
                {
                    UpdateOrderShipmentAddress(shippingAddressId);
                    Session["WareHouseId"] = newWareHouseIdSession;

                    ApplyPaymentPreviosBalance();
                    if (NetSteps.Data.Entities.Business.CTE.paymentTables.Count > 0)
                        OrderService.UpdateOrder(OrderContext);

                    return Json(new { result = false, isPopup = isPopup, paymentsGrid = RenderRazorPartialViewToString("PaymentsGrid", OrderContext.Order.AsOrder()) });
                }


            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, orderID: OrderContext.Order != null ? OrderContext.Order.OrderID.ToIntNullable() : null, accountID: CoreContext.CurrentAccount != null ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        #endregion

        #region Payments

        [AcceptVerbs(HttpVerbs.Post)]
        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Orders-Order Entry", "~/Orders")]
        public virtual ActionResult ApplyPayment(PaymentTypeModel paymentTypeModel)
        {
            try
            {
                Order order = OrderContext.Order.AsOrder();
                var numberOfItems = (order.OrderCustomers.Sum(oc => oc.ParentOrderItems.Sum(oi => oi.Quantity)));
                if (numberOfItems == 0)
                    return
                        JsonError(Translation.GetTerm("Payment_ValidCantProduct", "Debe de tener al menos un producto"));


                var IsCredit = ValidateAccountCredit(paymentTypeModel.PaymentMethodID.ToInt());

                if (IsCredit > 0)
                {
                    if (IsCredit == 1)
                        return JsonError(Translation.GetTerm("Payment_ValidCanNotMethod", "No es posible utilizar el crédito, seleccione otro medio de pago"));

                    if (IsCredit == 2)
                        return JsonError(Translation.GetTerm("Payment_ValidOrderExceeds", "La compra excede su crédito disponible. Modifique la orden o seleccione otro medio de pago"));
                }

                var validate = validatePaymenType(int.Parse(paymentTypeModel.PaymentMethodID.ToString()));
                if (validate)
                {
                    return JsonError(Translation.GetTerm("Payment_ValidDifferent", "should not enter a different payment method"));
                }

                var KeyDecimals = ConfigurationManager.AppSettings["CultureDecimal"];
                if (KeyDecimals == "ES")
                {
                    var culture = CultureInfoCache.GetCultureInfo("En");
                    var amount = Convert.ToDecimal(paymentTypeModel.AmountConfiguration, culture);
                    paymentTypeModel.Amount = amount;
                }

                IPayment payment = new Payment()
                {
                    DecryptedAccountNumber = paymentTypeModel.AccountNumber,
                    CVV = paymentTypeModel.Cvv,
                    PaymentType = ConstantsGenerated.PaymentType.EFT,
                    NameOnCard = paymentTypeModel.AccountNumber,
                    BankName = paymentTypeModel.NameOnCard
                };
                //============================================================================================
                // Para Brasil('N') el flujo sigue su curso normal , mientras que para USA('S')  tiene que ser siempre el PaymentTypeID = 1;
                //if (Session["GeneralParameterVal"].ToString() == "N")
                //    payment.PaymentTypeID = Order.GetApplyPaymentType(paymentTypeModel.PaymentMethodID.Value);
                //else
                //    payment.PaymentTypeID = 1;
                //payment = Account.LoadPaymentMethodAndVerifyAccount(paymentTypeModel.PaymentMethodID.Value, OrderContext.Order.AsOrder().OrderCustomers[0].AccountID);
                payment.PaymentTypeID = Order.GetApplyPaymentType(paymentTypeModel.PaymentMethodID.Value);

                BasicResponseItem<OrderPayment> response =
                    OrderContext.Order.AsOrder()
                        .ApplyPaymentToCustomers(paymentTypeModel.PaymentType, paymentTypeModel.Amount, payment,
                            user: CoreContext.CurrentUser);

                //validationResponse = ValidatePayment(amount, order, PaymentTypeID, customer, user);
                if (!response.Success)
                {
                    return Json(new { result = false, message = response.Message });
                }
                ApplyPaymentSearchData objE = new ApplyPaymentSearchData();
                objE.OrderPaymentId = OrderContext.Order.AsOrder().OrderPayments.Count() * (-1);// response.Item.OrderPaymentID;
                objE.Amount = paymentTypeModel.Amount;
                objE.PaymentConfigurationID = paymentTypeModel.PaymentMethodID;
                objE.NameOnCard = "";
                objE.PaymentConfigurationID = paymentTypeModel.PaymentMethodID;

                if (payment.PaymentTypeID > 1)
                {
                    objE.NumberCuota = null;
                }
                else
                {
                    var numberTarget = NetSteps.Data.Entities.Business.PaymentMethods.GetNumberCuotasByPaymentConfigurationID(paymentTypeModel.PaymentMethodID.ToInt());
                    objE.NumberCuota = numberTarget;// paymentTypeModel.NumberCuota.Value;
                }
                objE.PreOrderID = Convert.ToInt32(Session["PreOrder"]);
                Session["FinalPaymentStatusID"] = NetSteps.Data.Entities.Business.CTE.ApplyPayment(objE);

                int OrderStatusId = Order.GetApplyPayment(objE);

                OrderService.UpdateOrder(OrderContext);


                if (OrderStatusId > 0)
                {
                    return Json(new { result = true, message = string.Empty, totals = Totals, paymentsGrid = RenderRazorPartialViewToString("PaymentsGrid", OrderContext.Order.AsOrder()), paymentId = 0 });
                    // return Json(new { result = true, message = string.Empty, totals = Totals, paymentsGrid = RenderRazorPartialViewToString("PaymentsGrid", OrderContext.Order.AsOrder()), paymentId = paymentResponse == null ? 0 : paymentResponse.OrderPaymentID });
                }
                return Json(new { result = false, message = "" });


            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, OrderContext.Order != null ? OrderContext.Order.OrderID.ToIntNullable() : null, accountID: CoreContext.CurrentAccount != null ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        private bool validatePaymenType(int PaymentConfiguration)
        {
            bool ret = false;

            int PaymentTypeID = Order.GetApplyPaymentType(PaymentConfiguration);
            // PaymentTypeID = 6(Product Credit)
            if (NetSteps.Data.Entities.Business.CTE.paymentTables != null)
            {
                var list = NetSteps.Data.Entities.Business.CTE.paymentTables.ToList().Where(x => x.PaymentType != 6).ToList();
                if (list.Count > 0)
                {
                    var i = list.Where(x => x.PaymentType != PaymentTypeID).ToList().Count;
                    if (i > 0)
                    {
                        ret = true;
                    }
                }
            }
            return ret;
        }

        protected virtual bool IsNonTraditionalPaymentMethod(int paymentMethodID)
        {
            return paymentMethodID < 0;
        }

        /// <summary>
        /// Override this method to add-in client specific PaymentTypes
        /// in addition to the base Encore ones.
        /// </summary>
        [NonAction]
        public virtual IPaymentTypeProvider GetPaymentTypeProvider()
        {
            return new PaymentTypeProvider();
        }

        [NonAction]
        public IPayment GetPaymentMethodFromProvider(PaymentTypeModel paymentTypeModel)
        {
            IPaymentTypeProvider provider = GetPaymentTypeProvider();
            IPayment payment = provider.GetPaymentMethod(paymentTypeModel);

            return payment;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Orders-Order Entry", "~/Orders")]
        public virtual ActionResult ValidAddNewPayment()
        {
            var MarketID = CoreContext.CurrentMarketId;  //siempre sale 56 , q es para brasil
            var s = OrderExtensions.GeneralParameterVal(MarketID);
            if (s == 1)
                return Json(new { result = true });
            else
                return Json(new { result = false });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Orders-Order Entry", "~/Orders")]
        public virtual ActionResult RemovePayment(string paymentId, int indice)
        {
            try
            {
                ////var customer = OrderContext.Order.AsOrder().OrderCustomers.FirstOrDefault();
                ////customer.RemovePayment(paymentId);

                //foreach (var item in Order.paymentReturn)
                //{
                //    if (item.PaymentId == Convert.ToInt32(paymentId))
                //    {
                //        Order.paymentReturn.Remove(item);
                //        break;
                //    }
                //}
                //OrderService.UpdateOrder(OrderContext);

                //return Json(new { result = true, totals = Totals, paymentsGrid = RenderRazorPartialViewToString("PaymentsGrid", OrderContext.Order.AsOrder())});
                //NetSteps.Data.Entities.Business.CTE.paymentTables.RemoveWhere(x => x.OrderPaymentId == indice);
                RemovePaymentsTable(indice);
                var customer = OrderContext.Order.AsOrder().OrderCustomers.FirstOrDefault();
                customer.RemovePayment(paymentId);
                OrderService.UpdateOrder(OrderContext);

                return Json(new { result = true, totals = Totals });

            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, OrderContext.Order != null ? OrderContext.Order.OrderID.ToIntNullable() : null, accountID: CoreContext.CurrentAccount != null ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }



        [HttpPost]
        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult LookupGiftCardBalance(string giftCardCode)
        {
            try
            {
                var gcService = Create.New<IGiftCardService>();
                decimal? balance = gcService.GetBalanceWithPendingPayments(giftCardCode, OrderContext.Order.AsOrder());
                if (!balance.HasValue)
                {
                    return Json(new { result = false, message = Translation.GetTerm("GiftCardNotFound", "Gift card not found") });
                }
                else
                {
                    return Json(new { result = true, balance = balance.Value.ToString(OrderContext.Order.AsOrder().CurrencyID), amountToApply = Math.Min(balance.Value, OrderContext.Order.AsOrder().Balance ?? 0m) });
                }
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, OrderContext.Order != null ? OrderContext.Order.OrderID.ToIntNullable() : null, accountID: CoreContext.CurrentAccount != null ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        #endregion

        [FunctionFilter("Orders", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public virtual ActionResult ApplyPromotionCode(string promotionCode)
        {
            try
            {
                var accountId = CoreContext.CurrentAccount.AccountID;
                if (string.IsNullOrWhiteSpace(promotionCode) || !OrderService.GetActivePromotionCodes(accountId).Contains(promotionCode))
                {
                    return JsonError(Translation.GetTerm("Promotions_PromotionCodeNotFound", "Promotion Code Not Found or Already Used"));
                }

                if (OrderContext.CouponCodes.Any(existing => existing.AccountID == CoreContext.CurrentAccount.AccountID && existing.CouponCode.Equals(promotionCode, StringComparison.InvariantCultureIgnoreCase)))
                {
                    return JsonError(Translation.GetTerm("Promotions_PromotionCodeAlreadyApplied", "Promotion Code Already Applied"));
                }

                var newCode = Create.New<ICouponCode>();
                newCode.CouponCode = promotionCode;
                newCode.AccountID = accountId;
                OrderContext.CouponCodes.Add(newCode);
                OrderContext.Order.AsOrder().OrderCustomers[0].RemoveAllPayments();
                OrderService.UpdateOrder(OrderContext);

                return Json(new
                {
                    totals = Totals,
                    result = true,
                    orderItems = GetOrderItemsHtml(OrderContext.Order.AsOrder()),
                    OrderEntryModelData = GetOrderEntryModelData(OrderContext.Order.AsOrder()),
                    promotions = GetApplicablePromotions(OrderContext.Order.AsOrder()),
                    shippingMethods = ShippingMethods,
                    outOfStockItems = GetOutOfStockItemsString(OrderContext.Order.AsOrder()),
                });
            }
            catch (Exception ex)
            {
                var message = ex.Log(orderID: OrderContext.Order.OrderID, accountID: CoreContext.CurrentAccount.AccountID).PublicMessage;
                return JsonError(message);
            }
        }

        [FunctionFilter("Orders", "~/", ConstantsGenerated.SiteType.BackOffice)]
        public virtual ActionResult RemovePromotionCode(string promotionCode)
        {
            if (string.IsNullOrWhiteSpace(promotionCode))
            {
                return JsonError(_errorInvalidPromotionCode(promotionCode));
            }

            try
            {
                var orderCustomer = OrderContext.Order.OrderCustomers.Single(customer => customer.AccountID == CoreContext.CurrentAccount.AccountID);
                var found = OrderContext.CouponCodes.SingleOrDefault(existing => existing.AccountID == orderCustomer.AccountID && existing.CouponCode.Equals(promotionCode, StringComparison.InvariantCultureIgnoreCase));
                if (found != null)
                {
                    OrderContext.CouponCodes.Remove(found);
                    OrderContext.Order.AsOrder().OrderCustomers[0].RemoveAllPayments();
                    OrderService.UpdateOrder(OrderContext);
                }

                return Json(new
                {
                    totals = Totals,
                    result = true,
                    orderItems = GetOrderItemsHtml(OrderContext.Order.AsOrder()),
                    OrderEntryModelData = GetOrderEntryModelData(OrderContext.Order.AsOrder()),
                    promotions = GetApplicablePromotions(OrderContext.Order.AsOrder()),
                    shippingMethods = ShippingMethods,
                    outOfStockItems = GetOutOfStockItemsString(OrderContext.Order.AsOrder()),
                });
            }
            catch (Exception ex)
            {
                var message = ex.Log(orderID: OrderContext.Order.OrderID, accountID: CoreContext.CurrentAccount.AccountID).PublicMessage;
                return JsonError(message);
            }
        }

        #region Overrides

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult GetOverrides()
        {
            try
            {
                StringBuilder html = new StringBuilder();
                foreach (OrderItem item in OrderContext.Order.AsOrder().OrderCustomers[0].ParentOrderItems)
                {
                    var product = Inventory.GetProduct(item.ProductID.ToInt());
                    var currency = SmallCollectionCache.Instance.Currencies.GetById(OrderContext.Order.AsOrder().CurrencyID);
                    var guidString = item.Guid.ToString("N");
                    html.Append("<tr id=\"").Append(guidString).Append("\">")
                        .AppendCell(product.SKU)
                        .AppendCell(product.Name)
                        .AppendCell(currency.CurrencySymbol + "<input type=\"text\" class=\"TextInput required price\" style=\"width: 50px;\" id=\"overridePrices" + guidString + "\" name=\"overridePrices\" value=\"" + Math.Round(item.GetAdjustedPrice(item.ProductPriceTypeID.Value), 2).ToString("F2") + "\" >")
                        .AppendCell(item.Quantity.ToString())
                        .AppendCell(currency.CurrencySymbol + "<input type=\"text\" class=\"TextInput required quantity\" style=\"width: 50px;\" id=\"cvAmount" + guidString + "\" name=\"overrideCVAmount\" value=\"" + Math.Round(item.CommissionableTotal.ToDecimal(), 2).ToString("F2") + "\" >")
                        .AppendCell((item.GetAdjustedPrice(item.ProductPriceTypeID.Value) * item.Quantity).ToString(currency))
                        .Append("</tr>");
                }

                return Json(new { products = html.ToString(), totals = Totals });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, OrderContext.Order != null ? OrderContext.Order.OrderID.ToIntNullable() : null, accountID: CoreContext.CurrentAccount != null ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        //[FunctionFilter("Orders-Override Order", "~/Orders/OrderEntry")]
        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult PerformOverrides(List<OrderItemOverride> items, decimal taxAmount, decimal shippingAmount)
        {
            try
            {
                bool result = true;
                string message = string.Empty;
                OrderContext.Order.AsOrder().StartEntityTracking();
                try
                {
                    if (OrderContext.Order.AsOrder().OrderTypeID == (int)NetSteps.Data.Entities.Constants.OrderType.ReplacementOrder)
                        OrderContext.Order.AsOrder().SetReplacementOrderPrices(items, taxAmount, shippingAmount);
                    else
                        OrderContext.Order.AsOrder().PerformOverrides(items, taxAmount, shippingAmount);
                }
                catch (Exception ex)
                {
                    result = false;
                    var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, orderID: OrderContext.Order != null ? OrderContext.Order.OrderID.ToIntNullable() : null);
                    // message = exception.PublicMessage;
                    message = Translation.GetTerm("InvalidOverrideValue", "Please check all of the values to make sure they are not negative or greater than the original amount.");
                }
                OrderService.UpdateOrder(OrderContext);

                return Json(new { result = result, message = message, orderItems = GetOrderItemsHtml(OrderContext.Order.AsOrder()), shippingMethods = ShippingMethods, totals = Totals });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, OrderContext.Order != null ? OrderContext.Order.OrderID.ToIntNullable() : null, accountID: CoreContext.CurrentAccount != null ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [FunctionFilter("Orders-Override Order", "~/Orders/OrderEntry")]
        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult CancelOverrides()
        {
            try
            {
                OrderContext.Order.AsOrder().StartEntityTracking();

                if (OrderContext.Order.AsOrder().OrderTypeID == (int)NetSteps.Data.Entities.Constants.OrderType.ReplacementOrder)
                    OrderContext.Order.AsOrder().UndoReplacementOrderPrices();
                else
                    OrderContext.Order.AsOrder().CancelOverrides();

                OrderService.UpdateOrder(OrderContext);

                return Json(new { result = true, orderItems = GetOrderItemsHtml(OrderContext.Order.AsOrder()), shippingMethods = ShippingMethods, totals = Totals });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, OrderContext.Order != null ? OrderContext.Order.OrderID.ToIntNullable() : null, accountID: CoreContext.CurrentAccount != null ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        #endregion

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Orders-Order Entry", "~/Orders")]
        public virtual ActionResult SubmitOrder(string invoiceNotes, string email, string PeriodID)
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
                OrderShipment shipments = OrderContext.Order.AsOrder().OrderShipments[0]; //OrderContext.Order.AsOrder().GetDefaultShipment();

                var shippingMethods = OrderContext.Order.AsOrder().GetShippingMethods(shipments);
                if (shippingMethods != null)
                    shippingMethods = shippingMethods.OrderBy(sm => sm.ShippingAmount).ToList();
                if (shippingMethods.Count() == 0)
                {
                    return Json(new { result = false, message = Translation.GetTerm("NoShippingAddress", "There is no shipping address.  Please choose an address to ship to.") });
                }


                OrderContext.Order.AsOrder().DateCreated = DateTime.Now;
                OrderContext.Order.AsOrder().StartEntityTracking();

                if (!invoiceNotes.ToCleanString().IsNullOrEmpty())
                    OrderContext.Order.AsOrder().InvoiceNotes = invoiceNotes;

                // Submit the order
                OrderContext.Order.AsOrder().IsTemplate = true;
                OrderContext.Order.AsOrder().Balance = 0;
                DateTime _dateCurrent = DateTime.Now;
                decimal totalAppliedAmount = 0;

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

                            totalAppliedAmount = totalAppliedAmount + paymentTable.AppliedAmount.ToDecimal();

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
                int shippingOrderType = OrderContext.Order.AsOrder().OrderShipments[0].ShippingMethodID.Value;
                OrderContext.Order.AsOrder().OrderShipments[0].ShippingMethodID = Order.GetShippingMethodID(shippingOrderType);
                OrderContext.Order.AsOrder().ParentOrderID = null;
                var result = OrderService.SubmitOrder(OrderContext);
                /*--------------------------------------------------------
                  BR-CC-007  KTC
                --------------------------------------------------------*/
                //CreateTickets();
                //BR-CC-007-END 

                if (!result.Success)
                {
                    return Json(new { result = false, validrule = false, message = result.Message, paymentsGrid = RenderRazorPartialViewToString("PaymentsGrid", OrderContext.Order.AsOrder()) });
                }
                else
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

                                objEP.ModifiedByUserID = CoreContext.CurrentUser.UserID;
                                objEP.ProductCredit = decimal.Parse(Session["ProductCredit"].ToString());

                                int filasAfectadas = Order.UPDPaymentConfigurations(item.OrderPaymentID,
                                    item.OrderID,
                                    paymentTable.PaymentConfigurationID.Value,
                                    paymentTable.NumberCuota
                                    , objEP, OrderExtensions.GeneralParameterVal(CoreContext.CurrentMarketId, "ANM"));


                                //--------------------------------------------------------------------//
                                if ((decimal.Parse(Session["ProductCredit"].ToString()) != 0) && paymentTable.PaymentConfigurationID.Value == 60)
                                {
                                    decimal productCredit = Order.GetProductCreditByAccount(OrderContext.Order.AsOrder().ConsultantID);
                                    var endingAmount = CreditApplied(totalAppliedAmount);
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
                    var orderStatusID =
                        AccountPropertiesBusinessLogic.GetValueByID(11, OrderContext.Order.OrderID).OrderStatusID;
                    if (orderStatusID == ConstantsGenerated.OrderStatus.Paid.ToShort())
                    {
                        PersonalIndicardorAsynExtensions personalIndicardorAsyn = new PersonalIndicardorAsynExtensions();
                        personalIndicardorAsyn.UpdatePersonalIndicatorAsyn(OrderContext.Order.OrderID, orderStatusID);
                        //OrderExtensions.UpdatePersonalIndicator(OrderContext.Order.OrderID, OrderStatusID);
                        //INI - GR6356 - CDAS              
                        var account = Account.Load(OrderContext.Order.OrderCustomers[0].AccountID);
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
                    if (orderStatusID == (short)Constants.OrderStatus.PendingPerPaidConfirmation)
                    {           
                        var account = Account.Load(OrderContext.Order.OrderCustomers[0].AccountID);
                        if (account != null && account.AccountStatusID == (short)Constants.AccountStatus.BegunEnrollment)
                        {
                            account.EnrollmentDateUTC = null;
                            account.Save();
                        }
                    }
                    //FIN - GR6356 - CDAS

                    var orderID = OrderContext.Order.AsOrder().OrderID;
                    var postalCode = OrderContext.Order.AsOrder().OrderShipments[0].PostalCode;
                    var warehouseID = Convert.ToInt32(Session[SessionConstants.WareHouseId]);
                    var dateEstimated = Convert.ToString(Session[SessionConstants.DateEstimated]);

                    SetKitItemPrices(OrderContext.Order);

                    var resultOrderShipments = PaymentsMethodsExtensions.ManagementKit(postalCode,
                        warehouseID,
                        orderID,
                        dateEstimated);

                    insertDispatchProductsFill();
                    // wv:20160603 inserta los dispatchProducts en tabla de control de dispatch
                }
                var scheduler = Create.New<IEventScheduler>();
                scheduler.ScheduleOrderCompletionEvents(OrderContext.Order.AsOrder().OrderID);

                var orderNumber = OrderContext.Order.AsOrder().OrderNumber;
                ApplyConfigurationCumulative(OrderContext);

                OrderContext.Clear();
                return Json(new { result = true, orderNumber = orderNumber });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, OrderContext.Order != null ? OrderContext.Order.OrderID.ToIntNullable() : null, accountID: CoreContext.CurrentAccount != null ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, validrule = false, message = exception.PublicMessage, paymentsGrid = RenderRazorPartialViewToString("PaymentsGrid", OrderContext.Order.AsOrder()) });
            }
        }

        private void SetKitItemPrices(IOrder order)
        {
            OrderService.AddKitItemPrices(order);
        }


        private void insertDispatchProductsFill()
        {
            // ------------------------------------------------------------------------------------------------------------------------------------------------
            /*
             * Recorre la variable de session registrada con los Dispatch filtrados y los almacena en la base de datos dentro de la tabla [DispatchItemControls]
             * wv: 20160531
             * Session => itemsProductsDispatch
             * 
             */

            string sessionEdit = Session["Edit"].ToString();
            //if (sessionEdit != "1")
            //{ 
            var lstProductsVal = (List<DispatchProducts>)Session["itemsProductsDispatch"];
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
                //} 
            }
            // ------------------------------------------------------------------------------------------------------------------------------------------------
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
        #       endregion
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
        //BR-CC-007 KTC        
        private decimal CreditApplied(decimal totalAppliedAmount)
        {

            // var KeyDecimals = ConfigurationManager.AppSettings["CultureDecimal"];

            var getBalanceDue = Totals.GetType().GetProperty("balanceDue").GetValue(Totals, null);
            string[] strBalanceDue = Convert.ToString(getBalanceDue).Split('$');
            decimal balancDue = 0;
            balancDue = NetSteps.Data.Entities.Extensions.DecimalExtensions.FormatGlobalizationDecimal(Convert.ToDecimal(strBalanceDue[1].ToString()));

            //if (KeyDecimals == "ES")
            //{
            //    var culture = CultureInfoCache.GetCultureInfo("En");
            //    balancDue = Convert.ToDecimal(strBalanceDue[1].ToString(), culture);


            //}
            //else
            //{
            //    balancDue = Convert.ToDecimal(strBalanceDue[1].ToString().Replace(",", "."));
            //}

            return balancDue;
        }

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
                int PaymentTypeID = 0;
                //response = OrderContext.Order.AsOrder().ApplyPaymentToCustomer(PaymentTypeID, amount);

                return_StatusIDPayment = response.Item.OrderPaymentStatusID;
            }
            catch
            {
                return_StatusIDPayment = 0;
            }
            return return_StatusIDPayment;

        }

        public DateTime GetShippingMethods(int WareHouse, int PostalCode)
        {
            return DateTime.Now;
        }

        //BR-CC-007 end 

        private JsonResult validateShippingAddress()
        {
            // Validate if a shipping address has been selected and contains enough information to ship the order
            if (OrderContext.Order.AsOrder().OrderShipments.Count < 1)
            {
                return Json(new
                {
                    result = false,
                    message = Translation.GetTerm("NoShippingAddress",
                        "There is no shipping address.  Please choose an address to ship to.")
                });
            }

            foreach (var shipment in OrderContext.Order.AsOrder().OrderShipments)
            {
                if (string.IsNullOrEmpty(shipment.Address1) ||
                    string.IsNullOrEmpty(shipment.City) ||
                    string.IsNullOrEmpty(shipment.State) ||
                    string.IsNullOrEmpty(shipment.PostalCode))
                {
                    return Json(
                            new
                            {
                                result = false,
                                message =
                                Translation.GetTerm("InvalidShippingAddress", "The shipping address is invalid.")
                            });
                }
            }

            return null;
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Orders-Order Entry", "~/Orders")]
        public virtual ActionResult SaveOrder(string invoiceNotes, string email, string PeriodID)
        {
            try
            {
                var preOrderId = Convert.ToInt32(Session[SessionConstants.PreOrder]);

                //Validate Shipping
                var validateShipping = validateShippingAddress();
                if (validateShipping != null) { return validateShipping; }

                OrderContext.Order.AsOrder().DateCreated = DateTime.Now;
                SetOrderCustomDefaultValues(invoiceNotes);
                OrderContext.Order.Save();
                SetKitItemPrices(OrderContext.Order);

                //Actualizar la PreOrder
                string session = Session[SessionConstants.Edit].ToString();
                if (session == "0")
                {
                    Order.UpdatePreOrder(OrderContext.Order.AsOrder().OrderID, preOrderId);
                }

                var orderID = OrderContext.Order.AsOrder().OrderID;
                var postalCode = OrderContext.Order.AsOrder().OrderShipments[0].PostalCode;
                var warehouseID = Convert.ToInt32(Session[SessionConstants.WareHouseId]);
                var dateEstimated = Convert.ToString(Session[SessionConstants.DateEstimated]);

                PaymentsMethodsExtensions.ManagementKit(postalCode, warehouseID, orderID,
                    dateEstimated);

                insertDispatchProductsFill();
                // wv: 20160603 inserta los Dispatch de productos filtrados para el account

                return Json(new { result = true, orderNumber = OrderContext.Order.AsOrder().OrderNumber });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex,
                    Constants.NetStepsExceptionType.NetStepsApplicationException,
                    OrderContext.Order != null ? OrderContext.Order.OrderID.ToIntNullable() : null,
                    accountID:
                    CoreContext.CurrentAccount != null ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        private void SetOrderCustomDefaultValues(string invoiceNotes)
        {
            if (!invoiceNotes.ToCleanString().IsNullOrEmpty()) OrderContext.Order.AsOrder().InvoiceNotes = invoiceNotes;

            // Set ExpirationStatusID
            OrderContext.Order.AsOrder()
                .OrderPayments.Each(o => o.ExpirationStatusID = (int)ConstantsGenerated.ExpirationStatuses.Unexpired);
            //TODO: Review this process doesn't look good - Set ShippingMethodID
            var shippingMethodID = OrderContext.Order.AsOrder().OrderShipments[0].ShippingMethodID;
            if (shippingMethodID != null)
            {
                OrderContext.Order.AsOrder().OrderShipments[0].ShippingMethodID = Order.GetShippingMethodID(shippingMethodID.Value);
            }

            OrderContext.Order.AsOrder().ParentOrderID = null;
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Orders-Order Entry", "~/Orders")]
        public ActionResult CancelOrder()
        {
            string message = null;
            bool result = true;
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

            try
            {
                if (OrderContext.Order != null && result)
                {
                    //Actualizar inventario
                    int count = 0;

                    int ppt = OrderContext.Order.AsOrder().OrderCustomers[0].ProductPriceTypeID;

                    foreach (var item in OrderContext.Order.AsOrder().OrderCustomers[0].OrderItems)
                    {
                        var ne = Order.GenerateAllocation(item.ProductID.Value, item.Quantity,
                            OrderContext.Order.AsOrder().OrderID,
                            Convert.ToInt32(Session[SessionConstants.WareHouseId]), EntitiesEnums.MaintenanceMode.Delete,
                            Convert.ToInt32(Session[SessionConstants.PreOrder]),
                            CoreContext.CurrentAccount.AccountTypeID, false);
                    }

                    OrderContext.Clear();
                }

                return Json(new { result = result, message = message });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, orderID: OrderContext.Order.OrderID.ToIntNullable());
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public ActionResult RedirectPage()
        {
            if (OrderContext.Order.OrderStatusID == 1)
            {
                OrderContext.Order.OrderStatusID = 5;
                OrderContext.Order.Save();
            }
            return RedirectToAction("NewOrder", "OrderEntry");
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
}


