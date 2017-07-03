using System.Net;
using System;
using System.Runtime.Serialization;

namespace NetSteps.Web.API.Base.Common
{
	public sealed class JsonSuccess : JsonResult
	{
		public JsonSuccess(object result)
			: base(new SuccessResultEnvelope(result)) { }
		public JsonSuccess(string message)
			: base(new SuccessResultEnvelope(message)) { }
		public JsonSuccess(string message, object result)
			: base(new SuccessResultEnvelope(message, result)) { }
		public JsonSuccess(HttpStatusCode code, object result)
			: base(code, new SuccessResultEnvelope(result)) { }
		public JsonSuccess(HttpStatusCode code, string message, object result)
			: base(code, new SuccessResultEnvelope(message, result)) { }
	}

	[Serializable]
	public sealed class SuccessResultEnvelope
	{
		public SuccessResultEnvelope()
			: this("Ok", null)
		{
		}
		public SuccessResultEnvelope(object result)
			: this("Ok", result)
		{
		}
		public SuccessResultEnvelope(string message)
			: this(message, null)
		{
		}
		public SuccessResultEnvelope(string message, object result)
		{
			this.success = message;
			this.result = result;
		}
		public string success { get; set; }
		public object result { get; set; }
	}
}
