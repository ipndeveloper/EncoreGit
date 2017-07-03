using System;
using System.Collections.Generic;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities
{
	public partial class CampaignSubscriber
	{
		public static int? IsSubscriberAdded(int campaignID, int accountID)
		{
			try
			{
				return Repository.IsSubscriberAdded(campaignID, accountID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static IEnumerable<CampaignSubscriber> LoadAllForCampaign(Campaign campaign)
		{
			return CampaignSubscriber.LoadAllForCampaign(campaign.CampaignID);
		}

		public static IEnumerable<CampaignSubscriber> LoadAllForCampaign(int campaignID)
		{
			try
			{
				return Repository.LoadAllForCampaign(campaignID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		//public static List<int> GetListOfPendingSubscribers(int campaignID)
		//{
		//    try
		//    {
		//        return Repository.GetListOfPendingSubscribers(campaignID);
		//    }
		//    catch (Exception ex)
		//    {
		//        throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
		//    }
		//}
	}
}
