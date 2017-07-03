using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Configuration;
using NetSteps.Common.EldResolver;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Extensions;

namespace NetSteps.Data.Entities.Business.Logic
{
    public partial class ProxyLinkBusinessLogic
    {
        public virtual IList<ProxyLinkData> GetAccountProxyLinks(
            int accountID,
            short accountTypeID)
        {
            // Currently, we only have proxy links for distributors.
            // This may need to be data-driven in the future.
            if (accountTypeID != (short)Constants.AccountType.Distributor)
            {
                return new List<ProxyLinkData>();
            }

            return SmallCollectionCache.Instance.ProxyLinks
                .Where(pl => pl.Active)
                .OrderBy(pl => pl.SortIndex)
                .Select(pl => new ProxyLinkData
                {
                    LocalizedName = pl.GetTerm(),
                    Url = FormatAccountProxyLinkUrl(pl, accountID, accountTypeID)
                })
                .ToList();
        }


		public virtual string FormatAccountProxyLinkUrl(
			ProxyLink proxyLink,
			int accountID,
			short accountTypeID)
		{
            var url = proxyLink.URL;

			//If it is distributor Url request,get the personal url from the siteurl table
			if (proxyLink.Name == "nsDistributor")
			{
				var sites = Site.LoadByAccountID(accountID);
				var site = sites.OrderByDescending(s => s.PrimaryUrl != null && !s.PrimaryUrl.Url.IsNullOrWhiteSpace()).FirstOrDefault(s => s.SiteStatusID == (short)Constants.SiteStatus.Active);
				var siteUrl = site == null ? null : site.PrimaryUrl == null ? null : site.PrimaryUrl.Url;
                var urlPws = "http://base.belcorpbra.qas.draftbrasil.com/Login";
                url = !string.IsNullOrEmpty(siteUrl) ? string.Concat(siteUrl, siteUrl.EndsWith("/") ? string.Empty : "/", "Login") : urlPws;
			}

			// ELD encode
			var uriBuilder = BuildUri(url, accountID, accountTypeID);

			// Make https if necessary.
			if (ConfigurationManager.ForceSSL)
			{
				uriBuilder.ToHttps();
			}

			return uriBuilder.Uri.AbsoluteUri;
		}

        protected virtual UriBuilder BuildUri(string url, int accountID, short accountTypeID)
        {
            return new UriBuilder(url)
                .EldEncode()
                .AppendQueryValues(new { token = Account.GetSingleSignOnToken(accountID, false) });
        }
    }
}