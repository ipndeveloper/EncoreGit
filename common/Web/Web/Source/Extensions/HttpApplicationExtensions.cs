using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using NetSteps.Common.Extensions;

namespace NetSteps.Web.Extensions
{
    public static class HttpApplicationExtensions
    {
		private static List<string> urlPartsAlwaysAllowed = new List<string> { "cache/", "scripts/", "content/" };

		public static void DenyRequestsFromUnauthorizedIPs(this HttpApplication application, string maintenancePagePath = "~/_app_offline.htm")
		{
			List<string> allowedIPs = (System.Configuration.ConfigurationManager.AppSettings["AllowedIPAddressList"] ?? "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
			if (allowedIPs.Count == 0)
				return;

			string url = application != null && application.Request != null && application.Request.Url != null ? application.Request.Url.OriginalString ?? string.Empty : string.Empty;
			if (urlPartsAlwaysAllowed.Exists(x => url.ContainsIgnoreCase(x)))
				return;

			if (!allowedIPs.Any(x => (x == application.Request.UserHostAddress) || x.EndsWith("*") && application.Request.UserHostAddress.StartsWith(x.TrimEnd("*"))))
			{
				try
				{
					try
					{
						using (TextReader reader = File.OpenText(HttpContext.Current.Server.MapPath(maintenancePagePath)))
						{
							application.Response.Write(reader.ReadToEnd());
						}
					}
					catch (Exception)
					{
						application.Response.Write("<html><head><title>503 : Service Unavailable</title></head><body><h1>503 : Service Unavailable</h1></body></html>");
						application.Response.StatusCode = 503;
					}
				}
				finally
				{
					application.Response.End();
				}
			}
		}
    }
}
