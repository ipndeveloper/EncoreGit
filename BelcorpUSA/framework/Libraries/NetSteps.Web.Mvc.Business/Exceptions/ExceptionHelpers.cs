using System.Web;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities;

namespace NetSteps.Web.Mvc.Business.Exceptions
{
    public class ExceptionHelpers
    {
        /// <summary>
        /// Author: John Egbert
        /// Description: This method will set all the 'web' related info on the ErrorLog before we save the error to the DB. - JHE
        /// Created: 03/1/2010
        /// </summary>
        public static void SetWebContextValues(ErrorLog errorLog)
        {
            HttpContext ctx = HttpContext.Current;

            if (ctx == null)
                return;

            errorLog.Referrer = ctx.Request.ServerVariables["HTTP_REFERER"] ?? string.Empty;
            errorLog.Form = (ctx.Request.Form != null) ? ctx.Request.Form.ToString() : string.Empty;
            errorLog.QueryString = (ctx.Request.QueryString != null) ? ctx.Request.QueryString.ToString() : string.Empty;
            errorLog.SessionID = (ctx.Session != null) ? ctx.Session.SessionID : string.Empty;
            errorLog.UserHostAddress = ctx.Request.UserHostAddress;
            errorLog.BrowserInfo = string.Format("Browser: {0} Browser Version: {1} Platform: {2}",
                            HttpContext.Current.Request.Browser.Browser,
                            HttpContext.Current.Request.Browser.Version,
                            HttpContext.Current.Request.Browser.Platform);

            if (errorLog.Referrer.IsNullOrEmpty())
                errorLog.Referrer = WebContext.PageUrlWithQueryString;
        }
    }
}
