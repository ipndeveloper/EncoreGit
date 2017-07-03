using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Configuration;
using NetSteps.Data.Entities.Repositories.Interfaces;
using NetSteps.Data.Entities.Utility;
using System.Xml;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Data.Entities.Dto;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using System.Globalization;
using NetSteps.Common.Globalization;
using System.Data;
using System.Data.SqlClient;

/*
 * @01 20150820 BR-E020 CSTI JMO: Added ConfirmacionDespachoGKO and  Method as well as E0020ErrorMessages constant class
 */

namespace NetSteps.Data.Entities.Business.Logic
{
     
    public static class Formater
    {
      public  static string Formato(this decimal Texto)
        {
           decimal valor=0.0m;
            var KeyDecimals = ConfigurationManager.AppSettings["CultureDecimal"];
            if (KeyDecimals == "ES")
            {
                var culture = CultureInfoCache.GetCultureInfo("En");
                valor = Convert.ToDecimal(Texto.ToString(), culture);
                return valor.ToString();
            }
            else
            {
                return (Texto == null ? 0.0m : Texto).ToString().Replace(",", ".");
            }

            return "";
          
        }
    }

    public class XmlGeneratorBusinessLogic
    {
        #region Methods

        #region Req. BR010 cancelacion de ordenes

        public string GenerateXmlForCancelledOrder(string xmlPath,int OrderID)
        {
            string TemplateCancelledOrder = FileHelper.GetText(string.Format("{0}{1}.xml", xmlPath, "TemplateCancelledOrder"));
            string TemplateOrderDetail = FileHelper.GetText(string.Format("{0}{1}.xml", xmlPath, "TemplateOrderDetail"));

            OrderHeaderXml OrderHeader = (from r in repository.GetOrderHead(OrderID) select DtoToBO(r)).ToList()[0];
            List<OrderDetailXml> ListOrderDetail = (from r in repository.GetOrderDetail(OrderID) select DtoToBO(r)).ToList();
            AdvancePaymentXml AdvancePayement = (from r in repository.GetAdvancePayment(OrderID) select DtoToBO(r)).ToList()[0];

            StringBuilder XmlOrderDetail = new StringBuilder();
            foreach (var item in ListOrderDetail) XmlOrderDetail.AppendFormat(TemplateOrderDetail, item.Linea, item.CategoriaItem, item.Material, item.Quantidade, item.CentroDistribucao, item.PresoPraticado, item.Desconto);

            string XmlCancelledOrder = string.Format(TemplateCancelledOrder,
                                                       OrderHeader.NumeroPedido,
                                                       OrderHeader.NumeroPedido,
                                                       OrderHeader.TipoOrdem,
                                                       OrderHeader.EmisordaOrdem,
                                                       OrderHeader.RecebedorMercaderia,
                                                       OrderHeader.Trasportador,
                                                       OrderHeader.LoteTransporte,
                                                       OrderHeader.DataPedido,
                                                       OrderHeader.FormaPagamento,
                                                       OrderHeader.Frete,
                                                       XmlOrderDetail.ToString(),
                                                       AdvancePayement.NumeroTitulo,
                                                       OrderHeader.NumeroPedido);
            
            return XmlCancelledOrder;
        }
        #endregion
        /// <summary>
        /// Developed By KTC - CSTI
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static List<string> MLM_MaterialesE_B200(string DocXml)
        {
            /*ARAMOS (CurrentCultureInfo).Replace(",", "."); decimales*/
            CultureInfo CurrentCultureInfo = CultureInfoCache.GetCultureInfo("en-US");
            /*ARAMOS*/

            string xmlPath = ConfigurationManager.AppSettings["TemplatesXML_Path"];

            string TemplateMaterialLog = FileHelper.GetText(string.Format("{0}{1}.xml", xmlPath, "TemplateB200LogHeader"));
            string TemplateMaterialLogDet = FileHelper.GetText(string.Format("{0}{1}.xml", xmlPath, "TemplateB200LogDetail"));

            string TemplateMaterial = FileHelper.GetText(string.Format("{0}{1}.xml", xmlPath, "TemplateB200HistHeader"));
            string TemplateMaterialDet = FileHelper.GetText(string.Format("{0}{1}.xml", xmlPath, "TemplateB200HistDetail"));

            MaterialXml materialXml = new MaterialXml();
            StringBuilder XmlLogReturn = new StringBuilder();
            StringBuilder XmlHistReturn = new StringBuilder();
            List<MaterialLogXmlDto> xmlLogDto = new List<MaterialLogXmlDto>();
            List<MaterialCentersXml> xmlCenterXml = new List<MaterialCentersXml>();
            XDocument xml = XDocument.Parse(DocXml);
            bool existError = false;
            bool insertMaterial = false;
            XmlGeneratorRepository rep = new XmlGeneratorRepository();

            foreach (XElement element in xml.Descendants("MATERIAL"))
            {
                xmlLogDto = new List<MaterialLogXmlDto>();
                xmlCenterXml = new List<MaterialCentersXml>();
                materialXml = new MaterialXml();
                existError = false;
                insertMaterial = false;

                var valid = ValidateDecimal(element.Element("volumen").Value.ToString(), "Volumen");
                if (valid.CampoError != null) { xmlLogDto.Add(valid); existError = true; }

                valid = ValidateDecimal(element.Element("peso").Value.ToString(), "Peso");
                if (valid.CampoError != null) { xmlLogDto.Add(valid); existError = true; }

                if (!existError)
                {
                    insertMaterial = true;
                    materialXml.SKU = element.Element("sku").Value.ToString();
                    materialXml.BPCS = element.Element("bpcs").Value.ToString();
                    materialXml.Brand = element.Element("marca").Value.ToString();
                    materialXml.Group = element.Element("grupo").Value.ToString();
                    materialXml.NamePort = element.Element("portugues").Value.ToString();
                    materialXml.NameEsp = element.Element("espanol").Value.ToString();
                    
                    //materialXml.Volume = element.Element("volumen").Value.ToString(CurrentCultureInfo);
                    //materialXml.Weight = element.Element("peso").Value.ToString(CurrentCultureInfo);
                    materialXml.Volume = element.Element("volumen").Value.ToString(CurrentCultureInfo).Replace(",", ".");
                    materialXml.Weight = element.Element("peso").Value.ToString(CurrentCultureInfo).Replace(",", ".");

                    materialXml.Hierachy = element.Element("jerarquia").Value.ToString();

                    instance = new XmlGeneratorBusinessLogic();
                    var materialXmlDto = instance.BOToDto(materialXml);

                    var lstInsert = rep.InsertMaterialDto(materialXmlDto);

                    if (lstInsert.Count > 0 && lstInsert.First().CampoError != "" && lstInsert.First().CampoError != null)
                    {
                        foreach (var item in lstInsert)
                        {
                            xmlLogDto.Add(new MaterialLogXmlDto()
                            {
                                CampoError = item.CampoError,
                                DescError = item.DescError
                            });
                        }
                    }
                }

                foreach (XElement element02 in element.Descendants("Centros"))
                {
                    valid = ValidateDecimal(element02.Element("Costo").Value.ToString(), "Costo");
                    if (valid.CampoError != null) { xmlLogDto.Add(valid); existError = true; }

                    if (!existError)
                    {
                        var matCenXml = new MaterialCentersXml
                        {
                            SKU = materialXml.SKU,
                            Centro = element02.Element("Centro").Value.ToString(),
                            Costo = element02.Element("Costo").Value.ToString(CurrentCultureInfo).Replace(",", ".")
                        };
                        rep = new XmlGeneratorRepository();

                        instance = new XmlGeneratorBusinessLogic();
                        var materialCenterXmlDto = instance.BOToDto(matCenXml);
                        var xmlLogDto02 = rep.InsertWarehouseMaterialsDto(materialCenterXmlDto);

                        if (xmlLogDto02.DescError != "" && xmlLogDto02.DescError != null)
                            xmlLogDto.Add(xmlLogDto02);
                        else
                            xmlCenterXml.Add(matCenXml);

                    }
                }

                List<MaterialLogXml> xmlLog = instance.DtoToBO(xmlLogDto);
                StringBuilder XmlDetailLog = new StringBuilder();
                StringBuilder XmlDetail = new StringBuilder();
                if (xmlLog.Count > 0)
                {
                    foreach (var item in xmlLog) XmlDetailLog.AppendFormat(
                        TemplateMaterialLogDet, item.CampoError, item.DescError
                        );
                    XmlDetailLog.Replace("<ERROR>", "");
                    XmlDetailLog.Replace("</ERROR>", "");

                    XmlLogReturn.AppendFormat(TemplateMaterialLog, materialXml.SKU, XmlDetailLog);
                }

                if (insertMaterial)
                {
                    foreach (var item in xmlCenterXml) XmlDetail.AppendFormat(
                        TemplateMaterialDet, item.Centro, item.Costo
                        );
                    XmlHistReturn.AppendFormat(TemplateMaterial,
                                                materialXml.SKU,
                                                materialXml.BPCS,
                                                materialXml.Brand,
                                                materialXml.Group,
                                                materialXml.NamePort,
                                                materialXml.NameEsp,
                                                materialXml.Volume,
                                                materialXml.Weight,
                                                materialXml.Hierachy,
                                                XmlDetail);
                }
            }
            StringBuilder builderXmlLog = new StringBuilder();
            builderXmlLog.Append("<?xml version=").Append("\"").Append("1.0").Append("\" ").Append("encoding=").Append("\"").Append("utf-8").Append("\"").Append(" ?>");
            builderXmlLog.Append("<ERROR>");
            builderXmlLog.Append(XmlLogReturn.ToString());
            builderXmlLog.Append("</ERROR>");

            StringBuilder builderXmlHist = new StringBuilder();
            builderXmlHist.Append("<?xml version=").Append("\"").Append("1.0").Append("\" ").Append("encoding=").Append("\"").Append("utf-8").Append("\"").Append(" ?>");
            builderXmlHist.Append("<MATERIALES>");
            builderXmlHist.Append(XmlHistReturn.ToString());
            builderXmlHist.Append("</MATERIALES>");

            List<string> lstRet = new List<string>();

            lstRet.Add(builderXmlLog.ToString());
            lstRet.Add(builderXmlHist.ToString());
            return lstRet;
        }

        /// <summary>
        /// Developed By KLC - CSTI
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        /// 

        ///Las siguientes líneas son las líneasa originales antes de la modificación
        //public string MLM_FacturacionE_B070(string DocXml)
        //{
        //    string xmlPath = ConfigurationManager.AppSettings["TemplatesXML_Path"];
        //    string timeStamp = GetTimeStamp();
        //    string fileNameHist = "MLM_FacturacionE_B070_LOG_" + timeStamp + ".xml";
        //    string filePathHist = ConfigurationManager.AppSettings["FileUploadAbsolutePath"] + ConfigurationManager.AppSettings["FileUploadPath_B070_Log"] + fileNameHist;
        //    string TemplateErrorInvoice = FileHelper.GetText(string.Format("{0}{1}.xml", xmlPath, "TemplateB070Error"));
        //    string TemplateErrorInvoiceHeader = FileHelper.GetText(string.Format("{0}{1}.xml", xmlPath, "TemplateB070LogHeader"));
        //    string TemplateErrorInvoiceDetail = FileHelper.GetText(string.Format("{0}{1}.xml", xmlPath, "TemplateB070LogDetail"));
            
        //    //string TemplateBillsBalance = FileHelper.GetText(string.Format("{0}{1}.xml", xmlPath, "TemplateBillsBalance"));
        //    //string TemplateBillsBalanceDetail = FileHelper.GetText(string.Format("{0}{1}.xml", xmlPath, "TemplateB070BillsBalanceDetail"));
        //    string XmlReturn = "";
        //    string XmlDoc = "";

        //    XDocument xml = XDocument.Parse(DocXml);

        //    //xml.LoadXml(DocXml);            
        //    //XmlNodeList Invoices = xml.GetElementsByTagName("ET_FATURAS");
        //    //XmlNodeList Invoice = ((XmlElement)Invoices[0]).GetElementsByTagName("item");

        //    StringBuilder XmlErrorDetail = new StringBuilder();
        //    List<BalancesBillOrdersXmlDto> balancesBillOrdersXml = new List<BalancesBillOrdersXmlDto>();
        //    XMLProductSearchData InvoiceError = new XMLProductSearchData();
        //    Errores error = new Errores();
        //    InvoiceError.Errores = new List<Errores>();
            
        //    StringBuilder XmlInvoiceLog = new StringBuilder();
        //    StringBuilder XmlInvoiceDetail = new StringBuilder();


        //    foreach (XElement element in xml.Descendants("item").Where(x => x.Element("ORDERNUMBER") != null))
        //    {
        //        XElement child = element.Descendants("item").FirstOrDefault();

        //        var InvoiceOrder = new BalancesBillOrdersXmlDto
        //            {
        //                OrderNumber = element.Element("ORDERNUMBER").Value.ToString(),
        //                InvoiceNumber = element.Element("INVOICENUMBER").Value.ToString(),
        //                InvoiceSerie = element.Element("INVOICESERIE").Value.ToString(),
        //                DateInvoice = element.Element("DATEINVOICE").Value.ToString(),
        //                InvoiceType = element.Element("INVOICETYPE").Value.ToString(),
        //                DistributionCenter = element.Element("DISTRIBUTIONCENTER").Value.ToString(),
        //                ChaveNFe = element.Element("CHAVENFE").Value.ToString(),
        //                Boxes = element.Element("BOXES").Value.ToString(),
        //                Weight = element.Element("WEIGHT").Value.ToString(),

        //                SortLine = child.Element("SORTLINE").Value.ToString(),
        //                Material = child.Element("MATERIAL").Value.ToString(),
        //                Quantity = child.Element("QUANTITY").Value.ToString(),
        //                ICMS = child.Element("ICMS").Value.ToString(),
        //                ICMS_ST = child.Element("ICMS_ST").Value.ToString(),
        //                IPI = child.Element("IPI").Value.ToString(),
        //                PIS = child.Element("PIS").Value.ToString(),
        //                COFINS = child.Element("COFINS").Value.ToString(),
        //                InvoiceUnitValue = child.Element("INVOICEUNITVALUE").Value.ToString(),
        //                InvoicePath = filePathHist.ToString()
        //            };


        //        if (Order.ExistsByOrderNumber(InvoiceOrder.OrderNumber))
        //        {
        //            //1
        //            Order.updateStatusOrderInvoiced(Convert.ToString(InvoiceOrder.OrderNumber));
        //            //                 
        //            repository.InsSAPEncoreFacturas(InvoiceOrder);
        //            // Proceso de Actualización de Saldo
        //            int xmlLogDto = repositoryWareHouseMaterialBussiness.UpdateSaldo(Convert.ToString(InvoiceOrder.Material), Convert.ToInt32(InvoiceOrder.DistributionCenter), Convert.ToInt32(InvoiceOrder.Quantity));
        //        }
        //        else
        //        {
        //            error.DescError = "Order Number";
        //            error.CampoError = "Numero de Orden no existe";
        //        }

        //        InvoiceError.Errores.Add(error);
        //        foreach (var item in InvoiceError.Errores)
        //        {
        //            XmlInvoiceLog.AppendFormat(TemplateErrorInvoiceDetail, item.DescError, item.CampoError);
        //        }

        //        XmlReturn += string.Format(TemplateErrorInvoiceHeader,InvoiceOrder.OrderNumber, XmlInvoiceLog);
        //        InvoiceError.Errores.Clear();
        //        XmlInvoiceLog.Clear();
        //    }
        //    XmlReturn.Replace("<Errores>", "");
        //    XmlReturn.Replace("</Errores>", "");            
        //    XmlDoc = string.Format(TemplateErrorInvoice, XmlReturn);
        //    FileHelper.Save(XmlDoc, filePathHist);
        //    return XmlDoc;

        //}

        #region B070

        public string MLM_FacturacionE_B070(string DocXml)
        {
            string xmlPath = ConfigurationManager.AppSettings["TemplatesXML_Path"];
            string timeStamp = GetTimeStamp();
            string fileNameHist = "MLM_FacturacionE_B070_LOG_" + timeStamp + ".xml";
            string filePathHist = ConfigurationManager.AppSettings["FileUploadAbsolutePath"] + ConfigurationManager.AppSettings["FileUploadPath_B070_Log"] + fileNameHist;
            string fileNamePDF = ConfigurationManager.AppSettings["FileUploadAbsolutePath"] + ConfigurationManager.AppSettings["FileUploadPath_B070_Log"];
            string TemplateErrorInvoice = FileHelper.GetText(string.Format("{0}{1}.xml", xmlPath, "TemplateB070Error"));
            string TemplateErrorInvoiceHeader = FileHelper.GetText(string.Format("{0}{1}.xml", xmlPath, "TemplateB070LogHeader"));
            string TemplateErrorInvoiceDetail = FileHelper.GetText(string.Format("{0}{1}.xml", xmlPath, "TemplateB070LogDetail"));

            //string TemplateBillsBalance = FileHelper.GetText(string.Format("{0}{1}.xml", xmlPath, "TemplateBillsBalance"));
            //string TemplateBillsBalanceDetail = FileHelper.GetText(string.Format("{0}{1}.xml", xmlPath, "TemplateB070BillsBalanceDetail"));
            string XmlReturn = "";
            string XmlDoc = "";
            bool isValorOk = true;

            XDocument xml = XDocument.Parse(DocXml);

            //xml.LoadXml(DocXml);            
            //XmlNodeList Invoices = xml.GetElementsByTagName("ET_FATURAS");
            //XmlNodeList Invoice = ((XmlElement)Invoices[0]).GetElementsByTagName("item");

            StringBuilder XmlErrorDetail = new StringBuilder();
            List<BalancesBillOrdersXmlDto> balancesBillOrdersXml = new List<BalancesBillOrdersXmlDto>();
            XMLProductSearchData InvoiceError = new XMLProductSearchData();
            Errores error = new Errores();
            InvoiceError.Errores = new List<Errores>();

            StringBuilder XmlInvoiceLog = new StringBuilder();
            StringBuilder XmlInvoiceDetail = new StringBuilder();

            #region recorridonodoinvoice
                foreach (XElement element in xml.Descendants("item").Where(x => x.Element("ORDERNUMBER") != null))
                {

                    var InvoiceOrder = new BalancesBillOrdersXmlDto
                    {
                        OrderNumber = element.Element("ORDERNUMBER").Value.ToString(),
                        InvoiceNumber = element.Element("INVOICENUMBER").Value.ToString(),
                        InvoiceSerie = element.Element("INVOICESERIE").Value.ToString(),
                        DateInvoice = element.Element("DATEINVOICE").Value.ToString(),
                        InvoiceType = element.Element("INVOICETYPE").Value.ToString(),
                        DistributionCenter = element.Element("DISTRIBUTIONCENTER").Value.ToString(),
                        ChaveNFe = element.Element("CHAVENFE").Value.ToString(),
                        Boxes = element.Element("BOXES").Value.ToString(),
                        Weight = element.Element("WEIGHT").Value.ToString(),

                        /// Modificación: Campos nuevos 
                        /// Fecha: 17/03/2016 
                        /// Author: MAM - CSTI
                        WeightLiq = element.Element("WEIGHT_LIQ").Value.ToString(),
                        HeadICMSBase = element.Element("ICMS_BASE").Value.ToString(),
                        HeadICMSValue = element.Element("ICMS_VALUE").Value.ToString(),
                        HeadICMSSTBase = element.Element("ICMSST_BASE").Value.ToString(),
                        HeadICMSSTValue = element.Element("ICMSST_VALUE").Value.ToString(),
                        HeadIPIBase = element.Element("IPI_BASE").Value.ToString(),
                        HeadIPIValue = element.Element("IPI_VALUE").Value.ToString(),
                        /// Fin Modificación.

                        //InvoicePath = filePathHist.ToString()
                        InvoicePath = fileNamePDF.ToString() + element.Element("INVOICENUMBER").Value.ToString() + ".PDF"

                        
                    };

                    /// Recorrer InvoiceDetail
                    /// Primero obtenemos el nodo de detalles.

                    XElement xInvoiceDetails = element.Element("INVOICEDETAIL");

                    /// Si tiene elementos los recorremos, creamos el objeto representativo del detalle y lo agregamos a la lista de detalles de la factura, 
                    /// sino, lanza una excepción.
                    if (xInvoiceDetails.HasElements)
                    {
                        foreach (var item in xInvoiceDetails.Elements())
                        {
                            BalancesBillOrdersItemsXmlDto invoiceDetail = new BalancesBillOrdersItemsXmlDto()
                            {
                                QuantityPicked = item.Element("QUANTITY_PICKED").Value.ToString(),
                                ICMSAliq = item.Element("ICMS_ALIQ").Value.ToString(),
                                IPIAliq = item.Element("IPI_ALIQ").Value.ToString(),
                                CFOP = item.Element("CFOP").Value.ToString(),
                                SortLine = item.Element("SORTLINE").Value.ToString(),
                                Material = item.Element("MATERIAL").Value.ToString(),
                                Quantity = item.Element("QUANTITY").Value.ToString(),
                                ICMS = item.Element("ICMS").Value.ToString(),
                                ICMS_ST = item.Element("ICMS_ST").Value.ToString(),
                                IPI = item.Element("IPI").Value.ToString(),
                                PIS = item.Element("PIS").Value.ToString(),
                                COFINS = item.Element("COFINS").Value.ToString(),
                                InvoiceUnitValue = item.Element("INVOICEUNITVALUE").Value.ToString()
                            };

                            InvoiceOrder.DetailList.Add(invoiceDetail);
                        }
                    }
                    else
                    {
                        error.DescError = "InvoiceDetail Error";
                        error.CampoError = "No contiene detalles de factura: " + InvoiceOrder.OrderNumber;
                        isValorOk = false;
                    }

                    if (Order.ExistsByOrderNumber(InvoiceOrder.OrderNumber.TrimStart('0')) && isValorOk)
                    {
                        //Actualizar el estado a "Invoiced"
                        Order.updateOrderStatus(Convert.ToString(InvoiceOrder.OrderNumber.TrimStart('0')), (int)Constants.OrderStatus.Invoiced);
                        //

                        foreach (var item in InvoiceOrder.DetailList)
	                    {
                            repository.InsSAPEncoreFacturas(InvoiceOrder, item);
	                    }

                        repository.InsSAPEncoreFacturasUpdateCvValue(InvoiceOrder.OrderNumber);
                    }
                    else
                    {
                        error.DescError = "Order Number";
                        error.CampoError = "Numero de Orden no existe en el sistema: " + InvoiceOrder.OrderNumber;
                        isValorOk = false;
                    }

                    if (!isValorOk)
                    {
                        InvoiceError.Errores.Add(error);
                        foreach (var item in InvoiceError.Errores)
                        {
                            XmlInvoiceLog.AppendFormat(TemplateErrorInvoiceDetail, item.DescError, item.CampoError);
                        }

                        XmlReturn += string.Format(TemplateErrorInvoiceHeader, InvoiceOrder.OrderNumber, XmlInvoiceLog);
                        InvoiceError.Errores.Clear();
                        XmlInvoiceLog.Clear();
                    }
                }
            #endregion

            
                if (!isValorOk)
                {
                    XmlReturn.Replace("<Errores>", "");
                    XmlReturn.Replace("</Errores>", "");
                    XmlDoc = string.Format(TemplateErrorInvoice, XmlReturn);
                    FileHelper.Save(XmlDoc, filePathHist);
                    return XmlDoc;
                }
                else
                    return "";
                
                
        }

        #endregion

        #region B055

        public string MLM_Picking_B055(string DocXml)
        {

            string xmlPath = ConfigurationManager.AppSettings["TemplatesXML_Path"];
            string timeStamp = GetTimeStamp();
            string fileNameHist = "MLM_FacturacionE_B070_LOG_" + timeStamp + ".xml";
            string filePathHist = ConfigurationManager.AppSettings["FileUploadAbsolutePath"] + ConfigurationManager.AppSettings["FileUploadPath_B070_Log"] + fileNameHist;
            string fileNamePDF = ConfigurationManager.AppSettings["FileUploadAbsolutePath"] + ConfigurationManager.AppSettings["FileUploadPath_B070_Log"];
            string TemplateErrorInvoice = FileHelper.GetText(string.Format("{0}{1}.xml", xmlPath, "TemplateB070Error"));
            string TemplateErrorInvoiceHeader = FileHelper.GetText(string.Format("{0}{1}.xml", xmlPath, "TemplateB070LogHeader"));
            string TemplateErrorInvoiceDetail = FileHelper.GetText(string.Format("{0}{1}.xml", xmlPath, "TemplateB070LogDetail"));

            //string TemplateBillsBalance = FileHelper.GetText(string.Format("{0}{1}.xml", xmlPath, "TemplateBillsBalance"));
            //string TemplateBillsBalanceDetail = FileHelper.GetText(string.Format("{0}{1}.xml", xmlPath, "TemplateB070BillsBalanceDetail"));
            string XmlReturn = "";
            string XmlDoc = "";
            bool isValorOk = true;
            XDocument xml = XDocument.Parse(DocXml);

            StringBuilder XmlInvoiceLog = new StringBuilder();
            StringBuilder XmlInvoiceDetail = new StringBuilder();

            XMLProductSearchData InvoiceError = new XMLProductSearchData();
            Errores error = new Errores();
            InvoiceError.Errores = new List<Errores>();


            #region recorridonodoinvoice
            foreach (XElement element in xml.Descendants("item").Where(x => x.Element("ORDERNUMBER") != null))
            {

                var InvoiceOrder = new BalancesBillOrdersXmlDto
                {
                    OrderNumber = element.Element("ORDERNUMBER").Value.ToString(),
                    InvoiceNumber = element.Element("INVOICENUMBER").Value.ToString(),
                    InvoiceSerie = element.Element("INVOICESERIE").Value.ToString(),
                    DateInvoice = element.Element("DATEINVOICE").Value.ToString(),
                    InvoiceType = element.Element("INVOICETYPE").Value.ToString(),
                    DistributionCenter = element.Element("DISTRIBUTIONCENTER").Value.ToString(),
                    ChaveNFe = element.Element("CHAVENFE").Value.ToString(),
                    Boxes = element.Element("BOXES").Value.ToString(),
                    Weight = element.Element("WEIGHT").Value.ToString(),

                    /// Modificación: Campos nuevos 
                    /// Fecha: 17/03/2016 
                    /// Author: MAM - CSTI
                    WeightLiq = element.Element("WEIGHT_LIQ").Value.ToString(),
                    HeadICMSBase = element.Element("ICMS_BASE").Value.ToString(),
                    HeadICMSValue = element.Element("ICMS_VALUE").Value.ToString(),
                    HeadICMSSTBase = element.Element("ICMSST_BASE").Value.ToString(),
                    HeadICMSSTValue = element.Element("ICMSST_VALUE").Value.ToString(),
                    HeadIPIBase = element.Element("IPI_BASE").Value.ToString(),
                    HeadIPIValue = element.Element("IPI_VALUE").Value.ToString(),
                    /// Fin Modificación.

                    //InvoicePath = filePathHist.ToString()
                    InvoicePath = fileNamePDF.ToString() + element.Element("INVOICENUMBER").Value.ToString() + ".PDF"


                };

                /// Recorrer InvoiceDetail
                /// Primero obtenemos el nodo de detalles.

                XElement xInvoiceDetails = element.Element("INVOICEDETAIL");

                /// Si tiene elementos los recorremos, creamos el objeto representativo del detalle y lo agregamos a la lista de detalles de la factura, 
                /// sino, lanza una excepción.
                if (xInvoiceDetails.HasElements)
                {
                    foreach (var item in xInvoiceDetails.Elements())
                    {
                        BalancesBillOrdersItemsXmlDto invoiceDetail = new BalancesBillOrdersItemsXmlDto()
                        {
                            QuantityPicked = item.Element("QUANTITY_PICKED").Value.ToString(),
                            ICMSAliq = item.Element("ICMS_ALIQ").Value.ToString(),
                            IPIAliq = item.Element("IPI_ALIQ").Value.ToString(),
                            CFOP = item.Element("CFOP").Value.ToString(),
                            SortLine = item.Element("SORTLINE").Value.ToString(),
                            Material = item.Element("MATERIAL").Value.ToString(),
                            Quantity = item.Element("QUANTITY").Value.ToString(),
                            ICMS = item.Element("ICMS").Value.ToString(),
                            ICMS_ST = item.Element("ICMS_ST").Value.ToString(),
                            IPI = item.Element("IPI").Value.ToString(),
                            PIS = item.Element("PIS").Value.ToString(),
                            COFINS = item.Element("COFINS").Value.ToString(),
                            InvoiceUnitValue = item.Element("INVOICEUNITVALUE").Value.ToString()
                        };

                        InvoiceOrder.DetailList.Add(invoiceDetail);
                    }
                }
                else
                {
                    error.DescError = "InvoiceDetail Error";
                    error.CampoError = "No contiene detalles de factura: " + InvoiceOrder.OrderNumber;
                    isValorOk = false;
                }

                if (Order.ExistsByOrderNumber(InvoiceOrder.OrderNumber.TrimStart('0')) && isValorOk)
                {
                    //Actualizar el estado a "Invoiced"
                    Order.updateOrderStatus(Convert.ToString(InvoiceOrder.OrderNumber.TrimStart('0')), (int)Constants.OrderStatus.Embarked);
                    //

                    foreach (var item in InvoiceOrder.DetailList)
                    {
                        repository.InsSAPEncorePicking(InvoiceOrder, item);
                    }

                    repository.InsSAPEncorePickingGenerateResidual(InvoiceOrder.OrderNumber);

                }
                else
                {
                    error.DescError = "Order Number";
                    error.CampoError = "Numero de Orden no existe en el sistema: " + InvoiceOrder.OrderNumber;
                    isValorOk = false;
                }

                if (!isValorOk)
                {
                    InvoiceError.Errores.Add(error);
                    foreach (var item in InvoiceError.Errores)
                    {
                        XmlInvoiceLog.AppendFormat(TemplateErrorInvoiceDetail, item.DescError, item.CampoError);
                    }

                    XmlReturn += string.Format(TemplateErrorInvoiceHeader, InvoiceOrder.OrderNumber, XmlInvoiceLog);
                    InvoiceError.Errores.Clear();
                    XmlInvoiceLog.Clear();
                }
            }
            #endregion

            #region recorridoNodoSaldos
                string TemplateErrorProduct = FileHelper.GetText(string.Format("{0}{1}.xml", xmlPath, "TemplateB020Error"));
                string TemplateErrorProductHeader = FileHelper.GetText(string.Format("{0}{1}.xml", xmlPath, "TemplateB020ErrorHeader"));
                string TemplateErrorProductDetail = FileHelper.GetText(string.Format("{0}{1}.xml", xmlPath, "TemplateB020ErrorDetail"));

                XmlDocument xmlSaldo = new XmlDocument();
                xmlSaldo.LoadXml(DocXml);
                XmlNodeList saldos = xmlSaldo.GetElementsByTagName("SALDO");
                XmlNodeList product = ((XmlElement)saldos[0]).GetElementsByTagName("item");

                //StringBuilder XmlErrorDetail = new StringBuilder();
                XMLProductSearchData material = new XMLProductSearchData();
                Errores hijo = new Errores();
                material.Errores = new List<Errores>();
                //string XmlReturn = "";
                StringBuilder XmlProduct = new StringBuilder();
                StringBuilder XmlProductDetail = new StringBuilder();                

                foreach (XmlElement nodo in product)
                {
                    int i = 0;
                    XmlNodeList nSKU = nodo.GetElementsByTagName("SKU");
                    XmlNodeList nSaldo = nodo.GetElementsByTagName("SALDO");
                    XmlNodeList nCentro = nodo.GetElementsByTagName("CENTRO");
                    if (XmlProductMaterialBusinessLogic.Instance.ExistMaterialBySKU(nSKU[i].InnerText) > 0)
                    {
                        int WareHouseID = XmlProductMaterialBusinessLogic.Instance.ExistWareHouseByExternalCode(nCentro[i].InnerText);
                        if (WareHouseID > 0)
                        {
                            if (XmlProductMaterialBusinessLogic.Instance.ExisWareHouseMaterialBySKU_ExternalCode(nSKU[i].InnerText, nCentro[i].InnerText) > 0)
                            {
                                //En caso exista, las validaciones seran en el Store Procedure
                                //XmlProductMaterialBusinessLogic.Instance.InsertWareHouseMaterial(WareHouseID, Convert.ToInt32(nSKU[i].InnerText), Convert.ToDecimal(nSaldo[i].InnerText));
                                //XmlProductMaterialBusinessLogic.Instance.UpdateInsertWareHouseMaterial(nSKU[i].InnerText, nCentro[i].InnerText, Convert.ToDecimal(nSaldo[i].InnerText.Replace('.', ',')));
                                XmlProductMaterialBusinessLogic.Instance.UpdateInsertWareHouseMaterial(nSKU[i].InnerText, nCentro[i].InnerText, nSaldo[i].InnerText);
                                isValorOk = true;
                            }
                            else
                            {
                                hijo.CampoError = "No existe WarehouseMaterials";
                                hijo.DescError = "WareHouseMaterials";
                                isValorOk = false;
                            }
                        }
                        else
                        {
                            hijo.CampoError = "Centro de distribución no existe";
                            hijo.DescError = "WareHouses";
                            isValorOk = false;
                        }
                    }
                    else
                    {
                        hijo.CampoError = "Producto no existe";
                        hijo.DescError = "Products";
                        isValorOk = false;
                    }
                    if (!isValorOk)
                    {
                        material.Errores.Add(hijo);
                        foreach (var item in material.Errores)
                        {
                            XmlProductDetail.AppendFormat(TemplateErrorProductDetail, item.DescError, item.CampoError);
                        }
                        XmlReturn += string.Format(TemplateErrorProductHeader, nSKU[i].InnerText, XmlProductDetail);
                        material.Errores.Clear();
                        XmlProductDetail.Clear();
                    }
                    
                }
            #endregion

            if (!isValorOk)
            {
                XmlReturn.Replace("<Errores>", "");
                XmlReturn.Replace("</Errores>", "");
                XmlDoc = string.Format(TemplateErrorInvoice, XmlReturn);
                FileHelper.Save(XmlDoc, filePathHist);
                return XmlDoc;
            }
            
            return "";
        }

        #endregion

        /// <summary>
        /// Método que se encarga de validar los productos de error Create - FHP
        /// </summary>
        /// <param name="xmlString">XML con los productos establecidos</param>
        /// <returns>Un XML de error en caso fuera ese el caso</returns>
        /// 

        #region B020
        public string GenerateXmlB020(string xmlString) 
        {
            string xmlPath = ConfigurationManager.AppSettings["TemplatesXML_Path"];
            string fileNameHist = "MLM_SALDOSE_B020_LOG_{0}.xml";
            string filePathHist = ConfigurationManager.AppSettings["FileUploadAbsolutePath"] + ConfigurationManager.AppSettings["FileUploadPath_B020_Log"] + fileNameHist; 
            string TemplateErrorProduct = FileHelper.GetText(string.Format("{0}{1}.xml", xmlPath, "TemplateB020Error"));
            string TemplateErrorProductHeader = FileHelper.GetText(string.Format("{0}{1}.xml", xmlPath, "TemplateB020ErrorHeader"));
            string TemplateErrorProductDetail = FileHelper.GetText(string.Format("{0}{1}.xml", xmlPath, "TemplateB020ErrorDetail"));

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(xmlString);

            XmlNodeList saldos = xml.GetElementsByTagName("ET_SALDO");
            XmlNodeList product = ((XmlElement)saldos[0]).GetElementsByTagName("item");

            StringBuilder XmlErrorDetail = new StringBuilder();
            XMLProductSearchData material = new XMLProductSearchData();
            Errores hijo = new Errores();
            material.Errores = new List<Errores>();
            string XmlReturn = "";
            StringBuilder XmlProduct = new StringBuilder();
            StringBuilder XmlProductDetail = new StringBuilder();

           
            int j = 0;
            foreach (XmlElement nodo in product)
            {
                XmlNodeList nSKU = nodo.GetElementsByTagName("MATERIAL");
                XmlNodeList nSaldo = nodo.GetElementsByTagName("QTDE");
                XmlNodeList nCentro = nodo.GetElementsByTagName("CENTRO");
                int i = 0; 
                int insert = 0;
                if (XmlProductMaterialBusinessLogic.Instance.ExistMaterialBySKU(nSKU[i].InnerText) > 0)
                { 
                    //int WareHouseID = XmlProductMaterialBusinessLogic.Instance.ExistWareHouseByExternalCode(nSaldo[i].InnerText);
                    int WareHouseID = XmlProductMaterialBusinessLogic.Instance.ExistWareHouseByExternalCode(nCentro[i].InnerText);
                    if (WareHouseID > 0)
                    {
                        //if (XmlProductMaterialBusinessLogic.Instance.ExisWareHouseMaterialByWareHouseMaterial(Convert.ToInt32(nSKU[i].InnerText), WareHouseID) > 0)
                        if (XmlProductMaterialBusinessLogic.Instance.ExisWareHouseMaterialBySKU_ExternalCode(nSKU[i].InnerText, nCentro[i].InnerText) > 0)
                        {
                            //En caso exista, las validaciones seran en el Store Procedure
                            //XmlProductMaterialBusinessLogic.Instance.InsertWareHouseMaterial(WareHouseID, Convert.ToInt32(nSKU[i].InnerText), Convert.ToInt32(nSaldo[i].InnerText));
                            //XmlProductMaterialBusinessLogic.Instance.UpdateInsertWareHouseMaterial(nSKU[i].InnerText, nCentro[i].InnerText, Convert.ToDecimal(nSaldo[i].InnerText.Replace('.', ',')));
                            XmlProductMaterialBusinessLogic.Instance.UpdateInsertWareHouseMaterial(nSKU[i].InnerText, nCentro[i].InnerText, nSaldo[i].InnerText);
                            insert = 1;
                        }
                        else
                        {
                            hijo.CampoError = "No existe WarehouseMaterials";
                            hijo.DescError = "WareHouseMaterials";
                            j = j + 1;

                        }
                    }
                    else
                    {
                        hijo.CampoError = "Centro de distribución no existe";
                        hijo.DescError = "WareHouses";
                        j = j + 1;
                    }
                }
                else
                {
                    hijo.CampoError = "Producto no existe";
                    hijo.DescError = "Products";
                    j = j + 1;
                }
                material.Errores.Add(hijo);
                foreach (var item in material.Errores)
                {
                    XmlProductDetail.AppendFormat(TemplateErrorProductDetail, item.DescError, item.CampoError);
                }
                if (insert == 0)
                {
                    XmlReturn += string.Format(TemplateErrorProductHeader, nSKU[i].InnerText, XmlProductDetail);
                }
                material.Errores.Clear();
                XmlProductDetail.Clear();
                
            }
            if (j == 0)
            {
                return "";
            }
            XmlReturn.Replace("<Errores>", "");
            XmlReturn.Replace("</Errores>", "");
            string XmlErrorDetails = string.Format(TemplateErrorProduct, XmlReturn);
            FileHelper.Save(XmlErrorDetails, filePathHist);
            return XmlErrorDetails;
        }

        #endregion

        #region B160
        public string ConfirmacionPagosB160(string DocXml)
        {
            string xmlPath = ConfigurationManager.AppSettings["TemplatesXML_Path"];
            string timeStamp = GetTimeStamp();
            string fileNameHist = "MLM_PAGOSE_B160_LOG_" + timeStamp + ".xml";
            string filePathHist = ConfigurationManager.AppSettings["FileUploadAbsolutePath"] + ConfigurationManager.AppSettings["FileUploadPath_B160_Log"] + fileNameHist;
            string TemplateErrorItemComissao = FileHelper.GetText(string.Format("{0}{1}.xml", xmlPath, "TemplateB160Error"));
            //string TemplateErrorItemComissaoHeader = FileHelper.GetText(string.Format("{0}{1}.xml", xmlPath, "TemplateB160LogHeader"));
            string TemplateErrorItemComissaoDetail = FileHelper.GetText(string.Format("{0}{1}.xml", xmlPath, "TemplateB160LogDetail"));

            string XmlReturn = "";
            string XmlDoc = "";
            string ValorOk = "0";
            int ValorRetornoSP;

            XDocument xml = XDocument.Parse(DocXml);

            StringBuilder XmlErrorDetail = new StringBuilder();
            List<ConfirmacionPagos> ConfirmacionPagosXML = new List<ConfirmacionPagos>();
            XMLProductSearchData ItemComissaoError = new XMLProductSearchData();
            ErroresConfirmacionPago error = new ErroresConfirmacionPago();
            ItemComissaoError.ErroresCP = new List<ErroresConfirmacionPago>();

            StringBuilder XmlItemComissaoLog = new StringBuilder();
            StringBuilder XmlItemComissaoDetail = new StringBuilder();

            #region recorridonodoItemComissao
            foreach (XElement element in xml.Descendants("ItemComissaoRetorno").Where(x => x.Element("CodigoPessoa") != null))
            {
                XElement child = element.Descendants("Imposto").FirstOrDefault();
                XElement child2 = element.Descendants("Imposto").LastOrDefault();

                var ItemComissaoRet = new ConfirmacionPagos
                {
                    DocId = element.Attribute("docId").Value.ToString(),
                    CodigoPessoa = element.Element("CodigoPessoa").Value.ToString(),
                    CodigoOrdenPagamento = element.Element("CodigoOrdemPagamento").Value.ToString(),
                    ValorTotalPago = element.Element("ValorTotalPago").Value.ToString(),

                    CodigoImposto = child.Element("CodigoImposto").Value.ToString(),
                    PercDesconto = child.Element("PercDesconto").Value.ToString(),
                    ValorTotalDescontado = child.Element("ValorTotalDescontado").Value.ToString(),

                    CodigoImposto2 = child2.Element("CodigoImposto").Value.ToString(),
                    PercDesconto2 = child2.Element("PercDesconto").Value.ToString(),
                    ValorTotalDescontado2 = child2.Element("ValorTotalDescontado").Value.ToString(),
                };

                ValorRetornoSP = XmlProductMaterialBusinessLogic.Instance.ValidarConfirmacionPagos(ItemComissaoRet.CodigoOrdenPagamento, ItemComissaoRet.ValorTotalDescontado, ItemComissaoRet.ValorTotalDescontado2);

                if (ValorRetornoSP == 1)
                {
                    error.CodigoOrdemPagamento = ItemComissaoRet.CodigoOrdenPagamento.ToString();
                    error.CampoError = "CodigoOrdemPagamento";
                    error.DescError = "No existen datos en la Base para el valor enviado";
                }
                if (ValorRetornoSP == 2)
                {
                    error.CodigoOrdemPagamento = ItemComissaoRet.CodigoOrdenPagamento.ToString();
                    error.CampoError = "Tax1";
                    error.DescError = "Error en el valor de impuestos. Se encuentra vacío o nulo.";
                }
                if (ValorRetornoSP == 3)
                {
                    error.CodigoOrdemPagamento = ItemComissaoRet.CodigoOrdenPagamento.ToString();
                    error.CampoError = "Tax2";
                    error.DescError = "Error en el valor de impuestos. Se encuentra vacío o nulo.";
                }

                if (ValorRetornoSP == 1 || ValorRetornoSP == 2 || ValorRetornoSP == 3)
                {
                    ItemComissaoError.ErroresCP.Add(error);
                    foreach (var item in ItemComissaoError.ErroresCP)
                    {
                        XmlItemComissaoLog.AppendFormat(TemplateErrorItemComissaoDetail, item.CodigoOrdemPagamento, item.CampoError, item.DescError);
                    }
                    //XmlReturn += string.Format(TemplateErrorItemComissaoDetail, XmlItemComissaoLog);
                    XmlReturn += XmlItemComissaoLog.ToString();
                    ItemComissaoError.ErroresCP.Clear();
                    XmlItemComissaoLog.Clear();
                }
                if (ValorRetornoSP == 0 || ValorRetornoSP == 4)
                {
                    ValorOk = "1";
                }
            }
            #endregion

            if (ValorOk == "0")
            {
                //XmlReturn += string.Format(TemplateErrorItemComissao, XmlItemComissaoLog);
                //XmlReturn.Replace("<Errores>", "");
                //XmlReturn.Replace("</Errores>", "");
                XmlDoc = string.Format(TemplateErrorItemComissao, XmlReturn);
                FileHelper.Save(XmlDoc, filePathHist);
                return XmlDoc;
            }
            else
                return "";

        }

        #endregion

        #region B090
        public string MLM_CtaCteE_B090(string fechaMov)
        {
            string xmlPath = ConfigurationManager.AppSettings["TemplatesXML_Path"];
            string timeStamp = GetTimeStamp();
            string fileNameHist = "MLM_GENERATE_B090_LOG_" + timeStamp + ".xml";
            string filePathHist = ConfigurationManager.AppSettings["FileUploadAbsolutePath"] + ConfigurationManager.AppSettings["FileUploadPath_B090_Log"] + fileNameHist;
            string flagArchivo = ConfigurationManager.AppSettings["FlagArchivo"];
            string XmlB090 = "";

            //XmlB090 = XmlProductMaterialBusinessLogic.Instance.ReturnXMLB090(fechaMov);

            if (flagArchivo == "1") //Se crea el archivo XML
            {
                XmlB090 = XmlProductMaterialBusinessLogic.Instance.ReturnXMLB090(fechaMov);
                FileHelper.Save(XmlB090, filePathHist);
                XmlB090 = "";
            }
            else
                XmlB090 = XmlProductMaterialBusinessLogic.Instance.ReturnXMLB090(fechaMov);
            
            return XmlB090;
        }
        #endregion

        #region E080
        public string MLM_E080_WSAutenticacion(string DocXml)
        {
            string xmlPath = ConfigurationManager.AppSettings["TemplatesXML_Path"];
            string timeStamp = GetTimeStamp();
            string fileNameHist = "MLM_E080_WSAutenticacion_LOG_" + timeStamp + ".xml";
            string filePathHist = ConfigurationManager.AppSettings["FileUploadAbsolutePath"] + ConfigurationManager.AppSettings["FileUploadPath_E080_Log"] + fileNameHist;
            string fileNamePDF = ConfigurationManager.AppSettings["FileUploadAbsolutePath"] + ConfigurationManager.AppSettings["FileUploadPath_E080_Log"];
            string XmlE080 = "";

            string valLogin = "";
            string valPassword = "";

            XDocument xml = XDocument.Parse(DocXml);

            StringBuilder XmlInvoiceLog = new StringBuilder();
            StringBuilder XmlInvoiceDetail = new StringBuilder();

            foreach (XElement element in xml.Descendants("item").Where(x => x.Element("LOGIN") != null))
            {
                XElement child = element.Descendants("item").FirstOrDefault();

                var dataUser = new E080XmlDto
                {
                    Login = element.Element("LOGIN").Value.ToString(),
                    Password = element.Element("SENHA").Value.ToString(),

                };
                valLogin = dataUser.Login;
                valPassword = dataUser.Password;
            }

            XmlE080 = XmlProductMaterialBusinessLogic.Instance.ReturnXMLE080WSAutenticacion(valLogin, valPassword);
            FileHelper.Save(XmlE080, filePathHist);
            return XmlE080;
        }

        public string MLM_E080_WSDatosConsultores(string DocXml)
        {
            string xmlPath = ConfigurationManager.AppSettings["TemplatesXML_Path"];
            string timeStamp = GetTimeStamp();
            string fileNameHist = "MLM_E080_WSAutenticacion_LOG_" + timeStamp + ".xml";
            string filePathHist = ConfigurationManager.AppSettings["FileUploadAbsolutePath"] + ConfigurationManager.AppSettings["FileUploadPath_E080_Log"] + fileNameHist;
            string fileNamePDF = ConfigurationManager.AppSettings["FileUploadAbsolutePath"] + ConfigurationManager.AppSettings["FileUploadPath_E080_Log"];
            string XmlE080 = "";

            string valLogin = "";
            string valToken = "";
            string valCodConsultor = "";

            XDocument xml = XDocument.Parse(DocXml);

            StringBuilder XmlInvoiceLog = new StringBuilder();
            StringBuilder XmlInvoiceDetail = new StringBuilder();

            foreach (XElement element in xml.Descendants("item").Where(x => x.Element("LOGIN") != null))
            {
                XElement child = element.Descendants("item").FirstOrDefault();

                var dataUser = new E080XmlDtoConsultores
                {
                    Token = element.Element("CHAVEACESSO").Value.ToString(),
                    UserName = element.Element("LOGIN").Value.ToString(),
                    CodConsultor = element.Element("CODIGO").Value.ToString()

                };
                valToken = dataUser.Token;
                valLogin = dataUser.UserName;
                valCodConsultor = dataUser.CodConsultor;
            }

            XmlE080 = XmlProductMaterialBusinessLogic.Instance.ReturnXMLE080WSDatosConsultores(valToken, valLogin, valCodConsultor);
            FileHelper.Save(XmlE080, filePathHist);
            return XmlE080;
        }
        #endregion

        /* @01 A01*/
        #region [E020]

        /// <summary>
        /// Realiza proceso de Confirmación de Despacho GKO
        /// </summary>
        public string ConfirmacionDespachoGKO(string stringXML)
        {
            List<int> ListOrderID = new List<int>();

            string fileNameHist = "MLM_CDespacho_E0020_{0}.xml";
            string filePathHist = ConfigurationManager.AppSettings["FileUploadAbsolutePath"] + ConfigurationManager.AppSettings["FileUploadPath_E020_Hist"] + fileNameHist;

            string fileNameLog = "MLM_CDespacho_E0020_LOG_{0}.xml";
            string filePathLog = ConfigurationManager.AppSettings["FileUploadAbsolutePath"] + ConfigurationManager.AppSettings["FileUploadPath_E020_Log"] + fileNameLog;

            string templatesPath = ConfigurationManager.AppSettings["TemplatesXML_Path"];

            //Header Template
            string TemplateHeader = GetText(templatesPath + "TemplateE020LogHeader.xml"); //Header Template
            //Detail Template
            string TemplateDetail = GetText(templatesPath + "TemplateE020LogDetail.xml");
            //Detail Content Template
            string TemplateDetailContent = GetText(templatesPath + "TemplateE020LogDetailContent.xml");

            //Final Result
            string stringXMLResult = TemplateHeader;

            //Reading XML IN in order to generate Detail
            string stringXMLDetail = string.Empty;

            XDocument objectXML = XDocument.Parse(stringXML);

            foreach (XElement element in objectXML.Descendants("NotaFiscal"))
            {
                string stringXMLDetailNode = string.Empty;

                int NumeroNotaFiscal = Convert.ToInt32(element.Element("NumeroNotaFiscal").Value);
                int NumeroSerie = Convert.ToInt32(element.Element("NumeroSerie").Value);

                string ErrorDescription1 = string.Empty;
                string ErrorDescription2 = string.Empty;

                //T1: Validación de relación 
                //T2: Validación de Order
                //T3: OrderID
                Tuple<bool, bool, int, int> validation = repository.ValidarNotaFiscal(NumeroNotaFiscal, NumeroSerie);

                int OrderID = validation.Item3;

                if (validation.Item1)
                {
                    if (validation.Item2)
                    {
                        ListOrderID.Add(OrderID);
                    }
                    else
                    {
                        stringXMLDetailNode += string.Format(TemplateDetailContent, OrderID, E0020ErrorMessages.Order);
                    }
                }
                else
                {
                    stringXMLDetailNode += string.Format(TemplateDetailContent, NumeroNotaFiscal, E0020ErrorMessages.NotaSerie);
                    stringXMLDetailNode += string.Format(TemplateDetailContent, NumeroSerie, E0020ErrorMessages.NotaSerie);
                }

                if (!string.IsNullOrEmpty(stringXMLDetailNode))
                    stringXMLDetail += string.Format(TemplateDetail, stringXMLDetailNode);
            }

            //Inserts Detail into Header
            stringXMLResult = string.Format(stringXMLResult, stringXMLDetail);

            //Updates Orders that had no errors
            if (ListOrderID.Count() > 0)
            {
                repository.UpdateOrderStatusToShipped(ListOrderID);
            }

            //Saves XML File   
            string timeStamp = GetTimeStamp();

            filePathHist = string.Format(filePathHist, timeStamp);
            FileHelper.Save(stringXML, filePathHist);

            filePathLog = string.Format(filePathLog, timeStamp);
            FileHelper.Save(stringXMLResult, filePathLog);

            return stringXMLResult;
        }

        #endregion
        /* @01 A01*/

        #region [E030]

        //E030 - LIB
        public string ConfirmacionOcurrenciasGKO(string stringXML)
        {
            string fileNameHist = "MLM_COcurrencia_E030_{0}.xml";
            string filePathHist = ConfigurationManager.AppSettings["FileUploadAbsolutePath"] + ConfigurationManager.AppSettings["FileUploadPath_E030_Hist"] + fileNameHist;

            string fileNameLog = "MLM_COcurrencia_E030_LOG_{0}.xml";
            string filePathLog = ConfigurationManager.AppSettings["FileUploadAbsolutePath"] + ConfigurationManager.AppSettings["FileUploadPath_E030_Log"] + fileNameLog;

            string templatesPath = ConfigurationManager.AppSettings["TemplatesXML_Path"];

            string stringXMLresult = GetText(templatesPath + "TemplateE030Log.xml");
            string stringXMLErrorDetailTemplate = GetText(templatesPath + "TemplateE030LogDetail.xml");

            string stringXMLDetail = string.Empty;

            XDocument objectXML = XDocument.Parse(stringXML);

            foreach (XElement element in objectXML.Descendants("ResultadoEntrega"))
            {
                string stringXMLDetailNode = stringXMLErrorDetailTemplate;

                int NumeroNotaFiscal = Convert.ToInt32(element.Element("NumeroNotaFiscal").Value);
                int NumeroSerie = Convert.ToInt32(element.Element("NumeroSerie").Value);
                string cnpjValue = Convert.ToString(element.Element("CnpjEmissor").Value);
                int situacao = Convert.ToInt32(element.Element("Situacao").Value);
                string observacao = Convert.ToString(element.Element("Observacao").Value);
                string dataOcorrencia = Convert.ToString(element.Element("DataOcorrencia").Value);

                string CampoError = string.Empty;
                string ErrorDescription = string.Empty;


                Tuple<bool, bool, int, int> validation = repository.ValidarNotaFiscal(NumeroNotaFiscal, NumeroSerie);

                int orderCustomerID = validation.Item4;

                if (validation.Item1)
                {
                    if (validation.Item2)
                    {
                        bool validateCnpj = repository.ValidarCnpj(cnpjValue);

                        if (validateCnpj)
                        {

                            repository.InsertOrderShipmentTracking(orderCustomerID, situacao, observacao, dataOcorrencia);

                        }
                        else
                        {
                            CampoError = "CnpjEmissor";
                            ErrorDescription = "CnpjEmissor no existe";
                        }
                    }
                    else
                    {
                        ErrorDescription = "OrderCustomer Id no Existe ";
                    }
                }
                else
                {
                    CampoError = "NumeroSerie";
                    ErrorDescription = "No existe la relación Nota-Serie";
                }

                stringXMLDetailNode = string.Format(stringXMLErrorDetailTemplate, orderCustomerID, NumeroNotaFiscal, CampoError, ErrorDescription);

                stringXMLDetail += stringXMLDetailNode;
            }

            stringXMLresult = string.Format(stringXMLresult, stringXMLDetail);

            string timeStamp = GetTimeStamp();

            filePathHist = string.Format(filePathHist, timeStamp);
            FileHelper.Save(stringXML, filePathHist);

            filePathLog = string.Format(filePathLog, timeStamp);
            FileHelper.Save(stringXMLresult, filePathLog);

            return stringXMLresult;
        }

        #endregion

        #endregion

        #region Singleton

        /// <summary>
        /// Obtiene una instancia singleton de la clase AccountPerformanceDataBusinessLogic
        /// </summary>
        public static XmlGeneratorBusinessLogic Instance
        {
            get 
            {
                if (instance == null)
                {
                    instance = new XmlGeneratorBusinessLogic();
                    //IOC
                    repository = new NetSteps.Data.Entities.Repositories.XmlGeneratorRepository();
                    repositoryWareHouseMaterialBussiness = new NetSteps.Data.Entities.Repositories.WareHouseMaterialsRepository();
                }

                return instance;
            }
        }

	#endregion
        
        #region privates

        public OrderHeaderXml DtoToBO(NetSteps.Data.Entities.Dto.OrderHeaderXmlDto dto)
        {
            return new OrderHeaderXml()
            {
                TipoOrdem = dto.TipoOrdem,
                EmisordaOrdem = dto.EmisordaOrdem,
                SourceAddressID = dto.SourceAddressID,
                Trasportador = dto.Trasportador,
                NumeroPedido = dto.NumeroPedido,
                DataPedido = dto.DataPedido,
                Incoterm = dto.Incoterm,
                Frete = dto.Frete,
                RecebedorMercaderia = dto.RecebedorMercaderia,
                LoteTransporte = dto.LoteTransporte
            };
        } 

        /// <summary>
        /// Mapper
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public OrderHeaderXml DtoToBO02(NetSteps.Data.Entities.Dto.OrderHeaderXmlDto dto)
        //public static OcCabeceraXml ToBO(this NetSteps.Data.Entities.Dto.OcCabeceraXmlDto dto)
        {
            return new OrderHeaderXml()
            {
                  TipoOrdem = dto.TipoOrdem,
		          EmisordaOrdem = dto.EmisordaOrdem,
		          SourceAddressID  = dto.SourceAddressID,
		          Trasportador  = dto.Trasportador,
		          NumeroPedido  = dto.NumeroPedido,
                  DataPedido = dto.DataPedido,
		          Incoterm  = dto.Incoterm,
                  Frete  = dto.Frete
            };
        } 

        /// <summary>
        /// Convierte un objeto Bo a Dto
        /// </summary>
        /// <param name="bo">Account Performance Data</param>
        /// <returns>Account Performance Dto</returns>
        public NetSteps.Data.Entities.Dto.OrderHeaderXmlDto BOToDto(OrderHeaderXml bo)
        //public NetSteps.Data.Entities.Dto.OcCabeceraXmlDto ToDto(this OcCabeceraXml bo)
        {
            return new NetSteps.Data.Entities.Dto.OrderHeaderXmlDto()
            {
                TipoOrdem = bo.TipoOrdem,
		        EmisordaOrdem = bo.EmisordaOrdem,
		        SourceAddressID  = bo.SourceAddressID,
		        Trasportador  = bo.Trasportador,
		        NumeroPedido  = bo.NumeroPedido,
                DataPedido = bo.DataPedido,
		        Incoterm  = bo.Incoterm,
                Frete  = bo.Frete
            };
        }

        public OrderDetailXml DtoToBO(NetSteps.Data.Entities.Dto.OrderDetailXmlDto dto)
        {
            return new OrderDetailXml()
            {
                Linea = dto.Linea,
                CategoriaItem = dto.CategoriaItem,
                Material = dto.Material,
                Quantidade = dto.Quantidade,
                CentroDistribucao = dto.CentroDistribucao,
                PresoPraticado = dto.PresoPraticado,
                Desconto = dto.Desconto
            };
        }

        public NetSteps.Data.Entities.Dto.OrderDetailXmlDto BOToDto(OrderDetailXml bo)
        {
            return new NetSteps.Data.Entities.Dto.OrderDetailXmlDto()
            {
                Linea = bo.Linea,
                CategoriaItem = bo.CategoriaItem,
                Material = bo.Material,
                Quantidade = bo.Quantidade,
                CentroDistribucao = bo.CentroDistribucao,
                PresoPraticado = bo.PresoPraticado,
                Desconto = bo.Desconto
            };
        }

        public AdvancePaymentXml DtoToBO(NetSteps.Data.Entities.Dto.AdvancePaymentXmlDto dto)
        {
            return new AdvancePaymentXml()
            {
                ValorFatura  = dto.ValorFatura,
	            CreditoPedidoAnterior  = dto.CreditoPedidoAnterior,
	            DebitoPedidoAnterior = dto.DebitoPedidoAnterior,
	            PrimeiraParcelaBoleto = dto.PrimeiraParcelaBoleto,
	            RecebidoBoleto = dto.RecebidoBoleto,
	            DataRecebimentoBoleto = dto.DataRecebimentoBoleto,
	            BancoRecebedorBoleto = dto.BancoRecebedorBoleto,
	            ValorCobradoCartaoCred = dto.ValorCobradoCartaoCred,
	            OperadoraCartaoCred  = dto.OperadoraCartaoCred,
                ValorAdiantamento  = dto.ValorAdiantamento,
                NumeroTitulo = dto.NumeroTitulo
            };
        }

        public NetSteps.Data.Entities.Dto.AdvancePaymentXmlDto BOToDto(AdvancePaymentXml bo)
        {
            return new NetSteps.Data.Entities.Dto.AdvancePaymentXmlDto()
            {
                ValorFatura = bo.ValorFatura,
                CreditoPedidoAnterior = bo.CreditoPedidoAnterior,
                DebitoPedidoAnterior = bo.DebitoPedidoAnterior,
                PrimeiraParcelaBoleto = bo.PrimeiraParcelaBoleto,
                RecebidoBoleto = bo.RecebidoBoleto,
                DataRecebimentoBoleto = bo.DataRecebimentoBoleto,
                BancoRecebedorBoleto = bo.BancoRecebedorBoleto,
                ValorCobradoCartaoCred = bo.ValorCobradoCartaoCred,
                OperadoraCartaoCred = bo.OperadoraCartaoCred,
                ValorAdiantamento = bo.ValorAdiantamento
            };
        }

        public MaterialXmlDto BOToDto(MaterialXml bo)
        {
            return new NetSteps.Data.Entities.Dto.MaterialXmlDto()
            {
                SKU = bo.SKU,
                BPCS = bo.BPCS,
                Brand = bo.Brand,
                Group = bo.Group,
                NamePort = bo.NamePort,
                NameEsp = bo.NameEsp,
                Volume = bo.Volume,
                Weight = bo.Weight,
                Hierachy = bo.Hierachy
            };
        }

        public MaterialCentersXmlDto BOToDto(MaterialCentersXml bo)
        {
            return new NetSteps.Data.Entities.Dto.MaterialCentersXmlDto()
            {
                SKU = bo.SKU,
                Centro = bo.Centro,
                Costo = bo.Costo
            };
        }

        public List<MaterialLogXml> DtoToBO(List<MaterialLogXmlDto> dto)
        {
            List<MaterialLogXml> lstRet = new List<MaterialLogXml>();
            foreach (var item in dto)
            {
                var dat = new MaterialLogXml()
                {
                    CampoError = item.CampoError,
                    DescError = item.DescError
                };
                lstRet.Add(dat);
            }
            return lstRet;
        }

        private static MaterialLogXmlDto ValidateDecimal(string val, string campo)
        {
            MaterialLogXmlDto ret = new MaterialLogXmlDto();
            try
            {

                Convert.ToDecimal(val);
                return ret;
            }
            catch
            {
                ret = new MaterialLogXmlDto()
                {
                    CampoError = campo,
                    DescError = "Dato no valido"
                };
                return ret;
            }


        }

        public static string GetText(string pathTemplate)
        {
            string resul = "";
            using (StreamReader reader = new StreamReader(pathTemplate))
            {
                resul = reader.ReadToEnd();
            }
            return resul;
        }

        /* @01 A02*/

        private string GetTimeStamp()
        {
            DateTime currentDate = DateTime.Now;
            return currentDate.Year.ToString() +
                   currentDate.Month.ToString().PadLeft(2, '0') +
                   currentDate.Day.ToString().PadLeft(2, '0') +
                   currentDate.Hour.ToString().PadLeft(2, '0') +
                   currentDate.Minute.ToString().PadLeft(2, '0');
        }

        private string GetTimeStampB150()
        {
            DateTime currentDate = DateTime.Now;
            return "_"+currentDate.Year.ToString() +
                   currentDate.Month.ToString().PadLeft(2, '0') +
                   currentDate.Day.ToString().PadLeft(2, '0') +
                   "_"+
                   currentDate.Hour.ToString().PadLeft(2, '0') +
                   currentDate.Minute.ToString().PadLeft(2, '0')+
                   currentDate.Second.ToString().PadLeft(2, '0');
        }

        /* @01 A02*/

        /// <summary>
        /// Previene una instancia por defecto de la clase AccountPerformanceDataBusinessLogic
        /// </summary>
        private XmlGeneratorBusinessLogic()
        { }
              
        /// <summary>
        /// Obtiene o establece una instancia de AccountPerformanceDataBusinessLogic
        /// </summary>
        private static XmlGeneratorBusinessLogic instance;

        /// <summary>
        /// Obtiene o establece una implementacion de IAccountPerformanceDataRepository
        /// </summary>
        private static IXmlGeneratorRepository repository;

        private static IWareHouseMaterialsRepository repositoryWareHouseMaterialBussiness;
        #endregion

        /* @01 A03*/
        #region [Constants]

        #region [E020]

        private static class E0020ErrorMessages
        {
            public const string NotaSerie = "Relación Nota-Serie no existe.";
            public const string Order = "Order ID no existe.";
        }

        #endregion

        #endregion
        /* @01 A03*/

        #region Req BR010 Pedidos Encore - SAP 3PL  Dev. SvG G&S

        public int InsertB010Log(int OrderID, string Estructura, string Mensaje)
        {
            var result = DataAccess.ExecWithStoreProcedureSaveIdentity("Core", "uspInsertB010Log",
                new SqlParameter("OrderID", SqlDbType.VarChar) { Value = OrderID },
                new SqlParameter("Estructura", SqlDbType.VarChar) { Value = Estructura },
                new SqlParameter("Mensaje", SqlDbType.VarChar) { Value = Mensaje }
                );
            return result;
        }


        public  string[,] CrearOrderPedidoXml(string PathMainTemplateClientsOrders ,string pathTemplateClientsOrders, string PathTemplateOrderItem, int LoteID, int OrderInvoiceIDIniPOut,    int  OrderInvoiceIDFinPOut, CultureInfo  CurrentCultureInfo )
        {
            CurrentCultureInfo = CultureInfoCache.GetCultureInfo("en-US");

            //try
            //{
                NumberFormatInfo nfi = new NumberFormatInfo();
                nfi.NumberGroupSeparator = ".";

                //Func<DateTime, string> fechaText = (dtFecha) => {
                //    int dia = dtFecha.Day;
                //    int mes = dtFecha.Month;
                //    int year = dtFecha.Year;
                //    return string.Format("{0}-{1}-{2}",year,mes,dia);
                //};
                string formato = "00.##";

                Func<DateTime, string> fechaText = (dtFecha) =>
                {
                    string dia = dtFecha.Day.ToString(formato);
                    string mes = dtFecha.Month.ToString(formato);
                    string year = dtFecha.Year.ToString(formato);
                    return string.Format("{0}-{1}-{2}", year, mes, dia);
                };

                string ClientsOrdersTemplate = GetText(pathTemplateClientsOrders);
                string OrderItemsXmlDtotemplate = GetText(PathTemplateOrderItem);
                string plantillaPrincipal = GetText(PathMainTemplateClientsOrders);
                #region funciones cargar plantilla
                Func<ClienteOrder, string, string, string> FuncCrearClienteOrderXml = (objCliente, strOrder, strDetaillOrderItem) =>
                {

                    return string.Format(strOrder,
                                                "",/*objCliente.objPedidoXml.NumeroPedido*/
                                                objCliente.objPedidoXml.NumeroPedido,
                                                objCliente.objClientXml.ClienteID.ToString(),
                                                objCliente.objClientXml.ENovo,
                                                objCliente.objClientXml.Nome,
                                                objCliente.objClientXml.Sexo,
                                                objCliente.objClientXml.Rua,
                                                objCliente.objClientXml.NumeroRua,
                                                objCliente.objClientXml.Barrio,
                                                objCliente.objClientXml.CEP,
                                                objCliente.objClientXml.Cidade,
                                                objCliente.objClientXml.Regiao,
                                                objCliente.objClientXml.Email,
                                                objCliente.objClientXml.CPF,
                                                objCliente.objClientXml.SetorIndustrial,
                                                objCliente.objClientXml.Recebedor,
                                                objCliente.objClientXml.RuaRecebedor,
                                                objCliente.objClientXml.NumeroRuaRecebedor,
                                                objCliente.objClientXml.BairroRecebedor,
                                                objCliente.objClientXml.CEPRecebedor,
                                                objCliente.objClientXml.CidadeRecebedor,
                                                
                                                objCliente.objClientXml.RegiaoRecebedor,
                                                objCliente.objClientXml.EmailRecebedor,
                                                objCliente.objClientXml.REFERENCIALOCL,
                                                



                                                objCliente.objPedidoXml.TipoOrdem.ToString(),
                                                objCliente.objPedidoXml.EmisordaOrdem,
                                                objCliente.objPedidoXml.RecebedorMercaderia,
                                                objCliente.objPedidoXml.Trasportador,
                                                objCliente.objPedidoXml.LoteTransporte,
                                                "",// objCliente.objPedidoXml.NumeroPedido,
                                                (objCliente.objPedidoXml.DataPedido == default(DateTime) ? "" : fechaText(objCliente.objPedidoXml.DataPedido)),
                                                
                                                objCliente.objPedidoXml.FormaPgto,
                                                objCliente.objPedidoXml.Incoterm,
                                                objCliente.objPedidoXml.Frete.ToString(CurrentCultureInfo),
                                               
                                                strDetaillOrderItem,
                                                objCliente.objAdiantamentoXml.NumeroPedido,
                                                objCliente.objAdiantamentoXml.ValorFatura.ToString(CurrentCultureInfo),
                                                objCliente.objAdiantamentoXml.CreditoPedidoAnterior.ToString(CurrentCultureInfo),
                                                objCliente.objAdiantamentoXml.DebitoPedidoAnterior.ToString(CurrentCultureInfo),
                                                objCliente.objAdiantamentoXml.PrimeiraParcelaBoleto.ToString(CurrentCultureInfo),
                                                objCliente.objAdiantamentoXml.RecebidoBoleto.ToString(CurrentCultureInfo),

                                                (objCliente.objAdiantamentoXml.DataRecebimentoBoleto == default(DateTime) ? "" : fechaText(objCliente.objAdiantamentoXml.DataRecebimentoBoleto)),


                                                objCliente.objAdiantamentoXml.BancoRecebedorBoleto.ToString(CurrentCultureInfo),
                                                objCliente.objAdiantamentoXml.ValorCobradoCartaoCred.ToString(CurrentCultureInfo),
                                                objCliente.objAdiantamentoXml.OperadoraCartaoCred.ToString(),

                                                objCliente.objAdiantamentoXml.NumeroParcelas,
                                                objCliente.objAdiantamentoXml.NumeroTitulo,
                                                objCliente.objPedidoXml.NumeroPedido
                        );

                };
                Func<OrderItemsXml, string, string> FuncCrearOrderItemsXml = (obj, strPlantillaItems) =>
                {
                    return string.Format(strPlantillaItems,
                         obj.Linea.ToString(),
                         obj.CategoriaItem.ToString(),
                         obj.Material.ToString(),
                         obj.Quantidade.ToString(),
                         obj.CentroDistribucao.ToString(),
                         obj.PresoPraticado.ToString(CurrentCultureInfo),
                         obj.Desconto.ToString(CurrentCultureInfo)
                         );

                };
                #endregion
                IEnumerable<ClientXml> lstClientXml = GetClientOrder(LoteID).Select((item) => { return (ClientXml)item; });
                IEnumerable<AdiantamentoXml> lstAdiantamentoXml = GetAdiantamentoOrder(LoteID, CurrentCultureInfo).Select((item) => { return (AdiantamentoXml)item; });
                IEnumerable<PedidoXml> lstPedidoXml = GetPedidoOrder(LoteID, CurrentCultureInfo).Select((item) => { return (PedidoXml)item; });
                IEnumerable<OrderItemsXml> lstOrderItemsXml = GetDetailOrder(LoteID, OrderInvoiceIDIniPOut, OrderInvoiceIDFinPOut, CurrentCultureInfo).Select((item) => { return (OrderItemsXml)item; });

                List<ClienteOrder> lstClienteOrder = new List<ClienteOrder>();
                ClienteOrder objClienteOrder = null;
                string[,] data = new string[1, 2];
                var index = 0;
                string OrderXml="";
                string ListaOrderNumbers = "";
                foreach (ClientXml obClientXml in lstClientXml)
                {
                    objClienteOrder = new ClienteOrder();
                    objClienteOrder.objClientXml = obClientXml;
                    if (!ValidarCliente(obClientXml))
                    {
                        InsertB010Log(Convert.ToInt32(obClientXml.OrderNumber), "Cliente", "Campos en blanco");
                        continue;
                    }

                    var objPedido = lstPedidoXml.FirstOrDefault((obj) => { return obj.OrderCustomerID == obClientXml.OrderCustomerID; });
                    objClienteOrder.objPedidoXml = objPedido;
                    if (!ValidarPedido(objPedido))
                    {
                        InsertB010Log(Convert.ToInt32(obClientXml.OrderNumber), "Pedido Cabecera", "Campos en blanco");
                        continue;
                    }

                    var listaOrderItem = lstOrderItemsXml.Where((obj) => { return obj.OrderCustomerID == obClientXml.OrderCustomerID; });
                    objClienteOrder.lstOrderItemsXml = listaOrderItem;
                    decimal montoDetalleFactura = ValidarOrderItems(listaOrderItem);
                    if (montoDetalleFactura == -1)
                    {
                        InsertB010Log(Convert.ToInt32(obClientXml.OrderNumber), "Pedido Detalle", "Campos en blanco");
                        continue;
                    }
                    
                    var objAdiantamento = lstAdiantamentoXml.FirstOrDefault((obj) => { return obj.OrderCustomerID == obClientXml.OrderCustomerID; }) ?? new AdiantamentoXml();
                    objClienteOrder.objAdiantamentoXml = objAdiantamento;

                    if (!ValidarAdiantamento(objAdiantamento, (montoDetalleFactura + objPedido.Frete)))
                    {
                        InsertB010Log(Convert.ToInt32(obClientXml.OrderNumber), "Adiantamento", "Campos en blanco o cantidad = 0");
                        //break;
                        continue;
                    }
                    

                    string detaillXml = "";
                    foreach (var obj in objClienteOrder.lstOrderItemsXml)
                    {
                        detaillXml += FuncCrearOrderItemsXml(obj, OrderItemsXmlDtotemplate);
                    }
                    OrderXml += FuncCrearClienteOrderXml(objClienteOrder, ClientsOrdersTemplate, detaillXml);
                    ListaOrderNumbers += obClientXml.OrderNumber.ToString() + ";";
                
                    
                }
            
                data[index, 0] = string.Format(plantillaPrincipal, OrderXml);
                data[index, 1] = ListaOrderNumbers;
                index++;
                return data;
            //}
            //catch (Exception ex)
            //{
               

            //    throw ex;
            //}
        }

        #region Validar
        private decimal ValidarOrderItems(IEnumerable<OrderItemsXml> listaOrderItem)
        {
            decimal monto = 0;
            foreach (var item in listaOrderItem)
            {
                if ((item.Linea.ToString().Trim().Length == 0) |
                    (item.CategoriaItem.ToString().Trim().Length == 0) |
                    (item.Material == null) |
                    (item.CentroDistribucao == null) |
                    (item.Quantidade.ToString().Trim().Length == 0 | item.Quantidade == 0) |
                    (item.PresoPraticado.ToString().Trim().Length == 0) |
                    (item.Desconto.ToString().Trim().Length == 0)
            )
                {
                    monto = -1;
                    return monto;
                }
                else
                {
                    if (item.Material.ToString().Trim().Length == 0 | item.CentroDistribucao.ToString().Trim().Length == 0)
                    {
                        monto = -1;
                        return monto;
                    }
                    else
                        monto += item.PresoPraticado * item.Quantidade;
                }
            }
            return monto;
        }

        private bool ValidarAdiantamento(AdiantamentoXml entidadAdiantamento,decimal montoDetalleFactura)
        {
            bool resultado = true;
            if ((entidadAdiantamento.NumeroPedido == null) |
                (entidadAdiantamento.ValorFatura == 0) |
                (Math.Abs(entidadAdiantamento.ValorFatura - montoDetalleFactura) > 10)
            )
                resultado = false;
            else
            {
                if ((entidadAdiantamento.NumeroPedido.ToString().Trim().Length == 0))
                    resultado = false;
            }
            return resultado;
        }

        private bool ValidarPedido(PedidoXml entidadPedido)
        {
            bool resultado = true;
            if (
                (entidadPedido.TipoOrdem == null) |
                (entidadPedido.EmisordaOrdem.ToString().Trim().Length == 0) |
                (entidadPedido.RecebedorMercaderia == null) |
                (entidadPedido.Trasportador == null) |
                (entidadPedido.LoteTransporte.ToString().Trim().Length == 0) |
                (entidadPedido.FormaPgto == null) |
                (entidadPedido.Incoterm == null) |
                (entidadPedido.Frete.ToString().Trim().Length == 0)
            )
                resultado = false;
            else
            {
                if ((entidadPedido.TipoOrdem.ToString().Trim().Length == 0) |
                  (entidadPedido.RecebedorMercaderia.ToString().Trim().Length == 0) |
                  (entidadPedido.Trasportador.ToString().Trim().Length == 0) |
                  (entidadPedido.FormaPgto.ToString().Trim().Length == 0) |
                  (entidadPedido.Incoterm.ToString().Trim().Length == 0)
                    )
                    resultado = false;
            }
            return resultado;
        }
        private bool ValidarCliente(ClientXml entidadCliente)
        {
            bool resultado = true;
            if (
                (entidadCliente.ClienteID == null) |
                        (entidadCliente.ENovo == null) |
                        (entidadCliente.Nome == null) |
                        (entidadCliente.Sexo == null) |
                        (entidadCliente.Rua == null) |
                        (entidadCliente.NumeroRua == null) |
                        (entidadCliente.Barrio == null) |
                        (entidadCliente.CEP == null) |
                        (entidadCliente.Cidade == null) |
                        (entidadCliente.Regiao == null) |
                        (entidadCliente.Email == null) |
                        (entidadCliente.CPF == null) |
                        (entidadCliente.SetorIndustrial == null) |
                        (entidadCliente.Recebedor == null) |
                        (entidadCliente.RuaRecebedor == null) |
                        (entidadCliente.NumeroRuaRecebedor == null) |
                        (entidadCliente.BairroRecebedor == null) |
                        (entidadCliente.CEPRecebedor == null) |
                        (entidadCliente.CidadeRecebedor == null) |
                        (entidadCliente.RegiaoRecebedor == null) |
                        (entidadCliente.EmailRecebedor == null)
                        )
                resultado = false;
            else
            {
                if (
                    (entidadCliente.ClienteID.ToString().Trim().Length == 0) |
                            (entidadCliente.ENovo.ToString().Trim().Length == 0) |
                            (entidadCliente.Nome.ToString().Trim().Length == 0) |
                            (entidadCliente.Sexo.ToString().Trim().Length == 0) |
                            (entidadCliente.Rua.ToString().Trim().Length == 0) |
                            (entidadCliente.NumeroRua.ToString().Trim().Length == 0) |
                            (entidadCliente.Barrio.ToString().Trim().Length == 0) |
                            (entidadCliente.CEP.ToString().Trim().Length == 0) |
                            (entidadCliente.Cidade.ToString().Trim().Length == 0) |
                            (entidadCliente.Regiao.ToString().Trim().Length == 0) |
                            (entidadCliente.Email.ToString().Trim().Length == 0) |
                            (entidadCliente.CPF.ToString().Trim().Length == 0) |
                            (entidadCliente.SetorIndustrial.ToString().Trim().Length == 0) |
                            (entidadCliente.Recebedor.ToString().Trim().Length == 0) |
                            (entidadCliente.RuaRecebedor.ToString().Trim().Length == 0) |
                            (entidadCliente.NumeroRuaRecebedor.ToString().Trim().Length == 0) |
                            (entidadCliente.BairroRecebedor.ToString().Trim().Length == 0) |
                            (entidadCliente.CEPRecebedor.ToString().Trim().Length == 0) |
                            (entidadCliente.CidadeRecebedor.ToString().Trim().Length == 0) |
                            (entidadCliente.RegiaoRecebedor.ToString().Trim().Length == 0) |
                            (entidadCliente.EmailRecebedor.ToString().Trim().Length == 0)
                            )
                    resultado = false;
            }
            return resultado;
        }
        #endregion

        private static List<ClientXmlDto> GetClientOrder(int LoteID)
        {
         
            return repository.GetClientOrder(LoteID);
        }
        private static List<AdiantamentoXmlDto> GetAdiantamentoOrder(int LoteID, CultureInfo CurrentCultureInfo)
        {

            return repository.GetAdiantamentoOrder(LoteID,   CurrentCultureInfo);
        }
        private static List<PedidoXmlDto> GetPedidoOrder(int LoteID, CultureInfo CurrentCultureInfo)
        {

            return repository.GetPedidoOrder(LoteID, CurrentCultureInfo);
        }
        private static List<OrderItemsXmlDto> GetDetailOrder(int LoteID, int OrderInvoiceIDIniPOut, int OrderInvoiceIDFinPOut, CultureInfo CurrentCultureInfo)
        {
            return repository.GetDetailOrder(LoteID, OrderInvoiceIDIniPOut, OrderInvoiceIDFinPOut, CurrentCultureInfo);
        }
        #endregion

        #region BR-B010 Interfaz Órdenes Devoluciones
        /// <summary>
        /// XML to Returned Order
        /// </summary>
        /// <param name="xmlPath"></param>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        public string GenerateXmlForReturnedOrder(string TemplateClientOrderReturn, string ReturOrderItemDetaill, int OrderID)
        {
            Func<string, ReturnOrderDetailXml, string> fncCargarplatillaDetalle = (strTemplateDetaill, objOrderDetailXml) =>
            {
                return string.Format(strTemplateDetaill,
                        objOrderDetailXml.Linea,
                        objOrderDetailXml.CategoriaItem,
                        objOrderDetailXml.Material,
                        objOrderDetailXml.Quantidade,
                        objOrderDetailXml.CentroDistribucao,
                        objOrderDetailXml.PresoPraticado.ToString("0.00", CultureInfo.InvariantCulture),
                        objOrderDetailXml.Desconto.ToString("0.00", CultureInfo.InvariantCulture));
            };
            string TemplateReturnedOrder = FileHelper.GetText(TemplateClientOrderReturn);
            string TemplateReturnedOrderDetail = FileHelper.GetText(ReturOrderItemDetaill);

            ReturnOrderHeaderXml OrderHeader = (from r in repository.GetHeaderReturnedOrder(OrderID) select (ReturnOrderHeaderXml)r).ToList()[0];
            List<ReturnOrderDetailXml> ListOrderDetail = (from r in repository.GetDetailReturnedOrder(OrderID) select (ReturnOrderDetailXml)r).ToList();

            StringBuilder sbDetaill = new StringBuilder();

            //StringBuilder XmlOrderDetail = new StringBuilder();
            foreach (var item in ListOrderDetail.Where(donde => !donde.TieneProductosLibre))
            {
                sbDetaill.Append(fncCargarplatillaDetalle(TemplateReturnedOrderDetail, item));
            }

            #region Numero Pedido
            string numeroPedido = string.Empty;
            string numeroPedidoOrigninal = string.Empty;
            if (ListOrderDetail.Where(donde => donde.TieneProductosLibre).Count() > 0)
            {
                numeroPedidoOrigninal = OrderHeader.NumeroPedido;
                numeroPedido = numeroPedidoOrigninal + "-1";
                OrderHeader.NumeroPedido = numeroPedido;
            }
            #endregion

            string XmlReturnOrder = string.Format(TemplateReturnedOrder,
                                                  OrderHeader.NumeroPedido,
                                                  OrderHeader.NumeroPedido,
                                                  OrderHeader.TipoOrdem,
                                                  OrderHeader.EmisordaOrdem,
                                                  OrderHeader.RecebedorMercaderia,
                                                  OrderHeader.Trasportador,
                                                  OrderHeader.loteTransporte,
                                                  OrderHeader.NumeroPedidoAnterior,
                                                  OrderHeader.DataOrder,
                                                  OrderHeader.FormaPgto,
                                                  OrderHeader.Incoterm,
                                                  OrderHeader.Frete.ToString("0.00", CultureInfo.InvariantCulture),
                                                  OrderHeader.TipoDevol,
                                                  sbDetaill.ToString(),
                                                  OrderHeader.numeroTitulo,
                                                  OrderHeader.NumeroPedido);

            sbDetaill = new StringBuilder();
            bool tieneProductosLibre = false;
            foreach (var item in ListOrderDetail.Where(donde => donde.TieneProductosLibre))
            {
                sbDetaill.Append(fncCargarplatillaDetalle(TemplateReturnedOrderDetail, item));
                tieneProductosLibre = true;
            }

            if (tieneProductosLibre)
            {
                numeroPedido = numeroPedidoOrigninal + "-2";
                OrderHeader.NumeroPedido = numeroPedido;
                XmlReturnOrder=XmlReturnOrder.Replace("</IT_PEDIDOS>", "");
                XmlReturnOrder=XmlReturnOrder.Replace("</n0:ZWS_MLM_PEDIDOS_B010>", "");
                XmlReturnOrder = XmlReturnOrder;
                string XmlOrderBrinde=string.Format(TemplateReturnedOrder,
                                                  OrderHeader.NumeroPedido,
                                                  OrderHeader.NumeroPedido,
                                                  OrderHeader.TipoOrdem,
                                                  OrderHeader.EmisordaOrdem,
                                                  OrderHeader.RecebedorMercaderia,
                                                  OrderHeader.Trasportador,
                                                  OrderHeader.loteTransporte,
                                                  OrderHeader.NumeroPedidoAnterior,
                                                  OrderHeader.DataOrder,
                                                  OrderHeader.FormaPgto,
                                                  OrderHeader.Incoterm,
                                                  OrderHeader.Frete.ToString("0.00", CultureInfo.InvariantCulture),
                                                  OrderHeader.TipoDevol,
                                                  sbDetaill.ToString(),
                                                  OrderHeader.numeroTitulo,
                                                  OrderHeader.NumeroPedido);
                XmlOrderBrinde=XmlOrderBrinde.Replace("<?xml version="+'"'+"1.0"+'"'+"?>", "");
                XmlOrderBrinde=XmlOrderBrinde.Replace("<n0:ZWS_MLM_PEDIDOS_B010 xmlns:n0="+'"'+"urn:sap-com:document:sap:rfc:functions"+'"'+">", "");
                XmlOrderBrinde=XmlOrderBrinde.Replace("<IT_PEDIDOS>", "");
                XmlReturnOrder = XmlReturnOrder + XmlOrderBrinde;
            }
            return XmlReturnOrder;
        }

        public string GenerateXmlForReturnedOrderB(string TemplateClientOrderReturn, string ReturOrderItemDetaill, int OrderID)
        {
            Func<string, ReturnOrderDetailXml, string> fncCargarplatillaDetalle = (strTemplateDetaill, objOrderDetailXml) =>
            {
                return string.Format(strTemplateDetaill,
                        objOrderDetailXml.Linea,
                        objOrderDetailXml.CategoriaItem,
                        objOrderDetailXml.Material,
                        objOrderDetailXml.Quantidade,
                        objOrderDetailXml.CentroDistribucao,
                        objOrderDetailXml.PresoPraticado.ToString("0.00", CultureInfo.InvariantCulture),
                        objOrderDetailXml.Desconto.ToString("0.00", CultureInfo.InvariantCulture));
            };
            string TemplateReturnedOrder = FileHelper.GetText(TemplateClientOrderReturn);
            string TemplateReturnedOrderDetail = FileHelper.GetText(ReturOrderItemDetaill);

            ReturnOrderHeaderXml OrderHeader = (from r in repository.GetHeaderReturnedOrder(OrderID) select (ReturnOrderHeaderXml)r).ToList()[0];
            List<ReturnOrderDetailXml> ListOrderDetail = (from r in repository.GetDetailReturnedOrder(OrderID) select (ReturnOrderDetailXml)r).ToList();

            StringBuilder sbDetaill = new StringBuilder();

            StringBuilder XmlOrderDetail = new StringBuilder();
            foreach (var item in ListOrderDetail)
            {
                sbDetaill.Append(fncCargarplatillaDetalle(TemplateReturnedOrderDetail, item));
            }

            string XmlReturnOrder = string.Format(TemplateReturnedOrder,
                                                  OrderHeader.NumeroPedido,
                                                  OrderHeader.NumeroPedido,
                                                  OrderHeader.TipoOrdem,
                                                  OrderHeader.EmisordaOrdem,
                                                  OrderHeader.RecebedorMercaderia,
                                                  OrderHeader.Trasportador,
                                                  OrderHeader.loteTransporte,
                                                  OrderHeader.NumeroPedidoAnterior,
                                                  OrderHeader.DataOrder,
                                                  OrderHeader.FormaPgto,
                                                  OrderHeader.Incoterm,
                                                  OrderHeader.Frete.ToString("0.00", CultureInfo.InvariantCulture),
                                                  OrderHeader.TipoDevol,
                                                  sbDetaill.ToString(),
                                                  OrderHeader.numeroTitulo,
                                                  OrderHeader.NumeroPedido);

            return XmlReturnOrder;
        }
        #endregion


        #region Interface B150

        public string MLM_ComisionesE_B150(int? period)
        {
            //string fileNameHist = "G_COMISSOES_S_BR" + GetTimeStampB150() + ".txt";
            //string filePathHist = ConfigurationManager.AppSettings["FileUploadAbsolutePath"] + ConfigurationManager.AppSettings["FileUploadPath_B150_Int"] + fileNameHist;
            string txt = "";
            List<DisbursementsSearchData> getDisbursements = new List<DisbursementsSearchData>();
            if (period == null)
            {
                getDisbursements = ProcessInEntered();
                if (getDisbursements.Count > 0)
                {
                    //using (StreamWriter txt = new StreamWriter(filePathHist))
                    //{
                    foreach (var item in getDisbursements)
                    {
                        if (getDisbursements.Count == 1)
                        {
                            txt = (item.Operacion + item.Empresa + item.CPFConsultora + item.CNPJConsultora + item.FechaEmision + item.Campania + ',' + item.Periodo + ',' + item.IdConsultora + item.IdDisbursement + item.IdConsultora + item.FechaVencimiento + item.Valor + item.Bloqueo + item.Campania + ',' + item.Periodo+ ','+ item.IdConsultora);
                            break;
                        }
                        txt = txt + (item.Operacion + item.Empresa + item.CPFConsultora + item.CNPJConsultora + item.FechaEmision + item.Campania + ',' + item.Periodo + ',' + item.IdConsultora + item.IdDisbursement + item.IdConsultora + item.FechaVencimiento + item.Valor + item.Bloqueo + item.Campania + ',' + item.Periodo + ',' + item.IdConsultora + "\r\n");
                    }                    
                }
            }
            else
            {
                getDisbursements = ProcessInPeriod(period.Value);
                if (getDisbursements.Count > 0)
                {
                    //using (StreamWriter txt = new StreamWriter(filePathHist))
                    //{ 
                    foreach (var item in getDisbursements)
                    {
                        if (getDisbursements.Count == 1)
                        {
                            txt = (item.Operacion + item.Empresa + item.CPFConsultora + item.CNPJConsultora + item.FechaEmision + item.Campania + ',' + item.Periodo + ',' + item.IdConsultora + item.IdDisbursement + item.IdConsultora + item.FechaVencimiento + item.Valor + item.Bloqueo + item.Campania + ',' + item.Periodo + ',' + item.IdConsultora);
                            break;
                        }
                        txt = txt + (item.Operacion + item.Empresa + item.CPFConsultora + item.CNPJConsultora + item.FechaEmision + item.Campania + ',' + item.Periodo + ',' + item.IdConsultora + item.IdDisbursement + item.IdConsultora + item.FechaVencimiento + item.Valor + item.Bloqueo + item.Campania + ',' + item.Periodo + ',' + item.IdConsultora + "\r\n");
                    }  
                    //}
                }
            }
            return txt;
        }

        public List<DisbursementsSearchData> ProcessInEntered()
        { 
             return XmlProductMaterialBusinessLogic.Instance.ObtenerDisbursementsService(null);
        }

        public List<DisbursementsSearchData> ProcessInPeriod(int period)
        {
            return XmlProductMaterialBusinessLogic.Instance.ObtenerDisbursementsService(period);
        }
        
        #endregion

        #region B155

        public List<DisbursementProfilesSearchData> GenerateOneLineCadastro(int? period)
        {
            return XmlProductMaterialBusinessLogic.Instance.ObtenerDisbursementProfilesService(period);
        }
        public string MLM_GeneraciónDeTxtCadastro_B155(int? period)
        {
            /*WV: 20160415 -- Solicitado por Hector Casas*/
            /* Se incluye modifacion del valor de BankCode para que en posicion 324 de interface texto llegue de 4 posiciones.*/
            string txt = ""; 
            char pad = '0';
            string ClaveBancoFil;
            
            List<DisbursementProfilesSearchData> getGenerateOneLineCadastro = new List<DisbursementProfilesSearchData>();
            getGenerateOneLineCadastro = GenerateOneLineCadastro(period);
            if (getGenerateOneLineCadastro.Count > 0)
            {
                foreach (var P in getGenerateOneLineCadastro)
                {
                    ClaveBancoFil = P.ClaveBanco.PadLeft(4, pad);
                    if (getGenerateOneLineCadastro.Count == 1)
                    {
                        txt = P.Empresa + P.Fisica + P.GrupoCuentaPersonal + P.FormaTratamiento + P.Nombre + P.ExpacioAuxiliar + P.IdConsultora + P.RUA + P.Numero + P.Barrio + P.CodigoPostal + P.Ciudad + P.Pais + P.Departamento + P.Idioma + P.Telefono + P.IdFiscal1 + P.PersonaFisica + P.DisbursementeProfileId + P.Pais + ClaveBancoFil + P.CuentaBancaria + P.NombreTitular + P.ClaveControlBanco + P.TipoBanco + P.ClaveBanco + P.CtaPF + P.CtaPJ + P.GrupoTesouraria + P.ClaveCondicionesPegamento + P.NumeroAgencia + "X" + P.FormaPago + "IF" + "F2" + "X" + "IN" + "N1" + "X" + "RP" + "P0" + "X" + P.NumerPIS;
                        break;
                    }
                    else
                    {
                        txt = txt + (P.Empresa + P.Fisica + P.GrupoCuentaPersonal + P.FormaTratamiento + P.Nombre + P.ExpacioAuxiliar + P.IdConsultora + P.RUA + P.Numero + P.Barrio + P.CodigoPostal + P.Ciudad + P.Pais + P.Departamento + P.Idioma + P.Telefono + P.IdFiscal1 + P.PersonaFisica + P.DisbursementeProfileId + P.Pais + ClaveBancoFil + P.CuentaBancaria + P.NombreTitular + P.ClaveControlBanco + P.TipoBanco + P.ClaveBanco + P.CtaPF + P.CtaPJ + P.GrupoTesouraria + P.ClaveCondicionesPegamento + P.NumeroAgencia + "X" + P.FormaPago + "IF" + "F2" + "X" + "IN" + "N1" + "X" + "RP" + "P0" + "X" + P.NumerPIS + "\r\n");
                    }

                }
            }
            return txt;
        }
        #endregion

    }
}
