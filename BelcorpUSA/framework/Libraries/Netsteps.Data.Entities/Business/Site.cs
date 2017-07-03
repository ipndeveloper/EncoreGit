using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;

namespace NetSteps.Data.Entities
{
	public partial class Site
	{
		#region Internals

		/// <summary>
		/// Related entities that can be included by the 'Load' methods (see <see cref="LoadRelationsExtensions"/>).
		/// WARNING: Changes to this list will affect various 'Load' methods, be careful.
		/// </summary>
		[Flags]
		public enum Relations
		{
			// These are bit flags so they must be numbered appropriately (eg. 0, 1, 2, 4, 8, 16)
			// Use bit-shifting for convenience (eg. 0, 1 << 0, 1 << 1, 1 << 2)
			None = 0,
			Account = 1 << 0,
			Archives = 1 << 1,
			CalendarEvents = 1 << 2,
			HtmlSections = 1 << 3,
			Languages = 1 << 4,
			Navigations = 1 << 5,
			News = 1 << 6,
			Pages = 1 << 7,
			SiteSettingValues = 1 << 8,
			SiteSettings = 1 << 9,
			SiteType = 1 << 10,
			SiteUrls = 1 << 11,
			SiteWidgets = 1 << 12,

			/// <summary>
			/// The default relations used by the 'LoadFull' methods.
			/// </summary>
			LoadFull =
				Account
				| Archives
				| CalendarEvents
				| HtmlSections
				| Languages
				| Navigations
				| News
				| Pages
				| SiteSettingValues
				| SiteSettings
				| SiteType
				| SiteUrls
				| SiteWidgets,

			/// <summary>
			/// The default relations used for loading base sites.
			/// </summary>
			LoadFullBaseSite =
				Archives
				| CalendarEvents
				| HtmlSections
				| Languages
				| Navigations
				| News
				| Pages
				| SiteSettingValues
				| SiteSettings
				| SiteType
				| SiteUrls
				| SiteWidgets,

			/// <summary>
			/// The default relations used for loading child sites.
			/// </summary>
			LoadFullChildSite =
				HtmlSections
				| Languages
				| SiteSettingValues
				| SiteUrls,

            /// <summary>
            /// This is mainly for editing a site in GMP.
            /// </summary>
            LoadFullWithoutContent =
                Account
                | Archives
                | CalendarEvents
                | Languages
                | Navigations
                | News
                | Pages
                | SiteSettingValues
                | SiteSettings
                | SiteType
                | SiteUrls
                | SiteWidgets,
		};

		#endregion

		#region Fields

		private SiteSettingList _settings;
		private string _backofficeurl;

		#endregion

		#region Properties

		public SiteSettingList Settings
		{
			get
			{
				if (_settings == null)
					_settings = new SiteSettingList(this, Repository.LoadSiteSettingsInherited(SiteID));

				return _settings;
			}
			set
			{
				_settings = value;
			}
		}

		[Serializable]
		public class SiteSettingList : List<SiteSettingItem>
		{
			private Site _site;
			private List<SiteSettingItem> _inheritedSiteSettings;

			public SiteSettingList(Site site, List<SiteSettingItem> inheritedSiteSettings)
			{
				_site = site;
				_inheritedSiteSettings = inheritedSiteSettings;
			}

			public SiteSettingItem this[string title]
			{
				get
				{
					return _inheritedSiteSettings.FirstOrDefault(s => s.Name == title);
				}
			}

			public void Add(SiteSettingValue siteSettingValue)
			{
				// TODO Capture the Add/Remove to Synchronize collections with Entities - JHE
			}

			public void Remove(SiteSettingValue siteSettingValue)
			{
				// TODO Capture the Add/Remove to Synchronize collections with Entities - JHE
			}
		}

		public SiteUrl PrimaryUrl
		{
			get
			{
				return SiteUrls.FirstOrDefault(obj => obj.IsPrimaryUrl);
			}
		}

		public string BackOfficeUrl
		{
			get
			{
				try
				{
					if (_backofficeurl == null)
						_backofficeurl = Repository.LoadBackOfficeURL(this.SiteID);
				}
				catch (Exception ex)
				{
					throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
				}
				return _backofficeurl;
			}
		}

		#endregion

		#region Methods

		public static List<Site> LoadByAccountID(int accountID)
		{
			try
			{
				List<Site> sites = Repository.LoadByAccountID(accountID);
				foreach (Site site in sites)
				{
					site.StartTracking();
					site.IsLazyLoadingEnabled = true;
				}
				return sites;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static Site LoadByAutoshipOrderID(int autoshipOrderID)
		{
			try
			{
				var site = Repository.LoadByAutoshipOrderID(autoshipOrderID);
				if (site != null)
				{
					site.StartEntityTracking();
					site.IsLazyLoadingEnabled = true;
				}
				return site;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static List<Site> LoadByBaseSiteID(int baseSiteID)
		{
			try
			{
				var list = Repository.LoadByBaseSiteID(baseSiteID);
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

		public static List<Site> LoadBaseSites()
		{
			try
			{
				var list = Repository.LoadBaseSites();
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

		public static Site LoadSiteWithSiteURLs(int siteID)
		{
			try
			{
				var site = Repository.LoadSiteWithSiteURLs(siteID);

				site.StartTracking();
				site.IsLazyLoadingEnabled = true;

				return site;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static Site LoadBaseSiteForNewPWS(int marketID)
		{
			try
			{
				var site = Repository.LoadBaseSiteForNewPWS(marketID);

				//site.StartTracking();
				//site.IsLazyLoadingEnabled = true;

				return site;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static List<Site> LoadBaseSites(int marketID, int userID)
		{
			try
			{
				var list = Repository.LoadBaseSites(marketID, userID);
				foreach (var item in list)
				{
					item.StartTracking();
					item.IsLazyLoadingEnabled = true;
                    if (!item.PrimaryUrl.Url.Contains("https"))
                    {
                        item.PrimaryUrl.Url = item.PrimaryUrl.Url.Replace("http", "https");   
                    }
				}
				return list;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static Site FindCorporateSite(int marketID)
		{
			try
			{
				var result = Repository.FindCorporateSite(marketID);
				if (result != null)
				{
					result.StartTracking();
					result.IsLazyLoadingEnabled = true;
				}
				return result;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static PaginatedList<SiteSearchData> Search(SiteSearchParameters searchParams)
		{
			try
			{
				return Repository.Search(searchParams);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		#region Internal Helper Methods
		internal static void CloneContentFromExistingSiteForNewBaseSite(int newBaseSiteID, int existingBaseSiteID)
		{
			try
			{
				BusinessLogic.CloneContentFromExistingSiteForNewBaseSite(newBaseSiteID, existingBaseSiteID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}
		internal static void ClonePagesFromExistingSiteForNewBaseSite(int newBaseSiteID, int existingBaseSiteID)
		{
			try
			{
				BusinessLogic.ClonePagesFromExistingSiteForNewBaseSite(newBaseSiteID, existingBaseSiteID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}
		#endregion

		public static IEnumerable<Page> LoadPages(int id)
		{
			try
			{
				return BusinessLogic.LoadPages(Repository, id);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static Site SiteWithNewsAndArchive(int siteID)
		{
			try
			{
				return BusinessLogic.SiteWithNewsAndArchive(Repository, siteID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static Site SiteWithLanguage(int siteID)
		{
			try
			{
				return BusinessLogic.SiteWithLanguage(Repository, siteID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static Site SiteWithNews(int siteID)
		{
			try
			{
				return BusinessLogic.SiteWithNews(Repository, siteID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static Site SiteWithSiteMap(int siteID)
		{
			try
			{
				return BusinessLogic.SiteWithSiteMap(Repository, siteID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

        public static Site LoadFullWithoutContent(int siteID)
        {
            return Repository.LoadFullWithoutContent(siteID);
        }

		public static Site CreateBaseSite(short siteTypeID, int marketId, string siteName, string displayName, int defaultLanguageID, IEnumerable<string> urls = null, bool saveNewSite = true)
		{
			try
			{
				return BusinessLogic.CreateBaseSite(siteTypeID, marketId, siteName, displayName, defaultLanguageID, urls, saveNewSite);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static Site CopyBaseSite(int siteIDToCopyFrom, int marketId, string displayName, string description, int defaultLanguageID, IEnumerable<string> urls = null)
		{
			try
			{
				return BusinessLogic.CopyBaseSite(siteIDToCopyFrom, marketId, displayName, description, defaultLanguageID, urls);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static void UpdateSiteLastEditDate(int siteID)
		{
			try
			{
				BusinessLogic.UpdateSiteLastEditDate(siteID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}


		public Site CreateChildSite(Account account, int marketId, int? autoshipOrderId, string siteName = "", string displayName = "", IEnumerable<string> urls = null, bool saveNewSite = true)
		{
			try
			{
				return BusinessLogic.CreateChildSite(this, account, marketId, autoshipOrderId, siteName, displayName, urls, saveNewSite);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		/// <summary>
		/// Default to the User's LanguageID if it exists in the SiteLanguages else English. - JHE
		/// If neither of those options exist, show the site in the first language - DES
		/// </summary>
		/// <returns></returns>
		public int GetDefaultEditingLanguageID()
		{
			if (this.Languages.Any(l => l.LanguageID == ApplicationContext.Instance.CurrentLanguageID))
				return ApplicationContext.Instance.CurrentLanguageID;
			else if (this.Languages.Any(l => l.LanguageID == Language.English.LanguageID))
				return Language.English.LanguageID;
			else if (this.Languages.Count > 0)
				return this.Languages.First().LanguageID;
			else
				return 0;
		}

		public List<string> GetDomains()
		{
			List<string> domains;

			domains = SiteUrls.OrderByDescending(x => x.IsPrimaryUrl).Select(x => x.GetDomain()).Where(x => !x.Contains(":")).Distinct().ToList();

			return domains;
		}

		public static Site LoadSiteForCache(int siteID)
		{
			try
			{
				return Repository.LoadSiteForCache(siteID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		/// <summary>
		/// Saves specified setting to Database - JHE
		/// </summary>
		/// <param name="siteID"></param>
		/// <param name="settingName"></param>
		/// <param name="settingValue"></param>
		public static void SaveSiteSetting(int siteID, string settingName, string settingValue)
		{
			try
			{
				Repository.SaveSiteSetting(siteID, settingName, settingValue);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public void SaveSiteSetting(string settingName, string settingValue)
		{
			try
			{
				Repository.SaveSiteSetting(this.SiteID, settingName, settingValue);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static Site LoadByUrl(string url)
		{
			return LoadByUrl(url, x => x);
		}

		public static T LoadByUrl<T>(string url, Func<Site, T> selector)
		{
			return Repository.LoadByUrl(url, selector);
		}

		public static Site LoadFullClone(int siteID)
		{
			try
			{
				return Repository.LoadFullClone(siteID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static List<Site> LoadOtherBaseSites(Site baseSite)
		{
			return Repository.GetOtherBaseSites(baseSite);
		}

		#endregion
	}
}
