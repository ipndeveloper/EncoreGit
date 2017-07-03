using System.Collections.Generic;
using NetSteps.Common.Interfaces;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business.Logic.Interfaces
{
	public partial interface ICampaignActionTokenValueBusinessLogic
	{
		CampaignActionTokenValue SaveTokenValue(ICampaignActionTokenValueRepository repository, Constants.Token token, int languageID, int campaignActionID, string value, int? accountID = null);
        Dictionary<string, string> GetMergedTokenValues(IEnumerable<CampaignActionTokenValue> fullyLoadedValues, int campaignActionID, int languageID, int accountID);
        ITokenValueProvider CreateDistributorTokenValueProvider(IEnumerable<CampaignActionTokenValue> fullyLoadedValues, int campaignActionID, int languageID, int? accountID = null);
        ITokenValueProvider CreateCompositeDistributorTokenValueProvider(IEnumerable<CampaignActionTokenValue> fullyLoadedValues, int campaignActionID, int languageID, int accountID);
    }
}
