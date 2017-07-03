using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Web.Routing;
using NetSteps.Common;
using NetSteps.Web.Routing;

namespace NetSteps.Web
{
    public static class RouteExtensions
    {
        public static Route Redirect(this RouteCollection routes, string url, string targetUrl)
        {
            Contract.Requires<ArgumentNullException>(routes != null);
            Contract.Requires<ArgumentNullException>(url != null);
            Contract.Requires<ArgumentException>(url.Length > 0);
            Contract.Requires<ArgumentNullException>(targetUrl != null);
            Contract.Requires<ArgumentException>(targetUrl.Length > 0);

            return Redirect(routes, url, targetUrl, false);
        }

        public static Route RedirectPermanent(this RouteCollection routes, string url, string targetUrl)
        {
            Contract.Requires<ArgumentNullException>(routes != null);
            Contract.Requires<ArgumentNullException>(url != null);
            Contract.Requires<ArgumentException>(url.Length > 0);
            Contract.Requires<ArgumentNullException>(targetUrl != null);
            Contract.Requires<ArgumentException>(targetUrl.Length > 0);

            return Redirect(routes, url, targetUrl, true);
        }

        public static Route Redirect(this RouteCollection routes, IUrlRedirect urlRedirect)
        {
            Contract.Requires<ArgumentNullException>(routes != null);
            Contract.Requires<ArgumentNullException>(urlRedirect != null);
            Contract.Requires<ArgumentException>(urlRedirect.Url != null);
            Contract.Requires<ArgumentException>(urlRedirect.Url.Length > 0);
            Contract.Requires<ArgumentException>(urlRedirect.TargetUrl != null);
            Contract.Requires<ArgumentException>(urlRedirect.TargetUrl.Length > 0);

            return Redirect(routes, urlRedirect.Url, urlRedirect.TargetUrl, urlRedirect.IsPermanent);
        }

        public static void RegisterRedirectRoutes(this RouteCollection routes, IEnumerable<IUrlRedirect> urlRedirects)
        {
            Contract.Requires<ArgumentNullException>(routes != null);
            Contract.Requires<ArgumentNullException>(urlRedirects != null);

            RegisterRedirectRoutes(routes, urlRedirects, null);
        }

        public static void RegisterRedirectRoutes(this RouteCollection routes, IEnumerable<IUrlRedirect> urlRedirects, Action<Exception> exceptionHandler)
        {
            Contract.Requires<ArgumentNullException>(routes != null);
            Contract.Requires<ArgumentNullException>(urlRedirects != null);

            foreach (var urlRedirect in urlRedirects)
            {
                try
                {
                    Redirect(routes, urlRedirect);
                }
                catch (Exception ex)
                {
                    if (exceptionHandler != null)
                    {
                        exceptionHandler(ex);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        public static Route Redirect(this RouteCollection routes, string url, string targetUrl, bool isPermanent)
        {
            Contract.Requires<ArgumentNullException>(routes != null);
            Contract.Requires<ArgumentNullException>(url != null);
            Contract.Requires<ArgumentException>(url.Length > 0);
            Contract.Requires<ArgumentNullException>(targetUrl != null);
            Contract.Requires<ArgumentException>(targetUrl.Length > 0);

            var route = new RedirectRoute(url, targetUrl, isPermanent);
            routes.Add(route);
            return route;
        }
    }
}
