using System;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;

namespace DistributorBackOffice.Areas.Communication.Models.MailStats
{
    public class IndexModel
    {
        public Constants.MailMessageGroupAddressSearchType SearchType { get; set; }
        public int? CampaignActionID { get; set; }
        public int? CampaignSubscriptionAddedByAccountID { get; set; }
        public DateTime? MailingDate { get; set; }
        public MailMessageGroupAddressSearchTotals Totals { get; set; }

        public RecipientListModel RecipientListModel { get; set; }
    }
}