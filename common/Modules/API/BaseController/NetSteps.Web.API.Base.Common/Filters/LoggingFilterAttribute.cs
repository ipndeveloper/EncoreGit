using System;
using System.Diagnostics;
using System.Web.Mvc;
using NetSteps.Encore.Core.Log;

namespace NetSteps.Web.API.Base.Common
{
	public class TraceFilterAttribute : ActionFilterAttribute, IExceptionFilter
	{
		ILogSink _log;

		void IExceptionFilter.OnException(ExceptionContext filterContext)
		{
			if (_log == null)
				_log = filterContext.Controller.GetType().GetLogSink();

			if (filterContext != null && filterContext.Exception != null)
			{
				if (_log.IsLogging(SourceLevels.Error))
				{
					_log.Error(HttpConstants.EventID.UncaughtException,
						String.Concat("OnException: ", filterContext.RequestContext.HttpContext.Request.RawUrl),
						filterContext.Exception);
				}
			}
		}

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			_log = filterContext.Controller.GetType().GetLogSink();
			if (_log.IsLogging(SourceLevels.Information))
			{
				_log.Information(HttpConstants.EventID.MvcActionExecuting, HttpConstants.EventID.MvcActionExecuting.ToString(),
					String.Concat("OnActionExecuting: ", filterContext.RequestContext.HttpContext.Request.RawUrl),
					new
					{
						Controller = filterContext.ActionDescriptor.ControllerDescriptor,
						ActionName = filterContext.ActionDescriptor.ActionName,
						ActionParameters = filterContext.ActionParameters
					});
			}
			base.OnActionExecuting(filterContext);
		}

		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			if (filterContext.Exception == null)
			{
				if (_log.IsLogging(SourceLevels.Information))
				{
					_log.Information(HttpConstants.EventID.MvcActionExecuted, HttpConstants.EventID.MvcActionExecuted.ToString(),
						String.Concat("OnActionExecuted: ", filterContext.RequestContext.HttpContext.Request.RawUrl)
						);
				}
			}
			else if (_log.IsLogging(SourceLevels.Error))
			{
				_log.Error(HttpConstants.EventID.MvcActionExecutedWithError,
					String.Concat("OnActionExecuted: ", filterContext.RequestContext.HttpContext.Request.RawUrl),
					filterContext.Exception
					);
			}

			base.OnActionExecuted(filterContext);
		}

		private object MakeResultObjectForTraceLog(ActionResult actionResult)
		{
			object resultObj;
			if (actionResult is ViewResultBase)
			{
				var vrb = actionResult as ViewResultBase;
				resultObj = new { ViewName = vrb.ViewName, TempData = vrb.TempData, ViewData = vrb.ViewData };
			}
			else
			{
				resultObj = actionResult;
			}
			return resultObj;
		}

		public override void OnResultExecuting(ResultExecutingContext filterContext)
		{
			if (_log == null)
				_log = filterContext.Controller.GetType().GetLogSink();

			if (_log.IsLogging(SourceLevels.Information))
			{
				_log.Information(HttpConstants.EventID.MvcResultExecuting, HttpConstants.EventID.MvcResultExecuting.ToString(),
					String.Concat("OnResultExecuting: ", filterContext.RequestContext.HttpContext.Request.RawUrl),
					MakeResultObjectForTraceLog(filterContext.Result)
					);
			}

			base.OnResultExecuting(filterContext);
		}

		public override void OnResultExecuted(ResultExecutedContext filterContext)
		{
			if (filterContext.Exception == null)
			{
				int statusCode = filterContext.HttpContext.Response.StatusCode;
				if (statusCode > 299)
				{
					_log.Warning(HttpConstants.EventID.MvcResultExecuted, HttpConstants.EventID.MvcResultExecuted.ToString(),
						String.Concat("OnResultExecuted: ", filterContext.RequestContext.HttpContext.Request.RawUrl),
						MakeResultObjectForTraceLog(filterContext.Result)
						);
				}
				else if (_log.IsLogging(SourceLevels.Information))
				{
					_log.Information(HttpConstants.EventID.MvcResultExecuted, HttpConstants.EventID.MvcResultExecuted.ToString(),
						String.Concat("OnResultExecuted: ", filterContext.RequestContext.HttpContext.Request.RawUrl)
						);
				}
			}
			else if (_log.IsLogging(SourceLevels.Error))
			{
				_log.Error(HttpConstants.EventID.MvcResultExecutedWithError, HttpConstants.EventID.MvcResultExecutedWithError.ToString(),
					String.Concat("OnResultExecuted: ", filterContext.RequestContext.HttpContext.Request.RawUrl),
					filterContext.Exception
					);
			}

			base.OnResultExecuted(filterContext);
		}
	}
}
