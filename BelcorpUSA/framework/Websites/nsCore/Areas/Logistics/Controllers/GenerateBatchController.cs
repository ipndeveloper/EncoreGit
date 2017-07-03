using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Business.Logic;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Helpers;
using nsCore.Controllers;
using System;
using NetSteps.Data.Entities;
using System.Data;
using System.Xml.Linq;
using System.IO;
using Microsoft.Reporting.WebForms;
using NetSteps.Common.Configuration;
using NetSteps.Data.Entities.Utility;
using System.Data.SqlClient;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Xml;
using System.Threading.Tasks;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Dto;
using CodeBarGeerator;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace nsCore.Areas.Logistics.Controllers
{
    public class GenerateBatchController : BaseController
    {
        //
        // GET: /Logistics/Logistics/

        #region Index
        public ActionResult Index()
        {

            #region Pruebas gyenerar xml de devolucion de ordenes
            // /*
            Action<int> GenerarXmlRetorno = (PoriginalOrderId) =>
            {

                //Constants.Country.UnitedStates
                // Constants.Country.UnitedStates


                var fechaActual = DateTime.Now;
                string RutaUnica = "MLM_Pedidos_" + PoriginalOrderId.ToString() + "_" + fechaActual.Year.ToString() + fechaActual.Month.ToString() + fechaActual.Day.ToString() + fechaActual.Hour.ToString() + fechaActual.Minute.ToString() + fechaActual.Second.ToString();
                RutaUnica += ".XML";
                string ReturOrderItemDetaill = Server.MapPath(ConfigurationManager.AppSettings["TemplateOrderReturnItemDetaill"]);
                string TemplateClientOrderReturn = Server.MapPath(ConfigurationManager.AppSettings["TemplateClientOrderReturn"]);
                string TemplateClientOrderBrinde = Server.MapPath(ConfigurationManager.AppSettings["TemplateClientOrderBrinde"]);
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

            // GenerarXmlRetorno(50002);

            // string XmlString = XmlGeneratorBusinessLogic.Instance.GenerateXmlForCancelledOrder(Server.MapPath(ConfigurationManager.AppSettings["TemplatesXML_Path"]), 50002);
            //   FileHelper.Save(XmlString, string.Format("{0}{1}CANCEL_{2}.xml", ConfigurationManager.AppSettings["FileUploadAbsolutePath"], ConfigurationManager.AppSettings["FileUploadPath_B010_Log"], 50002));



            #endregion
            ViewBag.WareHouse = Warehouse.GetWareHouse();
            Dictionary<int, string> dcperiodsFrom = new Dictionary<int, string>();
            Dictionary<int, string> dcperiodsTo = new Dictionary<int, string>();

            List<int> lstperiodsFrom = GenerateBatchBusinessLogic.Getperiods();
            lstperiodsFrom.Each((item) =>
            {
                dcperiodsFrom[item] = item.ToString();
            });

            List<int> lstperiodsTo = GenerateBatchBusinessLogic.GetperiodsFin();
            lstperiodsTo.Each((item) =>
            {
                dcperiodsTo[item] = item.ToString();
            });

            Dictionary<int, string> dc = new Dictionary<int, string>();

            ViewBag.periodsFrom = dcperiodsFrom;
            ViewBag.periodsTo = dcperiodsTo;

            Dictionary<int, string> dcOrderTypes = new Dictionary<int, string>();
            var lstOrderTypes = SmallCollectionCache.Instance.OrderTypes;
            lstOrderTypes.Each((obj) =>
            {
                dcOrderTypes[obj.OrderTypeID] = obj.Name;
            });

            Dictionary<int, string> dcShowGenerated = new Dictionary<int, string>();
            dcShowGenerated.Add(0, Translation.GetTerm("ShowNotGenerated", "Show not generated"));
            dcShowGenerated.Add(1, Translation.GetTerm("ShowGenerated", "Show generated"));
            dcShowGenerated.Add(2, Translation.GetTerm("All", "All"));

            ViewBag.WareHouses = Warehouse.GetWareHouse();
            ViewBag.ShippingMethods = GenerateBatchBusinessLogic.ListShippingMethods();
            ViewBag.AccountTypes = GenerateBatchBusinessLogic.ListAccountTypes();

            ViewBag.OrderTypes = dcOrderTypes;
            ViewBag.ShowGenerated = dcShowGenerated;
            //ViewBag.ListWarehousePrinters = GenerateBatchBusinessLogic.ListWarehousePrinters();
            return View();
        }
        #endregion

        #region ConsultaGenerateBillingProcess


        //public ActionResult ConsultaGenerateBillingProcess(
        //     int? WarehouseID, int? AccountID,
        //     int? MaterialID, int? ProductID,
        //     DateTime? StartDate, DateTime? EndDate,
        //     int? PeriodID, int? PeriodID2,
        //     int? ShippingMethodID, int? AccountTypeID,
        //     int? OrderTypeID, int? WarehousePrinterID,
        //     string OrderNumber, int? LogisticProviderID,
        //     int? RouteID,
        //     bool? Reprocess,
        //     string ids,
        //     string strLstOrderCustomerIDs,
        //     int? parametroAdicional
        public ActionResult ConsultaGenerateBillingProcess(int page,
                                                            int pageSize,
                                                            string orderBy,
                                                            NetSteps.Common.Constants.SortDirection orderByDirection,
                                                            string WarehouseID,
                                                            int? accountId,
                                                            int? MaterialID,
                                                            int? productoId,
                                                            DateTime? StartDate,
                                                            DateTime? EndDate,
                                                            int? PeriodID,
                                                            int? PeriodID2,
                                                            int? ShippingMethodID,
                                                            int? AccountTypeID,
                                                            int? OrderTypeID,
                                                            byte ShowGenerated,
                                                            int? WarehousePrinterID,
                                                            string OrderNumber,
                                                            int? LogisticsProviderID,
                                                            int? RouteID,
                                                            int? Reprocess,
                                                            string ids,
                                                            string strLstOrderCustomerIDs,
                                                            int? parametroAdicional)
        {
            //int _WarehouseID = 0;
            //int _AccountID = 0;
            //int _MaterialID = 0;
            //int _ProductID = 0;
            //int _LogisticProviderID = 0;
            //int _RouteID = 0;
            //int _Reprocess;
            //Dictionary<string, int> dcParametros = ids != null ? (new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Dictionary<string, int>>(ids)) : new Dictionary<string, int>();

            //dcParametros.TryGetValue("WarehouseID", out _WarehouseID);
            //dcParametros.TryGetValue("AccountID", out _AccountID);
            //dcParametros.TryGetValue("MaterialID", out _MaterialID);
            //dcParametros.TryGetValue("ProductID", out _ProductID);
            //dcParametros.TryGetValue("LogisticProviderID", out _LogisticProviderID);
            //dcParametros.TryGetValue("RouteID", out _RouteID);
            //dcParametros.TryGetValue("RouteID", out _RouteID);
            //dcParametros.TryGetValue("Reprocess", out _Reprocess);

            DataTable dtOrderCustomerIDs = new DataTable();
            dtOrderCustomerIDs.Columns.Add(
                        new DataColumn()
                        {
                            ColumnName = "OrderCustomerID",
                            DataType = typeof(int)
                        }
                    );
            List<int> OrderCustomerIDs;

            OrderCustomerIDs = strLstOrderCustomerIDs == null ? new List<int>() : new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<List<int>>(strLstOrderCustomerIDs);

            ((OrderCustomerIDs ?? new List<int>()).Count() == 0 ? new List<int>() { 0 } : OrderCustomerIDs).Each((it) =>
            {
                dtOrderCustomerIDs.Rows.Add(new object[] { it });
            });


            var LstOrderToBatch = GenerateBatchBusinessLogic.GetAllByFilters(new GenerateBatchParameters()
            {
                //_WarehouseID, _AccountID,
                //_MaterialID, _ProductID,
                //StartDate, EndDate,
                //PeriodID.HasValue ? PeriodID.Value : 0, PeriodID2.HasValue ? PeriodID2.Value : 0,
                //ShippingMethodID.HasValue ? ShippingMethodID.Value : 0, AccountTypeID.HasValue ? AccountTypeID.Value : 0,
                //OrderTypeID.HasValue ? OrderTypeID.Value : 0, WarehousePrinterID.HasValue ? WarehousePrinterID.Value : 0,
                //OrderNumber, _LogisticProviderID,
                //_RouteID, (_Reprocess == 1 ? true : false), dtOrderCustomerIDs
                PageIndex = page,
                PageSize = pageSize,
                OrderBy = orderBy,
                OrderByDirection = orderByDirection,
                WarehouseID = Convert.ToInt32(WarehouseID),
                AccountID = Convert.ToInt32(accountId),
                MaterialID = Convert.ToInt32(MaterialID),
                ProductID = Convert.ToInt32(productoId),
                StartDate = StartDate,
                EndDate = EndDate,
                PeriodID = PeriodID.HasValue ? PeriodID.Value : 0,
                PeriodID2 = PeriodID2.HasValue ? PeriodID2.Value : 0,
                ShippingMethodID = ShippingMethodID.HasValue ? ShippingMethodID.Value : 0,
                AccountTypeID = AccountTypeID.HasValue ? AccountTypeID.Value : 0,
                OrderTypeID = OrderTypeID.HasValue ? OrderTypeID.Value : 0,
                WarehousePrinterID = WarehousePrinterID.HasValue ? WarehousePrinterID.Value : 0,
                OrderNumber = OrderNumber,
                LogisticProviderID = Convert.ToInt32(LogisticsProviderID),
                RouteID = Convert.ToInt32(RouteID),
                ShowGenerated = ShowGenerated,
                Reprocess = (Reprocess == 1 ? true : false),
                dtOrderCustomerIDs = dtOrderCustomerIDs
            });

            StringBuilder builder = new StringBuilder();
            int rowCount = LstOrderToBatch.Count();

            if (LstOrderToBatch.Any())
            {
                foreach (var item in LstOrderToBatch)
                {
                    builder.Append("<tr id='" + item.OrderCustomerID.ToString() + "'>")
                   .AppendCell("<input value='" + item.OrderCustomerID.ToString() + "' type='checkbox'/>")
                   .AppendCell(item.OrderID.ToString())
                   .AppendCell(item.Period.ToString())
                   .AppendCell(item.CompleteDate.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo))
                   .AppendCell(item.Quantity.ToString())
                   .AppendCell(item.Amount.ToString("N",CoreContext.CurrentCultureInfo))
                   .AppendCell(item.Consultant)
                   .AppendCell(item.Transporter)
                   .AppendCell(item.Route)
                   .AppendCell(item.CityState)
                   .AppendCell(item.ShipmentMethod)
                   .AppendCell(item.BatchGenerated.ToString())
                   .Append("</tr>");

                }
                return Json(new { result = true, totalPages = LstOrderToBatch.TotalPages, page = builder.ToString() });
            }
            else
                return Json(new { result = true, totalPages = 0, page = "<tr><td colspan=\"9\">" + Translation.GetTerm("ThereWereNoRecordsFoundThatMeetThatCriteriaPleaseTryAgain", "There were no records found that meet that criteria. Please try again.") + "</td></tr>" });
        }

        #endregion

        public ActionResult ListWarehousePrinters(short WarehouseID)
        {
            var result = GenerateBatchBusinessLogic.ListWarehousePrinters(WarehouseID).Select(c => new SelectListItem
            {
                Value = c.Key.ToString(),
                Text = c.Value
            });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region GenerateBatch

        public ActionResult GenerateBatch()
        {

            #region Pruebas gyenerar xml de devolucion de ordenes
            // /*
            Action<int> GenerarXmlRetorno = (PoriginalOrderId) =>
            {

                //Constants.Country.UnitedStates
                // Constants.Country.UnitedStates


                var fechaActual = DateTime.Now;
                string RutaUnica = "MLM_Pedidos_" + PoriginalOrderId.ToString() + "_" + fechaActual.Year.ToString() + fechaActual.Month.ToString() + fechaActual.Day.ToString() + fechaActual.Hour.ToString() + fechaActual.Minute.ToString() + fechaActual.Second.ToString();
                RutaUnica += ".XML";
                string ReturOrderItemDetaill = Server.MapPath(ConfigurationManager.AppSettings["TemplateOrderReturnItemDetaill"]);
                string TemplateClientOrderReturn = Server.MapPath(ConfigurationManager.AppSettings["TemplateClientOrderReturn"]);
                string TemplateClientOrderBrinde = Server.MapPath(ConfigurationManager.AppSettings["TemplateClientOrderBrinde"]);
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

            // GenerarXmlRetorno(50002);

            // string XmlString = XmlGeneratorBusinessLogic.Instance.GenerateXmlForCancelledOrder(Server.MapPath(ConfigurationManager.AppSettings["TemplatesXML_Path"]), 50002);
            //   FileHelper.Save(XmlString, string.Format("{0}{1}CANCEL_{2}.xml", ConfigurationManager.AppSettings["FileUploadAbsolutePath"], ConfigurationManager.AppSettings["FileUploadPath_B010_Log"], 50002));



            #endregion
            ViewBag.WareHouse = Warehouse.GetWareHouse();
            Dictionary<int, string> dcperiodsFrom = new Dictionary<int, string>();
            Dictionary<int, string> dcperiodsTo = new Dictionary<int, string>();

            List<int> lstperiodsFrom = GenerateBatchBusinessLogic.Getperiods();
            lstperiodsFrom.Each((item) =>
            {
                dcperiodsFrom[item] = item.ToString();
            });

            List<int> lstperiodsTo = GenerateBatchBusinessLogic.GetperiodsFin();
            lstperiodsTo.Each((item) =>
            {
                dcperiodsTo[item] = item.ToString();
            });

            Dictionary<int, string> dc = new Dictionary<int, string>();

            ViewBag.periodsFrom = dcperiodsFrom;
            ViewBag.periodsTo = dcperiodsTo;

            Dictionary<int, string> dcOrderTypes = new Dictionary<int, string>();
            var lstOrderTypes = SmallCollectionCache.Instance.OrderTypes;
            lstOrderTypes.Each((obj) =>
            {
                dcOrderTypes[obj.OrderTypeID] = obj.Name;
            });

            Dictionary<string, string> asd = Warehouse.GetWareHouse();

            Dictionary<int, string> dcShowGenerated = new Dictionary<int, string>();
            dcShowGenerated.Add(0, Translation.GetTerm("ShowNotGenerated", "Show not generated"));
            dcShowGenerated.Add(1, Translation.GetTerm("ShowGenerated", "Show generated"));
            dcShowGenerated.Add(2, Translation.GetTerm("All", "All"));

            ViewBag.WareHouses = Warehouse.GetWareHouse();
            ViewBag.ShippingMethods = GenerateBatchBusinessLogic.ListShippingMethods();
            ViewBag.AccountTypes = GenerateBatchBusinessLogic.ListAccountTypes();

            ViewBag.OrderTypes = dcOrderTypes;
            ViewBag.OrderTypes = dcShowGenerated;
            return View();
        }

        #endregion

        #region ConsultaGenerateBatchSeparation

        public ActionResult ConsultaGenerateBatchSeparation(int page,
                                                            int pageSize,
                                                            string orderBy,
                                                            NetSteps.Common.Constants.SortDirection orderByDirection,
                                                            string WarehouseID,
                                                            int? accountId,
                                                            int? MaterialID,
                                                            int? productoId,
                                                            DateTime? StartDate,
                                                            DateTime? EndDate,
                                                            int? PeriodID,
                                                            int? PeriodID2,
                                                            int? ShippingMethodID,
                                                            int? AccountTypeID,
                                                            int? OrderTypeID,
                                                            int? WarehousePrinterID,
                                                            string OrderNumber,
                                                            int? LogisticsProviderID,
                                                            int? RouteID,
                                                            string ids,
                                                            string strLstOrderCustomerIDs,
                                                            int? parametroAdicional)
        {
            try
            {
                if (string.IsNullOrEmpty(WarehouseID) || WarehouseID.Equals("0"))
                {
                    return Json(new { totalPages = 0, page = "", message = "" });
                }

                DataTable dtOrderCustomerIDs = new DataTable();
                dtOrderCustomerIDs.Columns.Add(
                            new DataColumn()
                            {
                                ColumnName = "OrderCustomerID",
                                DataType = typeof(int)
                            }
                        );
                List<int> OrderCustomerIDs;

                OrderCustomerIDs = strLstOrderCustomerIDs == null ? new List<int>() : new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<List<int>>(strLstOrderCustomerIDs);

                ((OrderCustomerIDs ?? new List<int>()).Count() == 0 ? new List<int>() { 0 } : OrderCustomerIDs).Each((it) =>
                {
                    dtOrderCustomerIDs.Rows.Add(new object[] { it });
                });

                var exception = EntityExceptionHelper.GetAndLogNetStepsException("Antes de llamar metodo Get", NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);

                var LstOrderToBatch = GenerateBatchBusinessLogic.GetAllOrdersByFilters(new GenerateBatchParameters()
                {
                    PageIndex = page,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    OrderByDirection = orderByDirection,
                    WarehouseID = Convert.ToInt32(WarehouseID),
                    AccountID = Convert.ToInt32(accountId),
                    MaterialID = Convert.ToInt32(MaterialID),
                    ProductID = Convert.ToInt32(productoId),
                    StartDate = StartDate,
                    EndDate = EndDate,
                    PeriodID = PeriodID.HasValue ? PeriodID.Value : 0,
                    PeriodID2 = PeriodID2.HasValue ? PeriodID2.Value : 0,
                    ShippingMethodID = ShippingMethodID.HasValue ? ShippingMethodID.Value : 0,
                    AccountTypeID = AccountTypeID.HasValue ? AccountTypeID.Value : 0,
                    OrderTypeID = OrderTypeID.HasValue ? OrderTypeID.Value : 0,
                    WarehousePrinterID = WarehousePrinterID.HasValue ? WarehousePrinterID.Value : 0,
                    OrderNumber = OrderNumber,
                    LogisticProviderID = Convert.ToInt32(LogisticsProviderID),
                    RouteID = Convert.ToInt32(RouteID),
                    dtOrderCustomerIDs = dtOrderCustomerIDs
                });

                var exception1 = EntityExceptionHelper.GetAndLogNetStepsException("Despues de llamar metodo Get", NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                StringBuilder builder = new StringBuilder();

                if (LstOrderToBatch.Any())
                {
                    foreach (var item in LstOrderToBatch)
                    {
                        builder.Append("<tr id='" + item.OrderCustomerID.ToString() + "'>")
                       .AppendCell("<input value='" + item.OrderID.ToString() + "' type='checkbox'/>")
                       .AppendCell(item.OrderID.ToString())
                       .AppendCell(item.Period.ToString())
                       .AppendCell(item.CompleteDate.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo))
                       .AppendCell(item.Quantity.ToString())
                       .AppendCell(item.Amount.ToString("N",CoreContext.CurrentCultureInfo))
                       .AppendCell(item.Consultant)
                       .AppendCell(item.Transporter)
                       .AppendCell(item.Route)
                       .AppendCell(item.CityState)
                       .AppendCell(item.ShipmentMethod)
                       .Append("</tr>");

                    }
                    return Json(new { result = true, totalPages = LstOrderToBatch.TotalPages, page = builder.ToString() });
                }
                else
                    return Json(new { result = true, totalPages = 0, page = "<tr><td colspan=\"11\">" + Translation.GetTerm("ThereWereNoRecordsFoundThatMeetThatCriteriaPleaseTryAgain", "There were no records found that meet that criteria. Please try again.") + "</td></tr>" });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }


        #endregion

        #region InsertarOrderInvoiceItem

        [HttpPost]
        public ActionResult InsertarOrderInvoiceItem(List<int> LstOrderCustomer, Boolean Reprocesse = false)
        {

            string fileTemp = Server.MapPath("~/Reports/FilesTemp/");

            #region creacion de Log

            if (!System.IO.File.Exists(Path.Combine(fileTemp, "log.txt")))
            {
                using (var file = System.IO.File.Create(Path.Combine(fileTemp, "log.txt")))
                {
                    file.Close();
                    file.Dispose();
                }
            }
            #endregion

            #region creacion de filtros

            int UserID = CoreContext.CurrentUser.UserID;
            int LoteID = 0;//lote insertado
            DataTable dtOrderCustomerIDs = new DataTable();
            LstOrderCustomer = LstOrderCustomer.Distinct().ToList();
            dtOrderCustomerIDs.Columns.Add(
                        new DataColumn()
                        {
                            ColumnName = "OrderCustomerID",
                            DataType = typeof(int)
                        }
                                        );
            LstOrderCustomer.Each((it) =>
            {
                dtOrderCustomerIDs.Rows.Add(new object[] { it });
            });

            #endregion

            try
            {
                int OrderInvoiceIDIniPOut = 0;
                int OrderInvoiceIDFinPOut = 0;
                string[,] data = null;
                #region variables de configuracion
                string pathTemplateClientsOrders = Server.MapPath(ConfigurationManager.AppSettings["TemplateClientsOrders"]);
                string PathTemplateOrderItem = Server.MapPath(ConfigurationManager.AppSettings["TemplateOrderItem"]);
                string PathMainTemplateClientsOrders = Server.MapPath(ConfigurationManager.AppSettings["MainTemplateClientsOrders"]);
                #endregion
                if (!Directory.Exists(Server.MapPath("~/Reports/FilesTemp/")))
                {
                    return Json(new { Estado = false, Resultado = 0, message = "you must create this folder Reports/FilesTemp " });
                }
                string rutaLog = Path.Combine("Reports", "FilesTemp", "logSendFiles.txt");

                LoteID = GenerateBatchBusinessLogic.InsOrderInvoicesOrderInvoiceItems(dtOrderCustomerIDs, UserID, Reprocesse, out OrderInvoiceIDIniPOut, out OrderInvoiceIDFinPOut);
                if (LoteID > 0)
                {
                    try
                    {
                        data = XmlGeneratorBusinessLogic.Instance.CrearOrderPedidoXml(PathMainTemplateClientsOrders, pathTemplateClientsOrders, PathTemplateOrderItem, LoteID, OrderInvoiceIDIniPOut, OrderInvoiceIDFinPOut, CoreContext.CurrentCultureInfo);
                    }
                    catch (Exception ex)
                    {
                        return JsonError(Translation.GetTerm("Validation Generate", ex.Message));
                    }
                }
                else
                {
                    return Json(new { Estado = false, Resultado = LoteID, message = Translation.GetTerm("ErrorProc", "Error try again") });
                }

                string rutaXmlSap = ConfigurationManager.AppSettings["FileUploadXmlSap"];

                string ModoLocal = ConfigurationManager.AppSettings["ModoLocal"];
                string ListaOrdersNumber = data[0, 1];
                string XmlGeneradoTodasOrdenes = data[0, 0];

                string[] ArchivosCreados = new string[ListaOrdersNumber.Split(';').Length - 1];
                string[] ArchivosCreadosSap = new string[ListaOrdersNumber.Split(';').Length - 1];


                var fechaActual = DateTime.Now;

                string RutaUnica = Right(fechaActual.Year.ToString(), 2) + Right(("00" + fechaActual.Month.ToString()), 2) + Right(("00" + fechaActual.Day.ToString()), 2) + Right(("00" + fechaActual.Hour.ToString()), 2) + Right(("00" + fechaActual.Minute.ToString()), 2) + Right(("00" + fechaActual.Second.ToString()), 2);

                var fileNameSapXml = "MLM_Pedidos_B010_" + RutaUnica + ".xml";

                for (int index = 0; index < ListaOrdersNumber.Split(';').Length - 1; index++)
                {
                    //var file = PrintInvoicePDF(ListaOrdersNumber.Split(';')[index]);


                    //var fileName = "Order" + ListaOrdersNumber.Split(';')[index] + RutaUnica + " .pdf";


                    //if (!Directory.Exists(Server.MapPath("~/Reports/FilesTemp/")))
                    //{
                    //    Directory.CreateDirectory(Server.MapPath("~/Reports/FilesTemp/"));
                    //}
                    //string directorioTemporal = Path.Combine(Server.MapPath("~/Reports/FilesTemp/"), fileName);
                    string directorioTemporalSapXml = Path.Combine(Server.MapPath("~/Reports/FilesTemp/"), fileNameSapXml);

                    //ArchivosCreados[index] = Path.Combine("Reports", "FilesTemp", fileName);
                    ArchivosCreadosSap[index] = Path.Combine("Reports", "FilesTemp", fileNameSapXml);

                    //System.IO.File.WriteAllBytes(Path.Combine(directorioTemporal), file);
                }

                try
                {
                    FileHelper.Save(XmlGeneradoTodasOrdenes, Path.Combine(rutaXmlSap, fileNameSapXml));
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return Json(new { Estado = true, rutaLog = rutaLog, Resultado = LoteID, ArchivoGenerados = ArchivosCreados, ArchivosCreadosSap = ArchivosCreadosSap });
            }
            catch (Exception ex)
            {
                using (var file = System.IO.File.AppendText(Path.Combine(fileTemp, "logSendFiles.txt")))
                {
                    string fechaMs = DateTime.Now.ToString("yyyyMMddHHmmss");
                    file.WriteLine(string.Format("[Fecha Hora: {0}]", fechaMs));
                    file.WriteLine("Error [Message]" + ex.Message);
                    file.WriteLine("LoteID " + LoteID.ToString());
                    file.WriteLine("OrderCustomerIDs " + string.Join(",", LstOrderCustomer.ToArray()));
                    file.WriteLine(string.Format("[Fecha Hora: {0}]", fechaMs));
                }
                throw ex;
            }
        }

        #endregion

        #region GenerarLY037

        [HttpPost]
        public ActionResult GenerarLY037(List<int> LstOrderIds, string WareHouseID)
        {

            string fileTemp = Server.MapPath("~/Reports/FilesTemp/");

            #region creacion de Log

            if (!System.IO.File.Exists(Path.Combine(fileTemp, "log.txt")))
            {
                using (var file = System.IO.File.Create(Path.Combine(fileTemp, "log.txt")))
                {
                    file.Close();
                    file.Dispose();
                }
            }
            #endregion

            #region creacion de filtros

            int UserID = CoreContext.CurrentUser.UserID;
            int SeparationLoteID = 0;//lote insertado
            DataTable dtOrderIDs = new DataTable();
            LstOrderIds = LstOrderIds.Distinct().ToList();
            dtOrderIDs.Columns.Add(
                        new DataColumn()
                        {
                            ColumnName = "OrderCustomerID",
                            DataType = typeof(int)
                        }
                                        );
            LstOrderIds.Each((it) =>
            {
                dtOrderIDs.Rows.Add(new object[] { it });
            });

            #endregion

            try
            {
                var fechaActual = DateTime.Now;
                string rutaLog = Path.Combine("Reports", "FilesTemp", "logSendFiles.txt");
                string rutaFTPArchive = WareHouseID.Equals("1") ? ConfigurationManager.AppSettings["FileUploadLY037_CDSummare"] : ConfigurationManager.AppSettings["FileUploadLY037_CDCabo"];
                string RutaUnica = Right(fechaActual.Year.ToString(), 2) + Right(("00" + fechaActual.Month.ToString()), 2) + Right(("00" + fechaActual.Day.ToString()), 2) + Right(("00" + fechaActual.Hour.ToString()), 2) + Right(("00" + fechaActual.Minute.ToString()), 2) + Right(("00" + fechaActual.Second.ToString()), 2);
                var fileNameY037 = "LY037V" + RutaUnica + ".txt";

                SeparationLoteID = GenerateBatchBusinessLogic.InsertOrderSeparationLote(dtOrderIDs);
                if (SeparationLoteID > 0)
                {
                    var dtStringE010 = GenerateBatchBusinessLogic.GetOrdersValuesForE010(SeparationLoteID);

                    /*if (dtStringE010 != null)
                    {
                        StringBuilder FileToFTP = new StringBuilder();
                        foreach (DataRow row in dtStringE010.Rows)
                        {
                            FileToFTP.Append(row["STRING"].ToString() + "\n");
                        }
                        FileHelper.Save(FileToFTP.ToString(), Path.Combine(rutaFTPArchive, fileNameY037));
                    }
                    else
                    {
                        return Json(new { Estado = false, Resultado = SeparationLoteID, message = Translation.GetTerm("ErrorProcdtStringE010", "Error dtStringE010") });
                    }*/

                }
                else
                {
                    return Json(new { Estado = false, Resultado = SeparationLoteID, message = Translation.GetTerm("ErrorProc", "Error try again") });
                }

                #region GeneratePDF

                Task.Factory.StartNew(() => GeneratePDF(dtOrderIDs, WareHouseID));

                #endregion

                return Json(new { Estado = true, rutaLog = rutaLog, Resultado = SeparationLoteID, message = Translation.GetTerm("Correcto", "Batch Generado Correctamente") });
            }
            catch (Exception ex)
            {
                ex.Log(Constants.NetStepsExceptionType.NetStepsDataException, internalMessage: string.Format("Error al Cargar el Archivo: {1}", ex.Message, "GenerarLY037"));
                return Json(new { Estado = false, Resultado = SeparationLoteID, message = ex.Message });
                /*using (var file = System.IO.File.AppendText(Path.Combine(fileTemp, "logSendFiles.txt")))
                {
                    string fechaMs = DateTime.Now.ToString("yyyyMMddHHmmss");
                    file.WriteLine(string.Format("[Fecha Hora: {0}]", fechaMs));
                    file.WriteLine("Error [Message]" + ex.Message);
                    file.WriteLine("LoteID " + SeparationLoteID.ToString());
                    file.WriteLine("OrderCustomerIDs " + string.Join(",", LstOrderIds.ToArray()));
                    file.WriteLine(string.Format("[Fecha Hora: {0}]", fechaMs));
                }
                throw ex;*/
            }
        }

        private void GeneratePDF(DataTable dtOrderNumbers, string WareHouseID)
        {
            Dictionary<int, string[]> OrderList = OrderExtensions.GetInvoiceKey(dtOrderNumbers);
            var fechaActual = DateTime.Now;
            StringBuilder FileTSM = new StringBuilder();
            string rutaTSMArchive = WareHouseID.Equals("1") ? ConfigurationManager.AppSettings["FileUploadTSM_CDSummare"] : ConfigurationManager.AppSettings["FileUploadTSM_CDCabo"];
            string RutaUnica = Right(fechaActual.Year.ToString(), 2) + Right(("00" + fechaActual.Month.ToString()), 2) + Right(("00" + fechaActual.Day.ToString()), 2) + Right(("00" + fechaActual.Hour.ToString()), 2) + Right(("00" + fechaActual.Minute.ToString()), 2) + Right(("00" + fechaActual.Second.ToString()), 2);
            var fileNameTSM = "TSM" + RutaUnica + ".txt";
            string AñoMes = string.Empty;

            foreach (KeyValuePair<int, string[]> kvp in OrderList)
            {
                byte[] data = PrintInvoicePDF(kvp.Key.ToString());
                AñoMes = string.IsNullOrEmpty(kvp.Value[1]) ? string.Format("{0}{1}", DateTime.Now.Year.ToString().Substring(2, 2), DateTime.Now.Month.ToString("00.##")) : kvp.Value[1];

                var fileName = string.Format("{0}_{1}.pdf", string.IsNullOrEmpty(kvp.Value[0]) ? kvp.Key.ToString() : kvp.Value[0], AñoMes);
                string PathFTP = ConfigurationManager.AppSettings["FileUploadGenerateBatchSeparation"];
                FileHelper.Save(new MemoryStream(data), Path.Combine(PathFTP, fileName));
                //'1605/26160512342436000490550020010001961869003075_1605.pdf,26160512342436000490550020010001961869003075_1605.pdf'
                FileTSM.AppendLine(string.Format("{0}/{1},{2}", AñoMes, fileName, fileName));
            }

            FileHelper.Save(FileTSM.ToString(), Path.Combine(rutaTSMArchive, fileNameTSM));
        }

        #endregion

        #region cargarLogisticsProvider

        [HttpPost]
        public ActionResult cargarLogisticsProvider(int LogisticProviderID)
        {
            Session["LogisticProviderID"] = LogisticProviderID;
            return Json(new { resultado = LogisticProviderID }); ;
        }

        #endregion

        #region  Autocomplete
        [OutputCache(CacheProfile = "AutoCompleteData")]
        public virtual ActionResult WarehouseSearch(string query)
        {
            try
            {

                var dcAccounts = Warehouse.GetWareHouse()
                                    .Where((dc) => { return dc.Value.Contains(query); })
                                    .Select((dc) => new { id = dc.Key, text = dc.Value });
                return Json(dcAccounts);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "AutoCompleteData")]
        public virtual ActionResult AccountSearch(string query)
        {
            try
            {

                var dcAccounts = PaymentTicktesBussinessLogic.AccountSearchAuto(query).Select((dc) => new { id = dc.Key, text = dc.Value });
                return Json(dcAccounts);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "AutoCompleteData")]
        public virtual ActionResult MaterialsSearch(string query)
        {
            try
            {

                var dcMaterial = GenerateBatchBusinessLogic.ListMaterials(query).Select((dc) => new { id = dc.Key, text = dc.Value });
                return Json(dcMaterial);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
        [OutputCache(CacheProfile = "AutoCompleteData")]
        public virtual ActionResult ProductsSearch(string query)
        {
            try
            {

                var dcProducts = GenerateBatchBusinessLogic.ListProducts(query).Select((dc) => new { id = dc.Key, text = dc.Value });
                return Json(dcProducts);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "AutoCompleteData")]
        public virtual ActionResult LogisticProviderSearch(string query)
        {
            try
            {
                var dcLogisticProvider = (LogisticShippingRules.LogisticProviderLookUp(query) ?? new Dictionary<int, string>()).Select((dc) => new { id = dc.Key, text = dc.Value });
                return Json(dcLogisticProvider);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
        [OutputCache(CacheProfile = "AutoCompleteData")]
        public virtual ActionResult RouteXLogisticProviderSearch(string query)
        {
            try
            {
                int LogisticProviderID = 0;
                if (Session["LogisticProviderID"] != null)
                {
                    LogisticProviderID = (int)Session["LogisticProviderID"];
                }


                var dcLogisticProvider = GenerateBatchBusinessLogic.ListRouteXlogisticsProvider(query, LogisticProviderID).Select((dc) => new { id = dc.Key, text = dc.Value });
                return Json(dcLogisticProvider);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }


        #endregion

        //public static bool HasWritePermissionOnDir(string path)
        //{
        //    var writeAllow = false;
        //    var writeDeny = false;
        //    var accessControlList = Directory.GetAccessControl(path);
        //    if (accessControlList == null)
        //        return false;
        //    var accessRules = accessControlList.GetAccessRules(true, true,
        //                                typeof(System.Security.Principal.SecurityIdentifier));
        //    if (accessRules == null)
        //        return false;

        //    foreach (FileSystemAccessRule rule in accessRules)
        //    {
        //        if ((FileSystemRights.Write & rule.FileSystemRights) != FileSystemRights.Write)
        //            continue;

        //        if (rule.AccessControlType == AccessControlType.Allow)
        //            writeAllow = true;
        //        else if (rule.AccessControlType == AccessControlType.Deny)
        //            writeDeny = true;
        //    }
        //}

        #region Funciones
        public static string Left(string param, int length)
        {
            //we start at 0 since we want to get the characters starting from the
            //left and with the specified lenght and assign it to a variable
            string result = param.Substring(0, length);
            //return the result of the operation
            return result;
        }
        public static string Right(string param, int length)
        {
            //start at the index based on the lenght of the sting minus
            //the specified lenght and assign it a variable
            string result = param.Substring(param.Length - length, length);
            //return the result of the operation
            return result;
        }
        #endregion

        //impresion de documento de orden [Req:BR-PD-005]
        //creado por salcedo vila G. GYS
        //verificar que este el archivo nsCore/Reports/RptOrder.rdlc
        #region Impresion

        private byte[] PrintInvoicePDF(string orderNumber)
        {
            List<ReportParameter> lstParams = new List<ReportParameter>();
            lstParams = CreateParameterLabelReport();
            byte[] buffer = CreateReport(lstParams: lstParams, orderNumber: orderNumber);
            return buffer;
            //   return File(buffer, "application/pdf", "Order" + orderNumber + ".pdf");

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
            dsData = OrderReportBusinessLogic.OrderSearch(orderNumber, CoreContext.CurrentLanguageID);

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