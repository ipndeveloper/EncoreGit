using System.Collections.Generic;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business.Logic.Interfaces
{
    public partial interface ISiteBusinessLogic
    {
        Site CreateBaseSite(short siteTypeID, int marketId, string siteName, string displayName, int defaultLanguageID, IEnumerable<string> urls = null, bool saveNewSite = true);
        Site CopyBaseSite(int siteIDToCopyFrom, int marketId, string displayName, string description, int defaultLanguageID, IEnumerable<string> urls = null);
        void CloneContentFromExistingSiteForNewBaseSite(int newBaseSiteID, int existingBaseSiteID);
        void ClonePagesFromExistingSiteForNewBaseSite(int newBaseSiteID, int existingBaseSiteID);

        void UpdateSiteLastEditDate(int siteID);

        Site CreateChildSite(Site baseSite, Account account, int marketId, int? autoshipOrderId, string siteName = "", string displayName = "", IEnumerable<string> urls = null, bool saveNewSite = true);
        IEnumerable<Page> LoadPages(ISiteRepository siteRepository, int siteID);
        Site SiteWithLanguage(ISiteRepository siteRepository, int siteID);
        Site SiteWithNews(ISiteRepository siteRepository, int siteID);
        Site SiteWithSiteMap(ISiteRepository siteRepository, int siteID);
        Site SiteWithNewsAndArchive(ISiteRepository siteRepository, int siteID);
    }
}
