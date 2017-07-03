using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Extensions;
using NetSteps.Web.Mvc.Helpers;
using nsCore.Areas.Accounts.Models.Browse;
using System.Collections.Generic;
using System.Data;
using NetSteps.Data.Entities.Business.Logic;
using nsCore.Areas.Accounts.Controllers;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Web.Mvc.Business.Controllers;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using NetSteps.Diagnostics.Utilities;
using nsCore.Areas.Products.Models;
using System.Diagnostics.Contracts;
using System.Transactions;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Globalization;



namespace nsCore.Areas.CTE.Controllers
{
    public class PaymentApplicationController : BaseController
    {

        [FunctionFilter("Accounts", "~/Sites")]
        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult Index(string q, short? status, short? type, int? state, string city, string postalCode, int? country, string email, string coApplicant, int? sponsor, DateTime? startDate, DateTime? endDate, string siteUrl, string phone, int? title)
        {
            try
            {

                Dictionary<int, string> dcBank = new Dictionary<int, string>();
                dcBank = (new Dictionary<int, string>() { { 0, Translation.GetTerm("All", "All") } }).AddRange(PaymentTicktesBussinessLogic.BanksActiveDrop());

                ViewBag.dcBank = dcBank;

                Dictionary<int, string> dcStatusLog = new Dictionary<int, string>();
                dcStatusLog = (new Dictionary<int, string>() { { 0, Translation.GetTerm("All", "All") }, { 2, Translation.GetTerm("Aplicado", "Aplicado ") }, {1, Translation.GetTerm("PorAplicar", "Por Aplicar") } });//.AddRange(PaymentTicktesBussinessLogic.BanksActiveDrop());

                ViewBag.StatusLog = dcStatusLog;

                return View();
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }


        [OutputCache(CacheProfile = "PagedGridData")]
        public virtual ActionResult GetPaymentApplication(int page, int pageSize, int? BankId, int? FileSequenceLog,
            int? StatusLog, string DateBankLog, string DateProcess, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection)
        {
            try
            {


                LogErrorBankPaymentsRepository rep = new LogErrorBankPaymentsRepository();
                StringBuilder builder = new StringBuilder();
                var LogBankPayments = rep.SearchLogErrorBankPayments(new LogErrorBankPaymentsSearchParameter()
                {
                    BankId = BankId,
                    FileSequenceLog = FileSequenceLog.HasValue ? FileSequenceLog : 0,
                    StatusLog = StatusLog.HasValue ? StatusLog : 0,
                    DateBankLog = DateBankLog,//(DateBankLog.HasValue ? DateBankLog.ToShortDateString() : ""),
                    Date = DateProcess,//(Date.HasValue ? Date.ToShortDateString() : ""),
                    PageIndex = page,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    OrderByDirection = orderByDirection
                });



                if (LogBankPayments.Count > 0)
                {
                    string mnsApli = "";
                    string color = "";
                   

                    foreach (var item in LogBankPayments)
                    {


                        if (item.StatusLog == 1)
                        {
                            mnsApli = Translation.GetTerm("PorAplicar", "Por Aplicar");
                            color = "red";
                        }
                        else
                        {
                            mnsApli = Translation.GetTerm("Aplicado", "Aplicado");
                            color = "";
                        }

                        builder.Append("<tr >");

                        builder
                        .AppendCheckBoxCell(value: item.LogErrorBankPaymentID.ToString())
                        .AppendCell(item.LogErrorBankPaymentID.ToString())
                        .AppendCell(item.BankName.ToString())
                        .AppendCell(item.TicketNumber.ToString())
                    .AppendCell(item.LogAmount.ToString("N", CoreContext.CurrentCultureInfo))
                         //.AppendCell(item.LogAmount.ToString("N"))
                        .AppendCell(item.OrderNumber.ToString())

                         .AppendCell(FormatDatetimeByLenguage(item.DateBankLog,CoreContext.CurrentCultureInfo))
                        .AppendCell(FormatDatetimeByLenguage(item.Date,CoreContext.CurrentCultureInfo))
                   
                        .AppendCell(item.FileSequenceLog.ToString())

                       .AppendCell(item.logSequenceLog.ToString())
                       .AppendCell((item.StatusLog == 1 ? Translation.GetTerm("PorAplicar", "Por Aplicar") : Translation.GetTerm("Aplicado", "Aplicado")), style: "color :" + color)
                   
                  
                       .Append("</tr>");

                    }


                    return Json(new { totalPages = LogBankPayments.TotalPages, page = builder.ToString() });
                }
                else
                {
                    return Json(new { totalPages = 0, page = "<tr><td colspan=\"9\">" + Translation.GetTerm("ThereWereNoRecordsFoundThatMeetThatCriteriaPleaseTryAgain", "There were no records found that meet that criteria. Please try again.") + "</td></tr>" });
                }
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }


        public static string FormatDatetimeByLenguage(string fecha, CultureInfo culture)
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


        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult EditPaymentApplication(LogErrorBankPaymentsSearchParameter log)
        {
            try
            {

                //  var resp = NetSteps.Data.Entities.Business.CTE.Instance.SaveAccountCredit(items, 1);

                return Json(new { message = Translation.GetTerm("LogProcess", "Log Process") });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }



     

        public virtual ActionResult validarPayment(int id)//(List<int> items)
        {
            try
            {
                LogErrorBankPaymentsSearchParameter data = new LogErrorBankPaymentsSearchParameter();
                var reg = AccountPropertiesBusinessLogic.GetValueByID(8, id);
                data.StatusLog = reg.StatusLog;
                data.TicketNumber = reg.TicketNumber;

                if (data.StatusLog == 2)
                    return Json(new { result = false, message = Translation.GetTerm("BankConsolidate", "El archivo ya fue procesado") });

                return Json(new { result = true, TicketNumber = reg.TicketNumber, message = "" });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public virtual ActionResult ApplyManualPayment(int ticketNumber, int logErrorBankPaymentID)//(List<int> items)
        {
            try
            {
                int validate = 0;

                LogErrorBankPaymentsRepository rep = new LogErrorBankPaymentsRepository();

                validate = rep.ValidatePaymentApplication(new LogErrorBankPaymentsSearchParameter()
                {
                    TicketNumber = ticketNumber,
                    LogErrorBankPaymentID = logErrorBankPaymentID,
                    UserID = CoreContext.CurrentUser.UserID,
                });

                if (validate == 1)
                    return Json(new { result = false, message = Translation.GetTerm("TituloNoExiste", "Titulo No existe") });

                if (validate == 2)
                    return Json(new { result = false, message = Translation.GetTerm("TituloYaPago", "Titulo ya pago") });


                return Json(new { result = true, message = Translation.GetTerm("PaymentAplicationOK", "Abono aplicado") });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }




    }
}
