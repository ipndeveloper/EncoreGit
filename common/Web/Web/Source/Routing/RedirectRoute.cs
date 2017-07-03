using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace NetSteps.Web.Routing
{
    public class RedirectRoute : Route
    {
        public RedirectRoute(string url, string targetUrl, bool isPermanent)
            : base(url, new RedirectHttpHandler(targetUrl, isPermanent))
        {
            Contract.Requires<ArgumentNullException>(url != null);
            Contract.Requires<ArgumentException>(url.Length > 0);
            Contract.Requires<ArgumentNullException>(targetUrl != null);
            Contract.Requires<ArgumentException>(targetUrl.Length > 0);
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            // Route is inbound-only and should never calculate an outbound virtual path.
            return null;
        }
    }
}
