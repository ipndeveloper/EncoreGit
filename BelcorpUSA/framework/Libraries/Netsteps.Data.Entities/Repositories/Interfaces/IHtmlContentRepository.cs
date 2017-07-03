using System.Collections.Generic;

namespace NetSteps.Data.Entities.Repositories
{
    public partial interface IHtmlContentRepository
    {
        List<HtmlContentAccountStatus> GetContentAndDistributorNameByStatus(int statusID);
        //List<HtmlContent> LoadBatch(List<int> htmlContentIds);
    }
}
