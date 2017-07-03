using System.Collections.Generic;

namespace NetSteps.Data.Entities.Repositories
{
    public partial interface IPageRepository
    {
        Page PageWithTranslations(int pageID);
        List<Page> PagesForSite(int siteID);
    }
}
