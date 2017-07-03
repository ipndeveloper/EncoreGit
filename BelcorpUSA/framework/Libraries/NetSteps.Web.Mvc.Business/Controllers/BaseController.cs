using System;
using System.IO;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Web.Mvc.ActionResults;
using NetSteps.Web.Mvc.Helpers;

namespace NetSteps.Web.Mvc.Business.Controllers
{
    /// <summary>
    /// Author: John Egbert
    /// Description: BaseController for shared functionality in multiple web apps
    /// Created: 10/07/2011
    /// </summary>
    public abstract class BaseController : Controller
    {
        /// <summary>
        /// This MessageHandler will be checked by the MessageCenter user control on both normal and asynchronous requests
        /// </summary>
        public static MessageHandler MessageHandler
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["MessageHandler"] == null)
                    System.Web.HttpContext.Current.Session["MessageHandler"] = new MessageHandler();
                return System.Web.HttpContext.Current.Session["MessageHandler"] as MessageHandler;
            }
        }
   
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Result is JsonResult)
            {
                var jResult = filterContext.Result as JsonResult;

                if (MessageHandler.HasAnyMessages && jResult.Data != null)
                {
                    jResult.Data = jResult.Data.AddProperty("MessageHandler", MessageHandler.Clone());
                    MessageHandler.Clear();
                }
            }

			if (!filterContext.IsChildAction && filterContext.Result is ViewResult)
			{
				//If the result is a ViewResult, we need to set the view data that is used by the master page
				SetViewData();
			}

			base.OnActionExecuted(filterContext);
		}

		/// <summary>
		/// Establishes default view data. Subclasses should override this method
		/// to provide any non view-model data to the view (such as environmental or
		/// context info).
		/// </summary>
		protected virtual void SetViewData()
		{
			// does nothing.
		}

		/// <summary>
		/// MVC 2.0 requires a request behavior, MVC 1.0 did not, so we will default the request behavior to AllowGet when it is not passed in
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		[NonAction]
		public new JsonResult Json(object data) { return base.Json(data, JsonRequestBehavior.AllowGet); }

		[NonAction]
		public virtual JsonBasicResult JsonSuccess() { return new JsonBasicResult(true); }

		[NonAction]
		public virtual JsonBasicResult JsonSuccess(string message) { return new JsonBasicResult(true, message); }

		[NonAction]
		public virtual JsonBasicResult JsonError() { return new JsonBasicResult(false); }

		[NonAction]
		public virtual JsonBasicResult JsonError(string message) { return new JsonBasicResult(false, message); }

		[NonAction]
		public virtual JsonBasicResult JsonBasic(BasicResponse basicResponse) { return new JsonBasicResult(basicResponse); }

		[NonAction]
		public virtual RedirectToRouteResult RedirectToAction(string actionName, string controllerName, string areaName) { return base.RedirectToRoute(new {action = actionName, controller = controllerName, area = areaName}); }

		[NonAction]
		public new virtual RedirectToRouteResult RedirectToRoute(object routeValues) { return base.RedirectToRoute(routeValues); }


		[NonAction]
		protected virtual string RenderPartialToString(string partialName, object model = null) { return RenderPartialToString(partialName, new ViewDataDictionary(this.ViewData), model); }

        [NonAction]
        protected virtual string RenderPartialToString(string partialName, ViewDataDictionary viewData, object model = null)
        {
            var vp = new ViewPage
            {
                ViewData = viewData,
                ViewContext = new ViewContext(),
                Url = new UrlHelper(this.ControllerContext.RequestContext)
            };

            ViewEngineResult result = ViewEngines.Engines.FindPartialView(this.ControllerContext, partialName);
			
			if (result.View == null)
            {
                throw new InvalidOperationException(string.Format("The partial view '{0}' could not be found", partialName));
            }
            var partialPath = ((WebFormView)result.View).ViewPath;

            vp.ViewData.Model = model;

            Control control = vp.LoadControl(partialPath);
            vp.Controls.Add(control);

            var sb = new StringBuilder();

            using (var sw = new StringWriter(sb))
            {
                using (var tw = new HtmlTextWriter(sw))
                {
                    vp.RenderControl(tw);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Probably works for non razor partial views too. In any case, the method above does not work for razor partial views.
        /// </summary>
        protected string RenderRazorPartialViewToString(string viewName, object model, ViewDataDictionary viewData = null)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            if (viewData != null)
                ViewData = viewData;

			using (StringWriter sw = new StringWriter())
			{
				ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
				ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View,
																									new ViewDataDictionary(ViewData) {Model = model}, TempData, sw);
				viewResult.View.Render(viewContext, sw);

				return sw.GetStringBuilder().ToString();
            }
        }

        //Supports the rendering of child partial views
        //http://stackoverflow.com/questions/2537741/how-to-render-partial-view-into-a-string/5801502#5801502
        [NonAction]
        protected virtual string RenderPartialToString(string viewName, object model, ControllerContext ControllerContext)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");
            ViewDataDictionary ViewData = new ViewDataDictionary();
            TempDataDictionary TempData = new TempDataDictionary();
            ViewData.Model = model;
            using (System.IO.StringWriter sw = new System.IO.StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                return sw.GetStringBuilder().ToString();
            }
        }

        protected string GetBrowserIpAddress()
        {
            string xForwardedFor = HttpContext.Request.Headers["X-Forwarded-For"];
            string httpXForwardedFor = HttpContext.Request.Headers["HTTP_X_FORWARDED_FOR"];

			var browserIPAddress =
				!String.IsNullOrWhiteSpace(xForwardedFor)
					? xForwardedFor
					: !String.IsNullOrWhiteSpace(httpXForwardedFor)
						? httpXForwardedFor
						: HttpContext.Request.UserHostAddress;

            return browserIPAddress;
        }
    }
}
