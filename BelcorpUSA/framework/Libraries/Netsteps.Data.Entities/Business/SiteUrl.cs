using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Comparer;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities
{
    public partial class SiteUrl
	{
		#region Basic Crud
		public static bool IsAvailable(string url)
		{
			try
			{
				return IsBlacklisted(url) ? false : Repository.IsAvailable(url);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

        public static bool IsAvailable(string url, int marketId)
        {
            try
            {
                return IsBlacklisted(url) ? false : Repository.IsAvailable(url, marketId);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

		public static bool IsAvailable(int siteID, string url)
		{
			try
			{
                return IsBlacklisted(url) ? false : Repository.IsAvailable(siteID, url);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

        public static bool IsBlacklisted(string url)
        {
            //  Currently implies blacklisting is at the lowest sub-domain level
            var urlSplit = url.Split(new string[] { "://" }, 2, StringSplitOptions.RemoveEmptyEntries);
            if(urlSplit.Length == 2)
                url = urlSplit[1].ToUpper();

            var blacklistedSites = System.Configuration.ConfigurationManager.AppSettings["BlacklistedSites"];
            if (string.IsNullOrEmpty(blacklistedSites)) return false;
            return blacklistedSites
                .ToUpper()
                .Split(new char[] { '|' })
                .Contains(
                url, new LambdaComparer<string>((string item, string match) =>
                {
                    return url.Split(new char[] { '.' })[0] == item;
                }));
        }

        public static string Match(string url)
        {
            try
            {
                return Repository.Match(url);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

		#endregion

        public static List<SiteUrl> LoadBySiteID(int siteID)
        {
            try
            {
                var list = Repository.LoadBySiteID(siteID);
                foreach (var item in list)
                {
                    item.StartTracking();
                    item.IsLazyLoadingEnabled = true;
                }
                return list;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public string GetDomain()
        {
            return Url.GetURLDomain();
        }

        public string GetSubDomain()
        {
            return Url.GetURLSubdomain();
        }

        /// <summary>
        /// Given a site's URL, determines if the site exists in underlying data.
        /// </summary>
        /// <param name="url">the site's URL</param>
        /// <returns>true if the site exists; otherwise false</returns>
        public static bool Exists(string url)
        {
            return SiteCache.GetSiteByUrl(url) != null;
        }

        /// <summary>
        /// Performs logic to determine if a site exists; this version is intended as the 
        /// callback used by the cache when existence is not already known.
        /// </summary>
        /// <param name="url">the site's URL</param>
        /// <returns>true if the site exists; otherwise false</returns>
        internal static bool PerformExists(string url)
        {
            try
            {
                return Repository.Exists(url);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
    }
}
