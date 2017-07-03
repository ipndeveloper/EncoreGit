using System;
using System.Web.Mvc;

namespace NetSteps.Web.Mvc.ActionResults
{
	[Obsolete("Use System.Web.Mvc.HttpNotFoundResult.", true)]
	public class FileNotFoundResult : ActionResult
	{
		public override void ExecuteResult(ControllerContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			context.HttpContext.Response.Clear();
			context.HttpContext.Response.BufferOutput = true;
			context.HttpContext.Response.StatusCode = 404;
			context.HttpContext.Response.End();
		}
	}
}
