using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WcfOfiBridgeService.ServiceJsonContracts
{
    //WcfAssetService.ServiceContracts.IAssetServiceRequest 
    [ServiceContract]
    public interface IOfiBridgeJsonRequest
    {
        #region ADD

        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        BoolResultDC AddOFRequestQueueJson(OFRequestQueueDC requestQueue);

        #endregion

        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        bool KeepAlive();





    }
}