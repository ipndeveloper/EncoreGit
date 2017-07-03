using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NetSteps.Common.Base;
using NetSteps.Common.Configuration;
using NetSteps.Common.Extensions;
using System.Text;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Helpers;
using nsCore.Controllers;
using NetSteps.Data.Entities.Business.Logic;
using NetSteps.Data.Entities.Repositories;
using System.Data;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using Microsoft.Reporting.WebForms;
using iTextSharp.text.pdf;
using CodeBarGeerator;
using System.Globalization;
using System.IO;
using NetSteps.Web.Mvc.Controls.Models;
using System.Net.Mail;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Xml;
using System.Xml.Serialization;
using System.ComponentModel;
using OfficeOpenXml;

//using NetSteps.Data.Entities.Mail;
//using NetSteps.Data.Entities.Mail;



namespace nsCore.Areas.CTE.Controllers
{
    public class PaymentTicketsReportController : BaseController
    {
        #region Payment Tickets
        //Developer by salcedo vila G.
        public ActionResult BrowseTickets() 
        {
            Dictionary<int, string> dcOrderPaymentStatuses = new Dictionary<int, string>();
            dcOrderPaymentStatuses = PaymentTicktesBussinessLogic.OrderPaymentStatuDrop();
            dcOrderPaymentStatuses = (new Dictionary<int, string>() { { 0, Translation.GetTerm("All", "All") } }).AddRange(dcOrderPaymentStatuses);

            Dictionary<int, string> dcCountry = new Dictionary<int, string>();
            dcCountry = PaymentTicktesBussinessLogic.CountriesActiveDrop();


            Dictionary<int, string> dcBank = new Dictionary<int, string>();
            //dcBank.Add(0, "All");
            //dcBank = PaymentTicktesBussinessLogic.BanksActiveDrop();
            dcBank = (new Dictionary<int, string>() { { 0, Translation.GetTerm("All", "All") } }).AddRange(PaymentTicktesBussinessLogic.BanksActiveDrop());

            Dictionary<int, string> dcNegotiationLevels = new Dictionary<int, string>();
            //dcNegotiationLevels = PaymentTicktesBussinessLogic.NegotiationLevelsActiveDrop();
            dcNegotiationLevels = (new Dictionary<int, string>() { { 0, Translation.GetTerm("All", "All") } }).AddRange(PaymentTicktesBussinessLogic.NegotiationLevelsActiveDrop());

            Dictionary<int, string> dcExpirationStatuses = new Dictionary<int, string>();
            //dcExpirationStatuses = PaymentTicktesBussinessLogic.ExpirationStatusesDrop();
            dcExpirationStatuses = (new Dictionary<int, string>() { { 0, Translation.GetTerm("All", "All") } }).AddRange(PaymentTicktesBussinessLogic.ExpirationStatusesDrop());

            Dictionary<int, string> dcForefit = new Dictionary<int, string>();
            dcForefit = (new Dictionary<int, string>() { { -1, Translation.GetTerm("All", "All") } ,
                                                         { 0, Translation.GetTerm("Send", "Send") } ,
                                                         { 1, Translation.GetTerm("Lose", "Lose") } });

            ViewBag.dcOrderPaymentStatuses = dcOrderPaymentStatuses;
            ViewBag.dcCountry = dcCountry;
            ViewBag.dcBank = dcBank;
            ViewBag.dcNegotiationLevels = dcNegotiationLevels;
            ViewBag.dcExpirationStatuses = dcExpirationStatuses;
            ViewBag.dcForefit = dcForefit;
            Session["GridTicket"] = null;
            return View("~/Areas/CTE/Views/PaymentTicketsReport/BrowseTickets.aspx");
        }

        public ActionResult Index(int? id)
        {
            Dictionary<int, string> dcOrderPaymentStatuses = new Dictionary<int, string>();
            dcOrderPaymentStatuses = PaymentTicktesBussinessLogic.OrderPaymentStatuDrop();

            Dictionary<int, string> dcCountry = new Dictionary<int, string>();
            dcCountry = PaymentTicktesBussinessLogic.CountriesActiveDrop();


            Dictionary<int, string> dcBank = new Dictionary<int, string>();
            dcBank = PaymentTicktesBussinessLogic.BanksActiveDrop();

            Dictionary<int, string> dcNegotiationLevels = new Dictionary<int, string>();
            dcNegotiationLevels = PaymentTicktesBussinessLogic.NegotiationLevelsActiveDrop();

            Dictionary<int, string> dcExpirationStatuses = new Dictionary<int, string>();
            dcExpirationStatuses = PaymentTicktesBussinessLogic.ExpirationStatusesDrop();

            Dictionary<int, string> dcForefit = new Dictionary<int, string>();
            dcForefit = (new Dictionary<int, string>() { { -1, Translation.GetTerm("All", "All") } ,
                                                         { 0, Translation.GetTerm("Send", "Send") } ,
                                                         { 1, Translation.GetTerm("Lose", "Lose") } });

            ViewBag.dcOrderPaymentStatuses = dcOrderPaymentStatuses;
            ViewBag.dcCountry = (new Dictionary<int, string>() { { 0, Translation.GetTerm("SelectCountry", "Select a Country...") } }).AddRange(dcCountry);
            ViewBag.dcBank = (new Dictionary<int, string>() { { 0, Translation.GetTerm("SelectBank", "Select a Bank...") } }).AddRange(dcBank);
            ViewBag.dcNegotiationLevels = (new Dictionary<int, string>() { { 0, Translation.GetTerm("SelectLevel", "Select a Level...") } }).AddRange(dcNegotiationLevels);
            ViewBag.dcExpirationStatuses = (new Dictionary<int, string>() { { 0, Translation.GetTerm("SelectSituationExpiration", "Select a Situation Expiration...") } }).AddRange(dcExpirationStatuses);
            ViewBag.IsFirstLoad = true;
            ViewBag.dcForefit = dcForefit;
            Session["GridTicket"] = null;
            return View("~/Areas/CTE/Views/PaymentTicketsReport/Index.aspx");
        }

        private DateTime?       ConvertDateMMDDYYY(string indate)
        {
           
            string[] sep = indate.Split('/');
            if (sep != null && sep.Count()==3) {
              return    new DateTime(sep[2].ToInt(), sep[1].ToInt(), sep[0].ToInt());
               
            }

            return null;
    
        }
        //Developer by salcedo vila G.
        [OutputCache(CacheProfile = "PagedGridData")]
        public virtual ActionResult GetPaymentTicketsReport(
                    int page,
                    int pageSize,

                    int? AccountID,
                    string Name,
                    int? LevelId,

                    string _startDateExpirationDate,
                    string _endDateExpirationDate,

                    string _startDateLiquidationDate,
                    string _endDateLiquidationDate,

                    int? OrderId,
                    int? TicketNumber,
                    string FiscalNote,
                    int? ExpirationSituationId,
                    int? OrderPaymentStatusId,

                   DateTime? startOrderDate,
                    DateTime? endOrderDate,

                    int? CountryId,
                    int? BankId,
                    int? Forefit,

                    string orderBy,
                  NetSteps.Common.Constants.SortDirection orderByDirection
                )
        {
            try
            {
                if (Session["GridTicket"] != null)
                {
                    var param = new PaymentTicketsSearchParameters()
                    {
                        NegotiationLevelID = (LevelId == 0 ? null : LevelId),
                        ExpirationStatusID = (ExpirationSituationId == 0 ? null : ExpirationSituationId),
                        Country = (CountryId == 0 ? null : CountryId),
                        BankID = (BankId == 0 ? null : BankId),
                        Forefit = (Forefit == -1 ? null : Forefit),
                        _AccountID = (AccountID == 0 ? null : AccountID),
                        ExpiriedDateFrom = ConvertDateMMDDYYY(_startDateExpirationDate),
                        ExpiriedDateTo = ConvertDateMMDDYYY(_endDateExpirationDate),
                        LiquidationDateFrom = ConvertDateMMDDYYY(_startDateLiquidationDate),
                        LiquidationDateTo = ConvertDateMMDDYYY(_endDateLiquidationDate),
                        OrderId = (OrderId == 0 ? null : OrderId),
                        OrderPaymentStatusId = OrderPaymentStatusId == 0 ? null : OrderPaymentStatusId,

                        FiscalNote = (FiscalNote == "" ? null : FiscalNote),
                        TicketNumber = (TicketNumber == 0 ? null : TicketNumber),
                        PageIndex = page,
                        PageSize = pageSize,
                        OrderBy = orderBy,
                        OrderByDirection = orderByDirection

                    };

                    StringBuilder builder = new StringBuilder();
                    var lstResultado = PaymetTycketsReportBusinessLogic.SearchTickets(param);


                    int total = 0;

                    total = lstResultado.Count();


                    if (total > 0)
                    {

                        for (int index = 0; index < total; index++)
                        {
                            string desforefit = "";
                            if (lstResultado[index].Forefit == 1)
                                desforefit = Translation.GetTerm("Lose", "Lose");
                            else
                                desforefit = Translation.GetTerm("Send", "Send");


                            builder.Append("<tr>")
                                .AppendCell(lstResultado[index].AccountNumber)
                               .AppendCell(lstResultado[index].AccountName)
                               .AppendLinkCell("~/CTE/PaymentTickets/TicketDetails/" + lstResultado[index].TicketNumber, lstResultado[index].TicketNumber.ToString())
                               .AppendCell(Convert.ToString(lstResultado[index].OrderID))
                               .AppendCell(Convert.ToString(lstResultado[index].StatusPaymentName))
                                .AppendCell(lstResultado[index].DescPayConf)
                                 .AppendCell(lstResultado[index].NegotiationLevelName)
                                .AppendCell(lstResultado[index].NameExpiration, style: "color :" + (lstResultado[index].ExpirationStatusID == 1 ? "red" : ""))
                                .AppendCell(Convert.ToString(lstResultado[index].ExpirationDays))
                                .AppendCell(desforefit)
                               .AppendCell(FormatDatetimeByLenguage(lstResultado[index].Period,CoreContext.CurrentCultureInfo))
                                .AppendCell(FormatDatetimeByLenguage(lstResultado[index].DateCreatedUTC,CoreContext.CurrentCultureInfo))
                               .AppendCell(FormatDatetimeByLenguage(lstResultado[index].CurrentExpirationDateUTC,CoreContext.CurrentCultureInfo))
                               .AppendCell(FormatDatetimeByLenguage(lstResultado[index].DateValidity,CoreContext.CurrentCultureInfo))
                               .AppendCell(Convert.ToString(lstResultado[index].InitialAmount.ToMoneyString(CoreContext.CurrentCultureInfo)))
                               .AppendCell(Convert.ToString(lstResultado[index].FinancialAmount.ToMoneyString(CoreContext.CurrentCultureInfo)))
                               .AppendCell(Convert.ToString(lstResultado[index].DiscountedAmount.ToMoneyString(CoreContext.CurrentCultureInfo)), style: "color: red")
                               .AppendCell(Convert.ToString(lstResultado[index].TotalAmount.ToMoneyString(CoreContext.CurrentCultureInfo)))
                               .AppendCell((lstResultado[index].PaymentTypeID == "11") ? "<a href='javascript:void(0)' onclick='cargarBillingInformation(" + lstResultado[index].TicketNumber.ToString() + ")'>" + Translation.GetTerm(lstResultado[index].ViewTicket, "View Ticket") + "</a>" : Translation.GetTerm(lstResultado[index].ViewTicket, "View Ticket"))
                               .AppendCell((lstResultado[index].PaymentTypeID == "11") ? "<a href='javascript:void(0)' onclick='loadBillingInformation(" + lstResultado[index].TicketNumber.ToString() + ")'> " + Translation.GetTerm("SendEmailPDF", "Send Email") + "</a>" : Translation.GetTerm("SendEmailPDF", "Send Email"))

                           .Append("</tr>");
                        }
                        return Json(new { result = true, totalPages = lstResultado.TotalPages, page = builder.ToString() });
                    }
                    else
                        return Json(new { result = true, totalPages = 0, page = "<tr><td colspan=\"8\">" + Translation.GetTerm("ThereWereNoRecordsFoundThatMeetThatCriteriaPleaseTryAgain", "There were no records found that meet that criteria. Please try again.") + "</td></tr>" });
                }
                else
                {
                    Session["GridTicket"] = "1";
                    return Json(new { result = true, totalPages = 0, page = "<tr><td colspan=\"8\">" + Translation.GetTerm("ThereWereNoRecordsFoundThatMeetThatCriteriaPleaseTryAgain", "There were no records found that meet that criteria. Please try again.") + "</td></tr>" });

                }
            }
            catch (Exception ex)
            {

                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }

        }


        private string FormatDatetimeByLenguage(string fecha, CultureInfo culture)
        {
                if (string.IsNullOrEmpty(fecha))
                return "N/A";
                else
                {
                    DateTime? fec = null;
                    DateTime Temp;

                    var split = fecha.Split('/');
                 if(split.Length >=3)
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
        //Developer by salcedo vila G.
        #region  Automcomplete
        [OutputCache(CacheProfile = "AutoCompleteData")]
        public virtual ActionResult AccountSearch(string query)
        {
            try
            {

                var dcAccounts = PaymentTicktesBussinessLogic.AccountSearchAuto(query).Select((dc) => new { id = dc.Key, text = dc.Value + "(" + dc.Key + ")" });
                return Json(dcAccounts);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
        #endregion
        #endregion        

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

        #region exportacion Excel
        /// <summary>
        ///  Listar la grilla para exportar excel 
        ///  los parametros se envian via Request
        /// </summary>
        /// <returns>retorna </returns>
        public ActionResult ExportarExcelGrillaTicket()
        {

            PaymentTicketsSearchParameters param = new PaymentTicketsSearchParameters();

            param.NegotiationLevelID = (int.Parse(Request.QueryString["LevelId"]) == 0 ? null : (int?)int.Parse(Request.QueryString["LevelId"]));
            param.ExpirationStatusID = (int.Parse(Request.QueryString["ExpirationSituationId"]) == 0 ? null : (int?)int.Parse(Request.QueryString["ExpirationSituationId"]));

            param.Country = (int?)int.Parse(Request.QueryString["CountryId"]);
            param.BankID = (Request.QueryString["BankId"] == "0" ? null : (int?)int.Parse(Request.QueryString["BankId"]));
            param.Forefit = (Request.QueryString["Forefit"] == "-1" ? null : (int?)int.Parse(Request.QueryString["Forefit"]));
            param._AccountID = (Request.QueryString["AccountID"] == "0" || Request.QueryString["AccountID"] == "" ? null : (int?)int.Parse(Request.QueryString["AccountID"]));
            param.ExpiriedDateFrom = ConvertDateMMDDYYY(Request.QueryString["startDateExpirationDate"].ToString());
            param.ExpiriedDateTo = ConvertDateMMDDYYY(Request.QueryString["endDateExpirationDate"].ToString());
            param.LiquidationDateFrom = ConvertDateMMDDYYY(Request.QueryString["startDateLiquidationDate"].ToString());
            param.LiquidationDateTo = ConvertDateMMDDYYY(Request.QueryString["endDateLiquidationDate"].ToString());
            param.OrderId = (Request.QueryString["OrderId"] == "" ? null : (int?)int.Parse(Request.QueryString["OrderId"]));
            param.OrderPaymentStatusId = (Request.QueryString["OrderPaymentStatusId"] == "0" ? null : (int?)int.Parse(Request.QueryString["OrderPaymentStatusId"]));

            param.FiscalNote = (Request.QueryString["FiscalNote"] == "" ? null : Request.QueryString["FiscalNote"]);
            param.TicketNumber = (Request.QueryString["TicketNumber"] == "" || Request.QueryString["TicketNumber"] == null ? null : (int?)int.Parse(Request.QueryString["TicketNumber"]));

            param.OrderBy = Request.QueryString["orderBy"].ToString();
            param.OrderByDirection = NetSteps.Common.Constants.SortDirection.Descending;



            StringBuilder builder = new StringBuilder();
            DataTable lstResultado = new DataTable();
            lstResultado.Columns.Add(Translation.GetTerm("AccountNumber"), typeof(string));
            lstResultado.Columns.Add(Translation.GetTerm("AccountName"), typeof(string));

            lstResultado = PaymetTycketsReportBusinessLogic.SearchTicketsReport(param);

            if (param.OrderBy != null && param.OrderBy != "")
            {
                switch (param.OrderByDirection)
                {
                    case NetSteps.Common.Constants.SortDirection.Descending:
                        lstResultado.DefaultView.Sort = param.OrderBy + " desc";

                        break;
                    case NetSteps.Common.Constants.SortDirection.Ascending:
                        lstResultado.DefaultView.Sort = param.OrderBy + "  ASC";

                        break;
                    default:
                        break;
                }
            }

            string fileNameSave = string.Format("{0}_{1}.xlsx", Translation.GetTerm("TicketSearch", "Ticket Search"), DateTime.Now.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo));




            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "Web App";
                excelPackage.Workbook.Properties.Title = "Export from the Web";
                var sheet = excelPackage.Workbook.Worksheets.Add("Export Results");
                // output a line for the headers
                //CreateHeader(sheet);

                sheet.Cells[1, 1].Value = Translation.GetTerm("AccountNumber");
                sheet.Cells[1, 2].Value = Translation.GetTerm("AccountName");
                sheet.Cells[1, 3].Value = Translation.GetTerm("TicketNumber");
                sheet.Cells[1, 4].Value = Translation.GetTerm("OrderID");
                sheet.Cells[1, 5].Value = Translation.GetTerm("StatusPaymentName");

                sheet.Cells[1, 6].Value = Translation.GetTerm("NegotiationLevelName");
                sheet.Cells[1, 7].Value = Translation.GetTerm("NameExpiration");
                sheet.Cells[1, 8].Value = Translation.GetTerm("ExpirationDays");
                sheet.Cells[1, 9].Value = Translation.GetTerm("Period");
                sheet.Cells[1, 10].Value = Translation.GetTerm("Forefit");

                sheet.Cells[1, 11].Value = Translation.GetTerm("DateCreatedUTC");
                sheet.Cells[1, 12].Value = Translation.GetTerm("ExperiedDate");
                sheet.Cells[1, 13].Value = Translation.GetTerm("DateValidity");
                sheet.Cells[1, 14].Value = Translation.GetTerm("PrincipalAmount");
                sheet.Cells[1, 15].Value = Translation.GetTerm("FinancialAmount");
                sheet.Cells[1, 16].Value = Translation.GetTerm("DiscountedAmount");
                sheet.Cells[1, 17].Value = Translation.GetTerm("TotalAmount");


                sheet.Name = "export results";
                // all indexes start at 1
                var rowIndex = 2;
                foreach (DataRow dtRow in lstResultado.Rows)
                {
                    var col = 1;
                    sheet.Cells[rowIndex, col++].Value = dtRow["AccountNumber"].ToString();
                    sheet.Cells[rowIndex, col++].Value = dtRow["AccountName"].ToString();
                    sheet.Cells[rowIndex, col++].Value = dtRow["TicketNumber"].ToString();
                    sheet.Cells[rowIndex, col++].Value = dtRow["OrderNumber"].ToString();
                    sheet.Cells[rowIndex, col++].Value = dtRow["StatusPaymentName"].ToString();

                    sheet.Cells[rowIndex, col++].Value = dtRow["NegotiationLevelName"].ToString();
                    sheet.Cells[rowIndex, col++].Value = dtRow["NameExpiration"].ToString();
                    sheet.Cells[rowIndex, col++].Value = dtRow["ExpirationDays"].ToString();
                    sheet.Cells[rowIndex, col++].Value = dtRow["Period"].ToString();
                    sheet.Cells[rowIndex, col++].Value = dtRow["Forefit"].ToString();

                    sheet.Cells[rowIndex, col++].Value = dtRow["DateCreatedUTC"].ToString();
                    sheet.Cells[rowIndex, col++].Value = dtRow["CurrentExpirationDateUTC"].ToString();
                    sheet.Cells[rowIndex, col++].Value = dtRow["DateValidity"].ToString();
                    sheet.Cells[rowIndex, col++].Value = dtRow["InitialAmount"].ToString();
                    sheet.Cells[rowIndex, col++].Value = dtRow["FinancialAmount"].ToString();
                    sheet.Cells[rowIndex, col++].Value = dtRow["DiscountedAmount"].ToString();
                    sheet.Cells[rowIndex, col++].Value = dtRow["TotalAmount"].ToString();



                    rowIndex++;
                }

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition",
                          "attachment;filename=" + fileNameSave + ".xlsx");
                Response.ContentType = "application/excel";
                Response.Flush();
                Response.End();
            }


            return View("MyView");
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
                Code += dtInfoBank.Rows[0]["BankAgence"].ToString().Substring(0,4);
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

                var ruta = ConfigurationManager.AppSettings["FileUploadWebPath"];

                var path = ruta + nameFile; //@"\\10.12.6.183\FileUploads\ReportsPDF\" + nameFile;//Server.MapPath("~/Reports/FilesTemp/" + nameFile);  //   se coloca  o exporta el archivo
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                System.IO.File.WriteAllBytes(path, output);
                File(path, "application/pdf", Path.GetFileName(path));


                 var v  = AccountPropertiesBusinessLogic.GetEmailTemplate(OrderPaymentID, nameFile);
                 bool sentSuccessfully = v.Respuesta == 1 ? true : false;
               

                if (sentSuccessfully)
                    return Json(new { result = true, message = Translation.GetTerm("EmailSentSuccessfully", "Email Sent Successfully!") });
                else
                    return Json(new { result = false, message = Translation.GetTerm("EmailNotSentSuccessfully", "Not process the Email") });


            } 
                 

            catch (Exception ex)
            {
                string msg = Translation.GetTerm("EmailNotSentForData", "Not process e-mail due to the format of values.");
                //throw ex;
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = msg });// exception.PublicMessage });
            }
        }

        private EmailTemplateContentModel ConvertDinamicToModel(dynamic dat)
        {
            EmailTemplateContentModel model = new EmailTemplateContentModel();
            model.Body = dat.Body;
            model.EmailTemplateTypeID = dat.EmailTemplateTypeID;
            model.LanguageID = dat.LanguageID;
            model.Subject = dat.Subject;
            model.To = dat.To;
            return model;
        }

        [FunctionFilter("Communication", "~/Accounts")]
        public virtual bool SendEmail(EmailTemplateContentModel model, string attachmentPath)
        {
            bool result = false;
            var revisar = Request.Url;
            //string ruta02 = string.Format(@"\\{0}{1}{2}{3}{4}{5}{6}", "10.12.6.183",
            //                                   System.IO.Path.DirectorySeparatorChar,
            //                                   "FileUploads",
            //                                   System.IO.Path.DirectorySeparatorChar,
            //                                   "ReportsPDF",
            //                                   System.IO.Path.DirectorySeparatorChar
            //                                   , attachmentPath
            //                                   );

            //string ruta03 = string.Format("{0}\{2}","//10.12.6.183","FileUploads");


            //string r="";
            //if (ConfigurationManager.AppSettings["FileReportsPDF"] == null)
            //    r = @"\\10.12.6.183\FileUploads\ReportsPDF\";
            //else
            //    r = ConfigurationManager.AppSettings["FileReportsPDF"];

            //var FileReportsPDF = r + attachmentPath;
            //string rutaPDF = " <a  href='" + FileReportsPDF + "' >descargar archivo02</a>";

            string rutaenBdy = " <a  href='http://portal.belcorpbra.qas.draftbrasil.com/GeneralLedger/PaymentTicketsReport/Descargar-Archivo/" + attachmentPath + "'>Baixar o arquivo Banco</a>";
            //string rutaenBdy = " <a  href='http://localhost:40000/GeneralLedger/PaymentTicketsReport/Descargar-Archivo/" + attachmentPath + "'>Baixar o arquivo Banco</a>";
          // string rutaenBdy = " <a  href='"+ruta02+"'>Baixar o arquivo Banco</a>";

            EmailTemplateTranslation tempTranslation = new EmailTemplateTranslation()
            {
                Subject = model.Subject,
                Body = rutaenBdy + "<br>" +  model.Body,
            };

            var mailMessage = EmailTemplateContentModel.GetPreviewMailMessage(model, tempTranslation);

            mailMessage.To.Add(new NetSteps.Data.Entities.Mail.MailMessageRecipient(model.To));


            var corporateMailAccountId = ConfigurationManager.GetAppSetting<int?>(ConfigurationManager.VariableKey.CorporateAccountID);
            int mailMessageID = mailMessage.Send(MailAccount.LoadByAccountID(corporateMailAccountId ?? 1), 420);

            if (mailMessageID > 0)
            {
                result = true;
            }

            return result;
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
