using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Net;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Process;

namespace NetSteps.Web.API.Base.Common
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
					//var processID = c.New<IProcessIdentity>();
					var result = svc.VerifyAccess(guid);
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
				catch (Exception e)
				{
					res.TrySkipIisCustomErrors = true;
					filterContext.Result = new JsonError(HttpStatusCode.InternalServerError, "Unable to verify API access: " + e.Message);
					return;
				}
			}
			base.OnActionExecuting(filterContext);
		}
	}
}
