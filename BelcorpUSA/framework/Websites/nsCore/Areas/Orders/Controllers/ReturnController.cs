using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;
using NetSteps.Common.Base;
using NetSteps.Common.Configuration;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Common.Reflection;
using NetSteps.Data.Common.Services;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business.Interfaces;
using NetSteps.Data.Entities.Business.Logic;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Services;
using NetSteps.Data.Entities.Utility;
using NetSteps.Diagnostics.Utilities;
using NetSteps.Encore.Core.IoC;
using NetSteps.Web;
using NetSteps.Web.Mvc.Attributes;
using nsCore.Areas.Orders.Models;
using nsCore.Contexts;
using nsCore.Controllers;
using nsCore.Models;
using AvataxAPI = NetSteps.Data.Entities.AvataxAPI;
using System.IO;
using System.Xml.Linq;
using NetSteps.Data.Entities.Business.Logic;
using NetSteps.Data.Entities.Utility;
using NetSteps.Data.Entities.Business;
using NetSteps.Web.Mvc.Helpers;
using NetSteps.Data.Entities.Dto;
using NetSteps.Data.Entities.Generated;
using NetSteps.Data.Entities.Repositories;

//Modifications:
//@01 20150716 BR-AT-005 G&S PGCT: Create imputs and functions needed for the requeriment

namespace nsCore.Areas.Orders.Controllers
{
    public partial class ReturnController : BaseController
    {
        #region Members
        private Product _restockingFeeProduct;
        protected Product RestockingFeeProduct
        {
            get
            {
                if (this._restockingFeeProduct == null)
                {
                    this._restockingFeeProduct = Order.GetRestockingFeeProduct();
                }
                return this._restockingFeeProduct;
            }
        }

        protected string RestockingFeeSku { get { return this.RestockingFeeProduct == null ? string.Empty : this.RestockingFeeProduct.SKU ?? string.Empty; } }

        protected virtual Order ReturnOrder
        {
            get
            {
                Order returnOrder = null;

                if (OrderContext.Order != null)
                {
                    returnOrder = OrderContext.Order.AsOrder();
                }

                if (returnOrder == null || returnOrder.ParentOrderID != OrderContext.OriginalOrder.OrderID)
                {
                    // Create the new return order with the items they selected
                    returnOrder = CreateReturnOrder();
                }

                returnOrder.OrderPendingState = Constants.OrderPendingStates.Quote;
                returnOrder.GetOrderItemsInheritance(OrderContext.OriginalOrder.OrderID); //Developed by Wesley Campos S. - CSTI
                //OrderContext.Order = returnOrder; 
                return returnOrder;
            }
            set
            {
                OrderContext.Order = value;
            }
        }

        protected virtual List<Order> ReturnOrders
        {
            get { return Session["ReturnOrders"] as List<Order>; }
            set { Session["ReturnOrders"] = value; }
        }

        protected virtual List<OrderProductAmount> ReturnableProducts
        {
            get { return Session["ReturnableProducts"] as List<OrderProductAmount>; }
            set { Session["ReturnableProducts"] = value; }
        }

        /// <summary>
        /// The current order context retrieved from session on each request.
        /// </summary>
        protected virtual IReturnOrderContext OrderContext
        {
            get
            {
                return _orderContext;
            }
        }
        private IReturnOrderContext _orderContext;
        #endregion

        public IOrderService OrderService { get { return Create.New<IOrderService>(); } }

        public InventoryBaseRepository Inventory { get { return Create.New<InventoryBaseRepository>(); } }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            // Get OrderContext from session.
            if (filterContext.HttpContext == null || filterContext.HttpContext.Session == null)
            {
                return;
            }

            var context = OrderContextSessionProvider.Get(filterContext.HttpContext.Session);
            this._orderContext = context is IReturnOrderContext ? (IReturnOrderContext)context : Create.New<IReturnOrderContext>();
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            if (_orderContext != null && HttpContext != null && HttpContext.Session != null)
            {
                OrderContextSessionProvider.Set(HttpContext.Session, _orderContext);
            }
        }

        [FunctionFilter("Orders-Return Order", "~/Orders")]
        public virtual ActionResult Index(int id = 0)
        {
            try
            {
                if (id > 0)
                {
                    if (OrderContext.OriginalOrder == null || OrderContext.OriginalOrder.OrderID != id)
                    {
                        Order order = Order.LoadFull(id);
                        OrderContext.OriginalOrder = order;
                    }
                }

                // se verifica q esta viniendo de SUpport ticket
                // en caso se pregunte por q se suma +10000 desde l pantalla de Support se envia el supportTicketID pero en esta pantalla se usa el supportticketnumber(el cual se halla la suma del supportticketId mas 10000) 
                int SupportTicketNumber = Request.QueryString["SupportTicketID"] != null ? 10000 + Convert.ToInt32(Request.QueryString["SupportTicketID"]) : 0;
                string SupportTicketID = Request.QueryString["SupportTicketID"] != null ? Request.QueryString["SupportTicketID"] : "0";

                if (OrderContext.OriginalOrder == null)
                    return RedirectToAction("Index");

                var originalOrder = OrderContext.OriginalOrder.AsOrder().Clone();

                ActionResult finalResult = SetReturnOrdersAndViewData(originalOrder);

                //@01 C01 - Calculate total and validate if is active the "Cancel Paid Order" option.
                decimal totalItems = OrderContext.OriginalOrder.OrderCustomers.Select(c => c.OrderItems.Select(o => o.ItemPrice).Sum()).Sum();
                ViewData["ProductCreditAmount"] = totalItems;
                ViewData["ProductCreditAmountString"] = totalItems.ToString(originalOrder.CurrencyID);
                ViewData["isCancelPaidOrder"] = (originalOrder.IsCancellable() && originalOrder.OrderStatusID == (short)Constants.OrderStatus.Paid) ||
                                                    (originalOrder.OrderStatusID == (short)Constants.OrderStatus.CancelledPaid);
                //@01 C01 - End calculate total and validate if is active the "Cancel Paid Order" option.

                ViewData["SupportTicketID"] = SupportTicketID;
                ViewData["SupportTicketNumber"] = SupportTicketNumber;

                foreach (OrderCustomer customer in originalOrder.OrderCustomers)
                {
                    var ProductPriceTypeID = customer.ProductPriceTypeID;

                    foreach (OrderItem orderItem in customer.ParentOrderItems)
                    {
                        foreach (OrderItem childItem in orderItem.ChildOrderItems)
                        {
                            childItem.ItemPrice = OrderBusinessLogic.Instance.GetItemPriceByIdAndType(childItem.OrderItemID, ProductPriceTypeID);
                        }
                    }
                }

                return finalResult ?? View(originalOrder);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        /// <summary>
        /// Index por parametros --At-005
        /// </summary>
        /// <param name="orderNumber">Numero de Orden</param>
        /// <param name="idSupport">Id de soporte</param>
        /// <returns></returns>
        [FunctionFilter("Orders-Return Order", "~/Orders")]
        public virtual ActionResult OrderParams(int accountID, string orderNumber, int? idSupport)
        {
            try
            {
                //if (!idSupport.HasValue)
                //    throw new Exception("The current order needs an ID support ticket");

                int id = NetSteps.Data.Entities.Business.Logic.OrderBusinessLogic.Instance.OrderIdByFilters(orderNumber, idSupport.Value);
                if (id > 0)
                {
                    if (OrderContext.OriginalOrder == null || OrderContext.OriginalOrder.OrderID != id)
                    {
                        Account account = null;
                        //int accountID = Order.Load(id).OrderCustomers.ElementAt(0).AccountID;
                        //account = Account.LoadForSession(562490);
                        account = Account.LoadForSession(accountID);
                        CoreContext.CurrentAccount = account.Clone();
                        CoreContext.CurrentAccount.StartEntityTracking();
                        Order order = Order.LoadFull(id);
                        OrderContext.Order = order;
                        OrderContext.OriginalOrder = order;

                        //Order order = Order.LoadFull(id);
                        //OrderContext.OriginalOrder = order;
                    }
                }

                if (OrderContext.OriginalOrder == null)
                    return RedirectToAction("Index");

                var originalOrder = OrderContext.OriginalOrder.AsOrder().Clone();

                ActionResult finalResult = SetReturnOrdersAndViewData(originalOrder);

                //@01 C01 - Calculate total and validate if is active the "Cancel Paid Order" option.
                decimal totalItems = OrderContext.OriginalOrder.OrderCustomers.Select(c => c.OrderItems.Select(o => o.ItemPrice).Sum()).Sum();
                ViewData["ProductCreditAmount"] = totalItems;
                ViewData["ProductCreditAmountString"] = totalItems.ToString(originalOrder.CurrencyID);
                ViewData["isCancelPaidOrder"] = (originalOrder.IsCancellable() && originalOrder.OrderStatusID == (short)Constants.OrderStatus.Paid) ||
                                                    (originalOrder.OrderStatusID == (short)Constants.OrderStatus.CancelledPaid);
                //@01 C01 - End calculate total and validate if is active the "Cancel Paid Order" option.    

                return finalResult ?? View("Index", originalOrder);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        [FunctionFilter("Orders-Return Order", "~/Orders")]
        public virtual ActionResult Edit(string orderNumber)
        {
            try
            {
                Order returnOrder = null;
                if (!string.IsNullOrEmpty(orderNumber))
                    returnOrder = Order.LoadByOrderNumberFull(orderNumber);

                if (returnOrder == null)
                    return RedirectToAction("Index");

                if (returnOrder.OrderStatusID == Constants.OrderStatus.Paid.ToInt())
                {
                    TempData["Error"] = Translation.GetTerm("ReturnOrderCannotBeEditedAfterItHasEnteredSubmittedStatus", "Return order cannot be edited after it has entered Submitted status.");
                    return RedirectToAction("Index", "Details", new { id = returnOrder.OrderID });
                }

                if (returnOrder.ParentOrderID != null && returnOrder.ParentOrderID > 0)
                {
                    if (OrderContext.OriginalOrder == null || OrderContext.OriginalOrder.OrderID != returnOrder.ParentOrderID)
                    {
                        Order order = Order.LoadFull(returnOrder.ParentOrderID.ToInt());
                        OrderContext.OriginalOrder = order;
                    }
                }

                if (OrderContext.OriginalOrder == null)
                    return RedirectToAction("Index");

                var originalOrder = OrderContext.OriginalOrder.AsOrder().Clone();

                ActionResult finalResult = SetReturnOrdersAndViewData(originalOrder);

                ReturnOrder = returnOrder;

                return finalResult ?? View("Index", OrderContext.OriginalOrder.AsOrder());
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        [FunctionFilter("Orders-Return Order", "~/Orders")]
        public virtual ActionResult UpdateProductReturns(int originalOrderId, bool overridenShipping, string restockingFee, List<ProductReturn> returnedProducts, decimal refundedShipping = 0m)
        {
            decimal restockingFeeDecimal = decimal.Parse(restockingFee, System.Globalization.NumberFormatInfo.InvariantInfo);
            try
            {
                var originalOrder = GetOriginalOrder(originalOrderId);
                // Add all of the returned products to the order

                UpdateReturnItems(overridenShipping, restockingFeeDecimal, returnedProducts, originalOrder, ReturnOrder);

                if (overridenShipping)
                {
                    ReturnOrder.ShippingTotalOverride = refundedShipping;
                    ReturnOrder.ShippingTotal = refundedShipping;
                    OrderService.UpdateOrder(OrderContext);
                }

                ViewData["ReturnOrder"] = ReturnOrder;
                Session["ProductCreditAmountString"] = 22222;



                return Json(new
                {
                    result = true,
                    totals = new
                    {
                        subtotal = ReturnOrder.Subtotal.ToString(ReturnOrder.CurrencyID),
                        taxTotal = ReturnOrder.TaxAmountTotal.ToString(ReturnOrder.CurrencyID),
                        grandTotal = ReturnOrder.GrandTotal.ToString(ReturnOrder.CurrencyID),
                        shippingTotal = ReturnOrder.ShippingTotal,
                        originalShippingTotal = ReturnOrder.ShippingTotal ?? 0m
                    },
                    returnedProducts = returnedProducts
                });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, orderID: (ReturnOrder != null) ? ReturnOrder.OrderID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
        public decimal HandleDecimalCulture(string value)
        {
            var KeyDecimals = ConfigurationManager.AppSettings["CultureDecimal"];
            var amount = 0M;
            if (KeyDecimals == "ES")
            {
                var culture = CultureInfoCache.GetCultureInfo("En");
                amount = Convert.ToDecimal(value, culture);
            }

            return amount;
        }

        [FunctionFilter("Orders-Return Order", "~/Orders")]
        public virtual ActionResult UpdateShippingRefunded(string shippingAmount)
        {
            try
            {
                string[] shippingAmountNew = shippingAmount.Split('(', ')');
                string newValue = "";
                foreach (var item in shippingAmountNew)
                {
                    newValue = newValue + item;
                }
                var culture = CultureInfoCache.GetCultureInfo("En");
                decimal totalOverride;

                totalOverride = Convert.ToDecimal(newValue, culture);

                Order order = ReturnOrder;
                order.ShippingTotalOverride = totalOverride;
                order.ShippingTotal = totalOverride;
                OrderService.UpdateOrder(OrderContext);
                ViewData["ReturnOrder"] = order;

                return Json(new
                {
                    result = true,
                    grandTotal = (order.GrandTotal).ToString(order.CurrencyID)
                });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, orderID: (OrderContext.OriginalOrder != null) ? OrderContext.OriginalOrder.OrderID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }


        [FunctionFilter("Orders-Return Order", "~/Orders")]
        public virtual ActionResult PendingConfirm(int originalOrderId, bool refundOriginalPayments, Dictionary<int, decimal> refundPayments, int returnType
            , string invoiceNotes, bool creditType, string creditAmount, List<ReturnOrderItemDto> OrderItemList, bool esOrdenTotal)
        {
            Order order = ReturnOrder;
            try
            {
                string SQLInsert = "";
                string str13 = "\r\n ";
                foreach (var item in OrderItemList)
                {
                    SQLInsert = SQLInsert + "INSERT @OrderItemList VALUES(" + (item.ParentOrderItemID.ToString().Length == 0 ? "NULL" : item.ParentOrderItemID.ToString())
                        + "," + item.OrderItemID.ToString() + "," + item.ProductID.ToString() + ",'" + item.SKU + "'," + item.ParentQuantity.ToString() + ","
                        + item.Quantity.ToString() + "," + item.QuantityOrigen.ToString() + "," + item.ItemPrice.ToString() + ","
                        + (item.HasComponents ? "1" : "0") + "," + (item.AllHeader ? "1" : "0") + "," + (item.IsChild ? "1" : "0") + ")" + str13;
                }
                var culture = CultureInfoCache.GetCultureInfo("En");
                // Create the new return order with the items they selected
                decimal creditAmountDecimal = decimal.Parse(creditAmount, System.Globalization.NumberFormatInfo.InvariantInfo);
                var response = UpdateReturnOrder(originalOrderId, refundOriginalPayments, refundPayments, returnType, invoiceNotes, creditType, creditAmountDecimal, false);
                if (!response.Success)
                    return Json(new { result = false, message = response.Message });
                order = response.Item;
                OrderContext.Order.OrderStatusID = (short)Constants.OrderStatus.PendingConfirm;

                //Grabar la orden con la fecha actual
                order.CompleteDate = DateTime.Today;
                order.CompleteDateUTC = DateTime.Today;
                order.ShippingTotal = 0;
                order.ShippingTotalOverride = null;
                
                OrderContext.Order.Save();

                //Para agregar el periodo segun la fecha 
                Dictionary<int, bool> result = new Dictionary<int, bool>();
                result = NetSteps.Data.Entities.Periods.GetPeriodByDate(DateTime.Now);

                int resultOrderUpdate = Order.UPDOrderbyID(OrderContext.Order.AsOrder().OrderID,
                                                           1,
                                                           Convert.ToInt32(Session["WareHouseId"]),
                                                           Convert.ToInt32(result.ElementAt(0).Key),
                                                           CoreContext.CurrentAccount.AccountTypeID);


                //Recorrer los items registrados
                decimal subTotal = 0;
                //foreach (var item in OrderContext.Order.AsOrder().OrderCustomers)
                //{
                //    foreach (var orderItem in item.OrderItems)
                //    {
                //        foreach (var childItems in OrderItemList.Where(x => x.ParentOrderItemID == orderItem.ParentOrderItemID))
                //        {
                //            if (childItems.ParentOrderItemID != null)
                //            {
                //                //insertar items
                //                subTotal = subTotal + Convert.ToDecimal(childItems.ItemPrice, culture);
                //                PreOrderExtension.InsOrderItemsPending(item.OrderCustomerID, childItems.OrderItemID, orderItem.OrderItemID, subTotal);
                //            }
                //        }
                //    }
                //}

                order.NegateValuesForReturn();
                
                OrderContext.Order.Save();

                int OrderIDReturn = OrderContext.Order.OrderID;
                int resultOrderItem = Order.UPDOrderItemProductReturn(originalOrderId, OrderIDReturn, OrderItemList, 1, esOrdenTotal);

                PreOrderExtension.UpdTotalsPending(OrderContext.Order.OrderID, OrderContext.Order.ParentOrderID.Value, esOrdenTotal);
                //PreOrderExtension.udpOrderPending(OrderContext.Order.OrderID);
                var oderNumber = OrderContext.Order.AsOrder().OrderNumber;
                OrderContext.Order = Order.LoadByOrderNumberFull(oderNumber);
                return Json(new { result = true, returnOrderNumber = OrderContext.Order.AsOrder().OrderNumber });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, orderID: (ReturnOrder != null) ? ReturnOrder.OrderID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
        
        [FunctionFilter("Orders-Return Order", "~/Orders")]
        public virtual ActionResult SubmitReturn(int originalOrderId, bool refundOriginalPayments, Dictionary<int, decimal> refundPayments, int returnType, string invoiceNotes, bool creditType, string creditAmount, bool isTotal, List<ReturnOrderItemDto> OrderItemList)
        {
            Order returnOrder = ReturnOrder;
            try
            {
                string SQLInsert = "";
                string str13 = "\r\n ";
                foreach (var item in OrderItemList)
                {
                    SQLInsert = SQLInsert + "INSERT @OrderItemList VALUES(" + (item.ParentOrderItemID.ToString().Length == 0 ? "NULL" : item.ParentOrderItemID.ToString())
                        + "," + item.OrderItemID.ToString() + "," + item.ProductID.ToString() + ",'" + item.SKU + "'," + item.ParentQuantity.ToString() + ","
                        + item.Quantity.ToString() + "," + item.QuantityOrigen.ToString() + "," + item.ItemPrice.ToString() + ","
                        + (item.HasComponents ? "1" : "0") + "," + (item.AllHeader ? "1" : "0") + "," + (item.IsChild ? "1" : "0") + ")" + str13;
                }

                bool EsRetornoTotal = false;
                EsRetornoTotal = isTotal;
                bool allowClearAllocation = false;
                bool allowSAP = false;
                bool allowApplyCredit = false;
                bool originalOrderWasCancelled = false;
                bool allowReverseStatusActivity = false;

                var originalOrderTmp = Order.LoadFull(originalOrderId);
                // Create the new return order with the items they selected
                decimal creditAmountDecimal = decimal.Parse(creditAmount, System.Globalization.NumberFormatInfo.InvariantInfo);

                //Evaluar si la devolución es total por medio del monto a retornar.
                isTotal = false;
                if (creditAmountDecimal == originalOrderTmp.GrandTotal)
                {
                    isTotal = true;
                }

                bool UPDOrderItemProductReturn = true;
                if (originalOrderTmp.OrderStatusID == (short)Constants.OrderStatus.PendingConfirm)
                {
                    UPDOrderItemProductReturn = false;
                    returnOrder = originalOrderTmp;
                }
                else
                {
                    var response = UpdateReturnOrder(originalOrderId, refundOriginalPayments, refundPayments, returnType, invoiceNotes, creditType, creditAmountDecimal, false);
                    if (!response.Success)
                        return Json(new { result = false, message = response.Message });

                    OrderContext.Order = response.Item;

                    var validationResponse = ValidateReturn(originalOrderId, OrderContext.Order.AsOrder());
                    if (!validationResponse.Success)
                        return Json(new { result = false, message = validationResponse.Message });

                    OrderContext.Order.Save();
                    returnOrder = OrderContext.Order.AsOrder();
                }

                string message = string.Empty;
                bool result = true;

                if (result)
                {
                    var totalPayments = returnOrder.OrderPayments.Sum(op => op.Amount);
                    returnOrder.PaymentTotal = totalPayments;
                    returnOrder.Balance = returnOrder.GrandTotal - totalPayments;
                    returnOrder.ChangeToPaidStatus(); //TODO: cambiar logica a los nuevos estados
                    returnOrder.CompleteDate = DateTime.Now;
                    returnOrder.CommissionDate = DateTime.Now;

                    var originalOrder = Order.Load(originalOrderId);

                    OrderBusinessLogic.ActionOnReturnOrCancelOrder(originalOrder.OrderStatusID, ref originalOrderWasCancelled, ref allowClearAllocation, ref allowApplyCredit, ref allowSAP, ref allowReverseStatusActivity);

                    //returnOrder.UpdateInventoryLevels(true, originalOrderWasCancelled);
                    returnOrder.NegateValuesForReturn();

                    foreach (OrderCustomer customer in returnOrder.OrderCustomers)
                    {
                        foreach (OrderItem orderItem in customer.ParentOrderItems)
                        {
                            if (!OrderItemList.Any(x => x.OrderItemID == orderItem.OrderItemID))
                            {
                                returnOrder.ParentUnselected(orderItem.OrderItemID);
                            }
                        }
                    }

                    OrderContext.Order = returnOrder;
                    
                    OrderContext.Order.Save();

                    //Para agregar el periodo segun la fecha 
                    Dictionary<int, bool> resultado = new Dictionary<int, bool>();
                    resultado = NetSteps.Data.Entities.Periods.GetPeriodByDate(DateTime.Now);
                    var periodo = resultado.FirstOrDefault().Key;
                    var OrderID = OrderContext.Order.AsOrder().OrderID;
                    var WareHouseId = Convert.ToInt32(Session["WareHouseId"]);
                    var AccountTypeID = CoreContext.CurrentAccount.AccountTypeID;
                    int resultOrderUpdate = Order.UPDOrderbyID(OrderID, 1, WareHouseId ,periodo, AccountTypeID);

                    int statusPending = PreOrderExtension.uspGetStatusOrder(OrderContext.Order.AsOrder().ParentOrderID.Value);
                    if (statusPending == 21)
                    {
                        PreOrderExtension.udpOrderPending(OrderContext.Order.OrderID, EsRetornoTotal);
                    }
                    else
                    {
                        PreOrderExtension.updOrderAmounts(OrderContext.Order.OrderID);
                    }
                    var returnedOrderNumber = returnOrder.OrderNumber;

                    if(UPDOrderItemProductReturn)
                        Order.UPDOrderItemProductReturn(originalOrderId, returnOrder.OrderNumber.ToInt(), OrderItemList, 1, EsRetornoTotal);

                    //TODO: Change this logic   //AvataxAPI
                    //if Avatax config is enabled
                    int countryID = Country.GetCountriesByMarketID(returnOrder.OrderCustomers[0].MarketID)[0].CountryID;
                    if (countryID == (int)Constants.Country.UnitedStates)
                    {
                        if (!string.IsNullOrEmpty(NetSteps.Common.Configuration.ConfigurationManager.GetConfigValues(AvataxAPI.Constants.AVATAX_CONFIGSECTION, AvataxAPI.Constants.AVATAX_ACCOUNT).Trim()) && !string.IsNullOrEmpty(NetSteps.Common.Configuration.ConfigurationManager.GetConfigValues(AvataxAPI.Constants.AVATAX_CONFIGSECTION, AvataxAPI.Constants.AVATAX_LICENSE).Trim()))
                        {
                            OrderContext.Order.AsOrder().CancelTax();
                        }
                    }

                    #region Apply Credit - Clear Alocation
                    if (allowApplyCredit)
                    {
                        if (creditType)
                        {
                            if (isTotal)
                            {
                                ProcedimientoCuentaCorriente(originalOrderId, "C", 0, ApplicationContext.Instance.CurrentUserID);
                            }
                            else
                            {
                                ProcedimientoCuentaCorriente(originalOrderId, "P", creditAmountDecimal, ApplicationContext.Instance.CurrentUserID);
                            }

                            //ApplyCredit(OrderContext.Order.AsOrder().OrderCustomers[0].AccountID, creditAmountDecimal, OrderContext.Order.OrderID, OrderContext.Order.ParentOrderID);
                        }
                    }

                    if (allowClearAllocation)
                    {
                        ClearAllocation(OrderContext.Order.AsOrder());
                    }

                    if (allowReverseStatusActivity)
                    {
                        ReverseStatusActivity(OrderContext.Order.AsOrder().OrderCustomers[0].AccountID, OrderContext.Order.ParentOrderID.Value, PeriodBusinessLogic.Instance.GetOpenPeriodID());
                        OrderExtensions.UpdatePersonalIndicator(OrderContext.Order.OrderID, OrderContext.Order.OrderStatusID);
                    }
                    #endregion

                    #region Interfaz Orden Cancelación - SAP

                    //if (allowSAP)
                    //{
                    //    string XmlString = XmlGeneratorBusinessLogic.Instance.GenerateXmlForCancelledOrder(Server.MapPath(ConfigurationManager.AppSettings["TemplatesXML_Path"]), originalOrderId);
                    //    FileHelper.Save(XmlString, string.Format("{0}{1}CANCEL_{2}.xml", ConfigurationManager.AppSettings["FileUploadAbsolutePath"], ConfigurationManager.AppSettings["FileUploadPath_B010_Log"], originalOrderId));
                    //}
                    #endregion


                    /*CS.19JUL2016.Inicio.Comentado*/
                    #region Interfaz Orden Devolucion - SAP

                    //if (returnType != 3)/*CS.30AGO2016.Inicio.Si ReturnTypeID = 3 entonces no genera Interfaz. Refused: 3*/
                    //{
                    //    Order ParentOrder = returnOrder.ParentOrder;/*CS.09MAY2016:Comentado Por Antonio*/
                    //    Action<int> GenerarXmlRetorno = (PoriginalOrderId) =>
                    //    {
                    //        var fechaActual = DateTime.Now;
                    //        /*CS.11MAY2016.Inicio.Comentado*/
                    //        //string RutaUnica = "MLM_Pedidos_" + PoriginalOrderId.ToString() + "_" + fechaActual.Year.ToString() + fechaActual.Month.ToString() + fechaActual.Day.ToString() + fechaActual.Hour.ToString() + fechaActual.Minute.ToString() + fechaActual.Second.ToString();
                    //        //RutaUnica += ".XML";
                    //        /*CS.11MAY2016.Fin.Comentado*/

                    //        /*CS.11MAY2016.Inicio.Nueva Estructura:MLM_Pedidos_B010_YYMMDDHHMMSS.xml 
                    //            =>AA=   Año ultimos digitos (Ej.: 2016 = 16)
                    //            =>MM=   Mes (Ej: 05)
                    //            =>DD=   Dia (Ej: 21)
                    //            =>HH=   Hora
                    //            =>MM=   Minutos
                    //            =>SS=   Segundos
                    //         */
                    //        string formatoFecha = Right(fechaActual.Year.ToString(), 2) + Right(("00" + fechaActual.Month.ToString()), 2) + Right(("00" + fechaActual.Day.ToString()), 2) + Right(("00" + fechaActual.Hour.ToString()), 2) + Right(("00" + fechaActual.Minute.ToString()), 2) + Right(("00" + fechaActual.Second.ToString()), 2);
                    //        var RutaUnica = "MLM_Pedidos_B010_" + formatoFecha + ".xml";
                    //        /*CS.11MAY2016.Fin.Nueva Estructura*/

                    //        string ReturOrderItemDetaill = Server.MapPath(ConfigurationManager.AppSettings["TemplateOrderReturnItemDetaill"]);
                    //        string TemplateClientOrderReturn = Server.MapPath(ConfigurationManager.AppSettings["TemplateClientOrderReturn"]);
                    //        string MainRootServer = ConfigurationManager.AppSettings["MainRootServer"];
                    //        string SubDirectory = ConfigurationManager.AppSettings["SubDirectory"];

                    //        string data = XmlGeneratorBusinessLogic.Instance.GenerateXmlForReturnedOrder(TemplateClientOrderReturn, ReturOrderItemDetaill, PoriginalOrderId);

                    //        string rutaGuardar = Path.Combine(MainRootServer, SubDirectory, RutaUnica);
                    //        string RutaDiretorio = Path.Combine(MainRootServer, SubDirectory);
                    //        var ExistePath = Directory.Exists(RutaDiretorio);

                    //        if (ExistePath)
                    //        {
                    //            FileHelper.Save(data, rutaGuardar);
                    //        }
                    //    };

                    //    if (originalOrder != null)
                    //    {
                    //        /*CS.09MAY2016.Incio*/
                    //        switch (originalOrder.OrderStatusID)
                    //        /*CS.09MAY2016.Fin*/
                    //        //switch (orden.OrderStatusID)
                    //        {
                    //            case 8://Printed
                    //            case 20://Invoiced
                    //            case 21://Deliveried
                    //            case 9://Shipped
                    //            case 22://Embarking
                    //                /*CS.11MAY2016.Inicio.Comentado*/
                    //                //GenerarXmlRetorno(originalOrderId);
                    //                /*CS.11MAY2016.Fin.Comentado*/
                    //                /*CS.11MAY2016.Inicio.Enviar Nueva Orden Generada*/
                    //                GenerarXmlRetorno(returnOrder.OrderID);
                    //                /*CS.11MAY2016.Fin.Enviar Nueva Orden Generada*/
                    //                break;
                    //            default:
                    //                break;
                    //        }
                    //    }
                    //}
                    #endregion
                    /*CS.19JUL2016.Fin.Comentado*/

                    OrderContext.OriginalOrder = null;
                    OrderContext.Order = null;

                    return Json(new { result = true, returnOrderNumber = returnedOrderNumber });
                }
                else
                    return Json(new { result = false, message = message });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, orderID: (ReturnOrder != null) ? ReturnOrder.OrderID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public static string Right(string param, int length)
        {
            //start at the index based on the lenght of the sting minus
            //the specified lenght and assign it a variable
            string result = param.Substring(param.Length - length, length);
            //return the result of the operation
            return result;
        }

        [FunctionFilter("Orders-Return Order", "~/Orders")]
        public virtual ActionResult UpdateReturn(int originalOrderId, bool refundOriginalPayments, Dictionary<int, decimal> refundPayments, int returnType, string invoiceNotes, bool creditType, string creditAmount)
        {
            Order order = ReturnOrder;
            try
            {
                // Create the new return order with the items they selected
                decimal creditAmountDecimal = decimal.Parse(creditAmount, System.Globalization.NumberFormatInfo.InvariantInfo);
                var response = UpdateReturnOrder(originalOrderId, refundOriginalPayments, refundPayments, returnType, invoiceNotes, creditType, creditAmountDecimal, false);
                if (!response.Success)
                    return Json(new { result = false, message = response.Message });

                order = response.Item;
                OrderContext.Order.Save();

                return Json(new { result = true, returnOrderNumber = OrderContext.Order.AsOrder().OrderNumber });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, orderID: (ReturnOrder != null) ? ReturnOrder.OrderID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        /// <summary>
        /// Procede la orden a Pending confirm
        /// </summary>
        /// <param name="originalOrderId">Id de la Orden Padre</param>
        /// <param name="refundOriginalPayments">Monto de retorno</param>
        /// <param name="refundPayments">monto de pagos</param>
        /// <param name="returnType">tipo de retorno</param>
        /// <param name="invoiceNotes">Notas</param>
        /// <param name="returnedProducts">Productos retornados</param>
        /// <param name="idSupportTicket">Id ticket de soporte</param>
        /// <param name="idNationalMail">Mail nacional</param>
        /// <param name="productCreditAmount">Monto de Credito</param>
        /// <returns></returns>
        [FunctionFilter("Orders-Return Order", "~/Orders")]
        public virtual ActionResult SavePedingConfirm(int originalOrderId, bool refundOriginalPayments, Dictionary<int, decimal> refundPayments, int returnType, string invoiceNotes, List<ProductReturn> returnedProducts, int idSupportTicket, string idNationalMail, bool creditType, string creditAmount)
        {
            Order order = ReturnOrder;

            try
            {
                // Create the new return order with the items they selected
                decimal creditAmountDecimal = decimal.Parse(creditAmount, System.Globalization.NumberFormatInfo.InvariantInfo);
                var response = UpdateReturnOrder(originalOrderId, refundOriginalPayments, refundPayments, returnType, invoiceNotes, creditType, creditAmountDecimal, false);
                if (!response.Success)
                    return Json(new { result = false, message = response.Message });

                order = response.Item;
                OrderContext.Order.Save();

                //@at-005
                UpdateOrderItemReturn(OrderContext.Order.AsOrder(), returnedProducts);
                OrderItemReturnConfirmExtension.UpdOrderStatus(OrderContext.Order.AsOrder().OrderID, (Int32)Constants.OrderStatus.PendingConfirm, idSupportTicket, idNationalMail);
                string orderNumber = OrderContext.Order.AsOrder().OrderNumber;
                OrderContext.Clear();

                return Json(new { result = true, returnOrderNumber = orderNumber });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, orderID: (ReturnOrder != null) ? ReturnOrder.OrderID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        #region Helper Methods

        private ActionResult SetReturnOrdersAndViewData(Order originalOrder)
        {
            ActionResult result = null;

            ReturnOrders = GetAllChildOrders(originalOrder);

            var returnableProducts = FilterReturnableProducts(originalOrder);
            var previousPayments = RetrievePreviousReturnedPayments(ReturnOrders);
            var returnedItems = RetrievePreviousReturnedItems(ReturnOrders);

            ReturnableProducts = returnableProducts;
            ViewData["ReturnableProducts"] = returnableProducts;
            ViewData["PreviousReturnPayments"] = previousPayments;
            ViewData["ReturnedItems"] = returnedItems;

            /*CS.31JUL2016.Inicio*/
            OrderItemReturnRepository repositorio = new OrderItemReturnRepository();
            var listaOrderItem = repositorio.GetQuantityItemsValidReturnByParentOrderID(originalOrder.OrderID);
            ViewData["listaOrderItem"] = listaOrderItem;

            var listaOrderItemChildren = repositorio.GetOrderItemChildrenReturnByOrderID(originalOrder.OrderID);
            ViewData["listaOrderItemChildren"] = listaOrderItemChildren;
            /*CS.31JUL2016.Fin*/

            // If there's no more quantities left to return on original order
            if (AllItemsReturned(originalOrder))
            {
                TempData["Error"] = Translation.GetTerm("AllItemsHaveBeenReturnedOnThisOrder", "All Items have been returned on this order.");
                result = RedirectToAction("Index", "Details", new { id = originalOrder.OrderID });
            }
            else if (result.IsNull() && originalOrder.OrderCustomers.SelectMany(w => w.OrderItems).Where(oi => !oi.HasChildOrderItems).Count(oi => oi.Quantity > 0) == 0)
            {
                var allOriginalItems = GetAllOriginalItems();
                var unreturnedItems = UnReturnedItems(originalOrder);
                foreach (OrderItem parentItem in unreturnedItems)
                {
                    var unreturnedItemIds = unreturnedItems.Select(oi => oi.OrderItemID).ToList();
                    var currentParentItem = allOriginalItems.FirstOrDefault(oi => oi.OrderItemID == parentItem.OrderItemID);
                    if (currentParentItem != null)
                    {
                        foreach (OrderItem childItem in currentParentItem.ChildOrderItems)
                        {
                            var unreturnedChildItem = unreturnedItems.FirstOrDefault(oi => oi.OrderItemID == childItem.OrderItemID);
                            if (!unreturnedItemIds.Contains(childItem.OrderItemID) ||
                                (unreturnedChildItem != null && unreturnedChildItem.Quantity != childItem.Quantity))
                            {
                                TempData["Error"] = Translation.GetTerm("AllReturnableItemsHaveBeenReturnedOnThisOrder", "All returnable items have been returned on this order.");
                                result = RedirectToAction("Index", "Details", new { id = originalOrder.OrderID });
                            }
                        }
                    }
                }
            }

            ViewData["OriginalOrder"] = OrderContext.OriginalOrder;
            if (ReturnOrder != null)
            {
                ViewData["ReturnOrder"] = ReturnOrder;
            }

            return result;
        }

        protected virtual BasicResponseItem<Order> ValidateReturn(int originalOrderId, Order returnOrder)
        {
            BasicResponseItem<Order> response = new BasicResponseItem<Order>();
            response.Success = true;
            string message = string.Empty;
            Order originalOrder = OrderContext.OriginalOrder.IsNotNull() ? OrderContext.OriginalOrder.AsOrder() : Order.LoadFull(originalOrderId);

            //Cannot return more items than were purchased
            if (IncorrectNumOfOrderItemsForReturn(returnOrder, originalOrder))
            {
                message = Translation.GetTerm("CannotReturnMoreItemsThanTheNumberPurchased", "You cannot return more items than the number originally purchased.");
                return FailedResponseItem(response, message);
            }

            List<OrderItem> originalItems = originalOrder.OrderCustomers.SelectMany(oc => oc.OrderItems).ToList();
            List<OrderItemReturn> itemsBeingReturned = GetReturnOrderItems(returnOrder);
            List<int> originalItemIds = GetOriginalItemIDs(itemsBeingReturned);

            foreach (int originalItemId in originalItemIds)
            {
                var originalItem = originalItems.FirstOrDefault(oi => oi.OrderItemID == originalItemId);
                if (originalItem.IsNotNull())
                {
                    if (originalItem.HasChildOrderItems)
                    {
                        var childOrderItemIds = originalItem.ChildOrderItems.Select(c => c.OrderItemID).ToList();

                        //if (InvalidChildOrderItems(originalItem, itemsBeingReturned, childOrderItemIds, originalItemIds))
                        //{
                        //    if (originalItem.ProductID != null)
                        //    {
                        //        var product = Inventory.GetProduct(originalItem.ProductID.Value);
                        //        message = Translation.GetTerm("CannotReturnKitItemWithoutAllChildItems",
                        //            "You cannot return kit item ({0}) unless all associated items are also returned.",
                        //            originalItem.SKU + "-" + product.Translations.Name());
                        //    }

                        //    return FailedResponseItem(response, message);
                        //}

                        List<Order> existingReturnOrders = ReturnOrders;
                        foreach (var orderItem in GetOrderItems(existingReturnOrders))
                        {
                            if (IsAssociatedItemsAlreadyReturned(orderItem, childOrderItemIds))
                            {
                                if (originalItem.ProductID.HasValue && orderItem.ProductID.HasValue)
                                {
                                    var product = Inventory.GetProduct(originalItem.ProductID.Value);
                                    var childProduct = Inventory.GetProduct(orderItem.ProductID.Value);
                                    message = Translation.GetTerm("CannotReturnKitItemBecauseAnAssociatedItemHasAlreadyBeenReturned", "You cannot return kit item ({0}) when an associated item ({1}) has already been returned.", originalItem.SKU + "-" + product.Translations.Name(), orderItem.SKU + "-" + childProduct.Translations.Name());
                                }

                                return FailedResponseItem(response, message);
                            }
                        }
                    }
                    else if (originalItem.ParentOrderItem.IsNotNull())
                    {
                        var childOrderItemIds = originalItem.ParentOrderItem.ChildOrderItems.Select(c => c.OrderItemID).ToList();
                        if (!originalItemIds.Contains(originalItem.ParentOrderItemID.Value))
                        {
                            if (childOrderItemIds.All(cId => originalItemIds.Contains(cId))
                                && originalItem.ParentOrderItem.ChildOrderItems.Sum(c => c.Quantity) ==
                                    itemsBeingReturned.Where(r => childOrderItemIds.Contains(r.OriginalOrderItemID.Value)).Sum(r => r.Quantity))
                            {
                                if (originalItem.ParentOrderItem.ProductID.HasValue)
                                {
                                    var product = Inventory.GetProduct(originalItem.ParentOrderItem.ProductID.Value);
                                    message = Translation.GetTerm("CannotReturnAllChildItemsWithoutParentItem", "You cannot return all items associated with a kit unless the kit item ({0}) is also returned.", originalItem.ParentOrderItem.SKU + "-" + product.Translations.Name());
                                }

                                return FailedResponseItem(response, message);
                            }
                        }
                    }
                }
            }

            return response;
        }

        protected virtual BasicResponseItem<Order> ValidateReturnExtended(int originalOrderId, Order returnOrder)
        {
            this._orderContext = Create.New<IReturnOrderContext>();
            OrderContext.OriginalOrder = Order.LoadFull(originalOrderId);
            return ValidateReturn(originalOrderId, returnOrder);
        }

        private void UpdateReturnItems(bool overridenShipping, decimal restockingFee, List<ProductReturn> returnedProducts, Order originalOrder, Order returnOrder)
        {
            OrderContext.Order = returnOrder;
            RemoveUncheckedItems(OrderContext.Order.AsOrder(), originalOrder, returnedProducts ?? new List<ProductReturn>());
            AddReturnedItemsToReturnOrder(returnedProducts, OrderContext.Order.AsOrder(), originalOrder);

            //only run calculations if there are items on the return otherwise we get initial bad values - Scott Wilson
            if (returnOrder.OrderCustomers.Any(x => x.OrderItems.Count > 0))
            {
                CalculateRestockingFee(restockingFee, OrderContext.Order.AsOrder());
                CalculateReturnOrderShippingTotal(overridenShipping, originalOrder, OrderContext.Order.AsOrder());
                OrderService.UpdateOrder(OrderContext);
            }
        }

        private List<OrderProductAmount> FilterReturnableProducts(Order originalOrder)
        {
            var returnableProducts = new List<OrderProductAmount>();
            foreach (OrderItem item in OrderContext.OriginalOrder.OrderCustomers.SelectMany(w => w.OrderItems))
            {
                if (!returnableProducts.Select(p => p.OrderItemID).Contains(item.OrderItemID))
                    returnableProducts.Add(new OrderProductAmount()
                    {
                        OrderID = OrderContext.OriginalOrder.OrderID,
                        ProductID = item.ProductID.ToInt(),
                        Quantity = item.Quantity,
                        OrderItemID = item.OrderItemID
                    });
            }

            List<Order> returnOrders = ReturnOrders;
            List<int> bundleIdsToRemove = new List<int>();
            if (returnOrders.Count > 0)
            {
                foreach (OrderItem item in originalOrder.OrderCustomers.SelectMany(w => w.OrderItems))
                {
                    foreach (Order existingReturn in returnOrders)
                    {
                        if (existingReturn.OrderStatusID == Constants.OrderStatus.Cancelled.ToInt())
                            continue;

                        OrderItem existingItem = null;
                        if (existingReturn.OrderCustomers != null && existingReturn.OrderCustomers.Count > 0)
                        {
                            foreach (OrderItem returnItem in existingReturn.OrderCustomers.SelectMany(w => w.OrderItems))
                            {
                                if (returnItem.OrderItemReturns.FirstOrDefault(oi => oi.OriginalOrderItemID == item.OrderItemID) != null)
                                {
                                    existingItem = returnItem;
                                    break;
                                }
                            }
                        }

                        if (existingItem == null)
                            continue;

                        item.Quantity = item.Quantity + existingItem.Quantity; //return order quantities are negative
                        var returnableProduct = returnableProducts.FirstOrDefault(p => p.OrderItemID == item.OrderItemID);
                        returnableProduct.Quantity = item.Quantity;
                    }
                }

                foreach (OrderItem orderItem in originalOrder.OrderCustomers.SelectMany(oc => oc.OrderItems))
                {
                    if (orderItem.ChildOrderItems.Count == 0)
                        continue;

                    bool allItemsReturned = true;
                    foreach (OrderItem childOrderItem in orderItem.ChildOrderItems)
                    {
                        if (returnableProducts.Any(rp => rp.OrderItemID == childOrderItem.OrderItemID && rp.Quantity > 0))
                        {
                            allItemsReturned = false;
                            break;
                        }
                    }

                    if (allItemsReturned)
                    {
                        returnableProducts.Where(rp => rp.OrderItemID == orderItem.OrderItemID).FirstOrDefault().Quantity = 0;
                    }
                }
            }
            //check to see if there are any bundles that do not have child items.

            return returnableProducts;
        }

        private List<OrderItem> RetrievePreviousReturnedItems(List<Order> returnOrders)
        {
            List<OrderItem> previouslyReturnedItems = new List<OrderItem>();
            foreach (OrderCustomer orderCustomer in returnOrders.SelectMany(returnOrder => returnOrder.OrderCustomers))
            {
                decimal balance = orderCustomer.Balance.HasValue ? orderCustomer.Balance.Value : 0;
                //Some items are created on a return like restockingfee
                //Check to see if this item was actually a return by looking at it's orderitemreturns value
                foreach (OrderItem orderItem in orderCustomer.OrderItems.Where(oi => oi.OrderItemReturns != null && oi.OrderItemReturns.Any()))
                {
                    orderItem.ReturnedPrice = balance < orderItem.GetAdjustedPrice() ? balance : orderItem.GetAdjustedPrice();
                    balance -= orderItem.ReturnedPrice;
                    previouslyReturnedItems.Add(orderItem);
                }
            }

            return previouslyReturnedItems;
        }

        private List<PreviousReturnOrderPayment> RetrievePreviousReturnedPayments(List<Order> ReturnOrders)
        {
            List<PreviousReturnOrderPayment> payments = new List<PreviousReturnOrderPayment>();
            foreach (OrderPayment orderPayment in ReturnOrders.SelectMany(ro => ro.OrderPayments))
            {
                PreviousReturnOrderPayment previousPayment = payments.FirstOrDefault(p => p.AccountNumber == orderPayment.MaskedAccountNumber);
                if (previousPayment == null)
                {
                    payments.Add(new PreviousReturnOrderPayment() { AccountNumber = orderPayment.MaskedAccountNumber, Amount = orderPayment.Amount });
                }
                else
                {
                    previousPayment.Amount += orderPayment.Amount;
                }
            }

            return payments;
        }

        private List<Order> GetAllChildOrders(Order originalOrder)
        {
            // Return orders containing only non-shippable items will be marked as shipped.
            return Order.LoadChildOrdersFull(originalOrder.OrderID, Constants.OrderType.ReturnOrder.ToInt())
                .Where(o => o.OrderStatusID == (int)Constants.OrderStatus.Paid
                    || o.OrderStatusID == (int)Constants.OrderStatus.Shipped).Select(o => (Order)o).ToList();
        }

        private List<OrderItem> GetAllOriginalItems()
        {
            return OrderContext.OriginalOrder.AsOrder().OrderCustomers.SelectMany(w => w.OrderItems).ToList();
        }

        private List<OrderItem> UnReturnedItems(Order originalOrder)
        {
            return originalOrder.OrderCustomers.SelectMany(w => w.OrderItems).Where(oi => oi.Quantity > 0).ToList();
        }

        private bool AllItemsReturned(Order originalOrder)
        {
            return originalOrder.OrderCustomers.SelectMany(w => w.OrderItems).Count(oi => oi.Quantity > 0) == 0;
        }

        private bool IncorrectNumOfOrderItemsForReturn(Order returnOrder, Order originalOrder)
        {
            return NumOfOrderItems(returnOrder) > NumOfOrderItems(originalOrder);
        }

        private int NumOfOrderItems(Order order)
        {
            return order.OrderCustomers.SelectMany(x => x.OrderItems).Where(oi => !oi.SKU.Equals(this.RestockingFeeSku, StringComparison.OrdinalIgnoreCase)).Sum(oi => oi.Quantity);
        }

        private List<OrderItemReturn> GetReturnOrderItems(Order returnOrder)
        {
            return returnOrder.OrderCustomers.SelectMany(x => x.OrderItems.SelectMany(o => o.OrderItemReturns)).ToList();
        }

        private List<int> GetOriginalItemIDs(IEnumerable<OrderItemReturn> returnItems)
        {
            return returnItems.Where(x => x.OriginalOrderItemID.HasValue).Select(x => x.OriginalOrderItemID.Value).ToList();
        }

        private bool InvalidChildOrderItems(OrderItem item, List<OrderItemReturn> returnItems, List<int> childItemIDs, IEnumerable<int> originalItemIDs)
        {
            return !OriginalAndChildItemsMatch(childItemIDs, originalItemIDs); //|| InvalidReturnItemQuantity(item, returnItems, childItemIDs);
        }

        private bool OriginalAndChildItemsMatch(IEnumerable<int> childItemIDs, IEnumerable<int> originalItemIDs)
        {
            return childItemIDs.All(cId => originalItemIDs.Contains(cId));
        }

        private bool InvalidReturnItemQuantity(OrderItem item, IEnumerable<OrderItemReturn> returnItems, List<int> childItemIDs)
        {
            return item.ChildOrderItems.Sum(c => c.Quantity) !=
                returnItems.Where(x => x.OriginalOrderItemID.HasValue && childItemIDs.Contains(x.OriginalOrderItemID.Value)).Sum(x => x.Quantity);
        }

        private IEnumerable<OrderItem> GetOrderItems(List<Order> existingReturnOrders)
        {
            return from existingReturnOrder in existingReturnOrders
                   from customer in existingReturnOrder.OrderCustomers
                   from orderItem in customer.OrderItems
                   select orderItem;
        }

        private bool IsAssociatedItemsAlreadyReturned(OrderItem orderItem, IEnumerable<int> chiltItemIDs)
        {
            return orderItem.OrderItemReturns.Any(ori => ori.OriginalOrderItemID != null && chiltItemIDs.Contains(ori.OriginalOrderItemID.Value));
        }

        private BasicResponseItem<Order> FailedResponseItem(BasicResponseItem<Order> response, string failureMessage)
        {
            response.Success = false;
            response.Message = failureMessage;
            return response;
        }

        private void CalculateReturnOrderShippingTotal(bool overridenShipping, Order originalOrder, Order returnOrder)
        {
            decimal? shippingTotal = null;
            decimal availableShipping = CalculateReturnableShipping(originalOrder);
            if (!overridenShipping)
            {
                if (ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.Orders_Returns_RefundPercentOfShipping))
                {
                    shippingTotal = returnOrder.ShippingTotal = originalOrder.ShippingTotal * (returnOrder.Subtotal / originalOrder.Subtotal);
                }
                else
                {
                    shippingTotal = returnOrder.ShippingTotal = availableShipping;
                }
            }
            returnOrder.ShippingTotalOverride = (shippingTotal.HasValue && (shippingTotal.Value > availableShipping)) ? availableShipping : shippingTotal;
        }

        private Order GetOriginalOrder(int originalOrderId)
        {
            Order originalOrder;
            if (OrderContext.OriginalOrder != null && originalOrderId == OrderContext.OriginalOrder.OrderID)
            {
                originalOrder = OrderContext.OriginalOrder.AsOrder();
            }
            else
            {
                originalOrder = Order.LoadFull(originalOrderId);
                OrderContext.OriginalOrder = originalOrder;
            }

            return originalOrder;
        }

        private decimal CalculateReturnableShipping(Order originalOrder)
        {
            //deduct the amount of previously returned shipping from the total shipping on the order - Scott Wilson
            decimal totalAvailableShipping = originalOrder.ShippingTotal.HasValue ? originalOrder.ShippingTotal.Value : 0.0M;
            decimal previouslyReturnedShipping = 0.0M;
            foreach (Order o in ReturnOrders)
            {
                previouslyReturnedShipping += o.ShippingTotalOverride.HasValue ? Math.Abs(o.ShippingTotalOverride.Value) : 0.0M; //the shipping total on a return is in the negative
            }

            return totalAvailableShipping - previouslyReturnedShipping;
        }

        private void CalculateRestockingFee(decimal restockingFee, Order returnOrder)
        {
            using (var calculateRestockingFeeTrace = this.TraceActivity("ReturnController::CalculateRestockingFee"))
            {
                if (string.IsNullOrEmpty(this.RestockingFeeSku))
                {
                    this.TraceEvent("RestockingFeeSKU was null or empty, returning");
                    return;
                }

                // Check for the fee already existing on this return order.
                OrderItem restockingItem = null;
                foreach (OrderCustomer orderCustomer in returnOrder.OrderCustomers.Where(x => x.OrderItems.Count > 0))
                {
                    restockingItem = orderCustomer.OrderItems.GetOrderItemBySku(this.RestockingFeeSku);
                    if (restockingItem != null)
                    {
                        this.TraceEvent("found existing restockingItem");
                        break;
                    }
                }

                if (restockingFee > 0)
                {
                    this.TraceEvent(string.Format("using restockingFee of {0}", restockingFee));

                    var amount = -1 * restockingFee;
                    if (restockingItem != null)
                    {
                        //TODO: Restocking fees should be an order adjustment on the order item (set the discount amount).
                        if (restockingItem.GetAdjustedPrice() != amount)
                        {
                            this.TraceEvent(string.Format("updating existing restockingFee to {0}", amount));
                            restockingItem.ItemPriceActual = amount;
                        }
                        else
                        {
                            this.TraceEvent("existing restockingItem matches restockingFee, did not update");
                        }
                    }
                    else
                    {
                        this.TraceEvent("adding new restockingItem to order");

                        var addedRestockItem = returnOrder.AddItem(returnOrder.OrderCustomers.First(x => x.OrderItems.Count > 0), this.RestockingFeeProduct, 1, Constants.OrderItemType.Fees, amount);
                        if (addedRestockItem.GetAdjustedPrice() != amount)
                        {
                            this.TraceEvent(string.Format("setting restockingItem fee to {0}", amount));
                            ((OrderItem)addedRestockItem).ItemPriceActual = amount;
                        }
                    }
                }
                else if (restockingItem != null)
                {
                    // Remove existing restocking fee if there is one
                    this.TraceEvent("removing existing restockingItem");

                    returnOrder.RemoveItem(restockingItem.OrderCustomer, restockingItem.Guid.ToString("N"));
                }

                this.TraceEvent("recalculating totals");
                OrderContext.Order = returnOrder;
                OrderService.UpdateOrder(OrderContext);
            }
        }

        private void RemoveUncheckedItems(Order returnOrder, Order originalOrder, IEnumerable<ProductReturn> returningItems)
        {
            foreach (var orderItem in returnOrder.OrderCustomers.SelectMany(oc => oc.OrderItems).ToList())
            {
                if (returningItems.FirstOrDefault(ri => orderItem.OrderItemReturns.Count > 0 && ri.OrderItemID == orderItem.OrderItemReturns[0].OriginalOrderItemID) == null && orderItem.OrderItemReturns != null
                    && orderItem.OrderItemReturns.Count > 0)
                {
                    foreach (var orderItemReturn in orderItem.OrderItemReturns.ToList())
                    {
                        if (orderItemReturn.ChangeTracker.State != ObjectState.Added)
                            orderItemReturn.MarkAsDeleted();
                        orderItem.OrderItemReturns.Remove(orderItemReturn);
                        if (orderItem.OrderCustomer != null)
                        {
                            returnOrder.RemoveItem(orderItem.OrderCustomer, orderItem);
                        }
                        else
                        {
                            returnOrder.RemoveItem(orderItem);
                        }
                    }
                }
                OrderContext.Order = returnOrder;
                OrderService.UpdateOrder(OrderContext);
            }
        }

        private void AddReturnedItemsToReturnOrder(IEnumerable<ProductReturn> returnedProducts, Order returnOrder, Order originalOrder)
        {
            if (returnedProducts == null)
                return;

            returnedProducts = returnedProducts.Where(p => p.OrderItemID > 0).ToList();

            //get a list of all the items being returned for each order customer
            foreach (var originalOrderCustomer in originalOrder.OrderCustomers)
            {
                var parentReturnedProducts = originalOrderCustomer.OrderItems.Select(oi => returnedProducts.FirstOrDefault(rp => rp.OrderItemID == oi.OrderItemID));
                var customer = originalOrderCustomer;
                var orphanReturnedProducts = returnedProducts.Where(rp => rp.OrderCustomerID == customer.OrderCustomerID);
                OrderCustomer returnOrderCustomer = null;
                if (returnedProducts.Any(rp => originalOrderCustomer.OrderItems.Any(oi => oi.OrderItemID == rp.OrderItemID)))
                {
                    if (!returnOrder.OrderCustomers.Any(oc => oc.AccountID == originalOrderCustomer.AccountID))
                    {
                        // we need to create a new return order customer
                        returnOrderCustomer = returnOrder.AddNewCustomer(originalOrderCustomer.AccountID);
                        //Reference the original order customer's sale tax transaction number for tax needs
                        returnOrderCustomer.SalesTaxTransactionNumber = originalOrderCustomer.SalesTaxTransactionNumber;
                    }
                    else
                    {
                        // we already have a return customer for this customer.
                        returnOrderCustomer = returnOrder.OrderCustomers.FirstOrDefault(oc => oc.AccountID == originalOrderCustomer.AccountID);
                    }
                }

                foreach (ProductReturn returnedProduct in parentReturnedProducts)
                {
                    if (returnedProduct == null)
                        continue;

                    OrderItem originalOrderItem = originalOrderCustomer.OrderItems.FirstOrDefault(oi => oi.OrderItemID == returnedProduct.OrderItemID);
                    var returnedItem = ReturnedOrderItem(returnOrderCustomer, returnedProduct);
                    // we need to add the return product to the return order customer
                    returnedItem = AddReturnedItem(returnOrder, originalOrderItem, returnedItem, returnedProduct, returnOrderCustomer);
                    ProductReturn product = returnedProduct;
                    foreach (ProductReturn childReturnedProduct in returnedProducts.Where(p => p.ParentOrderItemID == product.OrderItemID))
                    {
                        OrderItem childOrderItem = originalOrderCustomer.OrderItems.FirstOrDefault(oi => oi.OrderItemID == childReturnedProduct.OrderItemID);
                        OrderItem childReturnedItem = ReturnedOrderItem(returnOrderCustomer, childReturnedProduct);
                        AddReturnedItem(returnOrder, childOrderItem, childReturnedItem, childReturnedProduct,
                            returnOrderCustomer, returnedItem.Guid.ToString("N"),
                            childReturnedProduct.DynamicKitGroupID);
                    }
                }

                foreach (ProductReturn orphanReturnedProduct in orphanReturnedProducts)
                {
                    OrderItem orderItem =
                        originalOrder.OrderCustomers.SelectMany(w => w.OrderItems).FirstOrDefault(
                            oi => oi.OrderItemID == orphanReturnedProduct.OrderItemID);

                    OrderItem returnedItem = ReturnedOrderItem(returnOrderCustomer, orphanReturnedProduct);
                    AddReturnedItem(returnOrder, orderItem, returnedItem, orphanReturnedProduct, originalOrderCustomer);
                }

                OrderContext.Order = returnOrder;
            }
        }

        private OrderItem ReturnedOrderItem(OrderCustomer returnOrderCustomer, ProductReturn returnedProduct)
        {
            return returnOrderCustomer.OrderItems.Where(x => x.OrderItemReturns.Any())
                .FirstOrDefault(oi => oi.OrderItemReturns.Any(x => x.OriginalOrderItemID == returnedProduct.OrderItemID));
        }

        private OrderItem AddReturnedItem(Order returnOrder, OrderItem originalOrderItem, OrderItem returnedItem, ProductReturn returnedProduct, OrderCustomer returnOrderCustomer, string parentGuid = null, int? dynamicKitGroupId = null)
        {
            var returnableProduct = this.ReturnableProducts.FirstOrDefault(p => p.OrderItemID == returnedProduct.OrderItemID);

            if (returnableProduct != null)
            {
                if (returnedProduct.QuantityReturned > returnableProduct.Quantity)
                {
                    returnedProduct.QuantityReturned = returnableProduct.Quantity;
                }
            }

            var returnItemCV = returnedProduct.ReturnItemCV * returnedProduct.QuantityReturned;

            if (returnedItem == null)
            {
                var product = Inventory.GetProduct(originalOrderItem.ProductID.ToInt());

                returnedItem = (OrderItem)returnOrder.AddItem(
                    returnOrderCustomer,
                    product,
                    returnedProduct.QuantityReturned,
                    (Constants.OrderItemType)originalOrderItem.OrderItemTypeID,
                    returnedProduct.ReturnItemPrice,
                    returnItemCV,
                    false,
                    originalOrderItem.HostessRewardRuleID,
                    parentGuid,
                    dynamicKitGroupId);

                foreach (var oipO in originalOrderItem.OrderItemPrices)
                {
                    var orderItemPrice = returnedItem.OrderItemPrices.SingleOrDefault(oip => oip.ProductPriceTypeID == oipO.ProductPriceTypeID);
                    if (orderItemPrice != null)
                    {
                        orderItemPrice.OriginalUnitPrice = oipO.OriginalUnitPrice;
                        orderItemPrice.UnitPrice = oipO.UnitPrice;

                    }
                    else
                    {
                        orderItemPrice = new OrderItemPrice();
                        orderItemPrice.ProductPriceTypeID = oipO.ProductPriceTypeID;
                        returnedItem.OrderItemPrices.Add(orderItemPrice);
                        orderItemPrice.OriginalUnitPrice = oipO.OriginalUnitPrice;
                        orderItemPrice.UnitPrice = oipO.UnitPrice;
                    }
                }

                // this should not be in the controller... but we have no structure in place to handle returns well.
                // In any case, the return original price should match the adjusted price for the original item.
                // ...
                // Pricing on the returned item is handled by "overriding" the price on the item.
                // The returnedItem.ItemPriceActual gets set below in the call to returnOrder.UpdateItem.
                // We set the ItemPrice to the current price for the product to avoid problems
                // when the price has changed since the original order.
                // (The item's ItemPriceActual gets un-overridden if it is the same amount as the ItemPrice.
                // Which should be OK if the price hasn't changed.)
                var originalPriceTypeID = originalOrderItem.ProductPriceTypeID ?? originalOrderItem.OrderCustomer.ProductPriceTypeID;
                returnedItem.SetItemPrice(originalPriceTypeID);
                var pricingService = Create.New<IProductPricingService>();
                returnedItem.ItemPrice = pricingService.GetPrice(
                    originalOrderItem.ProductID ?? 0,
                    originalPriceTypeID,
                    originalOrderItem.OrderCustomer.Order.CurrencyID);

                OrderItemReturn itemReturnDetail = new OrderItemReturn()
                {
                    OrderItemID = originalOrderItem.OrderItemID,
                    ReturnReasonID = returnedProduct.ReturnReasonID,
                    IsRestocked = returnedProduct.IsRestockable,
                    HasBeenReceived = returnedProduct.HasBeenReceived,
                    Quantity = returnedProduct.QuantityReturned,
                    OriginalOrderItemID = originalOrderItem.OrderItemID
                };

                returnedItem.OrderItemReturns.Add(itemReturnDetail);
                returnOrder.UpdateItem(returnOrderCustomer, returnedItem, returnedProduct.QuantityReturned, returnedProduct.ReturnItemPrice, returnItemCV);
            }
            else
            {
                OrderItemReturn itemReturnDetail = returnedItem.OrderItemReturns.FirstOrDefault(oi => oi.OrderItemID == returnedItem.OrderItemID);
                if (itemReturnDetail == null)
                {
                    itemReturnDetail = OrderItemReturn.LoadByOrderItemID(returnedItem.OrderItemID);
                }
                if (itemReturnDetail == null)
                {
                    itemReturnDetail = new OrderItemReturn();
                    itemReturnDetail.StartEntityTracking();
                }

                if (returnOrder.OrderID == 0)
                {
                    returnOrder.UpdateItem(returnOrderCustomer, returnedItem, returnedProduct.QuantityReturned, returnedProduct.ReturnItemPrice, returnItemCV);
                    itemReturnDetail.Quantity = returnedProduct.QuantityReturned;
                }
                else
                {
                    returnOrder.UpdateItem(returnOrderCustomer, returnedItem, returnedProduct.QuantityReturned, returnedProduct.ReturnItemPrice, returnItemCV);
                    itemReturnDetail.Quantity = returnedProduct.QuantityReturned;
                }

                itemReturnDetail.ReturnReasonID = returnedProduct.ReturnReasonID;
                itemReturnDetail.IsRestocked = returnedProduct.IsRestockable;
                itemReturnDetail.HasBeenReceived = returnedProduct.HasBeenReceived;
            }

            // since we're using price overrides from the original order, clear all discount fields on hostess reward items on the return order
            if (returnedItem.IsHostReward)
            {
                returnedItem.Discount = 0m;
                returnedItem.DiscountPercent = 0m;
            }

            OrderContext.Order = returnOrder;

            return returnedItem;
        }

        /// <summary>
        /// Helper method used for saving and submitting the order. - JHE
        /// </summary>
        /// <param name="originalOrderId"></param>
        /// <param name="refundOriginalPayments"></param>
        /// <param name="refundPayments"></param>
        /// <param name="returnType"></param>
        /// <param name="invoiceNotes"></param>
        /// <returns></returns>
        protected virtual BasicResponseItem<Order> UpdateReturnOrder(int originalOrderId, bool refundOriginalPayments, Dictionary<int, decimal> refundPayments, int returnType, string invoiceNotes, bool creditType, decimal creditAmount, bool finishOrder = false)
        {
            BasicResponseItem<Order> response = new BasicResponseItem<Order>();
            response.Success = false;

            // Create the new return order with the items they selected
            Order returnOrder = ReturnOrder;
            returnOrder.StartEntityTracking();
            returnOrder.ReturnTypeID = returnType;
            try
            {
                Order originalOrder = OrderContext.OriginalOrder != null ? OrderContext.OriginalOrder.AsOrder() : Order.LoadFull(originalOrderId);

                int cantidadItem = returnOrder.OrderCustomers.SelectMany(oc => oc.OrderItems).Count();
                int cantidadItemParents = returnOrder.OrderCustomers.SelectMany(oc => oc.ParentOrderItems).Count();
                if (returnOrder == null || returnOrder.OrderCustomers == null || returnOrder.OrderCustomers.Count == 0 || (cantidadItem == 0 && cantidadItemParents == 0))//returnOrder.OrderCustomers.SelectMany(oc => oc.OrderItems).Count() == 0)
                {
                    response.Success = false;
                    response.Message = Translation.GetTerm("NoItemsHaveBeenSelectedToReturn", "No items have been selected to return.");

                    return response;
                }
                response.Item = returnOrder;

                // Remove items not being returned - JHE
                foreach (OrderCustomer orderCustomer in returnOrder.OrderCustomers)
                {
                    foreach (var orderItem in orderCustomer.OrderItems.Where(oi => oi.SKU != this.RestockingFeeSku).ToList())
                    {
                        OrderItemReturn itemReturnDetail = null;
                        if (orderItem.OrderItemReturns != null)
                        {
                            itemReturnDetail = orderItem.OrderItemReturns.FirstOrDefault();
                        }
                        if (itemReturnDetail == null)
                        {
                            itemReturnDetail = OrderItemReturn.LoadByOrderItemID(orderItem.OrderItemID);
                        }
                        if (itemReturnDetail == null)
                        {
                            if (orderItem.ChangeTracker.State != ObjectState.Added)
                            {
                                orderItem.MarkAsDeleted();
                            }
                            orderCustomer.OrderItems.Remove(orderItem);
                        }
                    }
                }

                if (finishOrder)
                {
                    bool allItemsHaveBeenReceived = true;
                    foreach (OrderCustomer orderCustomer in returnOrder.OrderCustomers)
                    {
                        allItemsHaveBeenReceived = orderCustomer.OrderItems.Where(oi => oi.SKU != this.RestockingFeeSku).All(oi => oi.OrderItemReturns.All(oir => oir.HasBeenReceived));
                        if (!allItemsHaveBeenReceived)
                            break;
                    }

                    if (!allItemsHaveBeenReceived)
                    {
                        response.Success = false;
                        response.Message = Translation.GetTerm("NotAllItemsHaveBeenReceived", "Not all Items have been received.");

                        return response;
                    }
                }

                if (!invoiceNotes.ToCleanString().IsNullOrEmpty())
                {
                    returnOrder.InvoiceNotes = invoiceNotes;
                }

                if (refundPayments != null)
                {
                    foreach (OrderPayment orderPayment in originalOrder.OrderPayments.Where(op => op.OrderCustomerID == null && op.OrderPaymentStatusID == Convert.ToInt32(Constants.OrderPaymentStatus.Completed)))
                    {
                        KeyValuePair<int, decimal> refundedPayment = refundPayments.FirstOrDefault(rp => rp.Key == orderPayment.OrderPaymentID);

                        if (refundedPayment.Key == 0)
                            continue;

                        OrderPayment newPayment = new OrderPayment
                        {
                            OrderID = returnOrder.OrderID,
                            CurrencyID = returnOrder.CurrencyID,
                            TransactionID = orderPayment.TransactionID,                            
                        };

                        newPayment = CreateOrderPayment(orderPayment, newPayment, refundedPayment.Value);
                        returnOrder.ApplyPaymentToOrder(newPayment, refundedPayment.Value);
                    }
                }

                #region Add the payments to refund

                // Add the payments to refund
                if ((Constants.OrderType)returnOrder.OrderTypeID == Constants.OrderType.ReturnOrder && returnOrder.GrandTotal == 0)
                {
                    //No payments will be added
                }
                else
                {
                    foreach (OrderCustomer customer in returnOrder.OrderCustomers)
                    {
                        customer.RemoveAllPayments();
                        if (refundOriginalPayments)
                        {


                            if (refundPayments == null || refundPayments.Count == 0)
                            {
                                response.Success = false;
                                response.Message = Translation.GetTerm("ThereAreNoPaymentsToReturnPaymentTo", "There are no payment(s) to return payment to.");
                                return response;
                            }

                            foreach (KeyValuePair<int, decimal> payment in refundPayments)
                            {
                                if (payment.Value > 0 && originalOrder.OrderCustomers.First(oc => oc.AccountID == customer.AccountID).OrderPayments.Any(op => op.OrderPaymentID == payment.Key && op.OrderPaymentStatusID == Convert.ToInt32(Constants.OrderPaymentStatus.Completed)))
                                {
                                    OrderPayment orderCustomerOrderPayment = customer.AddNewPayment(Constants.PaymentType.CreditCard);
                                    orderCustomerOrderPayment.OrderID = returnOrder.OrderID;
                                    orderCustomerOrderPayment.CurrencyID = returnOrder.CurrencyID;

                                    OrderPayment originalPayment =
                                        originalOrder.OrderPayments.FirstOrDefault(
                                              op => op.OrderPaymentID == payment.Key && op.OrderPaymentStatusID == Convert.ToInt32(Constants.OrderPaymentStatus.Completed));
                                    if (originalPayment == null)
                                    {
                                        response.Success = false;
                                        response.Message = Translation.GetTerm("PaymentNotFoundOnExistingOrder", "Payment not found on existing order.");
                                        return response;
                                    }
                                    if (payment.Value > originalPayment.Amount)
                                    {
                                        response.Message =
                                            Translation.GetTerm(
                                                "TheRefundAmountCanNotBeGreaterThantheOriginalAmountCharged",
                                                "The Refund amount ({0}) can not be greater than the original amount charged ({1}) on card ({2}).",
                                                payment.Value.ToMoneyString(), originalPayment.Amount.ToMoneyString(),
                                                originalPayment.MaskedAccountNumber);
                                        response.Success = false;
                                        return response;
                                    }

                                    orderCustomerOrderPayment = CreateOrderPayment(originalPayment, orderCustomerOrderPayment, payment.Value);
                                    orderCustomerOrderPayment.TransactionID = originalPayment.TransactionID;
                                }
                            }
                        }
                        else if (creditType)//@at-005 OrderPaymentCreditType
                        {
                            OrderPayment orderPayment = customer.AddNewPayment(Constants.PaymentType.ProductCredit);
                            orderPayment.NameOnCard = Translation.GetTerm("ProductCredit", "Product Credit");
                            orderPayment.Amount = creditAmount;
                            orderPayment.OrderID = returnOrder.OrderID;
                            orderPayment.CurrencyID = returnOrder.CurrencyID;
                            orderPayment.BillingPostalCode = string.Empty;
                        }
                        else
                        {
                            OrderPayment orderPayment = customer.AddNewPayment(Constants.PaymentType.Check);
                            orderPayment.NameOnCard = Translation.GetTerm("ManualRefund", "Manual Refund");
                            orderPayment.Amount = returnOrder.GrandTotal.ToDecimal();
                            orderPayment.OrderID = returnOrder.OrderID;
                            orderPayment.CurrencyID = returnOrder.CurrencyID;
                            orderPayment.BillingPostalCode = string.Empty;
                        }
                    }
                }

                #endregion

                var queryOrderCustomer = returnOrder.OrderCustomers.FirstOrDefault();
                if (queryOrderCustomer != null) queryOrderCustomer.OrderPayments.Each(l => l.ExpirationStatusID = ConstantsGenerated.ExpirationStatuses.Expired.ToInt());
                OrderContext.Order = returnOrder;
                OrderService.UpdateOrder(OrderContext);

                var totalOriginalOrderPaymentAmount = originalOrder.OrderPayments.Where(op => op.OrderPaymentStatusID == (int)Constants.OrderPaymentStatus.Completed).Sum(op => op.Amount);
                var totalnewOrderPaymentAmount = OrderContext.Order.AsOrder().OrderPayments.Sum(op => op.Amount);
                //if (totalnewOrderPaymentAmount > totalOriginalOrderPaymentAmount)
                //{
                //    response.Message = Translation.GetTerm("TotalRefundAmountCanNotBeGreaterThantheOriginalAmountCharged", "Total Refund amount ({0}) can not be greater than the original amount charged ({1}).", totalnewOrderPaymentAmount.ToMoneyString(), totalOriginalOrderPaymentAmount.ToMoneyString());
                //    response.Success = false;
                //    return response;
                //}
                var totalexistingreturnOrdersPaymentAmount = OrderContext.Order.AsOrder().OrderPayments.Where(op => op.OrderPaymentStatusID == (int)Constants.OrderPaymentStatus.Completed).Sum(op => op.Amount);
                var totalRefundAmount = totalnewOrderPaymentAmount + totalexistingreturnOrdersPaymentAmount;
                //if (totalRefundAmount > totalOriginalOrderPaymentAmount)
                //{
                //    response.Message = Translation.GetTerm("TotalRefundAmountFromAllReturnOrdersCanNotBeGreaterThantheOriginalAmountCharged", "Total Refund amount from all return orders ({0}) can not be greater than the original amount charged ({1}).", totalRefundAmount.ToMoneyString(), totalOriginalOrderPaymentAmount.ToMoneyString());
                //    response.Success = false;
                //    return response;
                //}
                response.Success = true;
                return response;
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, orderID: (ReturnOrder != null) ? ReturnOrder.OrderID.ToIntNullable() : null);
                response.Success = false;
                response.Message = exception.PublicMessage;
                return response;
            }
            finally
            {
                ViewData["ReturnOrder"] = OrderContext.Order.AsOrder();
            }
        }

        private OrderPayment CreateOrderPayment(OrderPayment originalPayment, OrderPayment orderPayment, decimal paymentAmount)
        {
            Reflection.CopyPropertiesDynamic<IPayment, IPayment>(originalPayment, orderPayment);
            Address.CopyPropertiesTo(originalPayment, orderPayment);
            orderPayment.TransactionID = originalPayment.TransactionID;
            orderPayment.Amount = paymentAmount;
            orderPayment.PaymentGatewayID = originalPayment.PaymentGatewayID;
            
            return orderPayment;
        }

        /// <summary>
        /// Returns a return order created from CoreContext.CurrentOrder
        /// </summary>
        /// <returns></returns>
        private Order CreateReturnOrder()
        {
            Order order = new Order();

            order.ConsultantID = OrderContext.OriginalOrder.ConsultantID;
            order.CommissionDate =
                DateTime.Today.AddDays(
                    ConfigurationManager.GetAppSetting<double>(
                        ConfigurationManager.VariableKey.ReturnOrderCommissionDateAddition));
            order.OrderTypeID = ConfigurationManager.GetAppSetting<short>(ConfigurationManager.VariableKey.ReturnOrderTypeID, Constants.OrderType.ReturnOrder.ToShort());
            order.ParentOrderID = OrderContext.OriginalOrder.OrderID;
            order.OrderStatusID = (int)Constants.OrderStatus.Pending;
            order.OrderPendingState = Constants.OrderPendingStates.Quote;
            order.SiteID = ConfigurationManager.GetAppSetting<int?>(ConfigurationManager.VariableKey.NSCoreSiteID, null);
            order.CurrencyID = OrderContext.OriginalOrder.CurrencyID;

            // Add hostess to order
            order.AddNewCustomer(OrderContext.OriginalOrder.AsOrder().GetHostess() != null
                ? OrderContext.OriginalOrder.AsOrder().GetHostess().AccountID
                : OrderContext.OriginalOrder.OrderCustomers[0].AccountID);

            var orderShippment = OrderContext.OriginalOrder.AsOrder().GetDefaultShipment();
            var newOrderShippment = order.GetDefaultShipment();
            order.UpdateOrderShipmentAddress(newOrderShippment, orderShippment);

            order.OrderCustomers[0].SalesTaxTransactionNumber = OrderContext.OriginalOrder.AsOrder().OrderCustomers[0].SalesTaxTransactionNumber;
            order.OrderCustomers[0].UseTaxTransactionNumber = OrderContext.OriginalOrder.AsOrder().OrderCustomers[0].UseTaxTransactionNumber;

            order.Subtotal = OrderContext.OriginalOrder.Subtotal;
            order.GrandTotal = OrderContext.OriginalOrder.GrandTotal;
            return order;
        }
        #endregion
    }
}
