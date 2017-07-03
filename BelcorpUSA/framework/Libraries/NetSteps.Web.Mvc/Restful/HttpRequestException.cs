using System;
using System.Diagnostics.Contracts;
using System.Net;
using System.Runtime.Serialization;
using NetSteps.Web.Mvc.Properties;

namespace NetSteps.Web.Mvc.Restful
{
	[Serializable]
	public class HttpRequestException : ApplicationException
	{
		static readonly string StatusCodeOutOfRange = "HTTP Status Codes in the 2xx range indicate success and are inappropriate here.";

		public HttpRequestException(HttpStatusCode statusCode)
			: this((int)statusCode, 0, Convert.ToString(statusCode), null)
		{
			Contract.Requires<ArgumentOutOfRangeException>(statusCode < HttpStatusCode.OK || statusCode > (HttpStatusCode)299, StatusCodeOutOfRange);
		}

		public HttpRequestException(HttpStatusCode statusCode, object data)
			: this((int)statusCode, 0, Convert.ToString(statusCode), data)
		{
			Contract.Requires<ArgumentOutOfRangeException>(statusCode < HttpStatusCode.OK || statusCode > (HttpStatusCode)299, StatusCodeOutOfRange);
		}

		public HttpRequestException(HttpStatusCode statusCode, int statusSubCode, object data)
			: this((int)statusCode, statusSubCode, Convert.ToString(statusCode), data)
		{
			Contract.Requires<ArgumentOutOfRangeException>(statusCode < HttpStatusCode.OK || statusCode > (HttpStatusCode)299, StatusCodeOutOfRange);
		}

		public HttpRequestException(int statusCode, string statusDescription)
			: this((int)statusCode, 0, statusDescription, null)
		{
			Contract.Requires<ArgumentOutOfRangeException>(statusCode < 200 || statusCode > 299, StatusCodeOutOfRange);
			Contract.Requires<ArgumentNullException>(statusDescription != null, Resources.Chk_CannotBeNull);
		}

		public HttpRequestException(int statusCode, string statusDescription, object data)
			: this(statusCode, 0, statusDescription, data)
		{
			Contract.Requires<ArgumentOutOfRangeException>(statusCode < 200 || statusCode > 299, StatusCodeOutOfRange);
			Contract.Requires<ArgumentNullException>(statusDescription != null, Resources.Chk_CannotBeNull);
		}

		public HttpRequestException(int statusCode, int statusSubCode, string statusDescription, object data)
			: base(statusDescription)
		{
			Contract.Requires<ArgumentOutOfRangeException>(statusCode < 200 || statusCode > 299, StatusCodeOutOfRange);
			Contract.Requires<ArgumentNullException>(statusDescription != null, Resources.Chk_CannotBeNull);

			StatusCode = statusCode;
			StatusSubcode = statusSubCode;
			StatusDescription = statusDescription;
			ResultItem = data;
		}

		/// <summary>
		/// Used during serialization.
		/// </summary>
		/// <param name="si">SerializationInfo</param>
		/// <param name="sc">StreamingContext</param>
		protected HttpRequestException(SerializationInfo si, StreamingContext sc)
			: base(si, sc)
		{
		}

		public int StatusCode { get; private set; }
		public int StatusSubcode { get; private set; }
		public string StatusDescription { get; private set; }
		public object ResultItem { get; private set; }
	}

}
