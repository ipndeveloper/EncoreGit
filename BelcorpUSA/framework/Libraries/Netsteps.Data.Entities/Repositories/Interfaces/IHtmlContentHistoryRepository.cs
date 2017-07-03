using System.Collections.Generic;

namespace NetSteps.Data.Entities.Repositories
{
	public partial interface IHtmlContentHistoryRepository
	{
		List<HtmlContentHistory> GetHistoryForContent(int contentId);
		List<HtmlContentHistory> GetFullHistoryForUser(int userId);
		List<HtmlContentHistory> GetUnseenHistoryForUser(int siteId, int userId);
		List<HtmlContentHistory> GetUnseenHistoryForSectionAndUser(int htmlSectionId, int userId);
        int GetUnseenHistoryCountForUser(int siteId, int userId, bool hasCMSContentApprovingFunction, int baseSiteId);
    }
}
