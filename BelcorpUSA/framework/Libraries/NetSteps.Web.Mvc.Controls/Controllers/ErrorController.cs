using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Diagnostics.Utilities;

namespace NetSteps.Web.Mvc.Controls.Controllers
{
	/// <summary>
	/// A utility controller for returning error responses.
	/// To avoid overhead, this controller should not derive from Encore base controllers.
	/// </summary>
	public class ErrorController : Controller
	{
		[NonAction]
		public virtual void Application_Error(HttpApplication app, EventArgs e)
		{
			using (var applicationErrorTrace = this.TraceActivity("trace Application_Error"))
			{
				Exception ex = null;
				NetStepsException nsEx = null;
				HttpContext context = null;
				HttpRequest request = null;
				HttpSessionState session = null;

				// Get info from context.
				if (app != null)
				{
					ex = app.Server.GetLastError();
					if (ex != null)
					{
						ex.TraceEvent(DateTime.Now.ToString() + "ERROR: " + ex.ToString());
						ex.TraceException(ex);
					}
					else
					{
						this.TraceError("Application_Error: exception was null");
					}

					context = app.Context;
					if (context != null)
					{
						request = context.Request;
						session = context.Session;
					}
				}
				else
				{
					this.TraceEvent("Application_Error: app was null");
				}

				// Log exception
				try
				{
					this.TraceEvent("trying to LogException");
					nsEx = LogException(ex, session);
				}
				catch
				{
					this.TraceEvent("error while in LogException");
				}

#if DEBUG
				// If debug, local & non-ajax, show yellow screen of death.
				if (request != null
					&& request.IsLocal
					&& !new HttpRequestWrapper(request).IsAjaxRequest())
				{
					if (context.Response != null)
					{
						context.Response.TrySkipIisCustomErrors = true;
					}
					return;
				}
#endif

				try
				{
					ExecuteErrorResponse(context, nsEx ?? ex);
				}
				catch
				{
					this.TraceError("error while in ExecuteErrorResponse");
				}
			}
		}

		public virtual ActionResult Index()
		{
			int? statusCode = RouteData.Values["statusCode"] as int?;
			Exception ex = RouteData.Values["ex"] as Exception;
			int? errorLogID = GetErrorLogID(ex);
			var httpStatusCode = GetHttpStatusCode(statusCode);

			if (Request.IsAjaxRequest())
			{
				string errorMessage = GetErrorMessage(ex);
				return new NetSteps.Web.Mvc.Restful.JsonError(httpStatusCode, errorMessage);
			}
			else
			{
				Response.StatusCode = (int)httpStatusCode;
				Response.TrySkipIisCustomErrors = true;
				ViewBag.ErrorLogID = errorLogID;
				return View();
			}
		}

		public virtual ActionResult SiteNotFound()
		{
			return Http404();
		}

		public virtual ActionResult SiteNotActive()
		{
			return Http404();
		}

		public virtual ActionResult Http404()
		{
			var httpStatusCode = HttpStatusCode.NotFound;
			if (Request.IsAjaxRequest())
			{
				return new NetSteps.Web.Mvc.Restful.JsonError(httpStatusCode);
			}
			else
			{
				Response.StatusCode = (int)httpStatusCode;
				Response.TrySkipIisCustomErrors = true;
				return View();
			}
		}

		public ActionResult LiveExplosivesTest(bool? IsNullRef, bool? IsViewError)
		{
			if (IsNullRef == true)
			{
				string fakeVar = null;
				fakeVar.Trim();
			}
			else if (IsViewError == true)
			{
				return View("LiveExplosivesTest");
			}

			throw new Exception("Oh Noes...BOOM!");
		}

		public ActionResult AsyncExplosivesTest()
		{
			throw new Exception("Oh Noes...Async BOOM!");
		}

		#region Helpers
		protected virtual NetStepsException LogException(Exception ex, HttpSessionState session)
		{
			// Don't log HttpExceptions (i.e. 404s).
			if (ex == null || ex is HttpException)
			{
				this.TraceError(string.Format("{1} LogException: ex was {0}", (ex == null) ? "null": "HttpException", DateTime.Now.ToString()));
				if (ex != null) this.TraceError(ex.ToString());
				return null;
			}

			int? orderID = null;
			int? accountID = null;

			// Get helpful values from session.
			if (session != null)
			{
				try
				{
					var order = (session["CurrentOrder"] ?? session["Order"]) as Order;
					if (order != null)
					{
						orderID = order.OrderID;
					}

					var account = (session["CurrentAccount"]) as Account;
					if (account != null)
					{
						accountID = account.AccountID;
					}
				}
				catch { }
			}

			// Log the exception.
			return ex.Log(orderID: orderID, accountID: accountID);
		}

		protected virtual void ExecuteErrorResponse(HttpContext context, Exception ex)
		{
			this.TraceEvent("entering ExecuteErrorResponse");

			if (context == null)
			{
				return;
			}

			context.Response.Clear();
			context.ClearError();

			// Include status code for HttpExceptions (i.e. 404s),
			// or default to 500.
			var httpException = ex as HttpException;
			int statusCode = httpException != null
				? httpException.GetHttpCode()
				: (int)HttpStatusCode.InternalServerError;

			var routeData = new RouteData();
			routeData.Values["controller"] = "Error";
			routeData.Values["action"] = statusCode == (int)HttpStatusCode.NotFound
				? "Http404"
				: "Index";
			routeData.Values["ex"] = ex;
			routeData.Values["statusCode"] = statusCode;

			this.Execute(new RequestContext(new HttpContextWrapper(context), routeData));
		}

		protected virtual string GetErrorMessage(Exception ex)
		{
			string errorMessage;

			var nsEx = ex as NetStepsException;
			if (nsEx != null)
			{
				errorMessage = nsEx.PublicMessage;
			}
			else if (ex != null)
			{
				errorMessage = ex.Message;
			}
			else
			{
				errorMessage = DefaultErrorMessage();
			}

			return errorMessage;
		}

		protected virtual int? GetErrorLogID(Exception ex)
		{
			var nsEx = ex as NetStepsException;
			if (nsEx != null)
			{
				var errorLog = nsEx.ErrorLog as ErrorLog;
				if (errorLog != null)
				{
					return errorLog.ErrorLogID;
				}
			}

			return null;
		}

		protected virtual HttpStatusCode GetHttpStatusCode(int? statusCode)
		{
			HttpStatusCode httpStatusCode;
			if (statusCode.HasValue
				&& Enum.TryParse<HttpStatusCode>(statusCode.Value.ToString(), out httpStatusCode))
			{
				return httpStatusCode;
			}

			return HttpStatusCode.InternalServerError;
		}

		public static string DefaultErrorMessage() { return Translation.GetTerm("ErrorText", "We apologize, but we are unable to handle your request at this time. Please try your request again later."); }
		#endregion
	}
}