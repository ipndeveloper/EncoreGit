using System;
using System.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using NetSteps.Common.Comparer;
using NetSteps.Common.Configuration;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Helpers;
using NetSteps.Diagnostics.Utilities;
using nsCore.Areas.Products.Models;
using System.Diagnostics.Contracts;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Services;
using nsCore.Controllers;
using System.Data;
using NetSteps.Common.Base;
using System.Data.SqlClient;
using NetSteps.Data.Entities.Business.Logic;
using NetSteps.Data.Entities.Dto;
using System.Globalization;
using System.Threading;


namespace nsCore.Areas.CTE.Controllers
{
    public class PaymentTicketsController : BaseController
    {
        //
        // GET: /GeneralLedger/TicketPayment/

        public ActionResult BrowseTickets()
        {
            return View();
        }

        /// <summary>
        /// Developed By KLC - CSTI
        /// </summary>
        /// <returns>Retorna la Vista Ticket Details</returns>
        public ActionResult TicketDetails(int id)
        {
            try
            {
                ViewBag.Status = ValidateStatusPayment(id);
                List<GLPaymeTycketsSearchData> Result = NetSteps.Data.Entities.Business.GeneralLedger.BrowseTicketDetails(id);
                return View(Result);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
           
        }


        public  static string FormatDatetimeByLenguage(string fecha, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(fecha))
                return "N/A";
            else
            {

            }
            {
                DateTime? fec = null;
                DateTime Temp;

                var split = fecha.Split('/');
                if (split.Length >= 3)
                {

                    if (culture.Name == "en-US")

                        return string.Format("{0}/{1}/{2}", split[1], split[0], split[2]);

                    else
                        return string.Format("{0}/{1}/{2}", split[0], split[1], split[2]);

                }

                else
                    return fecha;

            }
        }
        /// <summary>
        /// Developed By KLC - CSTI
        /// </summary>
        /// <returns>Retorna la Vista Pay Ticket</returns>
        public ActionResult PayTicket(int id)
        {
            try
            {
       
                List<GLCalculateUpdateBalanceSearchData> Result = NetSteps.Data.Entities.Business.GeneralLedger.GetCalculateUpdateBalance(id);

                ViewData["getEntitys"] = NetSteps.Data.Entities.Business.GeneralLedger.GetEntity();
                return View(Result);
               
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }
        public virtual ActionResult GetPaymentTypes(int BanckID)
        {
            try
            {
                return Json(new
                {
                    result = true,
                    PaymentTypes = (NetSteps.Data.Entities.Business.GeneralLedger.GetPaymentType(BanckID)).Select(pp => new 
                                {   ID=pp.id,
                                    Name = pp.Name }),

                });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        /// <summary>
        /// Developed By KLC - CSTI
        /// </summary>
        /// <returns>Retorna la Vista Extent Expiration</returns>
        public ActionResult ExtendExpiration(int id)
        {
            try
            {
                ViewBag.Status = ValidateStatusPayment(id);
                List<GLCalculateUpdateBalanceSearchData> Result = NetSteps.Data.Entities.Business.GeneralLedger.GetCalculateUpdateBalance(id);
                return View(Result);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        /// <summary>
        /// Developed By KLC - CSTI
        /// </summary>
        /// <returns>Retorna la Vista Calcule UpdateBalance</returns>
        public ActionResult CalculeUpdateBalance(int id)
        {
            try
            {
                ViewBag.Status = ValidateStatusPayment(id);
                List<GLCalculateUpdateBalanceSearchData> Result = NetSteps.Data.Entities.Business.GeneralLedger.GetCalculateUpdateBalance(id);
                return View(Result);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
            
        }
        private int ValidateStatusPayment(int id)
        {
            var orderPayment = AccountPropertiesBusinessLogic.GetValueByID(3, id);
            if (orderPayment.OrderPaymentStatusID.Equals(1) || orderPayment.OrderPaymentStatusID.Equals(4))
                return 1;
            else
                return 0;
        }
        public ActionResult AlterDueDate(int TicketNumber, string NewDate)
        {
            try
            {
                var orderPayment = AccountPropertiesBusinessLogic.GetValueByID(3, TicketNumber);
                
                if (orderPayment.OrderPaymentStatusID.Equals(1) || orderPayment.OrderPaymentStatusID.Equals(4))
                {
                    NetSteps.Data.Entities.Business.GeneralLedger.spUpdateAlterDueDate(TicketNumber, NewDate);
                    InsertOrderPaymentLOG(TicketNumber, 9);
                    return Json(new { result = true });
                    
                }
                else
                {
                    return JsonError(Translation.GetTerm("Payment_ValidExtendExpiration", "For the payment status is not possible process"));
                }

                
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        /// <summary>
        /// Developed By KLC - CSTI
        /// </summary>
        /// <returns>Retorna la Vista Control Log</returns>
        public ActionResult ControlLog(int id)
        {
            try
            {
                ViewBag.Status = ValidateStatusPayment(id);
                List<GLPaymeTycketsSearchData> Result = NetSteps.Data.Entities.Business.GeneralLedger.BrowseTicketDetails(id);
                return View(Result);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }
        /// <summary>
        /// Developed By KLC - CSTI
        /// </summary>
        /// <returns>Retorna los datos de la tabla Control Log</returns>
        [OutputCache(CacheProfile = "PagedGridData")]
        public virtual ActionResult GetControlLog(int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection,
            int id, DateTime? startDate, int? accountNumberId)
        {
            try
            {

                StringBuilder builder = new StringBuilder();
                int count = 0;
                var orderPayment = AccountPropertiesBusinessLogic.GetValueByID(3, id);

                var ControlLogs = NetSteps.Data.Entities.Business.GeneralLedger.GetControlLog(new GLControlLogParameters()
                {
                    OrderPaymentID = orderPayment.OrderPaymentID,
                    DateModifiedUTC = startDate,
                    ModifiedByUserID=accountNumberId,
                    PageIndex = page,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    OrderByDirection = orderByDirection
                });

                foreach (var controlLog in ControlLogs)
                {
                    builder.Append("<tr>");
                    builder
                        .AppendCell(controlLog.Reason.ToString())
                        .AppendCell(controlLog.InitialAmount.ToString("N" , CoreContext.CurrentCultureInfo))
                        .AppendCell(controlLog.InterestAmount.ToString("N" , CoreContext.CurrentCultureInfo))
                        .AppendCell(controlLog.FineAmount.ToString("N" , CoreContext.CurrentCultureInfo))
                        .AppendCell(controlLog.TotalAmount.ToString("N" , CoreContext.CurrentCultureInfo))
                        .AppendCell(controlLog.DateModifiedUTC.ToString())
                        .AppendCell(controlLog.ModifiedByUserName.ToString())
                        .AppendCell(controlLog.TicketNumber.ToString())
                        .Append("</tr>");
                    ++count;
                }
                return Json(new { result = true, totalPages = ControlLogs.TotalPages,
                    page = ControlLogs.TotalCount == 0 ? "<tr><td colspan=\"6\">There are no date.</td></tr>" : builder.ToString() });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        //@01   AINI
        /// <summary>
        /// Developed By AGA - Background
        /// </summary>
        /// <returns>Retorna la Vista Renegotiation</returns>
        public ActionResult Renegotiation(int id)
        {
            try
            {
                ViewBag.Status = ValidateStatusPayment(id);
                List<GeneralLedgerNegotiationData> rule = NetSteps.Data.Entities.Business.GeneralLedger.BrowseRulesNegotiation(id);
             
                return View(rule);               
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }

        }

        public virtual ActionResult GeListRenegotiationMethodsByOrder(string TicketNumber)        
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                int count = 0;
                var dat = new RenegotiationMethodDto()
                {
                    Site = "G",
                    OrderPaymentID = int.Parse(TicketNumber)
                };
                
                var List = RenegotiationMethods.ListRenegotiationMethodsByOrder(dat);
           
                foreach (var reneg in List)
                {

                   
                    builder.Append("<tr>");
                    builder
                        .Append("<td><input type=\"radio\"   name=\"rbMethod\"   onclick='ViewShareds(" + TicketNumber + "," + reneg.RenegotiationConfigurationID + "," + reneg.FineAndInterestRulesPerNegotiationLevelID + ")' /> </td>")                                                
                        .AppendCell(reneg.Plano)
                        .AppendCell(reneg.Cuotas.ToString())
                        .AppendCell(reneg.Juros_Dia.ToString())
                        .AppendCell(reneg.Taxa)
                        .AppendCell(reneg.DiscountDesc)
                        
                       .Append("</tr>");

                    ++count;
                }

                return Json(new { result = true,  Items = List.Count == 0 ? "<tr><td colspan=\"7\">There are no rules</td></tr>" : builder.ToString() });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }


        public virtual ActionResult GeListRenegotiationShares(string TicketNumber, string RenegotiationConfigurationID, string NegotiationLevelID)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                int count = 0;
                var dat = new RenegotiationMethodDto()
                {                   
                    OrderPaymentID = int.Parse(TicketNumber),
                    RenegotiationConfigurationID = int.Parse(RenegotiationConfigurationID),
                    FineAndInterestRulesPerNegotiationLevelID = int.Parse(NegotiationLevelID)
                };

                var data = RenegotiationMethods.ListRenegotiationShares(dat);
                var ListShared = data.ListShared;

                string canDisabledValues = data.ModifiesValues == "S" ? "" : "disabled=\"true\"";
                string canDisabledDates = data.ModifiesDates == "S" ? "" : "disabled=\"true\"";
               
                foreach (var reneg in ListShared)
                {

                    builder.Append("<tr>");
                    builder
                        .AppendCell(reneg.Parcela)
                        .Append("<td><input type=\"Text\"  class=\"classShared\"   id=\"txtValShared" + count + "\"   " + canDisabledValues + "   value=\"" + FormtDec(reneg.ValShared) + "\"    /> </td>")
                         .Append("<td><input type=\"Text\"  id=\"txtExpirationDate" + count + "\"     " + canDisabledDates + "  value=\"" + FormtDate(reneg.ExpirationDate) + "\"    /> </td>")
                       .Append("</tr>");

                    ++count;
                }

                return Json(new
                {
                    result = true,
                    TotalAmount = FormtDec(data.TotalAmount),
                    Discount =FormtDec(data.Discount),
                    TotalPay = FormtDec(data.TotalPay),
                    FirstDateExpirated = FormtDate(data.FirstDateExpirated),
                    SharesInterval = data.SharesInterval,
                    LastDateExpirated = FormtDate(data.LastDateExpirated),
                    DayValidate = data.DayValidate,
                    NumShared = FormtDec(data.ValShared),
                    Items = ListShared.Count == 0 ? "<tr><td colspan=\"7\">There are no rules</td></tr>" : builder.ToString()
                });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        private string FormtDec(string val)
        {
            return val.Replace(",",".");
        }

        private string FormtDate(string val)
        {
            return val.Replace("0:00:00", "");
        }
        public virtual ActionResult RegisterOrderPayments(
       int OrderPaymentID, int DayValidate, int RenegotiationConfigurationID,
          List<RenegotiationSharedDetDto> ListSharedDet, int NumberCuotas, string DescuentoGlobal)
        {

            try
            {
               
                var KeyDecimals = NetSteps.Common.Configuration.ConfigurationManager.AppSettings["CultureDecimal"];
                int ultRegistro = ListSharedDet.Count;
                decimal amount = 0;
                foreach (var item in ListSharedDet)
                {
                    ultRegistro--;


                    OrderPaymentNegotiationData oenOrderPayment = new OrderPaymentNegotiationData();
                 
                    oenOrderPayment.TicketNumber = OrderPaymentID;
                    oenOrderPayment.CurrentExpirationDateUTC = Convert.ToDateTime(item.ExpirationDate).ToString("MM/dd/yyyy");
                    oenOrderPayment.InitialAmount = item.ValShared;
                    oenOrderPayment.TotalAmount = item.ValShared;
                    oenOrderPayment.ModifiedByUserID = CoreContext.CurrentUser.UserID;
                    oenOrderPayment.DateValidity = Convert.ToDateTime(item.ExpirationDate).AddDays(DayValidate).ToString("MM/dd/yyyy");
                    oenOrderPayment.RenegotiationConfigurationID = RenegotiationConfigurationID;

                     
                    List<OrderPaymentNegotiationData> list = new List<OrderPaymentNegotiationData>();
                    list.Add(oenOrderPayment);
                    RenegotiationMethods ObjOrderPayment = new RenegotiationMethods();

                   
                    if (KeyDecimals == "ES")
                    {
                        var culture = CultureInfoCache.GetCultureInfo("En");
                        amount = Convert.ToDecimal(DescuentoGlobal, culture);                       
                    }

                    ObjOrderPayment.RegisterRenegotiationOrderPayment("G", list, ultRegistro, NumberCuotas, amount);
                }

                return Json(new { result = true });
            }
            catch (Exception ex)
            {

                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

       



        public virtual ActionResult CalculateNewAmount(int id)
        {
            try
            {
                var datos = UpdateBalance.UpdateBalances(id);
                datos[0].NumDocument = id;
               var  reg= UpdateBalance.UpdateBalancesPayments(datos.First());
        
                return Json(new { result = true,
                    //MultaCalulada = reg[0].MultaCalulada, 
                    //InteresCalculado = reg[0].InteresCalculado ,
                    FinancialAmount = reg[0].FinancialAmount,
                    TotalAmount = reg[0].TotalAmount
                });

            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        //public virtual ActionResult RegisterOrderPayments(OrderPaymentNegotiationData oenOrderPayment)
        //{

        //    try
        //    {
        //        OrderPayment ObjOrderPayment = new OrderPayment();
        //        ObjOrderPayment.Register(oenOrderPayment);

        //        return Json(new { result = true });
        //    }
        //    catch (Exception ex)
        //    {

        //        var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
        //        return Json(new { result = false, message = exception.PublicMessage });
        //    }
        //}

        //@01 AFIN

        /// <summary>
        /// Developed KLC - CSTI
        /// BR-CC-013
        /// </summary>
        /// <param name="TicketNumber"></param>
        /// <returns></returns>
        public virtual ActionResult GetApplyManualPayment(Int32 TicketNumber, Int32 BankID, string ProcessOnDateUTC)
        {
            try
            {

                var orderPayment = AccountPropertiesBusinessLogic.GetValueByID(3, TicketNumber);

                if (orderPayment.OrderPaymentStatusID.Equals(1) || orderPayment.OrderPaymentStatusID.Equals(4))
                {
                    int UserID = CoreContext.CurrentUser.UserID;
                    CTERepository.ApplyManualPayment(TicketNumber, BankID, UserID, Convert.ToDateTime(ProcessOnDateUTC),"M",0);

                    InsertOrderPaymentLOG(TicketNumber, 10);


                    return Json(new { result = true, message = "Proceso ApplyManualPayment Satisfactorio" });
                }
                else
                {
                    return JsonError(Translation.GetTerm("Payment_ValidExtendExpiration", "For the payment status is not possible process"));
                }

            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        private void InsertOrderPaymentLOG(int TicketNumber,int ReasonID)
        {
            var vlOrderPaymentRepository = new OrderPaymentsRepository();
            //var orderPayment = vlOrderPaymentRepository.BrowseOrderPaymentsByTicketNumber(TicketNumber).FirstOrDefault();
            var orderPayment = AccountPropertiesBusinessLogic.GetValueByID(3, TicketNumber);
            OrderPaymentsLog vlOrderPaymentsLog = new OrderPaymentsLog()
            {
                OrderPaymentID = orderPayment.OrderPaymentID,
                ReasonID = ReasonID,
                InitialAmount = Convert.ToDecimal(orderPayment.InitialAmount == null ? 0 : orderPayment.InitialAmount),
                InterestAmount = 0,
                FineAmount = Convert.ToDecimal(orderPayment.FinancialAmount == null ? 0 : orderPayment.FinancialAmount),
                DisccountedAmount = Convert.ToDecimal(orderPayment.DiscountedAmount == null ? 0 : orderPayment.DiscountedAmount),
                TotalAmount = Convert.ToDecimal(orderPayment.TotalAmount == null ? 0 : orderPayment.TotalAmount),
                ExpirationDateUTC = Convert.ToDateTime(orderPayment.CurrentExpirationDateUTC),
                ModifiedByUserID = ApplicationContext.Instance.CurrentUserID
            };
            OrderPaymentsLogExtensions.CreateLog(vlOrderPaymentsLog);
        }

        public virtual ActionResult ApplicateDescto(string DesctoAplicate, int TicketNumber)
        {
            try
            {

                CTERepository.AplicateDescto(DesctoAplicate, TicketNumber);
                InsertOrderPaymentLOG(TicketNumber, 40);
                return Json(new { result = true });

            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

    }
}
