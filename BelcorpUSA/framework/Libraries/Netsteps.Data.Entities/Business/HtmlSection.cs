using System;
using System.Collections.Generic;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities
{
    public partial class HtmlSection
    {
        #region Methods
        public static HtmlSection LoadFullByContentID(int htmlContentID, int siteID)
        {
            try
            {
                var htmlSection = Repository.LoadFullByContentID(htmlContentID, siteID);
                if (htmlSection != null)
                {
                    htmlSection.StartTracking();
                    htmlSection.IsLazyLoadingEnabled = true;
                }
                return htmlSection;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static HtmlSection LoadFullByTypeAndSectionName(short htmlSectionEditTypeID, string sectionName)
        {
            try
            {
                var htmlSection = Repository.LoadFullByTypeAndSectionName(htmlSectionEditTypeID, sectionName);
                if (htmlSection != null)
                {
                    htmlSection.StartTracking();
                    htmlSection.IsLazyLoadingEnabled = true;
                }
                return htmlSection;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static HtmlSection LoadFullByHtmlSectionIDAndSiteID(int htmlSectionID, int siteID)
        {
            try
            {
                var htmlSection = Repository.LoadFullByHtmlSectionIDAndSiteID(htmlSectionID, siteID);
                if (htmlSection != null)
                {
                    htmlSection.StartTracking();
                    htmlSection.IsLazyLoadingEnabled = true;
                }
                return htmlSection;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static void SelectChoice(int siteID, int htmlSectionID, int htmlContentID)
        {
            try
            {
                Repository.SelectChoice(siteID, htmlSectionID, htmlContentID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }


        public static PaginatedList<HtmlContentSearchData> SearchContent(HtmlContentSearchParameters orderSearchParameters)
        {
            try
            {
                return Repository.SearchContent(orderSearchParameters);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static Dictionary<int, int> GetChoiceUsage(int htmlSectionId, int productionContentId, int baseSiteId)
        {
            try
            {
                return Repository.GetChoiceUsage(htmlSectionId, productionContentId, baseSiteId);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
        #endregion

        #region Internal Helper Methods
        internal static HtmlSection CloneHtmlSection(int htmlSectionIdToClone, int? siteID = null)
        {
            try
            {
                return BusinessLogic.CloneHtmlSection(htmlSectionIdToClone, siteID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
        // NOTE: The htmlSectionToClone variable passed in must be fully loaded. - JHE
        internal static HtmlSection CloneHtmlSection(HtmlSection htmlSectionToClone, int? existingSiteID = null, int? newSiteID = null)
        {
            try
            {
                return BusinessLogic.CloneHtmlSection(htmlSectionToClone, existingSiteID, newSiteID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
        #endregion

        /// <summary>
        /// Created to replace the ContentByStatus methods. Those methods pull from the SitesWithContentCache, because the items are all related when an individual
        /// item is saved the entire collection of content is checked for changes. This will add 10 or more seconds to a save of one individual content. To get around that
        /// we use this method to pull directly from EF/Database and thus when save is called this idividual item is the only one checked for changes.
        /// </summary>
        /// <param name="htmlSection">The HtmlSection the content is related to.</param>
        /// <param name="site">The site of the content.</param>
        /// <param name="status">The status of the content.</param>
        /// <param name="languageId">The language id of the content.</param>
        /// <returns></returns>
        public HtmlContent GetContentUncached(Site site, Constants.HtmlContentStatus status, int languageId)
        {
            var htmlContentRepository = new HtmlContentRepository();
            return htmlContentRepository.GetHtmlContent(this, site, status, languageId);
        }

        public static HtmlSection GetById(int htmlSectionId)
        {
            HtmlSectionRepository htmlSectionRepository = new HtmlSectionRepository();
            return htmlSectionRepository.GetById(htmlSectionId);
        }
    }
}
