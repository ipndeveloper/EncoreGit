using System;
using System.Web.Mvc;

namespace Encore.ApiSite.Helpers
{
    public class JsonpResult : JsonResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentException("context");

            var response = context.HttpContext.Response;
            var jsonCallback = context.RouteData.Values["jsoncallback"] as string;
            if (!string.IsNullOrEmpty(jsonCallback))
            {
                if (string.IsNullOrEmpty(ContentType))
                    ContentType = "application/x-javascript";

                response.Write(string.Format("{0}(", jsonCallback));
            }
            base.ExecuteResult(context);
            if (!string.IsNullOrEmpty(jsonCallback))
                response.Write(")");
        }
    }

    public static class ControllerExtensions
    {
        public static JsonpResult Jsonp(this Controller controller, object data)
        {
            var result = new JsonpResult { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            result.ExecuteResult(controller.ControllerContext);
            return result;
        }
    }
}