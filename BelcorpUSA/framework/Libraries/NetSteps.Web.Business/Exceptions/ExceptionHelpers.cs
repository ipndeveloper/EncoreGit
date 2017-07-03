using System.Web;
using NetSteps.Data.Entities;

namespace NetSteps.Web.Mvc.Business.Exceptions
{
    public class ExceptionHelpers
    {
        /// <summary>
        /// This method will set all the 'web' related info on the ErrorLog before we save the error to the DB. - JHE
        /// </summary>
        /// <param name="errorLog"></param>
        public static void SetWebContextValues(ErrorLog errorLog)
        {
            HttpContext ctx = HttpContext.Current;

            errorLog.Referer = ctx.Request.ServerVariables["HTTP_REFERER"] ?? string.Empty;
            errorLog.Form = (ctx.Request.Form != null) ? ctx.Request.Form.ToString() : string.Empty;
            errorLog.QueryString = (ctx.Request.QueryString != null) ? ctx.Request.QueryString.ToString() : string.Empty;
            errorLog.SessionID = ctx.Session.SessionID;
            errorLog.UserHostAddress = ctx.Request.UserHostAddress;
            errorLog.BrowserInfo = string.Format("Browser: {0} Browser Version: {1} Platform: {2}",
                            HttpContext.Current.Request.Browser.Browser,
                            HttpContext.Current.Request.Browser.Version,
                            HttpContext.Current.Request.Browser.Platform);
        }
    }
}
