using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Sites.Common.Models;
using System.Diagnostics.Contracts;

namespace NetSteps.Sites.Common
{
	/// <summary>
	/// Site service extension methods.
	/// </summary>
	public static class ISiteServiceExtensions
	{
		/// <summary>
		/// Gets a site by URI or returns null if the site is not found.
		/// </summary>
		/// <param name="siteService">The <see cref="ISiteService"/>.</param>
		/// <param name="uri">The <see cref="Uri"/>.</param>
		/// <returns>The site or null.</returns>
		public static ISite GetSite(this ISiteService siteService, Uri uri)
		{
			Contract.Requires<ArgumentNullException>(siteService != null);
			Contract.Requires<ArgumentNullException>(uri != null);

			var siteId = siteService.GetSiteId(uri);
			if (!siteId.HasValue)
			{
				return null;
			}

			return siteService.GetSite(siteId.Value);
		}

		/// <summary>
		/// Gets a site ID by URI or returns null if the site is not found.
		/// </summary>
		/// <param name="siteService">The <see cref="ISiteService"/>.</param>
		/// <param name="uri">The <see cref="Uri"/>.</param>
		/// <returns>The site ID or null.</returns>
		public static int? GetSiteId(this ISiteService siteService, Uri uri)
		{
			Contract.Requires<ArgumentNullException>(siteService != null);
			Contract.Requires<ArgumentNullException>(uri != null);

			return siteService.GetSiteId(
				siteService.FormatSiteUrl(uri)
			);
		}

		/// <summary>
		/// Returns Google Analytics settings for a site or returns null if none are found.
		/// </summary>
		/// <param name="siteService">The <see cref="ISiteService"/>.</param>
		/// <param name="uri">The <see cref="Uri"/>.</param>
		/// <returns>The <see cref="ISiteGoogleAnalyticsSettings"/> for the site or null if none are found.</returns>
		public static ISiteGoogleAnalyticsSettings GetGoogleAnalyticsSettings(this ISiteService siteService, Uri uri)
		{
			Contract.Requires<ArgumentNullException>(siteService != null);
			Contract.Requires<ArgumentNullException>(uri != null);

			var siteId = siteService.GetSiteId(uri);
			if (!siteId.HasValue)
			{
				return null;
			}

			return siteService.GetGoogleAnalyticsSettings(siteId.Value);
		}
	}
}
