using System.ServiceModel;
using System.Xml;

/*
 * @01 20150820 BR-E020 CSTI JMO: Added MLM_CDespacho_E0020 Method
 */

namespace NetSteps.Integrations.Service.Interfaces
{
    [ServiceContract(Name = "sapIntegration", Namespace = "netSteps.sapIntegration")]
    public interface ISAPIntegration
    {
        /// <summary>
        /// Developed By MAM - CSTI
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        [OperationContract]
        string MLM_Picking_B055(string xml);

        [OperationContract]
        string MLM_MaterialesE_B200(string xml);

        /// <summary>
        /// Developed By KLC - CSTI
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>  
        [OperationContract]
        string MLM_FacturacionE_B070(string xml);

        #region [@01 A01]

        [OperationContract]
        string MLM_CDespacho_E020(string stringXML);

        #endregion

        //E030-LIB
        [OperationContract]
        string MLM_COcurrencia_E030(string stringXML);
        
        [OperationContract]
        string MLM_SALDOSE_B020(string stringXML);
         
        [OperationContract]
        string MLM_ComisionesE_B150(int? period);
        
        [OperationContract]
        string CrearXmlBDI(string contenido, string Ruta);
        
        [OperationContract]
        string MLM_GeneraciónDeTxtCadastro_B155(int? period);

        [OperationContract]
        string MLM_CtaCteE_B090(string fechaMov);

        [OperationContract]
        string MLM_ConfirmacionPagos_B160(string stringXML);

        [OperationContract]
        string MLM_E080_WSAutenticacion(string stringXML);

        [OperationContract]
        string MLM_E080_WSDatosConsultores(string stringXML);
    }
}
