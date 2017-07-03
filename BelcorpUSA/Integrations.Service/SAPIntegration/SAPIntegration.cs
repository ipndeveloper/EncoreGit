using System.ServiceModel;
using NetSteps.Integrations.Service.Interfaces;
using System;
using System.Xml.Linq;
using System.Xml;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using NetSteps.Data.Entities.Business.Logic;
using NetSteps.Common.Configuration;
using NetSteps.Data.Entities.Utility;
using System.IO;
using System.Reflection;

/*
 * @01 20150820 BR-E020 CSTI JMO: Added MLM_CDespacho_E0020 Method
 */

namespace NetSteps.Integrations.Service
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall, Name = "sapIntegration")]
    public class SAPIntegration : ISAPIntegration
    {
        /// <summary>
        /// Developed By MAM - CSTI
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public string MLM_Picking_B055(string xml)
        {
            string XmlDoc = XmlGeneratorBusinessLogic.Instance.MLM_Picking_B055(xml);
            return XmlDoc;
        }

        public  string MLM_MaterialesE_B200(string stringXML)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(stringXML);
            var XmlString = XmlGeneratorBusinessLogic.MLM_MaterialesE_B200(stringXML);
            var ruta = ConfigurationManager.AppSettings["FileUploadAbsolutePath"];
            var B200_log = ConfigurationManager.AppSettings["FileUploadPath_B200_Log"];
            var B200_int = ConfigurationManager.AppSettings["FileUploadPath_B200_Int"];
            var B200_hist = ConfigurationManager.AppSettings["FileUploadPath_B200_Hist"];

            byte[] bytesLog = System.Text.ASCIIEncoding.ASCII.GetBytes(XmlString[0].ToString());
            using (MemoryStream ms = new MemoryStream(bytesLog))
            {
                FileHelper.Save(ms, ruta + B200_log + "log.xml");
            }

            byte[] bytesHist = System.Text.ASCIIEncoding.ASCII.GetBytes(XmlString[1].ToString());
            using (MemoryStream ms = new MemoryStream(bytesHist))
            {
                FileHelper.Save(ms, ruta + B200_hist + "hist.xml");
            }

            XDocument x = new XDocument();

            byte[] bytesInt = System.Text.ASCIIEncoding.ASCII.GetBytes(xml.InnerXml);
            using (MemoryStream ms = new MemoryStream(bytesInt))
            {
                FileHelper.Save(ms, ruta + B200_int + "int.xml");
            }

            return XmlString[0].ToString();
        }

        /// <summary>
        /// Developed By KLC - CSTI
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public string MLM_FacturacionE_B070(string xml)
        {            
           string XmlDoc = XmlGeneratorBusinessLogic.Instance.MLM_FacturacionE_B070(xml);
           return XmlDoc;                
        }

        public string MLM_CtaCteE_B090(string fechaMov)
        {
            string XmlDoc = XmlGeneratorBusinessLogic.Instance.MLM_CtaCteE_B090(fechaMov);
            return XmlDoc;
        }

        public string MLM_ConfirmacionPagos_B160(string stringXML)
        {
            string XmlDoc = XmlGeneratorBusinessLogic.Instance.ConfirmacionPagosB160(stringXML);
            return XmlDoc;
        }

        #region [@01 A01]

        public string MLM_CDespacho_E020(string stringXML)
        {
            return XmlGeneratorBusinessLogic.Instance.ConfirmacionDespachoGKO(stringXML);
        }

        #endregion

        public string MLM_COcurrencia_E030(string stringXML)
        {
            return XmlGeneratorBusinessLogic.Instance.ConfirmacionOcurrenciasGKO(stringXML);
        }

        public string MLM_SALDOSE_B020(string stringXML)
        {
            string XmlString = XmlGeneratorBusinessLogic.Instance.GenerateXmlB020(stringXML);
            //FileHelper.Save(XmlString, string.Format("{0}{1}B020{2}.xml", ConfigurationManager.AppSettings["FileUploadAbsolutePath"], ConfigurationManager.AppSettings["FileUploadPath_B020_Log"]));
            return XmlString;
        } 

        public string MLM_ComisionesE_B150(int? period)
        {
            return XmlGeneratorBusinessLogic.Instance.MLM_ComisionesE_B150(period); 
        } 

        public string CrearXmlBDI(string Contenido, string Ruta)
        {
            try
            {
                FileHelper.Save(Contenido, Ruta);
                if (File.Exists(Ruta))
                {
                    return "Ok";
                }
                return "Error";
            }
            catch (Exception ex)
            {
                return ex.Message;
                throw ex;
            }
            
        } 

        public string MLM_GeneraciónDeTxtCadastro_B155(int? period)
        {
            return XmlGeneratorBusinessLogic.Instance.MLM_GeneraciónDeTxtCadastro_B155(period); 
        }

        public string MLM_E080_WSAutenticacion(string stringXML)
        {
            return XmlGeneratorBusinessLogic.Instance.MLM_E080_WSAutenticacion(stringXML);
        }

        public string MLM_E080_WSDatosConsultores(string stringXML)
        {
            return XmlGeneratorBusinessLogic.Instance.MLM_E080_WSDatosConsultores(stringXML);
        }
    }
}
