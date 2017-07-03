
using System.Collections.Generic;
namespace NetSteps.Data.Entities.Repositories
{
	public partial interface ISiteUrlRepository
	{
		bool IsAvailable(string url);
	    bool IsAvailable(string url, int marketId);
		bool IsAvailable(int siteID, string url);
		bool Exists(string url);
        string Match(string url);
		List<SiteUrl> LoadBySiteID(int siteID);
	}
}
