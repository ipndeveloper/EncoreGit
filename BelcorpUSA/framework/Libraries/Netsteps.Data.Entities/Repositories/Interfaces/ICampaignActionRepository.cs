using System.Collections.Generic;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business;

namespace NetSteps.Data.Entities.Repositories
{
	public partial interface ICampaignActionRepository
	{
		/// <summary>
		/// Gets the list of pending subscribers.
		/// </summary>
		/// <param name="campaignActionID">The campaign action ID.</param>
		/// <returns></returns>
		IEnumerable<int> GetPendingSubscribers(int campaignActionID);

		/// <summary>
		/// Searches the specified search parameters.
		/// </summary>
		/// <param name="searchParameters">The search parameters.</param>
		/// <returns></returns>
		PaginatedList<CampaignActionSearchData> Search(CampaignActionSearchParameters searchParameters);

		/// <summary>
		/// Loads all active and not completed for campaign.
		/// </summary>
		/// <param name="campaignID">The campaign ID.</param>
		/// <returns></returns>
		IEnumerable<CampaignAction> LoadAllActiveAndNotCompletedForCampaign(int campaignID);
	}
}
