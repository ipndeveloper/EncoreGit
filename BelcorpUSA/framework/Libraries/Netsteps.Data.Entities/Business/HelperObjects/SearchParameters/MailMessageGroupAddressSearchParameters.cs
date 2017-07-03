using NetSteps.Common.Base;
using NetSteps.Data.Entities.Mail;

namespace NetSteps.Data.Entities.Business
{
    public class MailMessageGroupAddressSearchParameters : FilterDateRangePaginatedListParameters<MailMessageGroupAddress>
    {
        public NetSteps.Data.Entities.Constants.MailMessageGroupAddressSearchType SearchType { get; set; }

        public int? MailMessageID { get; set; }

        public int? CampaignActionID { get; set; }

        public int? CampaignSubscriptionAddedByAccountID { get; set; }

        public int? SenderMailAccountID { get; set; }

        public string SenderEmailAddress { get; set; }

        public int? RecipientAccountID { get; set; }

        public string RecipientEmailAddress { get; set; }
    }
}
