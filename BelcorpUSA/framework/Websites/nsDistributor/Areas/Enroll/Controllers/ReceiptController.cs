using System.Linq;
using System.Web.Mvc;
using nsDistributor.Areas.Enroll.Models.Receipt;
using System.Collections.Generic;
using Microsoft.Reporting.WebForms;
using System.Data;
using System;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities.Dto;
using CodeBarGeerator;
using iTextSharp.text.pdf;
using iTextSharp.text;
using NetSteps.Data.Entities.Business.Logic;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Common.Configuration;
using System.IO;
using NetSteps.Data.Entities.Exceptions;

namespace nsDistributor.Areas.Enroll.Controllers
{
    public class ReceiptController : EnrollStepBaseController
    {
        public virtual ActionResult Index()
        {
            // Make sure enrollment is complete
            if (!_enrollmentContext.EnrollmentComplete)
            {
                return RedirectToStep(_enrollmentContext.EnrollmentConfig.Steps.First());
            }

            var cultura= CoreContext.CurrentCultureInfo.Name;
            var model = new IndexModel();

            bool showInitialOrder = true;
            bool showAutoshipOrder = !_enrollmentContext.EnrollmentConfig.Autoship.Hidden && ShouldShowAutoshipOrders();
            bool showSubscriptionOrder = ShouldShowAutoshipOrders();

            model.LoadResources(
                false,
                ShouldShowSponsor(),
                _enrollmentContext,
                FormatPWSUrl(_enrollmentContext.SiteSubscriptionUrl),
                showInitialOrder,
                showAutoshipOrder,
                showSubscriptionOrder);

            LoadPageHtmlContent();
            //ValidateSendEmailBoleto(Convert.ToInt32(model.OrderNumber));
            this.ResetCurrentAccount();  
            return View(model);
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

        protected virtual bool ShouldShowSponsor()
        {
            return true;
        }

        protected virtual bool ShouldShowAutoshipOrders()
        {
            return true;
        }

        protected virtual void ResetCurrentAccount()
        {
            if (CoreContext.CurrentAccount != null)
            {
                CoreContext.CurrentAccount = NetSteps.Data.Entities.Account.LoadForSession(CoreContext.CurrentAccount.AccountID);
            }
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
