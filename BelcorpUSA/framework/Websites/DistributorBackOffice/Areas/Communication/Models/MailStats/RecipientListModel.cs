using NetSteps.Data.Entities;

namespace DistributorBackOffice.Areas.Communication.Models.MailStats
{
    public class RecipientListModel
    {
        public Constants.MailMessageGroupAddressSearchType SearchType { get; set; }
        public int? CampaignActionID { get; set; }
        public int? CampaignSubscriptionAddedByAccountID { get; set; }
    }
}