using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;

namespace NetSteps.Web.API.Base.Common
{
	public enum FilterResultBehaviors
	{
		None = 0,
		SkipIisCustomErrors = 1,
		CustomResult = 2,
	}

	public abstract class BaseFilterAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var req = filterContext.HttpContext.Request;
			var res = filterContext.HttpContext.Response;

			if (FilterAppliesToRequest(req))
			{
				ActionResult customResult;
				FilterResultBehaviors behaviors = PerformFilter(req, res, out customResult);
				if (behaviors.HasFlag(FilterResultBehaviors.SkipIisCustomErrors))
				{
					res.TrySkipIisCustomErrors = true;
				}
				if (behaviors.HasFlag(FilterResultBehaviors.CustomResult))
				{
					filterContext.Result = customResult;
					return;
				}
			}
			base.OnActionExecuting(filterContext);
		}

		protected abstract FilterResultBehaviors PerformFilter(HttpRequestBase req, HttpResponseBase res, out ActionResult customResult);

		protected abstract bool FilterAppliesToRequest(HttpRequestBase req);
	}
}
