using System;
using System.Collections.Generic;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities
{
	public partial class CampaignAction
	{
		/// <summary>
		/// Loads all active and not completed for campaign.
		/// </summary>
		/// <param name="campaign">The campaign.</param>
		/// <returns></returns>
		public static IEnumerable<CampaignAction> LoadAllActiveAndNotCompletedForCampaign(Campaign campaign)
		{
			return CampaignAction.LoadAllActiveAndNotCompletedForCampaign(campaign.CampaignID);
		}

		/// <summary>
		/// Loads all active and not completed for campaign.
		/// </summary>
		/// <param name="campaignID">The campaign ID.</param>
		/// <returns></returns>
		public static IEnumerable<CampaignAction> LoadAllActiveAndNotCompletedForCampaign(int campaignID)
		{
			try
			{
				return Repository.LoadAllActiveAndNotCompletedForCampaign(campaignID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		/// <summary>
		/// Gets the pending subscribers.
		/// </summary>
		/// <param name="campaignActionID">The campaign action ID.</param>
		/// <returns></returns>
		public static IEnumerable<int> GetPendingSubscribers(int campaignActionID)
		{
			try
			{
				return Repository.GetPendingSubscribers(campaignActionID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		/// <summary>
		/// Searches the specified search parameters.
		/// </summary>
		/// <param name="searchParameters">The search parameters.</param>
		/// <returns></returns>
		public static PaginatedList<CampaignActionSearchData> Search(CampaignActionSearchParameters searchParameters)
		{
			try
			{
				return Repository.Search(searchParameters);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}
	}
}
