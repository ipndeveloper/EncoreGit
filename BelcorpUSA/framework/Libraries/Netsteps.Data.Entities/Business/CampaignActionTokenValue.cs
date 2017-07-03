using System;
using System.Collections.Generic;
using NetSteps.Common.Interfaces;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities
{
    public partial class CampaignActionTokenValue
    {
        public static List<CampaignActionTokenValue> LoadAllByCampaignActionID(int campaignActionID)
        {
            try
            {
                return Repository.LoadAllByCampaignActionID(campaignActionID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
		public static CampaignActionTokenValue LoadByUniqueKey(Constants.Token token, int languageID, int campaignActionID, int accountID)
        {
            try
            {
				return Repository.LoadByUniqueKey(token, languageID, campaignActionID, accountID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
        public static List<CampaignActionTokenValue> LoadAll(CampaignActionTokenValueSearchParameters searchParams)
        {
            try
            {
                return Repository.LoadAll(searchParams);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
        public static List<CampaignActionTokenValue> LoadAllFull(CampaignActionTokenValueSearchParameters searchParams)
        {
            try
            {
                return Repository.LoadAllFull(searchParams);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
		public static CampaignActionTokenValue SaveTokenValue(Constants.Token token, int languageID, int campaignActionID, string value, int? accountID = null)
		{
			try
			{
				return BusinessLogic.SaveTokenValue(Repository, token, languageID, campaignActionID, value, accountID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}
        public static Dictionary<string, string> GetMergedTokenValues(IEnumerable<CampaignActionTokenValue> fullyLoadedValues, int campaignActionID, int languageID, int accountID)
        {
            try
            {
                return BusinessLogic.GetMergedTokenValues(fullyLoadedValues, campaignActionID, languageID, accountID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
        public static ITokenValueProvider CreateDistributorTokenValueProvider(IEnumerable<CampaignActionTokenValue> fullyLoadedValues, int campaignActionID, int languageID, int? accountID = null)
        {
            try
            {
                return BusinessLogic.CreateDistributorTokenValueProvider(fullyLoadedValues, campaignActionID, languageID, accountID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
        public static ITokenValueProvider CreateCompositeDistributorTokenValueProvider(IEnumerable<CampaignActionTokenValue> fullyLoadedValues, int campaignActionID, int languageID, int accountID)
        {
            try
            {
                return BusinessLogic.CreateCompositeDistributorTokenValueProvider(fullyLoadedValues, campaignActionID, languageID, accountID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
    }
}
