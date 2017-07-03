using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using System.Data;
using NetSteps.Data.Entities.Business.Logic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NetSteps.Data.Entities.Utility;
using System.Globalization;
using NetSteps.Data.Entities;




namespace Generate_Manual_Billing
{
    class GenerateBatch
    {

        #region ProcesarBatch
        public string ProcesarBatch()
        {
            var LstOrderCustomerTemp = DataAccess.ExecWithStoreProcedureLists<int>("Core", "sp_GetOrdersID");

            List<int> LstOrderCustomer = LstOrderCustomerTemp.ToList();
            
            String resultado ="Ningun Dato a Procesar .... ";

            if(LstOrderCustomer.Count>0)
                resultado = InsertarOrderInvoiceItem(LstOrderCustomer, false);

            return resultado;
        }
        #endregion

        #region InsertarOrderInvoiceItem

        public string InsertarOrderInvoiceItem(List<int> LstOrderCustomer, Boolean Reprocesse = false)
        {
            CultureInfo cultureInfo = new CultureInfo("pt-BR");
           
            string fileTemp = ConfigurationManager.AppSettings["fileTemp"];           //Server.MapPath("~/Reports/FilesTemp/");

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

            int UserID = 16; //VERIFICAR
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
            LstOrderCustomer.ForEach((it) =>
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
                string pathTemplateClientsOrders = ConfigurationManager.AppSettings["TemplateClientsOrders"];  //Server.MapPath(ConfigurationManager.AppSettings["TemplateClientsOrders"]);
                string PathTemplateOrderItem = ConfigurationManager.AppSettings["TemplateOrderItem"];  //Server.MapPath(ConfigurationManager.AppSettings["TemplateOrderItem"]);
                string PathMainTemplateClientsOrders = ConfigurationManager.AppSettings["MainTemplateClientsOrders"];  //Server.MapPath(ConfigurationManager.AppSettings["MainTemplateClientsOrders"]);
                #endregion
                if (!Directory.Exists(ConfigurationManager.AppSettings["fileTemp"])) //Server.MapPath("~/Reports/FilesTemp/")))
                {
                    return "{ Estado = false, Resultado = 0, message = 'you must create this folder Reports/FilesTemp ' }";
                    //return Json(new "{ Estado = false, Resultado = 0, message = 'you must create this folder Reports/FilesTemp ' }");
                }
                string rutaLog = Path.Combine("Reports", "FilesTemp", "logSendFiles.txt");
                
                Console.WriteLine("Cargando los datos ......");

                LoteID = GenerateBatchBusinessLogic.InsOrderInvoicesOrderInvoiceItems(dtOrderCustomerIDs, UserID, Reprocesse, out OrderInvoiceIDIniPOut, out OrderInvoiceIDFinPOut);
                
                if (LoteID > 0)
                {
                    try
                    {
                        Console.WriteLine("Generando el XML .......");
                        data =XmlGeneratorBusinessLogic.Instance.CrearOrderPedidoXml(PathMainTemplateClientsOrders, pathTemplateClientsOrders, PathTemplateOrderItem, LoteID, OrderInvoiceIDIniPOut, OrderInvoiceIDFinPOut, cultureInfo);
                    }
                    catch (Exception ex)
                    {
                        return "{ message = '"+ex.Message+"' }";
                        //return JsonError(Translation.GetTerm("Validation Generate", ex.Message));
                    }
                }
                else
                {
                    return "{ Estado = false, Resultado = LoteID, message = 'Error try again' }";
                    //return Json(new { Estado = false, Resultado = LoteID, message = Translation.GetTerm("ErrorProc", "Error try again") });
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
                    
                    string directorioTemporalSapXml = Path.Combine(ConfigurationManager.AppSettings["fileTemp"], fileNameSapXml);

                    ArchivosCreadosSap[index] = Path.Combine("Reports", "FilesTemp", fileNameSapXml);

                }

                try
                {
                    Console.WriteLine("Guardando Archivo .......");
                    FileHelper.Save(XmlGeneradoTodasOrdenes, Path.Combine(rutaXmlSap, fileNameSapXml));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return "{ Estado = true, rutaLog = " + rutaLog + ", Resultado = " + LoteID + ", ArchivoGenerados = " + ArchivosCreados + ", ArchivosCreadosSap = " + ArchivosCreadosSap + " }";
                //return Json(new { Estado = true, rutaLog = rutaLog, Resultado = LoteID, ArchivoGenerados = ArchivosCreados, ArchivosCreadosSap = ArchivosCreadosSap });
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
                return ex.Message;
            }
        }

        #endregion

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
    }
}
