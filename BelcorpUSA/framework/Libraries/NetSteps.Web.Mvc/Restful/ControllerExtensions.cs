using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Net;
using System.Web.Mvc;
using NetSteps.Diagnostics.Logging.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Log;
using NetSteps.Encore.Core.Reflection;
using NetSteps.Encore.Core.Representation;
using NetSteps.Web.Mvc.Validation;
using Newtonsoft.Json;

namespace NetSteps.Web.Mvc.Restful
{

    public static class ControllerExtensions
	{
		static readonly ILogSink __log = typeof(ControllerExtensions).GetLogSink();
		public static TInput ValidatePostBodyAs<TInput>(this Controller controller)
		{
			return ValidatePostBodyAs<TInput>(controller, null, null, null);
		}
		public static TInput ValidatePostBodyAs<TInput>(this Controller controller, IInputValidator<TInput> validator) {
		
			return ValidatePostBodyAs<TInput>(controller, validator, null, null);
		}
		public static TInput ValidatePostBodyAs<TInput>(this Controller controller, IInputValidator<TInput> validator, string ietfLanguageTag)
		{
			return ValidatePostBodyAs<TInput>(controller, validator, ietfLanguageTag, null);
		}		
		public static TInput ValidatePostBodyAs<TInput>(this Controller controller, IInputValidator<TInput> validator, IInputValidationErrorLookupService errorLookup)
		{
			return ValidatePostBodyAs<TInput>(controller, validator, null, errorLookup);
		}
		public static TInput ValidatePostBodyAs<TInput>(this Controller controller, IInputValidator<TInput> validator, string ietfLanguageTag, IInputValidationErrorLookupService errorLookup)
		{
			var encoding = default(ResultEncoding);
			var incomming = default(TInput);
			try
			{
				if (controller.Request.IsContentType(HttpConstants.MimeType_ApplicationJSON)
					|| controller.Request.IsContentType(HttpConstants.MimeType_TextJSON))
				{
					if (controller.CanAcceptJson())
					{
						if (controller.TryDeserializePostData<TInput>(out encoding, out incomming))
						{
							if (validator != null)
							{
								var lang = ietfLanguageTag ?? "us-en";
								var lookup = errorLookup ?? Create.New<IInputValidationErrorLookupService>(LifespanTracking.Automatic);								
								var collector = new ValidationErrorCollector(lang, lookup);
								if (!validator.IsValid(incomming, collector))
								{
									throw new HttpRequestException(422, HttpConstants.Error_422, collector.Errors);
								}
							}
							return incomming;
						}
						else
						{
							throw new HttpRequestException(HttpStatusCode.BadRequest);
						}
					}
					else
					{
						throw new HttpRequestException(HttpStatusCode.NotAcceptable);
					}
				}
				else
				{
					throw new HttpRequestException(HttpStatusCode.UnsupportedMediaType, "Content-Type must be 'application/json'");
				}
			}
			catch (JsonSerializationException jsonException)
			{
				throw new HttpRequestException(HttpStatusCode.BadRequest, jsonException.Message);
			}
		}

		public static ActionResult ProtectedResourceAction(this Controller c, Func<IContainer, ActionResult> action)
		{
			return ProtectedResourceAction(c, action, false);
		}
		public static ActionResult ProtectedResourceAction(this Controller c, Func<IContainer, ActionResult> action, bool showStackTraceOnUnexpectedError)
		{
			try
			{
				using (var container = Create.SharedOrNewContainer())
				{
					return action(container);
				}
			}
			catch (HttpRequestException rex)
			{
				var logger = Create.New<ILogResolver>();
				logger.LogException(rex, rex.Message, true);
				return new JsonError((HttpStatusCode)rex.StatusCode, rex.StatusSubcode, rex.Message, rex.ResultItem);
			}
			catch (Exception e)
			{
				var logger = Create.New<ILogResolver>();
				logger.LogException(e, e.Message, true);
				return c.Result_500_InternalServerError(e.ToString()/*e.Message*/, e.FormatForLogging(showStackTraceOnUnexpectedError));
			}
		}

		private static ActionResult ErrorResult(this Controller controller, HttpStatusCode statusCode, string error, object reason)
		{
			Contract.Requires(error != null, "error cannot be null");

			if (reason is Exception)
			{
				//if (controller.Request.IsLocal)
				//    return new JsonError(statusCode, error, (reason as Exception).FormatForLogging());
				//else
				//    return new JsonError(statusCode, error);
				return new JsonError(statusCode, error, (reason as Exception).StackTrace);
			}

			return new JsonError(statusCode, error, reason);
		}

		private static ActionResult ErrorResult(this Controller controller, HttpStatusCode statusCode, int statusSubcode, string error, object reason)
		{
			Contract.Requires(error != null, "error cannot be null");
			if (reason is Exception)
			{
				if (controller.Request.IsLocal)
					return new JsonError(statusCode, statusSubcode, error, (reason as Exception).StackTrace);
				else
					return new JsonError(statusCode, statusSubcode, error, null);
			}
			else
			{
				return new JsonError(statusCode, statusSubcode, error, reason);
			}
		}

		public static ActionResult Result_200_OK(this Controller controller)
		{
			return new JsonSuccess(HttpStatusCode.OK, "OK");
		}
		public static ActionResult Result_200_OK(this Controller controller, string msg)
		{
			return new JsonSuccess(HttpStatusCode.OK, msg);
		}
		public static ActionResult Result_200_OK(this Controller controller, object reason)
		{
			return new JsonSuccess(HttpStatusCode.OK, "OK", reason);
		}
		public static ActionResult Result_200_OK(this Controller controller, string msg, object reason)
		{
			return new JsonSuccess(HttpStatusCode.OK, msg, reason);
		}

		public static ActionResult Result_201_Created(this Controller controller, string location)
		{
			Contract.Requires<ArgumentNullException>(location != null);
			Contract.Requires<ArgumentNullException>(location.Length > 0);
			controller.Response.AddHeader("Location", location);
			return new JsonSuccess(HttpStatusCode.Created, "Created");
		}
		public static ActionResult Result_201_Created(this Controller controller, string location, string msg)
		{
			Contract.Requires<ArgumentNullException>(location != null);
			Contract.Requires<ArgumentNullException>(location.Length > 0);
			controller.Response.AddHeader("Location", location);
			return new JsonSuccess(HttpStatusCode.Created, msg);
		}
		public static ActionResult Result_201_Created(this Controller controller, string location, object reason)
		{
			Contract.Requires<ArgumentNullException>(location != null);
			Contract.Requires<ArgumentNullException>(location.Length > 0);
			controller.Response.AddHeader("Location", location);
			return new JsonSuccess(HttpStatusCode.Created, "Created", reason);
		}
		public static ActionResult Result_201_Created(this Controller controller, string location, string msg, object reason)
		{
			Contract.Requires<ArgumentNullException>(location != null);
			Contract.Requires<ArgumentNullException>(location.Length > 0);
			controller.Response.AddHeader("Location", location);
			return new JsonSuccess(HttpStatusCode.Created, msg, reason);
		}

		public static ActionResult Result_202_Accepted(this Controller controller)
		{
			return new JsonSuccess(HttpStatusCode.Accepted, "Accepted");
		}
		public static ActionResult Result_202_Accepted(this Controller controller, string msg)
		{
			return new JsonSuccess(HttpStatusCode.Accepted, msg);
		}
		public static ActionResult Result_202_Accepted(this Controller controller, object reason)
		{
			return new JsonSuccess(HttpStatusCode.Accepted, "Accepted", reason);
		}
		public static ActionResult Result_202_Accepted(this Controller controller, string msg, object reason)
		{
			return new JsonSuccess(HttpStatusCode.Accepted, msg, reason);
		}

		public static ActionResult Result_204_NoContent(this Controller controller)
		{
			return new JsonSuccess(HttpStatusCode.NoContent, "NoContent");
		}
		public static ActionResult Result_204_NoContent(this Controller controller, string msg)
		{
			return new JsonSuccess(HttpStatusCode.NoContent, msg);
		}
		public static ActionResult Result_204_NoContent(this Controller controller, object reason)
		{
			return new JsonSuccess(HttpStatusCode.NoContent, "NoContent", reason);
		}
		public static ActionResult Result_204_NoContent(this Controller controller, string msg, object reason)
		{
			return new JsonSuccess(HttpStatusCode.NoContent, msg, reason);
		}

		public static ActionResult Result_400_BadRequest(this Controller controller)
		{
			return controller.ErrorResult(HttpStatusCode.BadRequest, HttpConstants.Error_400, null);
		}
		public static ActionResult Result_400_BadRequest(this Controller controller, string msg)
		{
			return controller.ErrorResult(HttpStatusCode.BadRequest, msg, null);
		}
		public static ActionResult Result_400_BadRequest(this Controller controller, object reason)
		{
			return controller.ErrorResult(HttpStatusCode.BadRequest, HttpConstants.Error_400, reason);
		}
		public static ActionResult Result_400_BadRequest(this Controller controller, string msg, object reason)
		{
			return controller.ErrorResult(HttpStatusCode.BadRequest, msg, reason);
		}

		public static ActionResult Result_401_Unauthorized(this Controller controller)
		{
			return controller.ErrorResult(HttpStatusCode.Unauthorized, HttpConstants.Error_401, null);
		}
		public static ActionResult Result_401_Unauthorized(this Controller controller, string msg)
		{
			return controller.ErrorResult(HttpStatusCode.Unauthorized, msg, null);
		}
		public static ActionResult Result_401_Unauthorized(this Controller controller, object reason)
		{
			return controller.ErrorResult(HttpStatusCode.Unauthorized, HttpConstants.Error_401, reason);
		}
		public static ActionResult Result_401_Unauthorized(this Controller controller, string msg, object reason)
		{
			return controller.ErrorResult(HttpStatusCode.Unauthorized, msg, reason);
		}

		public static ActionResult Result_402_PaymentRequired(this Controller controller)
		{
			return controller.ErrorResult(HttpStatusCode.PaymentRequired, HttpConstants.Error_402, null);
		}
		public static ActionResult Result_402_PaymentRequired(this Controller controller, string msg)
		{
			return controller.ErrorResult(HttpStatusCode.PaymentRequired, msg, null);
		}
		public static ActionResult Result_402_PaymentRequired(this Controller controller, object reason)
		{
			return controller.ErrorResult(HttpStatusCode.PaymentRequired, HttpConstants.Error_402, reason);
		}
		public static ActionResult Result_402_PaymentRequired(this Controller controller, string msg, object reason)
		{
			return controller.ErrorResult(HttpStatusCode.PaymentRequired, msg, reason);
		}

		public static ActionResult Result_403_Forbidden(this Controller controller)
		{
			return controller.ErrorResult(HttpStatusCode.Forbidden, HttpConstants.Error_403, null);
		}
		public static ActionResult Result_403_Forbidden(this Controller controller, string msg)
		{
			return controller.ErrorResult(HttpStatusCode.Forbidden, msg, null);
		}
		public static ActionResult Result_403_Forbidden(this Controller controller, object reason)
		{
			return controller.ErrorResult(HttpStatusCode.Forbidden, HttpConstants.Error_403, reason);
		}
		public static ActionResult Result_403_Forbidden(this Controller controller, string msg, object reason)
		{
			return controller.ErrorResult(HttpStatusCode.Forbidden, msg, reason);
		}

		public static ActionResult Result_404_NotFound(this Controller controller)
		{
			return controller.ErrorResult(HttpStatusCode.NotFound, HttpConstants.Error_404, null);
		}
		public static ActionResult Result_404_NotFound(this Controller controller, string msg)
		{
			return controller.ErrorResult(HttpStatusCode.NotFound, msg, null);
		}
		public static ActionResult Result_404_NotFound(this Controller controller, object reason)
		{
			return controller.ErrorResult(HttpStatusCode.NotFound, HttpConstants.Error_404, reason);
		}
		public static ActionResult Result_404_NotFound(this Controller controller, string msg, object reason)
		{
			return controller.ErrorResult(HttpStatusCode.NotFound, msg, reason);
		}

		/// <summary>
		/// Indicate that the client imposed restrictions that can't be met. Example is an accept header that doesn't
		/// include our representation type.
		/// </summary>
		/// <param name="controller"></param>
		/// <returns></returns>
		public static ActionResult Result_406_NotAcceptable(this Controller controller)
		{
			return controller.ErrorResult(HttpStatusCode.NotAcceptable, HttpConstants.Error_406, null);
		}
		public static ActionResult Result_406_NotAcceptable(this Controller controller, string msg)
		{
			return controller.ErrorResult(HttpStatusCode.NotAcceptable, msg, null);
		}
		public static ActionResult Result_406_NotAcceptable(this Controller controller, object reason)
		{
			return controller.ErrorResult(HttpStatusCode.NotAcceptable, HttpConstants.Error_406, reason);
		}
		public static ActionResult Result_406_NotAcceptable(this Controller controller, string msg, object reason)
		{
			return controller.ErrorResult(HttpStatusCode.NotAcceptable, msg, reason);
		}

		public static ActionResult Result_409_Conflict(this Controller controller)
		{
			return controller.ErrorResult(HttpStatusCode.Conflict, HttpConstants.Error_409, null);
		}
		public static ActionResult Result_409_Conflict(this Controller controller, string msg)
		{
			return controller.ErrorResult(HttpStatusCode.Conflict, msg, null);
		}
		public static ActionResult Result_409_Conflict(this Controller controller, object reason)
		{
			return controller.ErrorResult(HttpStatusCode.Conflict, HttpConstants.Error_409, reason);
		}
		public static ActionResult Result_409_Conflict(this Controller controller, string msg, object reason)
		{
			return controller.ErrorResult(HttpStatusCode.Conflict, msg, reason);
		}

		public static ActionResult Result_409_Conflict_Resubmit(this Controller controller)
		{
			return controller.ErrorResult(HttpStatusCode.Conflict, HttpConstants.HttpSubstatusCodes.Conflict409_ResubmitRequired, HttpConstants.Error_409_5, null);
		}
		public static ActionResult Result_409_Conflict_Resubmit(this Controller controller, string msg)
		{
			return controller.ErrorResult(HttpStatusCode.Conflict, HttpConstants.HttpSubstatusCodes.Conflict409_ResubmitRequired, msg, null);
		}
		public static ActionResult Result_409_Conflict_Resubmit(this Controller controller, object reason)
		{
			return controller.ErrorResult(HttpStatusCode.Conflict, HttpConstants.HttpSubstatusCodes.Conflict409_ResubmitRequired, HttpConstants.Error_409_5, reason);
		}
		public static ActionResult Result_409_Conflict_Resubmit(this Controller controller, string msg, object reason)
		{
			return controller.ErrorResult(HttpStatusCode.Conflict, HttpConstants.HttpSubstatusCodes.Conflict409_ResubmitRequired, msg, reason);
		}

		public static ActionResult Result_410_Gone(this Controller controller)
		{
			return controller.ErrorResult(HttpStatusCode.Gone, HttpConstants.Error_410, null);
		}
		public static ActionResult Result_410_Gone(this Controller controller, string msg)
		{
			return controller.ErrorResult(HttpStatusCode.Gone, msg, null);
		}
		public static ActionResult Result_410_Gone(this Controller controller, object reason)
		{
			return controller.ErrorResult(HttpStatusCode.Gone, HttpConstants.Error_410, reason);
		}
		public static ActionResult Result_410_Gone(this Controller controller, string msg, object reason)
		{
			return controller.ErrorResult(HttpStatusCode.Gone, msg, reason);
		}

		public static ActionResult Result_422_UnprocessableEntity(this Controller controller)
		{
			return controller.ErrorResult((HttpStatusCode)422, HttpConstants.Error_422, null);
		}
		public static ActionResult Result_422_UnprocessableEntity(this Controller controller, string msg)
		{
			return controller.ErrorResult((HttpStatusCode)422, msg, null);
		}
		public static ActionResult Result_422_UnprocessableEntity(this Controller controller, object reason)
		{
			return controller.ErrorResult((HttpStatusCode)422, HttpConstants.Error_422, reason);
		}
		public static ActionResult Result_422_UnprocessableEntity(this Controller controller, string msg, object reason)
		{
			return controller.ErrorResult((HttpStatusCode)422, msg, reason);
		}

		public static ActionResult Result_415_UnsupportedMediaType(this Controller controller)
		{
			return controller.ErrorResult(HttpStatusCode.UnsupportedMediaType, HttpConstants.Error_415, null);
		}
		public static ActionResult Result_415_UnsupportedMediaType(this Controller controller, string msg)
		{
			return controller.ErrorResult(HttpStatusCode.UnsupportedMediaType, msg, null);
		}
		public static ActionResult Result_415_UnsupportedMediaType(this Controller controller, object reason)
		{
			return controller.ErrorResult(HttpStatusCode.UnsupportedMediaType, HttpConstants.Error_415, reason);
		}
		public static ActionResult Result_415_UnsupportedMediaType(this Controller controller, string msg, object reason)
		{
			return controller.ErrorResult(HttpStatusCode.UnsupportedMediaType, msg, reason);
		}

		public static ActionResult Result_500_InternalServerError(this Controller controller)
		{
			return controller.ErrorResult(HttpStatusCode.InternalServerError, HttpConstants.Error_500, null);
		}
		public static ActionResult Result_500_InternalServerError(this Controller controller, string msg)
		{
			return controller.ErrorResult(HttpStatusCode.InternalServerError, msg, null);
		}
		public static ActionResult Result_500_InternalServerError(this Controller controller, object reason)
		{
			return controller.ErrorResult(HttpStatusCode.InternalServerError, HttpConstants.Error_500, reason);
		}
		public static ActionResult Result_500_InternalServerError(this Controller controller, string msg, object reason)
		{
			return controller.ErrorResult(HttpStatusCode.InternalServerError, msg, reason);
		}

		public static ActionResult Result_501_NotImplemented(this Controller controller)
		{
			return controller.ErrorResult(HttpStatusCode.NotImplemented, HttpConstants.Error_501, null);
		}
		public static ActionResult Result_501_NotImplemented(this Controller controller, string msg)
		{
			return controller.ErrorResult(HttpStatusCode.NotImplemented, msg, null);
		}
		public static ActionResult Result_501_NotImplemented(this Controller controller, object reason)
		{
			return controller.ErrorResult(HttpStatusCode.NotImplemented, HttpConstants.Error_501, reason);
		}
		public static ActionResult Result_501_NotImplemented(this Controller controller, string msg, object reason)
		{
			return controller.ErrorResult(HttpStatusCode.NotImplemented, msg, reason);
		}

		public static ActionResult Result_503_ServiceUnavailable(this Controller controller)
		{
			return controller.ErrorResult(HttpStatusCode.ServiceUnavailable, HttpConstants.Error_503, null);
		}
		public static ActionResult Result_503_ServiceUnavailable(this Controller controller, string msg)
		{
			return controller.ErrorResult(HttpStatusCode.ServiceUnavailable, msg, null);
		}
		public static ActionResult Result_503_ServiceUnavailable(this Controller controller, object reason)
		{
			return controller.ErrorResult(HttpStatusCode.ServiceUnavailable, HttpConstants.Error_503, reason);
		}
		public static ActionResult Result_503_ServiceUnavailable(this Controller controller, string msg, object reason)
		{
			return controller.ErrorResult(HttpStatusCode.ServiceUnavailable, msg, reason);
		}

		public static ActionResult Result_504_GatewayTimeout(this Controller controller)
		{
			return controller.ErrorResult(HttpStatusCode.GatewayTimeout, HttpConstants.Error_504, null);
		}
		public static ActionResult Result_504_GatewayTimeout(this Controller controller, string msg)
		{
			return controller.ErrorResult(HttpStatusCode.GatewayTimeout, msg, null);
		}
		public static ActionResult Result_504_GatewayTimeout(this Controller controller, object reason)
		{
			return controller.ErrorResult(HttpStatusCode.GatewayTimeout, HttpConstants.Error_504, reason);
		}
		public static ActionResult Result_504_GatewayTimeout(this Controller controller, string msg, object reason)
		{
			return controller.ErrorResult(HttpStatusCode.GatewayTimeout, msg, reason);
		}

		public static bool TryDeserializePostData<T>(this Controller controller, out ResultEncoding encoding, out T data)
		{
			var stream = controller.Request.InputStream;
			if (stream.Position == stream.Length) stream.Seek(0, SeekOrigin.Begin);
			using (var reader = new StreamReader(controller.Request.InputStream))
			{				
				var postData = reader.ReadToEnd();
				var log = controller.GetType().GetLogSink();
				if (log.IsLogging(SourceLevels.Verbose))
				{
					log.Information(HttpConstants.EventID.MvcPostDataReceived, HttpConstants.EventID.MvcPostDataReceived.ToString(),
						String.Concat("TryDeserializePostData<", typeof(T).GetReadableSimpleName(), ">"),
						new { PostData = postData });
				}
				if (!String.IsNullOrEmpty(postData))
				{
					var transform = Create.New<IJsonRepresentation<T>>();
					data = transform.RestoreItem(postData);
					if (data != null)
					{
						if (log.IsLogging(SourceLevels.Verbose))
						{
							log.Information(HttpConstants.EventID.MvcPostDataReceived, HttpConstants.EventID.MvcPostDataReceived.ToString(),
								String.Concat("TryDeserializePostData<", typeof(T).GetReadableSimpleName(), ">"),
								data);
						}
						encoding = ResultEncoding.Json;
						return true;
					}
				}
			}
			encoding = default(ResultEncoding);
			data = default(T);
			return false;
		}

		public static bool CanAcceptJson(this Controller controller)
		{
			return controller.Request.CanAcceptType(HttpConstants.MimeType_ApplicationJSON)
					 || controller.Request.CanAcceptType(HttpConstants.MimeType_TextJSON);
		}

		public static bool CanAcceptXml(this Controller controller)
		{
			return controller.Request.CanAcceptType(HttpConstants.MimeType_ApplicationXML)
					 || controller.Request.CanAcceptType(HttpConstants.MimeType_TextXML);
		}

	}

}
