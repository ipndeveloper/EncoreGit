//Modifications:
//@01 20150716 BR-AT-005 G&S PGCT: Create imputs and functions needed for the requeriment
namespace nsCore.Areas.Orders.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using NetSteps.Common.Extensions;
    using NetSteps.Common.Globalization;
    using NetSteps.Data.Entities;
    using NetSteps.Data.Entities.Business.Logic;
    using NetSteps.Data.Entities.Exceptions;
    using NetSteps.Data.Entities.Extensions;
    using NetSteps.Data.Entities.Repositories;
    using NetSteps.Web.Extensions;
    using NetSteps.Web.Mvc.Helpers;
    using nsCore.Contexts;
    using nsCore.Controllers;
    using AvataxAPI = NetSteps.Data.Entities.AvataxAPI;
    using System.Text;
    using NetSteps.Web;
    using NetSteps.Encore.Core.IoC;
    using NetSteps.Data.Entities.Business;
    using NetSteps.Common.Configuration;
    using NetSteps.Data.Entities.Utility;
    using System.IO;

    /// <summary>
    /// Controlador para Order Item Pending Confirm
    /// </summary>
    public partial class PendingConfirmController : BaseController
    {
        #region privates
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
        #endregion

        #region show Order Product  detaill confirm
        /// <summary>
        /// Index de controlador - Developed By GSV - GYS
        /// </summary>
        /// <returns>Retorna la Vista de Index lista de ordenes con el estado Pending confirm</returns>
        public virtual ActionResult Index(int id = 0)
        {
            return View();
        }

        /// <summary>
        /// Grilla de order item confirms -Developed By GSV - GYS
        /// </summary>
        /// <returns>Retorna la grilla paginada con las ordenes con estado pending confirm</returns>      
        [OutputCache(CacheProfile = "PagedGridData")]
        public virtual ActionResult GetOrdersPending(
            int page,
            int pageSize,
            string OrderNumber,
            string AccountNumber,
            string numberSupportTicket,
            string numberMail,
            DateTime? startDate,
            DateTime? endDate,
            string orderBy,
            NetSteps.Common.Constants.SortDirection orderByDirection)
        {
            page = page + 1;
            try
            {
                List<NombreColumnas> Filtros = new List<NombreColumnas>();

                if (AccountNumber.Trim() != "")
                    Filtros.Add(new NombreColumnas() { NombreColumna = "AccountNumber", ValorColumna = AccountNumber });
                if (numberSupportTicket.Trim() != "")
                    Filtros.Add(new NombreColumnas() { NombreColumna = "IDSupportTicket", ValorColumna = numberSupportTicket });
                if (numberMail.Trim() != "")
                    Filtros.Add(new NombreColumnas() { NombreColumna = "IDNationalMail", ValorColumna = numberMail });
                if (startDate.HasValue && startDate.Value.Year < 1900)
                    startDate = null;
                if (endDate.HasValue && endDate.Value.Year < 1900)
                    endDate = null;

                StringBuilder builder = new StringBuilder();

                var lstOrderPending = OrderPendingCofirmBusinessLogic.Instance.OrderSearch(startDate, endDate, OrderNumber);
                paginar<Order> pros = new paginar<Order>(page, pageSize, lstOrderPending);
                var listado = pros.Filtrar(Filtros)
                                  .Ordenar(orderBy, orderByDirection)
                                  .Paginar()
                                  .lista
                                  .ToList();

                int total = 0;
                total = listado.Count();
                if (total > 0)
                {
                    for (int index = 0; index < total; index++)
                    {
                        builder.Append("<tr>")
                               .AppendLinkCell("~/Orders/Return/Index/" + listado[index].OrderID, listado[index].OrderNumber)
                               .AppendCell(listado[index].AccountNumber)
                               .AppendCell(listado[index].CustomerFirstName)
                               .AppendCell(listado[index].CustomerLastName)
                               .AppendCell(Convert.ToString(listado[index].IDSupportTicket))
                               .AppendCell(listado[index].IDNationalMail)
                               .AppendCell(listado[index].DateCreatedUTC.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo))
                               .Append("</tr>");
                    }

                    return Json(new { result = true, totalPages = pros.totalpages, page = builder.ToString(), totalCount = total });
                }
                else
                    return Json(new { result = true, totalPages = 0, page = "<tr><td colspan=\"9\">" + Translation.GetTerm("ThereWereNoRecordsFoundThatMeetThatCriteriaPleaseTryAgain", "There were no records found that meet that criteria. Please try again.") + "</td></tr>", totalCount = 0 });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }
     
        /// <summary>
        /// Obtiene el detalle de la orden por confirmar Developed By GSV - GYS
        /// </summary>
        /// <returns>Retorna el detalle de la orden seleccionada</returns>
        public virtual ActionResult DetaillProductPendingConfirm(int id)
        {
            try
            {
                Order order = Order.LoadFull(id);
                List<OrderItemReturnConfirm> lstOrderItemReturnConfirm = OrderPendingCofirmBusinessLogic.Instance.GetDetaillOrderItemReturnConfirm(order.OrderNumber);
                ViewData["lstOrderItemReturnConfirm"] = lstOrderItemReturnConfirm;
                ViewData["Note"] = order.Notes.Where(m => m.NoteTypeID == NetSteps.Data.Entities.Constants.NoteType.LogisticNotes.ToInt()).Select(m => m.NoteText).FirstOrDefault();
                Dictionary<string, string> dcObservaciones = new Dictionary<string, string>();
                foreach (var dc in lstOrderItemReturnConfirm)
                {
                    dcObservaciones[dc.OrderItemReturnID.ToString()] = dc.Note == null ? "" : dc.Note;
                }

                string JsonDc = dcObservaciones.ToJSON();
                ViewData["JsonDc"] = JsonDc;

                return View(order);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }
        #endregion 

        #region Insert Detaill

        /// <summary>
        /// SaveOrderReturn Developed By GSV - GYS
        /// </summary>
        /// <returns>Inserta la lista ordenItemReturnConfirm modificados </returns>
        [HttpPost]
        public virtual ActionResult InsertarOrderItemReturnConfirm(List<OrderItemReturnConfirm> lstOrderItemReturnConfirm, string orderNote, int orderId)
        {
            try
            {
                int noteTypeID = NetSteps.Data.Entities.Constants.NoteType.LogisticNotes.ToInt();
                int resultado = OrderPendingCofirmBusinessLogic.Instance.InsertOrderPendingCofirm(lstOrderItemReturnConfirm, orderId, orderNote, noteTypeID);
                bool result = resultado > 0;
                return Json(new { result = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = ex.Message });
            }
        }
        #endregion

        /// <summary>
        /// Confirm Order - es similar a "SubmitReturn" de PendingConfirm
        /// </summary>
        /// <param name="orderId">Id de Orden de retorno</param>
        /// <returns>Confirmacion o reclazo del proceso</returns>
        public virtual ActionResult ConfirmOrden(int orderId)
        {
            try
            {
                bool allowClearAllocation = false;
                bool allowSAP = false;
                bool allowApplyCredit = false;
                bool originalOrderWasCancelled = false;
                bool allowReverseStatusActivity = false;

                Order returnOrder = Order.LoadFull(orderId);
                OrderContext.Order = returnOrder;

                #region RetunOrder copy from SubmitReturn                
                
                var validationResponse = new overridereturn().ValidateReturnAccesible(returnOrder.ParentOrderID.Value, OrderContext.Order.AsOrder());
                if (!validationResponse.Success)
                    return Json(new { result = false, message = validationResponse.Message });

                returnOrder = OrderContext.Order.AsOrder();

                string message = string.Empty;
                bool result = true;

                foreach (OrderPayment orderPayment in returnOrder.OrderPayments)
                {
                    if (orderPayment.PaymentTypeID == (int)Constants.PaymentType.CreditCard)
                    {
                        var authResponse = returnOrder.Refund(orderPayment, orderPayment.Amount);
                        message = authResponse.Message;
                        result = authResponse.Success;
                    }
                    else // For a manual check, no processing will take place.
                    {
                        result = true;
                        orderPayment.OrderPaymentStatusID = (int)Constants.OrderPaymentStatus.Completed;
                        orderPayment.Save();
                    }
                }

                if (result)
                {
                    var totalPayments = returnOrder.OrderPayments.Sum(op => op.Amount);
                    returnOrder.PaymentTotal = totalPayments;
                    returnOrder.Balance = returnOrder.GrandTotal - totalPayments;
                    returnOrder.ChangeToPaidStatus();
                    returnOrder.CompleteDate = DateTime.Now;
                    returnOrder.CommissionDate = DateTime.Now;

                    var originalOrder = Order.Load(returnOrder.ParentOrderID.Value);
                    OrderBusinessLogic.ActionOnReturnOrCancelOrder(originalOrder.OrderStatusID, ref originalOrderWasCancelled, ref allowClearAllocation, ref allowApplyCredit, ref allowSAP, ref allowReverseStatusActivity);

                    returnOrder.UpdateInventoryLevels(true, originalOrderWasCancelled);
                    returnOrder.NegateValuesForReturn();
                    OrderContext.Order = returnOrder;
                    OrderContext.Order.Save();

                    if (!string.IsNullOrEmpty(NetSteps.Common.Configuration.ConfigurationManager.GetConfigValues(AvataxAPI.Constants.AVATAX_CONFIGSECTION, AvataxAPI.Constants.AVATAX_ACCOUNT).Trim()) && !string.IsNullOrEmpty(NetSteps.Common.Configuration.ConfigurationManager.GetConfigValues(AvataxAPI.Constants.AVATAX_CONFIGSECTION, AvataxAPI.Constants.AVATAX_LICENSE).Trim()))
                    {
                        OrderContext.Order.AsOrder().CancelTax();
                    }

                    #region Apply Credit - Clear Alocation                   
                    if (allowApplyCredit)
                    {
                        if (NeedsProductCredit(returnOrder))
                        {
                            List<OrderItemReturnConfirm> lstOrderItemReturnConfirm = OrderPendingCofirmBusinessLogic.Instance.GetDetaillOrderItemReturnConfirm(returnOrder.OrderNumber);
                            decimal creditAmountDecimal = lstOrderItemReturnConfirm.Sum(m => m.QuantityOrderItemReturnConfirm * m.ItemPrice);
                            ReturnController.ApplyCredit(returnOrder.OrderCustomers[0].AccountID, creditAmountDecimal, returnOrder.OrderID, returnOrder.ParentOrderID);
                        }
                    }

                    if (allowClearAllocation)
                    {
                        ReturnController.ClearAllocation(OrderContext.Order.AsOrder());
                    }

                    if (allowReverseStatusActivity)
                    {
                        ReturnController.ReverseStatusActivity(OrderContext.Order.AsOrder().OrderCustomers[0].AccountID, OrderContext.Order.ParentOrderID.Value, PeriodBusinessLogic.Instance.GetOpenPeriodID());
                    }
                    #endregion
                }
                if(result)
                {
                   #region Interfaz Orden Devolucion - SAP
                    //
                    Order ParentOrder=returnOrder.ParentOrder;
                    Action<int> GenerarXmlRetorno = (PoriginalOrderId) =>
                    {
                        var fechaActual = DateTime.Now;
                        string RutaUnica = "MLM_Pedidos_" + PoriginalOrderId.ToString() + "_" + fechaActual.Year.ToString() + fechaActual.Month.ToString() + fechaActual.Day.ToString() + fechaActual.Hour.ToString() + fechaActual.Minute.ToString() + fechaActual.Second.ToString();
                        RutaUnica += ".XML";
                        string ReturOrderItemDetaill = Server.MapPath(ConfigurationManager.AppSettings["TemplateOrderReturnItemDetaill"]);
                        string TemplateClientOrderReturn = Server.MapPath(ConfigurationManager.AppSettings["TemplateClientOrderReturn"]);
                        string MainRootServer = ConfigurationManager.AppSettings["MainRootServer"];
                        string SubDirectory = ConfigurationManager.AppSettings["SubDirectory"];

                        string data = XmlGeneratorBusinessLogic.Instance.GenerateXmlForReturnedOrder(TemplateClientOrderReturn, ReturOrderItemDetaill, PoriginalOrderId);

                        string rutaGuardar = Path.Combine(MainRootServer, SubDirectory, RutaUnica);
                        string RutaDiretorio = Path.Combine(MainRootServer, SubDirectory);
                        var ExistePath = Directory.Exists(RutaDiretorio);

                        if (ExistePath)
                        {
                            FileHelper.Save(data, rutaGuardar);
                        }
                    };
                    switch (ParentOrder.OrderStatusID)
                      {
                          case 8://Printed
                          case 20://Invoiced
                          case 21://Deliveried
                          case 9://Shipped
                              GenerarXmlRetorno(orderId);
                            break;
                        default:
                            break;
                      }
                    #endregion
                }
                #endregion

                return Json(new { result = result });
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = ex.Message });
            }
        }
                
        private bool NeedsProductCredit(Order order)
        {
            bool result = true;
            //Si todos son de tipo Produc Credit
            result = !order.OrderCustomers
                            .SelectMany(m => m.OrderPayments
                                              .Where(op => op.PaymentTypeID != (int)Constants.PaymentType.ProductCredit))
                            .Any();
            return result;
        }

        class overridereturn : ReturnController
        {
            /// <summary>
            /// Make accesible ValidateReturn base method
            /// </summary>
            /// <param name="originalOrderId">Original Order Id</param>
            /// <param name="returnOrder">Return Order</param>
            /// <returns>Basic Response Item</returns>
            public NetSteps.Common.Base.BasicResponseItem<Order> ValidateReturnAccesible(int originalOrderId, Order returnOrder)
            {
                return ValidateReturnExtended(originalOrderId, returnOrder);
            }

            /// <summary>
            /// Base ValidateReturn method
            /// </summary>
            /// <param name="originalOrderId"></param>
            /// <param name="returnOrder"></param>
            /// <returns></returns>
            protected override NetSteps.Common.Base.BasicResponseItem<Order> ValidateReturnExtended(int originalOrderId, Order returnOrder)
            {
                return base.ValidateReturnExtended(originalOrderId, returnOrder);
            }
        }
    }
}