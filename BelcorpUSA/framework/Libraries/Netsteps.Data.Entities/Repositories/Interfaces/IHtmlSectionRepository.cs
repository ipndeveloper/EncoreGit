using System.Collections.Generic;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business;

namespace NetSteps.Data.Entities.Repositories
{
	public partial interface IHtmlSectionRepository
	{
		HtmlSection LoadFullByContentID(int htmlContentID, int siteID);
		HtmlSection LoadFullByTypeAndSectionName(short htmlSectionEditTypeID, string sectionName);
		HtmlSection LoadFullByHtmlSectionIDAndSiteID(int htmlSectionID, int siteID);
		void SelectChoice(int siteID, int htmlSectionID, int htmlContentID);
		PaginatedList<HtmlContentSearchData> SearchContent(HtmlContentSearchParameters searchParameters);
		Dictionary<int, int> GetChoiceUsage(int htmlSectionId, int productionContentId, int baseSiteId);
	}
}
