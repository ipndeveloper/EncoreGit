using System;
using System.Net;
using System.Web.Mvc;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Process;
using NetSteps.Web.Mvc.Restful;

namespace NetSteps.Web.Mvc.Filters
{	
	public class ApiAccessKeyFilterAttribute : ActionFilterAttribute
	{
		public static readonly string AccessKeyHeader = "X-NS-ACCESS-KEY";
		
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var req = filterContext.HttpContext.Request;
			var res = filterContext.HttpContext.Response;

			var id = req.Headers[AccessKeyHeader];

			if (String.IsNullOrEmpty(id))
			{
				res.TrySkipIisCustomErrors = true;
				filterContext.Result = new JsonError(HttpStatusCode.Forbidden, "Access key missing.");
				return;
			}

			Guid guid;
			if (!Guid.TryParse(id, out guid))
			{
				res.TrySkipIisCustomErrors = true;
				filterContext.Result = new JsonError(HttpStatusCode.BadRequest, "Access key cannot be parsed.");
				return;
			}

			using (var c = Create.SharedOrNewContainer())
			{
				try
				{
					var svc = c.New<IApiAccessKeyValidationService>();
					var processID = c.New<IProcessIdentity>();
					var result = svc.VerifyAccess(processID, guid);
					switch (result.Kind)
					{
						case ApiAccessKind.Disabled:
							res.TrySkipIisCustomErrors = true;
							filterContext.Result = new JsonError(HttpStatusCode.Forbidden, "API access disabled; please call customer support.");
							return;
						case ApiAccessKind.Full:
							if (result.Attachement != null)
								filterContext.HttpContext.Items[AccessKeyHeader] = result.Attachement;
							break;
						default:
							res.TrySkipIisCustomErrors = true;
							filterContext.Result = new JsonError(HttpStatusCode.Forbidden, "Access key invalid.");
							return;							
					}
				}
				catch (Exception)
				{
					res.TrySkipIisCustomErrors = true;
					filterContext.Result = new JsonError(HttpStatusCode.InternalServerError, "Unable to verify API access.");
					return;
				}
			}
			base.OnActionExecuting(filterContext);
		}
	}
}
