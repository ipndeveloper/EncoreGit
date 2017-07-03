using System;
using System.Diagnostics.Contracts;
using System.Web;
using System.Web.Routing;

namespace NetSteps.Web.Routing
{
    public class RedirectHttpHandler : IHttpHandler, IRouteHandler
    {
        public RedirectHttpHandler(string targetUrl, bool isPermanent)
        {
            Contract.Requires<ArgumentNullException>(targetUrl != null);
            Contract.Requires<ArgumentException>(targetUrl.Length > 0);

            TargetUrl = targetUrl;
            IsPermanent = isPermanent;
            IsReusable = false;
        }

        public string TargetUrl { get; set; }

        public bool IsPermanent { get; private set; }

        public bool IsReusable { get; private set; }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return this;
        }

        public void ProcessRequest(HttpContext context)
        {
            Redirect(context.Response, TargetUrl, IsPermanent);
        }

        private static void Redirect(HttpResponse response, string url, bool isPermanent)
        {
            Contract.Requires<ArgumentNullException>(response != null);
            Contract.Requires<ArgumentNullException>(url != null);
            Contract.Requires<ArgumentException>(url.Length > 0);

            if (isPermanent)
            {
                response.RedirectPermanent(url, true);
            }
            else
            {
                response.Redirect(url, false);
            }
        }
    }
}
