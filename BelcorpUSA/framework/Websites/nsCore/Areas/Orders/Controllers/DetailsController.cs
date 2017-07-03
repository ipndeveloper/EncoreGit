using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Web.Caching;
using System.Web.Mvc;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Common.Services;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Business.HelperObjects.OrderPackages;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Diagnostics.Utilities;
using NetSteps.Encore.Core.IoC;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Controls.Services.Interfaces;
using NetSteps.Web.Mvc.Helpers;
using nsCore.Areas.Orders.Models.Details;
using IPaymentMethodModel = nsCore.Areas.Orders.Models.Details.IPaymentMethodModel;
using Microsoft.Reporting.WebForms;
using System.Data;
using NetSteps.Data.Entities.Business.Logic;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Common.Configuration;
using NetSteps.Data.Entities.Dto;
using CodeBarGeerator;
using iTextSharp.text.pdf;
using iTextSharp.text;
using nsCore.nsCoreWebConstants;
using NetSteps.Data.Entities.Business.Common;
using NetSteps.Data.Entities.Services;

//Modifications: 
//@01 20150630 BR-AT-006 CSTI BAL : Add link to Tracking
//@02 20150817 BR-AT-008 GYS EFP : Add link to Claim Items

namespace nsCore.Areas.Orders.Controllers
{
    public class DetailsController : OrdersBaseController
    {
        #region Properties

        #region AmazonS3Site

        private string AmazonS3Site
        {
            get
            {
                if (ConfigurationManager.AppSettings["AmazonS3Site"] != null)
                {
                    return ConfigurationManager.AppSettings["AmazonS3Site"].ToString();
                }
                else
                {
                    // Default Value
                    return "https://s3.amazonaws.com/EncoreBrasilQAS/B080/";
                }
            }
        }

        #endregion

        #endregion

        protected IDetailsControllerService DetailsService { get { return Create.New<IDetailsControllerService>(); } }

        [FunctionFilter("Orders", "~/Accounts")]
        public virtual ActionResult Index(string id)
        { 
            OrderContext.Clear();
            foreach (var item in ShippingCalculatorExtensions.GetPreOderDetail(Convert.ToInt32(id)))
            {
                Session["WareHouseId"] = item.WareHouseID;
                Session["PreOrder"] = item.PreOrderId;
            }

            Session["ProductCredit"] = Order.GetProductCreditByAccountDet(Convert.ToInt32(id));

            Account account = null;
            using (var indexTracer = this.TraceActivity(string.Format("requesting ~/Accounts/Index/{0}", id)))
            {
                try
                {
                    if (string.IsNullOrEmpty(id))
                    {
                        string message = "Invalid Order. Order# not specified: ";
                        this.TraceError(message);
                        TempData["Error"] = message;
                        return RedirectToAction("Index", "Landing");
                    }

                    if (OrderContext.Order == null || OrderContext.Order.OrderID.ToString() != id)
                    {
                        this.TraceEvent("resetting OrderContext");
                        OrderContext.Clear();
                        OrderContext.Order = DetailsService.LoadOrder(id);

                        account = Account.LoadForSession(OrderContext.Order.OrderCustomers[0].AccountID);
                        CoreContext.CurrentAccount = account.Clone();
                    }

                    if (OrderContext.Order == null)
                    {
                        string message = "No order found for: " + id;
                        this.TraceEvent(message);
                        TempData["Error"] = message;
                        return RedirectToAction("Index", "Browse", new { orderNumber = id });
                    }

                    this.TraceEvent("Order.GetPopupMessageForOrderDetail");
                    TempData["PopupMessage"] = Order.GetPopupMessageForOrderDetail(OrderContext.Order.AsOrder());

                    this.TraceEvent("DetailsService.DisallowAutoshipTemplateEdits");
                    BasicResponse response = DetailsService.DisallowAutoshipTemplateEdits(OrderContext.Order.AsOrder());
                    if (!response.Success)
                    {
                        this.TraceEvent(response.Message);
                        TempData["Error"] = response.Message;
                        return RedirectToAction("Index", "Browse");
                    }

                    this.TraceEvent("SetOrderViewBagData");
                    SetOrderViewBagData(OrderContext.Order.AsOrder());

                    this.TraceEvent("CreateOrderCustomerPartialOrderItemDetailsModel");
                    ViewBag.OrderCustomerDetails = CreateOrderCustomerPartialOrderItemDetailsModel(OrderContext.Order.AsOrder());

                    this.TraceEvent("GetOrderCustomerPartialDetails");
                    ViewBag.OrderCustomersPartialDetails = GetOrderCustomerPartialDetails(OrderContext.Order.AsOrder());

                    this.TraceEvent("rendering View");
                    ViewData["SupportTicketID"] = Request.QueryString["SupportTicketID"] != null ? Request.QueryString["SupportTicketID"] : "0";
                    return View(OrderContext.Order.AsOrder());
                }
                catch (Exception excp)
                {
                    excp.TraceException(excp);
                    throw EntityExceptionHelper.GetAndLogNetStepsException(excp, Constants.NetStepsExceptionType.NetStepsApplicationException);
                }
            }
        }

        protected virtual List<PartialOrderCustomerDetailsModel> GetOrderCustomerPartialDetails(Order order)
        {
            List<PartialOrderCustomerDetailsModel> orderCustomersPartialDetails = new List<PartialOrderCustomerDetailsModel>();
            foreach (OrderCustomer oc in order.OrderCustomers)
            {
                orderCustomersPartialDetails.Add(new PartialOrderCustomerDetailsModel(order, oc));
            }

            var orderRepo = Create.New<IOrderRepository>();
            var childOrders = orderRepo.LoadChildOrdersFull(
                order.OrderID,
                (short)Constants.OrderType.OnlineOrder,
                (short)Constants.OrderType.OnlinePartyOrder);

            // Include child orders of parties.
            foreach (Order childOrder in childOrders)
            {
                foreach (OrderCustomer oc in childOrder.OrderCustomers)
                {
                    orderCustomersPartialDetails.Add(new PartialOrderCustomerDetailsModel(childOrder, oc, true));
                }
            }

            return orderCustomersPartialDetails;
        }

        protected virtual List<PartySearchData> GetConsultantOpenParties(int accountID)
        {
            return Party.GetOpenParties(accountID);
        }

        protected virtual Dictionary<int, PartialOrderItemDetailsModel> CreateOrderCustomerPartialOrderItemDetailsModel(Order order)
        {
            Dictionary<int, PartialOrderItemDetailsModel> model = new Dictionary<int, PartialOrderItemDetailsModel>();
            foreach (OrderCustomer oc in order.OrderCustomers)
            {
                model.Add(oc.OrderCustomerID, CreatePartialOrderItemDetailsModel(oc));
            }

            var orderRepo = Create.New<IOrderRepository>();
            var childOrders = orderRepo.LoadChildOrdersFull(
                order.OrderID,
                (short)Constants.OrderType.OnlineOrder,
                (short)Constants.OrderType.OnlinePartyOrder);

            // Include child orders of parties.
            foreach (Order childOrder in childOrders)
            {
                foreach (OrderCustomer oc in childOrder.OrderCustomers)
                {
                    model.Add(oc.OrderCustomerID, CreatePartialOrderItemDetailsModel(oc));
                }
            }

            return model;
        }

        public virtual PartialOrderItemDetailsModel CreatePartialOrderItemDetailsModel(OrderCustomer orderCustomer)
        {

      
            PartialOrderItemDetailsModel model = new PartialOrderItemDetailsModel();
            model.OrderCustomer = orderCustomer;
            model.Order = orderCustomer.Order;
            model.IsReturnOrder = model.Order.OrderTypeID == Constants.OrderType.ReturnOrder.ToShort();
            model.IsReplacementOrder = model.Order.OrderTypeID == Constants.OrderType.ReplacementOrder.ToShort();


            //model.Currency = SmallCollectionCache.Instance.Currencies.GetById(lenguaje);


            model.Currency = SmallCollectionCache.Instance.Currencies.GetById(model.Order.CurrencyID);
            model.OrderAdjustmentAddOperationKindID = (int)NetSteps.OrderAdjustments.Common.Model.OrderAdjustmentOrderLineOperationKind.AddedItem;
            model.Inventory = NetSteps.Encore.Core.IoC.Create.New<InventoryBaseRepository>();
            model.OrderItemDetails = new List<OrderItemDetailModel>();
            foreach (OrderItem orderItem in orderCustomer.ParentOrderItems.Where(x => !x.OrderAdjustmentOrderLineModifications.Any(y => y.ModificationOperationID == model.OrderAdjustmentAddOperationKindID)))
            {
                OrderItemDetailModel detailModel = CreateOrderItemDetailModel(orderItem, model.IsReturnOrder, model.IsReplacementOrder);
                model.OrderItemDetails.Add(detailModel);
            }
            model.AddedOrderItems = orderCustomer.OrderItems.Where(x => x.OrderAdjustmentOrderLineModifications.Any(y => y.ModificationOperationID == model.OrderAdjustmentAddOperationKindID));

            return model;
        }

        public virtual OrderItemDetailModel CreateOrderItemDetailModel(OrderItem orderItem, bool isReturnOrder, bool isReplacementOrder)
        {
            OrderItemDetailModel detailModel = GetOrderDetailModel(orderItem);
            detailModel.Product = Inventory.GetProduct(orderItem.ProductID.ToInt());
            var priceTypeService = Create.New<IPriceTypeService>();
            //some legacy/imported orders are missing productpricetypeid, so use default if missing. 
            var currencyPriceTypeID = orderItem.ProductPriceTypeID.HasValue ? orderItem.ProductPriceTypeID.Value : priceTypeService.GetPriceType(orderItem.OrderCustomer.AccountTypeID, (int)Constants.PriceRelationshipType.Products, ApplicationContext.Instance.StoreFrontID).PriceTypeID;
            detailModel.AdjustedItemPrice = orderItem.GetAdjustedPrice(currencyPriceTypeID);
            detailModel.PreAdjustedItemPrice = orderItem.GetPreAdjustmentPrice(currencyPriceTypeID);
            detailModel.ItemPriceTotal = detailModel.AdjustedItemPrice * orderItem.Quantity;
            detailModel.AdjustedCommissionTotal = orderItem.GetAdjustedPrice(orderItem.OrderCustomer.CommissionablePriceTypeID) * orderItem.Quantity;
            detailModel.PreAdjustedCommissionTotal = orderItem.GetPreAdjustmentPrice(orderItem.OrderCustomer.CommissionablePriceTypeID) * orderItem.Quantity;

            if (isReturnOrder)
            {
                detailModel.OrderItemReturn = orderItem.OrderItemReturns.FirstOrDefault();
                if (detailModel.OrderItemReturn == null)
                    detailModel.OrderItemReturn = OrderItemReturn.LoadByOrderItemID(orderItem.OrderItemID);
            }

            if (isReplacementOrder)
            {
                detailModel.OrderItemReplacement = orderItem.OrderItemReplacement;
                if (detailModel.OrderItemReplacement == null)
                    detailModel.OrderItemReplacement = OrderItemReplacement.LoadFull(orderItem.OrderItemID);
            }

            if (detailModel.OrderItemReturn != null)
            {
                detailModel.ReturnReason = SmallCollectionCache.Instance.ReturnReasons.GetById(detailModel.OrderItemReturn.ReturnReasonID).GetTerm();
            }

            return detailModel;
        }

        protected virtual OrderItemDetailModel GetOrderDetailModel(OrderItem orderItem)
        {
            Contract.Requires<ArgumentNullException>(orderItem != null);

            var orderItemDetailModel = new OrderItemDetailModel();
            orderItemDetailModel.OrderItem = orderItem;

            foreach (var message in orderItem.OrderItemMessages.Select(m => m.Message))
            {
                orderItemDetailModel.Messages.Add(message);
            }

            return orderItemDetailModel;
        }

        protected virtual void SetOrderViewBagData(Order order)
        {
            var packageInfoList = new List<OrderPackageInfoModel>();
            //if (order.OrderShipments != null)
            //{
            //    foreach (var shipment in order.OrderShipments)
            //    {
            //        packageInfoList.AddRange(PackageInfoModels(shipment));

            //    }
            //}

            foreach (var shipment in order.OrderShipments)
            {
                List<ShippingCalculatorSearchData.GetShipping> objGetShipping = ShippingCalculatorExtensions.GetShippingResult(shipment.PostalCode);

                foreach (var item in objGetShipping.Where(x => x.ShippingMethodID == shipment.ShippingMethodID && x.OrderTypeID == shipment.Order.OrderTypeID).ToList())
                {
                    OrderPackageInfoModel objE = new OrderPackageInfoModel();
                    objE.ShipMethodName = item.DaysForDelivery + " " + shipment.ShippingMethodName + " " + item.Name;
                    objE.OrderCustomerID = shipment.OrderCustomerID;
                    packageInfoList.Add(objE);
                }

            }
            ViewBag.PackageInfoList = packageInfoList;



            ViewBag.OpenConsultantParties = GetConsultantOpenParties(order.ConsultantID);

            Party party = order.ParentOrderID != null ? Party.LoadByOrderID((int)order.ParentOrderID) : null;
            ViewBag.CurrentAttachedParty = party != null ? party.Name + " " + "(" + party.OrderID + ")" : null;
        }

        /// <summary>
        /// Added to prevent users from editing an autoship from the Orders tab
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public virtual BasicResponse DisallowAutoshipTemplateEdits(Order order)
        {
            BasicResponse response = new BasicResponse() { Success = true };

            if (order.OrderTypeID == (int)Constants.OrderType.AutoshipTemplate)
            {
                response.Success = false;
                response.Message = Translation.GetTerm("CannotEditAutoshipTemplate", "Cannot edit Autoship template");
            }

            return response;
        }

        [FunctionFilter("Orders", "~/Accounts")]
        public virtual ActionResult CheckIfOrderFullyReturned(string orderNumber)
        {
            int? orderId = null;
            try
            {
                Order order = Order.LoadByOrderNumberFull(orderNumber);
                orderId = order.OrderID;
                //var returnOrders = CoreContext.LoadChildReturnOrdersFull(order.OrderID);
                //bool isOrderFullyReturned = Order.IsOrderFullyReturned(order, returnOrders);
                OrderService servicio = new OrderService();
                bool isOrderFullyReturned = servicio.OrderDevueltaTotalmente(order);

                return Json(new { fullyReturned = isOrderFullyReturned });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, orderID: orderId.ToIntNullable());
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [FunctionFilter("Orders", "~/Accounts")]
        public virtual ActionResult ChangeCommissionConsultant(string orderNumber, int commissionConsultantId)
        {
            int? orderID = null;
            try
            {
                Order order = DetailsService.ChangeCommissionConsultant(orderNumber, commissionConsultantId);
                orderID = order.OrderID;
                order.Save();
                return Json(new
                {
                    result = true,
                    accountNumber = order.ConsultantInfo.AccountNumber,
                    fullName = order.ConsultantInfo.FullName
                });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, orderID: orderID.ToIntNullable());
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [FunctionFilter("Orders", "~/Accounts")]
        public virtual ActionResult ChangeAttachedParty(string orderNumber, int newPartyOrderId)
        {
            int? orderId = null;
            try
            {
                Order order = DetailsService.ChangeAttachedParty(orderNumber, newPartyOrderId);
                orderId = order.OrderID;
                order.Save();
                return Json(new
                {
                    result = true
                });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, orderID: orderId.ToIntNullable());
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [FunctionFilter("Orders", "~/Accounts")]
        public virtual ActionResult ChangeCommissionDate(string orderNumber, DateTime commissionDate)
        {
            int? orderId = null;
            try
            {
                Order order = DetailsService.ChangeCommissionDate(orderNumber, commissionDate);
                orderId = order.OrderID;
                order.Save();

                return Json(new
                {
                    result = true
                });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, orderID: orderId.ToIntNullable());
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public virtual ActionResult GetPayment(string paymentGuid)
        {
            //This call should come from Order Entry
            Order order = CoreContext.CurrentOrder;
            var customer = order.OrderCustomers.FirstOrDefault();
            var orderPayment = customer.OrderPayments.FirstOrDefault(oi => oi.Guid.ToString("N") == paymentGuid);
            if (orderPayment == null)
                return Json(new
                {
                    result = false,
                    message = Translation.GetTerm("PaymentDoesNotExist", "That payment does not exist.")
                });
            OrderPayment payment = OrderPayment.LoadFull(orderPayment.OrderPaymentID);

            return Content(BuildPaymentDisplay(payment));
        }

        public virtual ActionResult GetPaymentInfo(int paymentId)
        {
            //This call should come from the details page
            // To avoid the use of the ("CurrentOrder") session var for improved reliability, is the user has multiple tabs/browsers open. - JHE
            OrderPayment payment = OrderPayment.LoadFull(paymentId);
            var model = GetPaymentMethodModel(payment);

            return Json(new { result = true, paymentHTML = RenderPartialToString(model.GetPartialViewName(), model) });
        }

        public virtual IPaymentMethodModel GetPaymentMethodModel(OrderPayment payment)
        {
            if (payment.PaymentTypeID == Constants.PaymentType.EFT.ToShort())
            {
                return GetEftMethodModel(payment);
            }

            return GetDefaultMethodModel(payment);
        }

        public virtual DefaultPaymentMethodModalModel GetDefaultMethodModel(OrderPayment payment)
        {
            var model = new DefaultPaymentMethodModalModel
            {
                DecryptedAccountNumber = payment.DecryptedAccountNumber,
                ExpirationDate = payment.ExpirationDate,
                BillingName = payment.NameOnCard,
                BillingAddress1 = payment.BillingAddress1,
                BillingCity = payment.BillingCity,
                BillingState = payment.BillingState,
                BillingPostalCode = payment.BillingPostalCode,
                BillingCountryId = payment.BillingCountryID,
                TransactionId = payment.TransactionID
            };
            return model;
        }

        public virtual EFTPaymentMethodModalModel GetEftMethodModel(OrderPayment payment)
        {
            var model = new EFTPaymentMethodModalModel
            {
                DecryptedAccountNumber = payment.DecryptedAccountNumber,
                RoutingNumber = payment.RoutingNumber,
                BankName = payment.BankName,
                AccountType = payment.BankAccountTypeID > 0 ? BankAccountType.Load(payment.BankAccountTypeID.ToShort()).Name : "",
                BillingName = payment.NameOnCard,
                BillingAddress1 = payment.BillingAddress1,
                BillingCity = payment.BillingCity,
                BillingState = payment.BillingState,
                BillingPostalCode = payment.BillingPostalCode,
                BillingCountryId = payment.BillingCountryID
            };

            return model;
        }

        private string BuildPaymentDisplay(OrderPayment payment)
        {
            StringBuilder paymentDisplay = new StringBuilder();
            paymentDisplay.Append("<tr>").AppendCell(Translation.GetTerm("AccountNumber", "Account Number") + ":").AppendCell(payment.DecryptedAccountNumber.MaskString(4)).Append("</tr>")
                 .Append("<tr class=\"Alt\">").AppendCell(Translation.GetTerm("ExpirationDate", "Expiration Date") + ":").AppendCell(payment.ExpirationDate.ToExpirationStringDisplay(CoreContext.CurrentCultureInfo)).Append("</tr>")
                 .Append("<tr>").AppendCell(Translation.GetTerm("NameOnCard", "Name on card") + ":").AppendCell(payment.NameOnCard).Append("</tr>")
                 .Append("<tr class=\"Alt\">").AppendCell(Translation.GetTerm("Name") + ":").AppendCell(payment.BillingName).Append("</tr>")
                 .Append("<tr>").AppendCell(Translation.GetTerm("FirstName", "First Name") + ":").AppendCell(payment.BillingFirstName).Append("</tr>")
                 .Append("<tr class=\"Alt\">").AppendCell(Translation.GetTerm("LastName", "Last Name") + ":").AppendCell(payment.BillingLastName).Append("</tr>")
                 .Append("<tr>").AppendCell(Translation.GetTerm("Address") + ":").AppendCell(payment.BillingAddress1).Append("</tr>")
                 .Append("<tr class=\"Alt\">").AppendCell("").AppendCell(payment.BillingCity + ", " + payment.BillingState + " " + payment.BillingPostalCode).Append("</tr>")
                 .Append("<tr>").AppendCell(Translation.GetTerm("Country") + ":").AppendCell(SmallCollectionCache.Instance.Countries.GetById(payment.BillingCountryID.ToInt()).GetTerm()).Append("</tr>")
                 .Append("<tr  class=\"Alt\">").AppendCell(Translation.GetTerm("PaymentStatus", "Payment Status") + ":").AppendCell(SmallCollectionCache.Instance.OrderPaymentStatuses.GetById(payment.OrderPaymentStatusID).GetTerm()).Append("</tr>")
                 .Append("<tr>").AppendCell(Translation.GetTerm("TransactionID", "Transaction ID") + ":").AppendCell(payment.TransactionID).Append("</tr>");
            if (payment.OrderPaymentResults.Count > 0)
            {
                paymentDisplay.Append("<tr>").AppendCell("<b>" + Translation.GetTerm("PaymentResults", "Payment Results") + ": </b>", columnSpan: 2).Append("</tr>");
                foreach (var orderPaymentResult in payment.OrderPaymentResults)
                {
                    paymentDisplay.Append("<tr>").AppendCell(orderPaymentResult.DateAuthorized.ToShortDateString(CoreContext.CurrentCultureInfo) + ":").AppendCell(orderPaymentResult.ResponseReasonText.ToCleanString()).Append("</tr>");
                }

                if (payment.OrderPaymentResults[0].BalanceOnCard != null)
                    paymentDisplay.Append("<tr>").AppendCell(Translation.GetTerm("PrepaidAvailableBalance", "Prepaid Available Balance") + ":").AppendCell(payment.OrderPaymentResults[0].BalanceOnCard.ToMoneyString()).Append("</tr>");
            }
            return paymentDisplay.ToString();
        }

        [FunctionFilter("Orders", "~/Accounts")]
        [OutputCache(CacheProfile = "PagedGridData")]
        public virtual ActionResult AuditHistory(string orderNumber)
        {
            int? orderId = null;
            try
            {
                if (string.IsNullOrEmpty(orderNumber))
                    return RedirectToAction("Index");

                // TODO: Try and optimize this later to not load order here; (but we can't use the common session var for order here here) - JHE
                Order order = Order.LoadByOrderNumberFull(orderNumber);
                orderId = order.OrderID;

                ViewData["EntityName"] = "Order";
                ViewData["ID"] = order.OrderID;

                string cancelOrderScript = "<script type=\"text/javascript\"> $(function () { $('#cancelOrder').click(function () { if (confirm('Are you sure?')) { $.post('/Orders/Details/Cancel', { orderId: '<%= Model.OrderID %>' }, function (results) { if (results.result) location.reload(true); else showMessage(results.message, true); }); } }); }); </script>";

                StringBuilder links = new StringBuilder();
                links.Append(cancelOrderScript);

                string editLink = (order.OrderTypeID == NetSteps.Data.Entities.Constants.OrderType.ReturnOrder.ToInt()) ? System.Web.VirtualPathUtility.ToAbsolute("~/Orders/Return/Edit") + "?orderNumber=" + order.OrderNumber : System.Web.VirtualPathUtility.ToAbsolute("~/Orders/OrderEntry/Edit") + "?orderNumber=" + order.OrderNumber;
                links.Append("<a id=\"orderDetails\" href=\"" + System.Web.VirtualPathUtility.ToAbsolute("~/Orders/Details/Index/") + order.OrderNumber + "\">" + Translation.GetTerm("Details", "Details") + "</a> | ");
                if (order.OrderStatusID == SmallCollectionCache.Instance.OrderStatuses.FirstOrDefault(os => os.Name == "Pending").OrderStatusID)
                    links.Append("<a id=\"cancelOrder\" href=\"javascript:void(0);\">" + Translation.GetTerm("CancelOrder", "Cancel Order") + "</a> | <a id=\"editOrder\" href=\"" + editLink + "\">" + Translation.GetTerm("EditOrder", "Edit Order") + "</a> | ");
                links.Append(Translation.GetTerm("AuditHistory", "Audit History")).ToString();
                links.Append(" | <a id=\"orderDetailTrakings\" href=\"" + System.Web.VirtualPathUtility.ToAbsolute("~/Orders/Details/Tracking?orderNumber=") + order.OrderNumber + "\">" + Translation.GetTerm("Tracking", "Tracking") + "</a>");

                ViewData["Links"] = links;

                return View("AuditHistory", "Orders", order);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, orderID: orderId.ToIntNullable());
                throw exception;
            }
        }

        /*************************************************************************************************************************/

        [FunctionFilter("Orders", "~/Accounts")]
        [OutputCache(CacheProfile = "PagedGridData")]
        public virtual ActionResult Tracking(string orderNumber)
        {
            int? orderId = null;
            try
            {
                if (string.IsNullOrEmpty(orderNumber))
                    return RedirectToAction("Index");

                // TODO: Try and optimize this later to not load order here; (but we can't use the common session var for order here here) - JHE
                Order order = Order.LoadByOrderNumberFull(orderNumber);
                orderId = order.OrderID;

                //orderId = order.OrderCustomers[0].OrderItems .OrderID;

                var result = Order.GetExpectedDeliveryDateByOrderNumber(orderNumber);

                ViewData["EntityName"] = "Order";
                ViewData["ID"] = order.OrderID;
                ViewData["OrderNumber"] = orderNumber;
                ViewData["DeliveryDateUTC"] = (result.Equals(null) || result.ToString("dd/MM/yyyy").Equals("01/01/1900") ? "" : result.ToString("dd/MM/yyyy"));

                string cancelOrderScript = "<script type=\"text/javascript\"> $(function () { $('#cancelOrder').click(function () { if (confirm('Are you sure?')) { $.post('/Orders/Details/Cancel', { orderId: '<%= Model.OrderID %>' }, function (results) { if (results.result) location.reload(true); else showMessage(results.message, true); }); } }); }); </script>";

                StringBuilder links = new StringBuilder();
                links.Append(cancelOrderScript);
                string editLink = (order.OrderTypeID == NetSteps.Data.Entities.Constants.OrderType.ReturnOrder.ToInt()) ? System.Web.VirtualPathUtility.ToAbsolute("~/Orders/Return/Edit") + "?orderNumber=" + order.OrderNumber : System.Web.VirtualPathUtility.ToAbsolute("~/Orders/OrderEntry/Edit") + "?orderNumber=" + order.OrderNumber;
                links.Append("<a id=\"orderDetails\" href=\"" + System.Web.VirtualPathUtility.ToAbsolute("~/Orders/Details/Index/") + order.OrderNumber + "\">" + Translation.GetTerm("Details", "Details") + "</a> | ");
                if (order.OrderStatusID == SmallCollectionCache.Instance.OrderStatuses.FirstOrDefault(os => os.Name == "Pending").OrderStatusID)
                    links.Append("<a id=\"cancelOrder\" href=\"javascript:void(0);\">" + Translation.GetTerm("CancelOrder", "Cancel Order") + "</a> | <a id=\"editOrder\" href=\"" + editLink + "\">" + Translation.GetTerm("EditOrder", "Edit Order") + "</a> | ");
                links.Append("<a id=\"orderDetailAuditHistory\" href=\"" + System.Web.VirtualPathUtility.ToAbsolute("~/Orders/Details/AuditHistory?orderNumber=") + order.OrderNumber + "\">" + Translation.GetTerm("AuditHistory", "Audit History") + "</a> | ");
                links.Append(Translation.GetTerm("Tracking", "Audit Tracking")).ToString();



                ViewData["Links"] = links;
                ViewData["OrderNumber"] = orderNumber;

                return View("Tracking", "Orders", order);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, orderID: orderId.ToIntNullable());
                throw exception;
            }
        }
        /***********************************************************************************************************************/


        #region Invoices

        [FunctionFilter("Orders", "~/Accounts")]
        [OutputCache(CacheProfile = "PagedGridData")]
        public virtual ActionResult Invoices(string orderNumber)
        {
            int orderId = 0;
            try
            {
                if (string.IsNullOrEmpty(orderNumber))
                    return RedirectToAction("Index");

                // TODO: Try and optimize this later to not load order here; (but we can't use the common session var for order here here) - JHE
                Order order = Order.LoadByOrderNumberFull(orderNumber);
                orderId = order.OrderID;

                //orderId = order.OrderCustomers[0].OrderItems .OrderID;

                #region DropDawnList

                var result = Order.GetInvoiceNumbersByOrderID(orderId);

                List<SelectListItem> items = new List<SelectListItem>();

                if (result != null && result.Count > 0)
                {


                    foreach (var item in result)
                    {
                        if (item != 0)
                        {
                            var newItem = new SelectListItem()
                                         {
                                             Text = item.ToString(),
                                             Value = item.ToString(),
                                             Selected = false
                                         };

                            items.Add(newItem);
                        }
                    }

                }

                TempData["InvoiceNumbers"] = items;

                #endregion

                #region Links

                string cancelOrderScript = "<script type=\"text/javascript\"> $(function () { $('#cancelOrder').click(function () { if (confirm('Are you sure?')) { $.post('/Orders/Details/Cancel', { orderId: '<%= Model.OrderID %>' }, function (results) { if (results.result) location.reload(true); else showMessage(results.message, true); }); } }); }); </script>";

                StringBuilder links = new StringBuilder();
                links.Append(cancelOrderScript);
                string editLink = (order.OrderTypeID == NetSteps.Data.Entities.Constants.OrderType.ReturnOrder.ToInt()) ? System.Web.VirtualPathUtility.ToAbsolute("~/Orders/Return/Edit") + "?orderNumber=" + order.OrderNumber : System.Web.VirtualPathUtility.ToAbsolute("~/Orders/OrderEntry/Edit") + "?orderNumber=" + order.OrderNumber;
                links.Append("<a id=\"orderDetails\" href=\"" + System.Web.VirtualPathUtility.ToAbsolute("~/Orders/Details/Index/") + order.OrderNumber + "\">" + Translation.GetTerm("Details", "Details") + "</a> | ");
                if (order.OrderStatusID == SmallCollectionCache.Instance.OrderStatuses.FirstOrDefault(os => os.Name == "Pending").OrderStatusID)
                    links.Append("<a id=\"cancelOrder\" href=\"javascript:void(0);\">" + Translation.GetTerm("CancelOrder", "Cancel Order") + "</a> | <a id=\"editOrder\" href=\"" + editLink + "\">" + Translation.GetTerm("EditOrder", "Edit Order") + "</a> | ");
                links.Append("<a id=\"orderDetailAuditHistory\" href=\"" + System.Web.VirtualPathUtility.ToAbsolute("~/Orders/Details/AuditHistory?orderNumber=") + order.OrderNumber + "\">" + Translation.GetTerm("AuditHistory", "Audit History") + "</a> | ");
                links.Append(Translation.GetTerm("Invoices", "Audit Tracking")).ToString();



                ViewData["Links"] = links;

                #endregion

                return View("Invoices", "Orders", order);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, orderID: orderId.ToIntNullable());
                throw exception;
            }
        }

        #endregion

        #region LoadPDFReport

        [HttpPost]
        public ActionResult LoadPDFReport(string InvoiceNumber)
        {
            try
            {
                string URL = this.AmazonS3Site;

                DataTable dtDetails = Order.GetOrderInvoiceDetail(InvoiceNumber);

                if (dtDetails != null && dtDetails.Rows.Count > 0)
                {
                    string date = dtDetails.Rows[0]["YEAR"].ToString() + dtDetails.Rows[0]["MONTH"].ToString();
                    string key = dtDetails.Rows[0]["KEY"].ToString();

                    URL = URL + date + "/" + key + "_" + date + ".pdf";
                    return Json(new { url = URL, flag = true });
                }
                else
                {
                    return Json(new { url = "Error getting invoice values", flag = false });
                }
            }
            catch (Exception ex)
            {
                return Json(new { url = ex.Message, flag = false });
            }
        }

        #endregion

        //@01 AINI
        [FunctionFilter("Orders", "~/Accounts")]
        [OutputCache(CacheProfile = "PagedGridData")]
        public virtual ActionResult TrackingDetail(string id, int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                var order = (orderByDirection.ToString().ToUpper().StartsWith("ASC")) ? "asc" : "desc";
                int rowCount = 0;
                var result = Order.SearchOrderTrackingByOrderNumber(id, page, pageSize, orderBy, order);

                if (result.Count > 0)
                {
                    foreach (var OrderTracking in result)
                    {
                        builder.Append("<tr>");
                        string key = "";
                        switch (OrderTracking.OrderStatusID)
                        {
                            case 20:     //Invoice
                                key = "Invoice_" + id;
                                break;
                            case 6:     //Partially Paid
                                key = "PartiallyPaid_" + id;
                                break;

                            case 9:     //Shipped
                                key = "Shipped_" + id;
                                break;
                        }
                        string textoViewDetaillTracking = Translation.GetTerm("ViewDetaillTracking", "View detaill");

                        builder.AppendCell(OrderTracking.FinalTackingDateUTC.Year == 1900 && OrderTracking.FinalTackingDateUTC.Day == 1 && OrderTracking.FinalTackingDateUTC.Month == 1 ? "" : "<img src=\"/Content/Images/icon-Success.png\" width=\"20\" height=\"20\" >");
                        builder.AppendCell(OrderTracking.Etapa.ToString());// SE TIENE Q MOSTRAR LAS ETAPAS SEGUN EL CAMPO InitialTackingDateUTC
                        //builder.AppendCell(OrderTracking.FinalTackingDateUTC.Year == 1900 && OrderTracking.FinalTackingDateUTC.Day == 1 && OrderTracking.FinalTackingDateUTC.Month == 1 ? "" : "<img src=\"/Content/Images/" + OrderTracking.ImagenStatus + "\" width=\"20\" height=\"20\" >");
                        builder.AppendCell("<img src=\"/Content/Images/" + OrderTracking.ImagenStatus + "\" width=\"20\" height=\"20\" >");
                        if (key == "")
                        {
                            builder.AppendCell(OrderTracking.Name.ToString());
                        }
                        else
                        {
                            builder.AppendCell(" <a title='" + textoViewDetaillTracking + "' style='text-decoration: underline;color:blue' id='" + key + "' href='javascript:void(0)' class='ShowPopup'>" + OrderTracking.Name + " <img style='margin-left:4px' src=\"/Content/Images/imgViewDetaill.PNG\" width=\"15\" height=\"15\" > </a>");
                            //  builder.AppendLinkCell("javascript:void(0);", OrderTracking.Name, linkCssClass: "ShowPopup", linkID: key);
                        }

                        builder.AppendCell(OrderTracking.InitialTackingDateUTC.Year == 1900 && OrderTracking.InitialTackingDateUTC.Day == 1 && OrderTracking.InitialTackingDateUTC.Month == 1 ? "" : OrderTracking.InitialTackingDateUTC.ToString("dd/MM/yyyy hh:mm", CoreContext.CurrentCultureInfo));
                        builder.AppendCell(OrderTracking.FinalTackingDateUTC.Year == 1900 && OrderTracking.FinalTackingDateUTC.Day == 1 && OrderTracking.FinalTackingDateUTC.Month == 1 ? "" : OrderTracking.FinalTackingDateUTC.ToString("dd/MM/yyyy hh:mm", CoreContext.CurrentCultureInfo));
                        builder.AppendCell(OrderTracking.Comment.ToString());
                        builder.Append("</tr>");
                        rowCount = OrderTracking.RowTotal;
                    }

                    int totalPages = (rowCount / pageSize);
                    totalPages = totalPages == 0 ? 1 : totalPages;

                    return Json(new { result = true, totalPages = totalPages, page = builder.ToString() });

                }
                else
                    return Json(new { result = true, totalPages = 0, page = "<tr><td colspan=\"7\">" + Translation.GetTerm("ThereWereNoRecordsFoundThatMeetThatCriteriaPleaseTryAgain", "There were no records found that meet that criteria. Please try again.") + "</td></tr>" });

            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "PagedGridData")]
        public virtual ActionResult ListOrderStatusPartiallyPaid(string id, int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection)
        {
            try
            {
                var order = (orderByDirection.ToString().ToUpper().StartsWith("ASC")) ? "asc" : "desc";

                StringBuilder sBuilder = new StringBuilder();
                int rowCount = 0;

                var result = OrderRepository.ListOrderStatusPartiallyPaid(id, page, pageSize, orderBy, order);

                if (result.Count > 0)
                {
                    foreach (var orderPayment in result)
                    {
                        sBuilder.Append("<tr>")
                        .AppendCell(orderPayment.TicketNumber.ToString())
                        .AppendCell(orderPayment.Name)
                        .Append("</tr>");
                        rowCount = orderPayment.RowTotal;
                    }

                    int totalPages = (rowCount / pageSize);
                    totalPages = totalPages == 0 ? 1 : totalPages;

                    return Json(new { result = true, totalPages = totalPages, page = sBuilder.ToString() });
                }
                else
                    return Json(new { result = true, totalPages = 0, page = "<tr><td colspan=\"2\">" + Translation.GetTerm("ThereWereNoRecordsFoundThatMeetThatCriteriaPleaseTryAgain", "There were no records found that meet that criteria. Please try again.") + "</td></tr>" });

            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "PagedGridData")]
        public virtual ActionResult ListOrderStatusShipped(string id, int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection)
        {
            try
            {
                var order = (orderByDirection.ToString().ToUpper().StartsWith("ASC")) ? "asc" : "desc";

                StringBuilder sBuilder = new StringBuilder();
                int rowCount = 0;

                var result = OrderRepository.ListOrderStatusShipped(id, page, pageSize, orderBy, order);

                if (result.Count > 0)
                {
                    foreach (var orderPayment in result)
                    {
                        sBuilder.Append("<tr>")
                        .AppendCell(orderPayment.LogDateUTC.ToString())
                        .AppendCell(orderPayment.Description)
                        .AppendCell(orderPayment.TrackingNumber.ToString())
                        .AppendCell(orderPayment.Name)
                        .Append("</tr>");
                        rowCount = orderPayment.RowTotal;
                    }

                    int totalPages = (rowCount / pageSize);
                    totalPages = totalPages == 0 ? 1 : totalPages;

                    return Json(new { result = true, totalPages = totalPages, page = sBuilder.ToString() });
                }
                else
                    return Json(new { result = true, totalPages = 0, page = "<tr><td colspan=\"4\">" + Translation.GetTerm("ThereWereNoRecordsFoundThatMeetThatCriteriaPleaseTryAgain", "There were no records found that meet that criteria. Please try again.") + "</td></tr>" });

            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "PagedGridData")]
        public virtual ActionResult ListOrderStatusInvoice(string id, int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection)
        {
            try
            {
                var order = (orderByDirection.ToString().ToUpper().StartsWith("ASC")) ? "asc" : "desc";

                StringBuilder sBuilder = new StringBuilder();
                int rowCount = 0;

                var result = OrderRepository.ListOrderStatusInvoice(id, page, pageSize, orderBy, order);

                if (result.Count > 0)
                {
                    foreach (var orderPayment in result)
                    {
                        sBuilder.Append("<tr>")
                        .AppendCell(orderPayment.InvoiceNumber.ToString())
                        .AppendCell(orderPayment.DateInvoice.ToString("dd/MM/yyyy"))
                        .Append("</tr>");
                        rowCount = orderPayment.RowTotal;
                    }

                    int totalPages = (rowCount / pageSize);
                    totalPages = totalPages == 0 ? 1 : totalPages;

                    return Json(new { result = true, totalPages = totalPages, page = sBuilder.ToString() });
                }
                else
                    return Json(new { result = true, totalPages = 0, page = "<tr><td colspan=\"2\">" + Translation.GetTerm("ThereWereNoRecordsFoundThatMeetThatCriteriaPleaseTryAgain", "There were no records found that meet that criteria. Please try again.") + "</td></tr>" });

            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        //@01 AFIN

        [FunctionFilter("Orders", "~/Accounts")]
        public virtual ActionResult Cancel(string orderNumber)
        {
            try
            {
                string message = null;
                bool result = false;
                Order order = Order.LoadByOrderNumberFull(orderNumber);
                int OrderStatusID = order.OrderStatusID;
                int ppt = order.OrderCustomers[0].ProductPriceTypeID;// OrderContext.Order.AsOrder().OrderCustomers[0].ProductPriceTypeID;
                int RowDispatchControlDel = ShippingCalculatorExtensions.DispatchItemControlsDel(order.OrderCustomers[0].AccountID, order.OrderID);
                foreach (var item in ShippingCalculatorExtensions.GetProductQuantity(orderNumber))
                {
                    Order.GenerateAllocation(item.ProductID,
                                             item.Quantity,
                                             order.OrderID,
                                             Convert.ToInt32(Session[SessionConstants.WareHouseId]),
                                             EntitiesEnums.MaintenanceMode.Delete, 
                                             Convert.ToInt32(Session[SessionConstants.PreOrder]),                                             
                                             CoreContext.CurrentAccount.AccountTypeID, false);
                }
                if (order != null)
                {
                    result = OrderService.TryCancel(order, out message);
                }

                if (OrderContext.Order != null && result)
                {                    
                    if (OrderContext.Order.OrderID == order.OrderID)
                    {
                        OrderContext.Clear();
                    }
                }

                if (!result && String.IsNullOrWhiteSpace(message))
                {
                    message = Translation.GetTerm("CannotCancelOrder", "Cannot Cancel Order. Unknown Reason.");
                }

                return Json(new { result = result, message = message });
            }
            catch (Exception ex)
            {
                int orderId;
                if (!Int32.TryParse(orderNumber, out orderId))
                {
                    orderId = 0;
                }
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, orderID: orderId);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        protected IEnumerable<OrderPackageInfoModel> PackageInfoModels(OrderShipment shipment)
        {
            var packageInfoModels = new List<OrderPackageInfoModel>();
            if (shipment.OrderShipmentPackages.Count == 0)
            {
                if (shipment.Order.OrderCustomers.SelectMany(x => x.OrderItems).Any(x => Inventory.GetProduct(x.ProductID ?? 0).ProductBase.IsShippable))
                {
                    packageInfoModels.Add(new OrderPackageInfoModel
                    {
                        OrderCustomerID = shipment.OrderCustomerID,
                        ShipMethodName = shipment.ShippingMethodName,
                        BaseTrackUrl = shipment.TrackingURL,
                        TrackingNumber = shipment.TrackingNumber,
                        ShipDate = shipment.DateShipped ?? DateTime.MinValue
                    });
                }
            }
            else
            {
                AddAllPackagesToTheList(shipment, packageInfoModels);
            }

            return packageInfoModels;
        }

        protected void AddAllPackagesToTheList(OrderShipment shipment, IList<OrderPackageInfoModel> packageInfoModels)
        {
            foreach (var package in shipment.OrderShipmentPackages)
            {
                var packageItem = GetOrderPackageInformation(shipment, package);
                packageInfoModels.Add(packageItem);
            }
        }

        protected virtual OrderPackageInfoModel GetOrderPackageInformation(OrderShipment shipment, OrderShipmentPackage package)
        {
            var model = new OrderPackageInfoModel();
            model.OrderCustomerID = shipment.OrderCustomerID;
            model.ShipMethodName = package.ShippingMethodName;
            model.ShipDate = package.DateShipped;
            model.TrackingNumber = package.TrackingNumber;
            if (package.ShippingMethodID != null)
            {
                model.BaseTrackUrl = OrderShipment.GetBaseTrackUrl(package.ShippingMethodID.Value, package.TrackingNumber);
            }

            return model;
        }

        #region Modifications @02

        public ActionResult ClaimItems(string orderNumber)
        {
            int? orderId = null;
            try
            {
                TempData["sReturnReasons"] = from x in PaymentsMethodsExtensions.GetReturnReasons()
                                             orderby x.Key
                                             select new SelectListItem()
                                             {
                                                 Text = x.Value,
                                                 Value = x.Key
                                             };
                if (string.IsNullOrEmpty(orderNumber))
                    return RedirectToAction("Index");

                Order order = Order.LoadByOrderNumberFull(orderNumber);
                orderId = order.OrderID;

                int SupportTicketID = Request.QueryString["SupportTicketID"] != null ? 10000 + Convert.ToInt32(Request.QueryString["SupportTicketID"]) : 0;

                ViewBag.supportTicket = SupportTicketID;// new OrderItem().GetSupportTicketID(order.OrderNumber);

                foreach (OrderCustomer customer in order.OrderCustomers)
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

                return View("ClaimItems", "Orders", order);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, orderID: orderId.ToIntNullable());
                throw exception;
            }
        }

        public JsonResult GetItemsToClaim(string orderNumber)
        {

            StringBuilder builder = new StringBuilder();
            string culture = String.Empty;
            //var Oitem = OrderContext.Order.AsOrder().OrderCustomers[0].OrderItems;

            //foreach (var item in Oitem.Where(x => x.ItemPrice > 0))
            //{
            //    builder.Append("<tr>")
            //    .AppendCheckBoxCell(String.Format("chk_{0}", item.OrderItemID))
            //    .AppendCell(item.SKU)
            //    .AppendCell(item.ProductName) 
            //    .AppendCell(item.Quantity.ToString())
            //    .AppendCell(String.Format("<input type='text' value='{0}' class='numeric' disabled='disabled' />", item.Quantity))
            //    .AppendCell(item.ItemPrice.ToString(OrderContext.Order.CurrencyID))
            //    .AppendCell((item.Quantity * item.ItemPrice).ToString(OrderContext.Order.CurrencyID))
            //    .Append("</tr>");

            //    //culture = String.Format("{0}|{1}", OrderContext.Order.CurrencyID);
            //    if (item.ChildOrderItems.Count > 0)
            //    {
            //        builder.Append("<tr>")
            //        .AppendCheckBoxCell(String.Format("chk_{0}", item.OrderItemID))
            //        .AppendCell(item.SKU)
            //        .AppendCell(item.ProductName)
            //        .AppendCell(item.Quantity.ToString())
            //        .AppendCell(String.Format("<input type='text' value='{0}' class='numeric' disabled='disabled' />", item.Quantity))
            //        .AppendCell(item.ItemPrice.ToString(OrderContext.Order.CurrencyID))
            //        .AppendCell((item.Quantity * item.ItemPrice).ToString(OrderContext.Order.CurrencyID))
            //        .Append("</tr>");
            //        //culture = String.Format("{0}|{1}", OrderContext.Order.CurrencyID);
            //    }
            //}

            var list = new OrderItem().LoadItemsToClaim(orderNumber);

            //foreach (var item in list)
            //{
            //    builder.Append("<tr>")
            //    .AppendCheckBoxCell(String.Format("chk_{0}", item.OrderItemID))
            //    .AppendCell(item.SKU)
            //    .AppendCell(item.ProductName)
            //    .AppendCell(item.ProductType)
            //    .AppendCell(item.OrderQuantity.ToString())
            //    .AppendCell(String.Format("<input type='text' value='{0}' class='numeric' disabled='disabled' />", item.ClaimQuantity))
            //    .AppendCell(item.PricePerItem.ToString(OrderContext.Order.CurrencyID))
            //    .AppendCell((item.ClaimQuantity * item.PricePerItem).ToString(OrderContext.Order.CurrencyID))
            //    .Append("</tr>");

            //    culture = String.Format("{0}|{1}", item.CurrencyCode, item.CultureInfo);
            //}

            return Json(new { rows = builder.ToString(), cultureInfo = culture }, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult ClaimItemsByOrderNumber(Dictionary<int, int> listToClaim, string orderNumber, string ticketNumber)
        //{

        public virtual ActionResult ClaimItemsByOrderNumber(int originalOrderId, bool refundOriginalPayments, Dictionary<int, decimal> refundPayments, int returnType, string invoiceNotes, bool creditType, string creditAmount, List<ReturnOrderItemDto> OrderItemList)
        {
            int ticketNumber = 0;
            if (ticketNumber < 1 && Request.QueryString["SupportTicketID"] != null)
                return Json(new
                {
                    success = false,
                    fatal = false,
                    message = Translation.GetTerm("TicketSupportRequired", "Ticket support is required.")
                }, JsonRequestBehavior.AllowGet);

            if (OrderItemList == null || OrderItemList.Count() == 0)
                return Json(new
                {
                    success = false,
                    fatal = false,
                    message = Translation.GetTerm("SelectItemsToClaim", "Select at least one item to claim.")
                }, JsonRequestBehavior.AllowGet);
            Dictionary<int, int> listToClaim = new Dictionary<int, int>();
            foreach (var item in OrderItemList)
            {
                listToClaim.Add(item.OrderItemID, item.Quantity);
            }

            int orderId = OrderExtensions.InsOrderClains(OrderContext.Order.OrderID);
            if (orderId > 0)
            {
                int orderCustomerId = OrderExtensions.InsOrderCustomerClains(OrderContext.Order.AsOrder().OrderCustomers[0].OrderCustomerID, orderId);
                int orderItem = 0;
                foreach (var item in OrderItemList)
                {
                    if (item.ParentOrderItemID == null)
                    {
                        orderItem = OrderExtensions.OrderItemClains(item.OrderItemID, OrderContext.Order.AsOrder().OrderCustomers[0].OrderCustomerID, orderCustomerId, null);
                    }
                    else
                    {
                        OrderExtensions.OrderItemClains(item.OrderItemID, OrderContext.Order.AsOrder().OrderCustomers[0].OrderCustomerID, orderCustomerId, orderItem);
                    }
                    //item.ParentOrderItemID;
                }
                //
            }
            bool rpta = new OrderItem().ClaimOrderItems(listToClaim, OrderContext.Order.AsOrder().OrderNumber, "");
            //bool rpta = true;
            //// key: OrderItemID - Value: ClaimQuantity
            string OrderNumber = Convert.ToString(orderId);
            return Json(new { result = true, orderNumber = OrderNumber });
        }

        #endregion

        [HttpPost]
        public virtual ActionResult ValidateSendEmailBoleto(int orderNumber)
        {
            try
            {

                List<PaymentInfoBancoOrden> lstInformacionFacturacion = PaymetTycketsReportBusinessLogic.GetInformacionOrder(orderNumber);

                string BankCode = "";
                int OrderPaymentID = 0;
                bool Result = false;
                if (lstInformacionFacturacion != null)
                {
                    PaymentInfoBancoOrden obj = lstInformacionFacturacion.First();
                    if (obj.PaymentTypeID == 11)
                    {
                        var ENT = OrderExtensions.GeneralParameterVal(CoreContext.CurrentMarketId, "ENT");
                        if (ENT == "S")
                        {
                            Result = true;
                            OrderPaymentID = obj.OrderPaymentID;
                            BankCode = obj.BankCode.ToString();
                        }

                    }
                }


                return Json(
                           new
                           {
                               result = Result,
                               BankCode = BankCode,
                               OrderPaymentID = OrderPaymentID
                           });

            }

            catch (Exception ex)
            {
                return Json(
                  new
                  {
                      result = false

                  });
            }
        }

        public virtual ActionResult ListWarehouseMaterialLacks()
        {
            var listWarehouseMaterialLacks = PreOrderExtension.GetWarehouseMaterialLacksByPreOrder(Convert.ToInt32(Session["PreOrder"]), 5);
            return Json(new { result = true, listWarehouseMaterialLacks = listWarehouseMaterialLacks });
        }

        //impresion de documento de orden [Req:BR-PD-005]
        //creado por salcedo vila G. GYS
        //verificar que este el archivo nsCore/Reports/RptOrder.rdlc
        #region Impresion

        public virtual ActionResult PrintInvoicePDF(string orderNumber)
        {
            List<ReportParameter> lstParams = new List<ReportParameter>();
            lstParams = CreateParameterLabelReport();
            byte[] buffer = CreateReport(lstParams: lstParams, orderNumber: orderNumber);
            return File(buffer, "application/pdf", "Order" + orderNumber + ".pdf");
        }

        private byte[] CreateReport(List<ReportParameter> lstParams = null, string ddlFileFormat = "pdf", string nombreReporte = "Order", string orderNumber = "")
        {
            #region Paquete Documentario
            if(lstParams!=null)
             lstParams.Add(new ReportParameter("LblLanguanje", CoreContext.CurrentCultureInfo.Name));


            string contentType = string.Empty;
            if (ddlFileFormat.Equals(".pdf"))
                contentType = "application/pdf";
            if (ddlFileFormat.Equals(".doc"))
                contentType = "application/ms-word";
            if (ddlFileFormat.Equals(".xls"))
                contentType = "application/xls";
            DataSet dsData = new DataSet();

           var ip = CoreContext.CurrentCultureInfo.LCID;
            dsData = OrderReportBusinessLogic.OrderSearch(orderNumber, CoreContext.CurrentLanguageID);

           

            dsData.Locale = CoreContext.CurrentCultureInfo;

            string extension;
            string encoding;
            string mimeType;
            string[] streams;
            Warning[] warnings;
            LocalReport report = new LocalReport();
            report.ReportPath = Server.MapPath("~/Reports/RptOrder.rdlc");

            ReportDataSource rdsOrdenProductos = new ReportDataSource();
            rdsOrdenProductos.Name = "DstOrdenProductos";//This refers to the dataset name in the RDLC file  
            rdsOrdenProductos.Value = dsData.Tables[0];

            ReportDataSource rdsdtPaymentsMade = new ReportDataSource();
            rdsdtPaymentsMade.Name = "dstpaymentsMade";//This refers to the dataset name in the RDLC file  
            rdsdtPaymentsMade.Value = dsData.Tables[1];

            ReportDataSource rdsdtPromotions = new ReportDataSource();
            rdsdtPromotions.Name = "DtsPromotions";//This refers to the dataset name in the RDLC file  
            rdsdtPromotions.Value = dsData.Tables[2];

            ReportDataSource rdsdtDetails = new ReportDataSource();
            rdsdtDetails.Name = "dtsDetails";//This refers to the dataset name in the RDLC file  
            rdsdtDetails.Value = dsData.Tables[3];


            ReportDataSource rdsdtConsultora = new ReportDataSource();
            rdsdtConsultora.Name = "dtConsultora";//This refers to the dataset name in the RDLC file  
            rdsdtConsultora.Value = dsData.Tables[4];

            ReportDataSource rdsdtVariables = new ReportDataSource();
            rdsdtVariables.Name = "dtCVVariables";//This refers to the dataset name in the RDLC file  
            rdsdtVariables.Value = dsData.Tables[5];

            ReportDataSource rdsdtOrderPeriods = new ReportDataSource();
            rdsdtOrderPeriods.Name = "dtOrderPeriods";//This refers to the dataset name in the RDLC file  
            rdsdtOrderPeriods.Value = dsData.Tables[6];

            ReportDataSource rdsdtIncentivos = new ReportDataSource();
            rdsdtIncentivos.Name = "dtIncentivos";//This refers to the dataset name in the RDLC file  
            rdsdtIncentivos.Value = dsData.Tables[7];

            ReportDataSource rdsdstOrder = new ReportDataSource();
            rdsdstOrder.Name = "dstOrder";//This refers to the dataset name in the RDLC file  
            rdsdstOrder.Value = dsData.Tables[8];

            ReportDataSource rdsdstTitle23 = new ReportDataSource();
            rdsdstTitle23.Name = "dstTitle23";//This refers to the dataset name in the RDLC file  
            rdsdstTitle23.Value = dsData.Tables[9];

            ReportDataSource rdsdstDataSection23 = new ReportDataSource();
            rdsdstDataSection23.Name = "dstDataSection23";//This refers to the dataset name in the RDLC file  
            rdsdstDataSection23.Value = dsData.Tables[10];

            ReportDataSource rdsdstTitle24 = new ReportDataSource();
            rdsdstTitle24.Name = "dstTitle24";//This refers to the dataset name in the RDLC file  
            rdsdstTitle24.Value = dsData.Tables[11];

            ReportDataSource rdsdstDataSection24 = new ReportDataSource();
            rdsdstDataSection24.Name = "dstDataSection24";//This refers to the dataset name in the RDLC file  
            rdsdstDataSection24.Value = dsData.Tables[12];

            report.DataSources.Add(rdsOrdenProductos);
            report.DataSources.Add(rdsdtDetails);
            report.DataSources.Add(rdsdtPromotions);
            report.DataSources.Add(rdsdtPaymentsMade);
            report.DataSources.Add(rdsdstOrder);

            report.DataSources.Add(rdsdtConsultora);
            report.DataSources.Add(rdsdtOrderPeriods);
            report.DataSources.Add(rdsdtVariables);
            report.DataSources.Add(rdsdtIncentivos);

            report.DataSources.Add(rdsdstTitle23);
            report.DataSources.Add(rdsdstDataSection23);
            report.DataSources.Add(rdsdstTitle24);
            report.DataSources.Add(rdsdstDataSection24);

            if (lstParams != null)
            {
                report.SetParameters(lstParams);
            }
            Byte[] mybytes = report.Render(ddlFileFormat, null,
                                out extension, out encoding,
                                out mimeType, out streams, out warnings); //for exporting to PDF  

            #endregion

            #region Agrega Ticket a Paquete Documentario

            int TicketNumber = dsData.Tables[13].Rows.Count > 0 ? Convert.ToInt32(dsData.Tables[13].Rows[0]["TicketNumber"]) : 0;
            int BankCode = dsData.Tables[13].Rows.Count > 0 ? Convert.ToInt32(dsData.Tables[13].Rows[0]["BankCode"]) : 0;
            Byte[] ResponseFile = null;

            switch (BankCode)
            {
                case 1:// "Banco Do Brasil":
                    ResponseFile = CrearTicketBB(TicketNumber);
                    break;
                case 104://"Caixa":
                    ResponseFile = CrearTicketCaixa(TicketNumber);
                    break;
                case 341://"Itaú":
                    ResponseFile = CrearTicketItau(TicketNumber);
                    break;
                default:
                    break;
            }

            #endregion

            #region Retorna Reporte

            List<byte[]> ListPDFs = new List<byte[]>();
            ListPDFs.Add(mybytes);
            if (ResponseFile != null) ListPDFs.Add(ResponseFile);

            //return MergePDFs(new List<byte[]>() { mybytes, ResponseFile }); ;
            return Pdf.MergePDFs(ListPDFs);

            #endregion
        }

        #region Utilidades

        private static List<ReportParameter> CreateParameterLabelReport()
        {

            #region listas  parametros
            string[] Etiquetas =
        {
            "LblPedidoNro",
            "LblTituloReporte",
            "LblData",
            "LblCiclo",
            "lblNombre",
            "LblDireccion",
            "LblTransportadora",
            "LbltemsPedido",
            "LblCod",
            "LblCantidad",
            "LblProduto",
            "LblCredito",
            "LblPrecioMenor",
            "LblPrecioFinal",
            "LblTipoOrden",           
            "LblPlazo",
            "lblAjustes",
            "LblValorSubTotal",

            "LblValorEntrega",
            "LblTotalDetails",
            "LblPago",
            "LblNroDoc",
            "LblForma",
            "LblVencimiento",
            "LblCreditoDetail",
            "LblPagarDetail",
            "LblProductoPromocion",
            "LblCodProdPromocion",
            "LblCantidadProdPromocion",
            "LblProdPromocion",
            "LblPromocionNombre",
            "LblPremioPromocion",
            "LblBelcorpNews",
            "LblRptOrderText1",
            "LblRptOrderText2",
            "LblRptOrderText3",
            "LblRptOrderText4",
            "LblRptOrderText5",
            "LblRptOrderText6",
            "LblRptOrderText7",
            "LblRptOrderText8",
            "LblRptOrderText9",
            "LblRptOrderText10"

        };
            string[] KeyTranslate =
         {
             "OrderNumber",
             "OrderDetails",
             "RptDta",
             "CompletePeriod",
             "AccountName",
             "ShippingAddress",
             "LogisticProvider",
             "YourOrderedItems",
             "SKU",
             "Quantity",
             "ProductName",
             "QV",
             "RetailPrice",
             "FinalPrice",
             "OrderItemType",
             "DaysForDelivery",
             "RptSettings",
             "OrderSubtotal",

             "ShippingFee",
             "TotalAmountToPay",
             "PaymentsMade",
             "TicketNumber",
             "PaymentType",
             "CurrentExpirationDateUTC",
             "DisccountedAmount",
             "TotalAmountToPay",
             "Promotions",
             "SKU",
             "Quantity",
             "ProductName",
             "Promotions_PromotionNameLabel",
             "PromotionRewardKindName",
             "BelcorpNews",
             "RptOrderText1",
             "RptOrderText2",
             "RptOrderText3",
             "RptOrderText4",
             "RptOrderText5",
             "RptOrderText6",
             "RptOrderText7",
             "RptOrderText8",
             "RptOrderText9",
             "RptOrderText10"
         };

            string[] ValorDafault =
        {
            "PEDIDO Nro",
            "EXTRACTO DE PEDIDO",
            "DATA",
            "CICLO",
            "NOMBRE",
            "DIRECCION",
            "TRANSPORTADORA",
            "PRODUCTOS DE PEDIDO",
            "COD.",
            "QTD",
            "PRODUCTO",
            "CRÉDITO",
            "PRECIO MENOR",
            "PRECIO A PAGAR",
            "DESCRIPCION",           
            "PLAZO",
            "AJUSTES",
            "VALOR SUBTOTAL",

            "VALOR ENTREGA",
            "TOTAL",
            "PAGO",
            "Nro.DOC.",
            "FORMA",
            "VENCIMIENTO",
            "CRÉDITO",
            "A PAGAR",
            "PRODUCTOS PROMOCIONADOS",
            "COD.",
            "QTD",
            "PRODUCTO",
            "PROMOCION",
            "PREMIO",
            "BELCORP NEWS",
            "RptOrderText1",
            "RptOrderText2",
            "RptOrderText3",
            "RptOrderText4",
            "RptOrderText5",
            "RptOrderText6",
            "RptOrderText7",
            "RptOrderText8",
            "RptOrderText9",
            "RptOrderText10"
        };
            #endregion

            List<ReportParameter> lstParams = new List<ReportParameter>();

            for (int indice = 0; indice < Etiquetas.Length; indice++)
            {
                lstParams.Add
                         (
                             new ReportParameter(Etiquetas[indice], Translation.GetTerm(KeyTranslate[indice], ValorDafault[indice]))
                         );

            }
            return lstParams;
        }

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

        /// <summary>
        /// crear el codigo de barra con el texto generado
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
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

        #endregion

    }
}