using System;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business
{
    [Serializable]
    public class MailMessageGroupAddressSearchTotals
    {
        [TermName("EmailsSent", "Emails Sent")]
        public int SentCount { get; set; }

        [TermName("EmailsBounced", "Emails Bounced")]
        public int FailedCount { get; set; }

        [TermName("EmailsOpened", "Emails Opened")]
        public int OpenedCount { get; set; }

        [TermName("UniqueClicks", "Unique Clicks")]
        public int ClickedCount { get; set; }
    }
}
