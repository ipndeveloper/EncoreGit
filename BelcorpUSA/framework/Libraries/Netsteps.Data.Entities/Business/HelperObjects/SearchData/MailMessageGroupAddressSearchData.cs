using System;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;
using NetSteps.Common.Extensions;

namespace NetSteps.Data.Entities.Business
{
    [Serializable]
    public class MailMessageGroupAddressSearchData
    {
        [TermName("ID")]
        [Display(AutoGenerateField = false)]
        public int MailMessageGroupAddressID { get; set; }

        [Display(AutoGenerateField = false)]
        public int MailMessageGroupID { get; set; }

        [Display(AutoGenerateField = false)]
        public int MailMessageID { get; set; }

        [Display(AutoGenerateField = false)]
        public int? CampaignActionID { get; set; }

        [Display(AutoGenerateField = false)]
        public int? SenderMailAccountID { get; set; }

        [Display(AutoGenerateField = false)]
        public int? AccountID { get; set; }

        [TermName("EmailAddress", "Email Address")]
        public string EmailAddress { get; set; }

        [TermName("FirstName", "First Name")]
        public string FirstName { get; set; }

        [TermName("LastName", "Last Name")]
        public string LastName { get; set; }

        public DateTime? DateSentUTC { get; set; }
        [TermName("DateSent", "Date Sent")]
        public DateTime? DateSent
        {
            get
            {
                return DateSentUTC == null ? null : (DateTime?)DateSentUTC.Value.UTCToLocal();
            }
        }

        [TermName("TotalActions", "Total Actions")]
        public int TotalActions { get; set; }

        public DateTime? LastActionDateUTC { get; set; }
        [TermName("LastActionDateTime", "Last Action Date/Time")]
        public DateTime? LastActionDate
        {
            get
            {
                return LastActionDateUTC == null ? null : (DateTime?)LastActionDateUTC.Value.UTCToLocal();
            }
        }

        [TermName("Link", "Link")]
        public string LastClickUrl { get; set; }
    }
}
