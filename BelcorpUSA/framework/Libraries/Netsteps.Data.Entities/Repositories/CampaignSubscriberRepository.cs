using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class CampaignSubscriberRepository
    {
        /// <summary>
        /// Find out if a subscriber was added to a campaign, and by whom if they are
        /// </summary>
        /// <param name="campaignID"></param>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public int? IsSubscriberAdded(int campaignID, int accountID)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var subscriber = context.CampaignSubscribers.FirstOrDefault(cs => cs.CampaignID == campaignID && cs.AccountID == accountID);

					return subscriber == default(CampaignSubscriber) ? (int?)null : subscriber.AddedByAccountID;
				}
			});
		}

		/// <summary>
		/// Loads all for campaign.
		/// </summary>
		/// <param name="campaignID">The campaign ID.</param>
		/// <returns></returns>
		public IEnumerable<CampaignSubscriber> LoadAllForCampaign(int campaignID)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					var subscribers = context.CampaignSubscribers.Where(cs => cs.CampaignID == campaignID);
					return subscribers.ToList();
				}
			});
		}
	}
}
