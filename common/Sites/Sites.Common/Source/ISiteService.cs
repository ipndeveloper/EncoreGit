namespace NetSteps.Sites.Common
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using NetSteps.Sites.Common.Models;

    /// <summary>
    /// The SiteService interface.
    /// </summary>
	[ContractClass(typeof(Contracts.SiteServiceContracts))]
    public interface ISiteService
    {
		/// <summary>
		/// Gets a site by ID or returns null if the site is not found.
		/// </summary>
		/// <param name="siteId">The site ID.</param>
		/// <returns>The site or null.</returns>
		ISite GetSite(int siteId);

		/// <summary>
		/// Gets a site ID by URL or returns null if the site is not found.
		/// </summary>
		/// <param name="url">A properly formatted site URL.</param>
		/// <returns>The site ID or null.</returns>
		int? GetSiteId(string url);

		/// <summary>
		/// Returns a URL string in the proper SiteUrl format for database lookups.
		/// </summary>
		/// <param name="uri">The <see cref="Uri"/>.</param>
		/// <returns></returns>
		string FormatSiteUrl(Uri uri);

		/// <summary>
		/// Returns Google Analytics settings for a site or returns null if none are found.
		/// </summary>
		/// <param name="siteId">The site ID.</param>
		/// <returns>The <see cref="ISiteGoogleAnalyticsSettings"/> for the site or null if none are found.</returns>
		ISiteGoogleAnalyticsSettings GetGoogleAnalyticsSettings(int siteId);
    }

	namespace Contracts
	{
		[ContractClassFor(typeof(ISiteService))]
		internal abstract class SiteServiceContracts : ISiteService
		{
			ISite ISiteService.GetSite(int siteId)
			{
				Contract.Requires<ArgumentOutOfRangeException>(siteId > 0);
				throw new NotImplementedException();
			}

			int? ISiteService.GetSiteId(string url)
			{
				Contract.Requires<ArgumentNullException>(url != null);
				Contract.Requires<ArgumentException>(url.Length > 0);
				throw new NotImplementedException();
			}

			string ISiteService.FormatSiteUrl(Uri uri)
			{
				Contract.Requires<ArgumentNullException>(uri != null);
				throw new NotImplementedException();
			}

			ISiteGoogleAnalyticsSettings ISiteService.GetGoogleAnalyticsSettings(int siteId)
			{
				Contract.Requires<ArgumentOutOfRangeException>(siteId > 0);
				throw new NotImplementedException();
			}
		}
	}
}
