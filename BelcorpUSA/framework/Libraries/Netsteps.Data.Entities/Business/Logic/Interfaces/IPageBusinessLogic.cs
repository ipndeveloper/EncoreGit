
using System.Collections.Generic;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business.Logic.Interfaces
{
    public partial interface IPageBusinessLogic
    {
        Page ClonePage(int pageIdToClone, int? existingSiteID = null, int? siteID = null);
        Page ClonePage(Page pageToClone, int? existingSiteID = null, int? siteID = null);
        void ChangePageLayout(Page pageLayoutToChange, Layout layout);
        void UpdateExternalUrlWithToken(Page page, Account account);
        Page PageWithTranslations(IPageRepository repository, int pageID);
        List<Page> PagesForSite(IPageRepository repository, int siteID);
    }
}