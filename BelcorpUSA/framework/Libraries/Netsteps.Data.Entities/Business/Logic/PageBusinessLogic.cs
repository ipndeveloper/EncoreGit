using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business.Logic
{


    public partial class PageBusinessLogic
    {
        // Helper method to clone/create data for new objects(site, pages, etc..) from existing data - JHE
        public Page ClonePage(int pageIdToClone, int? existingSiteID = null, int? siteID = null)
        {
            try
            {
                var pageToClone = Page.LoadFull(pageIdToClone);
                return ClonePage(pageToClone, existingSiteID, siteID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        //PageBusinessLogicIntegrationTests.ChangePageLayout
        public void ChangePageLayout(Page page, Layout layout)
        {
            if (page.LayoutID == layout.LayoutID) return;

            page.LayoutID = layout.LayoutID;
            RemoveHtmlSectionsFromPageThatAreNotInLayout(page, layout);
            AddNewHtmlSectionsToPageFromNewLayout(page, layout);
        }

        private void AddNewHtmlSectionsToPageFromNewLayout(Page page, Layout layout)
        {
            foreach (HtmlSection section in layout.HtmlSections)
            {
                var newSection = new HtmlSection
                {
	                HtmlContentEditorTypeID = section.HtmlContentEditorTypeID,
	                HtmlSectionEditTypeID = section.HtmlSectionEditTypeID,
	                SectionName = section.SectionName,
	                RequiresApproval = section.RequiresApproval,
	                Width = section.Width,
	                Height = section.Height
                };

	            page.HtmlSections.Add(newSection);
            }
        }

        private void RemoveHtmlSectionsFromPageThatAreNotInLayout(Page page, Layout layout)
        {
            //RemoveHtmlSectionsFromPageThatAreNotInLayout
            foreach (var existingSection in page.HtmlSections.ToList())
            {
                if (!layout.HtmlSections.Any(s => s.SectionName == existingSection.SectionName))
                {
                    foreach (var choice in existingSection.HtmlSectionChoices.ToList())
                    {
                        choice.HtmlContent.HtmlElements.RemoveAllAndMarkAsDeleted();
                        choice.HtmlContent.MarkAsDeleted();
                        choice.MarkAsDeleted();
                    }
                    foreach (var content in existingSection.HtmlSectionContents.ToList())
                    {
                        content.HtmlContent.HtmlElements.RemoveAllAndMarkAsDeleted();
                        content.HtmlContent.MarkAsDeleted();
                        content.MarkAsDeleted();
                    }
                    existingSection.MarkAsDeleted();
                }
            }
        }

        public virtual void UpdateExternalUrlWithToken(Page page, Account account)
        {
            // method to be overriden when an external embedded page has added arguments added to its url - JWL            
        }

        public Page PageWithTranslations(IPageRepository repository, int pageID)
        {
            try
            {
                return repository.PageWithTranslations(pageID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public List<Page> PagesForSite(IPageRepository repository, int siteID)
        {
            try
            {
                return repository.PagesForSite(siteID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        // NOTE: The htmlSectionToClone variable passed in must be fully loaded. - JHE
        public Page ClonePage(Page pageToClone, int? existingSiteID = null, int? siteID = null)
        {
            try
            {
                var newPage = new Page
                {
	                ParentID = pageToClone.ParentID,
	                SiteID = pageToClone.SiteID,
	                Name = pageToClone.Name,
	                Url = pageToClone.Url,
	                ExternalUrl = pageToClone.ExternalUrl,
	                RequiresAuthentication = pageToClone.RequiresAuthentication,
	                Active = pageToClone.Active,
	                Editable = pageToClone.Editable,
	                UseSsl = pageToClone.UseSsl,
	                IsStartPage = pageToClone.IsStartPage,
	                ModifiedByUserID = pageToClone.ModifiedByUserID,
	                LayoutID = pageToClone.LayoutID,
	                PageTypeID = pageToClone.PageTypeID
                };

	            // Clone Translations
                foreach (var translation in pageToClone.Translations)
                {
                    var newPageTranslation = new PageTranslation
                    {
	                    LanguageID = translation.LanguageID,
	                    Title = translation.Title,
	                    Description = translation.Description,
	                    Keywords = translation.Keywords
                    };

	                newPage.Translations.Add(newPageTranslation);
                }

                // Clone Content
                foreach (var htmlSection in pageToClone.HtmlSections)
                {
                    var loadedHtmlSection = HtmlSection.LoadFull(htmlSection.HtmlSectionID);

                    HtmlSection newHtmlSection = HtmlSection.CloneHtmlSection(loadedHtmlSection, existingSiteID, siteID);
                    newPage.HtmlSections.Add(newHtmlSection);
                }

                return newPage;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

    }
}