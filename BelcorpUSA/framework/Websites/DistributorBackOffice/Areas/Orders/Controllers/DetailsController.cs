using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;
using System.Web.Mvc;
using DistributorBackOffice.Areas.Account.Controllers;
using DistributorBackOffice.Areas.Orders.Models.Shared;
using NetSteps.Common.Base;
using NetSteps.Encore.Core.IoC;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Controls.Services;
using NetSteps.Web.Mvc.Controls.Services.Interfaces;
using Microsoft.Reporting.WebForms;
using System.Data;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business.HelperObjects.OrderPackages;
using NetSteps.Data.Entities.Business;
using System.Linq;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities.Business.Logic;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Cache;
using NetSteps.Common.Configuration;
using NetSteps.Web.Mvc.Helpers;
using NetSteps.Data.Entities.Dto;
using CodeBarGeerator;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Threading;

namespace DistributorBackOffice.Areas.Orders.Controllers
{
    public class DetailsController : BaseAccountsController
    {
        private IDetailsControllerService _service;
        public IDetailsControllerService Service
        {
            get
            {
                if (_service == null)
                    _service = Create.New<IDetailsControllerService>();
                return _service;
            }
        }


        [FunctionFilter("Orders", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public virtual ActionResult Index(string id)
        {

            //var p1 = CoreContext.CurrentCultureInfo;
            //var p3 = CoreContext.CurrentLanguage;
            //var p4 = Thread.CurrentThread.CurrentCulture;
            //var p2 = CoreContext.CurrentLanguageID;
            Session["ProductCredit"] = Order.GetProductCreditByAccountDet(id.ToInt());

            if (string.IsNullOrEmpty(id))
            {
                TempData["Error"] = "Invalid Order. Order # not specified: ";
                return RedirectToAction("Index", "Landing");
            }

            if (!Order.ExistsByOrderNumber(id))
            {
                TempData["Error"] = "No order found for: " + id;
                return RedirectToAction("Index", "Browse", new { orderNumber = id });
            }

            Order order = Service.LoadOrder(id);
            if (order == null)
            {
                TempData["Error"] = "No order found for: " + id;
                return RedirectToAction("Index", "Landing");
            }

            foreach (var item in ShippingCalculatorExtensions.GetPreOderDetail(Convert.ToInt32(id)))
            {
                Session["WareHouseId"] = item.WareHouseID;
                Session["PreOrder"] = item.PreOrderId;
            }

            if (Service.IsPartyOrder(order))
                return RedirectToAction("Receipt", "Party", new { orderId = order.OrderID });

            //If the order is in a non completed state 
            //(i.e. CC declined) then reload the order in the order entry so the user can
            //Rectify the issue and resolve the order - Scott Wilson
            if (Service.IsOrderNotComplete(order))
                return RedirectToAction("Index", "OrderEntry", new { orderTypeID = order.OrderTypeID, gd = false, orderId = order.OrderID });

            var packageHelper = Create.New<IOrderPackageInfoHelper>();
            var restockingFeeProduct = Order.GetRestockingFeeProduct();
            ViewBag.PackageInfoList = packageHelper.GetOrderPackageInfoList(order);
            ViewBag.RestockingFeeSku = restockingFeeProduct == null ? string.Empty : restockingFeeProduct.SKU;

            var model = new IndexModel
            {
                Order = order,
                OrderItemDetails = order.OrderCustomers
                                .SelectMany(oc => oc.ParentOrderItems)
                                .Select(GetOrderDetailModel)
                                .ToList()
            };

            return View(model);

        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Orders", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public virtual ActionResult GetPayment(int paymentId)
        {
            OrderPayment payment = Service.LoadPayment(paymentId);
            var model = Service.GetPaymentMethodModel(payment);
            return Json(new { result = true, paymentHTML = RenderPartialToString(model.GetPartialViewName(), model) });
        }

        #region PDF

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult GenerarReportePDF(string orderNumber)
        {
            List<string> OrderList = new List<string>();

            OrderList.Add(orderNumber);
            return new NetSteps.Web.Mvc.ActionResults.PdfResult(string.Format("Order{0}.pdf", orderNumber), Pdf.GeneratePDFMemoryStream(OrderList));

        }

        #endregion

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
                BankName = payment.BankName,
                RoutingNumber = payment.RoutingNumber,
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

        protected IEnumerable<OrderPackageInfoModel> PackageInfoModels(OrderShipment shipment)
        {
            var packageInfoModels = new List<OrderPackageInfoModel>();

            if (shipment.OrderShipmentPackages.Count == 0)
            {
                packageInfoModels.Add(new OrderPackageInfoModel
                {
                    ShipMethodName = shipment.ShippingMethodName,
                    BaseTrackUrl = shipment.TrackingURL,
                    TrackingNumber = shipment.TrackingNumber,
                    ShipDate = shipment.DateShipped ?? System.DateTime.MinValue
                });
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
                var packageItem = GetOrderPackageInformation(package);
                packageInfoModels.Add(packageItem);
            }
        }

        protected virtual OrderPackageInfoModel GetOrderPackageInformation(OrderShipmentPackage package)
        {
            var model = new OrderPackageInfoModel();
            model.ShipMethodName = package.ShippingMethodName;
            model.ShipDate = package.DateShipped;
            model.TrackingNumber = package.TrackingNumber;
            if (package.ShippingMethodID != null)
                model.BaseTrackUrl = OrderShipment.GetBaseTrackUrl(package.ShippingMethodID.Value, package.TrackingNumber);

            return model;
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





        //[FunctionFilter("Orders", "~/Accounts")]
        //[OutputCache(CacheProfile = "PagedGridData")]
        //[FunctionFilter("Orders", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public virtual ActionResult Tracking(string orderNumber)
        {
            ViewData["Message"] = "Test";
            Order order = Service.LoadOrder(orderNumber);
            var result = Order.GetExpectedDeliveryDateByOrderNumber(orderNumber);
            ViewData["DeliveryDateUTC"] = (result.Equals(null) || result.ToString("dd/MM/yyyy").Equals("01/01/1900") ? "" : result.ToString("dd/MM/yyyy"));

            var model = new IndexModel
            {
                Order = order,
                OrderItemDetails = order.OrderCustomers
                                .SelectMany(oc => oc.ParentOrderItems)
                                .Select(GetOrderDetailModel)
                                .ToList()
            };

            ViewData["OrderNumber"] = orderNumber;

            return View(model);
        }

        public virtual ActionResult TrackingDetail(string id, int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                var order = (orderByDirection.ToString().ToUpper().StartsWith("ASC")) ? "asc" : "desc";
                int rowCount = 0;
                //var result = Order.SearchOrderTrackingByOrderNumberDWS(id, page, pageSize, orderBy, order);
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
                            string textoViewDetaillTracking = Translation.GetTerm("ViewDetaillTracking", "View detaill");
                            builder.AppendCell(" <a title='" + textoViewDetaillTracking + "' style='text-decoration: underline;color:blue' id='" + key + "' href='javascript:void(0)' class='ShowPopup'>" + OrderTracking.Name + " <img style='margin-left:4px' src=\"/Content/Images/imgViewDetaill.PNG\" width=\"15\" height=\"15\" > </a>");
                        }

                        //builder.AppendCell(OrderTracking.InitialTackingDateUTC.Year == 1900 && OrderTracking.InitialTackingDateUTC.Day == 1 && OrderTracking.InitialTackingDateUTC.Month == 1 ? "" : OrderTracking.InitialTackingDateUTC.ToString("dd/MM/yyyy hh:mm"));
                        //builder.AppendCell(OrderTracking.FinalTackingDateUTC.Year == 1900 && OrderTracking.FinalTackingDateUTC.Day == 1 && OrderTracking.FinalTackingDateUTC.Month == 1 ? "" : OrderTracking.FinalTackingDateUTC.ToString("dd/MM/yyyy hh:mm"));

                        builder.AppendCell(OrderTracking.InitialTackingDateUTC.Year == 1900 && OrderTracking.InitialTackingDateUTC.Day == 1 && OrderTracking.InitialTackingDateUTC.Month == 1 ? "" : OrderTracking.InitialTackingDateUTC.ToString(CoreContext.CurrentCultureInfo));
                        builder.AppendCell(OrderTracking.FinalTackingDateUTC.Year == 1900 && OrderTracking.FinalTackingDateUTC.Day == 1 && OrderTracking.FinalTackingDateUTC.Month == 1 ? "" : OrderTracking.FinalTackingDateUTC.ToString(CoreContext.CurrentCultureInfo));


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
                         .AppendCell(orderPayment.DateInvoice.ToString(CoreContext.CurrentCultureInfo))
                        //.AppendCell(orderPayment.DateInvoice.ToString("dd/MM/yyyy"))
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
                        var ENT = OrderExtensions.GeneralParameterVal(NetSteps.Web.Mvc.Helpers.CoreContext.CurrentMarketId, "ENT");
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

        /*CS:18ABR2016.Inicio*/
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

                return View("Invoices", order);
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

        /*CS:18ABR2016.Fin*/

        public virtual ActionResult ListWarehouseMaterialLacks()
        {
            var listWarehouseMaterialLacks = PreOrderExtension.GetWarehouseMaterialLacksByPreOrder(Convert.ToInt32(Session["PreOrder"]), 5);//CurrentAccount.Language.LanguageID
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

            if (lstParams!=null)
                lstParams.Add(new ReportParameter("LblLanguanje", CoreContext.CurrentCultureInfo.Name));


            string contentType = string.Empty;
            if (ddlFileFormat.Equals(".pdf"))
                contentType = "application/pdf";
            if (ddlFileFormat.Equals(".doc"))
                contentType = "application/ms-word";
            if (ddlFileFormat.Equals(".xls"))
                contentType = "application/xls";
            DataSet dsData = new DataSet();
            dsData = NetSteps.Data.Entities.Business.Logic.OrderReportBusinessLogic.OrderSearch(orderNumber, CoreContext.CurrentLanguageID);

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
