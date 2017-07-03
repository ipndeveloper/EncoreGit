using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities;
using System.IO;
using System.Data;
using NetSteps.Common.Extensions;
using NetSteps.Common.Configuration;
using NetSteps.Data.Entities.Business.Logic;
using NetSteps.Data.Entities.Utility;
using System.Globalization;
using NetSteps.QueueProcessing.Modules.ModuleBase;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.QueueProcessing.Modules.B010Service
{
    public class B010Processor : QueueProcessor<IEnumerable<int>>
    {

        #region Properties

        public static readonly string CProcessorName = "B010Processor";

        #region LogPath

        /// <summary>
        /// Gets logPath for B010 process
        /// </summary>
        private string LogPath
        {
            get
            {
                if (ConfigurationManager.AppSettings["LogPath"] != null)
                {
                    return ConfigurationManager.AppSettings["LogPath"].ToString();
                }
                return "~/Reports/FilesTemp/";
            }
        } 
        #endregion

        #region FileUploadXmlSap

        /// <summary>
        /// Gets FileUploadXmlSap for B010 process
        /// </summary>
        private string FileUploadXmlSap
        {
            get
            {
                if (ConfigurationManager.AppSettings["FileUploadXmlSap"] != null)
                {
                    return ConfigurationManager.AppSettings["FileUploadXmlSap"].ToString();
                }
                return "//172.19.2.133/ftp_qas/B010/Int/";
            }
        } 

        #endregion

        #region FileUploadXmlRapidao

        /// <summary>
        /// Gets FileUploadXmlRapidao for B010 process
        /// </summary>
        private string FileUploadXmlRapidao
        {
            get
            {
                if (ConfigurationManager.AppSettings["FileUploadXmlRapidao"] != null)
                {
                    return ConfigurationManager.AppSettings["FileUploadXmlRapidao"].ToString();
                }
                return "//172.19.2.133/ftp_qas/B010/IntRap/";
            }
        } 

        #endregion

        #region TemplateClientsOrders

        /// <summary>
        /// Gets TemplateClientsOrders for B010 process
        /// </summary>
        private string TemplateClientsOrders
        {
            get
            {
                if (ConfigurationManager.AppSettings["TemplateClientsOrders"] != null)
                {
                    return ConfigurationManager.AppSettings["TemplateClientsOrders"].ToString();
                }
                return "~/FileUploads/XmlTemplates/TemplateClientsOrders.xml";
            }
        } 

        #endregion

        #region TemplateOrderItem

        /// <summary>
        /// Gets TemplateOrderItem for B010 process
        /// </summary>
        private string TemplateOrderItem
        {
            get
            {
                if (ConfigurationManager.AppSettings["TemplateOrderItem"] != null)
                {
                    return ConfigurationManager.AppSettings["TemplateOrderItem"].ToString();
                }
                return "~/FileUploads/XmlTemplates/TemplateOrderItem.xml";
            }
        } 

        #endregion

        #region MainTemplateClientsOrders

        /// <summary>
        /// Gets MainTemplateClientsOrders for B010 process
        /// </summary>
        private string MainTemplateClientsOrders
        {
            get
            {
                if (ConfigurationManager.AppSettings["MainTemplateClientsOrders"] != null)
                {
                    return ConfigurationManager.AppSettings["MainTemplateClientsOrders"].ToString();
                }
                return "~/FileUploads/XmlTemplates/MainTemplateClientsOrders.xml";
            }
        } 

        #endregion

        #region MinutesCount

        /// <summary>
        /// Gets MinutesCount for B010 process
        /// </summary>
        private int MinutesCount
        {
            get
            {
                if (ConfigurationManager.AppSettings["MinutesCount"] != null)
                {
                    return Convert.ToInt16(ConfigurationManager.AppSettings["MinutesCount"].ToString());
                }
                return 0;
            }
        }

        #endregion

        #endregion

        #region Constructor

        public B010Processor()
        {
            Name = CProcessorName;
        }
        
        #endregion

        #region Override Methods

        #region CreateQueueItems

        public override void CreateQueueItems(int maxNumberToPoll)
        {
            try
            {

                // obtener los numeros de ordenes en estado paid.

                List<int> orderCustomerIds = Order.GetOrderCustomerIdsByStatus((int)Constants.OrderStatus.Paid, this.MinutesCount);

                if (orderCustomerIds.Count > 0)
                {
                    EnqueueItem(orderCustomerIds);
                }

            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }

        }

        #endregion

        #region ProcessQueueItem

        /// <summary>
        /// 
        /// </summary>
        public override void ProcessQueueItem(IEnumerable<int> orderCustomerIds)
        {
            string fileTemp = Path.GetFullPath(this.LogPath);

            #region creacion de filtros

            int UserID = 1;//CoreContext.CurrentUser.UserID;
            int LoteID = 0;//lote insertado
            DataTable dtOrderCustomerIDs = new DataTable();
            orderCustomerIds = orderCustomerIds.Distinct().ToList();
            dtOrderCustomerIDs.Columns.Add(
                        new DataColumn()
                        {
                            ColumnName = "OrderCustomerID",
                            DataType = typeof(int)
                        }
                                        );
            orderCustomerIds.Each((it) =>
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
                string pathTemplateClientsOrders = Path.GetFullPath(this.TemplateClientsOrders);
                string PathTemplateOrderItem = Path.GetFullPath(this.TemplateOrderItem);
                string PathMainTemplateClientsOrders = Path.GetFullPath(this.MainTemplateClientsOrders);
                #endregion

                string rutaLog = Path.Combine("Reports", "FilesTemp", "logSendFiles.txt");

                LoteID = GenerateBatchBusinessLogic.InsOrderInvoicesOrderInvoiceItems(dtOrderCustomerIDs, UserID, false, out OrderInvoiceIDIniPOut, out OrderInvoiceIDFinPOut);
                if (LoteID > 0)
                {
                    data = XmlGeneratorBusinessLogic.Instance.CrearOrderPedidoXml(PathMainTemplateClientsOrders, pathTemplateClientsOrders, PathTemplateOrderItem, LoteID, OrderInvoiceIDIniPOut, OrderInvoiceIDFinPOut, CultureInfo.CurrentCulture);
                }
                else
                {
                    throw new Exception("Error, try again");
                }

                string ListaOrdersNumber = data[0, 1];
                string XmlGeneradoTodasOrdenes = data[0, 0];

                string[] ArchivosCreados = new string[ListaOrdersNumber.Split(';').Length - 1];
                string[] ArchivosCreadosSap = new string[ListaOrdersNumber.Split(';').Length - 1];

                var fechaActual = DateTime.Now;

                string RutaUnica = Right(fechaActual.Year.ToString(), 2) + Right(("00" + fechaActual.Month.ToString()), 2) + Right(("00" + fechaActual.Day.ToString()), 2) + Right(("00" + fechaActual.Hour.ToString()), 2) + Right(("00" + fechaActual.Minute.ToString()), 2) + Right(("00" + fechaActual.Second.ToString()), 2);

                var fileNameSapXml = "MLM_Pedidos_B010_" + RutaUnica + ".xml";

                try
                {
                    FileHelper.Save(XmlGeneradoTodasOrdenes, Path.Combine(this.FileUploadXmlSap, fileNameSapXml));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                using (var file = System.IO.File.AppendText(Path.Combine(fileTemp, "logSendFiles.txt")))
                {
                    string fechaMs = DateTime.Now.ToString("yyyyMMddHHmmss");
                    file.WriteLine(string.Format("[Fecha Hora: {0}]", fechaMs));
                    file.WriteLine("Error [Message]" + ex.Message);
                    file.WriteLine("LoteID " + LoteID.ToString());
                    file.WriteLine("OrderCustomerIDs " + string.Join(",", orderCustomerIds.ToArray()));
                    file.WriteLine(string.Format("[Fecha Hora: {0}]", fechaMs));
                }
                throw ex;
            }
        }

        #endregion
        
        #endregion

        #region Functions

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
