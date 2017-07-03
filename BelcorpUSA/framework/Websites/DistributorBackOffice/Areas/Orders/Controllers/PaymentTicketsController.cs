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

using System.Diagnostics.Contracts;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Services;
using System.Data;
using NetSteps.Common.Base;
using System.Data.SqlClient;
using DistributorBackOffice.Controllers;
using NetSteps.Data.Entities.Business.Logic;

using Microsoft.Reporting.WebForms;

using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using iTextSharp.text.pdf;
using CodeBarGeerator;
using System.Globalization;
using NetSteps.Web.Mvc.Controls.Models;
using NetSteps.Data.Entities.Dto;


namespace DistributorBackOffice.Areas.Orders.Controllers
{
    public class PaymentTicketsController : BaseController
    {
        //
        // GET: /Orders/PaymentTickets/


        private static CultureInfo _CultureUS = new CultureInfo("en-US");
       

        public ActionResult Index()
        {
            return View();
        }

        [OutputCache(CacheProfile = "PagedGridData")]
        public virtual ActionResult GetPaymentTickets(int page, int pageSize, string orderBy , NetSteps.Common.Constants.SortDirection orderByDirection ,
            int? id, int? isDeferred, int? forefit, string orderNumber, DateTime? expirationDate, DateTime? toDate
            )
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                int count = 0;

                var PaymentTickets = NetSteps.Data.Entities.Business.GeneralLedger.SearchPaymentTickets(new PaymentTicketsSearchParameters()
                {
                    TicketNumber = id,
                    IsDeferred = isDeferred,
                    Forefit = forefit,
                    OrderNumber=orderNumber,
                    ExpirationDate=expirationDate,
                    EndDate = toDate,
                    PageIndex = page,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    OrderByDirection = orderByDirection,
                    AccountID= CurrentAccount.AccountID
                });

             
                foreach (var paymentTicket in PaymentTickets.OrderByDescending(x=>x.TicketNumber).ToList())
                {
                    builder.Append("<tr>");
                    builder
                        //.AppendCell(paymentTicket.ID.ToString())
                        .AppendLinkCell("~/Orders/PaymentTickets/Renegotiation/" + paymentTicket.ID, paymentTicket.ID.ToString())
                        .AppendLinkCell("~/Orders/PaymentTickets/Renegotiation/" + paymentTicket.TicketNumber, paymentTicket.TicketNumber.ToString())
                        .AppendCell(paymentTicket.OrderNumber.ToString())
                        .AppendCell(paymentTicket.CompletedOn.HasValue ? paymentTicket.CompletedOn.ToShortDateString() : "")
                        .AppendCell(paymentTicket.Status.ToString())
                        //IPN
                        .AppendCell(paymentTicket.InitialAmount.ToString("N", CoreContext.CurrentCultureInfo))
                        .AppendCell(paymentTicket.FinancialAmount.ToString("N", CoreContext.CurrentCultureInfo))
                        .AppendCell(paymentTicket.TotalAmount.ToString("N", CoreContext.CurrentCultureInfo)) 
                        //IPN
                        //.AppendCell(paymentTicket.InitialAmount.ToString())
                        //.AppendCell(paymentTicket.FinancialAmount.ToString())
                        //.AppendCell(paymentTicket.TotalAmount.ToString())
                         .AppendCell(paymentTicket.ExpirationDate.HasValue ? paymentTicket.ExpirationDate.ToShortDateString() : "")
                        .AppendCell(paymentTicket.DateValidity.HasValue ? paymentTicket.DateValidity.ToShortDateString() : "")
                        .AppendCell(paymentTicket.EpirationStatus.ToString())
                        .AppendCell(paymentTicket.PaymentType.ToString())
                        .AppendCell((paymentTicket.PaymentType.ToString() == "Payment Ticket") ? "<a href='javascript:void(0)' onclick='cargarBillingInformation(" + paymentTicket.TicketNumber.ToString() + ")'> " + Translation.GetTerm("ViewTicket", "View Ticket") + "</a>" : Translation.GetTerm("ViewTicket", "View Ticket"))
                        .AppendCell((paymentTicket.PaymentType.ToString() == "Payment Ticket") ? "<a href='javascript:void(0)' onclick='loadBillingInformation(" + paymentTicket.TicketNumber.ToString() + ")'> " + Translation.GetTerm("SendEmailPDF", "Send Email") + "</a>" : Translation.GetTerm("SendEmailPDF", "Send Email"))


                        .Append("</tr>");
                    ++count;
                }
                return Json(new
                {
                    result = true,
                    totalPages = PaymentTickets.TotalPages,
                    page = PaymentTickets.TotalCount == 0 ? "<tr><td colspan=\"10\">There are no date.</td></tr>" : builder.ToString()
                });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public ActionResult Renegotiation(int id)
        {
            try
            {

                List<GeneralLedgerNegotiationData> rule = NetSteps.Data.Entities.Business.GeneralLedger.BrowseRulesNegotiation(id);

                rule.ForEach(x => {
                    
                    x.CurrentExpirationDateUTC = DateFormat(x.CurrentExpirationDateUTC);
                    x.DayValidate = DateFormat(x.DayValidate);
                });

                return View("Renegotiation",rule);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }

        }

        public string DateFormat(string date)
        {
            var delimiter = date.Split('/');
           
            if (string.IsNullOrEmpty(date))
                return date;
            if (delimiter.Length == 3)
                return new DateTime(delimiter[2].ToInt(), delimiter[1].ToInt(), delimiter[0].ToInt()).ToString("d", CoreContext.CurrentCultureInfo);
            else
                return date;
        }

     
        public virtual ActionResult GeListRenegotiationMethodsByOrder(string TicketNumber)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                int count = 0;
                var dat = new RenegotiationMethodDto()
                {
                    Site = "D",
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

                return Json(new { result = true, Items = List.Count == 0 ? "<tr><td colspan=\"7\">There are no rules</td></tr>" : builder.ToString() });
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
                           .Append("<td><input type=\"Text\"  class=\"classShared\"   id=\"txtValShared" + count + "\"   " + canDisabledValues + "   value=\"" + Convert.ToDecimal(reneg.ValShared).ToString("N", CoreContext.CurrentCultureInfo) + "\"    /> </td>")
                        //.Append("<td><input type=\"Text\"  class=\"classShared\"   id=\"txtValShared" + count + "\"   " + canDisabledValues + "   value=\"" + FormtDec(reneg.ValShared) + "\"    /> </td>")
                        .Append("<td><input type=\"Text\"  id=\"txtExpirationDate" + count + "\"     " + canDisabledDates + "  value=\"" + FormtDate(reneg.ExpirationDate) + "\"    /> </td>")
                        .Append("</tr>");

                    ++count;
                }

                CultureInfo culture = CultureInfo.CurrentCulture; 

                return Json(new
                {
                    result = true,
                    TotalAmount = Convert.ToDecimal(data.TotalAmount).ToString("N", CoreContext.CurrentCultureInfo),
                    Discount = Convert.ToDecimal(data.Discount).ToString("N", CoreContext.CurrentCultureInfo),
                    TotalPay = Convert.ToDecimal(data.TotalPay).ToString("N", CoreContext.CurrentCultureInfo),
                    //IPN
                    //TotalAmount = FormtDec(data.TotalAmount),
                    //Discount = FormtDec(data.Discount),
                    //TotalPay = FormtDec(data.TotalPay),
                    FirstDateExpirated = FormtDate(data.FirstDateExpirated),
                    SharesInterval = data.SharesInterval,
                    LastDateExpirated = FormtDate(data.LastDateExpirated),
                    DayValidate = data.DayValidate,
                    NumShared = Convert.ToDecimal(data.ValShared).ToString("N", CoreContext.CurrentCultureInfo),
                    //NumShared = FormtDec(data.ValShared),
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
            return val.Replace(",", ".");
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
                    oenOrderPayment.CurrentExpirationDateUTC = Convert.ToDateTime(item.ExpirationDate).ToString(CoreContext.CurrentCultureInfo);
                    //oenOrderPayment.CurrentExpirationDateUTC = Convert.ToDateTime(item.ExpirationDate).ToString("MM/dd/yyyy");
                    oenOrderPayment.InitialAmount = item.ValShared;
                    oenOrderPayment.TotalAmount = item.ValShared;
                    oenOrderPayment.ModifiedByUserID = CoreContext.CurrentUser == null ? CurrentAccount.UserID.ToInt() : CoreContext.CurrentUser.UserID;// CoreContext.CurrentUser.UserID;
                    //oenOrderPayment.DateValidity = Convert.ToDateTime(item.ExpirationDate).AddDays(DayValidate).ToString("MM/dd/yyyy");
                    oenOrderPayment.DateValidity = Convert.ToDateTime(item.ExpirationDate).AddDays(DayValidate).ToString(CoreContext.CurrentCultureInfo);
                    oenOrderPayment.RenegotiationConfigurationID = RenegotiationConfigurationID;


                    List<OrderPaymentNegotiationData> list = new List<OrderPaymentNegotiationData>();
                    list.Add(oenOrderPayment);
                    RenegotiationMethods ObjOrderPayment = new RenegotiationMethods();
                    if (KeyDecimals == "ES")
                    {
                        var culture = CultureInfoCache.GetCultureInfo("En");
                        //amount = Convert.ToDecimal(DescuentoGlobal, culture);
                        //amount = Convert.ToDecimal(DescuentoGlobal, _CultureUS);
                        amount = Convert.ToDecimal(DescuentoGlobal, CoreContext.CurrentCultureInfo);
                    }
                    ObjOrderPayment.RegisterRenegotiationOrderPayment("D", list, ultRegistro, NumberCuotas, amount);
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
                return Json(new { result = true, MultaCalulada = datos[0].MultaCalulada, InteresCalculado = datos[0].InteresCalculado });

            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        #region Exportacion reportes Pdf
        /// <summary>
        /// Exportacion en pdf de las filas seleccionados
        /// </summary>
        /// <param name="lstOrdenesSelecionado">OrderPaymentID seleccionados</param>
        /// <returns>retorna los nombres de los archivos generados y el viewTicket</returns>
        [HttpPost]
        public virtual ActionResult VerificarTipoCuenta(int OrderPaymentID)
        {
            int total;
            StringBuilder builder = new StringBuilder();
            try
            {
                IList<string> NombreArchivosGenerados = new List<string>();
                DataTable dtOrdenesSelecionados = new DataTable();
                System.Data.DataColumn dc;
                dc = new System.Data.DataColumn("OrderPaymentID");
                dtOrdenesSelecionados.Columns.Add(dc);

                dtOrdenesSelecionados.Rows.Add(new Object[] { OrderPaymentID.ToString() });


                List<PaymentInfoBancoOrden> lstInformacionFacturacion = PaymetTycketsReportBusinessLogic.GetInformacionBanco(OrderPaymentID);
                PaymentInfoBancoOrden obj = lstInformacionFacturacion.First();
                if (obj.IsCreditCard)
                {
                    if (dtOrdenesSelecionados.Rows.Count > 0)
                        CrearTablaViewTicket(dtOrdenesSelecionados, out total, out builder);
                }

                return Json(
                    new
                    {
                        IsCreditCard = obj.IsCreditCard,
                        BankName = obj.BankName,
                        BankCode = obj.BankCode,
                        html = builder.ToString()
                    });

            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
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
                    case 1://"Banco Do Brasil":
                        ResponseFile = CrearTicketBB(OrderPaymentID);
                        break;
                    case 104://"Caixa":
                        ResponseFile = CrearTicketCaixa(OrderPaymentID);
                        break;
                    case 341:// "Itaú":
                        ResponseFile = CrearTicketItau(OrderPaymentID);
                        break;
                    default:
                        break;
                }
                byte[] Libro = ExtractPages(ResponseFile);

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
        private byte[] CrearTicketBradesco(int TicketNumber)
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

                report.DataSources.Add(rdsInfoBank);
                report.DataSources.Add(rdsdtOrder);


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
        /// crear el view ticket con las ordenes seleccionados
        /// </summary>
        /// <param name="dtOrdenesSelecionados"></param>
        /// <param name="total">cantidad de filas devueltas </param>
        /// <param name="builder">el Html generado</param>
        private static void CrearTablaViewTicket(DataTable dtOrdenesSelecionados, out int total, out StringBuilder builder)
        {
            // verificar a que banco pertenece
            var lstResultado = PaymetTycketsReportBusinessLogic.ObtenerInformacionFacturacion(dtOrdenesSelecionados);
            total = 0;
            total = lstResultado.Count();

            builder = new StringBuilder();
            for (int index = 0; index < total; index++)
            {
                builder.Append("<tr>")
                    .AppendCell(Translation.GetTerm("AccountNumber", "Account Number"))
                    .AppendCell(lstResultado[index].AccountID.ToString())
                 .Append("</tr>")
                 .Append("<tr>")
                    .AppendCell(Translation.GetTerm("ExpirationDate", "Expiration Date"))
                    .AppendCell(lstResultado[index].CurrentExpirationDateUTC.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo))
                 .Append("</tr>")
                 .Append("<tr>")
                    .AppendCell(Translation.GetTerm("AccountName", "Expiration Date"))
                    .AppendCell(lstResultado[index].AccountName)
                 .Append("</tr>")
                 .Append("<tr>")
                    .AppendCell(Translation.GetTerm("Address", "Address"))
                    .AppendCell
                    (
                         lstResultado[index].Rua + " " + lstResultado[index].Numero + "</br>"
                        + lstResultado[index].Barrio + " " + lstResultado[index].Ciudad + " " + lstResultado[index].Estado
                    )
                 .Append("</tr>")
                  .Append("<tr>")
                    .AppendCell(Translation.GetTerm("Country", "Country"))
                    .AppendCell(lstResultado[index].Ciudad)
                 .Append("</tr>")
                 .Append("<tr>")
                    .AppendCell(Translation.GetTerm("PaymentStatus", "Payment Status"))
                    .AppendCell(lstResultado[index].PaymentStatus)
                 .Append("</tr>")
                   .Append("<tr>")
                    .AppendCell(Translation.GetTerm("TransactionId", "Transaction ID"))
                    .AppendCell(lstResultado[index].TransationId.ToString())
                 .Append("</tr>")

                .Append("<tr>")
                .Append("<td colspan='2'> <hr></td>")
                .Append("</tr>");

            }
            if (total == 0)
            {
                builder.Append("<tr><td colspan=\"2\">" + Translation.GetTerm("ThereWereNoRecordsFoundThatMeetThatCriteriaPleaseTryAgain", "There were no records found that meet that criteria. Please try again.") + "</td></tr>");
            }

        }
        #endregion

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
        /// <summary>
        /// crear el codigo de barra 
        /// </summary>
        /// <param name="text">texto segun especificacion</param>
        /// <returns>Array de Byte que se comvierte en imagen</returns>

        //crear el codigo de barra con el texto generado
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
        #endregion

        #region Enviar Correo con reportes Pdf

        public virtual ActionResult ExportarBoletaRepositorio(int OrderPaymentID)
        {
            try
            {
                List<PaymentInfoBancoOrden> lstInformacionFacturacion = PaymetTycketsReportBusinessLogic.GetInformacionBanco(OrderPaymentID);
                PaymentInfoBancoOrden obj = lstInformacionFacturacion.First();
                int BankCode = 0;
                BankCode = obj.BankCode;
                Byte[] ResponseFile = null;

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
                //var path = Server.MapPath("~/Reports/FilesTemp/" + nameFile);
                var ruta = NetSteps.Common.Configuration.ConfigurationManager.AppSettings["FileUploadWebPath"];

                var path = ruta + nameFile; 
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                System.IO.File.WriteAllBytes(path, output);
                File(path, "application/pdf", Path.GetFileName(path));

                
                var v = AccountPropertiesBusinessLogic.GetEmailTemplate(OrderPaymentID, nameFile);
                bool sentSuccessfully = v.Respuesta == 1 ? true : false;

                if (sentSuccessfully)
                    return Json(new { result = true, message = Translation.GetTerm("EmailSentSuccessfully", "Email Sent Successfully!") });
                else
                    return Json(new { result = false, message = Translation.GetTerm("EmailNotSentSuccessfully", "Not process the Email") });

            }
            catch (Exception ex)
            {
                //throw ex;
                string msg = Translation.GetTerm("EmailNotSentForData", "Not process e-mail due to the format of values.");
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = msg }); //exception.PublicMessage });
            }
        }

        

        [ActionName("Descargar-Archivo")]
        public ActionResult Download(string id)
        {
            string fullName = System.IO.Path.Combine(GetBaseDir(), id);

            byte[] fileBytes = GetFile(fullName);
            return File(
                fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fullName);
        }

        string GetBaseDir()
        {
            return HttpContext.Server.MapPath("~/Reports/FilesTemp/");
        }

        byte[] GetFile(string s)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(s);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(s);
            return data;
        }

        #endregion
    }
}
