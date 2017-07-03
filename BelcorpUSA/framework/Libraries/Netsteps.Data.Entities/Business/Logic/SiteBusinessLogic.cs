using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business.Logic
{
    public partial class SiteBusinessLogic
    {
        // Helper method to clone/create data for new objects(site, pages, etc..) from existing data - JHE
        public Site CreateBaseSite(short siteTypeID, int marketId, string siteName, string displayName, int defaultLanguageID, IEnumerable<string> urls = null, bool saveNewSite = true)
        {
            try
            {
                var site = new Site
                {
                    AccountID = null,
                    AccountNumber = string.Empty,
                    IsBase = true,
                    BaseSiteID = null,
                    MarketID = marketId,
                    AutoshipOrderID = null,
                    CreatedByUserID = ApplicationContext.Instance.CurrentUserID,
                    DateCreated = DateTime.Now,
                    DateSignedUp = DateTime.Now,
                    SiteStatusID = (int)Constants.SiteStatus.Active,
                    SiteTypeID = siteTypeID,
                    DefaultLanguageID = defaultLanguageID,
                    Name = siteName,
                    DisplayName = string.IsNullOrEmpty(displayName) ? null : displayName
                };

                site.Languages.Add(Language.Load(defaultLanguageID));

                bool primary = true;
	            if(urls != null)
	            {
		            foreach(string url in urls)
		            {
			            site.SiteUrls.Add(new SiteUrl
			            {
				            IsPrimaryUrl = primary,
				            Url = url,
				            LanguageID = defaultLanguageID
			            });
			            primary = false;
		            }
	            }

	            if(saveNewSite)
	            {
		            site.Save();
	            }

                return site;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        // Helper method to clone/create data for new objects(site, pages, etc..) from existing data - JHE
        public Site CopyBaseSite(int siteIDToCopyFrom, int marketId, string displayName, string description, int defaultLanguageID, IEnumerable<string> urls = null)
        {
            try
            {
                Site siteToCopyFrom = Site.LoadFullClone(siteIDToCopyFrom);

                var site = new Site
                {
                    AccountID = siteToCopyFrom.AccountID,
                    AccountNumber = siteToCopyFrom.AccountNumber,
                    IsBase = siteToCopyFrom.IsBase,
                    BaseSiteID = siteToCopyFrom.BaseSiteID,
                    MarketID = marketId,
                    AutoshipOrderID = siteToCopyFrom.AutoshipOrderID,
                    CreatedByUserID = ApplicationContext.Instance.CurrentUserID,
                    DateCreated = DateTime.Now,
                    DateSignedUp = DateTime.Now,
                    SiteStatusID = (int)Constants.SiteStatus.Active,
                    SiteTypeID = siteToCopyFrom.SiteTypeID,
                    DefaultLanguageID = defaultLanguageID,
                    Name = displayName,
                    DisplayName = string.IsNullOrEmpty(displayName) ? null : displayName,
                    Description = description
                };

                site.Languages.Add(Language.Load(defaultLanguageID));

                bool primary = true;
	            if(urls != null)
	            {
		            foreach(string url in urls)
		            {
			            site.SiteUrls.Add(new SiteUrl
			            {
				            IsPrimaryUrl = primary,
				            Url = url,
				            LanguageID = defaultLanguageID
			            });
			            primary = false;
		            }
	            }

	            CleanupSiteUrls(site);

                site.Save();

                site = CloneNavigationAndPages(siteToCopyFrom, site);

                site = CloneSiteLanguages(siteToCopyFrom, site);
                site = CloneSiteLayouts(siteToCopyFrom, site);
                site = CloneSiteSettings(siteToCopyFrom, site);
                site = CloneSiteWidgets(siteToCopyFrom, site);
                site = CloneContentFromExistingSiteForNewBaseSite(site, siteToCopyFrom);

                return site;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        private Site CloneNavigationAndPages(Site siteToCopyFrom, Site site)
        {
            site = CloneNavigations(site, siteToCopyFrom);
            site = ClonePagesFromExistingSiteForNewBaseSite(site, siteToCopyFrom);

            site.Save();

            return site;
        }

        private Site CloneNavigations(Site site, Site siteToCopyFrom)
        {
            List<Navigation> listToCopy = siteToCopyFrom.Navigations.OrderBy(x => x.NavigationID).Where(x => x.ParentID == null).ToList();

			IEnumerable<Navigation> newList = CopyNavigation(listToCopy, site, siteToCopyFrom);

            foreach (var item in newList)
            {
                site.Navigations.Add(item);
            }

            return site;
        }

		private IEnumerable<Navigation> CopyNavigation(IEnumerable<Navigation> listToCopy, Site site, Site siteToCopyFrom)
        {
            var copiedList = new List<Navigation>();

            foreach (var rootNodeToCopy in listToCopy)
            {
                copiedList.Add(CopyNavigationRecursive(rootNodeToCopy, site, siteToCopyFrom, null));
            }

            return copiedList;
        }

        private Navigation CopyNavigationRecursive(Navigation nodeToCopy, Site site, Site siteToCopyFrom, Navigation parent)
        {
            var copiedNode = new Navigation
            {
                BaseNavigationID = nodeToCopy.BaseNavigationID,
                NavigationTypeID = nodeToCopy.NavigationTypeID,
                Site = site,
                LinkUrl = nodeToCopy.LinkUrl,
                StartDateUTC = nodeToCopy.StartDateUTC,
                EndDateUTC = nodeToCopy.EndDateUTC,
                ParentID = nodeToCopy.ParentID,
                SortIndex = nodeToCopy.SortIndex,
                MinChildren = nodeToCopy.MinChildren,
                MaxChildren = nodeToCopy.MaxChildren,
                Active = nodeToCopy.Active,
                IsDeletable = nodeToCopy.IsDeletable,
                ModifiedByUserID = nodeToCopy.ModifiedByUserID,
                IsDropDown = nodeToCopy.IsDropDown,
                IsSecondaryNavigation = nodeToCopy.IsSecondaryNavigation,
                IsChildNavTree = nodeToCopy.IsChildNavTree,
                PageID = nodeToCopy.PageID,
                ParentNavigation = parent
            };

            foreach (var existingTranslation in nodeToCopy.Translations)
            {
                var copiedNavigationTranslation = new NavigationTranslation
                {
                    Navigation = copiedNode,
                    LanguageID = existingTranslation.LanguageID,
                    LinkText = existingTranslation.LinkText
                };

                copiedNode.Translations.Add(copiedNavigationTranslation);
            }

            var childrenOfOriginalNode = siteToCopyFrom.Navigations.Where(x => x.ParentID == nodeToCopy.NavigationID);

            foreach (var originalChildNode in childrenOfOriginalNode)
            {
                copiedNode.ChildNavigations.Add(CopyNavigationRecursive(originalChildNode, site, siteToCopyFrom, copiedNode));
            }
            
            return copiedNode;
        }

        private Site CloneSiteWidgets(Site siteToCopyFrom, Site site)
        {
            foreach (var copySiteWidget in siteToCopyFrom.SiteWidgets)
            {
                var newSiteSetting = new SiteWidget
                    {
                        WidgetID = copySiteWidget.WidgetID,
                        DisplayColumn = copySiteWidget.WidgetID,
                        SortIndex = copySiteWidget.SortIndex,
                        IsOnTop = copySiteWidget.IsOnTop,
                        Editable = copySiteWidget.Editable
                    };

                site.SiteWidgets.Add(newSiteSetting);
            }

            site.Save();

            return site;
        }

        private Site CloneSiteSettings(Site siteToCopyFrom, Site site)
        {
            foreach (var copySiteSetting in siteToCopyFrom.SiteSettings)
            {
                var newSiteSetting = new SiteSetting
                    {
                        BaseSiteID = copySiteSetting.BaseSiteID,
                        Name = copySiteSetting.Name
                    };

                site.SiteSettings.Add(newSiteSetting);

                site.Save();

                foreach (var copySiteSettingValue in copySiteSetting.SiteSettingValues)
                {
                    newSiteSetting.SiteSettingValues.Add(
                        new SiteSettingValue
                        {
                            Value = copySiteSettingValue.Value,
                            SiteID = site.SiteID
                        });
                }

                site.Save();
            }

            return site;
        }

        private Site CloneSiteLayouts(Site siteToCopyFrom, Site site)
        {
            var siteLayouts = Layout.GetLayoutsForSite(siteToCopyFrom.SiteID).Select(l => SmallCollectionCache.Instance.Layouts.GetById(l));

            foreach (var item in siteLayouts)
            {
                var newLayout = Layout.Load(item.LayoutID);

                foreach (var htmlSection in item.HtmlSections)
                {
                    HtmlSection newHtmlSection = HtmlSection.CloneHtmlSection(htmlSection, siteToCopyFrom.SiteID, site.SiteID);
                    newLayout.HtmlSections.Add(newHtmlSection);
                }

                site.Layouts.Add(newLayout);
            }

            site.Save();

            return site;
        }

        private Site CloneSiteLanguages(Site siteToCopyFrom, Site site)
        {
            foreach (var language in siteToCopyFrom.Languages)
            {
                if (!site.Languages.ContainsLanguageID(language.LanguageID))
                {
                    site.Languages.Add(Language.Load(language.LanguageID));
                }
            }

            site.Save();

            return site;
        }

        public Site CloneContentFromExistingSiteForNewBaseSite(Site newBaseSite, Site existingBaseSite)
        {
            foreach (var htmlSection in existingBaseSite.HtmlSections)
            {
                HtmlSection newHtmlSection = HtmlSection.CloneHtmlSection(htmlSection, existingBaseSite.SiteID, newBaseSite.SiteID);
                newBaseSite.HtmlSections.Add(newHtmlSection);
            }

            newBaseSite.Save();

            return newBaseSite;
        }

        // Helper method to clone/create data for new objects(site, pages, etc..) from existing data - JHE
        public Site ClonePagesFromExistingSiteForNewBaseSite(Site newBaseSite, Site existingBaseSite)
        {
            // First create template page - JHE
            foreach (var existingPage in existingBaseSite.Pages)
            {
                Page newPage = Page.ClonePage(existingPage, existingBaseSite.SiteID, siteID: newBaseSite.SiteID);
                newPage.SiteID = newBaseSite.SiteID;

	            int existingPageID = existingPage.PageID;
                foreach (var newNavigation in newBaseSite.Navigations.Where(p => p.PageID == existingPageID))
                {
                    newNavigation.PageID = null;
                    newNavigation.Page = newPage;
                }

                newBaseSite.Pages.Add(newPage);
            }

            return newBaseSite;
        }

        // Helper method to clone/create data for new objects(site, pages, etc..) from existing data - JHE
        public void CloneContentFromExistingSiteForNewBaseSite(int newBaseSiteID, int existingBaseSiteID)
        {
            Site newSite = Site.LoadFull(newBaseSiteID);
            Site existingBaseSite = Site.LoadFull(existingBaseSiteID);

            foreach (var htmlSection in existingBaseSite.HtmlSections)
            {
                HtmlSection newHtmlSection = HtmlSection.CloneHtmlSection(htmlSection, existingBaseSiteID, newSite.SiteID);
                newSite.HtmlSections.Add(newHtmlSection);
            }

            newSite.Save();
        }

        // Helper method to clone/create data for new objects(site, pages, etc..) from existing data - JHE
        public void ClonePagesFromExistingSiteForNewBaseSite(int newBaseSiteID, int existingBaseSiteID)
        {
            Site newSite = Site.LoadFull(newBaseSiteID);
            Site existingBaseSite = Site.LoadFull(existingBaseSiteID);

            // First create template page - JHE
            var templatePage = existingBaseSite.Pages.FirstOrDefault(p => p.ParentID == null);
            if (templatePage != null)
            {
                Page newTemplatePage = Page.ClonePage(templatePage, existingBaseSiteID, existingBaseSiteID);
                newTemplatePage.SiteID = newSite.SiteID;
                newSite.Pages.Add(newTemplatePage);
            }

            foreach (var page in existingBaseSite.Pages)
            {
                Page newPage = Page.ClonePage(page, existingBaseSiteID, newBaseSiteID);
                newPage.SiteID = newSite.SiteID;
                newSite.Pages.Add(newPage);
            }

            newSite.Save();
        }

        public Site CreateChildSite(Site baseSite, Account account, int marketId, int? autoshipOrderId, string siteName = "", string displayName = "", IEnumerable<string> urls = null, bool saveNewSite = true)
        {
            try
            {
				var site = new Site
				{
					AccountID = account.AccountID,
					AccountNumber = account.AccountNumber,
					IsBase = false,
					BaseSiteID = baseSite.SiteID,
					MarketID = marketId,
					AutoshipOrderID = autoshipOrderId,
					CreatedByUserID = ApplicationContext.Instance.CurrentUserID,
					DateCreated = DateTime.Now,
					DateSignedUp = DateTime.Now,
					SiteStatusID = (int)Constants.SiteStatus.Active,
					SiteTypeID = (int)Constants.SiteType.Replicated,
					DefaultLanguageID = account.DefaultLanguageID,
					Name = string.IsNullOrEmpty(siteName) ? account.FullName + "'s Site" : siteName,
					DisplayName = string.IsNullOrEmpty(displayName) ? null : displayName
				};

	            if(baseSite.Languages.Count > 0 && baseSite.Languages.ContainsLanguageID(account.DefaultLanguageID))
	            {
		            site.Languages.AddRange(baseSite.Languages);
	            }
	            else
	            {
		            site.Languages.Add(Language.Load(account.DefaultLanguageID));
	            }

                bool primary = true;
                foreach (string url in urls.Where(x => !String.IsNullOrWhiteSpace(x)))
                {
                    site.SiteUrls.Add(new SiteUrl
                    {
                        IsPrimaryUrl = primary,
                        Url = url,
                        LanguageID = account.DefaultLanguageID
                    });
                    primary = false;
                }

	            if(saveNewSite)
	            {
		            site.Save();
	            }

                return site;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public IEnumerable<Page> LoadPages(ISiteRepository siteRepository, int siteID)
        {
            try
            {
                return siteRepository.LoadPages(siteID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public Site SiteWithLanguage(ISiteRepository siteRepository, int siteID)
        {
            try
            {
                return siteRepository.SiteWithLanguages(siteID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public Site SiteWithNews(ISiteRepository siteRepository, int siteID)
        {
            try
            {
                Site site = siteRepository.SiteWithNews(siteID);
                site.StartEntityTracking();

                return site;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public Site SiteWithSiteMap(ISiteRepository siteRepository, int siteID)
        {
            try
            {
                Site site = siteRepository.SiteWithSiteMap(siteID);
                site.StartEntityTracking();
                return site;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public Site SiteWithNewsAndArchive(ISiteRepository siteRepository, int siteID)
        {
            try
            {
                Site site = siteRepository.SiteWithNewsAndArchive(siteID);
                return site;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }


        public override List<string> ValidatedChildPropertiesSetByParent(ISiteRepository repository)
        {
            return new List<string>
            {
	            "NavigationID",
				"PageID",
				"NewsID",
				"CalendarEventID",
				"ArchiveID",
				"HtmlSectionID",
				"HtmlSectionContentID",
				"HtmlSectionChoiceID",
				"HtmlContentID"
            };
        }

        public override void Save(ISiteRepository repository, Site entity)
        {
	        if(entity.IsModifiedRecursive())
	        {
		        entity.DateLastModified = DateTime.Now;
	        }
            base.Save(repository, entity);
        }

        public void UpdateSiteLastEditDate(int siteID)
        {
            Site site = Site.Load(siteID);
            site.DateLastModified = DateTime.Now;
            site.Save();
        }


        public override void CleanDataBeforeSave(ISiteRepository repository, Site entity)
        {
            CleanupSiteUrls(entity);

            base.CleanDataBeforeSave(repository, entity);
        }

        private static void CleanupSiteUrls(Site entity)
        {
            if (entity.SiteUrls != null)
            {
                foreach (var siteUrl in entity.SiteUrls)
                {
                    if (siteUrl.Url != null)
                    {
                        siteUrl.Url = siteUrl.Url.Trim();
	                    if(siteUrl.Url.Contains(" "))
	                    {
		                    siteUrl.Url = siteUrl.Url.Replace(" ", string.Empty);
	                    }
                        siteUrl.Url = siteUrl.Url.AppendForwardSlash();
                    }
                }
            }
        }
    }
}