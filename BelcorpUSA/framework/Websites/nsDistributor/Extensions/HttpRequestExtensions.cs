using System;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;
using System.Web;

namespace nsDistributor.Extensions
{
	public static class HttpRequestExtensions
	{
		static readonly String __currentSiteUrlKey = "NetSteps_CurrentSiteUrlKey";
		static readonly Regex __matchWord = new Regex(@"^/\w+");

		public static Object GetUntypedData(this HttpRequestBase req, string name)
		{
			return req.RequestContext.HttpContext.Items[name];
		}

		public static T GetData<T>(this HttpRequestBase req, string name)
		{
			return (T)req.RequestContext.HttpContext.Items[name];
		}

		public static T SetData<T>(this HttpRequestBase req, string name, T value)
		{
			req.RequestContext.HttpContext.Items[name] = value;
			return value;
		}

		public static String CurrentSiteUrl(this HttpRequestBase req, bool urlFormatIsSubdomain)
		{
			Contract.Requires<ArgumentNullException>(req != null);

			var result = req.GetData<string>(__currentSiteUrlKey);
			if (String.IsNullOrEmpty(result))
			{
				var path = req.Url.LocalPath;
				var distributor = String.Empty;
				if (!String.IsNullOrEmpty(path) && !urlFormatIsSubdomain
					&& __matchWord.IsMatch(path))
				{
					var idx = path.IndexOf('/', 1);
					if (idx > 0)
					{
						distributor = path.Substring(1, idx);
					}
					else
					{
						distributor = path.Substring(1, path.Length -1);
					}
				}
				result = String.Concat(req.Url.Scheme,"://", req.Url.Authority, req.ApplicationPath, distributor);
				req.SetData(__currentSiteUrlKey, result);				
			}
			return result;
		}
	}
}