using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Extensions;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Business.Filters.Interfaces;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class SiteUrlRepository
	{
		#region Members
		#endregion

		public List<SiteUrl> LoadBySiteID(int siteID)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
                using (NetStepsEntities context = CreateContext())
				{
					var urls = from u in context.SiteUrls
										 where u.SiteID == siteID
										 select u;

					return urls.ToList();
				}
			});
		}

		public bool IsAvailable(string url)
		{
			url = url.AppendForwardSlash().ToLower();

			var selector = Create.New<IUrlSelector>();
			selector.Url = url;

			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
                using (NetStepsEntities context = CreateContext())
				{
					// first check for exact matches--if there are any, return false
					if (context.SiteUrls.Any(s => s.Url.ToLower().StartsWith(url)))
					{
						return false;
					}
					// then check for matches using the specified IUrlSelector, which can be client-specific
					else
					{
						return !context.SiteUrls.Any(selector.Filter);
					}
				}
			});
		}
 public bool IsAvailable(string url, int marketId)
                {
                    url = url.AppendForwardSlash().ToLower();


                    return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
                    {
                        using (NetStepsEntities context = CreateContext())
                        {
                            var marketSiteIds = context.Sites.Where(s => s.MarketID == marketId).Select(s => s.SiteID).ToList();
                            return !context.SiteUrls.Any(s => marketSiteIds.Contains(s.SiteID.Value) && s.Url.ToLower().StartsWith(url));
                        }
                    });
                }

        public bool IsAvailable(int siteID, string url)
        {
            if (string.IsNullOrEmpty(url.ToCleanString()))
                return false;

			var selector = Create.New<IUrlSelector>();
			selector.Url = url;

            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    url = url.ToLower();
					var siteUrlsExact = context.SiteUrls.Where(s => s.Url.ToLower().StartsWith(url)).ToList();

					// if there's an exact match that's not already associatied with this siteID, return false
					if (siteUrlsExact.Count == 1 && !(siteUrlsExact.FirstOrDefault().SiteID == siteID))
						return false;
					else
					{
						// otherwise, check to see if there's an exact match that IS associated with this siteID, and if there is return true
						if (siteUrlsExact.Count == 1 && siteUrlsExact.FirstOrDefault().SiteID == siteID)
						{
							return true;
						}

						// if it hasn't returned yet, that means there are no exact matches, 
						// but we still need to check for matches using the specified IUrlSelector,
						// which can be client-specific
						var siteUrls = context.SiteUrls.Where(selector.Filter).ToList();

						// if this comes up with no matches or a match that is associated with this siteID, return true
                        if (siteUrls.Count == 0 || (siteUrls.Count == 1 && siteUrls.FirstOrDefault().SiteID == siteID))
                            return true;
						// otherwise return false--there's a match that isn't associated already with this site
                        else
                            return false;
					}
                }
            });
        }


		public bool Exists(string url)
		{
			url = url.AppendForwardSlash().ToLower();
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
                using (NetStepsEntities context = CreateContext())
				{
					return context.SiteUrls.Any(s => s.Url.ToLower() == url);
				}
			});
		}

		/// <summary>
		/// Looks in database for a site url that matches
		/// </summary>
		/// <param name="url">The url to search for</param>
		/// <returns>The url match that is found in the database, or null if no match is found</returns>
		public string Match(string url)
		{
			url = url.AppendForwardSlash().ToLower();

			// Create the selector/url logic object to use for this matching
			var selector = Create.New<IUrlSelector>();
			selector.Url = url;

			if (!selector.IsValid)
				return null;

			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				List<SiteUrl> matches;
				using (NetStepsEntities context = new NetStepsEntities())
				{
					matches = context.SiteUrls.Where(selector.Filter).ToList();
				}

				if (matches.Count == 1)
				{
					return matches[0].Url;
				}
				else if (matches.Count > 1)
				{
					var match = matches.FirstOrDefault(x => string.Equals(x.Url, url, System.StringComparison.InvariantCultureIgnoreCase));

					if (match != null)
						return match.Url;
					else
						return matches.OrderBy(x => x.Url.Length).First().Url;
				}
				else
					return null;
			});
		}
	}
}
