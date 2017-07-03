using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using NetSteps.WebService.Mobile.Models;

namespace NetSteps.WebService.Mobile
{
	[ServiceContract]
	public interface IMobileService
	{
		[AuthFilter]
		[OperationContract]
		[WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, 
            UriTemplate = "/GetNews?lastUpdate={lastUpdate}")]
		List<NewsModel> GetNews(string lastUpdate);

		[AuthFilter]
		[OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/GetCustomersPaginated?start={start}&count={count}&filter={filter}&sort={sort}")]
		List<ContactModel> GetCustomersPaginated(int start, int count, string filter, string sort);

		[AuthFilter]
		[OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/GetProspectsPaginated?start={start}&count={count}&filter={filter}&sort={sort}")]
        List<ContactModel> GetProspectsPaginated(int start, int count, string filter, string sort);

        [AuthFilter]
        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/GetTeamPaginated?periodID={periodID}&start={start}&count={count}&filter={filter}&sort={sort}")]
        List<ContactModel> GetTeamPaginated(string periodID, int start, int count, string filter, string sort);

        [AuthFilter]
        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/GetDownlinePaginated?periodID={periodID}&start={start}&count={count}&filter={filter}&sort={sort}")]
        List<ContactModel> GetDownlinePaginated(string periodID, int start, int count, string filter, string sort);

		[AuthFilter]
		[OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/GetPerformance?incremental={incremental}")]
		List<KPIModel> GetPerformance(bool incremental = false);

        [AuthFilter]
        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/GetPVOrdersPaginated?periodID={periodID}&start={start}&count={count}&filter={filter}&sort={sort}")]
        List<OrderModel> GetPVOrdersPaginated(string periodID = null, int start = 0, int count = 25, string filter = "", string sort = "");

        [AuthFilter]
        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/GetGVOrdersPaginated?periodID={periodID}&start={start}&count={count}&filter={filter}&sort={sort}")]
        List<OrderModel> GetGVOrdersPaginated(string periodID = null, int start = 0, int count = 25, string filter = "", string sort = "");

		[AuthFilter]
		[OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/GetOrders?lastUpdate={lastUpdate}")]
		List<OrderModel> GetOrders(string lastUpdate);

		[AuthFilter]
		[OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/GetPartyOrders")]
		List<OrderModel> GetPartyOrders();

		[OperationContract]
		[WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
		LoginModel Login(string username, string password);

		[OperationContract]
		[WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, 
            UriTemplate = "/RegisterDevice?deviceid={deviceid}&devicetype={devicetype}&country={country}&language={language}&accountnumber={accountnumber}")]
		DeviceModel RegisterDevice(string deviceid, DeviceType devicetype, string country, string language, string accountnumber);

		[OperationContract]
		[WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, 
            UriTemplate = "/UpdateDevice?deviceid={deviceid}&devicetype={devicetype}&country={country}&language={language}&accountnumber={accountnumber}&active={active}")]
		DeviceModel UpdateDevice(string deviceid, DeviceType devicetype, string country, string language, string accountnumber, bool active);

		[OperationContract]
		[WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/GetTerms")]
		TranslationModel GetTerms();

        #region Option Request Hacks
        // Until we find a better way, create a void implementation for each endpoint to handle OPTIONS requests so your normal requests dont get called twice
        [OperationContract]
        [WebInvoke(Method = "OPTIONS", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/GetNews")]
        void GetNews_Options();

        [OperationContract]
        [WebInvoke(Method = "OPTIONS", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/RegisterDevice")]
        void RegisterDevice_Options();

        [OperationContract]
        [WebInvoke(Method = "OPTIONS", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/UpdateDevice")]
        void UpdateDevice_Options();

        [OperationContract]
        [WebInvoke(Method = "OPTIONS", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/GetTerms")]
        void GetTerms_Options();
        
        [OperationContract]
        [WebInvoke(Method = "OPTIONS", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/GetCustomersPaginated")]
        void GetCustomersPaginated_Options();

        [OperationContract]
        [WebInvoke(Method = "OPTIONS", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/GetProspectsPaginated")]
        void GetProspectsPaginated_Options();

        [OperationContract]
        [WebInvoke(Method = "OPTIONS", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/GetTeamPaginated")]
        void GetTeamPaginated_Options();

        [OperationContract]
        [WebInvoke(Method = "OPTIONS", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/GetDownlinePaginated")]
        void GetDownlinePaginated_Options();
        
        [OperationContract]
        [WebInvoke(Method = "OPTIONS", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/GetPerformance")]
        void GetPerformance_Options();

        [OperationContract]
        [WebInvoke(Method = "OPTIONS", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/GetPVOrdersPaginated")]
        void GetPVOrdersPaginated_Options();

        [OperationContract]
        [WebInvoke(Method = "OPTIONS", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/GetGVOrdersPaginated")]
        void GetGVOrdersPaginated_Options();
        
        [OperationContract]
        [WebInvoke(Method = "OPTIONS", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/GetOrders")]
        void GetOrders_Options();
        
        [OperationContract]
        [WebInvoke(Method = "OPTIONS", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/GetPartyOrders")]
        void GetPartyOrders_Options();
        #endregion
	}
}
