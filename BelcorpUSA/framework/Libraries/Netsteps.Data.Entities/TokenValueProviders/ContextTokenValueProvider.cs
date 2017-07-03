using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Common.Interfaces;
using NetSteps.Data.Entities.Cache;
using NetSteps.Common.Configuration;
using NetSteps.Common.EldResolver;

namespace NetSteps.Data.Entities.TokenValueProviders
{
	public class ContextTokenValueProvider : ITokenValueProvider
	{
		private const string CURRENT_SITE_URL = "CURRENT_SITE_URL";
		private const string PWS_SITE_URL = "PWS_SITE_URL";

		private static string[] __tokens = new string[] { 
			CURRENT_SITE_URL,
			PWS_SITE_URL
		};

		public IEnumerable<string> GetKnownTokens()
		{
			return __tokens;
		}

		public string GetTokenValue(string token)
		{
			string result = token;
			if (__tokens.Contains(token))
			{
				switch (token)
				{
					case CURRENT_SITE_URL:
						if (System.Web.HttpContext.Current != null)
						{
							var request = System.Web.HttpContext.Current.Request.Url;
							string scheme = (ConfigurationManager.ForceSSL) ? Uri.UriSchemeHttps : Uri.UriSchemeHttp;
							var builder = new UriBuilder(scheme, request.Host);
							if (request.Port != 80 && request.Port != 443)
							{
								builder.Port = request.Port;
							}
							result = builder.Uri.AbsoluteUri;
						}
						break;
					case PWS_SITE_URL:
						if (ApplicationContext.Instance != null && ApplicationContext.Instance.CurrentAccount != null)
						{
							var sites = Site.LoadByAccountID(ApplicationContext.Instance.CurrentAccount.AccountID);
							if (sites != null && sites.Count > 0)
							{
								var request = System.Web.HttpContext.Current != null ? System.Web.HttpContext.Current.Request : null;
								SiteUrl defaultUrl = null;
								var allUrls = sites.Where(s => s.SiteStatusID == (int)Constants.SiteStatus.Active).SelectMany(s => s.SiteUrls).ToArray();
								if (allUrls != null && allUrls.Length > 0)
								{
									if (request != null && request.Url.Authority.Contains("localhost"))
									{
										defaultUrl = allUrls
											.FirstOrDefault(su => su.Url.Contains("localhost"));
									}
									if (defaultUrl == null)
									{
										defaultUrl = allUrls.FirstOrDefault(su => su.IsPrimaryUrl) ?? allUrls.FirstOrDefault();
									}
									if (defaultUrl != null)
									{
										string scheme = (ConfigurationManager.ForceSSL) ? Uri.UriSchemeHttps : Uri.UriSchemeHttp;
										var siteUrl = new Uri(defaultUrl.Url);
										var builder = new UriBuilder(scheme, siteUrl.Host);
										if (siteUrl.Port != 80 && siteUrl.Port != 443)
										{
											builder.Port = siteUrl.Port;
										}
										result = builder.Uri.EldEncode().AbsoluteUri;
									}
								}
							}
						}
						break;
					default:
						break;
				}
			}
			return result;
		}
	}
}
