using System.Text.RegularExpressions;
using System.Web;
using NetSteps.Common.Configuration;

namespace NetSteps.Common.Extensions
{
    public static class HttpContextExtensions
    {
        public static string CurrentSiteUrl(this HttpContext httpContext)
        {
            if (httpContext == null
                || httpContext.Request == null
                || httpContext.Request.Url == null)
            {
                return string.Empty;
            }

            return CurrentSiteUrl(new HttpContextWrapper(httpContext));
        }

        public static string CurrentSiteUrl(this HttpContextBase httpContext)
        {
            if (httpContext == null
                || httpContext.Request == null
                || httpContext.Request.Url == null)
            {
                return string.Empty;
            }

            bool isSubdomain = ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.UrlFormatIsSubdomain, true);
            var localPath = httpContext.Request.Url.LocalPath;
            var distributor = (!isSubdomain && !string.IsNullOrEmpty(localPath) && Regex.IsMatch(localPath, @"^/\w+"))
                ? localPath.Substring(1, localPath.IndexOf('/', 1) > 0 ? localPath.IndexOf('/', 1) : localPath.Length - 1)
                : string.Empty;

            return "http://" + httpContext.Request.Url.Authority + httpContext.Request.ApplicationPath + distributor;
        }

        public static string RootWebPath(this HttpContextBase httpContext)
        {
            if (httpContext == null
                || httpContext.Request == null
                || httpContext.Request.Url == null)
            {
                return string.Empty;
            }

            return string.Format("{0}://{1}{2}", httpContext.Request.Url.Scheme, httpContext.Request.Url.Authority, httpContext.Request.ApplicationPath);
        }
    }
}
