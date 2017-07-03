using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics.Contracts;
using System.Linq;
using NetSteps.Common;
using NetSteps.Common.EldResolver;
using NetSteps.Encore.Core.IoC;
using NetSteps.Sites.Common;
using NetSteps.Sites.Common.Configuration;
using NetSteps.Sites.Common.Models;
using NetSteps.Sites.Common.Repositories;
using NetSteps.Sites.Service.Configuration;

namespace NetSteps.Sites.Service
{
	/// <summary>
	/// The site service.
	/// </summary>
	public class SiteService : ISiteService
	{
		/// <summary>
		/// The site repository.
		/// </summary>
		private readonly ISiteRepository _siteRepository;

		/// <summary>
		/// The site setting value repository.
		/// </summary>
		private readonly ISiteSettingRepository _siteSettingRepository;

		/// <summary>
		/// The sites configuration.
		/// </summary>
		private readonly ISitesConfiguration _sitesConfiguration;

		/// <summary>
		/// The analytics configuration.
		/// </summary>
		private readonly IAnalyticsConfiguration _analyticsConfiguration;

		/// <summary>
		/// The ELD resolver.
		/// </summary>
		private readonly IEldResolver _eldResolver;

		/// <summary>
		/// Initializes a new instance of the <see cref="SiteService"/> class.
		/// </summary>
		/// <param name="siteRepository">The site repository.</param>
		/// <param name="siteSettingRepository">The site setting repository.</param>
		/// <param name="sitesConfiguration">The sites configuration.</param>
		/// <param name="analyticsConfiguration">The analytics configuration.</param>
		/// <param name="eldResolver">The ELD Resolver.</param>
		public SiteService(
			ISiteRepository siteRepository,
			ISiteSettingRepository siteSettingRepository,
			ISitesConfiguration sitesConfiguration,
			IAnalyticsConfiguration analyticsConfiguration,
			IEldResolver eldResolver)
		{
			Contract.Requires<ArgumentNullException>(siteRepository != null);
			Contract.Requires<ArgumentNullException>(siteSettingRepository != null);
			Contract.Requires<ArgumentNullException>(sitesConfiguration != null);
			Contract.Requires<ArgumentNullException>(analyticsConfiguration != null);
			Contract.Requires<ArgumentNullException>(eldResolver != null);

			_siteRepository = siteRepository;
			_siteSettingRepository = siteSettingRepository;
			_sitesConfiguration = sitesConfiguration;
			_analyticsConfiguration = analyticsConfiguration;
			_eldResolver = eldResolver;
		}

		/// <summary>
		/// Gets a site by ID or returns null if the site is not found.
		/// </summary>
		public ISite GetSite(int siteId)
		{
			return _siteRepository.GetSite(siteId);
		}

		/// <summary>
		/// Gets a site ID by URL or returns null if the site is not found.
		/// </summary>
		public int? GetSiteId(string url)
		{
			return _siteRepository.GetSiteId(url);
		}

		/// <summary>
		/// Returns a URL string in the proper SiteUrl format for database lookups.
		/// SiteUrls must be ELD-decoded, lowercase, begin with "http://", end with "/", and may
		/// include a port number if using a non-standard port.
		/// </summary>
		public string FormatSiteUrl(Uri uri)
		{
			return "http://" + _eldResolver.EldDecode(uri).Authority.ToLower() + "/";
		}

		/// <summary>
		/// Returns Google Analytics settings for a site or returns null if none are found.
		/// </summary>
		public virtual ISiteGoogleAnalyticsSettings GetGoogleAnalyticsSettings(int siteId)
		{
			var isDebug = _analyticsConfiguration.IsDebug;
			
			// Start with config-based IDs
			var propertyIds = _analyticsConfiguration.PropertyIds.ToList();

			// Add IDs from the database
			var siteSettings = _siteSettingRepository.GetSiteSettings(siteId);
			if (siteSettings != null)
			{
				propertyIds.AddRange(GetGoogleAnalyticsPropertyIdsFromSiteSettings(siteSettings));
			}

			var googleAnalyticsSettings = Create.New<ISiteGoogleAnalyticsSettings>();
			googleAnalyticsSettings.IsDebug = isDebug;
			googleAnalyticsSettings.PropertyIds = propertyIds;

			return googleAnalyticsSettings;
		}

		private IEnumerable<string> GetGoogleAnalyticsPropertyIdsFromSiteSettings(
			IDictionary<string, string> siteSettings)
		{
			Contract.Requires<ArgumentNullException>(siteSettings != null);

			string baseSitePropertyId;
			if (siteSettings.TryGetValue(SiteConstants.SiteSettingKeys.BaseGoogleAnalyticsTrackerID, out baseSitePropertyId))
			{
				yield return baseSitePropertyId;
			}

			string sitePropertyId;
			if (siteSettings.TryGetValue(SiteConstants.SiteSettingKeys.GoogleAnalyticsTrackerID, out sitePropertyId))
			{
				yield return sitePropertyId;
			}
		}
	}
}
