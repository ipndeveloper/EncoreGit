
namespace NetSteps.Web.Mvc
{
	public static class HttpConstants
	{
		public static readonly string DatabaseConnectivity = "DatabaseConnectivity";

		public static readonly string MimeType_ApplicationJSON = "application/json";
		public static readonly string MimeType_ApplicationXML = "application/xml";
		public static readonly string MimeType_TextXML = "text/xml";
		public static readonly string MimeType_TextJSON = "text/json";

		public static readonly string Error_400 = "Bad request.";
		public static readonly string Error_401 = "Unauthorized.";
		public static readonly string Error_402 = "Payment Required.";
		public static readonly string Error_403 = "Forbidden.";
		public static readonly string Error_404 = "Resource not found.";
		public static readonly string Error_404_Entity = "Not found. If the entity was recently accepted please retry later.";
		public static readonly string Error_406 = "Client must provide content as JSON (mime-type: application/json) or XML (mime-type: application/xml).";
		public static readonly string Error_409 = "The operation conflicts with other activity on the server.";
		public static readonly string Error_409_5 = "The operation was unable to complete due to conflicting activity on the server; please resubmit.";
		public static readonly string Error_410 = "The resource is no longer available.";
		public static readonly string Error_415 = "Unsupported media type.";
		public static readonly string Error_422 = "Unable to process posted data; content must match the declared Content-Type and have the appropriate structure. Please refer to the API documentation for more info.";
		public static readonly string Error_429 = "Your endpoint is being rate-limited because we have received too many requests in a short period of time.";
		public static readonly string Error_500 = "Server error processing your request.";
		public static readonly string Error_501 = "Not implemented.";
		public static readonly string Error_503 = "Service unavailable.";
		public static readonly string Error_504 = "Gateway timeout.";

		public static readonly string Error_DependentService = "A dependent service is unreachable.";

		public static readonly string VersionString = typeof(HttpConstants).Assembly.GetName().Version.ToString();

		public static readonly object TransactionAborted = new { Kind = "TransactionAbort" };

		public static class EventID
		{
			public const int MvcActionExecuting = 10001;
			public const int MvcActionExecuted = 10002;
			public const int MvcActionExecutedWithError = 10003;
			public const int MvcResultExecuting = 10004;
			public const int MvcResultExecuted = 10005;
			public const int MvcResultExecutedWithError = 10006;

			public const int MvcPostDataReceived = 10101;

			public const int EntityAlreadyExists = 99096;
			public const int TransactionAborted = 99097;
			public const int UnexpectedException = 99098;
			public const int UncaughtException = 99099;
		}

		public static class HttpStatusCodes
		{
			public const int UnproccessableEntity = 422;
			public const int TooManyRequests = 429;
		}
		public static class HttpSubstatusCodes
		{
			public const int Conflict409_ResubmitRequired = 5;
		}
	}
}
