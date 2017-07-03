using System.Collections.Generic;
using NetSteps.Data.Entities.Business;

namespace NetSteps.Data.Entities.Repositories
{
    public partial interface ICampaignActionTokenValueRepository
    {
        List<CampaignActionTokenValue> LoadAllByCampaignActionID(int campaignActionID);
		CampaignActionTokenValue LoadByUniqueKey(Constants.Token token, int languageID, int campaignActionID, int? accountID = null);
        List<CampaignActionTokenValue> LoadAll(CampaignActionTokenValueSearchParameters searchParams);
        List<CampaignActionTokenValue> LoadAllFull(CampaignActionTokenValueSearchParameters searchParams);
    }
}
