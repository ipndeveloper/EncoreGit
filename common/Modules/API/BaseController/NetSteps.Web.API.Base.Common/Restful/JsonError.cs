using System.Net;
using System;
using System.Runtime.Serialization;

namespace NetSteps.Web.API.Base.Common
{
	public sealed class JsonError : JsonResult
	{
		public JsonError(object reason)
			: base(HttpStatusCode.InternalServerError, new ErrorResultEnvelope(reason)) { }
		public JsonError(string message)
			: base(HttpStatusCode.InternalServerError, new ErrorResultEnvelope(message)) { }
		public JsonError(string message, object reason)
			: base(HttpStatusCode.InternalServerError, new ErrorResultEnvelope(message, reason)) { }
		public JsonError(HttpStatusCode code, object reason)
			: base(code, new ErrorResultEnvelope(reason)) { }
		public JsonError(HttpStatusCode code, string message)
			: base(code, new ErrorResultEnvelope(message)) { }
		public JsonError(HttpStatusCode code, string message, object reason)
			: base(code, new ErrorResultEnvelope(message, reason)) { }
		public JsonError(HttpStatusCode code, int statusSubcode, string message, object reason)
			: base(code, statusSubcode, null, new ErrorResultEnvelope(message, reason), false) { }
	}

	[Serializable]
	public sealed class ErrorResultEnvelope
	{
		public ErrorResultEnvelope()
			: this("unexpected error", null)
		{
		}
		public ErrorResultEnvelope(object reason)
			: this("unexpected error", reason)
		{
		}
		public ErrorResultEnvelope(string message, object result)
		{
			this.error = message;
			this.reason = result;
		}
		public string error { get; set; }
		public object reason { get; set; }
	}

}
