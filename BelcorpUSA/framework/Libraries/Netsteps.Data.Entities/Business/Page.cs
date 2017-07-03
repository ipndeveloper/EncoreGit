using System;
using System.Collections.Generic;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;

namespace NetSteps.Data.Entities
{
    public partial class Page
    {
        #region Basic Crud
        #endregion

        #region Methods
        public string GetTitle(int languageID)
        {
            if (this.Translations != null)
            {
                var pageTranslation = this.Translations.GetByLanguageID(languageID);
                if (pageTranslation != null)
                    return pageTranslation.Title;
                else
                {
                    pageTranslation = this.Translations.GetByLanguageID(Constants.Language.English.ToInt());
                    if (pageTranslation != null)
                        return pageTranslation.Title;
                    else
                        return string.Empty;
                }
            }
            else
                return string.Empty;
        }

        public string GetDescription(int languageID)
        {
            if (this.Translations != null)
            {
                var pageTranslation = this.Translations.GetByLanguageID(languageID);
                if (pageTranslation != null)
                    return pageTranslation.Description;
                else
                {
                    pageTranslation = this.Translations.GetByLanguageID(Constants.Language.English.ToInt());
                    if (pageTranslation != null)
                        return pageTranslation.Description;
                    else
                        return string.Empty;
                }
            }
            else
                return string.Empty;
        }
        #endregion

        #region Internal Helper Methods
        internal static Page ClonePage(int pageIdToClone, int? existingSiteID, int? siteID = null)
        {
            try
            {
                return BusinessLogic.ClonePage(pageIdToClone, existingSiteID, siteID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static void ChangePageLayout(Page page, Layout layout)
        {
            try
            {
                BusinessLogic.ChangePageLayout(page, layout);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        // NOTE: The pageToClone variable passed in must be fully loaded. - JHE
        internal static Page ClonePage(Page pageToClone, int existingSiteID, int? siteID = null)
        {
            try
            {
                return BusinessLogic.ClonePage(pageToClone, existingSiteID, siteID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public void UpdateExternalUrlWithToken(Account account)
        {
            try
            {
                BusinessLogic.UpdateExternalUrlWithToken(this, account);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
        #endregion

        public static Page PageWithTranslations(int pageID)
        {
            try
            {
                return BusinessLogic.PageWithTranslations(Repository, pageID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static List<Page> PagesForSite(int siteID)
        {
            try
            {
                return BusinessLogic.PagesForSite(Repository, siteID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
    }
}
