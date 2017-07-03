using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Extensions;
using NetSteps.Common.Interfaces;
using NetSteps.Common.TokenReplacement;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Data.Entities.TokenValueProviders;

namespace NetSteps.Data.Entities.Business.Logic
{
	public partial class CampaignActionTokenValueBusinessLogic
	{
		public virtual CampaignActionTokenValue SaveTokenValue(ICampaignActionTokenValueRepository repository, Constants.Token token, int languageID, int campaignActionID, string value, int? accountID = null)
		{
			try
			{
				CampaignActionTokenValue tokenValue = repository.LoadByUniqueKey(token, languageID, campaignActionID, accountID);
                if (tokenValue == null)
                {
                    //create new
                    tokenValue = new CampaignActionTokenValue()
                    {
                        TokenID = (int)token,
                        AccountID = accountID,
                        LanguageID = languageID,
                        CampaignActionID = campaignActionID,
                    };
                }
                else
                {
                    tokenValue.StartEntityTracking();
                }
				tokenValue.PlaceholderValue = value;
				tokenValue.Save();

				return tokenValue;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

        public virtual Dictionary<string, string> GetMergedTokenValues(IEnumerable<CampaignActionTokenValue> fullyLoadedValues, int campaignActionID, int languageID, int accountID)
        {
            var campaignActionLanguageValues = fullyLoadedValues.Where(x => x.CampaignActionID == campaignActionID && x.LanguageID == languageID);
            var accountValues = campaignActionLanguageValues.Where(x => x.AccountID == accountID);
            var defaultValues = campaignActionLanguageValues.Where(x => x.AccountID == null);

            // Note: Union() takes values from the first IEnumerable, then from the second.
            //       Make sure "accountValues" comes before "defaultValues" or the
            //       account values will be overwritten.
            var tokenValues =
                accountValues
                    .UnionBy(defaultValues, x => x.TokenID)
                    .ToDictionary(x => x.Token.Placeholder, x => x.PlaceholderValue);

            return tokenValues;
        }

        public virtual ITokenValueProvider CreateDistributorTokenValueProvider(IEnumerable<CampaignActionTokenValue> fullyLoadedValues, int campaignActionID, int languageID, int? accountID = null)
        {
            var values = fullyLoadedValues
                .Where(x => x.CampaignActionID == campaignActionID && x.LanguageID == languageID && x.AccountID == accountID)
                .ToDictionary(x => x.Token.Placeholder, x => x.PlaceholderValue);

            return new DistributorCampaignActionTokenValueProvider(values, accountID.HasValue ? accountID.Value.ToString() : "Default");
        }

        public virtual ITokenValueProvider CreateCompositeDistributorTokenValueProvider(IEnumerable<CampaignActionTokenValue> fullyLoadedValues, int campaignActionID, int languageID, int accountID)
        {
            return new CompositeTokenValueProvider(new[]
            {
                // Note: CompositeTVP takes values from the child TVPs in the order they were added.
                //       Make sure account values come before default values or the account values will be overwritten.
                CreateDistributorTokenValueProvider(fullyLoadedValues, campaignActionID, languageID, accountID),
                CreateDistributorTokenValueProvider(fullyLoadedValues, campaignActionID, languageID, null)
            });
        }
	}
}
