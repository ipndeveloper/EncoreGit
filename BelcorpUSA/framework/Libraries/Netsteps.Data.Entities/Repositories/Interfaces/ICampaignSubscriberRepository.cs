using System.Collections.Generic;

namespace NetSteps.Data.Entities.Repositories
{
	public partial interface ICampaignSubscriberRepository
	{
		int? IsSubscriberAdded(int campaignID, int accountID);

		IEnumerable<CampaignSubscriber> LoadAllForCampaign(int campaignID);
	}
}
