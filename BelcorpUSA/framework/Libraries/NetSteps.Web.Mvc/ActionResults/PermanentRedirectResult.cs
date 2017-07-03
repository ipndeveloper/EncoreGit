using System;
using System.Web;
using System.Web.Mvc;

namespace NetSteps.Web.Mvc.ActionResults
{
	[Obsolete("Use System.Web.Mvc.RedirectResult with the parameter 'permanent' set to true.", true)]
	public class PermanentRedirectResult : ActionResult
	{
		public string Url { get; set; }

		public PermanentRedirectResult(string url)
		{
			if (string.IsNullOrEmpty(url))
			{
				throw new ArgumentNullException("url");
			}
			if (url.StartsWith("~"))
				url = VirtualPathUtility.ToAbsolute(url);
			this.Url = url;
		}

		public override void ExecuteResult(ControllerContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			context.HttpContext.Response.Clear();
			context.HttpContext.Response.BufferOutput = true;
			context.HttpContext.Response.StatusCode = 301;
			context.HttpContext.Response.RedirectLocation = Url;
			context.HttpContext.Response.End();
		}
	}
}
