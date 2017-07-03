using System;
using System.Net;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace NetSteps.Web.Mvc.Restful
{
	public class JsonResult : ActionResult
	{
		JsonSerializerSettings __settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };

		public int StatusCode { get; private set; }
		public int StatusSubcode { get; private set; }
		public string StatusDescription { get; private set; }
		public object ResultItem { get; private set; }

		Formatting _formatting;

		protected JsonResult(object item)
			: this(HttpStatusCode.OK, String.Empty, item, false) { }
		protected JsonResult(HttpStatusCode statusCode, object item)
			: this(statusCode, String.Empty, item, false) { }
		protected JsonResult(HttpStatusCode statusCode, string description, object item)
			: this(statusCode, description, item, false) { }
		protected JsonResult(object item, bool prettyPrint)
			: this(HttpStatusCode.OK, String.Empty, item, prettyPrint) { }
		protected JsonResult(HttpStatusCode statusCode, object item, bool prettyPrint)
			: this(statusCode, String.Empty, item, prettyPrint) { }
		protected JsonResult(HttpStatusCode statusCode, string description, object item, bool prettyPrint)
			: this(statusCode, 0, description, item, prettyPrint)
		{
		}
		protected JsonResult(HttpStatusCode statusCode, int statusSubcode, string description, object item, bool prettyPrint)
		{
			this.StatusCode = (int)statusCode;
			StatusSubcode = statusSubcode;
			StatusDescription = description;
			ResultItem = item;
			_formatting = (prettyPrint) ? Formatting.Indented : Formatting.None;
		}

		public override void ExecuteResult(ControllerContext context)
		{
			var res = context.HttpContext.Response;
			var status = this.StatusCode;
			res.StatusCode = status;
			if (status < 200 || status > 299)
			{
				res.TrySkipIisCustomErrors = true;
			}
			if (StatusSubcode != 0)
			{
				res.SubStatusCode = StatusSubcode;
			}
			if (!String.IsNullOrEmpty(StatusDescription))
			{
				res.StatusDescription = StatusDescription;
			}
			res.Clear();
			res.ContentType = HttpConstants.MimeType_ApplicationJSON;
			res.Output.Write(JsonConvert.SerializeObject(ResultItem, _formatting, __settings));
		}
	}	
}
