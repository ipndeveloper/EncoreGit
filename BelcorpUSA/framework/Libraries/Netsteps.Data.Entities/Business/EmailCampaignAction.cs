using System.Collections.Generic;

namespace NetSteps.Data.Entities
{
    public partial class EmailCampaignAction
    {
        #region Properties

        #endregion

        #region Methods
        public static List<EmailCampaignAction> LoadAllDistributorVisible(Constants.CampaignType? campaignType = null, int? campaignID = null)
        {
            return BusinessLogic.LoadAllDistributorVisible(Repository, campaignType, campaignID);
        }

        public static List<EmailCampaignAction> LoadFullAllDistributorVisible(Constants.CampaignType? campaignType = null, int? campaignID = null)
        {
            return BusinessLogic.LoadFullAllDistributorVisible(Repository, campaignType, campaignID);
        }
        #endregion
    }
}
