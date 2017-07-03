using System.Web.Mvc;

namespace nsDistributor.Controllers.Attributes
{
	internal class AuthenticationFilter : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			ActionResult result = (filterContext.Controller as BaseController).CheckOwnerAuthentication();
			if (result != null)
			{
				filterContext.HttpContext.Session["ReturnUrl"] = filterContext.HttpContext.Request.Url.AbsoluteUri;
				filterContext.Result = result;
			}
		}
	}
}
